using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210504_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceTemplateLog2",
                schema: "log");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceTemplateLog2",
                schema: "log",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    AllowSLAChange = table.Column<bool>(type: "boolean", nullable: false),
                    BackButtonCss = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    BackButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    BackgroundFileId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    BannerFileId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CancelButtonCss = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CancelButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CompanyId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CompleteButtonCss = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CompleteButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CreateReturnType = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAction = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    DescriptionText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    EditReturnType = table.Column<int>(type: "integer", nullable: false),
                    EnableAttachment = table.Column<bool>(type: "boolean", nullable: false),
                    EnableBackButton = table.Column<bool>(type: "boolean", nullable: false),
                    EnableCancelButton = table.Column<bool>(type: "boolean", nullable: false),
                    EnableComment = table.Column<bool>(type: "boolean", nullable: false),
                    EnableCompleteButton = table.Column<bool>(type: "boolean", nullable: false),
                    EnableIndexPage = table.Column<bool>(type: "boolean", nullable: false),
                    EnableSaveAsDraft = table.Column<bool>(type: "boolean", nullable: false),
                    EnableServiceNumberManual = table.Column<bool>(type: "boolean", nullable: false),
                    HideBanner = table.Column<bool>(type: "boolean", nullable: false),
                    HideDescription = table.Column<bool>(type: "boolean", nullable: false),
                    HideExpiryDate = table.Column<bool>(type: "boolean", nullable: false),
                    HideHeader = table.Column<bool>(type: "boolean", nullable: false),
                    HidePriority = table.Column<bool>(type: "boolean", nullable: false),
                    HideStartDate = table.Column<bool>(type: "boolean", nullable: false),
                    HideSubject = table.Column<bool>(type: "boolean", nullable: false),
                    HideToolbar = table.Column<bool>(type: "boolean", nullable: false),
                    IconFileId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    IsCancelReasonRequired = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IsDescriptionMandatory = table.Column<bool>(type: "boolean", nullable: false),
                    IsSubjectMandatory = table.Column<bool>(type: "boolean", nullable: false),
                    LastUpdatedBy = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    LastUpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    NotificationSubject = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    OwnerUserText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PostScript = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PreScript = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    PriorityId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    RecordId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    RequestedByUserText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SLA = table.Column<TimeSpan>(type: "interval", nullable: true),
                    SaveAsDraftCss = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SaveAsDraftText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SequenceOrder = table.Column<long>(type: "bigint", nullable: true),
                    ServiceIndexPageTemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    ServiceNoText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SubjectText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SubmitButtonCss = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    SubmitButtonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    TemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    UdfTemplateId = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
                    VersionNo = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTemplateLog2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceTemplateLog2_LOV_PriorityId",
                        column: x => x.PriorityId,
                        principalSchema: "public",
                        principalTable: "LOV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceTemplateLog2_ServiceIndexPageTemplate_ServiceIndexPa~",
                        column: x => x.ServiceIndexPageTemplateId,
                        principalSchema: "public",
                        principalTable: "ServiceIndexPageTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceTemplateLog2_Template_TemplateId",
                        column: x => x.TemplateId,
                        principalSchema: "public",
                        principalTable: "Template",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceTemplateLog2_Template_UdfTemplateId",
                        column: x => x.UdfTemplateId,
                        principalSchema: "public",
                        principalTable: "Template",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTemplateLog2_PriorityId",
                schema: "log",
                table: "ServiceTemplateLog2",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTemplateLog2_ServiceIndexPageTemplateId",
                schema: "log",
                table: "ServiceTemplateLog2",
                column: "ServiceIndexPageTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTemplateLog2_TemplateId",
                schema: "log",
                table: "ServiceTemplateLog2",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTemplateLog2_UdfTemplateId",
                schema: "log",
                table: "ServiceTemplateLog2",
                column: "UdfTemplateId");
        }
    }
}
