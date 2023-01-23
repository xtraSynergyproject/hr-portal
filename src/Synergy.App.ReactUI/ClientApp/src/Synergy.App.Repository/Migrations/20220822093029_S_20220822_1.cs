using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class S_20220822_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserPreference",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    UserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PreferencePortalId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DefaultLandingPageId = table.Column<string>(type: "text", nullable: true)
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
                    table.PrimaryKey("PK_UserPreference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPreference_Page_DefaultLandingPageId",
                        column: x => x.DefaultLandingPageId,
                        principalSchema: "public",
                        principalTable: "Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPreference_Portal_PreferencePortalId",
                        column: x => x.PreferencePortalId,
                        principalSchema: "public",
                        principalTable: "Portal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPreference_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserPreferenceLog",
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
                    UserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PreferencePortalId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DefaultLandingPageId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferenceLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPreferenceLog_Page_DefaultLandingPageId",
                        column: x => x.DefaultLandingPageId,
                        principalSchema: "public",
                        principalTable: "Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPreferenceLog_Portal_PreferencePortalId",
                        column: x => x.PreferencePortalId,
                        principalSchema: "public",
                        principalTable: "Portal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPreferenceLog_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRolePreference",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    UserRoleId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PreferencePortalId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DefaultLandingPageId = table.Column<string>(type: "text", nullable: true)
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
                    table.PrimaryKey("PK_UserRolePreference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRolePreference_Page_DefaultLandingPageId",
                        column: x => x.DefaultLandingPageId,
                        principalSchema: "public",
                        principalTable: "Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRolePreference_Portal_PreferencePortalId",
                        column: x => x.PreferencePortalId,
                        principalSchema: "public",
                        principalTable: "Portal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRolePreference_UserRole_UserRoleId",
                        column: x => x.UserRoleId,
                        principalSchema: "public",
                        principalTable: "UserRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRolePreferenceLog",
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
                    UserRoleId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PreferencePortalId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DefaultLandingPageId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRolePreferenceLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRolePreferenceLog_Page_DefaultLandingPageId",
                        column: x => x.DefaultLandingPageId,
                        principalSchema: "public",
                        principalTable: "Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRolePreferenceLog_Portal_PreferencePortalId",
                        column: x => x.PreferencePortalId,
                        principalSchema: "public",
                        principalTable: "Portal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRolePreferenceLog_UserRole_UserRoleId",
                        column: x => x.UserRoleId,
                        principalSchema: "public",
                        principalTable: "UserRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPreference_DefaultLandingPageId",
                schema: "public",
                table: "UserPreference",
                column: "DefaultLandingPageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPreference_PreferencePortalId",
                schema: "public",
                table: "UserPreference",
                column: "PreferencePortalId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPreference_UserId",
                schema: "public",
                table: "UserPreference",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferenceLog_DefaultLandingPageId",
                schema: "log",
                table: "UserPreferenceLog",
                column: "DefaultLandingPageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferenceLog_PreferencePortalId",
                schema: "log",
                table: "UserPreferenceLog",
                column: "PreferencePortalId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferenceLog_UserId",
                schema: "log",
                table: "UserPreferenceLog",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRolePreference_DefaultLandingPageId",
                schema: "public",
                table: "UserRolePreference",
                column: "DefaultLandingPageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRolePreference_PreferencePortalId",
                schema: "public",
                table: "UserRolePreference",
                column: "PreferencePortalId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRolePreference_UserRoleId",
                schema: "public",
                table: "UserRolePreference",
                column: "UserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRolePreferenceLog_DefaultLandingPageId",
                schema: "log",
                table: "UserRolePreferenceLog",
                column: "DefaultLandingPageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRolePreferenceLog_PreferencePortalId",
                schema: "log",
                table: "UserRolePreferenceLog",
                column: "PreferencePortalId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRolePreferenceLog_UserRoleId",
                schema: "log",
                table: "UserRolePreferenceLog",
                column: "UserRoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPreference",
                schema: "public");

            migrationBuilder.DropTable(
                name: "UserPreferenceLog",
                schema: "log");

            migrationBuilder.DropTable(
                name: "UserRolePreference",
                schema: "public");

            migrationBuilder.DropTable(
                name: "UserRolePreferenceLog",
                schema: "log");
        }
    }
}
