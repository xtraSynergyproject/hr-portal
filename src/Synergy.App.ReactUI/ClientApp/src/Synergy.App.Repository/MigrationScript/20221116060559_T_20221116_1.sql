START TRANSACTION;

ALTER TABLE log."NtsNoteLog" ADD "DmsAttachmentId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."NtsNote" ADD "DmsAttachmentId" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20221116060559_T_20221116_1', '5.0.2');

COMMIT;
