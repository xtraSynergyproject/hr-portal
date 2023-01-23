START TRANSACTION;

ALTER TABLE log."StepTaskEscalationDataLog" ADD "StepTaskEscalationId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."StepTaskEscalationData" ADD "StepTaskEscalationId" text COLLATE cms_collation_ci NULL;

CREATE INDEX "IX_StepTaskEscalationDataLog_StepTaskEscalationId" ON log."StepTaskEscalationDataLog" ("StepTaskEscalationId");

CREATE INDEX "IX_StepTaskEscalationData_StepTaskEscalationId" ON public."StepTaskEscalationData" ("StepTaskEscalationId");

ALTER TABLE public."StepTaskEscalationData" ADD CONSTRAINT "FK_StepTaskEscalationData_StepTaskEscalation_StepTaskEscalatio~" FOREIGN KEY ("StepTaskEscalationId") REFERENCES public."StepTaskEscalation" ("Id") ON DELETE RESTRICT;

ALTER TABLE log."StepTaskEscalationDataLog" ADD CONSTRAINT "FK_StepTaskEscalationDataLog_StepTaskEscalation_StepTaskEscala~" FOREIGN KEY ("StepTaskEscalationId") REFERENCES public."StepTaskEscalation" ("Id") ON DELETE RESTRICT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220914140918_N_20220914_1', '5.0.2');

COMMIT;