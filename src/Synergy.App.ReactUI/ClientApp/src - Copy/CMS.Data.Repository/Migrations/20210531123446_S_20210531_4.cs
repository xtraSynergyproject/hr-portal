using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210531_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PayrollGroupId",
                schema: "public",
                table: "PayrollBatch",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.CreateTable(
                name: "PayrollRun",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Name = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PayRollNo = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PayrollRunDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ExecutionStartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ExecutionEndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ExecutionStatus = table.Column<int>(type: "integer", nullable: false),
                    YearMonth = table.Column<int>(type: "integer", nullable: true),
                    PayrollStateStart = table.Column<int>(type: "integer", nullable: false),
                    PayrollStateEnd = table.Column<int>(type: "integer", nullable: false),
                    TotalEarning = table.Column<double>(type: "double precision", nullable: false),
                    TotalDeduction = table.Column<double>(type: "double precision", nullable: false),
                    NetAmount = table.Column<double>(type: "double precision", nullable: false),
                    IsExecuteAllEmployee = table.Column<bool>(type: "boolean", nullable: false),
                    TotalProcessed = table.Column<int>(type: "integer", nullable: false),
                    TotalSucceeded = table.Column<int>(type: "integer", nullable: false),
                    ExecutePayrollTotal = table.Column<int>(type: "integer", nullable: false),
                    ExecutePayrollError = table.Column<int>(type: "integer", nullable: false),
                    PayslipTotal = table.Column<int>(type: "integer", nullable: false),
                    PayslipError = table.Column<int>(type: "integer", nullable: false),
                    BankLetterTotal = table.Column<int>(type: "integer", nullable: false),
                    BankLetterError = table.Column<int>(type: "integer", nullable: false),
                    VacationAccrual = table.Column<int>(type: "integer", nullable: true),
                    FlightTicketAccrual = table.Column<int>(type: "integer", nullable: true),
                    EOSAccrual = table.Column<int>(type: "integer", nullable: true),
                    LoanAccrual = table.Column<int>(type: "integer", nullable: true),
                    SickLeaveAccrual = table.Column<int>(type: "integer", nullable: true),
                    PayrollBatchId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PayrollPersonId = table.Column<string>(type: "text", nullable: true)
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
                    table.PrimaryKey("PK_PayrollRun", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PayrollRun",
                schema: "public");

            migrationBuilder.DropColumn(
                name: "PayrollGroupId",
                schema: "public",
                table: "PayrollBatch");
        }
    }
}
