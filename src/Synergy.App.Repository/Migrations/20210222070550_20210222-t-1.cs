using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210222t1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NtsType",
                schema: "public",
                table: "TaskTemplate",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StepTemplateIds",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "NtsType",
                schema: "public",
                table: "NtsTask",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToUserId",
                schema: "public",
                table: "Notification",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NtsType",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "StepTemplateIds",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "NtsType",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "ToUserId",
                schema: "public",
                table: "Notification");
        }
    }
}
