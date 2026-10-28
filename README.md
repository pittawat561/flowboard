# FlowBoard

A real-time Kanban board for small teams, built as a full-stack portfolio project.

> 🚧 Work in progress. Phase 1 (project setup + authentication) is done.

## Tech stack

| Layer | Tech |
| --- | --- |
| Frontend | React 19, TypeScript, Vite, Tailwind CSS v4, TanStack Query, React Router |
| Backend | ASP.NET Core (.NET 10) Web API, Clean Architecture, EF Core |
| Database | PostgreSQL 17 |
| Auth | JWT bearer tokens, PBKDF2 password hashing (ASP.NET Core Identity hasher) |
| Testing | xUnit |

## Project structure

```
backend/
  src/
    FlowBoard.Domain/          Entities and domain rules (no dependencies)
    FlowBoard.Application/     Use cases, service interfaces, DTOs
    FlowBoard.Infrastructure/  EF Core, repositories, JWT, password hashing
    FlowBoard.Api/             Controllers, middleware, composition root
  tests/
    FlowBoard.UnitTests/
frontend/
  src/
    components/ui/      Reusable UI components
    components/layout/  App shell and route guards
    features/           Feature modules (auth, ...)
    services/           API client and service classes (composition root in index.ts)
    lib/                Utilities and storage helpers
    pages/              Route pages
docker-compose.yml      PostgreSQL for local development
```

## Getting started

Requirements: .NET 10 SDK, Node.js 22+, Docker.

```bash
# 1. Start PostgreSQL
docker compose up -d

# 2. Run the API (applies migrations automatically in Development)
cd backend
dotnet run --project src/FlowBoard.Api --launch-profile http
# API:     http://localhost:5143
# Swagger: http://localhost:5143/swagger

# 3. Run the frontend
cd frontend
npm install
npm run dev
# App: http://localhost:5173
```

Run the tests:

```bash
cd backend
dotnet test
```

## API

| Method | Route | Auth | Description |
| --- | --- | --- | --- |
| POST | `/api/auth/register` | – | Create an account and get a token |
| POST | `/api/auth/login` | – | Sign in and get a token |
| GET | `/api/auth/me` | Bearer | Current user |
| GET | `/health` | – | Health check |

Errors use RFC 7807 `ProblemDetails`.

## Roadmap

- [x] Phase 1: Solution setup, registration and login
- [ ] Phase 2: Boards, columns and cards with drag and drop
- [ ] Phase 3: Team members, roles and real-time updates with SignalR
- [ ] Phase 4: Integration tests, CI with GitHub Actions, Docker images, deployment
