using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20200216_S_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobAdvertisementStatus",
                schema: "rec",
                table: "JobAdvertisement");

            migrationBuilder.RenameColumn(
                name: "ManpowerType",
                schema: "rec",
                table: "JobAdvertisement",
                newName: "ManpowerTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ManpowerTypeId",
                schema: "rec",
                table: "JobAdvertisement",
                newName: "ManpowerType");

            migrationBuilder.AddColumn<string>(
                name: "JobAdvertisementStatus",
                schema: "rec",
                table: "JobAdvertisement",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }
    }
}
