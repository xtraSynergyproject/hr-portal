START TRANSACTION;

ALTER TABLE log."UserLog" ADD "NationalId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."User" ADD "NationalId" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20221003115424_N_20221003_1', '5.0.2');

COMMIT;