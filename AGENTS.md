# Repository agent guidance

## Module architecture

- Keep controllers and API-facing behavior in the module's Application project, domain contracts and repository interfaces in Domain, and database access and repository implementations in Infrastructure.
- Application controllers and services must depend on Domain repository interfaces, never directly on a `DbContext` or Infrastructure database implementation.
- Register each repository implementation with dependency injection in its module registration.
- Follow the existing module patterns and keep persistence queries, includes, and `SaveChanges` calls inside Infrastructure repositories.
