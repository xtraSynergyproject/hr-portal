using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_2210315_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateForeignKeyReferenceColumn",
                schema: "public",
                table: "ColumnMetadata",
                newName: "HideForeignKeyTableColumns");

            migrationBuilder.AlterColumn<string[]>(
                name: "UserPermissionTypes",
                schema: "public",
                table: "Permission",
                type: "text[]",
                nullable: true,
                oldClrType: typeof(string[]),
                oldType: "text[]");

            migrationBuilder.AlterColumn<string[]>(
                name: "PageTypes",
                schema: "public",
                table: "Permission",
                type: "text[]",
                nullable: true,
                oldClrType: typeof(string[]),
                oldType: "text[]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HideForeignKeyTableColumns",
                schema: "public",
                table: "ColumnMetadata",
                newName: "CreateForeignKeyReferenceColumn");

            migrationBuilder.AlterColumn<string[]>(
                name: "UserPermissionTypes",
                schema: "public",
                table: "Permission",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0],
                oldClrType: typeof(string[]),
                oldType: "text[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<string[]>(
                name: "PageTypes",
                schema: "public",
                table: "Permission",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0],
                oldClrType: typeof(string[]),
                oldType: "text[]",
                oldNullable: true);
        }
    }
}
