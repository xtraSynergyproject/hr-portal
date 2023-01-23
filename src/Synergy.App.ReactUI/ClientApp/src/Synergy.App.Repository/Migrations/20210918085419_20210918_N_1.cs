using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210918_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OtherAttachmentId",
                schema: "log",
                table: "TemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OtherAttachmentId",
                schema: "public",
                table: "Template",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherAttachmentId",
                schema: "log",
                table: "TemplateLog");

            migrationBuilder.DropColumn(
                name: "OtherAttachmentId",
                schema: "public",
                table: "Template");
        }
    }
}
