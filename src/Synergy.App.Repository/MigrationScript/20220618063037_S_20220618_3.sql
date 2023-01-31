START TRANSACTION;

ALTER TABLE log."StepTaskSkipLogicLog" ADD "StepTaskComponentId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."StepTaskSkipLogic" ADD "StepTaskComponentId" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."StepTaskAssigneeLogicLog" ADD "StepTaskComponentId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."StepTaskAssigneeLogic" ADD "StepTaskComponentId" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220618063037_S_20220618_3', '5.0.2');

COMMIT;

