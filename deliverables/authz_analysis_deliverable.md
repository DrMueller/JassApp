# Authorization Analysis Report

## 1. Executive Summary

- **Analysis Status:** Complete
- **Key Outcome:** Ten high-confidence authorization vulnerabilities were identified across horizontal (IDOR) and context/workflow categories. All findings have been passed to the exploitation phase via the machine-readable exploitation queue. No vertical escalation candidates exist — the application has a single flat privilege level with no admin role. The application's authorization model is critically broken: the only guard in the entire codebase is `auth:user` (Azure AD authentication), and **no ownership check, role check, or tenant isolation guard exists anywhere**.
- **Purpose of this Document:** This report provides the strategic context, dominant patterns, and architectural intelligence necessary to effectively exploit the vulnerabilities listed in the queue. It is intended to be read alongside the JSON deliverable.

---

## 2. Dominant Vulnerability Patterns

### Pattern 1: Complete Absence of Ownership Validation (Horizontal IDOR)

- **Description:** Every endpoint that accepts a resource ID (player ID, game ID) performs a direct database lookup by that integer ID with zero verification that the requesting authenticated user created or is authorized to access that resource. The database schema has **no `UserId`, `OwnerId`, or `TenantId` column in any table**.
- **Implication:** Any authenticated Azure AD user can read, modify, and delete any other user's players and game rounds by enumerating or guessing integer IDs.
- **Root Cause:** `SpielerSpec` and `CoiffeurSpielrundeSpec` only filter by entity ID — no user/owner predicate is ever applied. Repositories (`SpielerRepository.DeleteAsync`, `CoiffeurSpielrundeRepository.DeleteAsync`) call through directly to EF Core with no ownership gate.
- **Representative:** AUTHZ-VULN-01, AUTHZ-VULN-02, AUTHZ-VULN-03, AUTHZ-VULN-04, AUTHZ-VULN-10

### Pattern 2: Global Data Listing Without User Scoping (Horizontal — Data Enumeration)

- **Description:** All listing endpoints query the entire database without any user filter. `new SpielerSpec()` (no-arg constructor), `new CoiffeurSpielrundeSpec()` (no-arg constructor), and `new SpielerHistorySpec()` each return ALL records from ALL users unconditionally.
- **Implication:** A single authenticated user can enumerate all player names, all game rounds, all team compositions, and all game history for every other user in the system.
- **Representative:** AUTHZ-VULN-05, AUTHZ-VULN-06, AUTHZ-VULN-07, AUTHZ-VULN-08

### Pattern 3: Route Parameter Controls Authorization Mode Without Guard (Context/Workflow)

- **Description:** The `spectatorMode` boolean route parameter in `coiffeur/game/{gameId:int}/{spectatorMode:bool}` controls whether the page renders in read-only (true) or edit (false) mode. There is no server-side authorization check separating these two modes — the sole difference is a UI rendering conditional at `RunningGamePage.razor.cs:19-21`.
- **Implication:** Any user holding a "spectate" link (`/true`) can convert it to an "edit" link (`/false`) by modifying the URL, gaining full write access to any game.
- **Representative:** AUTHZ-VULN-09

---

## 3. Strategic Intelligence for Exploitation

### Session Management Architecture

- **Session mechanism:** Azure AD OIDC flow produces an ASP.NET Core authentication cookie containing a serialized `ClaimsPrincipal`. No JWT access token is obtained (scope is `openid profile` only).
- **Circuit model:** All application interactivity flows through Blazor Server's SignalR WebSocket (`/_blazor`). The cookie is validated once at WebSocket upgrade; thereafter, the server-side circuit handles all state.
- **What the application trusts:** The `ClaimsPrincipal` in the cookie proves identity (who you are) but is **never checked against resource ownership** (what you own). The application's only authorization question is "are you authenticated?" — it never asks "do you own this?"
- **Critical Finding:** `ServiceInitialization.cs:43` — `options.FallbackPolicy = options.DefaultPolicy` — the single authorization policy applied globally. `DefaultPolicy` requires only a valid authenticated principal. This is the entire authorization system.

### Role/Permission Model

- **Roles identified:** Two levels — `anon` (unauthenticated) and `user` (any Azure AD tenant member). No admin, moderator, or owner role exists anywhere.
- **No RBAC:** `services.AddAuthorization()` is called only to set `FallbackPolicy`. No `RequireRole()`, `RequireClaim()`, `AddPolicy()`, or resource-based authorization handler exists.
- **Critical Finding:** All authenticated users in Azure AD tenant `d6fddda6-f690-4755-92c2-f22a3521bab0` have identical, unrestricted access to every piece of data in the application.

### Resource Access Patterns

- **Route parameters:** Player IDs and game IDs are integer primary keys passed directly as Blazor route parameters (e.g., `/players/edit/{playerId:int}`, `coiffeur/game/{gameId:int}/{spectatorMode:bool}`). These are the direct IDOR handles.
- **ID sequence:** IDs are auto-increment integers starting from 1. Enumeration via sequential scan (1, 2, 3, …) will discover all records.
- **No secondary checks:** There is no "load resource then check ownership" pattern anywhere. The resource is loaded and returned/mutated unconditionally.
- **Delete path:** Delete operations in `SpielerOverviewList.razor.cs` and `CoiffeurOverviewList.razor.cs` accept an integer ID from the UI data grid and call the repository `DeleteAsync` directly with no intermediate ownership validation.

### Database Schema (No Ownership Columns)

- Tables: `SpielerTable`, `CoiffeurSpielrundeTable`, `JassTeamTable`, `JassTeamSpielerTable`, `TrumpfrundeTable`
- **Critical Finding:** None of these tables have a `UserId`, `CreatedBy`, `OwnerId`, or `TenantId` column. There is no mechanism at the database level to scope records to a user even if the application wanted to add ownership checks — it would require a schema migration.

### Workflow Implementation

- **spectatorMode:** A boolean route parameter. `RunningGamePage.razor.cs:19-21` sets a `IsSpectatorMode` flag based on this parameter. The flag only affects UI rendering (disabling input components). No server-side action validates or enforces this flag before accepting mutations via SignalR events.
- **Game creation flow:** Games are created via `coiffeur/configuration`, which saves a new `CoiffeurSpielrundeTable` row and navigates to `coiffeur/game/{newId}/false`. Direct navigation to `coiffeur/game/{anyId}/false` skips configuration entirely and loads any existing game in edit mode.
- **Player create vs. edit:** `PlayerId == 0` in `SpielerEditPage.razor.cs:36` branches to create-mode; `PlayerId > 0` loads and edits. This is a standard create/edit pattern but the zero-ID path creates a new player owned by no one — no creator identity is recorded.

---

## 4. Vectors Analyzed and Confirmed Secure

These authorization checks were traced and confirmed to have robust, properly-placed guards or represent intended behavior with no exploitable authorization bypass.

| **Endpoint** | **Guard Location** | **Defense Mechanism** | **Verdict** |
|---|---|---|---|
| `POST /signin-oidc` | Microsoft.Identity.Web middleware | Nonce cookie + state correlation cookie validate OIDC callback integrity; framework-provided, not application code | SAFE |
| `GET /MicrosoftIdentity/Account/SignIn` | N/A (anon) | Public endpoint by design; only initiates redirect to Azure AD | SAFE (by design) |
| `GET /MicrosoftIdentity/Account/SignOut` | ASP.NET Core cookie auth | Requires valid auth cookie to sign out; no resource IDs involved | SAFE |
| All static assets (`/M.png`, `/app.css`, etc.) | None (by design) | Static files are public and contain no sensitive data | SAFE (by design) |
| `GET /` (HomePage) | `FallbackPolicy` | No resource IDs; redirects to login if unauthenticated | SAFE (no IDOR surface) |
| Route type constraints `{playerId:int}`, `{gameId:int}`, `{spectatorMode:bool}` | Blazor routing layer | Rejects non-integer/non-boolean values before component initialization — prevents type confusion attacks | SAFE (type safety only; does not prevent IDOR) |
| SQL query parameterization | EF Core LINQ | All database queries use parameterized queries; no raw SQL string concatenation; no SQL injection surface | SAFE |

---

## 5. Analysis Constraints and Blind Spots

- **Blazor Server SignalR circuit internals:** All application mutations occur via SignalR events through `/_blazor`. Static analysis confirms the component code paths; however, any server-side Blazor event callbacks not surfaced in the `.razor.cs` files (e.g., inline `@onclick` lambdas in `.razor` files) were not exhaustively analyzed. Given the pattern of zero ownership checks everywhere else, these are expected to be equally unguarded.
- **Azure AD tenant boundary:** All vulnerabilities require a valid Azure AD account in tenant `d6fddda6-f690-4755-92c2-f22a3521bab0`. If tenant registration is restricted to a specific organization, the practical exploitability is limited to insiders or users who can register in that tenant. If tenant allows open registration (Microsoft personal accounts or open B2B), any internet user can authenticate.
- **No runtime/dynamic authorization:** There is no dynamic permission loading from database, no claims transformation pipeline, and no resource-based authorization handler. The static analysis fully covers the authorization model.
- **Application Insights telemetry:** The connection string is a placeholder (`_`) in `appsettings.json` — Application Insights may not be active, so exploitation may not be logged server-side.
