using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210327_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sequenece",
                schema: "rec",
                table: "UserRoleStatusLabelCode",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ChildSequence",
                schema: "rec",
                table: "UserRoleStageParent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "StageSequence",
                schema: "rec",
                table: "UserRoleStageParent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TemplateName",
                schema: "rec",
                table: "UserRoleStageParent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TemplateShortName",
                schema: "rec",
                table: "UserRoleStageParent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sequenece",
                schema: "rec",
                table: "UserRoleStatusLabelCode");

            migrationBuilder.DropColumn(
                name: "ChildSequence",
                schema: "rec",
                table: "UserRoleStageParent");

            migrationBuilder.DropColumn(
                name: "StageSequence",
                schema: "rec",
                table: "UserRoleStageParent");

            migrationBuilder.DropColumn(
                name: "TemplateName",
                schema: "rec",
                table: "UserRoleStageParent");

            migrationBuilder.DropColumn(
                name: "TemplateShortName",
                schema: "rec",
                table: "UserRoleStageParent");
        }
    }
}
