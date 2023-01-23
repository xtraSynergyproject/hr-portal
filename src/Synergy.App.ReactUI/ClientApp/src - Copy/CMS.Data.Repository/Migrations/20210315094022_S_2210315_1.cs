using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_2210315_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ForeignKeyTableAliasName",
                schema: "public",
                table: "FormIndexPageColumn",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "IsForeignKeyTableColumn",
                schema: "public",
                table: "FormIndexPageColumn",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForeignKeyTableAliasName",
                schema: "public",
                table: "FormIndexPageColumn");

            migrationBuilder.DropColumn(
                name: "IsForeignKeyTableColumn",
                schema: "public",
                table: "FormIndexPageColumn");
        }
    }
}
