START TRANSACTION;

ALTER TABLE public."BusinessRuleModel" ADD "DataJson" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220623021016_S_20220623_1', '5.0.2');

COMMIT;

