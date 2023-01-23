using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210218_T_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DropdownValueMethod10",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownValueMethod6",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownValueMethod7",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownValueMethod8",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DropdownValueMethod9",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "IsAssigneeAvailableDropdownDisplay1",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAssigneeAvailableDropdownDisplay10",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAssigneeAvailableDropdownDisplay2",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAssigneeAvailableDropdownDisplay3",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAssigneeAvailableDropdownDisplay4",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAssigneeAvailableDropdownDisplay5",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAssigneeAvailableDropdownDisplay6",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAssigneeAvailableDropdownDisplay7",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAssigneeAvailableDropdownDisplay8",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAssigneeAvailableDropdownDisplay9",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAssigneeAvailableTextBoxDisplay1",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAssigneeAvailableTextBoxDisplay10",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAssigneeAvailableTextBoxDisplay2",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAssigneeAvailableTextBoxDisplay3",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAssigneeAvailableTextBoxDisplay4",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAssigneeAvailableTextBoxDisplay5",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAssigneeAvailableTextBoxDisplay6",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAssigneeAvailableTextBoxDisplay7",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAssigneeAvailableTextBoxDisplay8",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAssigneeAvailableTextBoxDisplay9",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "IsDropdownDisplay10",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "IsDropdownDisplay6",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "IsDropdownDisplay7",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "IsDropdownDisplay8",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "IsDropdownDisplay9",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "IsRequiredDropdownDisplay1",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequiredDropdownDisplay10",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequiredDropdownDisplay2",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequiredDropdownDisplay3",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequiredDropdownDisplay4",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequiredDropdownDisplay5",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequiredDropdownDisplay6",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequiredDropdownDisplay7",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequiredDropdownDisplay8",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequiredDropdownDisplay9",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequiredTextBoxDisplay1",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequiredTextBoxDisplay10",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequiredTextBoxDisplay2",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequiredTextBoxDisplay3",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequiredTextBoxDisplay4",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequiredTextBoxDisplay5",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequiredTextBoxDisplay6",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequiredTextBoxDisplay7",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequiredTextBoxDisplay8",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequiredTextBoxDisplay9",
                schema: "public",
                table: "TaskTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TextBoxDisplay10",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextBoxDisplay6",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextBoxDisplay7",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextBoxDisplay8",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "TextBoxDisplay9",
                schema: "public",
                table: "TaskTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "TextBoxDisplayType10",
                schema: "public",
                table: "TaskTemplate",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TextBoxDisplayType6",
                schema: "public",
                table: "TaskTemplate",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TextBoxDisplayType7",
                schema: "public",
                table: "TaskTemplate",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TextBoxDisplayType8",
                schema: "public",
                table: "TaskTemplate",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TextBoxDisplayType9",
                schema: "public",
                table: "TaskTemplate",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DropdownValueMethod10",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "DropdownValueMethod6",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "DropdownValueMethod7",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "DropdownValueMethod8",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "DropdownValueMethod9",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsAssigneeAvailableDropdownDisplay1",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsAssigneeAvailableDropdownDisplay10",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsAssigneeAvailableDropdownDisplay2",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsAssigneeAvailableDropdownDisplay3",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsAssigneeAvailableDropdownDisplay4",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsAssigneeAvailableDropdownDisplay5",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsAssigneeAvailableDropdownDisplay6",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsAssigneeAvailableDropdownDisplay7",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsAssigneeAvailableDropdownDisplay8",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsAssigneeAvailableDropdownDisplay9",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsAssigneeAvailableTextBoxDisplay1",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsAssigneeAvailableTextBoxDisplay10",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsAssigneeAvailableTextBoxDisplay2",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsAssigneeAvailableTextBoxDisplay3",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsAssigneeAvailableTextBoxDisplay4",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsAssigneeAvailableTextBoxDisplay5",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsAssigneeAvailableTextBoxDisplay6",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsAssigneeAvailableTextBoxDisplay7",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsAssigneeAvailableTextBoxDisplay8",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsAssigneeAvailableTextBoxDisplay9",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsDropdownDisplay10",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsDropdownDisplay6",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsDropdownDisplay7",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsDropdownDisplay8",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsDropdownDisplay9",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsRequiredDropdownDisplay1",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsRequiredDropdownDisplay10",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsRequiredDropdownDisplay2",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsRequiredDropdownDisplay3",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsRequiredDropdownDisplay4",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsRequiredDropdownDisplay5",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsRequiredDropdownDisplay6",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsRequiredDropdownDisplay7",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsRequiredDropdownDisplay8",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsRequiredDropdownDisplay9",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsRequiredTextBoxDisplay1",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsRequiredTextBoxDisplay10",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsRequiredTextBoxDisplay2",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsRequiredTextBoxDisplay3",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsRequiredTextBoxDisplay4",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsRequiredTextBoxDisplay5",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsRequiredTextBoxDisplay6",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsRequiredTextBoxDisplay7",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsRequiredTextBoxDisplay8",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "IsRequiredTextBoxDisplay9",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "TextBoxDisplay10",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "TextBoxDisplay6",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "TextBoxDisplay7",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "TextBoxDisplay8",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "TextBoxDisplay9",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "TextBoxDisplayType10",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "TextBoxDisplayType6",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "TextBoxDisplayType7",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "TextBoxDisplayType8",
                schema: "public",
                table: "TaskTemplate");

            migrationBuilder.DropColumn(
                name: "TextBoxDisplayType9",
                schema: "public",
                table: "TaskTemplate");
        }
    }
}
