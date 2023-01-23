START TRANSACTION;

ALTER TABLE log."LegalEntityLog" ADD "CityName" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."LegalEntityLog" ADD "GSTNo" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."LegalEntityLog" ADD "MobileNo" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."LegalEntityLog" ADD "PANNo" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."LegalEntityLog" ADD "PinCode" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."LegalEntityLog" ADD "StateName" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."LegalEntityLog" ADD "TANNo" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."LegalEntity" ADD "CityName" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."LegalEntity" ADD "GSTNo" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."LegalEntity" ADD "MobileNo" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."LegalEntity" ADD "PANNo" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."LegalEntity" ADD "PinCode" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."LegalEntity" ADD "StateName" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."LegalEntity" ADD "TANNo" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220404115618_S_20220403_1', '5.0.2');

COMMIT;

