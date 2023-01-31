START TRANSACTION;

ALTER TABLE log."NotificationLog" ADD "IsStarred" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."Notification" ADD "IsStarred" boolean NOT NULL DEFAULT FALSE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220817103502_S_20220817_1', '5.0.2');

COMMIT;

