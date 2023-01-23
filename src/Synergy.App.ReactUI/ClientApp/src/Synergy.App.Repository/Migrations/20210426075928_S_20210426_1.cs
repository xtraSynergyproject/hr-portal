using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210426_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StepTaskComponentUdf",
                schema: "public");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StepTaskComponentUdf",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ColumnMetadataId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CompanyId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CreatedBy = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EditableBy = table.Column<string[]>(type: "text[]", nullable: true),
                    EditableContext = table.Column<string[]>(type: "text[]", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    LastUpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    SequenceOrder = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StepTaskComponentId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    VersionNo = table.Column<long>(type: "bigint", nullable: false),
                    ViewableBy = table.Column<string[]>(type: "text[]", nullable: true),
                    ViewableContext = table.Column<string[]>(type: "text[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StepTaskComponentUdf", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StepTaskComponentUdf_StepTaskComponent_StepTaskComponentId",
                        column: x => x.StepTaskComponentId,
                        principalSchema: "public",
                        principalTable: "StepTaskComponent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StepTaskComponentUdf_StepTaskComponentId",
                schema: "public",
                table: "StepTaskComponentUdf",
                column: "StepTaskComponentId");
        }
    }
}
