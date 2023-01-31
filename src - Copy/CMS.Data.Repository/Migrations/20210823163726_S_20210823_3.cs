using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210823_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultRequesterTeamId",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DefaultRequesterUserId",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DefaultServiceRequesterTypeId",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DefaultRequesterTeamId",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DefaultRequesterUserId",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DefaultServiceRequesterTypeId",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTemplateLog_DefaultRequesterTeamId",
                schema: "log",
                table: "ServiceTemplateLog",
                column: "DefaultRequesterTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTemplateLog_DefaultRequesterUserId",
                schema: "log",
                table: "ServiceTemplateLog",
                column: "DefaultRequesterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTemplateLog_DefaultServiceRequesterTypeId",
                schema: "log",
                table: "ServiceTemplateLog",
                column: "DefaultServiceRequesterTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTemplate_DefaultRequesterTeamId",
                schema: "public",
                table: "ServiceTemplate",
                column: "DefaultRequesterTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTemplate_DefaultRequesterUserId",
                schema: "public",
                table: "ServiceTemplate",
                column: "DefaultRequesterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTemplate_DefaultServiceRequesterTypeId",
                schema: "public",
                table: "ServiceTemplate",
                column: "DefaultServiceRequesterTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTemplate_LOV_DefaultServiceRequesterTypeId",
                schema: "public",
                table: "ServiceTemplate",
                column: "DefaultServiceRequesterTypeId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTemplate_Team_DefaultRequesterTeamId",
                schema: "public",
                table: "ServiceTemplate",
                column: "DefaultRequesterTeamId",
                principalSchema: "public",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTemplate_User_DefaultRequesterUserId",
                schema: "public",
                table: "ServiceTemplate",
                column: "DefaultRequesterUserId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTemplateLog_LOV_DefaultServiceRequesterTypeId",
                schema: "log",
                table: "ServiceTemplateLog",
                column: "DefaultServiceRequesterTypeId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTemplateLog_Team_DefaultRequesterTeamId",
                schema: "log",
                table: "ServiceTemplateLog",
                column: "DefaultRequesterTeamId",
                principalSchema: "public",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTemplateLog_User_DefaultRequesterUserId",
                schema: "log",
                table: "ServiceTemplateLog",
                column: "DefaultRequesterUserId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTemplate_LOV_DefaultServiceRequesterTypeId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTemplate_Team_DefaultRequesterTeamId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTemplate_User_DefaultRequesterUserId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTemplateLog_LOV_DefaultServiceRequesterTypeId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTemplateLog_Team_DefaultRequesterTeamId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTemplateLog_User_DefaultRequesterUserId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_ServiceTemplateLog_DefaultRequesterTeamId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_ServiceTemplateLog_DefaultRequesterUserId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_ServiceTemplateLog_DefaultServiceRequesterTypeId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_ServiceTemplate_DefaultRequesterTeamId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropIndex(
                name: "IX_ServiceTemplate_DefaultRequesterUserId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropIndex(
                name: "IX_ServiceTemplate_DefaultServiceRequesterTypeId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "DefaultRequesterTeamId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "DefaultRequesterUserId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "DefaultServiceRequesterTypeId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "DefaultRequesterTeamId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "DefaultRequesterUserId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "DefaultServiceRequesterTypeId",
                schema: "public",
                table: "ServiceTemplate");
        }
    }
}
