.NET Core MVC portfolio website backed by PostgreSQL. Includes about, experiences, projects, education, skills, and certifications sections, plus a password-protected admin panel.

## Local Development

1. Create a PostgreSQL database.
2. Confirm local values in `appsettings.Development.json`.
3. Restore, migrate, and run:

```powershell
dotnet restore
dotnet ef database update
dotnet run --urls http://localhost:5130
```
