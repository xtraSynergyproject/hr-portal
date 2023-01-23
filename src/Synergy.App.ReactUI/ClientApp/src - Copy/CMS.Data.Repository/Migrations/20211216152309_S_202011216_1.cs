using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_202011216_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NtsRating",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NtsType = table.Column<int>(type: "integer", nullable: false),
                    NtsId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    RatingComment = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    RatedByUserId = table.Column<string>(type: "text", nullable: true)
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
                    LegalEntityId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    VersionNo = table.Column<long>(type: "bigint", nullable: false),
                    PortalId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NtsRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NtsRating_User_RatedByUserId",
                        column: x => x.RatedByUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NtsRatingLog",
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
                    IsDatedLatest = table.Column<bool>(type: "boolean", nullable: false),
                    IsVersionLatest = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NtsRatingLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NtsRatingLog_NtsRating_Id",
                        column: x => x.Id,
                        principalSchema: "public",
                        principalTable: "NtsRating",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NtsRating_RatedByUserId",
                schema: "public",
                table: "NtsRating",
                column: "RatedByUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NtsRatingLog",
                schema: "log");

            migrationBuilder.DropTable(
                name: "NtsRating",
                schema: "public");
        }
    }
}
