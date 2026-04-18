# Reconnaissance Deliverable: JassApp Attack Surface Map

## 0) HOW TO READ THIS

This reconnaissance report provides a comprehensive map of the JassApp application's attack surface, with special emphasis on authorization and privilege escalation opportunities for the Authorization Analysis Specialist.

**Key Sections for Authorization Analysis:**
- **Section 4 (API Endpoint Inventory):** Contains authorization details for each endpoint — focus on "Required Role" and "Object ID Parameters" columns to identify IDOR candidates. Every parameterized Blazor route is an IDOR candidate because there are zero ownership checks.
- **Section 6.4 (Guards Directory):** Catalog of authorization controls — the only guard is `auth:user` (Azure AD authenticated). There is no `auth:admin`, no `ownership:*`, and no `role:*` guard anywhere in the application.
- **Section 7 (Role & Privilege Architecture):** Single flat privilege level — all authenticated Azure AD users are equal. No admin vs. user distinction exists.
- **Section 8 (Authorization Vulnerability Candidates):** All object-parameterized routes are High-priority IDOR candidates due to the complete absence of ownership checks.

**How to Use the Network Mapping (Section 6):** The entity/flow mapping shows a minimal attack surface: all traffic enters via HTTPS to Kestrel, routes through the Blazor Server SignalR circuit (`/_blazor`), and queries a SQL Server database. The only meaningful trust boundary is between the unauthenticated browser and the authenticated Blazor circuit.

**Priority Order for Testing:** Start with Section 8.1's IDOR candidates (gameId and playerId route parameters), then the full-table listing endpoints (coiffeur/overview, coiffeur/game/playerhistory), and finally the delete operations reachable via those pages.

---

## 1. Executive Summary

JassApp is a .NET 9.0 Blazor Server application for tracking scores of the Swiss card game "Jass" (specifically the Coiffeur variant). It is a single-tenant ASP.NET Core Kestrel/IIS application hosted on Azure App Service, authenticating exclusively via Azure Active Directory (Microsoft Entra ID) OpenID Connect. The application has no REST API — all interactivity occurs via Blazor Server's SignalR circuit (`/_blazor`). There are 11 routable Blazor pages accessible after Azure AD authentication.

**Primary attack surface findings:**
1. **Complete IDOR vulnerability:** No user/tenant ownership model exists. Any authenticated Azure AD user can read, modify, and delete any other user's players and games by manipulating integer route parameters.
2. **No data isolation:** All EF Core specifications return unfiltered global data regardless of authenticated user.
3. **Stack trace disclosure:** `ErrorInfo.razor` renders full exception stack traces (type name, message, stack) in the browser UI for all authenticated users.
4. **Sensitive data logging:** `EnableSensitiveDataLogging()` is active in all environments, writing SQL parameter values to stdout.
5. **`DetailedErrors: true`:** Configured globally in `appsettings.json`, exposing Blazor circuit error details via SignalR to all clients.
6. **Missing security headers:** No CSP, X-Frame-Options, X-Content-Type-Options, Referrer-Policy, or Permissions-Policy headers.
7. **Dev/test pages in production:** `/test/exception`, `/test/informations`, and `/dev/audio-player` are live and authentication-protected but not disabled.

---

## 2. Technology & Service Map

- **Frontend:** Blazor Server (ASP.NET Core 9.0), MudBlazor 8.10.0 (Material Design UI), Toolbelt.Blazor.SpeechSynthesis 11.0.0 (browser Web Speech API via JS interop), Bootstrap (static assets)
- **Backend:** C# / .NET 9.0, ASP.NET Core 9.0 Blazor Server (Kestrel HTTP server), Lamar 15.0.1 IoC container, Entity Framework Core 9.0.7 (SQL Server provider), Microsoft.Identity.Web 3.12.0
- **Infrastructure:** Azure App Service (Windows), Azure SQL Database (SQL Server), Azure Active Directory / Microsoft Entra ID (OIDC authentication provider), Azure Application Insights 2.23.0 (telemetry — currently placeholder connection string)
- **Identified Subdomains:** None discovered via subfinder. Application is served at `host.docker.internal:7202` (HTTPS/TLS 1.3). Certificate is self-signed: `CN=localhost`.
- **Open Ports & Services:**
  - Port 7202: HTTPS/2 — ASP.NET Core 9.0 Blazor Server (Kestrel)
  - TLS: TLS 1.3, cipher `TLS_AES_256_GCM_SHA384`, key exchange `x25519`, RSA-2048 self-signed cert (CN=localhost, issued/expires 2026-01-14 to 2027-01-14)

---

## 3. Authentication & Session Management Flow

- **Entry Points:**
  - `GET /` — Unauthenticated requests redirect to Azure AD OIDC login (HTTP 302)
  - `GET /MicrosoftIdentity/Account/SignIn` — Explicit sign-in initiation (HTTP 302 to Azure AD)
  - `POST /signin-oidc` — OIDC callback endpoint (receives `id_token` via form_post from Azure AD)
  - `GET /MicrosoftIdentity/Account/SignOut` — Sign-out (requires auth)
  - `GET /MicrosoftIdentity/Account/AccessDenied` — Access denied display

- **Mechanism:**
  1. User navigates to any application URL; unauthenticated users receive HTTP 302 redirect to `https://login.microsoftonline.com/d6fddda6-f690-4755-92c2-f22a3521bab0/oauth2/v2.0/authorize` with `response_type=id_token`, `scope=openid profile`, `response_mode=form_post`.
  2. Two state cookies are set on the redirect: `.AspNetCore.OpenIdConnect.Nonce.*` (nonce validation) and `.AspNetCore.Correlation.*` (state correlation) — both `path=/signin-oidc; secure; samesite=none; httponly`.
  3. User authenticates with Azure AD. Azure AD POST-backs to `/signin-oidc` with the `id_token`.
  4. `Microsoft.Identity.Web` middleware validates the `id_token`, creates the ASP.NET Core authentication cookie, and redirects user to original URL.
  5. The auth cookie persists the session; subsequent Blazor Server requests include this cookie.
  6. All Blazor interactivity thereafter occurs via the SignalR WebSocket connection at `/_blazor`, authenticated via the cookie.

- **Code Pointers:**
  - Authentication registration: `Sources/Application/Presentation/Shell/Initialization/ServiceInitialization.cs` lines 40-41
  - AzureAd config: `Sources/Application/appsettings.json` lines 2-9
  - OIDC callback path: `/signin-oidc` (default, configured at `appsettings.json:8`)
  - Sign-out link: `Sources/Application/Presentation/Shell/Benutzer/BenutzerMenu.razor` line 5

### 3.1 Role Assignment Process

- **Role Determination:** No application-level roles exist. The only "role" is "authenticated Azure AD user." Roles are NOT sourced from the Azure AD token, database, or any claims mapping.
- **Default Role:** All Azure AD users in the configured tenant (TenantId: `d6fddda6-f690-4755-92c2-f22a3521bab0`) who authenticate become "authenticated users" with identical privileges.
- **Role Upgrade Path:** Not applicable — there is only one privilege level.
- **Code Implementation:** `Sources/Application/Presentation/Shell/Initialization/ServiceInitialization.cs` line 43: `services.AddAuthorization(options => { options.FallbackPolicy = options.DefaultPolicy; })` — `DefaultPolicy` requires an authenticated user, no role claims.

### 3.2 Privilege Storage & Validation

- **Storage Location:** User identity is stored in the ASP.NET Core authentication cookie (set by the OIDC middleware). The cookie contains a serialized `ClaimsPrincipal`. No JWT access token is obtained — only an `id_token` (scope is `openid profile`).
- **Validation Points:** The fallback authorization policy (`FallbackPolicy = DefaultPolicy`) is the sole authorization gate. It validates the presence of an authenticated `ClaimsPrincipal` on every request. No role checks, no policy checks, and no resource-based authorization exist.
- **Cache/Session Persistence:** Session lives in the ASP.NET Core cookie. Blazor Server circuit state persists server-side for the duration of the WebSocket connection. No server-side session store is configured.
- **Code Pointers:**
  - `Sources/Application/Presentation/Shell/Initialization/ServiceInitialization.cs` line 43 (FallbackPolicy)
  - `Sources/Application/Presentation/Shell/Benutzer/BenutzerMenu.razor.cs` lines 26-27 (user email read from `AuthenticationStateProvider`)
  - `Sources/Application/Presentation/Infrastructure/Logging/Services/LogInfoProvider.cs` lines 15, 27 (user identity accessed for telemetry)

### 3.3 Role Switching & Impersonation

- **Impersonation Features:** None. No admin impersonation, no sudo mode, no role switching.
- **Role Switching:** Not applicable.
- **Audit Trail:** Azure AD handles login audit logging on its platform. The application has no security event logging.
- **Code Implementation:** N/A

---

## 4. API Endpoint Inventory

**Network Surface Focus:** JassApp has NO REST API endpoints. All network-accessible functionality is provided by Blazor Server routable pages (served via HTTP then activated via SignalR `/_blazor`) and Microsoft Identity UI controller routes.

| Method | Endpoint Path | Required Role | Object ID Parameters | Authorization Mechanism | Description & Code Pointer |
|---|---|---|---|---|---|
| GET | `/` | auth:user | None | FallbackPolicy (Azure AD OIDC redirect) | Home/landing page. `Presentation/Areas/Home/HomePage.razor`, `HomePage.razor.cs:5` |
| GET | `/players/overview` | auth:user | None | FallbackPolicy (Azure AD OIDC redirect) | Lists ALL players from all users (no ownership filter). `Presentation/Areas/Spieler/SpielerOverviewPage.razor`, `SpielerOverviewPage.razor.cs:9` |
| GET | `/players/edit/{playerId:int}` | auth:user | `playerId` (int, route) | FallbackPolicy only — **NO ownership check** | Edit/view any player by integer ID. `playerId=0` creates new player. `Presentation/Areas/Spieler/SpielerEditPage.razor.cs:15,22-23` |
| GET | `coiffeur/overview` | auth:user | None | FallbackPolicy only | Lists ALL game rounds from all users. `Presentation/Areas/Coiffeur/Overview/CoiffeurOverviewPage.razor`, `CoiffeurOverviewList.razor.cs` |
| GET | `coiffeur/configuration` | auth:user | None | FallbackPolicy only | Create new game; player list loaded from ALL players. `Presentation/Areas/Coiffeur/Configuration/CoiffeurConfigurationPage.razor.cs:17` |
| GET | `coiffeur/game/{gameId:int}/{spectatorMode:bool}` | auth:user | `gameId` (int, route), `spectatorMode` (bool, route) | FallbackPolicy only — **NO ownership check** | View/edit any game round by integer ID. `spectatorMode=false` enables editing. `Presentation/Areas/Coiffeur/RunningGame/RunningGamePage.razor.cs:10,13-14,19-21` |
| GET | `coiffeur/game/playerhistory` | auth:user | None | FallbackPolicy only | Shows global game history across ALL users. `Presentation/Areas/Coiffeur/SpielerHistory/SpielerHistoryPage.razor.cs:9` |
| GET | `/test/exception` | auth:user | None | FallbackPolicy only | **Dev/test page** — intentionally throws exception. `Presentation/Areas/Test/Components/TestExceptionPage.razor.cs:8` |
| GET | `/test/informations` | auth:user | None | FallbackPolicy only | **Dev/test page** — test information display. `Presentation/Areas/Test/Components/TestInformationsPage.razor.cs:7` |
| GET | `/dev/audio-player` | auth:user | None | FallbackPolicy only | **Dev utility** — audio player demo. `Presentation/Areas/Dev/AudioPlayerDemoPage.razor` |
| GET | `/notfound` | auth:user | None | FallbackPolicy (unauthenticated 404s redirect to login) | Custom 404 page. `Presentation/Shell/Errors/NotFound/NotFoundPage.razor.cs:9` |
| GET | `MicrosoftIdentity/Account/SignIn` | anon | None | None | Initiates Azure AD OIDC login flow (302 redirect to login.microsoftonline.com). Framework-provided by `AddMicrosoftIdentityUI()` |
| GET | `MicrosoftIdentity/Account/SignOut` | auth:user | None | Cookie auth required | Signs out user (clears auth cookie + Azure AD session). Framework-provided |
| GET | `MicrosoftIdentity/Account/Challenge` | anon | None | None | Forces re-authentication challenge. Framework-provided |
| GET | `MicrosoftIdentity/Account/AccessDenied` | anon | None | None | Displays access denied page; redirects to login if unauthenticated. Framework-provided |
| POST | `/signin-oidc` | anon | None | Framework OIDC handler (nonce + state validation) | OIDC callback — receives `id_token` via form_post from Azure AD. Processes nonce/state cookies. Framework-provided by `Microsoft.Identity.Web` |
| WS | `/_blazor` | auth:user | None | FallbackPolicy (cookie validated before upgrade) | Blazor Server SignalR hub — all application interactivity flows through this endpoint. All Blazor page interactions, form submissions, and data operations go through this channel. `Presentation/Shell/Initialization/ServiceInitialization.cs:20-21` |
| GET | Static assets (`/M.png`, `/app.css`, `/audio/*.mp3`, `/js/localstorage.js`, `/lib/bootstrap/**`, `/_framework/blazor.web.js`, `/_content/MudBlazor/**`) | anon | None | None | Static files served via `MapStaticAssets()`. No authentication required. `AppInitialization.cs:20` |

---

## 5. Potential Input Vectors for Vulnerability Analysis

**Network Surface Focus:** All inputs flow through the Blazor Server SignalR circuit (`/_blazor`) or HTTP route parameters. No REST API endpoints accept JSON bodies.

### URL / Route Parameters

- **`playerId` (int):** Route parameter in `GET /players/edit/{playerId:int}`. Blazor type-constrains to `int`. Used directly as database PK lookup in `SpielerSpec(new SpielerId(PlayerId))` with no ownership check. File: `Presentation/Areas/Spieler/SpielerEditPage.razor.cs` line 47.
- **`gameId` (int):** Route parameter in `GET coiffeur/game/{gameId:int}/{spectatorMode:bool}`. Used directly as database PK lookup in `CoiffeurSpielrundeSpec(new CoiffeurSpielrundeId(GameId))` with no ownership check. File: `Presentation/Areas/Coiffeur/RunningGame/RunningGamePage.razor.cs` line 29.
- **`spectatorMode` (bool):** Route parameter in same route. Controls view-only vs. edit mode — no separate authorization between modes. File: `Presentation/Areas/Coiffeur/RunningGame/RunningGamePage.razor.cs` line 19-21.

### POST Body / Blazor SignalR Event Parameters (via `/_blazor`)

All form data is submitted via Blazor Server SignalR events, not HTTP POST bodies.

- **Player Name (`SpielerEditViewModel.Name`, `string`):** Free-text string bound to `MudTextField` in `SpielerEditPage.razor`. Only `[Required]` validation at ViewModel level — no `[MaxLength]`, no content validation. Flows to `nvarchar(max)` column. File: `Presentation/Areas/Spieler/SpielerEditPage.razor` line 9; `SpielerEditViewModel.cs` line 9-10; `SpielerEditPage.razor.cs` lines 65, 69; `DataAccess/Repositories/SpielerRepository.cs` line 24.
- **Game Point Value (`Punktwert`, `int`):** Numeric field in `CoiffeurConfigurationPage.razor` line 12 — `MudNumericField` with `Min="0"` only, no `Max`. Flows into `CoiffeurSpielrundeTable.Punktewert (int)`.
- **Game Flags (`IncludeRaucherpausen`, `IncludeShots`, `bool`):** Checkboxes in `CoiffeurConfigurationPage.razor` lines 13-14. Boolean values, no injection risk.
- **Trump Round Scores (`TrumpfrundeResultat.Punkte`, `int`):** Numeric field in `TrumpfTeamRunde.razor` lines 4-9 — `MudNumericField` with `Min="0"` and `Max="16"`. Flows to `TrumpfrundeTable.ResultatTeam1/2 (int nullable)`.
- **Match Flags (`IstMatch`, `IstKontermatsch`, `bool`):** Checkboxes in `TrumpfTeamRunde.razor` lines 11-14. Boolean values.
- **Speech Voice Selection (`VoiceIdentity`, `string` from browser):** Voice identifier string from browser's Web Speech API, persisted to `localStorage` via `LocalStorageProxy.SetItemAsync("voice", value)`. File: `Presentation/Shared/Voices/Components/VoiceSelect.razor` line 11-19; `Presentation/Shared/Storage/Implementation/LocalStorageProxy.cs` line 38.
- **Delete Player ID (via `SpielerOverviewList.DeleteSpielerAsync(spielerId)`):** Integer player ID from the data grid, passed directly to `SpielerRepository.DeleteAsync(spielerId)`. No ownership check. File: `Presentation/Areas/Spieler/SpielerOverviewList.razor.cs` line 33-43.
- **Delete Game ID (via `CoiffeurOverviewList.DeleteRundeAsync(rundeId)`):** Integer game ID from the data grid, passed directly to `CoiffeurSpielrundeRepository.DeleteAsync(new CoiffeurSpielrundeId(rundeId))`. No ownership check. File: `Presentation/Areas/Coiffeur/Overview/CoiffeurOverviewList.razor.cs` line 37-51.

### HTTP Headers (Processed by Application)

- **`Cookie: .AspNetCore.*`:** Authentication session cookie. The ASP.NET Core cookie handler processes `ClaimsPrincipal` from this cookie. If the cookie can be forged or its claims manipulated (unlikely given OIDC + data protection), it would bypass all authentication.
- **`Host:` header:** `AllowedHosts: "*"` in `appsettings.json` line 33 — no host header validation. Potential host header injection if application is directly internet-exposed without a reverse proxy enforcing host.

### Cookie Values

- **`.AspNetCore.OpenIdConnect.Nonce.*`:** Nonce cookie for OIDC flow (path=/signin-oidc; secure; samesite=none; httponly). Processed by `Microsoft.Identity.Web` during `/signin-oidc` callback.
- **`.AspNetCore.Correlation.*`:** Correlation/state cookie for OIDC flow (path=/signin-oidc; secure; samesite=none; httponly). Processed by `Microsoft.Identity.Web` during `/signin-oidc` callback.
- **Authentication cookie (unnamed in config):** ASP.NET Core data-protection encrypted cookie containing user `ClaimsPrincipal`. Exact cookie name defaults to `Microsoft.AspNetCore.Authentication.Cookies` handler defaults.

---

## 6. Network & Interaction Map

### 6.1 Entities

| Title | Type | Zone | Tech | Data | Notes |
|---|---|---|---|---|---|
| UserBrowser | ExternAsset | Internet | Any browser | PII, Tokens | End-user browser; untrusted. All UI rendered server-side via SignalR. |
| JassAppKestrel | Service | App | ASP.NET Core 9.0 / Blazor Server / Kestrel | PII, Tokens | Main application backend. Serves Blazor pages, hosts SignalR hub `/_blazor`, processes OIDC callbacks. Port 7202/HTTPS. |
| AzureAD | ThirdParty | ThirdParty | Microsoft Entra ID / OIDC | Tokens | Azure AD tenant `d6fddda6-f690-4755-92c2-f22a3521bab0`. Issues `id_token` to application. Client ID: `15d9a56c-eba8-4b76-99a4-57de18bcc73b`. |
| AzureSQLDatabase | DataStore | Data | SQL Server (Azure SQL) | PII, Public | Stores all application data: players (names), game rounds, scores, team assignments. No row-level security, no user ownership columns. |
| ApplicationInsights | ThirdParty | ThirdParty | Azure Monitor / Application Insights | PII | Receives telemetry including user email as `AuthenticatedUserId`. Connection string is currently a placeholder. |

### 6.2 Entity Metadata

| Title | Metadata Key: Value |
|---|---|
| JassAppKestrel | Hosts: `https://host.docker.internal:7202`; TLS: `TLS 1.3 / TLS_AES_256_GCM_SHA384 / x25519`; Cert: `self-signed CN=localhost`; Server: `Kestrel`; HTTP Version: `HTTP/2`; Auth: `Azure AD OIDC cookie + SignalR circuit`; Framework: `.NET 9.0 ASP.NET Core Blazor Server`; IoC: `Lamar 15.0.1`; Security Headers: `None (no CSP, X-Frame-Options, X-Content-Type-Options)`; HSTS: `configured (730d, includeSubDomains) but only in non-Development`; AllowedHosts: `*` |
| AzureAD | Issuer: `https://login.microsoftonline.com/d6fddda6-f690-4755-92c2-f22a3521bab0/v2.0`; TenantId: `d6fddda6-f690-4755-92c2-f22a3521bab0`; ClientId: `15d9a56c-eba8-4b76-99a4-57de18bcc73b`; CallbackPath: `/signin-oidc`; ResponseType: `id_token`; ResponseMode: `form_post`; Scope: `openid profile`; Token Format: `JWT (id_token only)`; Library: `Microsoft.Identity.Web 3.12.0` |
| AzureSQLDatabase | Engine: `SQL Server (Azure SQL)`; Exposure: `Internal only (connection string in appsettings.json placeholder, replaced at deploy time)`; Consumers: `JassAppKestrel (EF Core 9.0.7)`; Tables: `SpielerTable, CoiffeurSpielrundeTable, JassTeamTable, JassTeamSpielerTable, TrumpfrundeTable`; ORM: `Entity Framework Core 9.0.7`; Logging: `EnableSensitiveDataLogging() active (all environments)`; UserOwnership: `NONE — no UserId/TenantId columns in any table` |
| ApplicationInsights | Connection: `placeholder "_" in appsettings.json (may not be active)`; PII: `User email sent as AuthenticatedUserId on all telemetry events`; Source: `AuthenticatedUserIdTelemetryInitializer.cs:13` |

### 6.3 Flows (Connections)

| FROM → TO | Channel | Path/Port | Guards | Touches |
|---|---|---|---|---|
| UserBrowser → JassAppKestrel | HTTPS | `:7202 /` | None (redirects to AzureAD) | Public |
| UserBrowser → JassAppKestrel | HTTPS | `:7202 MicrosoftIdentity/Account/SignIn` | None | Public |
| JassAppKestrel → AzureAD | HTTPS (302 redirect) | `login.microsoftonline.com /oauth2/v2.0/authorize` | None | Tokens |
| AzureAD → UserBrowser → JassAppKestrel | HTTPS (form_post) | `:7202 /signin-oidc` | nonce-cookie, state-cookie | Tokens |
| UserBrowser → JassAppKestrel | HTTPS + WS | `:7202 /_blazor` | auth:user (cookie validated before WS upgrade) | PII, Public |
| UserBrowser → JassAppKestrel | HTTPS | `:7202 static assets` | None | Public |
| JassAppKestrel → AzureSQLDatabase | TCP | SQL Server port (internal) | vpc-only | PII, Public |
| JassAppKestrel → ApplicationInsights | HTTPS | Azure Monitor endpoint | None (outbound) | PII |
| UserBrowser ←→ JassAppKestrel | WebSocket (SignalR) | `/_blazor` | auth:user | PII |

### 6.4 Guards Directory

| Guard Name | Category | Statement |
|---|---|---|
| auth:user | Auth | Requires a valid Azure AD authenticated session (ASP.NET Core cookie containing valid `ClaimsPrincipal`). Enforced by `FallbackPolicy = DefaultPolicy` in `ServiceInitialization.cs:43`. All application routes require this guard. |
| nonce-cookie | Protocol | `.AspNetCore.OpenIdConnect.Nonce.*` cookie must be present and valid for OIDC callback processing. Set at OIDC initiation, consumed at `/signin-oidc` POST. Secure, SameSite=None, HttpOnly. |
| state-cookie | Protocol | `.AspNetCore.Correlation.*` cookie must be present and valid for OIDC state validation. Set at OIDC initiation, consumed at `/signin-oidc` POST. Secure, SameSite=None, HttpOnly. |
| type-constraint:int | Protocol | Blazor route constraint `{param:int}` — rejects non-integer values at routing layer before reaching component. Applies to `playerId` and `gameId` parameters. Does NOT enforce ownership or range. |
| type-constraint:bool | Protocol | Blazor route constraint `{param:bool}` — rejects non-boolean values at routing layer. Applies to `spectatorMode` parameter. |
| anon-allowed | Auth | `[AllowAnonymous]` attribute exempts component from FallbackPolicy. Applied only to `ErrorInfo.razor:2` (error boundary display component). |

**Notable absence of guards:**
- No `ownership:*` guard — no check that the requesting user owns the resource identified by route parameter
- No `auth:admin` guard — no administrative privilege level exists
- No `role:*` guard — no role-based access control of any kind
- No `tenant:isolation` guard — no multi-tenant data isolation
- No `vpc-only` guard on application endpoints — app is internet-facing

---

## 7. Role & Privilege Architecture

### 7.1 Discovered Roles

| Role Name | Privilege Level | Scope/Domain | Code Implementation |
|---|---|---|---|
| anon | 0 | Global | No authentication. Can only access: static assets, `MicrosoftIdentity/Account/SignIn`, `MicrosoftIdentity/Account/Challenge`, `MicrosoftIdentity/Account/AccessDenied`, `/signin-oidc` callback. |
| user | 1 | Global | Any Azure AD user authenticated against tenant `d6fddda6-f690-4755-92c2-f22a3521bab0`. Enforced by `FallbackPolicy = DefaultPolicy` in `ServiceInitialization.cs:43`. All authenticated users have identical, unrestricted access to all data. |

**There is no admin role, moderator role, or any role-based differentiation in the application.**

### 7.2 Privilege Lattice

```
Privilege Ordering (→ means "can access resources of"):
anon → user (with Azure AD credentials)

Parallel Isolation: NONE
- All authenticated users share identical privileges
- No role hierarchy above "authenticated user"
- No tenant/org isolation between users

Note: No role switching, no impersonation, no sudo mode exists.
```

### 7.3 Role Entry Points

| Role | Default Landing Page | Accessible Route Patterns | Authentication Method |
|---|---|---|---|
| anon | N/A — redirected to Azure AD login | Static assets only (`/M.png`, `/app.css`, etc.), OIDC flows (`/signin-oidc`, `MicrosoftIdentity/Account/*`) | None |
| user | `/` (HomePage) | ALL application routes: `/`, `/players/overview`, `/players/edit/{id}`, `coiffeur/overview`, `coiffeur/configuration`, `coiffeur/game/{id}/{mode}`, `coiffeur/game/playerhistory`, `/test/exception`, `/test/informations`, `/dev/audio-player`, `/notfound` | ASP.NET Core auth cookie (set by Azure AD OIDC flow) |

### 7.4 Role-to-Code Mapping

| Role | Middleware/Guards | Permission Checks | Storage Location |
|---|---|---|---|
| anon | None (FallbackPolicy excludes anon from all routes) | `[AllowAnonymous]` on `ErrorInfo.razor:2` only | N/A |
| user | `FallbackPolicy = DefaultPolicy` (`ServiceInitialization.cs:43`) | `auth.User.Identity!.Name!` (email display only, `BenutzerMenu.razor.cs:27`) | ASP.NET Core auth cookie (OIDC `id_token` claims) |

---

## 8. Authorization Vulnerability Candidates

### 8.1 Horizontal Privilege Escalation Candidates (IDOR)

All object-parameterized routes are HIGH priority because there are zero ownership checks in the entire codebase.

| Priority | Endpoint Pattern | Object ID Parameter | Data Type | Sensitivity |
|---|---|---|---|---|
| **High** | `GET /players/edit/{playerId:int}` | `playerId` (int, route) | player_data (name) | Any user can view/edit ANY player's name by iterating integers. `SpielerEditPage.razor.cs:47` — `new SpielerSpec(new SpielerId(PlayerId))` with no ownership check. |
| **High** | `GET coiffeur/game/{gameId:int}/false` | `gameId` (int, route) | game_data (scores, teams) | Any user can view AND edit ANY game round in edit mode by iterating integers. `RunningGamePage.razor.cs:29` — `new CoiffeurSpielrundeSpec(new CoiffeurSpielrundeId(GameId))`. |
| **High** | `GET coiffeur/game/{gameId:int}/true` | `gameId` (int, route) | game_data (scores) | Any user can view any game in spectator mode. Same IDOR, read-only mode. |
| **High** | Player delete (via `coiffeur/overview` list action) | `spielerId` (int, from UI list) | player_data | `SpielerOverviewList.razor.cs:33-43` — `SpielerRepository.DeleteAsync(spielerId)` no ownership check. Deletes any player. |
| **High** | Game delete (via `coiffeur/overview` list action) | `rundeId` (int, from UI list) | game_data | `CoiffeurOverviewList.razor.cs:37-51` — `CoiffeurSpielrundeRepository.DeleteAsync(rundeId)` no ownership check. Deletes any game + cascades to JassTeam, Trumpfrunde records. |
| **Medium** | `GET coiffeur/overview` | None (lists all) | game_data | Returns ALL games from ALL users via `CoiffeurSpielrundeSpec()` with no args. `CoiffeurOverviewList.razor.cs:61`. |
| **Medium** | `GET coiffeur/game/playerhistory` | None (returns all) | game_data, player stats | Returns global history for ALL players/games. `SpielerHistoryPage.razor.cs:20` — `SpielerHistorySpec` has no user filter. |
| **Medium** | `GET /players/overview` | None (lists all) | player_data | Returns ALL players from ALL users via `SpielerSpec()` with no args. `SpielerOverviewPage.razor.cs`. |
| **Medium** | `GET coiffeur/configuration` | None | player_data | Team/player selection populates from ALL players: `CoiffeurConfigurationPage.razor.cs:51` — `new SpielerSpec()` returns every player. |

### 8.2 Vertical Privilege Escalation Candidates

No admin-restricted routes exist. All authenticated users already have maximum application privilege. There is no vertical escalation to attempt within the application's authorization model.

**Notable:** The following routes are "privileged" in that they expose test/dev functionality, but there is no role gate to bypass — any authenticated user can already access them:

| Target Role | Endpoint Pattern | Functionality | Risk Level |
|---|---|---|---|
| user (dev/test intent) | `/test/exception` | Intentionally throws an unhandled exception — triggers `ErrorInfo.razor` stack trace display | Medium (information disclosure) |
| user (dev/test intent) | `/test/informations` | Test information display page | Low |
| user (dev intent) | `/dev/audio-player` | Audio player UI demo | Low |

### 8.3 Context-Based Authorization Candidates

| Workflow | Endpoint | Expected Prior State | Bypass Potential |
|---|---|---|---|
| Game Creation → Running | `coiffeur/game/{gameId:int}/false` | Game created via `coiffeur/configuration` | Direct navigation to `coiffeur/game/{id}/false` with any integer ID bypasses configuration step and directly loads/modifies any game. |
| Game Spectate vs Edit | `coiffeur/game/{gameId:int}/{spectatorMode:bool}` | SpectatorMode=true for view-only | Changing `true` → `false` in URL converts spectate access to edit access for any game; no separate authorization between modes. `RunningGamePage.razor.cs:19-21`. |
| Player Edit vs Create | `/players/edit/{playerId:int}` | playerId=0 creates new, playerId>0 edits existing | Direct navigation with `playerId=0` creates a new player slot without going through overview; `PlayerId=0` check at `SpielerEditPage.razor.cs:36`. |

---

## 9. Injection Sources

**Assessment: No SQL Injection, Command Injection, SSTI, LFI/RFI, or Deserialization sources were identified in network-accessible code paths.**

### SQL Injection
**None found.** All database queries use Entity Framework Core LINQ with parameterized queries exclusively. No `FromSqlRaw`, `ExecuteSqlRaw`, `ExecuteSqlInterpolated`, or ADO.NET `SqlCommand` with string concatenation exists anywhere. The complete data flow for user-controlled input:

- **Player Name:** `MudTextField` → `string` → `Guard.StringNotNullOrEmpty()` → `SpielerTable.Name` → EF Core parameterized INSERT/UPDATE (parameter `@p0`). File chain: `SpielerEditPage.razor:9` → `SpielerEditViewModel.cs:9-10` → `SpielerEditPage.razor.cs:65,69` → `SpielerRepository.cs:24` → EF Core.
- **Game config integers:** `MudNumericField` → `int` → EF Core parameterized INSERT. No string concatenation.

Note: `EnableSensitiveDataLogging()` in `DbContextOptionsFactory.cs:19` causes actual parameter values to be logged, but this is an information disclosure issue, not an injection source.

### Command Injection
**None found.** No `System.Diagnostics.Process`, `ProcessStartInfo`, shell execution, or OS command APIs exist anywhere in the codebase.

### LFI / RFI / Path Traversal
**None found.** No `File.ReadAllText`, `File.Open`, `StreamReader`, or file path construction with user input exists. `JavaScriptLocator.cs` constructs file paths from compiled type metadata only.

### SSTI (Server-Side Template Injection)
**None found.** Blazor Server uses a compiled Razor template engine — there is no dynamic template compilation, no `Template.Parse(userInput)`, no string-interpolated templates. All `@variable` expressions in Razor are HTML-encoded.

### Deserialization
**None found.** No `JsonSerializer.Deserialize` with user-supplied data, no `BinaryFormatter`, no `XmlSerializer` accepting user input.

### Notable Non-Injection Information Disclosure Sources

While not injection sinks, these sources expose sensitive data via network-accessible paths:

1. **Stack trace disclosure:** `Sources/Application/Presentation/Shell/Errors/Exceptions/ErrorInfo.razor` line 22 renders `@AppError?.StrackTrace` to the browser. Reachable by triggering any unhandled exception (e.g., navigating to `/test/exception`). The `[AllowAnonymous]` attribute on `ErrorInfo.razor:2` makes this component accessible regardless of auth state in error scenarios. Stack trace originates in `Routes.razor.cs:24`.

2. **`DetailedErrors: true`** in `appsettings.json:16` — sends detailed Blazor SignalR circuit error messages (including exception details) to clients via `/_blazor` WebSocket.

3. **SQL query logging with parameter values:** `DbContextOptionsFactory.cs:18-19` — `.LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging()`. All SQL with actual parameter values written to stdout. Not directly network-accessible but constitutes log-based information disclosure.

4. **GitHub commit URL disclosure:** `BenutzerMenu.razor.cs` constructs `https://github.com/DrMueller/JassApp/commit/{hash}` — the source code repository location and exact deployed commit hash is displayed in the authenticated UI.
