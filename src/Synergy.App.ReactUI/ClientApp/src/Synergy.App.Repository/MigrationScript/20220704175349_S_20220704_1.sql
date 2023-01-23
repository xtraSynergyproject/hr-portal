START TRANSACTION;

ALTER TABLE log."ColumnMetadataLog" ADD "ShowInBusinessLogic" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."ColumnMetadata" ADD "ShowInBusinessLogic" boolean NOT NULL DEFAULT FALSE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220704175349_S_20220704_1', '5.0.2');

COMMIT;

