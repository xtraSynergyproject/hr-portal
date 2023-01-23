﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210216_N_6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CandidateProfile_User_UserId",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.DropIndex(
                name: "IX_CandidateProfile_UserId",
                schema: "rec",
                table: "CandidateProfile");

            migrationBuilder.AddColumn<string>(
                name: "AgencyId",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgencyId",
                schema: "rec",
                table: "Application");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateProfile_UserId",
                schema: "rec",
                table: "CandidateProfile",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateProfile_User_UserId",
                schema: "rec",
                table: "CandidateProfile",
                column: "UserId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}