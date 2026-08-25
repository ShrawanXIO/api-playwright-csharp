# api-playwright-csharp

**Status:** In progress (scaffolding stage)

A Playwright + Reqnroll + NUnit BDD framework in C# for **API** test automation, targeting the [DummyJSON](https://dummyjson.com) practice API. Companion project to [`bdd-playwright-csharp`](https://github.com/ShrawanXIO/bdd-playwright-csharp) (SauceDemoBDD), which covers the UI side.

## What this demonstrates

- API automation using Playwright's `APIRequestContext` — no browser involved
- BDD-style API tests with Reqnroll: Gherkin scenarios backed by a programmatic service-layer core
- Full auth lifecycle — login, JWT capture, token-expiry validation, refresh flow
- CRUD coverage against a REST API
- Parallel test execution
- CI/CD via GitHub Actions

## Tech Stack

| Layer | Tool |
| --- | --- |
| Language | C# / .NET 10 |
| HTTP Client | Playwright `APIRequestContext` |
| BDD | Reqnroll |
| Test Runner | NUnit |
| Target API | DummyJSON |
| CI/CD | GitHub Actions |

## Getting Started

### Prerequisites

- .NET 10 SDK
- Git

### Setup

```bash
git clone https://github.com/ShrawanXIO/api-playwright-csharp.git
cd api-playwright-csharp
dotnet restore
```

### Running tests

```bash
dotnet test
```

This section will be refined as the project takes shape.

## Project Structure

To be filled in as the project is scaffolded.

## Roadmap

See [ROADMAP.md](./ROADMAP.md) for the full build plan and current progress.

## Related

- UI companion project: [bdd-playwright-csharp](https://github.com/ShrawanXIO/bdd-playwright-csharp)
