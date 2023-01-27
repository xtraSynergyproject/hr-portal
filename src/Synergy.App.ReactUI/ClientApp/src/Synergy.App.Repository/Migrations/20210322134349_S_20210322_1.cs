using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210322_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceId",
                schema: "public",
                table: "Component");

            migrationBuilder.RenameColumn(
                name: "TargetId",
                schema: "public",
                table: "Component",
                newName: "ParentId");

            migrationBuilder.AddColumn<string>(
                name: "ProcessDesignHtml",
                schema: "public",
                table: "ProcessDesign",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessDesignHtml",
                schema: "public",
                table: "ProcessDesign");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                schema: "public",
                table: "Component",
                newName: "TargetId");

            migrationBuilder.AddColumn<string>(
                name: "SourceId",
                schema: "public",
                table: "Component",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }
    }
}
