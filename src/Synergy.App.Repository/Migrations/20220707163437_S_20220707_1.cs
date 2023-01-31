using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20220707_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnableFifo",
                schema: "log",
                table: "StepTaskComponentLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TeamAssignmentType",
                schema: "log",
                table: "StepTaskComponentLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "EnableFifo",
                schema: "public",
                table: "StepTaskComponent",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TeamAssignmentType",
                schema: "public",
                table: "StepTaskComponent",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "EnableFifo",
                schema: "log",
                table: "NtsTaskLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableFifo",
                schema: "public",
                table: "NtsTask",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableFifo",
                schema: "log",
                table: "NtsServiceLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableFifo",
                schema: "public",
                table: "NtsService",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DisableForeignKey",
                schema: "log",
                table: "ColumnMetadataLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DontCreateTableColumn",
                schema: "log",
                table: "ColumnMetadataLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DisableForeignKey",
                schema: "public",
                table: "ColumnMetadata",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DontCreateTableColumn",
                schema: "public",
                table: "ColumnMetadata",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableFifo",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "TeamAssignmentType",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "EnableFifo",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "TeamAssignmentType",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "EnableFifo",
                schema: "log",
                table: "NtsTaskLog");

            migrationBuilder.DropColumn(
                name: "EnableFifo",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "EnableFifo",
                schema: "log",
                table: "NtsServiceLog");

            migrationBuilder.DropColumn(
                name: "EnableFifo",
                schema: "public",
                table: "NtsService");

            migrationBuilder.DropColumn(
                name: "DisableForeignKey",
                schema: "log",
                table: "ColumnMetadataLog");

            migrationBuilder.DropColumn(
                name: "DontCreateTableColumn",
                schema: "log",
                table: "ColumnMetadataLog");

            migrationBuilder.DropColumn(
                name: "DisableForeignKey",
                schema: "public",
                table: "ColumnMetadata");

            migrationBuilder.DropColumn(
                name: "DontCreateTableColumn",
                schema: "public",
                table: "ColumnMetadata");
        }
    }
}
