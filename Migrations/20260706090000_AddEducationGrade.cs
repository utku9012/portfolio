using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portfolio.Migrations
{
    [Migration("20260706090000_AddEducationGrade")]
    public partial class AddEducationGrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                ALTER TABLE "Educations"
                ADD COLUMN IF NOT EXISTS "Grade" character varying(40);
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                ALTER TABLE "Educations"
                DROP COLUMN IF EXISTS "Grade";
                """);
        }
    }
}
