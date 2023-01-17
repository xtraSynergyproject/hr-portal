START TRANSACTION;

ALTER TABLE log."HybridHierarchyLog" DROP COLUMN "HierarchyPath";

ALTER TABLE public."HybridHierarchy" DROP COLUMN "HierarchyPath";

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220301061347_S_20220301_1', '5.0.2');

COMMIT;

