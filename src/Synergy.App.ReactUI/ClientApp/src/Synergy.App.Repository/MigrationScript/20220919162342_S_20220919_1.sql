START TRANSACTION;

ALTER TABLE log."ServiceTemplateLog" ADD "ParentServiceCodes" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ServiceTemplate" ADD "ParentServiceCodes" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220919162342_S_20220919_1', '5.0.2');

COMMIT;

