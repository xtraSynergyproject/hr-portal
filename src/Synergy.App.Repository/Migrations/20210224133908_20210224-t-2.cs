using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210224t2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableServiceDetails",
                schema: "public",
                table: "TaskTemplate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableServiceDetails",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: true);
        }
    }
}
