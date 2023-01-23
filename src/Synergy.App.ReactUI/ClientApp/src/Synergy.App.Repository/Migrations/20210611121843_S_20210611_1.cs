using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210611_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentPermission",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PermissionType = table.Column<int>(type: "integer", nullable: false),
                    Access = table.Column<int>(type: "integer", nullable: false),
                    AppliesTo = table.Column<int>(type: "integer", nullable: false),
                    NoteId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PermittedUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PermittedUserGroupId = table.Column<string>(type: "text", nullable: true)
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
                    VersionNo = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentPermission_NtsNote_NoteId",
                        column: x => x.NoteId,
                        principalSchema: "public",
                        principalTable: "NtsNote",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentPermission_User_PermittedUserId",
                        column: x => x.PermittedUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentPermission_UserGroup_PermittedUserGroupId",
                        column: x => x.PermittedUserGroupId,
                        principalSchema: "public",
                        principalTable: "UserGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPermission_NoteId",
                schema: "public",
                table: "DocumentPermission",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPermission_PermittedUserGroupId",
                schema: "public",
                table: "DocumentPermission",
                column: "PermittedUserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentPermission_PermittedUserId",
                schema: "public",
                table: "DocumentPermission",
                column: "PermittedUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentPermission",
                schema: "public");
        }
    }
}
