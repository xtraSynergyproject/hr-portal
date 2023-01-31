START TRANSACTION;

ALTER TABLE log."StepTaskComponentLog" ADD "EnableFifo" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE log."StepTaskComponentLog" ADD "TeamAssignmentType" integer NOT NULL DEFAULT 0;

ALTER TABLE public."StepTaskComponent" ADD "EnableFifo" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."StepTaskComponent" ADD "TeamAssignmentType" integer NOT NULL DEFAULT 0;

ALTER TABLE log."NtsTaskLog" ADD "EnableFifo" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."NtsTask" ADD "EnableFifo" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE log."NtsServiceLog" ADD "EnableFifo" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."NtsService" ADD "EnableFifo" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE log."ColumnMetadataLog" ADD "DisableForeignKey" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE log."ColumnMetadataLog" ADD "DontCreateTableColumn" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."ColumnMetadata" ADD "DisableForeignKey" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."ColumnMetadata" ADD "DontCreateTableColumn" boolean NOT NULL DEFAULT FALSE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220707163437_S_20220707_1', '5.0.2');

COMMIT;

