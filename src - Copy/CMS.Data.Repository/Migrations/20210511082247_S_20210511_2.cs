using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210511_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MethodReturnValue",
                schema: "public",
                table: "BreResult",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ReturnIfMethodReturns",
                schema: "public",
                table: "BreResult",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MethodReturnValue",
                schema: "public",
                table: "BreResult");

            migrationBuilder.DropColumn(
                name: "ReturnIfMethodReturns",
                schema: "public",
                table: "BreResult");
        }
    }
}
