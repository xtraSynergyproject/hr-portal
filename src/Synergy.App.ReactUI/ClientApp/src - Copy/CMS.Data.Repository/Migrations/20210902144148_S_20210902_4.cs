using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Data.Repository.Migrations
{
    public partial class S_20210902_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "ServiceTemplateLog",
            //    schema: "log");

            //migrationBuilder.AddColumn<int>(
            //    name: "ServiceTemplateType",
            //    schema: "public",
            //    table: "ServiceTemplate",
            //    type: "integer",
            //    nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //    migrationBuilder.DropColumn(
            //        name: "ServiceTemplateType",
            //        schema: "public",
            //        table: "ServiceTemplate");

            //    migrationBuilder.CreateTable(
            //        name: "ServiceTemplateLog",
            //        schema: "log",
            //        columns: table => new
            //        {
            //            Id = table.Column<string>(type: "text", nullable: false)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            AdhocNoteTemplateIds = table.Column<string[]>(type: "text[]", nullable: true),
            //            AdhocServiceTemplateIds = table.Column<string[]>(type: "text[]", nullable: true),
            //            AdhocTaskTemplateIds = table.Column<string[]>(type: "text[]", nullable: true),
            //            AllowPastStartDate = table.Column<bool>(type: "boolean", nullable: false),
            //            AllowSLAChange = table.Column<bool>(type: "boolean", nullable: false),
            //            BackButtonCss = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            BackButtonText = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            BackgroundFileId = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            BannerFileId = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            CancelButtonCss = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            CancelButtonText = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            CompanyId = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            CompleteButtonCss = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            CompleteButtonText = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            CreateReturnType = table.Column<int>(type: "integer", nullable: false),
            //            CreatedBy = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //            DataAction = table.Column<int>(type: "integer", nullable: false),
            //            DataPermissionColumnId = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            DefaultOwnerTeamId = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            DefaultOwnerUserId = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            DefaultRequesterTeamId = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            DefaultRequesterUserId = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            DefaultServiceOwnerTypeId = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            DefaultServiceRequesterTypeId = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            Description = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            DescriptionText = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            EditReturnType = table.Column<int>(type: "integer", nullable: false),
            //            EnableAdhocTask = table.Column<bool>(type: "boolean", nullable: false),
            //            EnableAttachment = table.Column<bool>(type: "boolean", nullable: false),
            //            EnableBackButton = table.Column<bool>(type: "boolean", nullable: false),
            //            EnableCancelButton = table.Column<bool>(type: "boolean", nullable: false),
            //            EnableComment = table.Column<bool>(type: "boolean", nullable: false),
            //            EnableCompleteButton = table.Column<bool>(type: "boolean", nullable: false),
            //            EnableDataPermission = table.Column<bool>(type: "boolean", nullable: false),
            //            EnableIndexPage = table.Column<bool>(type: "boolean", nullable: false),
            //            EnableInlineComment = table.Column<bool>(type: "boolean", nullable: false),
            //            EnableLegalEntityFilter = table.Column<bool>(type: "boolean", nullable: false),
            //            EnablePermission = table.Column<bool>(type: "boolean", nullable: false),
            //            EnablePrintButton = table.Column<bool>(type: "boolean", nullable: false),
            //            EnableSaveAsDraft = table.Column<bool>(type: "boolean", nullable: false),
            //            EnableServiceNumberManual = table.Column<bool>(type: "boolean", nullable: false),
            //            HideBanner = table.Column<bool>(type: "boolean", nullable: false),
            //            HideDescription = table.Column<bool>(type: "boolean", nullable: false),
            //            HideExpiryDate = table.Column<bool>(type: "boolean", nullable: false),
            //            HideHeader = table.Column<bool>(type: "boolean", nullable: false),
            //            HideOwner = table.Column<bool>(type: "boolean", nullable: false),
            //            HidePriority = table.Column<bool>(type: "boolean", nullable: false),
            //            HideSLA = table.Column<bool>(type: "boolean", nullable: false),
            //            HideServiceOwner = table.Column<bool>(type: "boolean", nullable: false),
            //            HideStartDate = table.Column<bool>(type: "boolean", nullable: false),
            //            HideSubject = table.Column<bool>(type: "boolean", nullable: false),
            //            HideToolbar = table.Column<bool>(type: "boolean", nullable: false),
            //            IconFileId = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            IsCancelReasonRequired = table.Column<bool>(type: "boolean", nullable: false),
            //            IsDatedLatest = table.Column<bool>(type: "boolean", nullable: false),
            //            IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
            //            IsDescriptionMandatory = table.Column<bool>(type: "boolean", nullable: false),
            //            IsLatest = table.Column<bool>(type: "boolean", nullable: false),
            //            IsNumberNotMandatory = table.Column<bool>(type: "boolean", nullable: false),
            //            IsSubjectMandatory = table.Column<bool>(type: "boolean", nullable: false),
            //            IsSubjectUnique = table.Column<bool>(type: "boolean", nullable: false),
            //            IsVersionLatest = table.Column<bool>(type: "boolean", nullable: false),
            //            LastUpdatedBy = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            LastUpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //            LegalEntityId = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            LogEndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //            LogEndDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //            LogStartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //            LogStartDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
            //            LogVersionNo = table.Column<long>(type: "bigint", nullable: false),
            //            NotificationSubject = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            NumberGenerationType = table.Column<int>(type: "integer", nullable: false),
            //            OwnerUserText = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            PortalId = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            PostScript = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            PreScript = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            PrintButtonText = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            PriorityId = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            RecordId = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            RequestedByUserText = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            SLA = table.Column<TimeSpan>(type: "interval", nullable: true),
            //            SaveAsDraftCss = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            SaveAsDraftText = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            SequenceOrder = table.Column<long>(type: "bigint", nullable: true),
            //            ServiceIndexPageTemplateId = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            ServiceNoText = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            Status = table.Column<int>(type: "integer", nullable: false),
            //            Subject = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            SubjectText = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            SubmitButtonCss = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            SubmitButtonText = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            TemplateColor = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            TemplateId = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            UdfTemplateId = table.Column<string>(type: "text", nullable: true)
            //                .Annotation("Npgsql:DefaultColumnCollation", "cms_collation_ci"),
            //            VersionNo = table.Column<long>(type: "bigint", nullable: false)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_ServiceTemplateLog", x => x.Id);
            //            table.ForeignKey(
            //                name: "FK_ServiceTemplateLog_LOV_DefaultServiceOwnerTypeId",
            //                column: x => x.DefaultServiceOwnerTypeId,
            //                principalSchema: "public",
            //                principalTable: "LOV",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Restrict);
            //            table.ForeignKey(
            //                name: "FK_ServiceTemplateLog_LOV_DefaultServiceRequesterTypeId",
            //                column: x => x.DefaultServiceRequesterTypeId,
            //                principalSchema: "public",
            //                principalTable: "LOV",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Restrict);
            //            table.ForeignKey(
            //                name: "FK_ServiceTemplateLog_LOV_PriorityId",
            //                column: x => x.PriorityId,
            //                principalSchema: "public",
            //                principalTable: "LOV",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Restrict);
            //            table.ForeignKey(
            //                name: "FK_ServiceTemplateLog_ServiceIndexPageTemplate_ServiceIndexPag~",
            //                column: x => x.ServiceIndexPageTemplateId,
            //                principalSchema: "public",
            //                principalTable: "ServiceIndexPageTemplate",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Restrict);
            //            table.ForeignKey(
            //                name: "FK_ServiceTemplateLog_Team_DefaultOwnerTeamId",
            //                column: x => x.DefaultOwnerTeamId,
            //                principalSchema: "public",
            //                principalTable: "Team",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Restrict);
            //            table.ForeignKey(
            //                name: "FK_ServiceTemplateLog_Team_DefaultRequesterTeamId",
            //                column: x => x.DefaultRequesterTeamId,
            //                principalSchema: "public",
            //                principalTable: "Team",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Restrict);
            //            table.ForeignKey(
            //                name: "FK_ServiceTemplateLog_Template_TemplateId",
            //                column: x => x.TemplateId,
            //                principalSchema: "public",
            //                principalTable: "Template",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Restrict);
            //            table.ForeignKey(
            //                name: "FK_ServiceTemplateLog_Template_UdfTemplateId",
            //                column: x => x.UdfTemplateId,
            //                principalSchema: "public",
            //                principalTable: "Template",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Restrict);
            //            table.ForeignKey(
            //                name: "FK_ServiceTemplateLog_User_DefaultOwnerUserId",
            //                column: x => x.DefaultOwnerUserId,
            //                principalSchema: "public",
            //                principalTable: "User",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Restrict);
            //            table.ForeignKey(
            //                name: "FK_ServiceTemplateLog_User_DefaultRequesterUserId",
            //                column: x => x.DefaultRequesterUserId,
            //                principalSchema: "public",
            //                principalTable: "User",
            //                principalColumn: "Id",
            //                onDelete: ReferentialAction.Restrict);
            //        });

            //    migrationBuilder.CreateIndex(
            //        name: "IX_ServiceTemplateLog_DefaultOwnerTeamId",
            //        schema: "log",
            //        table: "ServiceTemplateLog",
            //        column: "DefaultOwnerTeamId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_ServiceTemplateLog_DefaultOwnerUserId",
            //        schema: "log",
            //        table: "ServiceTemplateLog",
            //        column: "DefaultOwnerUserId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_ServiceTemplateLog_DefaultRequesterTeamId",
            //        schema: "log",
            //        table: "ServiceTemplateLog",
            //        column: "DefaultRequesterTeamId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_ServiceTemplateLog_DefaultRequesterUserId",
            //        schema: "log",
            //        table: "ServiceTemplateLog",
            //        column: "DefaultRequesterUserId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_ServiceTemplateLog_DefaultServiceOwnerTypeId",
            //        schema: "log",
            //        table: "ServiceTemplateLog",
            //        column: "DefaultServiceOwnerTypeId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_ServiceTemplateLog_DefaultServiceRequesterTypeId",
            //        schema: "log",
            //        table: "ServiceTemplateLog",
            //        column: "DefaultServiceRequesterTypeId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_ServiceTemplateLog_PriorityId",
            //        schema: "log",
            //        table: "ServiceTemplateLog",
            //        column: "PriorityId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_ServiceTemplateLog_ServiceIndexPageTemplateId",
            //        schema: "log",
            //        table: "ServiceTemplateLog",
            //        column: "ServiceIndexPageTemplateId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_ServiceTemplateLog_TemplateId",
            //        schema: "log",
            //        table: "ServiceTemplateLog",
            //        column: "TemplateId");

            //    migrationBuilder.CreateIndex(
            //        name: "IX_ServiceTemplateLog_UdfTemplateId",
            //        schema: "log",
            //        table: "ServiceTemplateLog",
            //        column: "UdfTemplateId");
        }
    }
}
