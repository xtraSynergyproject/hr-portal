START TRANSACTION;

ALTER TABLE log."FormTemplateLog" ADD "EnableLegalEntityFilter" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."FormTemplate" ADD "EnableLegalEntityFilter" boolean NOT NULL DEFAULT FALSE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20221105100730_N_20220511_2', '5.0.2');

COMMIT;