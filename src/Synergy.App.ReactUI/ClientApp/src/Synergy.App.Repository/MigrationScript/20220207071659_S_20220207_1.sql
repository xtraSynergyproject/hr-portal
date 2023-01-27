START TRANSACTION;

ALTER TABLE log."HybridHierarchyLog" ADD "HierarchyPath" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."HybridHierarchy" ADD "HierarchyPath" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220207071659_S_20220207_1', '5.0.2');

COMMIT;

