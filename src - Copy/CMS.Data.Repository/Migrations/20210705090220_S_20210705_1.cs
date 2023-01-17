using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210705_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AutoApplyOnAllTemlates",
                schema: "log",
                table: "NotificationTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AutoApplyOnAllTemlates",
                schema: "public",
                table: "NotificationTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoApplyOnAllTemlates",
                schema: "log",
                table: "NotificationTemplateLog");

            migrationBuilder.DropColumn(
                name: "AutoApplyOnAllTemlates",
                schema: "public",
                table: "NotificationTemplate");
        }
    }
}
