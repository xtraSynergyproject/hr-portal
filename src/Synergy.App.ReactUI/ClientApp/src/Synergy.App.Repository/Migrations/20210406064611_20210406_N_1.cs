using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210406_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NotificationSubject",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "NotificationSubject",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "Subject",
                schema: "public",
                table: "ServiceTemplate");
        }
    }
}
