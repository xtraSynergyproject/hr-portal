using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210406_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NtsNoteComment",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Comment = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CommentedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CommentToUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CommentedByUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NtsNoteId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ParentCommentId = table.Column<string>(type: "text", nullable: true)
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
                    table.PrimaryKey("PK_NtsNoteComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NtsNoteComment_NtsNote_NtsNoteId",
                        column: x => x.NtsNoteId,
                        principalSchema: "public",
                        principalTable: "NtsNote",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsNoteComment_NtsNoteComment_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalSchema: "public",
                        principalTable: "NtsNoteComment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsNoteComment_User_CommentedByUserId",
                        column: x => x.CommentedByUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsNoteComment_User_CommentToUserId",
                        column: x => x.CommentToUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NtsNoteShared",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NoteSharedWithTypeId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SharedWithUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SharedWithTeamId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SharedByUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NtsNoteId = table.Column<string>(type: "text", nullable: true)
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
                    table.PrimaryKey("PK_NtsNoteShared", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NtsNoteShared_LOV_NoteSharedWithTypeId",
                        column: x => x.NoteSharedWithTypeId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsNoteShared_NtsNote_NtsNoteId",
                        column: x => x.NtsNoteId,
                        principalSchema: "public",
                        principalTable: "NtsNote",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsNoteShared_User_SharedByUserId",
                        column: x => x.SharedByUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsNoteShared_User_SharedWithTeamId",
                        column: x => x.SharedWithTeamId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsNoteShared_User_SharedWithUserId",
                        column: x => x.SharedWithUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NtsServiceComment",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Comment = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CommentedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CommentToUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CommentedByUserId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NtsServiceId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ParentCommentId = table.Column<string>(type: "text", nullable: true)
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
                    table.PrimaryKey("PK_NtsServiceComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NtsServiceComment_NtsService_NtsServiceId",
                        column: x => x.NtsServiceId,
                        principalSchema: "public",
                        principalTable: "NtsService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsServiceComment_NtsServiceComment_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalSchema: "public",
                        principalTable: "NtsServiceComment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsServiceComment_User_CommentedByUserId",
                        column: x => x.CommentedByUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NtsServiceComment_User_CommentToUserId",
                        column: x => x.CommentToUserId,
                        principalSchema: "public",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NtsNoteComment_CommentedByUserId",
                schema: "public",
                table: "NtsNoteComment",
                column: "CommentedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsNoteComment_CommentToUserId",
                schema: "public",
                table: "NtsNoteComment",
                column: "CommentToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsNoteComment_NtsNoteId",
                schema: "public",
                table: "NtsNoteComment",
                column: "NtsNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsNoteComment_ParentCommentId",
                schema: "public",
                table: "NtsNoteComment",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsNoteShared_NoteSharedWithTypeId",
                schema: "public",
                table: "NtsNoteShared",
                column: "NoteSharedWithTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsNoteShared_NtsNoteId",
                schema: "public",
                table: "NtsNoteShared",
                column: "NtsNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsNoteShared_SharedByUserId",
                schema: "public",
                table: "NtsNoteShared",
                column: "SharedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsNoteShared_SharedWithTeamId",
                schema: "public",
                table: "NtsNoteShared",
                column: "SharedWithTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsNoteShared_SharedWithUserId",
                schema: "public",
                table: "NtsNoteShared",
                column: "SharedWithUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsServiceComment_CommentedByUserId",
                schema: "public",
                table: "NtsServiceComment",
                column: "CommentedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsServiceComment_CommentToUserId",
                schema: "public",
                table: "NtsServiceComment",
                column: "CommentToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsServiceComment_NtsServiceId",
                schema: "public",
                table: "NtsServiceComment",
                column: "NtsServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_NtsServiceComment_ParentCommentId",
                schema: "public",
                table: "NtsServiceComment",
                column: "ParentCommentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NtsNoteComment",
                schema: "public");

            migrationBuilder.DropTable(
                name: "NtsNoteShared",
                schema: "public");

            migrationBuilder.DropTable(
                name: "NtsServiceComment",
                schema: "public");
        }
    }
}
