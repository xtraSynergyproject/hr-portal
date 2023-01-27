using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210821_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultOwnerTeamId",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DefaultOwnerUserId",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DefaultServiceOwnerTypeId",
                schema: "log",
                table: "ServiceTemplateLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DefaultOwnerTeamId",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DefaultOwnerUserId",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "DefaultServiceOwnerTypeId",
                schema: "public",
                table: "ServiceTemplate",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

           

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTemplateLog_DefaultOwnerTeamId",
                schema: "log",
                table: "ServiceTemplateLog",
                column: "DefaultOwnerTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTemplateLog_DefaultOwnerUserId",
                schema: "log",
                table: "ServiceTemplateLog",
                column: "DefaultOwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTemplateLog_DefaultServiceOwnerTypeId",
                schema: "log",
                table: "ServiceTemplateLog",
                column: "DefaultServiceOwnerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTemplate_DefaultOwnerTeamId",
                schema: "public",
                table: "ServiceTemplate",
                column: "DefaultOwnerTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTemplate_DefaultOwnerUserId",
                schema: "public",
                table: "ServiceTemplate",
                column: "DefaultOwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTemplate_DefaultServiceOwnerTypeId",
                schema: "public",
                table: "ServiceTemplate",
                column: "DefaultServiceOwnerTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTemplate_LOV_DefaultServiceOwnerTypeId",
                schema: "public",
                table: "ServiceTemplate",
                column: "DefaultServiceOwnerTypeId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTemplate_Team_DefaultOwnerTeamId",
                schema: "public",
                table: "ServiceTemplate",
                column: "DefaultOwnerTeamId",
                principalSchema: "public",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTemplate_User_DefaultOwnerUserId",
                schema: "public",
                table: "ServiceTemplate",
                column: "DefaultOwnerUserId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTemplateLog_LOV_DefaultServiceOwnerTypeId",
                schema: "log",
                table: "ServiceTemplateLog",
                column: "DefaultServiceOwnerTypeId",
                principalSchema: "public",
                principalTable: "LOV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTemplateLog_Team_DefaultOwnerTeamId",
                schema: "log",
                table: "ServiceTemplateLog",
                column: "DefaultOwnerTeamId",
                principalSchema: "public",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTemplateLog_User_DefaultOwnerUserId",
                schema: "log",
                table: "ServiceTemplateLog",
                column: "DefaultOwnerUserId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTemplate_LOV_DefaultServiceOwnerTypeId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTemplate_Team_DefaultOwnerTeamId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTemplate_User_DefaultOwnerUserId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTemplateLog_LOV_DefaultServiceOwnerTypeId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTemplateLog_Team_DefaultOwnerTeamId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTemplateLog_User_DefaultOwnerUserId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_ServiceTemplateLog_DefaultOwnerTeamId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_ServiceTemplateLog_DefaultOwnerUserId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_ServiceTemplateLog_DefaultServiceOwnerTypeId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropIndex(
                name: "IX_ServiceTemplate_DefaultOwnerTeamId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropIndex(
                name: "IX_ServiceTemplate_DefaultOwnerUserId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropIndex(
                name: "IX_ServiceTemplate_DefaultServiceOwnerTypeId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "DefaultOwnerTeamId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "DefaultOwnerUserId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "DefaultServiceOwnerTypeId",
                schema: "log",
                table: "ServiceTemplateLog");

            migrationBuilder.DropColumn(
                name: "DefaultOwnerTeamId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "DefaultOwnerUserId",
                schema: "public",
                table: "ServiceTemplate");

            migrationBuilder.DropColumn(
                name: "DefaultServiceOwnerTypeId",
                schema: "public",
                table: "ServiceTemplate");

           
        }
    }
}
