using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class _20220127_T_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OCRMapping",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    FieldName = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Cordinate1 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Cordinate2 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Cordinate3 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Cordinate4 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Cordinate5 = table.Column<string>(type: "text", nullable: true)
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
                    LegalEntityId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    VersionNo = table.Column<long>(type: "bigint", nullable: false),
                    PortalId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OCRMapping", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OCRMappingLog",
                schema: "log",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    RecordId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    LogVersionNo = table.Column<long>(type: "bigint", nullable: false),
                    IsLatest = table.Column<bool>(type: "boolean", nullable: false),
                    LogStartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LogEndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LogStartDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LogEndDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDatedLatest = table.Column<bool>(type: "boolean", nullable: false),
                    IsVersionLatest = table.Column<bool>(type: "boolean", nullable: false),
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
                    LegalEntityId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DataAction = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    VersionNo = table.Column<long>(type: "bigint", nullable: false),
                    PortalId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    FieldName = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Cordinate1 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Cordinate2 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Cordinate3 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Cordinate4 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Cordinate5 = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OCRMappingLog", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OCRMapping",
                schema: "public");

            migrationBuilder.DropTable(
                name: "OCRMappingLog",
                schema: "log");
        }
    }
}
