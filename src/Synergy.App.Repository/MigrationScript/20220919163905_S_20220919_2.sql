START TRANSACTION;

ALTER TABLE log."ServiceTemplateLog" ADD "RootServiceCode" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ServiceTemplate" ADD "RootServiceCode" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220919163905_S_20220919_2', '5.0.2');

COMMIT;

