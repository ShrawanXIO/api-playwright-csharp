# Roadmap — api-playwright-csharp

## Overview

A Playwright + Reqnroll + NUnit BDD framework in C# for **API** test automation, targeting the [DummyJSON](https://dummyjson.com) practice API. Companion project to `bdd-playwright-csharp` (SauceDemoBDD), which covers the UI side. Together they form a "Mastering Playwright + BDD" pair — Web and API.

## Tech Stack

| Layer | Tool |
| --- | --- |
| Language | C# / .NET 10 |
| HTTP Client | Playwright `APIRequestContext` |
| BDD | Reqnroll |
| Test Runner | NUnit |
| Target API | DummyJSON (`https://dummyjson.com`) |
| CI/CD | GitHub Actions |
| Mocking (stretch) | WireMock.NET |

## Milestones

### Phase 1 — Foundation

- [x] Solution + NUnit project scaffolding
- [x] Install Playwright, Reqnroll, NUnit packages
- [x] Raw `APIRequestContext` smoke test against DummyJSON (no BDD yet) — prove connectivity

### Phase 2 — Programmatic Core

- [x] `ApiClient` wrapper around `APIRequestContext` (base URL, default headers, request/response helpers)
- [x] `AuthService` — login, capture JWT + refresh token
- [x] `ProductsService` — CRUD wrapper methods
- [x] Request/response models (POCOs) for auth + products
- [x] Post-Phase 2 hardening: `BaseApiTest` shared base class (Template Method pattern) and a configuration-driven settings layer (`appsettings.json` + `ConfigLoader`) — no hardcoded URLs or credentials remain in any test class
- [x] Extended to a second API (JSONPlaceholder, no authentication) to prove the shared infrastructure is genuinely API-agnostic, then reorganized the whole project into vertical slices — each API self-contained under its own folder (`DummyJson/`, `JsonPlaceholder/`), with only `Core/` (ApiClient, ApiSettings, ConfigLoader, BaseApiTest) shared across all of them

### Phase 3 — Auth & Token Expiry

- [ ] Login with `expiresInMins=30`, decode the JWT, assert the `exp` claim is ~30 minutes out
- [ ] Negative case: expired/invalid token → 401 handling
- [ ] Refresh token flow

### Phase 4 — BDD Layer (Reqnroll)

- [ ] Feature file: Authentication
- [ ] Feature file: Product CRUD (Scenario Outline for create/read/update/delete)
- [ ] Step definitions call into the service layer only — no raw HTTP inside steps
- [ ] Tags: `@smoke` / `@regression` (same convention as bdd-playwright-csharp)

### Phase 5 — Execution & Reporting

- [ ] Parallel execution config (NUnit `ParallelScope`) — can likely go higher than the UI project's level of 3, since there's no browser contention
- [ ] HTML reporting via the Reqnroll formatter (same as the UI project)

### Phase 6 — CI/CD

- [ ] GitHub Actions workflow — runs on push to `main` + manual `workflow_dispatch`
- [ ] Env-var overridable settings (same pattern as bdd-playwright-csharp)
- [ ] Upload HTML report as a build artifact

### Phase 7 — Stretch: Deterministic Mocking

- [ ] Introduce WireMock.NET for a controlled, fake auth endpoint
- [ ] Simulate exact token-expiry timing without waiting real minutes
- [ ] Document real-API vs. mocked-API testing trade-offs in the README

## Concept Reference

A running reference of the underlying concepts this framework is built to demonstrate, updated alongside each phase as it lands.

1. **HTTP fundamentals** — Methods and idempotency, status code categories, headers (`Content-Type`, `Authorization`). The foundation everything else in this project sits on.
2. **Playwright's `APIRequestContext` model** — Context creation (`NewContextAsync`), `BaseURL`, default headers, and the response shape (`IAPIResponse`). Shares cookies and storage state the same way a browser context does, which is why it pairs naturally with a Playwright-based UI framework. *(Phase 1)*
3. **Authentication & token lifecycle** — Login and token capture via `AuthService`; expiry handling still ahead. *(Phase 2 done, Phase 3 in progress)*
4. **Framework architecture — separation of concerns** — Thin HTTP client wrapper (`ApiClient`) → service/endpoint classes (`AuthService`, `ProductsService`) → test layer, so each concern has exactly one place to change. Extended with `BaseApiTest`, a shared abstract base class (Template Method pattern) that owns connection setup/teardown for every test class, and a configuration layer (`appsettings.json` + `ConfigLoader`) so base URLs and credentials are never hardcoded. The project is organized as **vertical slices** — each API (`DummyJson/`, `JsonPlaceholder/`) is self-contained with its own models, services, and tests, while only truly shared infrastructure lives in `Core/`. *(Phase 2 done, Phase 4 pending)*
5. **BDD & Gherkin — when it earns its place** — Scenario Outlines for data-driven coverage; step definitions call into services rather than raw HTTP, so BDD adds clarity instead of ceremony. *(Phase 4)*
6. **Reqnroll mechanics** — Attribute-based step binding, `[BeforeScenario]`/`[AfterScenario]` hooks, and dependency injection sharing objects (like `ApiClient`) across step classes within a scenario. *(Phase 4)*
7. **Test execution model — isolation & parallelism** — No shared mutable state, correct `SetUp`/`TearDown` scoping. Currently proven with 7 independent tests (`SmokeTests`, `AuthTests`, `ProductsTests`) all passing; parallel execution itself lands in Phase 5. *(Phase 2 groundwork done, Phase 5 pending)*

## Non-Goals (for this project)

- No RestSharp — Playwright `APIRequestContext` only, to keep one HTTP client shared across the UI and API projects
- No production/company APIs — public practice API only (DummyJSON)

## Companion Project

UI counterpart: [`bdd-playwright-csharp`](https://github.com/ShrawanXIO/bdd-playwright-csharp) (SauceDemoBDD)
