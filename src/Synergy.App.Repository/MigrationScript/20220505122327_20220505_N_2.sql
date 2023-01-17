START TRANSACTION;

ALTER TABLE public."LOV" ADD "ReferenceId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."LOV" ADD "ReferenceType" integer NOT NULL DEFAULT 0;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220505122327_20220505_N_2', '5.0.2');

COMMIT;