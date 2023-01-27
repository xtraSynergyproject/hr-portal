using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210817_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsClosed",
                schema: "log",
                table: "NotificationLog",
                newName: "IsAutoArchive");

            migrationBuilder.RenameColumn(
                name: "IsClosed",
                schema: "public",
                table: "Notification",
                newName: "IsAutoArchive");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAutoArchive",
                schema: "log",
                table: "NotificationLog",
                newName: "IsClosed");

            migrationBuilder.RenameColumn(
                name: "IsAutoArchive",
                schema: "public",
                table: "Notification",
                newName: "IsClosed");
        }
    }
}
