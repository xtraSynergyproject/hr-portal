START TRANSACTION;

ALTER TABLE log."PortalLog" ADD "IconFileId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."Portal" ADD "IconFileId" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220819091945_S_20220819_1', '5.0.2');

COMMIT;

