using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210218_T_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SLA",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "interval",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AlterColumn<string>(
                name: "SLA",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "interval",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "SLA",
                schema: "public",
                table: "TaskTemplate",
                type: "interval",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "SLA",
                schema: "public",
                table: "NtsTask",
                type: "interval",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }
    }
}
