using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portfolio.Migrations
{
    [Migration("20260714090000_AddEducationGraduationRank")]
    public partial class AddEducationGraduationRank : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                ALTER TABLE "Educations"
                ADD COLUMN IF NOT EXISTS "GraduationRank" character varying(240);
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                ALTER TABLE "Educations"
                DROP COLUMN IF EXISTS "GraduationRank";
                """);
        }
    }
}
