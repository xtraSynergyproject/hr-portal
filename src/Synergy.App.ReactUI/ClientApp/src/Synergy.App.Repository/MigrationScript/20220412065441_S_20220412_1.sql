START TRANSACTION;

ALTER TABLE log."StepTaskComponentLog" ADD "WorkflowVisibility" integer NOT NULL DEFAULT 0;

ALTER TABLE public."StepTaskComponent" ADD "WorkflowVisibility" integer NOT NULL DEFAULT 0;

ALTER TABLE log."ServiceTemplateLog" ADD "WorkflowVisibility" integer NOT NULL DEFAULT 0;

ALTER TABLE public."ServiceTemplate" ADD "WorkflowVisibility" integer NOT NULL DEFAULT 0;

ALTER TABLE log."PortalLog" ADD "HideMainMenu" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."Portal" ADD "HideMainMenu" boolean NOT NULL DEFAULT FALSE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220412065441_S_20220412_1', '5.0.2');

COMMIT;

