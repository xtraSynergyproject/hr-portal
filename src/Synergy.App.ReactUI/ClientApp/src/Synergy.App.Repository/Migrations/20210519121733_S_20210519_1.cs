using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210519_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NtsGroup",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NtsType = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Code = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Description = table.Column<string>(type: "text", nullable: true)
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
                    table.PrimaryKey("PK_NtsGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserGroup",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Name = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Code = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Description = table.Column<string>(type: "text", nullable: true)
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
                    table.PrimaryKey("PK_UserGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NtsGroupTemplate",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NtsGroupId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TemplateId = table.Column<string>(type: "text", nullable: true)
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
                    table.PrimaryKey("PK_NtsGroupTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NtsGroupTemplate_NtsGroup_NtsGroupId",
                        column: x => x.NtsGroupId,
                        principalSchema: "public",
                        principalTable: "NtsGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsGroupTemplate_Template_TemplateId",
                        column: x => x.TemplateId,
                        principalSchema: "public",
                        principalTable: "Template",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NtsGroupUserGroup",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NtsGroupId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    UserGroupId = table.Column<string>(type: "text", nullable: true)
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
                    table.PrimaryKey("PK_NtsGroupUserGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NtsGroupUserGroup_NtsGroup_NtsGroupId",
                        column: x => x.NtsGroupId,
                        principalSchema: "public",
                        principalTable: "NtsGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsGroupUserGroup_UserGroup_UserGroupId",
                        column: x => x.UserGroupId,
                        principalSchema: "public",
                        principalTable: "UserGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserGroupUser",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    UserGroupId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    UserId = table.Column<string>(type: "text", nullable: true)
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
                    table.PrimaryKey("PK_UserGroupUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGroupUser_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserGroupUser_UserGroup_UserGroupId",
                        column: x => x.UserGroupId,
                        principalSchema: "public",
                        principalTable: "UserGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NtsGroupTemplate_NtsGroupId",
                schema: "public",
                table: "NtsGroupTemplate",
                column: "NtsGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsGroupTemplate_TemplateId",
                schema: "public",
                table: "NtsGroupTemplate",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsGroupUserGroup_NtsGroupId",
                schema: "public",
                table: "NtsGroupUserGroup",
                column: "NtsGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsGroupUserGroup_UserGroupId",
                schema: "public",
                table: "NtsGroupUserGroup",
                column: "UserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupUser_UserGroupId",
                schema: "public",
                table: "UserGroupUser",
                column: "UserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupUser_UserId",
                schema: "public",
                table: "UserGroupUser",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NtsGroupTemplate",
                schema: "public");

            migrationBuilder.DropTable(
                name: "NtsGroupUserGroup",
                schema: "public");

            migrationBuilder.DropTable(
                name: "UserGroupUser",
                schema: "public");

            migrationBuilder.DropTable(
                name: "NtsGroup",
                schema: "public");

            migrationBuilder.DropTable(
                name: "UserGroup",
                schema: "public");
        }
    }
}
