START TRANSACTION;

ALTER TABLE log."StepTaskAssigneeLogicLog" ADD "TeamAssignmentType" integer NOT NULL DEFAULT 0;

ALTER TABLE public."StepTaskAssigneeLogic" ADD "TeamAssignmentType" integer NOT NULL DEFAULT 0;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220712071959_S_20220712_1', '5.0.2');

COMMIT;