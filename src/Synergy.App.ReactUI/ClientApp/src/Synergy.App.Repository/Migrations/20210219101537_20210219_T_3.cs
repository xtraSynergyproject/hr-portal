using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210219_T_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReferenceTypeCode",
                schema: "public",
                table: "NtsTask",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceTypeId",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferenceTypeCode",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "ReferenceTypeId",
                schema: "public",
                table: "NtsTask");
        }
    }
}
