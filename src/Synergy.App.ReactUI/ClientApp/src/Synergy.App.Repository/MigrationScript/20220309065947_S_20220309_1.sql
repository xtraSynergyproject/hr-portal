START TRANSACTION;

ALTER TABLE public."ResourceLanguage" ADD "Hindi" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ResourceLanguage" ADD "HindiHelperText" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ResourceLanguage" ADD "HindiTooltip" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220309065947_S_20220309_1', '5.0.2');

COMMIT;

