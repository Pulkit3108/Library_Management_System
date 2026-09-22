# Contributing

Keep changes focused and preserve the MVC application structure.

## Scope

- Place routes in `Controllers/`, persistence models and repositories in `Models/`, and Razor pages in the matching `Views/` directory.
- Keep database schema changes in Entity Framework migrations and validate them against a disposable local database.
- Do not commit credentials, connection strings containing passwords, local databases, build output, or IDE-specific files.
- Treat authentication, authorization, and framework upgrades as security-sensitive changes that need focused review and validation.

## Documentation

- Keep setup instructions aligned with the `.csproj`, `appsettings.json`, and tracked migration history.
- Distinguish a confirmed local run from a source-only description.
- Update screenshots only when they reflect the current interface.

## Validation

For documentation-only changes, run `git diff --check` and verify local Markdown links. For application changes, run the narrowest relevant build, migration, or manual-flow validation before opening a pull request.
