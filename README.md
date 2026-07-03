# Portfolio MVC

ASP.NET Core MVC portfolio website backed by PostgreSQL. It includes public sections for about, experiences, projects, education, skills, and certifications, plus a password-protected admin panel.

## Local Development

1. Create a PostgreSQL database named `portfolio_db`.
2. Confirm local values in `appsettings.Development.json`.
3. Restore, migrate, and run:

```powershell
dotnet restore
dotnet ef database update
dotnet run --urls http://localhost:5130
```

## Production Configuration

Do not put production secrets in `appsettings.json`. Set these environment variables on the host:

```txt
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=Host=YOUR_HOST;Port=5432;Database=YOUR_DB;Username=YOUR_USER;Password=YOUR_PASSWORD;SSL Mode=Require;Trust Server Certificate=true
Admin__Password=YOUR_STRONG_ADMIN_PASSWORD
```

The app refuses to start in production if the connection string is empty/local or if the admin password is missing/`1234`.

## Deployment

Publish with:

```powershell
dotnet publish -c Release -o publish
```

Apply migrations before or during deployment:

```powershell
dotnet ef database update
```

The app also calls `Database.MigrateAsync()` on startup, so pending migrations are applied automatically when the configured database is reachable.

### Free Render + Neon Deployment

1. Create a free PostgreSQL database on Neon.
2. Copy the Neon connection string and convert it to Npgsql format if needed:

```txt
Host=YOUR_HOST;Port=5432;Database=YOUR_DB;Username=YOUR_USER;Password=YOUR_PASSWORD;SSL Mode=Require;Trust Server Certificate=true
```

3. Create a Render web service from this GitHub repository.
4. Select Docker deployment. Render can also detect `render.yaml`.
5. Add these Render environment variables:

```txt
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=YOUR_NEON_CONNECTION_STRING
Admin__Password=YOUR_STRONG_ADMIN_PASSWORD
```

6. Deploy. The app will apply EF Core migrations automatically on startup.

The Docker entrypoint uses Render's `PORT` environment variable automatically, so you do not need to set `ASPNETCORE_URLS` manually.

## Pages

- Public portfolio: `http://localhost:5130`
- Admin panel: `http://localhost:5130/Admin`
- Robots: `http://localhost:5130/robots.txt`
- Sitemap: `http://localhost:5130/sitemap.xml`

## Production Notes

- Runtime uploads under `wwwroot/uploads` are ignored by Git.
- Use persistent storage or object storage for uploaded profile images, project images, and CV files on cloud hosts.
- Admin login is rate-limited to 5 attempts per IP every 5 minutes.
- Basic security headers and production-only secure cookies are enabled.

## Architecture

- Controllers stay thin and delegate work to services.
- Services coordinate portfolio/admin use cases.
- Repository abstractions isolate EF Core persistence.
- Entity models represent the PostgreSQL-backed content.
- EF Core migrations manage database schema changes.
