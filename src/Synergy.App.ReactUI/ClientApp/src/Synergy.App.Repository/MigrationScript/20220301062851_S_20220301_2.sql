START TRANSACTION;

ALTER TABLE log."HybridHierarchyLog" ADD "HierarchyPath" text[] NULL;

ALTER TABLE public."HybridHierarchy" ADD "HierarchyPath" text[] NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220301062851_S_20220301_2', '5.0.2');

COMMIT;

