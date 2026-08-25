# Roadmap — api-playwright-csharp

## Overview

A Playwright + Reqnroll + NUnit BDD framework in C# for **API** test automation, targeting the [DummyJSON](https://dummyjson.com) practice API. Companion project to `bdd-playwright-csharp` (SauceDemoBDD), which covers the UI side. Together they form a "Mastering Playwright + BDD" pair — Web and API.

## Tech Stack

| Layer | Tool |
|---|---|
| Language | C# / .NET 10 |
| HTTP Client | Playwright `APIRequestContext` |
| BDD | Reqnroll |
| Test Runner | NUnit |
| Target API | DummyJSON (`https://dummyjson.com`) |
| CI/CD | GitHub Actions |
| Mocking (stretch) | WireMock.NET |

## Milestones

### Phase 1 — Foundation

- [ ] Solution + NUnit project scaffolding
- [ ] Install Playwright, Reqnroll, NUnit packages
- [ ] Raw `APIRequestContext` smoke test against DummyJSON (no BDD yet) — prove connectivity

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

## Non-Goals (for this project)

- No RestSharp — Playwright `APIRequestContext` only, to keep one HTTP client shared across the UI and API projects

- No production/company APIs — public practice API only (DummyJSON)

## Companion Project

UI counterpart: [`bdd-playwright-csharp`](https://github.com/ShrawanXIO/bdd-playwright-csharp) (SauceDemoBDD)
