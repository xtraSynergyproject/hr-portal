using Microsoft.EntityFrameworkCore.Migrations;

namespace Synergy.App.Repository.Migrations
{
    public partial class N_20220908_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuGroup_SubModule_SubModuleId",
                schema: "public",
                table: "MenuGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuGroupLog_SubModule_SubModuleId",
                schema: "log",
                table: "MenuGroupLog");

            migrationBuilder.AlterColumn<string>(
                name: "SubModuleId",
                schema: "log",
                table: "MenuGroupLog",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AlterColumn<string>(
                name: "SubModuleId",
                schema: "public",
                table: "MenuGroup",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuGroup_SubModule_SubModuleId",
                schema: "public",
                table: "MenuGroup",
                column: "SubModuleId",
                principalSchema: "public",
                principalTable: "SubModule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuGroupLog_SubModule_SubModuleId",
                schema: "log",
                table: "MenuGroupLog",
                column: "SubModuleId",
                principalSchema: "public",
                principalTable: "SubModule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuGroup_SubModule_SubModuleId",
                schema: "public",
                table: "MenuGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuGroupLog_SubModule_SubModuleId",
                schema: "log",
                table: "MenuGroupLog");

            migrationBuilder.AlterColumn<string>(
                name: "SubModuleId",
                schema: "log",
                table: "MenuGroupLog",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AlterColumn<string>(
                name: "SubModuleId",
                schema: "public",
                table: "MenuGroup",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuGroup_SubModule_SubModuleId",
                schema: "public",
                table: "MenuGroup",
                column: "SubModuleId",
                principalSchema: "public",
                principalTable: "SubModule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuGroupLog_SubModule_SubModuleId",
                schema: "log",
                table: "MenuGroupLog",
                column: "SubModuleId",
                principalSchema: "public",
                principalTable: "SubModule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
