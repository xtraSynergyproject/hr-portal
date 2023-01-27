START TRANSACTION;

ALTER TABLE log."NtsServiceLog" ADD "TriggeredByReferenceId" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."NtsServiceLog" ADD "TriggeredByReferenceType" integer NULL;

ALTER TABLE public."NtsService" ADD "TriggeredByReferenceId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."NtsService" ADD "TriggeredByReferenceType" integer NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220919151137_N_20220919_2', '5.0.2');

COMMIT;