START TRANSACTION;

ALTER TABLE log."TemplateCategoryLog" ADD "AllowedPortalIds" text[] NULL;

ALTER TABLE public."TemplateCategory" ADD "AllowedPortalIds" text[] NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220325052251_S_20220325_1', '5.0.2');

COMMIT;

