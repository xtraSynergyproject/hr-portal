using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210531_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PayrollBatch",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Name = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    RunType = table.Column<int>(type: "integer", nullable: false),
                    YearMonth = table.Column<int>(type: "integer", nullable: true),
                    PayrollStartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    PayrollEndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AttendanceStartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    AttendanceEndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PayrollStatus = table.Column<int>(type: "integer", nullable: false),
                    TotalEarning = table.Column<double>(type: "double precision", nullable: false),
                    TotalDeduction = table.Column<double>(type: "double precision", nullable: false),
                    NetAmount = table.Column<double>(type: "double precision", nullable: false),
                    ExecutePayrollTotal = table.Column<int>(type: "integer", nullable: false),
                    ExecutePayrollError = table.Column<int>(type: "integer", nullable: false),
                    PayslipTotal = table.Column<int>(type: "integer", nullable: false),
                    PayslipError = table.Column<int>(type: "integer", nullable: false),
                    BankLetterTotal = table.Column<int>(type: "integer", nullable: false),
                    BankLetterError = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_PayrollBatch", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PayrollTransaction",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Name = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    EffectiveDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    FromDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ToDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EarningAmount = table.Column<double>(type: "double precision", nullable: false),
                    DeductionAmount = table.Column<double>(type: "double precision", nullable: false),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    OpeningBalance = table.Column<double>(type: "double precision", nullable: true),
                    ClosingBalance = table.Column<double>(type: "double precision", nullable: true),
                    PostedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    PostedSource = table.Column<int>(type: "integer", nullable: false),
                    ProcessStatus = table.Column<int>(type: "integer", nullable: false),
                    ProcessedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PostedUserId = table.Column<long>(type: "bigint", nullable: false),
                    ElementType = table.Column<int>(type: "integer", nullable: false),
                    ElementCategory = table.Column<int>(type: "integer", nullable: false),
                    ElementClassification = table.Column<int>(type: "integer", nullable: false),
                    ReferenceNode = table.Column<int>(type: "integer", nullable: true),
                    ReferenceId = table.Column<long>(type: "bigint", nullable: true),
                    PayrollId = table.Column<long>(type: "bigint", nullable: true),
                    PayrollRunId = table.Column<long>(type: "bigint", nullable: true),
                    Rate = table.Column<double>(type: "double precision", nullable: true),
                    Uom = table.Column<int>(type: "integer", nullable: true),
                    Quantity = table.Column<double>(type: "double precision", nullable: true),
                    EarningQuantity = table.Column<double>(type: "double precision", nullable: true),
                    DeductionQuantity = table.Column<double>(type: "double precision", nullable: true),
                    OpeningQuantity = table.Column<double>(type: "double precision", nullable: true),
                    ClosingQuantity = table.Column<double>(type: "double precision", nullable: true),
                    IsTransactionClosed = table.Column<bool>(type: "boolean", nullable: true),
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
                    table.PrimaryKey("PK_PayrollTransaction", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PayrollBatch",
                schema: "public");

            migrationBuilder.DropTable(
                name: "PayrollTransaction",
                schema: "public");
        }
    }
}
