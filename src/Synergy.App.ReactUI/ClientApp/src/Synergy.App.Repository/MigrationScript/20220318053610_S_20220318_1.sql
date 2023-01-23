START TRANSACTION;

ALTER TABLE public."Page" ADD "GroupCode" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."MenuGroupLog" ADD "GroupCode" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."MenuGroup" ADD "GroupCode" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220318053610_S_20220318_1', '5.0.2');

COMMIT;

