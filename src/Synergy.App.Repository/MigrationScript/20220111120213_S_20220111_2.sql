START TRANSACTION;

ALTER TABLE public."CompanySetting" ADD "GroupCode" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220111120213_S_20220111_2', '5.0.2');

COMMIT;

