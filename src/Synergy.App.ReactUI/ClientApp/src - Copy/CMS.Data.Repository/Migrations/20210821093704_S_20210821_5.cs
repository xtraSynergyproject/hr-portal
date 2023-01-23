using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210821_5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ViewableByUserId",
                schema: "log",
                table: "UdfPermissionLog",
                newName: "UdfPermissionHeaderId");

            migrationBuilder.RenameColumn(
                name: "ViewableByTeamId",
                schema: "log",
                table: "UdfPermissionLog",
                newName: "UdfPermissionHeader");

            migrationBuilder.RenameColumn(
                name: "ViewableByUserId",
                schema: "public",
                table: "UdfPermission",
                newName: "UdfPermissionHeaderId");

            migrationBuilder.RenameColumn(
                name: "ViewableByTeamId",
                schema: "public",
                table: "UdfPermission",
                newName: "UdfPermissionHeader");

            migrationBuilder.AddColumn<bool>(
                name: "EnableDynamicGridBinding",
                schema: "public",
                table: "Page",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "UdfPermissionHeader",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    UserType = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TeamId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PageId = table.Column<string>(type: "text", nullable: true)
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
                    table.PrimaryKey("PK_UdfPermissionHeader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UdfPermissionHeader_Page_PageId",
                        column: x => x.PageId,
                        principalSchema: "public",
                        principalTable: "Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UdfPermissionHeader_Team_TeamId",
                        column: x => x.TeamId,
                        principalSchema: "public",
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UdfPermissionHeader_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UdfPermissionHeaderLog",
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
                    IsVersionLatest = table.Column<bool>(type: "boolean", nullable: false),
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
                    DataAction = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    VersionNo = table.Column<long>(type: "bigint", nullable: false),
                    PortalId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    UserType = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TeamId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PageId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UdfPermissionHeaderLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UdfPermissionHeaderLog_Page_PageId",
                        column: x => x.PageId,
                        principalSchema: "public",
                        principalTable: "Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UdfPermissionHeaderLog_Team_TeamId",
                        column: x => x.TeamId,
                        principalSchema: "public",
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UdfPermissionHeaderLog_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UdfPermissionHeader_PageId",
                schema: "public",
                table: "UdfPermissionHeader",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_UdfPermissionHeader_TeamId",
                schema: "public",
                table: "UdfPermissionHeader",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_UdfPermissionHeader_UserId",
                schema: "public",
                table: "UdfPermissionHeader",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UdfPermissionHeaderLog_PageId",
                schema: "log",
                table: "UdfPermissionHeaderLog",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_UdfPermissionHeaderLog_TeamId",
                schema: "log",
                table: "UdfPermissionHeaderLog",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_UdfPermissionHeaderLog_UserId",
                schema: "log",
                table: "UdfPermissionHeaderLog",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UdfPermissionHeader",
                schema: "public");

            migrationBuilder.DropTable(
                name: "UdfPermissionHeaderLog",
                schema: "log");

            migrationBuilder.DropColumn(
                name: "EnableDynamicGridBinding",
                schema: "public",
                table: "Page");

            migrationBuilder.RenameColumn(
                name: "UdfPermissionHeaderId",
                schema: "log",
                table: "UdfPermissionLog",
                newName: "ViewableByUserId");

            migrationBuilder.RenameColumn(
                name: "UdfPermissionHeader",
                schema: "log",
                table: "UdfPermissionLog",
                newName: "ViewableByTeamId");

            migrationBuilder.RenameColumn(
                name: "UdfPermissionHeaderId",
                schema: "public",
                table: "UdfPermission",
                newName: "ViewableByUserId");

            migrationBuilder.RenameColumn(
                name: "UdfPermissionHeader",
                schema: "public",
                table: "UdfPermission",
                newName: "ViewableByTeamId");
        }
    }
}
