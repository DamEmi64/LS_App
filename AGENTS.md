# Repository guide for agents

## Repository layout

- `V2.sln` is the main .NET solution. `V2.Server/Program.cs` starts the backend by calling `Connector.Main.InitializeConnector`.
- `Modules/Connector` is the composition root. `Connector.Modules` determines which modules are enabled for the normal server. `LocalConnector` is a separate local composition that also loads the older Files module.
- `Core/Base` contains shared module contracts, database and entity helpers, permissions, jobs, dictionaries, and module-to-module communication. `Core/SharedEvents` contains typed messages shared across modules. `Core/CommunicationBase` contains shared communication and template abstractions.
- `Modules/` contains backend features. Most use `Application`, `Domain`, and `Infrastructure` projects: Application owns module registration, controllers and API-facing behavior; Domain owns entities and contracts; Infrastructure owns persistence and external services. Check each module because a few use different project names or omit a layer.
- `v2.client` is the React, TypeScript, and Vite client. Its `AGENTS.md` has additional frontend-specific guidance. It reads backend APIs and some public Firebase Firestore content.
- `Tools/` contains standalone utilities, including the WPF `Translations` editor, the dictionary JSON generator, and scripts. These are separate from the server runtime.

## Backend modules

Normal server modules are registered in `Modules/Connector/Connector.cs`:

- `System`: users, authentication, roles, permissions, dictionaries, notifications, and process infrastructure.
- `Drive`: drive API integration.
- `RPG`: RPG sessions and related content, including Firebase synchronization.
- `Communication`: email templates and sending, plus Discord integration.
- `Automation`: automation definitions and execution.
- `Events`: events, invitations, reminders, and attendance.
- `FilesV2`: current file and folder management.
- `Files`: older file-management implementation. It is included by `LocalConnector`, not the normal `Connector`; check which host is being changed before editing it.

Modules implement `IModule` from `Core/Base`. Keep module-specific services and persistence within the module and use shared contracts such as `IConnect` for cross-module requests. Register new runtime modules through the appropriate connector rather than directly wiring feature services into `V2.Server`.

## Data and content flows

- Module database contexts and migrations live in Infrastructure projects. Shared database setup and configuration are in `Core/Base`; inspect `AppConfiguration` and `AddDatabase` before adding another configuration path.
- Public client dictionaries and translations have bundled JSON defaults under `v2.client/src/app` and `v2.client/public/locales`. The client can overlay content fetched from Firestore `clientContent` documents. The seed/download implementation and document mapping are in `v2.client/scripts/seed-firestore.mjs`; its service account is local configuration and must never be committed.
- `Tools/Translations` loads translation and dictionary JSON into editable tables and generates language JSON files. Keep its generated JSON shape aligned with the client files and Firestore document mapping.
- `Tools/DictionaryGenerator` is a separate console utility. Do not confuse it with the Translation editor when modifying translation workflows.

## Working in this repository

- Read the target project and its existing patterns before changing code. Prefer the existing module, API, database, localization, and UI abstractions over parallel implementations.
- Preserve the user's uncommitted changes. Inspect `git status` and the relevant diff before modifying files; do not reset, clean, or overwrite unrelated work.
- Treat `appsettings*.json`, Firebase service-account files, local database connection strings, and environment files as potentially sensitive. Do not expose or commit secrets. Prefer local overrides or environment variables.
- Generated API clients under `v2.client/src/shared/api/generated` are generated output; find and change the source schema/generation process when possible.
- Do not add packages or change module boundaries without a concrete need. Keep changes scoped to the relevant project.
- Do not run deployment, cloud writes, migrations, or external-service operations unless the task explicitly requires them. For local Firestore tooling, check the target project and credentials before running it.

## Useful commands

- Build the backend solution: `dotnet build V2.sln`
- Build the Translation editor: `dotnet build Tools/Translations/Translations.csproj`
- Client commands run from `v2.client`; inspect `package.json` for the current scripts. Common checks are `npm run typecheck`, `npm run build`, and `npm test`.
- Firestore content scripts run from `v2.client`. `firebase:seed-content` creates missing documents; `firebase:sync-content` updates existing documents. These commands make cloud changes and require valid local service-account credentials.

Run checks relevant to the change and report the exact commands and outcomes. Avoid broad cleanup or formatting commands that rewrite unrelated files.
