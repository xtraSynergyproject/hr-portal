using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210324_T_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsIncludeEmailAttachment",
                schema: "public",
                table: "RecTaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIncludeAttachment",
                schema: "public",
                table: "Email",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceTemplateCode",
                schema: "public",
                table: "Email",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsIncludeEmailAttachment",
                schema: "public",
                table: "RecTaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsIncludeAttachment",
                schema: "public",
                table: "Email");

            migrationBuilder.DropColumn(
                name: "ReferenceTemplateCode",
                schema: "public",
                table: "Email");
        }
    }
}
