START TRANSACTION;

ALTER TABLE log."ServiceTemplateLog" ADD "ClosedWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."ServiceTemplateLog" ADD "CompletedWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ServiceTemplate" ADD "ClosedWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ServiceTemplate" ADD "CompletedWorkflowStatus" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220715032456_N_20220715_1', '5.0.2');

COMMIT;