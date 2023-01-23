using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20200216_S_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JobCategory",
                schema: "rec",
                table: "JobAdvertisement",
                newName: "JobCategoryId");

            migrationBuilder.RenameColumn(
                name: "Action",
                schema: "rec",
                table: "JobAdvertisement",
                newName: "ActionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JobCategoryId",
                schema: "rec",
                table: "JobAdvertisement",
                newName: "JobCategory");

            migrationBuilder.RenameColumn(
                name: "ActionId",
                schema: "rec",
                table: "JobAdvertisement",
                newName: "Action");
        }
    }
}
