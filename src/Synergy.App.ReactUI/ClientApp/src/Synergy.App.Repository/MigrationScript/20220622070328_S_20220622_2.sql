START TRANSACTION;

ALTER TABLE log."ServiceTemplateLog" ADD "EnablePreviewAcknowledgement" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE log."ServiceTemplateLog" ADD "PreviewAcknowledgementText" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ServiceTemplate" ADD "EnablePreviewAcknowledgement" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."ServiceTemplate" ADD "PreviewAcknowledgementText" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220622070328_S_20220622_2', '5.0.2');

COMMIT;

