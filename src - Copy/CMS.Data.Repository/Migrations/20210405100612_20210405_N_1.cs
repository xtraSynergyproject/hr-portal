using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210405_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NtsTaskShared_User_SharedWithTeamId",
                schema: "public",
                table: "NtsTaskShared");

            migrationBuilder.AddColumn<string[]>(
                name: "EditableBy",
                schema: "public",
                table: "ColumnMetadata",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "EditableContext",
                schema: "public",
                table: "ColumnMetadata",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "ViewableBy",
                schema: "public",
                table: "ColumnMetadata",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "ViewableContext",
                schema: "public",
                table: "ColumnMetadata",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTaskShared_Team_SharedWithTeamId",
                schema: "public",
                table: "NtsTaskShared",
                column: "SharedWithTeamId",
                principalSchema: "public",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NtsTaskShared_Team_SharedWithTeamId",
                schema: "public",
                table: "NtsTaskShared");

            migrationBuilder.DropColumn(
                name: "EditableBy",
                schema: "public",
                table: "ColumnMetadata");

            migrationBuilder.DropColumn(
                name: "EditableContext",
                schema: "public",
                table: "ColumnMetadata");

            migrationBuilder.DropColumn(
                name: "ViewableBy",
                schema: "public",
                table: "ColumnMetadata");

            migrationBuilder.DropColumn(
                name: "ViewableContext",
                schema: "public",
                table: "ColumnMetadata");

            migrationBuilder.AddForeignKey(
                name: "FK_NtsTaskShared_User_SharedWithTeamId",
                schema: "public",
                table: "NtsTaskShared",
                column: "SharedWithTeamId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
