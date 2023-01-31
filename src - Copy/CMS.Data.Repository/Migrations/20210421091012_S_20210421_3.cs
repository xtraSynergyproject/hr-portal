using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210421_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProcessDesignResult",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ProcessDesignId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NtsServiceId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ProcessDesignStatusId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
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
                    table.PrimaryKey("PK_ProcessDesignResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessDesignResult_LOV_ProcessDesignStatusId",
                        column: x => x.ProcessDesignStatusId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessDesignResult_NtsService_NtsServiceId",
                        column: x => x.NtsServiceId,
                        principalSchema: "public",
                        principalTable: "NtsService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessDesignResult_ProcessDesign_ProcessDesignId",
                        column: x => x.ProcessDesignId,
                        principalSchema: "public",
                        principalTable: "ProcessDesign",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComponentResult",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ComponentType = table.Column<int>(type: "integer", nullable: false),
                    ProcessDesignResultId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ProcessDesignId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ComponentId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NtsServiceId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    NtsTaskId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ComponentStatusId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
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
                    table.PrimaryKey("PK_ComponentResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComponentResult_Component_ComponentId",
                        column: x => x.ComponentId,
                        principalSchema: "public",
                        principalTable: "Component",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComponentResult_LOV_ComponentStatusId",
                        column: x => x.ComponentStatusId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComponentResult_NtsService_NtsServiceId",
                        column: x => x.NtsServiceId,
                        principalSchema: "public",
                        principalTable: "NtsService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComponentResult_NtsTask_NtsTaskId",
                        column: x => x.NtsTaskId,
                        principalSchema: "public",
                        principalTable: "NtsTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComponentResult_ProcessDesign_ProcessDesignId",
                        column: x => x.ProcessDesignId,
                        principalSchema: "public",
                        principalTable: "ProcessDesign",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComponentResult_ProcessDesignResult_ProcessDesignResultId",
                        column: x => x.ProcessDesignResultId,
                        principalSchema: "public",
                        principalTable: "ProcessDesignResult",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComponentResult_ComponentId",
                schema: "public",
                table: "ComponentResult",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentResult_ComponentStatusId",
                schema: "public",
                table: "ComponentResult",
                column: "ComponentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentResult_NtsServiceId",
                schema: "public",
                table: "ComponentResult",
                column: "NtsServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentResult_NtsTaskId",
                schema: "public",
                table: "ComponentResult",
                column: "NtsTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentResult_ProcessDesignId",
                schema: "public",
                table: "ComponentResult",
                column: "ProcessDesignId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentResult_ProcessDesignResultId",
                schema: "public",
                table: "ComponentResult",
                column: "ProcessDesignResultId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessDesignResult_NtsServiceId",
                schema: "public",
                table: "ProcessDesignResult",
                column: "NtsServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessDesignResult_ProcessDesignId",
                schema: "public",
                table: "ProcessDesignResult",
                column: "ProcessDesignId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessDesignResult_ProcessDesignStatusId",
                schema: "public",
                table: "ProcessDesignResult",
                column: "ProcessDesignStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComponentResult",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ProcessDesignResult",
                schema: "public");
        }
    }
}
