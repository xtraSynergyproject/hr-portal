START TRANSACTION;

ALTER TABLE log."PortalLog" ADD "IconBgColor" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."PortalLog" ADD "IconCss" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."Portal" ADD "IconBgColor" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."Portal" ADD "IconCss" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220809165404_S_20220809_1', '5.0.2');

COMMIT;

