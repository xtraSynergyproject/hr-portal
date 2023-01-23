using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210521_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UdfTableMetadataId",
                schema: "log",
                table: "StepTaskComponentLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "UdfTableMetadataId",
                schema: "public",
                table: "StepTaskComponent",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskComponentLog_UdfTableMetadataId",
                schema: "log",
                table: "StepTaskComponentLog",
                column: "UdfTableMetadataId");

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskComponent_UdfTableMetadataId",
                schema: "public",
                table: "StepTaskComponent",
                column: "UdfTableMetadataId");

            migrationBuilder.AddForeignKey(
                name: "FK_StepTaskComponent_TableMetadata_UdfTableMetadataId",
                schema: "public",
                table: "StepTaskComponent",
                column: "UdfTableMetadataId",
                principalSchema: "public",
                principalTable: "TableMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StepTaskComponentLog_TableMetadata_UdfTableMetadataId",
                schema: "log",
                table: "StepTaskComponentLog",
                column: "UdfTableMetadataId",
                principalSchema: "public",
                principalTable: "TableMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StepTaskComponent_TableMetadata_UdfTableMetadataId",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropForeignKey(
                name: "FK_StepTaskComponentLog_TableMetadata_UdfTableMetadataId",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropIndex(
                name: "IX_StepTaskComponentLog_UdfTableMetadataId",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropIndex(
                name: "IX_StepTaskComponent_UdfTableMetadataId",
                schema: "public",
                table: "StepTaskComponent");

            migrationBuilder.DropColumn(
                name: "UdfTableMetadataId",
                schema: "log",
                table: "StepTaskComponentLog");

            migrationBuilder.DropColumn(
                name: "UdfTableMetadataId",
                schema: "public",
                table: "StepTaskComponent");
        }
    }
}
