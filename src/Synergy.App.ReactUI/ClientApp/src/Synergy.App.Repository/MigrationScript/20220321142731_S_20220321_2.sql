START TRANSACTION;

ALTER TABLE log."PortalLog" ADD "LicensePrivateKey" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."Portal" ADD "LicensePrivateKey" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220321142731_S_20220321_2', '5.0.2');

COMMIT;

