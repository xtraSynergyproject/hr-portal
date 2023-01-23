START TRANSACTION;

ALTER TABLE log."StepTaskComponentLog" ADD "CanceledWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."StepTaskComponentLog" ADD "DraftWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."StepTaskComponentLog" ADD "InprogressWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."StepTaskComponentLog" ADD "OverdueWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."StepTaskComponentLog" ADD "RejectedWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."StepTaskComponent" ADD "CanceledWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."StepTaskComponent" ADD "DraftWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."StepTaskComponent" ADD "InprogressWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."StepTaskComponent" ADD "OverdueWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."StepTaskComponent" ADD "RejectedWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."NtsServiceLog" ADD "CanceledWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."NtsServiceLog" ADD "DraftWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."NtsServiceLog" ADD "InprogressWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."NtsServiceLog" ADD "OverdueWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."NtsServiceLog" ADD "RejectedWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."NtsServiceLog" ADD "ReturnedWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."NtsService" ADD "CanceledWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."NtsService" ADD "DraftWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."NtsService" ADD "InprogressWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."NtsService" ADD "OverdueWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."NtsService" ADD "RejectedWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."NtsService" ADD "ReturnedWorkflowStatus" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220713095823_S_20220713_1', '5.0.2');

COMMIT;

