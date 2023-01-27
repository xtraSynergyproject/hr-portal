using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210328_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InboxStageId",
                schema: "rec",
                table: "UserRoleStageParent");

            migrationBuilder.DropColumn(
                name: "StatusLabelId",
                schema: "rec",
                table: "UserRoleStageChild");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InboxStageId",
                schema: "rec",
                table: "UserRoleStageParent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "StatusLabelId",
                schema: "rec",
                table: "UserRoleStageChild",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }
    }
}
