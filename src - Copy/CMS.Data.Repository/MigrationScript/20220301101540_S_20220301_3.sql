START TRANSACTION;

ALTER TABLE log."UserHierarchyPermissionLog" ADD "CustomRootId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."UserHierarchyPermission" ADD "CustomRootId" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220301101540_S_20220301_3', '5.0.2');

COMMIT;

