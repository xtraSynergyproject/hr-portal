using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_2210316_6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProcessDesignId",
                schema: "public",
                table: "ProcessDesignVariable",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessDesignVariable_ProcessDesignId",
                schema: "public",
                table: "ProcessDesignVariable",
                column: "ProcessDesignId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessDesignVariable_ProcessDesign_ProcessDesignId",
                schema: "public",
                table: "ProcessDesignVariable",
                column: "ProcessDesignId",
                principalSchema: "public",
                principalTable: "ProcessDesign",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessDesignVariable_ProcessDesign_ProcessDesignId",
                schema: "public",
                table: "ProcessDesignVariable");

            migrationBuilder.DropIndex(
                name: "IX_ProcessDesignVariable_ProcessDesignId",
                schema: "public",
                table: "ProcessDesignVariable");

            migrationBuilder.DropColumn(
                name: "ProcessDesignId",
                schema: "public",
                table: "ProcessDesignVariable");
        }
    }
}
