START TRANSACTION;

ALTER TABLE log."RuntimeWorkflowDataLog" ALTER COLUMN "TeamAssignmentType" DROP NOT NULL;

ALTER TABLE log."RuntimeWorkflowDataLog" ALTER COLUMN "AssignedToHierarchyMasterLevelId" DROP NOT NULL;

ALTER TABLE public."RuntimeWorkflowData" ALTER COLUMN "TeamAssignmentType" DROP NOT NULL;

ALTER TABLE public."RuntimeWorkflowData" ALTER COLUMN "AssignedToHierarchyMasterLevelId" DROP NOT NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20221017070516_20221017_N_1', '5.0.2');

COMMIT;