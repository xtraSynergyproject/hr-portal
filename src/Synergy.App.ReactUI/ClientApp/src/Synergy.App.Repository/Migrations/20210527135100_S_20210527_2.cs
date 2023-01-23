using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210527_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusinessRuleLogicType",
                schema: "log",
                table: "DecisionScriptComponentLog",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BusinessRuleLogicType",
                schema: "public",
                table: "DecisionScriptComponent",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BusinessRuleLogicType",
                schema: "public",
                table: "BusinessRuleNode",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Script",
                schema: "public",
                table: "BusinessRuleNode",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessRuleLogicType",
                schema: "log",
                table: "DecisionScriptComponentLog");

            migrationBuilder.DropColumn(
                name: "BusinessRuleLogicType",
                schema: "public",
                table: "DecisionScriptComponent");

            migrationBuilder.DropColumn(
                name: "BusinessRuleLogicType",
                schema: "public",
                table: "BusinessRuleNode");

            migrationBuilder.DropColumn(
                name: "Script",
                schema: "public",
                table: "BusinessRuleNode");
        }
    }
}
