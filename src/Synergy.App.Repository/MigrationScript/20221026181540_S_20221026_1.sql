START TRANSACTION;

ALTER TABLE log."StepTaskComponentLog" ADD "RuntimeWorkflowButtonText" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."StepTaskComponentLog" ADD "RuntimeWorkflowMandatory" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."StepTaskComponent" ADD "RuntimeWorkflowButtonText" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."StepTaskComponent" ADD "RuntimeWorkflowMandatory" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE log."ServiceTemplateLog" ADD "RuntimeWorkflowButtonText" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."ServiceTemplateLog" ADD "RuntimeWorkflowMandatory" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."ServiceTemplate" ADD "RuntimeWorkflowButtonText" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ServiceTemplate" ADD "RuntimeWorkflowMandatory" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE log."NtsTaskLog" ADD "RuntimeWorkflowDataId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."NtsTask" ADD "RuntimeWorkflowDataId" text COLLATE cms_collation_ci NULL;

CREATE INDEX "IX_NtsTaskLog_RuntimeWorkflowDataId" ON log."NtsTaskLog" ("RuntimeWorkflowDataId");

CREATE INDEX "IX_NtsTask_RuntimeWorkflowDataId" ON public."NtsTask" ("RuntimeWorkflowDataId");

ALTER TABLE public."NtsTask" ADD CONSTRAINT "FK_NtsTask_RuntimeWorkflowData_RuntimeWorkflowDataId" FOREIGN KEY ("RuntimeWorkflowDataId") REFERENCES public."RuntimeWorkflowData" ("Id") ON DELETE RESTRICT;

ALTER TABLE log."NtsTaskLog" ADD CONSTRAINT "FK_NtsTaskLog_RuntimeWorkflowData_RuntimeWorkflowDataId" FOREIGN KEY ("RuntimeWorkflowDataId") REFERENCES public."RuntimeWorkflowData" ("Id") ON DELETE RESTRICT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20221020065929_S_20221020_1', '5.0.2');

COMMIT;

START TRANSACTION;

ALTER TABLE log."ComponentResultLog" ADD "RuntimeWorkflowDataId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ComponentResult" ADD "RuntimeWorkflowDataId" text COLLATE cms_collation_ci NULL;

CREATE INDEX "IX_ComponentResultLog_RuntimeWorkflowDataId" ON log."ComponentResultLog" ("RuntimeWorkflowDataId");

CREATE INDEX "IX_ComponentResult_RuntimeWorkflowDataId" ON public."ComponentResult" ("RuntimeWorkflowDataId");

ALTER TABLE public."ComponentResult" ADD CONSTRAINT "FK_ComponentResult_RuntimeWorkflowData_RuntimeWorkflowDataId" FOREIGN KEY ("RuntimeWorkflowDataId") REFERENCES public."RuntimeWorkflowData" ("Id") ON DELETE RESTRICT;

ALTER TABLE log."ComponentResultLog" ADD CONSTRAINT "FK_ComponentResultLog_RuntimeWorkflowData_RuntimeWorkflowDataId" FOREIGN KEY ("RuntimeWorkflowDataId") REFERENCES public."RuntimeWorkflowData" ("Id") ON DELETE RESTRICT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20221026181540_S_20221026_1', '5.0.2');

COMMIT;

