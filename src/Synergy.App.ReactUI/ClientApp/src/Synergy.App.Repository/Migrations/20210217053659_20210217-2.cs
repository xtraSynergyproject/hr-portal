using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _202102172 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecruitmentCandidateElementInfo_RecruitmentPayElement_Recru~",
                schema: "rec",
                table: "RecruitmentCandidateElementInfo");

            migrationBuilder.DropIndex(
                name: "IX_RecruitmentCandidateElementInfo_RecruitmentPayElementId",
                schema: "rec",
                table: "RecruitmentCandidateElementInfo");

            migrationBuilder.DropColumn(
                name: "RecruitmentPayElementId",
                schema: "rec",
                table: "RecruitmentCandidateElementInfo");

            migrationBuilder.AlterColumn<string>(
                name: "ElementId",
                schema: "rec",
                table: "RecruitmentCandidateElementInfo",
                type: "text",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OfferDesigination",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OfferGrade",
                schema: "rec",
                table: "Application",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_RecruitmentCandidateElementInfo_ElementId",
                schema: "rec",
                table: "RecruitmentCandidateElementInfo",
                column: "ElementId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecruitmentCandidateElementInfo_RecruitmentPayElement_Eleme~",
                schema: "rec",
                table: "RecruitmentCandidateElementInfo",
                column: "ElementId",
                principalSchema: "rec",
                principalTable: "RecruitmentPayElement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecruitmentCandidateElementInfo_RecruitmentPayElement_Eleme~",
                schema: "rec",
                table: "RecruitmentCandidateElementInfo");

            migrationBuilder.DropIndex(
                name: "IX_RecruitmentCandidateElementInfo_ElementId",
                schema: "rec",
                table: "RecruitmentCandidateElementInfo");

            migrationBuilder.DropColumn(
                name: "OfferDesigination",
                schema: "rec",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "OfferGrade",
                schema: "rec",
                table: "Application");

            migrationBuilder.AlterColumn<long>(
                name: "ElementId",
                schema: "rec",
                table: "RecruitmentCandidateElementInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RecruitmentPayElementId",
                schema: "rec",
                table: "RecruitmentCandidateElementInfo",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_RecruitmentCandidateElementInfo_RecruitmentPayElementId",
                schema: "rec",
                table: "RecruitmentCandidateElementInfo",
                column: "RecruitmentPayElementId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecruitmentCandidateElementInfo_RecruitmentPayElement_Recru~",
                schema: "rec",
                table: "RecruitmentCandidateElementInfo",
                column: "RecruitmentPayElementId",
                principalSchema: "rec",
                principalTable: "RecruitmentPayElement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
