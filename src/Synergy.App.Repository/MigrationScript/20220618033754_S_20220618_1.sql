START TRANSACTION;

ALTER TABLE log."StepTaskComponentLog" RENAME COLUMN "OpenSameTaskOnServiceSubmit" TO "OpenSameTaskOnServiceReSubmit";

ALTER TABLE public."StepTaskComponent" ADD "OpenSameTaskOnServiceReSubmit" boolean NOT NULL DEFAULT FALSE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220618033754_S_20220618_1', '5.0.2');

COMMIT;

