START TRANSACTION;

ALTER TABLE log."NtsTaskSequenceLog" ADD "TemplateId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."NtsTaskSequence" ADD "TemplateId" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."NtsServiceSequenceLog" ADD "TemplateId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."NtsServiceSequence" ADD "TemplateId" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."NtsNoteSequenceLog" ADD "TemplateId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."NtsNoteSequence" ADD "TemplateId" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20221018100041_S_20221018_1', '5.0.2');

COMMIT;
