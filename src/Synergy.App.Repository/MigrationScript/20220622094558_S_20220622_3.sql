START TRANSACTION;

ALTER TABLE log."UserLog" ADD "Address" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."User" ADD "Address" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220622094558_S_20220622_3', '5.0.2');

COMMIT;

