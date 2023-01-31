START TRANSACTION;

ALTER TABLE public."ResourceLanguage" ADD "French" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ResourceLanguage" ADD "Spanish" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."NoteResourceLanguageLog" ADD "French" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."NoteResourceLanguageLog" ADD "Spanish" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."NoteResourceLanguage" ADD "French" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."NoteResourceLanguage" ADD "Spanish" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."LOVLog" ADD "NameFrench" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."LOVLog" ADD "NameSpanish" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."LOV" ADD "NameFrench" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."LOV" ADD "NameSpanish" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."FormResourceLanguageLog" ADD "French" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."FormResourceLanguageLog" ADD "Spanish" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."FormResourceLanguage" ADD "French" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."FormResourceLanguage" ADD "Spanish" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20221205122342_S_20221205_1', '5.0.2');

COMMIT;

