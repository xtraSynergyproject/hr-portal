START TRANSACTION;

ALTER TABLE log."PortalLog" ADD "LicenseKey" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."Portal" ADD "LicenseKey" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220321124535_S_20220321_1', '5.0.2');

COMMIT;

