using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210812_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FaceDetectionDescriptors",
                schema: "log",
                table: "UserLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "FaceDetectionDescriptors",
                schema: "public",
                table: "User",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FaceDetectionDescriptors",
                schema: "log",
                table: "UserLog");

            migrationBuilder.DropColumn(
                name: "FaceDetectionDescriptors",
                schema: "public",
                table: "User");
        }
    }
}
