using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portfolio.Migrations
{
    [Migration("20260714093000_AddEducationGraduationRankTr")]
    public partial class AddEducationGraduationRankTr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                ALTER TABLE "Educations"
                ADD COLUMN IF NOT EXISTS "GraduationRankTr" character varying(240);
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                ALTER TABLE "Educations"
                DROP COLUMN IF EXISTS "GraduationRankTr";
                """);
        }
    }
}
