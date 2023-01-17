START TRANSACTION;

ALTER TABLE log."LOVLog" ADD "ReferenceId" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."LOVLog" ADD "ReferenceType" integer NOT NULL DEFAULT 0;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220505115420_20220505_N_1', '5.0.2');

COMMIT;