START TRANSACTION;

ALTER TABLE public."Template" ADD "GroupCode" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."LOVLog" ADD "NameArabic" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."LOVLog" ADD "NameHindi" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."LOV" ADD "NameArabic" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."LOV" ADD "NameHindi" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220617125158_S_20220617_2', '5.0.2');

COMMIT;

