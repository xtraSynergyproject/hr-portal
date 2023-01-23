using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210524_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserSetId",
                schema: "public",
                table: "User",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "UserSetLogId",
                schema: "public",
                table: "User",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateTable(
                name: "UserSet",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Name = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Code = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    LastUpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    SequenceOrder = table.Column<long>(type: "bigint", nullable: true),
                    CompanyId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    VersionNo = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSetLog",
                schema: "log",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    RecordId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    LogVersionNo = table.Column<long>(type: "bigint", nullable: false),
                    IsLatest = table.Column<bool>(type: "boolean", nullable: false),
                    LogStartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LogEndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LogStartDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LogEndDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    LastUpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    SequenceOrder = table.Column<long>(type: "bigint", nullable: true),
                    CompanyId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DataAction = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    VersionNo = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Code = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSetLog", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_UserSetId",
                schema: "public",
                table: "User",
                column: "UserSetId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserSetLogId",
                schema: "public",
                table: "User",
                column: "UserSetLogId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_UserSet_UserSetId",
                schema: "public",
                table: "User",
                column: "UserSetId",
                principalSchema: "public",
                principalTable: "UserSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_UserSetLog_UserSetLogId",
                schema: "public",
                table: "User",
                column: "UserSetLogId",
                principalSchema: "log",
                principalTable: "UserSetLog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_UserSet_UserSetId",
                schema: "public",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_UserSetLog_UserSetLogId",
                schema: "public",
                table: "User");

            migrationBuilder.DropTable(
                name: "UserSet",
                schema: "public");

            migrationBuilder.DropTable(
                name: "UserSetLog",
                schema: "log");

            migrationBuilder.DropIndex(
                name: "IX_User_UserSetId",
                schema: "public",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_UserSetLogId",
                schema: "public",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserSetId",
                schema: "public",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserSetLogId",
                schema: "public",
                table: "User");
        }
    }
}
