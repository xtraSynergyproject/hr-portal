using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class N_20220511_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableLegalEntityFilter",
                schema: "log",
                table: "FormTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableLegalEntityFilter",
                schema: "public",
                table: "FormTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableLegalEntityFilter",
                schema: "log",
                table: "FormTemplateLog");

            migrationBuilder.DropColumn(
                name: "EnableLegalEntityFilter",
                schema: "public",
                table: "FormTemplate");
        }
    }
}
