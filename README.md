# Portfolio MVC

.NET Core MVC portfolio website backed by PostgreSQL. Includes sections for about, experiences, projects, education, skills, and certifications, plus a password-protected admin panel.

## Local Development

1. Create a PostgreSQL database.
2. Confirm local values in `appsettings.Development.json`.
3. Restore, migrate, and run:

```powershell
dotnet restore
dotnet ef database update
dotnet run --urls http://localhost:5130
```

## Pages

- Public portfolio: `http://localhost:5130`
- Admin panel: `http://localhost:5130/Admin`
- Robots: `http://localhost:5130/robots.txt`
- Sitemap: `http://localhost:5130/sitemap.xml`

## Architecture

- Runtime uploads under `wwwroot/uploads` are ignored by Git.
- Use persistent storage or object storage for uploaded profile images, project images, and CV files on cloud hosts.
- Admin login is rate-limited to 5 attempts per IP every 5 minutes.
- Entity models represent the PostgreSQL-backed content.
- EF Core migrations manage database schema changes.

