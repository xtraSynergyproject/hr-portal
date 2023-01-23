using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210504_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "HiringManagerOrganization",
                schema: "public",
                newName: "HiringManagerOrganization",
                newSchema: "rec");

            migrationBuilder.RenameTable(
                name: "HeadOfDepartmentOrganization",
                schema: "public",
                newName: "HeadOfDepartmentOrganization",
                newSchema: "rec");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "HiringManagerOrganization",
                schema: "rec",
                newName: "HiringManagerOrganization",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "HeadOfDepartmentOrganization",
                schema: "rec",
                newName: "HeadOfDepartmentOrganization",
                newSchema: "public");
        }
    }
}
