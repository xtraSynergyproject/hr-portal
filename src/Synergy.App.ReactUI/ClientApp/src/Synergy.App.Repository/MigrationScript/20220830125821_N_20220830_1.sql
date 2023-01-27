START TRANSACTION;

ALTER TABLE log."TaskTemplateLog" ADD "FormClientScript" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."TaskTemplate" ADD "FormClientScript" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."ServiceTemplateLog" ADD "FormClientScript" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ServiceTemplate" ADD "FormClientScript" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."NoteTemplateLog" ADD "FormClientScript" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."NoteTemplate" ADD "FormClientScript" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220830125821_N_20220830_1', '5.0.2');

COMMIT;

