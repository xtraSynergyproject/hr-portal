START TRANSACTION;

ALTER TABLE log."NotificationLog" ADD "TriggeredByReferenceType" integer NOT NULL DEFAULT 0;

ALTER TABLE log."NotificationLog" ADD "TriggeredByReferenceTypeId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."Notification" ADD "TriggeredByReferenceType" integer NOT NULL DEFAULT 0;

ALTER TABLE public."Notification" ADD "TriggeredByReferenceTypeId" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220904062733_S_20220904_1', '5.0.2');

COMMIT;