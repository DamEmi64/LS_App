# LS App

LS App is a modular web application built around a backend-first architecture. The backend owns the main application structure: API modules, authentication, permissions, background jobs, notifications, configuration, and shared infrastructure. A React frontend is included as the user interface.

## 1. Project Idea

The application is designed as a set of independent modules connected by a shared backend foundation.

Each module owns one feature area and can provide:

- API endpoints
- services and repositories
- database context and migrations
- permissions
- background job operations
- startup behavior such as hubs, middleware, or scheduled work

This keeps features separated while still letting them work together through shared base contracts.

## 2. Architecture Overview

### 2.1 Connector

The connector is the backend composition root.

It decides which modules are enabled, loads them into the application, registers their services and controllers, and runs their startup logic. It also wires shared application behavior such as logging, error handling, Swagger, authentication, authorization, migrations, dictionaries, and role setup.

In short: the server starts the connector, and the connector starts the modules.

### 2.2 Base Project

`Core/Base` contains the common building blocks used by all modules.

It provides:

- module and connector contracts
- database registration helpers
- job and process contracts
- permission models
- common entities
- controller helpers
- shared configuration access

Modules should use these shared elements instead of reimplementing their own infrastructure.

### 2.3 Modules

Every backend feature is packaged as a module. A module implements `IModule`, which gives the connector a consistent way to load and run it.

The module contract includes:

- `Configure`: registers services and infrastructure
- `OnStartup`: adds runtime behavior
- `Name`: identifies the module
- `Version`: exposes the module version
- `Permissions`: declares access rules
- `Operations`: declares background job operations

Most modules use three layers:

- **Application**: controllers, DTOs, filters, module setup
- **Domain**: entities, dictionaries, repository contracts
- **Infrastructure**: database contexts, repositories, services, jobs, hubs, external integrations

## 3. Main Modules

### System

Core module required for the application to run. It provides users, authentication, roles, permissions, logs, dictionaries, process tracking, notifications, and job infrastructure.

### Files

Handles file management, including imports, exports, moving, copying, and deleting.

### RPG

Handles RPG session data, stories, chapters, heroes, places, summaries, imports/exports, and Firebase synchronization.

### Communication

Handles email templates, generated emails, template rules, and email sending.

### Automation

Handles automation definitions and notification-based task execution.

### Events

Handles event management, invitations, reminders, and participant sign-in/sign-out.

## 4. Core Backend Concepts

### 4.1 AddDatabase

`AddDatabase<T>()` is the shared helper used by modules to register their EF Core database context.

It keeps database setup consistent across modules and allows the application to switch database providers through configuration. When automatic migrations are enabled, module contexts can be discovered and migrated during backend startup.

### 4.2 AppConfiguration

`AppConfiguration` centralizes access to backend settings, connection strings, module versions, and permissions.

Modules use it to read their own options without needing to know how the root configuration is loaded.

### 4.3 Module Communication Client

The base project provides a communication client for synchronization between modules. In code this is exposed through `IConnect` and implemented by `ConnectClient`.

Modules use it to send typed requests to other modules without depending directly on their services or infrastructure. This keeps module boundaries cleaner while still allowing shared workflows such as:

- providing basic roles from module permissions
- requesting user data from the System module
- sending emails from another module
- triggering cross-module background work

Internally, the client uses the application's request pipeline, so communication stays typed and consistent across modules.

### 4.4 Permissions

Modules declare permissions as part of their module definition.

During startup, the System module uses those permissions to provide the basic role setup for the application. This allows every module to describe its own access rules while keeping role management centralized.

### 4.5 Operations

Operations describe background work that a module can perform.

Jobs reference operations so the process engine knows what work is being executed and which queue should handle it.

## 5. Jobs And Processes

Long-running work is handled as background processes.

A process can contain multiple jobs. Jobs can be chained, child jobs can run after parent jobs, and milestones can wait for specific work to complete. Job handlers can add logs, record errors, and pass data between jobs during execution.

This model is used for tasks such as:

- file operations
- email generation and sending
- RPG imports and summaries
- Firebase export
- event reminders
- event invitations

## 6. Configuration

Configuration is kept in the backend settings and should be overridden per environment. Sensitive or environment-specific values should be provided through local overrides, deployment configuration, or user secrets.

### 6.1 Config Section

The most important application-specific settings are grouped under the `config` section.

```json
{
  "config": {
    "autoMigrate": true,
    "usePostgresql": false,
    "frontendUrl": [
      "https://frontend-origin.example"
    ],
    "FirebaseOptions": {
      "ProjectId": "firebase-project-id"
    },
    "EmailOptions": {
      "SmtpServer": "string",
      "SmtpPort": 587,
      "PublicKey": "string",
      "PrivateKey": "string",
      "ApiEmail": "string"
    },
    "EventOptions": {
      "EventLinkTemplate": "https://frontend-origin.example/events#{0}"
    }
  }
}
```

### 6.2 Field Meaning

- `autoMigrate`: controls whether module database migrations run automatically on startup.
- `usePostgresql`: switches module database registration to PostgreSQL instead of the default provider.
- `frontendUrl`: allowed frontend origins for browser requests.
- `FirebaseOptions`: settings used by RPG Firebase synchronization.
- `EmailOptions`: settings used by the Communication module for email sending.
- `EventOptions`: settings used by the Events module when generating event links.

## 7. Running The Application

The backend can be run from source during development or as a packaged local executable from a release.

The release package also includes a local launcher that starts both:

- the backend executable
- the packaged frontend

The frontend is a React application that consumes the backend API. It is useful for the full app experience, but the backend modules are the main architectural unit of the project.

## 8. Extending The Backend

To add a feature, create a new module or extend an existing one.

Basic flow:

1. Implement the module contract.
2. Register services, repositories, database context, and other infrastructure.
3. Add controllers or startup behavior if needed.
4. Define permissions and background operations.
5. Add the module to the connector.

This keeps new features isolated while still making them available to the shared API, permission system, and background process engine.
