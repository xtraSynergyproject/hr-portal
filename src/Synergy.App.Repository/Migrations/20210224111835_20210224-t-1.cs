using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210224t1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableServiceDetails",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceDetailsHeight",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableServiceDetails",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "ServiceDetailsHeight",
                schema: "public",
                table: "TaskTemplate");
        }
    }
}
