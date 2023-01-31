using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20220712_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamAssignmentType",
                schema: "log",
                table: "StepTaskAssigneeLogicLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TeamAssignmentType",
                schema: "public",
                table: "StepTaskAssigneeLogic",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamAssignmentType",
                schema: "log",
                table: "StepTaskAssigneeLogicLog");

            migrationBuilder.DropColumn(
                name: "TeamAssignmentType",
                schema: "public",
                table: "StepTaskAssigneeLogic");
        }
    }
}
