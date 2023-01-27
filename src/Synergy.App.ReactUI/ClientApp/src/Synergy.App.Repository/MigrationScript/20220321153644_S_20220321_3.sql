START TRANSACTION;

ALTER TABLE public."Page" ADD "DontShowInMainMenu" boolean NOT NULL DEFAULT FALSE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220321153644_S_20220321_3', '5.0.2');

COMMIT;

