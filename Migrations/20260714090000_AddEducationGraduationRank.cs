using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace portfolio.Migrations
{
    [Migration("20260714090000_AddEducationGraduationRank")]
    public partial class AddEducationGraduationRank : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GraduationRank",
                table: "Educations",
                type: "character varying(240)",
                maxLength: 240,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GraduationRank",
                table: "Educations");
        }
    }
}
