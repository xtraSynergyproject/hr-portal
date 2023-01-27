START TRANSACTION;

ALTER TABLE public."Page" ADD "IsPageRedirect" boolean NOT NULL DEFAULT FALSE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220127083123_S_20220127_1', '5.0.2');

COMMIT;

