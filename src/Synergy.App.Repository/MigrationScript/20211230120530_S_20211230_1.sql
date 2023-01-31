START TRANSACTION;

ALTER TABLE log."TaskTemplateLog" ADD "DescriptionUdfMappingColumn" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."TaskTemplateLog" ADD "SubjectUdfMappingColumn" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."TaskTemplate" ADD "DescriptionUdfMappingColumn" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."TaskTemplate" ADD "SubjectUdfMappingColumn" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."ServiceTemplateLog" ADD "DescriptionUdfMappingColumn" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."ServiceTemplateLog" ADD "SubjectUdfMappingColumn" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ServiceTemplate" ADD "DescriptionUdfMappingColumn" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ServiceTemplate" ADD "SubjectUdfMappingColumn" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."NoteTemplateLog" ADD "DescriptionUdfMappingColumn" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."NoteTemplateLog" ADD "SubjectUdfMappingColumn" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."NoteTemplate" ADD "DescriptionUdfMappingColumn" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."NoteTemplate" ADD "SubjectUdfMappingColumn" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20211230120530_S_20211230_1', '5.0.2');

COMMIT;

