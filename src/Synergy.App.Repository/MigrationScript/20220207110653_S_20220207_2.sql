START TRANSACTION;

ALTER TABLE log."HybridHierarchyLog" ADD "BulkRequestId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."HybridHierarchy" ADD "BulkRequestId" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220207110653_S_20220207_2', '5.0.2');

COMMIT;

