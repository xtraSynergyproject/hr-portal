using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210511_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Summary",
                schema: "public",
                table: "BreResult",
                newName: "MethodParamJson");

            migrationBuilder.RenameColumn(
                name: "Result",
                schema: "public",
                table: "BreResult",
                newName: "ReturnWithMessage");

            migrationBuilder.RenameColumn(
                name: "Detail",
                schema: "public",
                table: "BreResult",
                newName: "MethodNamespace");

            migrationBuilder.AddColumn<int>(
                name: "BreExecuteMethodType",
                schema: "public",
                table: "BreResult",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CustomMethodScript",
                schema: "public",
                table: "BreResult",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string[]>(
                name: "Message",
                schema: "public",
                table: "BreResult",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MethodName",
                schema: "public",
                table: "BreResult",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BreExecuteMethodType",
                schema: "public",
                table: "BreResult");

            migrationBuilder.DropColumn(
                name: "CustomMethodScript",
                schema: "public",
                table: "BreResult");

            migrationBuilder.DropColumn(
                name: "Message",
                schema: "public",
                table: "BreResult");

            migrationBuilder.DropColumn(
                name: "MethodName",
                schema: "public",
                table: "BreResult");

            migrationBuilder.RenameColumn(
                name: "ReturnWithMessage",
                schema: "public",
                table: "BreResult",
                newName: "Result");

            migrationBuilder.RenameColumn(
                name: "MethodParamJson",
                schema: "public",
                table: "BreResult",
                newName: "Summary");

            migrationBuilder.RenameColumn(
                name: "MethodNamespace",
                schema: "public",
                table: "BreResult",
                newName: "Detail");
        }
    }
}
