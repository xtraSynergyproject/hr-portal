using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210401_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedToUserText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OwnerUserText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RequestedByUserText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SubjectText",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "AssignedToUserText",
                schema: "public",
                table: "StepTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionText",
                schema: "public",
                table: "StepTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OwnerUserText",
                schema: "public",
                table: "StepTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RequestedByUserText",
                schema: "public",
                table: "StepTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SubjectText",
                schema: "public",
                table: "StepTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionText",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OwnerUserText",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RequestedByUserText",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SubjectText",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionText",
                schema: "public",
                table: "NoteTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "OwnerUserText",
                schema: "public",
                table: "NoteTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RequestedByUserText",
                schema: "public",
                table: "NoteTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "SubjectText",
                schema: "public",
                table: "NoteTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedToUserText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "DescriptionText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "OwnerUserText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "RequestedByUserText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "SubjectText",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "AssignedToUserText",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "DescriptionText",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "OwnerUserText",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "RequestedByUserText",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "SubjectText",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "DescriptionText",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "OwnerUserText",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "RequestedByUserText",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "SubjectText",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "DescriptionText",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "OwnerUserText",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "RequestedByUserText",
                schema: "public",
                table: "NoteTemplate");

            migrationBuilder.DropColumn(
                name: "SubjectText",
                schema: "public",
                table: "NoteTemplate");
        }
    }
}
