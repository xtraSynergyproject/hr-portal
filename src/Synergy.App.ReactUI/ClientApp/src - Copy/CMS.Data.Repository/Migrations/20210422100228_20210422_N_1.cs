using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20210422_N_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReferenceTypeCode",
                schema: "rec",
                table: "ListOfValue",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceTypeId",
                schema: "rec",
                table: "ListOfValue",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "ListOfValueTypeId",
                schema: "rec",
                table: "JobCriteria",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateTable(
                name: "JobDescriptionCriteria",
                schema: "rec",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    JobDescriptionId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Criteria = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Type = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Weightage = table.Column<int>(type: "integer", nullable: true),
                    CriteriaType = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ListOfValueTypeId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    LastUpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    SequenceOrder = table.Column<long>(type: "bigint", nullable: true),
                    CompanyId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    VersionNo = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDescriptionCriteria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobDescriptionCriteria_JobDescription_JobDescriptionId",
                        column: x => x.JobDescriptionId,
                        principalSchema: "rec",
                        principalTable: "JobDescription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobDescriptionCriteria_JobDescriptionId",
                schema: "rec",
                table: "JobDescriptionCriteria",
                column: "JobDescriptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobDescriptionCriteria",
                schema: "rec");

            migrationBuilder.DropColumn(
                name: "ReferenceTypeCode",
                schema: "rec",
                table: "ListOfValue");

            migrationBuilder.DropColumn(
                name: "ReferenceTypeId",
                schema: "rec",
                table: "ListOfValue");

            migrationBuilder.DropColumn(
                name: "ListOfValueTypeId",
                schema: "rec",
                table: "JobCriteria");
        }
    }
}
