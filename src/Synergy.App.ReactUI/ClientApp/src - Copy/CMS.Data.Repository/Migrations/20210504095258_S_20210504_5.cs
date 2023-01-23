using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210504_5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AllowOldStartDate",
                schema: "log",
                table: "TaskTemplateLog",
                newName: "AllowPastStartDate");

            migrationBuilder.RenameColumn(
                name: "AllowOldStartDate",
                schema: "public",
                table: "TaskTemplate",
                newName: "AllowPastStartDate");

            migrationBuilder.RenameColumn(
                name: "AllowOldStartDate",
                schema: "log",
                table: "ServiceTemplateLog",
                newName: "AllowPastStartDate");

            migrationBuilder.RenameColumn(
                name: "AllowOldStartDate",
                schema: "public",
                table: "ServiceTemplate",
                newName: "AllowPastStartDate");

            migrationBuilder.RenameColumn(
                name: "AllowOldStartDate",
                schema: "log",
                table: "NoteTemplateLog",
                newName: "AllowPastStartDate");

            migrationBuilder.RenameColumn(
                name: "AllowOldStartDate",
                schema: "public",
                table: "NoteTemplate",
                newName: "AllowPastStartDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AllowPastStartDate",
                schema: "log",
                table: "TaskTemplateLog",
                newName: "AllowOldStartDate");

            migrationBuilder.RenameColumn(
                name: "AllowPastStartDate",
                schema: "public",
                table: "TaskTemplate",
                newName: "AllowOldStartDate");

            migrationBuilder.RenameColumn(
                name: "AllowPastStartDate",
                schema: "log",
                table: "ServiceTemplateLog",
                newName: "AllowOldStartDate");

            migrationBuilder.RenameColumn(
                name: "AllowPastStartDate",
                schema: "public",
                table: "ServiceTemplate",
                newName: "AllowOldStartDate");

            migrationBuilder.RenameColumn(
                name: "AllowPastStartDate",
                schema: "log",
                table: "NoteTemplateLog",
                newName: "AllowOldStartDate");

            migrationBuilder.RenameColumn(
                name: "AllowPastStartDate",
                schema: "public",
                table: "NoteTemplate",
                newName: "AllowOldStartDate");
        }
    }
}
