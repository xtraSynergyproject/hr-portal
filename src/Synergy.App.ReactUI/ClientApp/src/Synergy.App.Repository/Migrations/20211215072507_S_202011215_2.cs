using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_202011215_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubjectMappingUserId",
                schema: "log",
                table: "TaskTemplateLog",
                newName: "SubjectMappingUdfId");

            migrationBuilder.RenameColumn(
                name: "DescriptionMappingUserId",
                schema: "log",
                table: "TaskTemplateLog",
                newName: "DescriptionMappingUdfId");

            migrationBuilder.RenameColumn(
                name: "SubjectMappingUserId",
                schema: "public",
                table: "TaskTemplate",
                newName: "SubjectMappingUdfId");

            migrationBuilder.RenameColumn(
                name: "DescriptionMappingUserId",
                schema: "public",
                table: "TaskTemplate",
                newName: "DescriptionMappingUdfId");

            migrationBuilder.RenameColumn(
                name: "SubjectMappingUserId",
                schema: "log",
                table: "ServiceTemplateLog",
                newName: "SubjectMappingUdfId");

            migrationBuilder.RenameColumn(
                name: "DescriptionMappingUserId",
                schema: "log",
                table: "ServiceTemplateLog",
                newName: "DescriptionMappingUdfId");

            migrationBuilder.RenameColumn(
                name: "SubjectMappingUserId",
                schema: "public",
                table: "ServiceTemplate",
                newName: "SubjectMappingUdfId");

            migrationBuilder.RenameColumn(
                name: "DescriptionMappingUserId",
                schema: "public",
                table: "ServiceTemplate",
                newName: "DescriptionMappingUdfId");

            migrationBuilder.RenameColumn(
                name: "SubjectMappingUserId",
                schema: "log",
                table: "NoteTemplateLog",
                newName: "SubjectMappingUdfId");

            migrationBuilder.RenameColumn(
                name: "DescriptionMappingUserId",
                schema: "log",
                table: "NoteTemplateLog",
                newName: "DescriptionMappingUdfId");

            migrationBuilder.RenameColumn(
                name: "SubjectMappingUserId",
                schema: "public",
                table: "NoteTemplate",
                newName: "SubjectMappingUdfId");

            migrationBuilder.RenameColumn(
                name: "DescriptionMappingUserId",
                schema: "public",
                table: "NoteTemplate",
                newName: "DescriptionMappingUdfId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubjectMappingUdfId",
                schema: "log",
                table: "TaskTemplateLog",
                newName: "SubjectMappingUserId");

            migrationBuilder.RenameColumn(
                name: "DescriptionMappingUdfId",
                schema: "log",
                table: "TaskTemplateLog",
                newName: "DescriptionMappingUserId");

            migrationBuilder.RenameColumn(
                name: "SubjectMappingUdfId",
                schema: "public",
                table: "TaskTemplate",
                newName: "SubjectMappingUserId");

            migrationBuilder.RenameColumn(
                name: "DescriptionMappingUdfId",
                schema: "public",
                table: "TaskTemplate",
                newName: "DescriptionMappingUserId");

            migrationBuilder.RenameColumn(
                name: "SubjectMappingUdfId",
                schema: "log",
                table: "ServiceTemplateLog",
                newName: "SubjectMappingUserId");

            migrationBuilder.RenameColumn(
                name: "DescriptionMappingUdfId",
                schema: "log",
                table: "ServiceTemplateLog",
                newName: "DescriptionMappingUserId");

            migrationBuilder.RenameColumn(
                name: "SubjectMappingUdfId",
                schema: "public",
                table: "ServiceTemplate",
                newName: "SubjectMappingUserId");

            migrationBuilder.RenameColumn(
                name: "DescriptionMappingUdfId",
                schema: "public",
                table: "ServiceTemplate",
                newName: "DescriptionMappingUserId");

            migrationBuilder.RenameColumn(
                name: "SubjectMappingUdfId",
                schema: "log",
                table: "NoteTemplateLog",
                newName: "SubjectMappingUserId");

            migrationBuilder.RenameColumn(
                name: "DescriptionMappingUdfId",
                schema: "log",
                table: "NoteTemplateLog",
                newName: "DescriptionMappingUserId");

            migrationBuilder.RenameColumn(
                name: "SubjectMappingUdfId",
                schema: "public",
                table: "NoteTemplate",
                newName: "SubjectMappingUserId");

            migrationBuilder.RenameColumn(
                name: "DescriptionMappingUdfId",
                schema: "public",
                table: "NoteTemplate",
                newName: "DescriptionMappingUserId");
        }
    }
}
