START TRANSACTION;

ALTER TABLE log."ColumnMetadataLog" ADD "EnableLanguageValidation" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE log."ColumnMetadataLog" ADD "EnableLocalization" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."ColumnMetadata" ADD "EnableLanguageValidation" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."ColumnMetadata" ADD "EnableLocalization" boolean NOT NULL DEFAULT FALSE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220618070000_S_20220618_4', '5.0.2');

COMMIT;

