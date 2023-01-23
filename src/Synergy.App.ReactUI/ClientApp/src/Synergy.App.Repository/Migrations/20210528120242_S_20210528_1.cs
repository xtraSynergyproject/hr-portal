using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210528_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OperationValue",
                schema: "log",
                table: "DecisionScriptComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OperationValue",
                schema: "public",
                table: "DecisionScriptComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OperationValue",
                schema: "public",
                table: "BusinessRuleNode",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OperationValue",
                schema: "public",
                table: "BreMasterTableMetadata",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OperationValue",
                schema: "log",
                table: "DecisionScriptComponentLog");

            migrationBuilder.DropColumn(
                name: "OperationValue",
                schema: "public",
                table: "DecisionScriptComponent");

            migrationBuilder.DropColumn(
                name: "OperationValue",
                schema: "public",
                table: "BusinessRuleNode");

            migrationBuilder.DropColumn(
                name: "OperationValue",
                schema: "public",
                table: "BreMasterTableMetadata");
        }
    }
}
