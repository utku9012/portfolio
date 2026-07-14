using Microsoft.EntityFrameworkCore;

namespace portfolio.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.MigrateAsync();
        await context.Database.ExecuteSqlRawAsync(
            """
            CREATE TABLE IF NOT EXISTS "UploadedAssets" (
                "Id" uuid NOT NULL,
                "FileName" character varying(255) NOT NULL,
                "ContentType" character varying(120) NOT NULL,
                "Data" bytea NOT NULL,
                "CreatedAtUtc" timestamp with time zone NOT NULL,
                CONSTRAINT "PK_UploadedAssets" PRIMARY KEY ("Id")
            );
            """);
        await context.Database.ExecuteSqlRawAsync(
            """
            ALTER TABLE "Educations"
            ADD COLUMN IF NOT EXISTS "Grade" character varying(40);
            """);
        await context.Database.ExecuteSqlRawAsync(
            """
            ALTER TABLE "Educations"
            ADD COLUMN IF NOT EXISTS "GraduationRank" character varying(240);
            """);
    }
}
