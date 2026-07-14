using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portfolio.Migrations
{
    [Migration("20260705090000_StoreUploadsInDatabase")]
    public partial class StoreUploadsInDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DROP TABLE IF EXISTS "UploadedAssets";
                """);
        }
    }
}
