# Ls App Project

**Ls App** is a modular application built with **ASP.NET Core** designed to be extensible through independent modules. It provides a backend API, optional frontend, and a local app that runs both. The app emphasizes modular architecture, background job processing, and clear separation of concerns.

---

## Features

* Modular architecture – extend functionality via independent modules.
* Core base project providing all necessary interfaces and classes required for modules.
* Core system module (`SystemModule`) implementing all required interfaces.
* Job system with background execution using **Hangfire**.
* Process and job management: support for parent-child job execution.
* Built-in modules for files, RPG, communication, and automation.
* Backend API ready for integration; optional React frontend.
* Centralized configuration and permission system.

---

## Technology

* **ASP.NET Core** – backend API and module hosting.
* **Entity Framework Core** – database context support via `AddDatabase`.
* **Hangfire** – background job processing.
* **React** – frontend interface (optional).
* PostgreSQL (optional) or default database provider.
* Firebase and email integration support.

---

## Architecture Overview

### Base Project

The base project is required for all modules. It provides:

* Core interfaces and classes necessary for module functionality.
* Helper methods for adding services and database context.
* `AddDatabase` method to register the database context (EF Core) with options for modified migration history and automatic migrations based on configuration.
* Supports `AutoMigrate` from configuration to run database migrations automatically.

---

### Modules

Modules are independent projects implementing the `IModule` interface and structured following clean architecture (app/domain/infrastructure). They are connected to the main app via a `Connector` instance. The `SystemModule` is mandatory.

---

### Jobs and Processes

Jobs implement `IJob` and are executed in background processes. Job context is provided via `IJobContext`. Summary of job context capabilities in JSON format:

```json
{
  "IJobContext": {
    "Id": "Job identifier",
    "JobId": "Hangfire job ID",
    "ServiceProvider": "Access to services",
    "Methods": [
      "AddLog(string) - log messages",
      "AddError(string) - record errors",
      "PassData<T>(T data) - share data between jobs",
      "GetData<T>() - retrieve shared data"
    ]
  }
}
```

* Supports logging, error handling, and data passing between jobs.
* Provides service provider access for dependency resolution.
* Jobs can be executed immediately or scheduled.
* Child jobs run after the parent completes successfully.

---

### Permissions

Modules define permissions. Core summary:

```json
{
  "Permissions": [
    {"Module":"rpg","Description":"Manage RPG sessions","Default":true},
    {"Module":"files","Description":"Manage files","Default":true},
    {"Module":"communication","Description":"Manage and send emails","Default":true},
    {"Module":"process","Description":"Manage background processes","Default":false},
    {"Module":"automation","Description":"Manage automation tasks","Default":false}
  ]
}
```

---

### Configuration

Configuration is mapped to `ConfigStructure`. Summary in JSON:

```json
{
  "ConfigStructure": {
    "AutoMigrate": "Automatically migrate database",
    "UsePostgresql": "Use PostgreSQL provider",
    "FirebaseOptions": "Firebase integration settings",
    "EmailOptions": "Email service settings"
  }
}
```

---

## Getting Started

### Releases

Three apps in each release:

1. Frontend – React app (optional)
2. Backend – ASP.NET Core API
3. Local – runs both frontend and backend locally

**Run local version:**

- Run Local.exe
- Endpoint:
  - Backend: https://localhost:5144
  - Frontend: http://localhost:8080


---

### Extending with Modules

1. Implement `IModule` interface.
2. Structure project (app/domain/infrastructure).
3. Add module to `Connector`.
4. Define operations and permissions.
5. Register database context using `AddDatabase` if needed.

---

### Notes

* Base Project is required for all modules.
* System Module is critical.
* Frontend is optional.
* Jobs and processes managed via `IJob` and `IJobContext`.
