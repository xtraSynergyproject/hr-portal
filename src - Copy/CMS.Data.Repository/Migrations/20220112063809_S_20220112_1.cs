using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20220112_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanySettingLog_CompanySetting_Id",
                schema: "log",
                table: "CompanySettingLog");

            migrationBuilder.DropForeignKey(
                name: "FK_NtsRatingLog_NtsRating_Id",
                schema: "log",
                table: "NtsRatingLog");

            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                schema: "log",
                table: "NtsRatingLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "log",
                table: "NtsRatingLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "log",
                table: "NtsRatingLog",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DataAction",
                schema: "log",
                table: "NtsRatingLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "log",
                table: "NtsRatingLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                schema: "log",
                table: "NtsRatingLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedDate",
                schema: "log",
                table: "NtsRatingLog",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsRatingLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "NtsId",
                schema: "log",
                table: "NtsRatingLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "NtsType",
                schema: "log",
                table: "NtsRatingLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PortalId",
                schema: "log",
                table: "NtsRatingLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "RatedByUserId",
                schema: "log",
                table: "NtsRatingLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                schema: "log",
                table: "NtsRatingLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RatingComment",
                schema: "log",
                table: "NtsRatingLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<long>(
                name: "SequenceOrder",
                schema: "log",
                table: "NtsRatingLog",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "log",
                table: "NtsRatingLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "VersionNo",
                schema: "log",
                table: "NtsRatingLog",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "log",
                table: "CompanySettingLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                schema: "log",
                table: "CompanySettingLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "log",
                table: "CompanySettingLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "log",
                table: "CompanySettingLog",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DataAction",
                schema: "log",
                table: "CompanySettingLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "GroupCode",
                schema: "log",
                table: "CompanySettingLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "log",
                table: "CompanySettingLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                schema: "log",
                table: "CompanySettingLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedDate",
                schema: "log",
                table: "CompanySettingLog",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LegalEntityId",
                schema: "log",
                table: "CompanySettingLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "log",
                table: "CompanySettingLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<string>(
                name: "PortalId",
                schema: "log",
                table: "CompanySettingLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<long>(
                name: "SequenceOrder",
                schema: "log",
                table: "CompanySettingLog",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "log",
                table: "CompanySettingLog",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                schema: "log",
                table: "CompanySettingLog",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci");

            migrationBuilder.AddColumn<long>(
                name: "VersionNo",
                schema: "log",
                table: "CompanySettingLog",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_NtsRatingLog_RatedByUserId",
                schema: "log",
                table: "NtsRatingLog",
                column: "RatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_NtsRatingLog_User_RatedByUserId",
                schema: "log",
                table: "NtsRatingLog",
                column: "RatedByUserId",
                principalSchema: "public",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NtsRatingLog_User_RatedByUserId",
                schema: "log",
                table: "NtsRatingLog");

            migrationBuilder.DropIndex(
                name: "IX_NtsRatingLog_RatedByUserId",
                schema: "log",
                table: "NtsRatingLog");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "log",
                table: "NtsRatingLog");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "log",
                table: "NtsRatingLog");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "log",
                table: "NtsRatingLog");

            migrationBuilder.DropColumn(
                name: "DataAction",
                schema: "log",
                table: "NtsRatingLog");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "log",
                table: "NtsRatingLog");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                schema: "log",
                table: "NtsRatingLog");

            migrationBuilder.DropColumn(
                name: "LastUpdatedDate",
                schema: "log",
                table: "NtsRatingLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "NtsRatingLog");

            migrationBuilder.DropColumn(
                name: "NtsId",
                schema: "log",
                table: "NtsRatingLog");

            migrationBuilder.DropColumn(
                name: "NtsType",
                schema: "log",
                table: "NtsRatingLog");

            migrationBuilder.DropColumn(
                name: "PortalId",
                schema: "log",
                table: "NtsRatingLog");

            migrationBuilder.DropColumn(
                name: "RatedByUserId",
                schema: "log",
                table: "NtsRatingLog");

            migrationBuilder.DropColumn(
                name: "Rating",
                schema: "log",
                table: "NtsRatingLog");

            migrationBuilder.DropColumn(
                name: "RatingComment",
                schema: "log",
                table: "NtsRatingLog");

            migrationBuilder.DropColumn(
                name: "SequenceOrder",
                schema: "log",
                table: "NtsRatingLog");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "log",
                table: "NtsRatingLog");

            migrationBuilder.DropColumn(
                name: "VersionNo",
                schema: "log",
                table: "NtsRatingLog");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "log",
                table: "CompanySettingLog");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "log",
                table: "CompanySettingLog");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "log",
                table: "CompanySettingLog");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "log",
                table: "CompanySettingLog");

            migrationBuilder.DropColumn(
                name: "DataAction",
                schema: "log",
                table: "CompanySettingLog");

            migrationBuilder.DropColumn(
                name: "GroupCode",
                schema: "log",
                table: "CompanySettingLog");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "log",
                table: "CompanySettingLog");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                schema: "log",
                table: "CompanySettingLog");

            migrationBuilder.DropColumn(
                name: "LastUpdatedDate",
                schema: "log",
                table: "CompanySettingLog");

            migrationBuilder.DropColumn(
                name: "LegalEntityId",
                schema: "log",
                table: "CompanySettingLog");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "log",
                table: "CompanySettingLog");

            migrationBuilder.DropColumn(
                name: "PortalId",
                schema: "log",
                table: "CompanySettingLog");

            migrationBuilder.DropColumn(
                name: "SequenceOrder",
                schema: "log",
                table: "CompanySettingLog");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "log",
                table: "CompanySettingLog");

            migrationBuilder.DropColumn(
                name: "Value",
                schema: "log",
                table: "CompanySettingLog");

            migrationBuilder.DropColumn(
                name: "VersionNo",
                schema: "log",
                table: "CompanySettingLog");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanySettingLog_CompanySetting_Id",
                schema: "log",
                table: "CompanySettingLog",
                column: "Id",
                principalSchema: "public",
                principalTable: "CompanySetting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NtsRatingLog_NtsRating_Id",
                schema: "log",
                table: "NtsRatingLog",
                column: "Id",
                principalSchema: "public",
                principalTable: "NtsRating",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
