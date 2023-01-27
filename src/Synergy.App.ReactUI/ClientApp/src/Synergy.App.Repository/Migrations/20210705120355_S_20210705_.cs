using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210705_ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AutoApplyOnAllTemlates",
                schema: "log",
                table: "NotificationTemplateLog",
                newName: "AutoApplyOnAllTemplates");

            migrationBuilder.RenameColumn(
                name: "AutoApplyOnAllTemlates",
                schema: "public",
                table: "NotificationTemplate",
                newName: "AutoApplyOnAllTemplates");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AutoApplyOnAllTemplates",
                schema: "log",
                table: "NotificationTemplateLog",
                newName: "AutoApplyOnAllTemlates");

            migrationBuilder.RenameColumn(
                name: "AutoApplyOnAllTemplates",
                schema: "public",
                table: "NotificationTemplate",
                newName: "AutoApplyOnAllTemlates");
        }
    }
}
