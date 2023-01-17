using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210917_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IconFileId",
                schema: "log",
                table: "TemplateCategoryLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "IconFileId",
                schema: "public",
                table: "TemplateCategory",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconFileId",
                schema: "log",
                table: "TemplateCategoryLog");

            migrationBuilder.DropColumn(
                name: "IconFileId",
                schema: "public",
                table: "TemplateCategory");
        }
    }
}
