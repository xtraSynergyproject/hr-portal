using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210709_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParentTableMetadata",
                schema: "log",
                table: "TableMetadataLog",
                newName: "ParentTableMetadataId");

            migrationBuilder.RenameColumn(
                name: "ParentTableMetadata",
                schema: "public",
                table: "TableMetadata",
                newName: "ParentTableMetadataId");

            migrationBuilder.AddColumn<double>(
                name: "BasicSalaryPercentage",
                schema: "log",
                table: "LegalEntityLog",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HousingAllowancePercentage",
                schema: "log",
                table: "LegalEntityLog",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TransportAllowancePercentage",
                schema: "log",
                table: "LegalEntityLog",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BasicSalaryPercentage",
                schema: "public",
                table: "LegalEntity",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HousingAllowancePercentage",
                schema: "public",
                table: "LegalEntity",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TransportAllowancePercentage",
                schema: "public",
                table: "LegalEntity",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasicSalaryPercentage",
                schema: "log",
                table: "LegalEntityLog");

            migrationBuilder.DropColumn(
                name: "HousingAllowancePercentage",
                schema: "log",
                table: "LegalEntityLog");

            migrationBuilder.DropColumn(
                name: "TransportAllowancePercentage",
                schema: "log",
                table: "LegalEntityLog");

            migrationBuilder.DropColumn(
                name: "BasicSalaryPercentage",
                schema: "public",
                table: "LegalEntity");

            migrationBuilder.DropColumn(
                name: "HousingAllowancePercentage",
                schema: "public",
                table: "LegalEntity");

            migrationBuilder.DropColumn(
                name: "TransportAllowancePercentage",
                schema: "public",
                table: "LegalEntity");

            migrationBuilder.RenameColumn(
                name: "ParentTableMetadataId",
                schema: "log",
                table: "TableMetadataLog",
                newName: "ParentTableMetadata");

            migrationBuilder.RenameColumn(
                name: "ParentTableMetadataId",
                schema: "public",
                table: "TableMetadata",
                newName: "ParentTableMetadata");
        }
    }
}
