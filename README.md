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

```text
api-playwright-csharp/
├── ApiTests/
│   ├── Core/
│   │   └── ApiClient.cs            # Wraps Playwright's APIRequestContext — the only file that touches Playwright directly
│   ├── Models/
│   │   ├── AuthModels.cs           # LoginRequest / LoginResponse POCOs
│   │   └── ProductModels.cs        # Product, ProductListResponse, and CRUD request/response POCOs
│   ├── Services/
│   │   ├── AuthService.cs          # Login logic, built on ApiClient
│   │   └── ProductsService.cs      # CRUD methods, built on ApiClient
│   ├── SmokeTests.cs               # Connectivity check — one GET, one assertion
│   ├── AuthTests.cs                # Verifies login against real DummyJSON credentials
│   ├── ProductsTests.cs            # Verifies all five ProductsService methods
│   └── ApiTests.csproj
├── ApiPlaywrightCSharp.slnx        # Solution file (.slnx — .NET 10's new default format)
├── ROADMAP.md                      # Phase-by-phase build plan and current progress
└── README.md
```

## Architecture

Every call flows through the project in one direction, and each layer has exactly one job:

```text
Test  →  Service  →  ApiClient  →  Playwright  →  DummyJSON
```

- A **test** never calls `APIRequestContext` directly — it calls a service method and asserts on the result.
- A **service** (`AuthService`, `ProductsService`) owns the actual endpoint calls, request serialization, and response deserialization for one feature area. It receives an already-initialized `ApiClient` through its constructor rather than creating one itself — constructor-based dependency injection.
- **`ApiClient`** is the only class that knows Playwright exists. It owns creating and disposing the driver and the request context.

This is also why every test is independent: each one creates and disposes its own `ApiClient` in `[SetUp]`/`[TearDown]`, so no test can leave behind state that affects another — the property that will make parallel execution (Phase 5) safe to enable later.

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

Currently runs 7 tests — connectivity, authentication, and full product CRUD — all executing against the live DummyJSON API, not mocks or stubs.

## What This Project Demonstrates

- **API automation without a browser** — using Playwright's `APIRequestContext` for pure HTTP request/response testing
- **Layered architecture** — test → service → `ApiClient` → Playwright, one direction of dependency, one place to change per concern
- **Constructor-based dependency injection** — services receive an already-initialized `ApiClient` rather than constructing their own, keeping each service focused on its own endpoint logic
- **Typed request/response models (POCOs)** instead of raw dictionaries or JSON strings, with case-insensitive deserialization bridging the API's camelCase and C#'s PascalCase conventions
- **Real authentication flow** — genuine JWT access and refresh tokens captured from DummyJSON's `/auth/login`, not mocked
- **Full CRUD coverage** against a real REST API, including correctly modeling the API's non-obvious response shapes — the product list endpoint returns a wrapper object with pagination metadata rather than a bare array, and the delete endpoint returns a different shape than a normal read
- **Test isolation** — every test creates and disposes its own `ApiClient`, so no test depends on or affects another's state
- **Async-safe initialization** — an `InitializeAsync`/`DisposeAsync` pattern used throughout, since C# constructors can't be `async`

## Tests Covered

| File | Test | What it verifies |
| --- | --- | --- |
| SmokeTests.cs | `GetProducts_ReturnsOk` | Basic connectivity — a GET call to DummyJSON returns a 200 |
| AuthTests.cs | `Login_WithValidCredentials_ReturnsAccessToken` | Login returns a valid JWT access token and refresh token |
| ProductsTests.cs | `GetAllProducts_ReturnsProducts` | Product list endpoint returns data with correct pagination metadata |
| ProductsTests.cs | `GetProductById_ReturnsCorrectProduct` | Single-product GET returns the requested product |
| ProductsTests.cs | `CreateProduct_ReturnsNewProductWithId` | POST to `/products/add` returns a new product with a generated ID |
| ProductsTests.cs | `UpdateProduct_ReturnsUpdatedTitle` | PUT to `/products/{id}` returns the updated field values |
| ProductsTests.cs | `DeleteProduct_ReturnsIsDeletedTrue` | DELETE returns the `isDeleted` flag and a deletion timestamp |

## Current Status & Roadmap

Phases 1 and 2 are complete: the project is scaffolded, and the full programmatic core — `ApiClient`, both services, and every model — is built and verified against live API responses. Still ahead: token-expiry testing (Phase 3), the Reqnroll BDD layer on top of the existing services (Phase 4), parallel execution (Phase 5), CI/CD (Phase 6), and an optional WireMock.NET stretch goal (Phase 7).

See [ROADMAP.md](./ROADMAP.md) for the full phase-by-phase plan and progress.

## Related

- UI companion project: [bdd-playwright-csharp](https://github.com/ShrawanXIO/bdd-playwright-csharp)

---
*Built as a hands-on learning project — [Shrawan](https://github.com/ShrawanXIO)*
