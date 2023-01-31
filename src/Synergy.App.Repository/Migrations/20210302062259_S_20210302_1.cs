using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210302_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserDataPermission",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    UserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PageId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ColumnMetadataId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Values = table.Column<string[]>(type: "text[]", nullable: true),
                    ColumnMetadataId2 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Values2 = table.Column<string[]>(type: "text[]", nullable: true),
                    LogicalOperationType = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_UserDataPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDataPermission_ColumnMetadata_ColumnMetadataId",
                        column: x => x.ColumnMetadataId,
                        principalSchema: "public",
                        principalTable: "ColumnMetadata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDataPermission_ColumnMetadata_ColumnMetadataId2",
                        column: x => x.ColumnMetadataId2,
                        principalSchema: "public",
                        principalTable: "ColumnMetadata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDataPermission_Page_PageId",
                        column: x => x.PageId,
                        principalSchema: "public",
                        principalTable: "Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDataPermission_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleDataPermission",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    UserRoleId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PageId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ColumnMetadataId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Values = table.Column<string[]>(type: "text[]", nullable: true),
                    ColumnMetadataId2 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Values2 = table.Column<string[]>(type: "text[]", nullable: true),
                    LogicalOperationType = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_UserRoleDataPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoleDataPermission_ColumnMetadata_ColumnMetadataId",
                        column: x => x.ColumnMetadataId,
                        principalSchema: "public",
                        principalTable: "ColumnMetadata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoleDataPermission_ColumnMetadata_ColumnMetadataId2",
                        column: x => x.ColumnMetadataId2,
                        principalSchema: "public",
                        principalTable: "ColumnMetadata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoleDataPermission_Page_PageId",
                        column: x => x.PageId,
                        principalSchema: "public",
                        principalTable: "Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoleDataPermission_UserRole_UserRoleId",
                        column: x => x.UserRoleId,
                        principalSchema: "public",
                        principalTable: "UserRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDataPermission_ColumnMetadataId",
                schema: "public",
                table: "UserDataPermission",
                column: "ColumnMetadataId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDataPermission_ColumnMetadataId2",
                schema: "public",
                table: "UserDataPermission",
                column: "ColumnMetadataId2");

            migrationBuilder.CreateIndex(
                name: "IX_UserDataPermission_PageId",
                schema: "public",
                table: "UserDataPermission",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDataPermission_UserId",
                schema: "public",
                table: "UserDataPermission",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleDataPermission_ColumnMetadataId",
                schema: "public",
                table: "UserRoleDataPermission",
                column: "ColumnMetadataId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleDataPermission_ColumnMetadataId2",
                schema: "public",
                table: "UserRoleDataPermission",
                column: "ColumnMetadataId2");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleDataPermission_PageId",
                schema: "public",
                table: "UserRoleDataPermission",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleDataPermission_UserRoleId",
                schema: "public",
                table: "UserRoleDataPermission",
                column: "UserRoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDataPermission",
                schema: "public");

            migrationBuilder.DropTable(
                name: "UserRoleDataPermission",
                schema: "public");
        }
    }
}
