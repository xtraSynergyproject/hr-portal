using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210408_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "Rating",
                schema: "public",
                table: "NtsService");

            migrationBuilder.RenameColumn(
                name: "Subject",
                schema: "public",
                table: "NtsTask",
                newName: "TaskSubject");

            migrationBuilder.AddColumn<double>(
                name: "UserRating",
                schema: "public",
                table: "NtsTask",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "UserRating",
                schema: "public",
                table: "NtsService",
                type: "double precision",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserRating",
                schema: "public",
                table: "NtsTask");

            migrationBuilder.DropColumn(
                name: "UserRating",
                schema: "public",
                table: "NtsService");

            migrationBuilder.RenameColumn(
                name: "TaskSubject",
                schema: "public",
                table: "NtsTask",
                newName: "Subject");

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                schema: "public",
                table: "NtsTask",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                schema: "public",
                table: "NtsService",
                type: "integer",
                nullable: true);
        }
    }
}
