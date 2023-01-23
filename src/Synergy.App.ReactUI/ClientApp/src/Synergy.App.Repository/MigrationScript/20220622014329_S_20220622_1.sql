START TRANSACTION;

ALTER TABLE public."ServiceTemplate" DROP CONSTRAINT "FK_ServiceTemplate_ColumnMetadata_LocalizedColumnId";

ALTER TABLE log."ServiceTemplateLog" DROP CONSTRAINT "FK_ServiceTemplateLog_ColumnMetadata_LocalizedColumnId";

DROP INDEX log."IX_ServiceTemplateLog_LocalizedColumnId";

DROP INDEX public."IX_ServiceTemplate_LocalizedColumnId";

ALTER TABLE log."ServiceTemplateLog" RENAME COLUMN "LocalizedColumnId" TO "PostSubmitPageParams";

ALTER TABLE public."ServiceTemplate" RENAME COLUMN "LocalizedColumnId" TO "PostSubmitPageParams";

ALTER TABLE log."ServiceTemplateLog" ADD "EnablePostSubmitPage" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE log."ServiceTemplateLog" ADD "PostSubmitPageAction" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."ServiceTemplateLog" ADD "PostSubmitPageArea" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."ServiceTemplateLog" ADD "PostSubmitPageController" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ServiceTemplate" ADD "EnablePostSubmitPage" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."ServiceTemplate" ADD "PostSubmitPageAction" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ServiceTemplate" ADD "PostSubmitPageArea" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ServiceTemplate" ADD "PostSubmitPageController" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220622014329_S_20220622_1', '5.0.2');

COMMIT;

