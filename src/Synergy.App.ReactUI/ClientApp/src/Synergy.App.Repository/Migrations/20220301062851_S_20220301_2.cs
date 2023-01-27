using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20220301_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "HierarchyPath",
                schema: "log",
                table: "HybridHierarchyLog",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "HierarchyPath",
                schema: "public",
                table: "HybridHierarchy",
                type: "text[]",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HierarchyPath",
                schema: "log",
                table: "HybridHierarchyLog");

            migrationBuilder.DropColumn(
                name: "HierarchyPath",
                schema: "public",
                table: "HybridHierarchy");
        }
    }
}
