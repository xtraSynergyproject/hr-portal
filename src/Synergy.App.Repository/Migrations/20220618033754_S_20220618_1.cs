using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20220618_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OpenSameTaskOnServiceSubmit",
                schema: "log",
                table: "StepTaskComponentLog",
                newName: "OpenSameTaskOnServiceReSubmit");

            migrationBuilder.AddColumn<bool>(
                name: "OpenSameTaskOnServiceReSubmit",
                schema: "public",
                table: "StepTaskComponent",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpenSameTaskOnServiceReSubmit",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.RenameColumn(
                name: "OpenSameTaskOnServiceReSubmit",
                schema: "log",
                table: "StepTaskComponentLog",
                newName: "OpenSameTaskOnServiceSubmit");
        }
    }
}
