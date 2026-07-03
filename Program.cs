using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using System.Threading.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using portfolio.Data;
using portfolio.Repositories;
using portfolio.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = NormalizePostgresConnectionString(
    builder.Configuration.GetConnectionString("DefaultConnection"));
var adminPassword = builder.Configuration["Admin:Password"];

if (!builder.Environment.IsDevelopment())
{
    if (string.IsNullOrWhiteSpace(connectionString)
        || connectionString.Contains("localhost", StringComparison.OrdinalIgnoreCase)
        || connectionString.Contains("Password=1234", StringComparison.OrdinalIgnoreCase))
    {
        throw new InvalidOperationException(
            "Set a production PostgreSQL connection string with ConnectionStrings__DefaultConnection.");
    }

    if (string.IsNullOrWhiteSpace(adminPassword) || adminPassword == "1234")
    {
        throw new InvalidOperationException(
            "Set a strong production admin password with Admin__Password.");
    }
}

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IPortfolioService, PortfolioService>();
builder.Services.AddScoped<IAdminContentService, AdminContentService>();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Admin/Login";
        options.LogoutPath = "/Admin/Logout";
        options.AccessDeniedPath = "/Admin/Login";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
        options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
            ? CookieSecurePolicy.SameAsRequest
            : CookieSecurePolicy.Always;
    });
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.ContentType = "text/plain";
        await context.HttpContext.Response.WriteAsync(
            "Too many login attempts. Please wait a few minutes and try again.",
            cancellationToken);
    };

    options.AddPolicy("admin-login", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(5),
                QueueLimit = 0,
                AutoReplenishment = true
            }));
});

var app = builder.Build();

app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithReExecute("/Home/StatusCodePage", "?code={0}");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    context.Response.Headers.TryAdd("X-Content-Type-Options", "nosniff");
    context.Response.Headers.TryAdd("X-Frame-Options", "DENY");
    context.Response.Headers.TryAdd("Referrer-Policy", "strict-origin-when-cross-origin");
    context.Response.Headers.TryAdd("Permissions-Policy", "camera=(), microphone=(), geolocation=()");

    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await DbInitializer.InitializeAsync(app.Services);

app.Run();

static string? NormalizePostgresConnectionString(string? connectionString)
{
    if (string.IsNullOrWhiteSpace(connectionString)
        || (!connectionString.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase)
            && !connectionString.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase)))
    {
        return connectionString;
    }

    var databaseUri = new Uri(connectionString);
    var userInfo = databaseUri.UserInfo.Split(':', 2);
    var builder = new NpgsqlConnectionStringBuilder
    {
        Host = databaseUri.Host,
        Port = databaseUri.Port > 0 ? databaseUri.Port : 5432,
        Database = Uri.UnescapeDataString(databaseUri.AbsolutePath.TrimStart('/')),
        Username = userInfo.Length > 0 ? Uri.UnescapeDataString(userInfo[0]) : string.Empty,
        Password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : string.Empty
    };

    foreach (var (key, value) in ParseQueryString(databaseUri.Query))
    {
        if (key.Equals("sslmode", StringComparison.OrdinalIgnoreCase)
            && Enum.TryParse<SslMode>(value, ignoreCase: true, out var sslMode))
        {
            builder.SslMode = sslMode;
        }
        else if (key.Equals("channel_binding", StringComparison.OrdinalIgnoreCase)
                 || key.Equals("channelbinding", StringComparison.OrdinalIgnoreCase))
        {
            builder["Channel Binding"] = value;
        }
    }

    return builder.ConnectionString;
}

static IEnumerable<(string Key, string Value)> ParseQueryString(string query)
{
    if (string.IsNullOrWhiteSpace(query))
    {
        yield break;
    }

    foreach (var pair in query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries))
    {
        var parts = pair.Split('=', 2);
        var key = Uri.UnescapeDataString(parts[0].Replace("+", " "));
        var value = parts.Length > 1 ? Uri.UnescapeDataString(parts[1].Replace("+", " ")) : string.Empty;

        if (!string.IsNullOrWhiteSpace(key))
        {
            yield return (key, value);
        }
    }
}
