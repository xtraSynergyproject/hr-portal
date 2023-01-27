START TRANSACTION;

ALTER TABLE log."NtsServiceLog" DROP COLUMN "CanceledWorkflowStatus";

ALTER TABLE log."NtsServiceLog" DROP COLUMN "DraftWorkflowStatus";

ALTER TABLE log."NtsServiceLog" DROP COLUMN "EnableFifo";

ALTER TABLE log."NtsServiceLog" DROP COLUMN "InprogressWorkflowStatus";

ALTER TABLE log."NtsServiceLog" DROP COLUMN "OverdueWorkflowStatus";

ALTER TABLE log."NtsServiceLog" DROP COLUMN "RejectedWorkflowStatus";

ALTER TABLE log."NtsServiceLog" DROP COLUMN "ReturnedWorkflowStatus";

ALTER TABLE public."NtsService" DROP COLUMN "CanceledWorkflowStatus";

ALTER TABLE public."NtsService" DROP COLUMN "DraftWorkflowStatus";

ALTER TABLE public."NtsService" DROP COLUMN "EnableFifo";

ALTER TABLE public."NtsService" DROP COLUMN "InprogressWorkflowStatus";

ALTER TABLE public."NtsService" DROP COLUMN "OverdueWorkflowStatus";

ALTER TABLE public."NtsService" DROP COLUMN "RejectedWorkflowStatus";

ALTER TABLE public."NtsService" DROP COLUMN "ReturnedWorkflowStatus";

ALTER TABLE log."ServiceTemplateLog" ADD "CanceledWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."ServiceTemplateLog" ADD "DraftWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."ServiceTemplateLog" ADD "EnableFifo" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE log."ServiceTemplateLog" ADD "InprogressWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."ServiceTemplateLog" ADD "OverdueWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."ServiceTemplateLog" ADD "RejectedWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."ServiceTemplateLog" ADD "ReturnedWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ServiceTemplate" ADD "CanceledWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ServiceTemplate" ADD "DraftWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ServiceTemplate" ADD "EnableFifo" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."ServiceTemplate" ADD "InprogressWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ServiceTemplate" ADD "OverdueWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ServiceTemplate" ADD "RejectedWorkflowStatus" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ServiceTemplate" ADD "ReturnedWorkflowStatus" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220714102450_S_20220714_1', '5.0.2');

COMMIT;

