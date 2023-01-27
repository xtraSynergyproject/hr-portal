START TRANSACTION;

ALTER TABLE log."NtsCategoryLog" ADD "DisplayName" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."NtsCategory" ADD "DisplayName" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."CustomTemplateLog" ADD "AllowedCategories" text[] NULL;

ALTER TABLE log."CustomTemplateLog" ADD "AllowedTemplates" text[] NULL;

ALTER TABLE public."CustomTemplate" ADD "AllowedCategories" text[] NULL;

ALTER TABLE public."CustomTemplate" ADD "AllowedTemplates" text[] NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220823095430_S_20220823_1', '5.0.2');

COMMIT;

