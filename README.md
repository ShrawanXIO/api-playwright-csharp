# api-playwright-csharp

**Status:** In progress — Phase 2 complete (programmatic core built and verified against live DummyJSON responses)

An API test automation framework built with **Playwright**, **Reqnroll**, and **C#**, targeting the [DummyJSON](https://dummyjson.com) practice API — authentication and full product CRUD. Companion project to [`bdd-playwright-csharp`](https://github.com/ShrawanXIO/bdd-playwright-csharp) (SauceDemoBDD), which covers the UI side with the same stack. Built as a hands-on learning project to understand API testing architecture, request/response modeling, and dependency injection from first principles — no browser involved, no mocking, real HTTP calls against a real API.

## Tech Stack

- **C# (.NET 10)**
- **Playwright** — HTTP-only API testing via `APIRequestContext`; no browser is launched
- **System.Text.Json** — request/response serialization, with case-insensitive property matching to bridge the API's camelCase and C#'s PascalCase conventions
- **NUnit** — test runner
- **Reqnroll** — BDD framework (Gherkin syntax); the service layer is already built to support this, wiring it in is Phase 4
- **DummyJSON** — the public REST API used as the test target
- **GitHub Actions** — CI pipeline; planned for Phase 6

## Project Structure

The project is organized as **vertical slices** — each API gets its own self-contained folder holding everything specific to it (models, services, tests), rather than grouping all models together, all services together, and all tests together across every API. Only genuinely shared infrastructure — code that doesn't vary by which API is being tested — lives in `Core/`.

```text
api-playwright-csharp/
├── ApiTests/
│   ├── Core/
│   │   ├── ApiClient.cs            # Wraps Playwright's APIRequestContext — the only file that touches Playwright directly
│   │   ├── ApiSettings.cs          # Typed shape of appsettings.json
│   │   ├── ConfigLoader.cs         # Reads and deserializes appsettings.json into ApiSettings
│   │   ├── BaseApiTest.cs          # Shared connection setup/teardown — every API's test classes inherit from this
│   │   └── BaseService.cs          # Shared ApiClient reference and JSON options — every service inherits from this
│   ├── DummyJson/
│   │   ├── Models/                 # AuthModels.cs, ProductModels.cs
│   │   ├── Services/                # AuthService.cs, ProductsService.cs
│   │   └── Tests/                   # SmokeTests.cs, AuthTests.cs, ProductsTests.cs
│   ├── JsonPlaceholder/
│   │   ├── Models/                 # PostModels.cs
│   │   ├── Services/                # PostsService.cs
│   │   └── Tests/                   # PostsTests.cs
│   ├── appsettings.json            # Base URLs and credentials for every API — no hardcoded values anywhere in test code
│   ├── scaffold-api.ps1            # Generates the Models/Services/Tests skeleton for a new API
│   └── ApiTests.csproj
├── ApiPlaywrightCSharp.sln         # Solution file
├── ROADMAP.md                      # Phase-by-phase build plan and current progress
└── README.md
```

Adding a third API means running `scaffold-api.ps1` with the new API's name, adding its base URL to `appsettings.json`/`ApiSettings.cs`, and filling in the generated model/service/test skeletons — nothing in `Core/` needs to change to support it.

## Architecture

Every call flows through the project in one direction, and each layer has exactly one job:

```text
Test  →  Service  →  ApiClient  →  Playwright  →  DummyJSON
```

- A **test** never calls `APIRequestContext` directly — it calls a service method and asserts on the result.
- A **service** (`AuthService`, `ProductsService`) owns the actual endpoint calls, request serialization, and response deserialization for one feature area. It receives an already-initialized `ApiClient` through its constructor rather than creating one itself — constructor-based dependency injection.
- **`ApiClient`** is the only class that knows Playwright exists. It owns creating and disposing the driver and the request context.

This is also why every test is independent: each one creates and disposes its own `ApiClient` in `[SetUp]`/`[TearDown]`, so no test can leave behind state that affects another — the property that will make parallel execution (Phase 5) safe to enable later.

Every test class also inherits from **`BaseApiTest`**, an abstract base class that owns the one thing every test class needs regardless of which API it targets: creating and disposing the `ApiClient` connection. Each derived class supplies only its own `BaseUrl` — a compulsory, compiler-enforced override — and whatever service objects it specifically needs. This is the Template Method design pattern: the base class defines the fixed skeleton, derived classes fill in the one piece that varies.

Base URLs and credentials are no longer hardcoded anywhere either — they're loaded once from `appsettings.json` via `ConfigLoader`, and shared across every test class through a single field on `BaseApiTest`.

Services follow the identical idea: every service (`AuthService`, `ProductsService`, `PostsService`) inherits from **`BaseService`**, which holds the shared `ApiClient` reference and JSON deserialization options that were previously duplicated, word-for-word, in every service file. `BaseApiTest` also exposes an optional `DefaultHeaders` hook — a `virtual` property, not a required one — so a future API needing an `Authorization` header on every request can supply one, while APIs that need no auth at all (both current ones) are unaffected.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Git
- No browser install required — unlike a UI framework, `APIRequestContext` sends HTTP requests directly, so there's nothing for Playwright to launch or download

## Setup

```bash
git clone https://github.com/ShrawanXIO/api-playwright-csharp.git
cd api-playwright-csharp
dotnet restore
```

## Running the Tests

```bash
dotnet test
```

Currently runs 9 tests across two independent APIs — DummyJSON (connectivity, authentication, full product CRUD) and JSONPlaceholder (no authentication) — all executing against live APIs, not mocks or stubs.

## What This Project Demonstrates

- **API automation without a browser** — using Playwright's `APIRequestContext` for pure HTTP request/response testing
- **Layered architecture** — test → service → `ApiClient` → Playwright, one direction of dependency, one place to change per concern
- **Constructor-based dependency injection** — services receive an already-initialized `ApiClient` rather than constructing their own, keeping each service focused on its own endpoint logic
- **Typed request/response models (POCOs)** instead of raw dictionaries or JSON strings, with case-insensitive deserialization bridging the API's camelCase and C#'s PascalCase conventions
- **Real authentication flow** — genuine JWT access and refresh tokens captured from DummyJSON's `/auth/login`, not mocked
- **Full CRUD coverage** against a real REST API, including correctly modeling the API's non-obvious response shapes — the product list endpoint returns a wrapper object with pagination metadata rather than a bare array, and the delete endpoint returns a different shape than a normal read
- **Test isolation** — every test creates and disposes its own `ApiClient`, so no test depends on or affects another's state
- **Async-safe initialization** — an `InitializeAsync`/`DisposeAsync` pattern used throughout, since C# constructors can't be `async`
- **A shared base test class (Template Method pattern)** — `BaseApiTest` owns connection setup/teardown for every test class; each derived class only supplies its own base URL and whatever services it needs
- **Configuration-driven settings** — base URLs and credentials loaded from `appsettings.json` at runtime, not hardcoded into test or service code
- **Multi-API support, proven not just planned** — a second, unrelated API (JSONPlaceholder, no authentication at all) was added with zero changes to `Core/` or `BaseApiTest`, confirming the shared infrastructure genuinely is API-agnostic
- **Vertical-slice project organization** — each API is self-contained (its own models, services, and tests together), rather than every API's code scattered across shared layer folders
- **A shared service base class** — `BaseService` removes duplicated client-reference and JSON-options code that was previously repeated in every service
- **An optional, extensible auth hook** — `BaseApiTest.DefaultHeaders` lets a future API attach required headers (like a Bearer token) without affecting APIs that need none
- **Repeatable scaffolding** — a PowerShell script generates a new API's folder and starter files with the correct namespaces and base-class inheritance already in place

## Tests Covered

| API | File | Test | What it verifies |
| --- | --- | --- | --- |
| DummyJson | SmokeTests.cs | `GetProducts_ReturnsOk` | Basic connectivity — a GET call to DummyJSON returns a 200 |
| DummyJson | AuthTests.cs | `Login_WithValidCredentials_ReturnsAccessToken` | Login returns a valid JWT access token and refresh token |
| DummyJson | ProductsTests.cs | `GetAllProducts_ReturnsProducts` | Product list endpoint returns data with correct pagination metadata |
| DummyJson | ProductsTests.cs | `GetProductById_ReturnsCorrectProduct` | Single-product GET returns the requested product |
| DummyJson | ProductsTests.cs | `CreateProduct_ReturnsNewProductWithId` | POST to `/products/add` returns a new product with a generated ID |
| DummyJson | ProductsTests.cs | `UpdateProduct_ReturnsUpdatedTitle` | PUT to `/products/{id}` returns the updated field values |
| DummyJson | ProductsTests.cs | `DeleteProduct_ReturnsIsDeletedTrue` | DELETE returns the `isDeleted` flag and a deletion timestamp |
| JsonPlaceholder | PostsTests.cs | `GetAllPosts_ReturnsPosts` | Post list endpoint (no authentication) returns data |
| JsonPlaceholder | PostsTests.cs | `GetPostById_ReturnsCorrectPost` | Single-post GET returns the requested post |

## Current Status & Roadmap

Phases 1 and 2 are complete: the project is scaffolded, and the full programmatic core — `ApiClient`, both services, and every model — is built and verified against live API responses. Since then, the framework has also been hardened with a shared `BaseApiTest` base class and a configuration-driven settings layer, and extended to a second, independent API (JSONPlaceholder) organized as a vertical slice alongside DummyJSON — all covered in the Architecture and Project Structure sections above. Still ahead: token-expiry testing (Phase 3), the Reqnroll BDD layer on top of the existing services (Phase 4), parallel execution (Phase 5), CI/CD (Phase 6), and an optional WireMock.NET stretch goal (Phase 7).

See [ROADMAP.md](./ROADMAP.md) for the full phase-by-phase plan and progress.

## Related

- UI companion project: [bdd-playwright-csharp](https://github.com/ShrawanXIO/bdd-playwright-csharp)

---
*Built as a hands-on learning project — [Shrawan](https://github.com/ShrawanXIO)*
