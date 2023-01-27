﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210221_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrganizationId",
                schema: "rec",
                table: "JobAdvertisementTrack",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OrganizationId",
                schema: "rec",
                table: "JobAdvertisement",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganizationId",
                schema: "rec",
                table: "JobAdvertisementTrack");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                schema: "rec",
                table: "JobAdvertisement");
        }
    }
}