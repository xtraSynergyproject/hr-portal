using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210305_T_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TextBoxLink10",
                schema: "public",
                table: "RecTaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextBoxLink3",
                schema: "public",
                table: "RecTaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextBoxLink4",
                schema: "public",
                table: "RecTaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextBoxLink5",
                schema: "public",
                table: "RecTaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextBoxLink6",
                schema: "public",
                table: "RecTaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextBoxLink7",
                schema: "public",
                table: "RecTaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextBoxLink8",
                schema: "public",
                table: "RecTaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextBoxLink9",
                schema: "public",
                table: "RecTaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TextBoxLink10",
                schema: "public",
                table: "RecTaskTemplate");

            migrationBuilder.DropColumn(
                name: "TextBoxLink3",
                schema: "public",
                table: "RecTaskTemplate");

            migrationBuilder.DropColumn(
                name: "TextBoxLink4",
                schema: "public",
                table: "RecTaskTemplate");

            migrationBuilder.DropColumn(
                name: "TextBoxLink5",
                schema: "public",
                table: "RecTaskTemplate");

            migrationBuilder.DropColumn(
                name: "TextBoxLink6",
                schema: "public",
                table: "RecTaskTemplate");

            migrationBuilder.DropColumn(
                name: "TextBoxLink7",
                schema: "public",
                table: "RecTaskTemplate");

            migrationBuilder.DropColumn(
                name: "TextBoxLink8",
                schema: "public",
                table: "RecTaskTemplate");

            migrationBuilder.DropColumn(
                name: "TextBoxLink9",
                schema: "public",
                table: "RecTaskTemplate");
        }
    }
}
