# Code Analysis Deliverable — JassApp Security Assessment

---

# Penetration Test Scope & Boundaries

**Primary Directive:** This analysis is strictly limited to the **network-accessible attack surface** of the JassApp application. All findings adhere to the scope criteria below.

### In-Scope: Network-Reachable Components
- All 11 Blazor Server routable pages served via the ASP.NET Core Kestrel/IIS pipeline
- The Blazor Server SignalR WebSocket endpoint (`/_blazor`)
- Microsoft Identity UI controller routes (`MicrosoftIdentity/Account/*`)
- Static assets served via `MapStaticAssets()` from `wwwroot/`
- The ASP.NET Core middleware pipeline (HTTPS redirect, antiforgery, exception handling)

### Out-of-Scope: Locally Executable Only
- **Unit Tests** (`Sources/UnitTests/`): Test project, not deployed or network-accessible
- **Azure DevOps Pipelines** (`Pipelines/`): CI/CD YAML definitions, run only in Azure DevOps agents
- **EF Core Migrations** (`Sources/Application/Migrations/`): Applied via CLI tooling or deployment scripts, not network-callable
- **`GitAddPushAll.cmd`**: Local developer convenience script
- **`DesignTimeAppDbContextFactory`**: Used only by `dotnet ef` CLI tooling at design time
- **Build artifacts** (`bin/`, `obj/`): Compilation outputs, not independently accessible

---

## 1. Executive Summary

JassApp is a .NET 9.0 Blazor Server application that tracks scores for the Swiss card game "Jass" (specifically the Coiffeur variant). The application is deployed to Azure App Service and authenticates all users via Azure Active Directory (Microsoft Entra ID) using OpenID Connect. The security posture benefits significantly from this design choice: there are no local credentials, no password storage, and the authentication boundary is delegated entirely to Microsoft's identity platform. The global authorization fallback policy (`FallbackPolicy = DefaultPolicy`) ensures that all endpoints require authentication by default, which is a strong secure-by-default architecture.

However, several significant security concerns exist. The most critical is the **complete absence of data-level authorization**: once authenticated, any user can read and modify all players and game data belonging to every other user in the system (Insecure Direct Object Reference / IDOR). The database schema contains no user/tenant ownership columns, and no query-level filtering by authenticated user identity exists. Additionally, **`EnableSensitiveDataLogging()` is unconditionally active** in all environments, causing EF Core to log SQL parameter values (including player names) to stdout in production. The application also exposes **detailed error information** including stack traces to clients via both the `DetailedErrors: true` SignalR configuration and the `[AllowAnonymous]` error display component.

From an infrastructure perspective, the middleware pipeline has notable gaps: `UseAuthentication()` and `UseAuthorization()` are not explicitly registered, `UseCookiePolicy()` is configured but never applied, and no security headers (CSP, X-Frame-Options, X-Content-Type-Options) are set at the application level. The `GlobalExceptionHandlingMiddleware` class exists but is dead code — never registered in the pipeline. The attack surface is relatively small for a Blazor Server app (no REST APIs, no file uploads, no custom SignalR hubs), but the IDOR vulnerability and information disclosure issues represent the primary risks for an external attacker who has obtained valid Azure AD credentials.

---

## 2. Architecture & Technology Stack

- **Framework & Language:** ASP.NET Core 9.0 with C#, targeting `net9.0`. The application uses the Blazor Server interactive rendering model with SignalR WebSocket transport. All UI rendering occurs server-side; no WebAssembly code is sent to clients. This means component state, event handlers, and data access all execute on the server, reducing client-side attack surface but concentrating risk on the SignalR connection and server memory. The security implication is that an attacker cannot inspect or tamper with component logic client-side, but can manipulate SignalR messages to trigger server-side event handlers. The `InteractiveServerRenderMode(false)` configuration disables prerendering, which prevents the common Blazor issue where prerendered content is briefly visible before authentication redirects.

- **Architectural Pattern:** The application follows a clean layered architecture: `Presentation` (Blazor Razor components) → `Domain` (business logic, models, specifications) → `DataAccess` (EF Core repositories, Unit of Work). The IoC container is Lamar 15.0.1 (a StructureMap successor), which replaces the default ASP.NET Core DI with assembly-scanning registration. Trust boundaries exist between the browser (untrusted) and the Blazor Server SignalR circuit (trusted server-side), and between the application server and Azure SQL Database. There is no API gateway, no microservice decomposition, and no message queue — this is a monolithic server-rendered application. The single trust boundary of interest is the SignalR connection, where all user interactions are serialized as SignalR invocations.

- **Critical Security Components:**
  - **Authentication:** Microsoft Identity Web 3.12.0 with Azure AD OpenID Connect (`Microsoft.Identity.Web`)
  - **Database:** Entity Framework Core 9.0.7 with SQL Server (`Microsoft.EntityFrameworkCore.SqlServer`)
  - **UI Framework:** MudBlazor 8.10.0 (Material Design component library for Blazor)
  - **Telemetry:** Application Insights 2.23.0 (Azure Monitor integration)
  - **Speech:** `Microsoft.CognitiveServices.Speech` 1.47.0 is referenced but **unused** — the actual speech synthesis uses `Toolbelt.Blazor.SpeechSynthesis` 11.0.0 (browser Web Speech API via JS interop)
  - **IoC:** Lamar 15.0.1 with assembly scanning

| Dependency | Version | Security Relevance |
|---|---|---|
| `Microsoft.Identity.Web` | 3.12.0 | Azure AD OIDC authentication library |
| `Microsoft.Identity.Web.UI` | 3.12.0 | Pre-built sign-in/sign-out controller routes |
| `Microsoft.EntityFrameworkCore.SqlServer` | 9.0.7 | Database access layer (SQL injection surface) |
| `Microsoft.ApplicationInsights.AspNetCore` | 2.23.0 | Telemetry (PII collection surface) |
| `Microsoft.CognitiveServices.Speech` | 1.47.0 | Referenced but unused (dead dependency) |
| `MudBlazor` | 8.10.0 | UI component library (XSS encoding responsibility) |
| `Lamar` | 15.0.1 | IoC container with assembly scanning |

---

## 3. Authentication & Authorization Deep Dive

### Authentication Mechanism

The application uses **Azure Active Directory (Microsoft Entra ID)** via OpenID Connect, implemented through the `Microsoft.Identity.Web` library. Configuration is in `Sources/Application/Presentation/Shell/Initialization/ServiceInitialization.cs` (lines 40-41):

```csharp
services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(Program.Configuration.GetSection("AzureAd"));
```

The Azure AD configuration is sourced from `appsettings.json` with placeholder values (`"your-tenant-id"`, `"your-client-id"`, `"your-client-secret"`). The callback path is `/signin-oidc` (OIDC default). In production, these placeholders are replaced via CI/CD token substitution (`replacetokens@5` Azure DevOps task). There are no local login forms, no password storage, and no registration flows — authentication is entirely delegated to Azure AD.

**Authentication API Endpoints** (provided by `Microsoft.Identity.Web.UI` via `MapControllers()`):

| Endpoint | Method | Purpose | Auth Required |
|---|---|---|---|
| `MicrosoftIdentity/Account/SignIn` | GET | Initiates Azure AD OIDC login flow | No |
| `MicrosoftIdentity/Account/SignOut` | GET | Signs out the user | Yes |
| `MicrosoftIdentity/Account/Challenge` | GET | Forces re-authentication | No |
| `MicrosoftIdentity/Account/AccessDenied` | GET | Displays access denied page | No |
| `/signin-oidc` | POST | OIDC callback (token receipt) | No (framework-handled) |

The sign-out link is used in `Sources/Application/Presentation/Shell/Benutzer/BenutzerMenu.razor` (line 5). The `BenutzerMenu.razor.cs` component accesses the authenticated user's email via `AuthenticationStateProvider` and displays it in the UI. It also constructs a GitHub commit URL: `https://github.com/DrMueller/JassApp/commit/{0}` — revealing the source code repository location.

**SSO/OAuth/OIDC Flow:** The `state` and `nonce` parameter validation is handled entirely by the `Microsoft.Identity.Web` library's built-in OIDC middleware. No custom validation code exists in the application. The callback endpoint `/signin-oidc` is processed by the framework's `OpenIdConnectHandler`. This is the standard secure implementation — no custom overrides that could introduce vulnerabilities.

### Session Management & Cookie Security

The application does **not** use traditional HTTP sessions (`AddSession`/`UseSession` are absent). Blazor Server maintains state via SignalR circuits (persistent WebSocket connections). The OIDC authentication cookie is managed by the `Microsoft.Identity.Web` middleware with default ASP.NET Core cookie settings.

**Cookie policy is configured but NEVER APPLIED.** In `ServiceInitialization.cs` (lines 32-36):
```csharp
services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = _ => true;
    options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
});
```
However, `UseCookiePolicy()` is **not called** in the middleware pipeline (`AppInitialization.cs`). This means the `SameSiteMode.Unspecified` setting and consent check are dead configuration. The actual cookie flags (`HttpOnly`, `Secure`, `SameSite`) are determined by the OIDC middleware's defaults, which typically set `HttpOnly = true`, `Secure` based on the request scheme, and `SameSite = Lax`.

**Exact location of cookie configuration:** `Sources/Application/Presentation/Shell/Initialization/ServiceInitialization.cs`, lines 32-36. The `UseCookiePolicy()` call is absent from `Sources/Application/Presentation/Shell/Initialization/AppInitialization.cs`.

### Authorization Model & Bypass Scenarios

The authorization configuration uses a **fallback policy** (`ServiceInitialization.cs`, line 43):
```csharp
services.AddAuthorization(options => { options.FallbackPolicy = options.DefaultPolicy; });
```

This means every endpoint requires an authenticated user by default. The only `[AllowAnonymous]` attribute is on `ErrorInfo.razor` (line 2) — a non-routable child component that displays error details.

**Critical gap — `RouteView` vs `AuthorizeRouteView`:** The `Routes.razor` file (line 8) uses `<RouteView>` instead of `<AuthorizeRouteView>`. In .NET 9 Blazor, `RouteView` does **not** enforce `[Authorize]` attributes within the Blazor component tree. The fallback policy works at the HTTP middleware level, but `AuthorizeRouteView` is the standard mechanism for Blazor-specific authorization enforcement. This could create race conditions where components briefly render before authorization middleware redirects.

**Critical gap — Missing middleware:** `UseAuthentication()` and `UseAuthorization()` are NOT explicitly called in the middleware pipeline (`AppInitialization.cs`). While `MapControllers()` and `MapRazorComponents()` may implicitly wire up authorization, the explicit middleware registration is the documented best practice. This inconsistency should be verified during testing.

**No role-based or policy-based authorization exists.** There are no `[Authorize(Roles=...)]` or `[Authorize(Policy=...)]` attributes anywhere in the codebase. All authenticated users have identical privileges.

### Multi-tenancy Security

**There is no multi-tenancy implementation.** The database schema contains no user ownership columns (`UserId`, `TenantId`, `CreatedBy`). All authenticated users share a single flat data space. This means:
- Any authenticated user can view all players created by any other user
- Any authenticated user can edit any player's name
- Any authenticated user can view and modify any game round
- Route parameters like `/players/edit/{playerId:int}` and `coiffeur/game/{gameId:int}/{spectatorMode:bool}` accept arbitrary integer IDs with no ownership validation

---

## 4. Data Security & Storage

### Database Security

The application uses **Entity Framework Core 9.0.7** with **SQL Server**. The database schema consists of 5 tables: `SpielerTable` (players), `CoiffeurSpielrundeTable` (game rounds), `JassTeamTable` (teams), `JassTeamSpielerTable` (team-player assignments), and `TrumpfrundeTable` (trump rounds). The only PII stored is the `Name` column in `SpielerTable` (`nvarchar(max)`) — player names/nicknames. No encryption at rest is configured at the column level.

All database queries use **EF Core LINQ** exclusively — no `FromSqlRaw`, `ExecuteSqlRaw`, or raw ADO.NET exists anywhere in the codebase. The repository pattern (`RepositoryBase.cs`) and specification pattern (`IQuerySpecification<T>`) ensure all queries are parameterized. The `CommandInterceptor` (`Sources/Application/DataAccess/DbContexts/Factories/Implementation/CommandInterceptor.cs`) is a read-only logging interceptor that writes `CommandText` to `Console.WriteLine` — it does not modify queries.

**CRITICAL ISSUE — `EnableSensitiveDataLogging()`** is unconditionally enabled in `DbContextOptionsFactory.cs` (line 19). Combined with `LogTo(Console.WriteLine, LogLevel.Information)` (line 18), this causes EF Core to log all SQL queries **with actual parameter values** to stdout in all environments, including production. This means player names and entity IDs are written to production logs.

### Data Flow Security

Sensitive data flows in this application are minimal:
1. **Authentication flow:** User email flows from Azure AD → OIDC middleware → `AuthenticationStateProvider` → `BenutzerMenu` component (display only, not persisted)
2. **Player data flow:** Player names are entered via `SpielerEditPage` → domain model `Spieler` → `SpielerRepository` → SQL Server `SpielerTable`. No sanitization or validation beyond EF Core's built-in parameterization
3. **Telemetry flow:** User email is extracted from claims and sent to Application Insights as `AuthenticatedUserId` via `AuthenticatedUserIdTelemetryInitializer.cs` (line 13). This constitutes PII transmission to a telemetry system.
4. **Game data flow:** Game configuration and scores flow through `CoiffeurSpielrundeFactory` → repositories → SQL Server. Integer IDs from route parameters are passed directly to EF Core specifications without ownership checks.

### Multi-tenant Data Isolation

**No data isolation exists.** All queries return all data in the database regardless of the authenticated user. The `SpielerSpec` with no arguments returns all players; `CoiffeurSpielrundeSpec` with no arguments returns all game rounds. There is no row-level security, no tenant filtering, and no ownership model.

---

## 5. Attack Surface Analysis

### External Entry Points (In-Scope, Network-Accessible)

**Blazor Server Routable Pages** (all require Azure AD authentication):

| Route | File | Parameters | Security Notes |
|---|---|---|---|
| `/` | `Presentation/Areas/Home/HomePage.razor` | None | Landing page |
| `/players/overview` | `Presentation/Areas/Spieler/SpielerOverviewPage.razor` | None | Lists all players (no user filtering) |
| `/players/edit/{playerId:int}` | `Presentation/Areas/Spieler/SpielerEditPage.razor` | `playerId` (int) | **IDOR: any user can edit any player** |
| `coiffeur/overview` | `Presentation/Areas/Coiffeur/Overview/CoiffeurOverviewPage.razor` | None | Lists all game rounds |
| `coiffeur/configuration` | `Presentation/Areas/Coiffeur/Configuration/CoiffeurConfigurationPage.razor` | None | Create new games |
| `coiffeur/game/{gameId:int}/{spectatorMode:bool}` | `Presentation/Areas/Coiffeur/RunningGame/RunningGamePage.razor` | `gameId` (int), `spectatorMode` (bool) | **IDOR: any user can access any game** |
| `coiffeur/game/playerhistory` | `Presentation/Areas/Coiffeur/SpielerHistory/SpielerHistoryPage.razor` | None | Player statistics |
| `/test/exception` | `Presentation/Areas/Test/Components/TestExceptionPage.razor` | None | **Test page — throws exception on purpose** |
| `/test/informations` | `Presentation/Areas/Test/Components/TestInformationsPage.razor` | None | Test page for info display |
| `/dev/audio-player` | `Presentation/Areas/Dev/AudioPlayerDemoPage.razor` | None | **Dev utility exposed in production** |
| `/notfound` | `Presentation/Shell/Errors/NotFound/NotFoundPage.razor` | None | Custom 404 page |

**Microsoft Identity Controller Routes** (framework-provided):

| Route | Auth Required | Notes |
|---|---|---|
| `MicrosoftIdentity/Account/SignIn` | No | Public OIDC login initiation |
| `MicrosoftIdentity/Account/SignOut` | Yes | Authenticated sign-out |
| `MicrosoftIdentity/Account/Challenge` | No | Re-authentication challenge |
| `MicrosoftIdentity/Account/AccessDenied` | No | Access denied display |

**SignalR Endpoint:**

| Route | Protocol | Notes |
|---|---|---|
| `/_blazor` | WebSocket (SignalR) | All Blazor interactivity; authentication enforced via fallback policy |

**Static Assets** (publicly accessible, no authentication):
- `/M.png` — Application icon
- `/app.css` — Application stylesheet
- `/audio/jeopardy.mp3` — Audio file
- `/js/localstorage.js` — JS interop module (localStorage get/set)
- `/lib/bootstrap/dist/` — Bootstrap CSS and JS files
- `/_framework/blazor.web.js` — Blazor framework JS
- `/_content/MudBlazor/*` — MudBlazor component assets

### Notable Out-of-Scope Components
- **`Sources/UnitTests/`**: Test project, not deployed
- **`Pipelines/`**: Azure DevOps CI/CD definitions
- **`Sources/Application/Migrations/`**: EF Core migration files, applied via CLI
- **`DesignTimeAppDbContextFactory`**: Design-time only factory for `dotnet ef`

### Input Validation Patterns

The application relies on:
1. **Route parameter type constraints** (`:int`, `:bool`) to reject non-numeric route values
2. **EF Core parameterization** for all database queries
3. **MudBlazor component validation** for form inputs (standard Blazor `EditForm` validation)
4. **No custom input validation middleware** or sanitization layers exist

There is no server-side input length validation on the `Spieler.Name` field — the database column is `nvarchar(max)` with no constraint, and no validation attribute limits the input length in the domain model or edit form.

### Background Processing

No background processing, async jobs, or queue workers exist in this application. All operations are synchronous request-response within the Blazor Server SignalR circuit.

---

## 6. Infrastructure & Operational Security

### Secrets Management

Secrets are managed through a **placeholder-and-replace** pattern:
- `appsettings.json` contains placeholders (`"your-client-secret"`, `"_"` for connection strings)
- In development: User Secrets are loaded via `ConfigurationFactory.cs` (line 17-19)
- In production: The CI/CD pipeline uses `replacetokens@5` to substitute placeholders with values from the `JassApp-Production` variable group
- **No Azure Key Vault integration** exists — the deployed `appsettings.json` on disk contains real secrets in plaintext after token replacement
- The `keepToken: true` setting in `Create_Artifacts.yaml` (line 37) means unreplaced tokens are preserved rather than cleared, potentially masking configuration errors

### Configuration Security

**HSTS:** Enabled in non-Development environments with 730-day max-age and `IncludeSubDomains = true` (`ServiceInitialization.cs`, lines 26-30). `Preload` is not set.

**HTTPS Redirection:** Enabled via `UseHttpsRedirection()` (`AppInitialization.cs`, line 18).

**Missing Security Headers:**
- No `Content-Security-Policy` (CSP) header
- No `X-Frame-Options` header (clickjacking risk)
- No `X-Content-Type-Options` header (MIME sniffing risk)
- No `Referrer-Policy` header
- No `Permissions-Policy` header

**`AllowedHosts: "*"`** in `appsettings.json` (line 33) — no host header filtering, enabling potential host header injection attacks unless the reverse proxy enforces host validation.

**`DetailedErrors: true`** in `appsettings.json` (line 16) — exposes detailed Blazor circuit error messages including stack traces via SignalR to clients. If this value is not overridden in production configuration, it constitutes significant information disclosure.

**No infrastructure configuration files** (Nginx, Kubernetes Ingress, CDN settings) exist in the repository. Security headers like `Strict-Transport-Security` and `Cache-Control` would need to be configured at the Azure App Service level or via a reverse proxy not defined in this codebase.

### External Dependencies

- **Azure Active Directory:** Authentication provider (outbound OIDC calls to `login.microsoftonline.com`)
- **Azure SQL Database:** Data persistence (connection via EF Core)
- **Azure Application Insights:** Telemetry collection (outbound telemetry to Azure Monitor — currently configured with placeholder connection string `"_"`)
- **Azure App Service:** Hosting platform (Windows, deployed via `AzureRmWebAppDeployment@4`)
- **Microsoft.CognitiveServices.Speech:** NuGet referenced but unused — no API keys configured, no code calls the SDK

### Monitoring & Logging

- **Application Insights** is configured (`ServiceInitialization.cs`, line 38) but with a placeholder connection string, meaning telemetry may not be active in production unless the connection string is replaced
- **Custom logging** via `LoggingService` uses `TelemetryClient.TrackEvent()` and `ILogger` for generic events and errors
- **No security-specific event logging** — no failed authentication logging, no authorization failure logging, no suspicious activity detection. Azure AD handles login auditing on its platform
- **PII in telemetry:** User email is sent as `AuthenticatedUserId` via `AuthenticatedUserIdTelemetryInitializer`
- **SQL query logging:** All EF Core queries (with parameter values due to `EnableSensitiveDataLogging`) are logged to stdout unconditionally via `Console.WriteLine`

---

## 7. Overall Codebase Indexing

The JassApp codebase is organized as a Visual Studio solution (`JassApp.sln`) with two projects: the main application (`Sources/Application/`) and a unit test project (`Sources/UnitTests/`). The application follows a clean layered architecture with four primary layers organized as top-level directories within `Sources/Application/`: `Common` (cross-cutting concerns like logging, settings, language extensions), `Domain` (business logic organized by bounded context — `Coiffeur` for game logic, `Spieler` for player management, `Shared` for common data access interfaces), `DataAccess` (EF Core implementation including `DbContexts`, `Repositories`, `Tables`, `Configurations`, `Querying`, and `UnitOfWorks`), and `Presentation` (Blazor UI organized into `Areas` by feature, `Infrastructure` for middleware and services, `Shared` for reusable components, and `Shell` for the application shell). The `Pipelines/` directory contains Azure DevOps YAML pipeline definitions for CI, artifact creation, and release. Build orchestration uses standard MSBuild via the `.csproj` file with no custom build tools. The IoC container (Lamar) uses assembly scanning with registries for service registration. Security-relevant components are distributed across multiple layers: authentication configuration in `Presentation/Shell/Initialization/`, database access in `DataAccess/`, and logging/telemetry in `Common/Logging/`. The `Integrations/Speech/` directory exists but is empty — it appears to be a placeholder for future server-side speech integration. The codebase uses German naming conventions for domain concepts (Spieler = Player, Coiffeur = a Jass game variant, Trumpfrunde = Trump round, Benutzer = User), which may slow down initial security analysis.

---

## 8. Critical File Paths

### Configuration
- `Sources/Application/appsettings.json`
- `Sources/Application/appsettings.Development.json`
- `Sources/Application/JassApp.csproj`
- `Sources/Application/Properties/launchSettings.json`
- `JassApp.sln`

### Authentication & Authorization
- `Sources/Application/Presentation/Shell/Initialization/ServiceInitialization.cs`
- `Sources/Application/Presentation/Shell/Initialization/AppInitialization.cs`
- `Sources/Application/Presentation/Shell/Benutzer/BenutzerMenu.razor`
- `Sources/Application/Presentation/Shell/Benutzer/BenutzerMenu.razor.cs`
- `Sources/Application/Presentation/Shell/App.razor`
- `Sources/Application/Presentation/Shell/Errors/Exceptions/ErrorInfo.razor`
- `Sources/Application/Presentation/Shell/Errors/Exceptions/ErrorInfo.razor.cs`
- `Sources/Application/Presentation/Shell/Errors/NotFound/NotFoundMiddleware.cs`
- `Sources/Application/Presentation/Shell/Errors/NotFound/NotFoundPage.razor`

### API & Routing
- `Sources/Application/Program.cs`
- `Sources/Application/Presentation/Areas/Home/HomePage.razor`
- `Sources/Application/Presentation/Areas/Spieler/SpielerEditPage.razor`
- `Sources/Application/Presentation/Areas/Spieler/SpielerEditPage.razor.cs`
- `Sources/Application/Presentation/Areas/Spieler/SpielerOverviewPage.razor`
- `Sources/Application/Presentation/Areas/Spieler/SpielerOverviewPage.razor.cs`
- `Sources/Application/Presentation/Areas/Spieler/SpielerOverviewList.razor.cs`
- `Sources/Application/Presentation/Areas/Coiffeur/Overview/CoiffeurOverviewPage.razor`
- `Sources/Application/Presentation/Areas/Coiffeur/Overview/CoiffeurOverviewList.razor.cs`
- `Sources/Application/Presentation/Areas/Coiffeur/Configuration/CoiffeurConfigurationPage.razor`
- `Sources/Application/Presentation/Areas/Coiffeur/Configuration/CoiffeurConfigurationPage.razor.cs`
- `Sources/Application/Presentation/Areas/Coiffeur/RunningGame/RunningGamePage.razor`
- `Sources/Application/Presentation/Areas/Coiffeur/RunningGame/RunningGamePage.razor.cs`
- `Sources/Application/Presentation/Areas/Coiffeur/SpielerHistory/SpielerHistoryPage.razor`
- `Sources/Application/Presentation/Areas/Test/Components/TestExceptionPage.razor`
- `Sources/Application/Presentation/Areas/Test/Components/TestExceptionPage.razor.cs`
- `Sources/Application/Presentation/Areas/Test/Components/TestInformationsPage.razor`
- `Sources/Application/Presentation/Areas/Dev/AudioPlayerDemoPage.razor`

### Data Models & DB Interaction
- `Sources/Application/DataAccess/Tables/SpielerTable.cs`
- `Sources/Application/DataAccess/Tables/CoiffeurSpielrundeTable.cs`
- `Sources/Application/DataAccess/Tables/JassTeamTable.cs`
- `Sources/Application/DataAccess/Tables/JassTeamSpielerTable.cs`
- `Sources/Application/DataAccess/Tables/TrumpfrundeTable.cs`
- `Sources/Application/DataAccess/Tables/Base/TableBase.cs`
- `Sources/Application/DataAccess/Querying/QueryService.cs`
- `Sources/Application/DataAccess/Repositories/Base/RepositoryBase.cs`
- `Sources/Application/DataAccess/Repositories/SpielerRepository.cs`
- `Sources/Application/DataAccess/Repositories/CoiffeurSpielrundeRepository.cs`
- `Sources/Application/DataAccess/DbContexts/Contexts/Implementation/AppDbContext.cs`
- `Sources/Application/DataAccess/DbContexts/Factories/Implementation/DbContextOptionsFactory.cs`
- `Sources/Application/DataAccess/DbContexts/Factories/Implementation/AppDbContextFactory.cs`
- `Sources/Application/DataAccess/DbContexts/Factories/Implementation/CommandInterceptor.cs`
- `Sources/Application/DataAccess/Configurations/SpielerTableConfig.cs`
- `Sources/Application/Domain/Spieler/Models/Spieler.cs`
- `Sources/Application/Domain/Coiffeur/Models/CoiffeurSpielrunde.cs`
- `Sources/Application/Domain/Coiffeur/Specifications/CoiffeurSpielrundeSpec.cs`
- `Sources/Application/Domain/Spieler/Specifications/SpielerSpec.cs`
- `Sources/Application/Domain/Spieler/Specifications/SpielerHistorySpec.cs`
- `Sources/Application/Migrations/20250826134350_Init.cs`

### Dependency Manifests
- `Sources/Application/JassApp.csproj`

### Sensitive Data & Secrets Handling
- `Sources/Application/Common/Settings/Provisioning/Models/AppSettings.cs`
- `Sources/Application/Common/Settings/Provisioning/Services/Implementation/SettingsProvider.cs`
- `Sources/Application/Common/Settings/Config/Services/ConfigurationFactory.cs`

### Middleware & Input Validation
- `Sources/Application/Presentation/Infrastructure/ExceptionHandling/GlobalExceptionHandlingMiddleware.cs`
- `Sources/Application/Presentation/Infrastructure/ExceptionHandling/ApplicationBuilderExtensions.cs`
- `Sources/Application/Presentation/Infrastructure/Navigation/Services/Implementation/Navigator.cs`
- `Sources/Application/Presentation/Infrastructure/Caching/Controllers/Implementation/CachingService.cs`

### Logging & Monitoring
- `Sources/Application/Common/Logging/Services/Implementation/LoggingService.cs`
- `Sources/Application/Common/Logging/Services/ILoggingService.cs`
- `Sources/Application/Common/Logging/Services/Models/LogInfo.cs`
- `Sources/Application/Common/Logging/Services/Servants/Implementation/AuthenticatedUserIdTelemetryInitializer.cs`
- `Sources/Application/Common/Logging/Services/Servants/Implementation/CloudRoleInstanceTelemetryInitializer.cs`
- `Sources/Application/Presentation/Infrastructure/Logging/Services/LogInfoProvider.cs`

### Infrastructure & Deployment
- `Pipelines/Building/Continous_Integration.yaml`
- `Pipelines/Building/Create_Artifacts.yaml`
- `Pipelines/Releasing/Release.yaml`
- `Pipelines/Releasing/Release_Steps.yaml`
- `Pipelines/Releasing/StepTemplates/Initialize.yaml`
- `Pipelines/Releasing/StepTemplates/Release_App.yaml`
- `Pipelines/Releasing/StepTemplates/Release_Database.yaml`

### Static Assets & JavaScript
- `Sources/Application/wwwroot/js/localstorage.js`
- `Sources/Application/Presentation/Shared/Audio/AudioPlayer.razor.js`
- `Sources/Application/Presentation/Shared/Voices/Components/VoiceSupport.razor.js`

---

## 9. XSS Sinks and Render Contexts

**Network Surface Focus:** This analysis covers only XSS sinks in Blazor Server pages served to authenticated users via the network-accessible application.

### Assessment: No Active XSS Sinks Found

The JassApp codebase does **not** contain any traditional XSS sinks in its network-accessible components. The analysis covered all Razor components, JavaScript interop calls, and static JavaScript files.

**Detailed findings:**

1. **MarkupString / Html.Raw:** No `(MarkupString)` casts or `@Html.Raw()` calls exist anywhere in the codebase. All Razor templates use standard MudBlazor components with Blazor's default HTML encoding.

2. **JavaScript Interop (IJSRuntime):** Several JS interop calls exist but none pass user-controlled data to dangerous sinks:
   - `Sources/Application/Presentation/Infrastructure/Navigation/Services/Implementation/Navigator.cs` (line 20): `jsRuntime.InvokeVoidAsync("open", target, "_blank")` — the `target` parameter comes from hardcoded `Path` constants formatted with integer IDs. The `OpenInNewTabAsync` method is **never called** from any component in the codebase.
   - `Sources/Application/Presentation/Shared/Voices/Components/VoiceSupport.razor.cs`: JS interop for browser speech synthesis feature detection — read-only, no DOM manipulation.
   - `Sources/Application/Presentation/Shared/Audio/AudioPlayer.razor.cs`: JS interop for audio playback (`play()`/`pause()`) on `ElementReference` — no user data passed.
   - `Sources/Application/Presentation/Shared/Storage/Implementation/LocalStorageProxy.cs`: JS interop for `localStorage.getItem`/`setItem` — keys and values are application-controlled.

3. **JavaScript files in wwwroot:**
   - `Sources/Application/wwwroot/js/localstorage.js`: Simple `localStorage.getItem`/`setItem` wrappers. No `innerHTML`, `eval`, `document.write`, or any DOM manipulation sinks.
   - `Sources/Application/Presentation/Shared/Audio/AudioPlayer.razor.js`: Calls `play()`/`pause()` on passed `ElementReference`. No DOM manipulation.
   - `Sources/Application/Presentation/Shared/Voices/Components/VoiceSupport.razor.js`: Read-only feature detection (`window.speechSynthesis`). No DOM manipulation.

4. **Blazor rendering model protection:** Blazor Server's default rendering pipeline HTML-encodes all `@variable` expressions in Razor templates. MudBlazor components (`MudTextField`, `MudButton`, `MudDataGrid`, etc.) use parameterized rendering that does not create raw HTML injection opportunities.

5. **Potential future concern — Speech synthesis:** Player names (user-supplied, stored in DB) flow to the browser's Web Speech API via `SpeechSynthesis.SpeakAsync()` in `RunningGameInfos.razor.cs` (lines 50-69) and `TeamDetails.razor.cs` (line 25). While this is not an XSS sink (speech synthesis does not render HTML), it could be a vector for social engineering or abuse if player names contain unexpected content.

6. **Error page information disclosure** (related but not XSS): `Sources/Application/Presentation/Shell/Errors/Exceptions/ErrorInfo.razor` (line 22) displays `@AppError?.StrackTrace` to the user. This renders via Blazor's safe encoding (not raw HTML), so it is not an XSS sink, but it is an information disclosure concern. This component is marked `[AllowAnonymous]`.

---

## 10. SSRF Sinks

**Network Surface Focus:** This analysis covers only SSRF sinks in network-accessible server-side components.

### Assessment: No SSRF Sinks Found

The JassApp application has an extremely small outbound network surface. No user-controlled data influences any server-side outbound requests.

**Detailed findings:**

### HTTP(S) Clients
**None found.** The application does not use `HttpClient`, `IHttpClientFactory`, `HttpClientHandler`, `WebClient`, `WebRequest`, or any REST client library.

### Raw Sockets & Connect APIs
**None found.** No socket connections, `net.Dial` equivalents, or raw TCP/UDP usage.

### URL Openers & File Includes
**None found.** No `File.ReadAllText` with URLs, no dynamic `Assembly.Load`, no remote file inclusion.

### Redirect & "Next URL" Handlers
All `NavigationManager.NavigateTo()` calls use hardcoded route templates with integer parameters from the database:
- `Sources/Application/Presentation/Shell/Errors/NotFound/NotFoundMiddleware.cs` (line 13): Redirects to hardcoded `NotFoundPage.Path`
- `Sources/Application/Presentation/Shell/Errors/Exceptions/ErrorInfo.razor.cs` (line 19): Navigates to hardcoded `HomePage.Path`
- `Sources/Application/Presentation/Areas/Coiffeur/Overview/CoiffeurOverviewList.razor.cs` (lines 56, 66): Navigates to `RunningGamePage.Path` with integer ID
- `Sources/Application/Presentation/Areas/Spieler/SpielerOverviewList.razor.cs` (line 47): Navigates to `SpielerEditPage.Path` with integer ID
- `Sources/Application/Presentation/Areas/Spieler/SpielerEditPage.razor.cs` (lines 57, 78): Navigates to hardcoded `SpielerOverviewPage.Path`

**No open redirect vulnerabilities.** All navigation targets are derived from hardcoded route constants.

### Headless Browsers & Render Engines
**None found.** No Puppeteer, Playwright, Selenium, or PDF generation.

### Media Processors
**None found.** No ImageMagick, FFmpeg, or image processing.

### Link Preview & Unfurlers
**None found.** No URL preview generation.

### Webhook Testers & Callback Verifiers
**None found.** No webhook endpoints or callback verification.

### SSO/OIDC Discovery & JWKS Fetchers
The OIDC middleware (`Microsoft.Identity.Web`) makes outbound calls to Azure AD for metadata discovery and JWKS retrieval. Configuration is entirely from static `appsettings.json` values — the tenant ID, client ID, and authority URL cannot be influenced by user input.
- `Sources/Application/Presentation/Shell/Initialization/ServiceInitialization.cs` (lines 40-41)
- `Sources/Application/appsettings.json` (lines 2-9)

### Importers & Data Loaders
**None found.** No "import from URL" functionality.

### Package/Plugin/Theme Installers
**None found.** No dynamic package or plugin installation.

### Monitoring & Health Check Frameworks
**None found.** No URL pingers or health check probes from the application side.

### Cloud Metadata Helpers
**None found.** No cloud metadata endpoint calls.

### Outbound Connection Summary

| Mechanism | Location | User-Controlled? | Severity |
|---|---|---|---|
| Azure AD OIDC outbound | `ServiceInitialization.cs:40-41` | No (static config) | Low |
| Application Insights telemetry | `ServiceInitialization.cs:38` | No (static config, placeholder) | Informational |
| Blazor SignalR circuit | `ServiceInitialization.cs:21` | No (framework-managed) | Informational |
| SQL Server connection | `AppDbContextFactory.cs:14` | No (static config) | Low |
| Browser SpeechSynthesis | `VoiceSupport.razor.cs` | Client-side only | Informational |
