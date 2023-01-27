using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210403_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "UserRoleStatusLabelCode",
                schema: "rec",
                newName: "UserRoleStatusLabelCode",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "UserRoleStageParent",
                schema: "rec",
                newName: "UserRoleStageParent",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "UserRoleStageChild",
                schema: "rec",
                newName: "UserRoleStageChild",
                newSchema: "public");

            migrationBuilder.AddColumn<bool>(
                name: "EnableRegularEmail",
                schema: "public",
                table: "User",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EnableSummaryEmail",
                schema: "public",
                table: "User",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InboxCode",
                schema: "public",
                table: "UserRoleStageParent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string[]>(
                name: "StatusCode",
                schema: "public",
                table: "UserRoleStageChild",
                type: "text[]",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnableRegularEmail",
                schema: "public",
                table: "User");

            migrationBuilder.DropColumn(
                name: "EnableSummaryEmail",
                schema: "public",
                table: "User");

            migrationBuilder.DropColumn(
                name: "InboxCode",
                schema: "public",
                table: "UserRoleStageParent");

            migrationBuilder.DropColumn(
                name: "StatusCode",
                schema: "public",
                table: "UserRoleStageChild");

            migrationBuilder.RenameTable(
                name: "UserRoleStatusLabelCode",
                schema: "public",
                newName: "UserRoleStatusLabelCode",
                newSchema: "rec");

            migrationBuilder.RenameTable(
                name: "UserRoleStageParent",
                schema: "public",
                newName: "UserRoleStageParent",
                newSchema: "rec");

            migrationBuilder.RenameTable(
                name: "UserRoleStageChild",
                schema: "public",
                newName: "UserRoleStageChild",
                newSchema: "rec");
        }
    }
}
