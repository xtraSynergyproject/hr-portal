using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210429_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttachmentBase64",
                schema: "public",
                table: "Email",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string[]>(
                name: "AttachmentIds",
                schema: "public",
                table: "Email",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentName",
                schema: "public",
                table: "Email",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentBase64",
                schema: "public",
                table: "Email");

            migrationBuilder.DropColumn(
                name: "AttachmentIds",
                schema: "public",
                table: "Email");

            migrationBuilder.DropColumn(
                name: "AttachmentName",
                schema: "public",
                table: "Email");
        }
    }
}
