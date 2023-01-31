using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _202102175 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DropdownValueMethod1",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownValueMethod2",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownValueMethod3",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownValueMethod4",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownValueMethod5",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "IsDropdownDisplay1",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "IsDropdownDisplay2",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "IsDropdownDisplay3",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "IsDropdownDisplay4",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "IsDropdownDisplay5",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TemplateCode",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextBoxDisplay1",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextBoxDisplay2",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextBoxDisplay3",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextBoxDisplay4",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextBoxDisplay5",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "TextBoxDisplayType1",
                schema: "public",
                table: "TaskTemplate",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TextBoxDisplayType2",
                schema: "public",
                table: "TaskTemplate",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TextBoxDisplayType3",
                schema: "public",
                table: "TaskTemplate",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TextBoxDisplayType4",
                schema: "public",
                table: "TaskTemplate",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TextBoxDisplayType5",
                schema: "public",
                table: "TaskTemplate",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DropdownDisplay1",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownDisplay2",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownDisplay3",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownDisplay4",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownDisplay5",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownValue1",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownValue2",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownValue3",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownValue4",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownValue5",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextDisplay1",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextDisplay2",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextDisplay3",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextDisplay4",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextDisplay5",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextValue1",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextValue2",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextValue3",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextValue4",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextValue5",
                schema: "public",
                table: "NtsTask",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DropdownValueMethod1",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "DropdownValueMethod2",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "DropdownValueMethod3",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "DropdownValueMethod4",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "DropdownValueMethod5",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsDropdownDisplay1",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsDropdownDisplay2",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsDropdownDisplay3",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsDropdownDisplay4",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsDropdownDisplay5",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "TemplateCode",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "TextBoxDisplay1",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "TextBoxDisplay2",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "TextBoxDisplay3",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "TextBoxDisplay4",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "TextBoxDisplay5",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "TextBoxDisplayType1",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "TextBoxDisplayType2",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "TextBoxDisplayType3",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "TextBoxDisplayType4",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "TextBoxDisplayType5",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "DropdownDisplay1",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownDisplay2",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownDisplay3",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownDisplay4",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownDisplay5",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownValue1",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownValue2",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownValue3",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownValue4",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "DropdownValue5",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextDisplay1",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextDisplay2",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextDisplay3",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextDisplay4",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextDisplay5",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextValue1",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextValue2",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextValue3",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextValue4",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "TextValue5",
                schema: "public",
                table: "NtsTask");
        }
    }
}
