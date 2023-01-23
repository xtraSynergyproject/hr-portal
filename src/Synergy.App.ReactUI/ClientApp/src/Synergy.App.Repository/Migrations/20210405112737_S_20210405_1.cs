using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210405_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NtsServiceShared",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ServiceSharedWithTypeId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SharedWithUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SharedWithTeamId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SharedByUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NtsServiceId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SharedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
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
                    table.PrimaryKey("PK_NtsServiceShared", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NtsServiceShared_LOV_ServiceSharedWithTypeId",
                        column: x => x.ServiceSharedWithTypeId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsServiceShared_NtsService_NtsServiceId",
                        column: x => x.NtsServiceId,
                        principalSchema: "public",
                        principalTable: "NtsService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsServiceShared_Team_SharedWithTeamId",
                        column: x => x.SharedWithTeamId,
                        principalSchema: "public",
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsServiceShared_User_SharedByUserId",
                        column: x => x.SharedByUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsServiceShared_User_SharedWithUserId",
                        column: x => x.SharedWithUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NtsServiceShared_NtsServiceId",
                schema: "public",
                table: "NtsServiceShared",
                column: "NtsServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsServiceShared_ServiceSharedWithTypeId",
                schema: "public",
                table: "NtsServiceShared",
                column: "ServiceSharedWithTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsServiceShared_SharedByUserId",
                schema: "public",
                table: "NtsServiceShared",
                column: "SharedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsServiceShared_SharedWithTeamId",
                schema: "public",
                table: "NtsServiceShared",
                column: "SharedWithTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsServiceShared_SharedWithUserId",
                schema: "public",
                table: "NtsServiceShared",
                column: "SharedWithUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NtsServiceShared",
                schema: "public");
        }
    }
}
