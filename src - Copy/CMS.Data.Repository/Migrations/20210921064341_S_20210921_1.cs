using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210921_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLOVCodeEnabled",
                schema: "log",
                table: "CustomTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLOVDescriptionEnabled",
                schema: "log",
                table: "CustomTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLOVDescriptionMandatory",
                schema: "log",
                table: "CustomTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLOVIamgeEnabled",
                schema: "log",
                table: "CustomTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLOVIamgeMandatory",
                schema: "log",
                table: "CustomTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLOVParentEnabled",
                schema: "log",
                table: "CustomTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLOVParentMandatory",
                schema: "log",
                table: "CustomTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLOVStatusEnabled",
                schema: "log",
                table: "CustomTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLOVStatusMandatory",
                schema: "log",
                table: "CustomTemplateLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LOVCodeLabel",
                schema: "log",
                table: "CustomTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LOVDescriptionLabel",
                schema: "log",
                table: "CustomTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LOVImageLabel",
                schema: "log",
                table: "CustomTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LOVNameLabel",
                schema: "log",
                table: "CustomTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LOVParentLabel",
                schema: "log",
                table: "CustomTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LOVStatusLabel",
                schema: "log",
                table: "CustomTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LOVTitle",
                schema: "log",
                table: "CustomTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LOVTypeId",
                schema: "log",
                table: "CustomTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "IsLOVCodeEnabled",
                schema: "public",
                table: "CustomTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLOVDescriptionEnabled",
                schema: "public",
                table: "CustomTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLOVDescriptionMandatory",
                schema: "public",
                table: "CustomTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLOVIamgeEnabled",
                schema: "public",
                table: "CustomTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLOVIamgeMandatory",
                schema: "public",
                table: "CustomTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLOVParentEnabled",
                schema: "public",
                table: "CustomTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLOVParentMandatory",
                schema: "public",
                table: "CustomTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLOVStatusEnabled",
                schema: "public",
                table: "CustomTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLOVStatusMandatory",
                schema: "public",
                table: "CustomTemplate",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LOVCodeLabel",
                schema: "public",
                table: "CustomTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LOVDescriptionLabel",
                schema: "public",
                table: "CustomTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LOVImageLabel",
                schema: "public",
                table: "CustomTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LOVNameLabel",
                schema: "public",
                table: "CustomTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LOVParentLabel",
                schema: "public",
                table: "CustomTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LOVStatusLabel",
                schema: "public",
                table: "CustomTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LOVTitle",
                schema: "public",
                table: "CustomTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "LOVTypeId",
                schema: "public",
                table: "CustomTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_CustomTemplateLog_LOVTypeId",
                schema: "log",
                table: "CustomTemplateLog",
                column: "LOVTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomTemplate_LOVTypeId",
                schema: "public",
                table: "CustomTemplate",
                column: "LOVTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomTemplate_LOV_LOVTypeId",
                schema: "public",
                table: "CustomTemplate",
                column: "LOVTypeId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomTemplateLog_LOV_LOVTypeId",
                schema: "log",
                table: "CustomTemplateLog",
                column: "LOVTypeId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomTemplate_LOV_LOVTypeId",
                schema: "public",
                table: "CustomTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomTemplateLog_LOV_LOVTypeId",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_CustomTemplateLog_LOVTypeId",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_CustomTemplate_LOVTypeId",
                schema: "public",
                table: "CustomTemplate");

            migrationBuilder.DropColumn(
                name: "IsLOVCodeEnabled",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "IsLOVDescriptionEnabled",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "IsLOVDescriptionMandatory",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "IsLOVIamgeEnabled",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "IsLOVIamgeMandatory",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "IsLOVParentEnabled",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "IsLOVParentMandatory",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "IsLOVStatusEnabled",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "IsLOVStatusMandatory",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "LOVCodeLabel",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "LOVDescriptionLabel",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "LOVImageLabel",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "LOVNameLabel",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "LOVParentLabel",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "LOVStatusLabel",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "LOVTitle",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "LOVTypeId",
                schema: "log",
                table: "CustomTemplateLog");

            migrationBuilder.DropColumn(
                name: "IsLOVCodeEnabled",
                schema: "public",
                table: "CustomTemplate");

            migrationBuilder.DropColumn(
                name: "IsLOVDescriptionEnabled",
                schema: "public",
                table: "CustomTemplate");

            migrationBuilder.DropColumn(
                name: "IsLOVDescriptionMandatory",
                schema: "public",
                table: "CustomTemplate");

            migrationBuilder.DropColumn(
                name: "IsLOVIamgeEnabled",
                schema: "public",
                table: "CustomTemplate");

            migrationBuilder.DropColumn(
                name: "IsLOVIamgeMandatory",
                schema: "public",
                table: "CustomTemplate");

            migrationBuilder.DropColumn(
                name: "IsLOVParentEnabled",
                schema: "public",
                table: "CustomTemplate");

            migrationBuilder.DropColumn(
                name: "IsLOVParentMandatory",
                schema: "public",
                table: "CustomTemplate");

            migrationBuilder.DropColumn(
                name: "IsLOVStatusEnabled",
                schema: "public",
                table: "CustomTemplate");

            migrationBuilder.DropColumn(
                name: "IsLOVStatusMandatory",
                schema: "public",
                table: "CustomTemplate");

            migrationBuilder.DropColumn(
                name: "LOVCodeLabel",
                schema: "public",
                table: "CustomTemplate");

            migrationBuilder.DropColumn(
                name: "LOVDescriptionLabel",
                schema: "public",
                table: "CustomTemplate");

            migrationBuilder.DropColumn(
                name: "LOVImageLabel",
                schema: "public",
                table: "CustomTemplate");

            migrationBuilder.DropColumn(
                name: "LOVNameLabel",
                schema: "public",
                table: "CustomTemplate");

            migrationBuilder.DropColumn(
                name: "LOVParentLabel",
                schema: "public",
                table: "CustomTemplate");

            migrationBuilder.DropColumn(
                name: "LOVStatusLabel",
                schema: "public",
                table: "CustomTemplate");

            migrationBuilder.DropColumn(
                name: "LOVTitle",
                schema: "public",
                table: "CustomTemplate");

            migrationBuilder.DropColumn(
                name: "LOVTypeId",
                schema: "public",
                table: "CustomTemplate");
        }
    }
}
