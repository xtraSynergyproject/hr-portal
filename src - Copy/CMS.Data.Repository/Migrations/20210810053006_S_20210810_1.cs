using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210810_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                schema: "log",
                table: "NtsTaskLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                schema: "public",
                table: "NtsTask",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                schema: "log",
                table: "NtsServiceLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                schema: "public",
                table: "NtsService",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchived",
                schema: "log",
                table: "NtsTaskLog");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                schema: "log",
                table: "NtsServiceLog");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                schema: "public",
                table: "NtsService");
        }
    }
}
