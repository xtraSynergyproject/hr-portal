using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_2210316_7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ForeignKeyTableAliasName",
                schema: "public",
                table: "TaskIndexPageColumn",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "IsForeignKeyTableColumn",
                schema: "public",
                table: "TaskIndexPageColumn",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ForeignKeyTableAliasName",
                schema: "public",
                table: "ServiceIndexPageColumn",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "IsForeignKeyTableColumn",
                schema: "public",
                table: "ServiceIndexPageColumn",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ForeignKeyTableAliasName",
                schema: "public",
                table: "NoteIndexPageColumn",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "IsForeignKeyTableColumn",
                schema: "public",
                table: "NoteIndexPageColumn",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForeignKeyTableAliasName",
                schema: "public",
                table: "TaskIndexPageColumn");

            migrationBuilder.DropColumn(
                name: "IsForeignKeyTableColumn",
                schema: "public",
                table: "TaskIndexPageColumn");

            migrationBuilder.DropColumn(
                name: "ForeignKeyTableAliasName",
                schema: "public",
                table: "ServiceIndexPageColumn");

            migrationBuilder.DropColumn(
                name: "IsForeignKeyTableColumn",
                schema: "public",
                table: "ServiceIndexPageColumn");

            migrationBuilder.DropColumn(
                name: "ForeignKeyTableAliasName",
                schema: "public",
                table: "NoteIndexPageColumn");

            migrationBuilder.DropColumn(
                name: "IsForeignKeyTableColumn",
                schema: "public",
                table: "NoteIndexPageColumn");
        }
    }
}
