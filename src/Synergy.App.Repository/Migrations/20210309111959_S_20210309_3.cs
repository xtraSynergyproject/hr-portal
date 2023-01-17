using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210309_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreateReturnType",
                schema: "public",
                table: "ServiceTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EditReturnType",
                schema: "public",
                table: "ServiceTemplate",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateReturnType",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "EditReturnType",
                schema: "public",
                table: "ServiceTemplate");
        }
    }
}
