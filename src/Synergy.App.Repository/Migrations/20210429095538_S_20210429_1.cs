using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210429_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HideDescription",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideDueDate",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideHeader",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HidePriority",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideSLA",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideStartDate",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideSubject",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDescriptionMandatory",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSubjectMandatory",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TaskNoText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "HideDescription",
                schema: "public",
                table: "ServiceTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideExpiryDate",
                schema: "public",
                table: "ServiceTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideHeader",
                schema: "public",
                table: "ServiceTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HidePriority",
                schema: "public",
                table: "ServiceTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideStartDate",
                schema: "public",
                table: "ServiceTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideSubject",
                schema: "public",
                table: "ServiceTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDescriptionMandatory",
                schema: "public",
                table: "ServiceTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSubjectMandatory",
                schema: "public",
                table: "ServiceTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ServiceNoText",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "HideDescription",
                schema: "public",
                table: "NoteTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideExpiryDate",
                schema: "public",
                table: "NoteTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideHeader",
                schema: "public",
                table: "NoteTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HidePriority",
                schema: "public",
                table: "NoteTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideStartDate",
                schema: "public",
                table: "NoteTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HideSubject",
                schema: "public",
                table: "NoteTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDescriptionMandatory",
                schema: "public",
                table: "NoteTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSubjectMandatory",
                schema: "public",
                table: "NoteTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NoteNoText",
                schema: "public",
                table: "NoteTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HideDescription",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "HideDueDate",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "HideHeader",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "HidePriority",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "HideSLA",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "HideStartDate",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "HideSubject",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsDescriptionMandatory",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsSubjectMandatory",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "TaskNoText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "HideDescription",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "HideExpiryDate",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "HideHeader",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "HidePriority",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "HideStartDate",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "HideSubject",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "IsDescriptionMandatory",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "IsSubjectMandatory",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "ServiceNoText",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "HideDescription",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "HideExpiryDate",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "HideHeader",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "HidePriority",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "HideStartDate",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "HideSubject",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "IsDescriptionMandatory",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "IsSubjectMandatory",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "NoteNoText",
                schema: "public",
                table: "NoteTemplate");
        }
    }
}
