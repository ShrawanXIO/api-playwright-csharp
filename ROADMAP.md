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
| Target APIs | DummyJSON (`https://dummyjson.com`), JSONPlaceholder (`https://jsonplaceholder.typicode.com`) |
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
- [x] Added `BaseService` to remove duplicated `ApiClient` references and JSON options across all three services; added an optional `DefaultHeaders` hook on `BaseApiTest` for future APIs requiring auth headers; added `scaffold-api.ps1` to generate a new API's folder skeleton automatically

### Phase 3 — Auth & Token Expiry

- [x] Login with `expiresInMins=30`, decode the JWT (`JwtHelper`), assert the `exp` claim is ~30 minutes out
- [x] Negative case: invalid token against `/auth/me` → 401 handling
- [x] Refresh token flow — verified via the refreshed token's own expiry, not string comparison against the original (see note below)

### Phase 4 — BDD Layer (Reqnroll)

- [x] Feature file: Authentication (`Authentication.feature`) — login scenario tagged `@smoke`
- [x] Feature file: Product CRUD (`Products.feature`) — all five operations covered; `Scenario Outline` used specifically for Update, the one operation with a naturally varying piece (the title), rather than forced onto every scenario
- [x] Step definitions (`AuthSteps`, `ProductsSteps`) call into the existing service layer only — no raw HTTP inside any step
- [x] Tags: `@smoke` / `@regression` applied throughout, same convention as bdd-playwright-csharp

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
3. **Authentication & token lifecycle** — Full lifecycle now covered: login and token capture, JWT expiry decoding and verification (`JwtHelper`, handling Base64URL's `-`/`_`/no-padding differences from standard Base64), rejection of invalid tokens (401 from `/auth/me`), and refresh producing a token with a correctly-verified new expiry. One real lesson from building this: an early version of the refresh test asserted the new access token was a different *string* than the original — it failed intermittently, not from a bug, but because DummyJSON's JWTs are deterministic (same payload always produces the same signature) and a JWT's `iat` has only second-level precision, so a login and refresh landing in the same second produce byte-identical tokens. The fix was asserting what refresh actually guarantees — a correctly-expiring token — rather than an incidental detail. *(Phase 2 and 3 done)*
4. **Framework architecture — separation of concerns** — Thin HTTP client wrapper (`ApiClient`) → service/endpoint classes (`AuthService`, `ProductsService`) → test layer, so each concern has exactly one place to change. Extended with `BaseApiTest`, a shared abstract base class (Template Method pattern) that owns connection setup/teardown for every test class, and a configuration layer (`appsettings.json` + `ConfigLoader`) so base URLs and credentials are never hardcoded. `BaseService` applies the same Template Method idea one layer over, removing duplicated setup code across every service. The project is organized as **vertical slices** — each API (`DummyJson/`, `JsonPlaceholder/`) is self-contained with its own models, services, and tests, while only truly shared infrastructure lives in `Core/`, and a scaffolding script (`scaffold-api.ps1`) generates that structure for any new API automatically. *(Phase 2 done, Phase 4 pending)*
5. **BDD & Gherkin — when it earns its place** — `Scenario Outline` used specifically for Update (the one Product operation with a naturally varying piece), not forced onto every scenario; step definitions call into the existing service layer, not raw HTTP, so the BDD layer adds clarity on top of already-proven code rather than duplicating it. *(Phase 4 done)*
6. **Reqnroll mechanics** — Attribute-based step binding via Cucumber Expressions (`{string}`, `{int}`) for parameterized steps; `[BeforeScenario]`/`[AfterScenario]` hooks (`Hooks.cs`) apply project-wide, not duplicated per feature; and Reqnroll's own dependency injection shares the same `ApiClient` instance across `Hooks`, `AuthSteps`, and `ProductsSteps` within one scenario, chaining through `BaseService` automatically. *(Phase 4 done)*
7. **Test execution model — isolation & parallelism** — No shared mutable state, correct `SetUp`/`TearDown` scoping. Currently proven with 19 tests total — 12 plain NUnit tests plus 7 Reqnroll scenarios (Authentication and Product CRUD) — all passing; parallel execution itself lands in Phase 5. *(Phase 2 groundwork done, Phase 5 pending)*

## Non-Goals (for this project)

- No RestSharp — Playwright `APIRequestContext` only, to keep one HTTP client shared across the UI and API projects
- No production/company APIs — public practice APIs only (DummyJSON, JSONPlaceholder)

## Companion Project

UI counterpart: [`bdd-playwright-csharp`](https://github.com/ShrawanXIO/bdd-playwright-csharp) (SauceDemoBDD)
