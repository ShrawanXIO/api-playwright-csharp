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

- [ ] `ApiClient` wrapper around `APIRequestContext` (base URL, default headers, request/response helpers)
- [ ] `AuthService` — login, capture JWT + refresh token
- [ ] `ProductsService` — CRUD wrapper methods
- [ ] Request/response models (POCOs) for auth + products

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
3. **Authentication & token lifecycle** — Where the token is stored between requests, how it's attached, and how expiry is handled. *(Phase 3)*
4. **Framework architecture — separation of concerns** — Thin HTTP client wrapper → service/endpoint classes → test/step-definition layer, so each concern has exactly one place to change. *(Phase 2, Phase 4)*
5. **BDD & Gherkin — when it earns its place** — Scenario Outlines for data-driven coverage; step definitions call into services rather than raw HTTP, so BDD adds clarity instead of ceremony. *(Phase 4)*
6. **Reqnroll mechanics** — Attribute-based step binding, `[BeforeScenario]`/`[AfterScenario]` hooks, and dependency injection sharing objects (like `ApiClient`) across step classes within a scenario. *(Phase 4)*
7. **Test execution model — isolation & parallelism** — No shared mutable state, correct `SetUp`/`TearDown` scoping, with parallel execution as a natural consequence of that design rather than just an attribute. *(Phase 5)*

## Non-Goals (for this project)

- No RestSharp — Playwright `APIRequestContext` only, to keep one HTTP client shared across the UI and API projects
- No production/company APIs — public practice API only (DummyJSON)

## Companion Project

UI counterpart: [`bdd-playwright-csharp`](https://github.com/ShrawanXIO/bdd-playwright-csharp) (SauceDemoBDD)
