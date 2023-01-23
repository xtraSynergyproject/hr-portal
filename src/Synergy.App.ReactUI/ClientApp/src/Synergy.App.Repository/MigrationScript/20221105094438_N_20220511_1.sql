START TRANSACTION;

ALTER TABLE log."LegalEntityLog" ADD "Latitude" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."LegalEntityLog" ADD "Longitude" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."LegalEntityLog" ADD "Street" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."LegalEntity" ADD "Latitude" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."LegalEntity" ADD "Longitude" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."LegalEntity" ADD "Street" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20221105094438_N_20220511_1', '5.0.2');

COMMIT;

