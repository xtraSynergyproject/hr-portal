using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210428_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "BookMarks",
                schema: "rec",
                table: "CandidateProfile",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                schema: "rec",
                table: "CandidateProfile",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookMarks",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropColumn(
                name: "Level",
                schema: "rec",
                table: "CandidateProfile");
        }
    }
}
