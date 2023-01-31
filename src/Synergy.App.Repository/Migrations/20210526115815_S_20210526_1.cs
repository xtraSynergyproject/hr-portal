using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210526_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DecisionScriptComponentId",
                schema: "public",
                table: "BusinessRuleModel",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "IsFromDecision",
                schema: "public",
                table: "BusinessRuleConnector",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DecisionScriptComponentId",
                schema: "public",
                table: "BusinessRuleModel");

            migrationBuilder.DropColumn(
                name: "IsFromDecision",
                schema: "public",
                table: "BusinessRuleConnector");
        }
    }
}
