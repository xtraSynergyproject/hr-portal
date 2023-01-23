START TRANSACTION;

CREATE TABLE public."StepTaskEscalationData" (
    "Id" text COLLATE cms_collation_ci NOT NULL,
    "StepTaskComponentId" text COLLATE cms_collation_ci NULL,
    "NtsTaskId" text COLLATE cms_collation_ci NULL,
    "NtsServiceId" text COLLATE cms_collation_ci NULL,
    "EscalatedToUserId" text COLLATE cms_collation_ci NULL,
    "EscalatedDate" timestamp without time zone NOT NULL,
    "EscalationComment" text COLLATE cms_collation_ci NULL,
    "CreatedDate" timestamp without time zone NOT NULL,
    "CreatedBy" text COLLATE cms_collation_ci NULL,
    "LastUpdatedDate" timestamp without time zone NOT NULL,
    "LastUpdatedBy" text COLLATE cms_collation_ci NULL,
    "IsDeleted" boolean NOT NULL,
    "SequenceOrder" bigint NULL,
    "CompanyId" text COLLATE cms_collation_ci NULL,
    "LegalEntityId" text COLLATE cms_collation_ci NULL,
    "Status" integer NOT NULL,
    "VersionNo" bigint NOT NULL,
    "PortalId" text COLLATE cms_collation_ci NULL,
    CONSTRAINT "PK_StepTaskEscalationData" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_StepTaskEscalationData_NtsService_NtsServiceId" FOREIGN KEY ("NtsServiceId") REFERENCES public."NtsService" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StepTaskEscalationData_NtsTask_NtsTaskId" FOREIGN KEY ("NtsTaskId") REFERENCES public."NtsTask" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StepTaskEscalationData_StepTaskComponent_StepTaskComponentId" FOREIGN KEY ("StepTaskComponentId") REFERENCES public."StepTaskComponent" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StepTaskEscalationData_User_EscalatedToUserId" FOREIGN KEY ("EscalatedToUserId") REFERENCES public."User" ("Id") ON DELETE RESTRICT
);

CREATE TABLE log."StepTaskEscalationDataLog" (
    "Id" text COLLATE cms_collation_ci NOT NULL,
    "RecordId" text COLLATE cms_collation_ci NULL,
    "LogVersionNo" bigint NOT NULL,
    "IsLatest" boolean NOT NULL,
    "LogStartDate" timestamp without time zone NOT NULL,
    "LogEndDate" timestamp without time zone NOT NULL,
    "LogStartDateTime" timestamp without time zone NOT NULL,
    "LogEndDateTime" timestamp without time zone NOT NULL,
    "IsDatedLatest" boolean NOT NULL,
    "IsVersionLatest" boolean NOT NULL,
    "CreatedDate" timestamp without time zone NOT NULL,
    "CreatedBy" text COLLATE cms_collation_ci NULL,
    "LastUpdatedDate" timestamp without time zone NOT NULL,
    "LastUpdatedBy" text COLLATE cms_collation_ci NULL,
    "IsDeleted" boolean NOT NULL,
    "SequenceOrder" bigint NULL,
    "CompanyId" text COLLATE cms_collation_ci NULL,
    "LegalEntityId" text COLLATE cms_collation_ci NULL,
    "DataAction" integer NOT NULL,
    "Status" integer NOT NULL,
    "VersionNo" bigint NOT NULL,
    "PortalId" text COLLATE cms_collation_ci NULL,
    "StepTaskComponentId" text COLLATE cms_collation_ci NULL,
    "NtsTaskId" text COLLATE cms_collation_ci NULL,
    "NtsServiceId" text COLLATE cms_collation_ci NULL,
    "EscalatedToUserId" text COLLATE cms_collation_ci NULL,
    "EscalatedDate" timestamp without time zone NOT NULL,
    "EscalationComment" text COLLATE cms_collation_ci NULL,
    CONSTRAINT "PK_StepTaskEscalationDataLog" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_StepTaskEscalationDataLog_NtsService_NtsServiceId" FOREIGN KEY ("NtsServiceId") REFERENCES public."NtsService" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StepTaskEscalationDataLog_NtsTask_NtsTaskId" FOREIGN KEY ("NtsTaskId") REFERENCES public."NtsTask" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StepTaskEscalationDataLog_StepTaskComponent_StepTaskCompone~" FOREIGN KEY ("StepTaskComponentId") REFERENCES public."StepTaskComponent" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StepTaskEscalationDataLog_User_EscalatedToUserId" FOREIGN KEY ("EscalatedToUserId") REFERENCES public."User" ("Id") ON DELETE RESTRICT
);

CREATE INDEX "IX_StepTaskEscalationData_EscalatedToUserId" ON public."StepTaskEscalationData" ("EscalatedToUserId");

CREATE INDEX "IX_StepTaskEscalationData_NtsServiceId" ON public."StepTaskEscalationData" ("NtsServiceId");

CREATE INDEX "IX_StepTaskEscalationData_NtsTaskId" ON public."StepTaskEscalationData" ("NtsTaskId");

CREATE INDEX "IX_StepTaskEscalationData_StepTaskComponentId" ON public."StepTaskEscalationData" ("StepTaskComponentId");

CREATE INDEX "IX_StepTaskEscalationDataLog_EscalatedToUserId" ON log."StepTaskEscalationDataLog" ("EscalatedToUserId");

CREATE INDEX "IX_StepTaskEscalationDataLog_NtsServiceId" ON log."StepTaskEscalationDataLog" ("NtsServiceId");

CREATE INDEX "IX_StepTaskEscalationDataLog_NtsTaskId" ON log."StepTaskEscalationDataLog" ("NtsTaskId");

CREATE INDEX "IX_StepTaskEscalationDataLog_StepTaskComponentId" ON log."StepTaskEscalationDataLog" ("StepTaskComponentId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220906081932_S_20220906_1', '5.0.2');

COMMIT;

