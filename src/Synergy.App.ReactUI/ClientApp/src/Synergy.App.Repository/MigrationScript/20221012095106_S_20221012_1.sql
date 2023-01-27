START TRANSACTION;

ALTER TABLE log."StepTaskComponentLog" ADD "EnableRuntimeWorkflow" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE log."StepTaskComponentLog" ADD "IsRuntimeComponent" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE log."StepTaskComponentLog" ADD "RuntimeAssigneeTeamListUrl" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."StepTaskComponentLog" ADD "RuntimeAssigneeUserListUrl" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."StepTaskComponent" ADD "EnableRuntimeWorkflow" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."StepTaskComponent" ADD "IsRuntimeComponent" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."StepTaskComponent" ADD "RuntimeAssigneeTeamListUrl" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."StepTaskComponent" ADD "RuntimeAssigneeUserListUrl" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."ServiceTemplateLog" ADD "EnableRuntimeWorkflow" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE log."ServiceTemplateLog" ADD "RuntimeAssigneeTeamListUrl" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."ServiceTemplateLog" ADD "RuntimeAssigneeUserListUrl" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ServiceTemplate" ADD "EnableRuntimeWorkflow" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."ServiceTemplate" ADD "RuntimeAssigneeTeamListUrl" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ServiceTemplate" ADD "RuntimeAssigneeUserListUrl" text COLLATE cms_collation_ci NULL;

CREATE TABLE public."RuntimeWorkflow" (
    "Id" text COLLATE cms_collation_ci NOT NULL,
    "RuntimeWorkflowSourceTemplateId" text COLLATE cms_collation_ci NULL,
    "SourceServiceId" text COLLATE cms_collation_ci NULL,
    "SourceTaskId" text COLLATE cms_collation_ci NULL,
    "RuntimeWorkflowExecutionMode" integer NOT NULL,
    "TriggeringStepTaskComponentId" text COLLATE cms_collation_ci NULL,
    "TriggeringTemplateId" text COLLATE cms_collation_ci NULL,
    "TriggeringComponentId" text COLLATE cms_collation_ci NULL,
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
    CONSTRAINT "PK_RuntimeWorkflow" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_RuntimeWorkflow_Component_TriggeringComponentId" FOREIGN KEY ("TriggeringComponentId") REFERENCES public."Component" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_RuntimeWorkflow_StepTaskComponent_TriggeringStepTaskCompone~" FOREIGN KEY ("TriggeringStepTaskComponentId") REFERENCES public."StepTaskComponent" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_RuntimeWorkflow_Template_RuntimeWorkflowSourceTemplateId" FOREIGN KEY ("RuntimeWorkflowSourceTemplateId") REFERENCES public."Template" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_RuntimeWorkflow_Template_TriggeringTemplateId" FOREIGN KEY ("TriggeringTemplateId") REFERENCES public."Template" ("Id") ON DELETE RESTRICT
);

CREATE TABLE log."RuntimeWorkflowLog" (
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
    "RuntimeWorkflowSourceTemplateId" text COLLATE cms_collation_ci NULL,
    "SourceServiceId" text COLLATE cms_collation_ci NULL,
    "SourceTaskId" text COLLATE cms_collation_ci NULL,
    "RuntimeWorkflowExecutionMode" integer NOT NULL,
    "TriggeringStepTaskComponentId" text COLLATE cms_collation_ci NULL,
    "TriggeringTemplateId" text COLLATE cms_collation_ci NULL,
    "TriggeringComponentId" text COLLATE cms_collation_ci NULL,
    CONSTRAINT "PK_RuntimeWorkflowLog" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_RuntimeWorkflowLog_Component_TriggeringComponentId" FOREIGN KEY ("TriggeringComponentId") REFERENCES public."Component" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_RuntimeWorkflowLog_StepTaskComponent_TriggeringStepTaskComp~" FOREIGN KEY ("TriggeringStepTaskComponentId") REFERENCES public."StepTaskComponent" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_RuntimeWorkflowLog_Template_RuntimeWorkflowSourceTemplateId" FOREIGN KEY ("RuntimeWorkflowSourceTemplateId") REFERENCES public."Template" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_RuntimeWorkflowLog_Template_TriggeringTemplateId" FOREIGN KEY ("TriggeringTemplateId") REFERENCES public."Template" ("Id") ON DELETE RESTRICT
);

CREATE TABLE public."RuntimeWorkflowData" (
    "Id" text COLLATE cms_collation_ci NOT NULL,
    "RuntimeWorkflowId" text COLLATE cms_collation_ci NULL,
    "AssignedToTypeId" text COLLATE cms_collation_ci NULL,
    "AssignedToUserId" text COLLATE cms_collation_ci NULL,
    "AssignedToTeamId" text COLLATE cms_collation_ci NULL,
    "TeamAssignmentType" integer NOT NULL,
    "AssignedToHierarchyMasterId" text COLLATE cms_collation_ci NULL,
    "AssignedToHierarchyMasterLevelId" integer NOT NULL,
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
    CONSTRAINT "PK_RuntimeWorkflowData" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_RuntimeWorkflowData_HierarchyMaster_AssignedToHierarchyMast~" FOREIGN KEY ("AssignedToHierarchyMasterId") REFERENCES public."HierarchyMaster" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_RuntimeWorkflowData_LOV_AssignedToTypeId" FOREIGN KEY ("AssignedToTypeId") REFERENCES public."LOV" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_RuntimeWorkflowData_RuntimeWorkflow_RuntimeWorkflowId" FOREIGN KEY ("RuntimeWorkflowId") REFERENCES public."RuntimeWorkflow" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_RuntimeWorkflowData_Team_AssignedToTeamId" FOREIGN KEY ("AssignedToTeamId") REFERENCES public."Team" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_RuntimeWorkflowData_User_AssignedToUserId" FOREIGN KEY ("AssignedToUserId") REFERENCES public."User" ("Id") ON DELETE RESTRICT
);

CREATE TABLE log."RuntimeWorkflowDataLog" (
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
    "RuntimeWorkflowId" text COLLATE cms_collation_ci NULL,
    "AssignedToTypeId" text COLLATE cms_collation_ci NULL,
    "AssignedToUserId" text COLLATE cms_collation_ci NULL,
    "AssignedToTeamId" text COLLATE cms_collation_ci NULL,
    "TeamAssignmentType" integer NOT NULL,
    "AssignedToHierarchyMasterId" text COLLATE cms_collation_ci NULL,
    "AssignedToHierarchyMasterLevelId" integer NOT NULL,
    CONSTRAINT "PK_RuntimeWorkflowDataLog" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_RuntimeWorkflowDataLog_HierarchyMaster_AssignedToHierarchyM~" FOREIGN KEY ("AssignedToHierarchyMasterId") REFERENCES public."HierarchyMaster" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_RuntimeWorkflowDataLog_LOV_AssignedToTypeId" FOREIGN KEY ("AssignedToTypeId") REFERENCES public."LOV" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_RuntimeWorkflowDataLog_RuntimeWorkflow_RuntimeWorkflowId" FOREIGN KEY ("RuntimeWorkflowId") REFERENCES public."RuntimeWorkflow" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_RuntimeWorkflowDataLog_Team_AssignedToTeamId" FOREIGN KEY ("AssignedToTeamId") REFERENCES public."Team" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_RuntimeWorkflowDataLog_User_AssignedToUserId" FOREIGN KEY ("AssignedToUserId") REFERENCES public."User" ("Id") ON DELETE RESTRICT
);

CREATE INDEX "IX_RuntimeWorkflow_RuntimeWorkflowSourceTemplateId" ON public."RuntimeWorkflow" ("RuntimeWorkflowSourceTemplateId");

CREATE INDEX "IX_RuntimeWorkflow_TriggeringComponentId" ON public."RuntimeWorkflow" ("TriggeringComponentId");

CREATE INDEX "IX_RuntimeWorkflow_TriggeringStepTaskComponentId" ON public."RuntimeWorkflow" ("TriggeringStepTaskComponentId");

CREATE INDEX "IX_RuntimeWorkflow_TriggeringTemplateId" ON public."RuntimeWorkflow" ("TriggeringTemplateId");

CREATE INDEX "IX_RuntimeWorkflowData_AssignedToHierarchyMasterId" ON public."RuntimeWorkflowData" ("AssignedToHierarchyMasterId");

CREATE INDEX "IX_RuntimeWorkflowData_AssignedToTeamId" ON public."RuntimeWorkflowData" ("AssignedToTeamId");

CREATE INDEX "IX_RuntimeWorkflowData_AssignedToTypeId" ON public."RuntimeWorkflowData" ("AssignedToTypeId");

CREATE INDEX "IX_RuntimeWorkflowData_AssignedToUserId" ON public."RuntimeWorkflowData" ("AssignedToUserId");

CREATE INDEX "IX_RuntimeWorkflowData_RuntimeWorkflowId" ON public."RuntimeWorkflowData" ("RuntimeWorkflowId");

CREATE INDEX "IX_RuntimeWorkflowDataLog_AssignedToHierarchyMasterId" ON log."RuntimeWorkflowDataLog" ("AssignedToHierarchyMasterId");

CREATE INDEX "IX_RuntimeWorkflowDataLog_AssignedToTeamId" ON log."RuntimeWorkflowDataLog" ("AssignedToTeamId");

CREATE INDEX "IX_RuntimeWorkflowDataLog_AssignedToTypeId" ON log."RuntimeWorkflowDataLog" ("AssignedToTypeId");

CREATE INDEX "IX_RuntimeWorkflowDataLog_AssignedToUserId" ON log."RuntimeWorkflowDataLog" ("AssignedToUserId");

CREATE INDEX "IX_RuntimeWorkflowDataLog_RuntimeWorkflowId" ON log."RuntimeWorkflowDataLog" ("RuntimeWorkflowId");

CREATE INDEX "IX_RuntimeWorkflowLog_RuntimeWorkflowSourceTemplateId" ON log."RuntimeWorkflowLog" ("RuntimeWorkflowSourceTemplateId");

CREATE INDEX "IX_RuntimeWorkflowLog_TriggeringComponentId" ON log."RuntimeWorkflowLog" ("TriggeringComponentId");

CREATE INDEX "IX_RuntimeWorkflowLog_TriggeringStepTaskComponentId" ON log."RuntimeWorkflowLog" ("TriggeringStepTaskComponentId");

CREATE INDEX "IX_RuntimeWorkflowLog_TriggeringTemplateId" ON log."RuntimeWorkflowLog" ("TriggeringTemplateId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20221012095106_S_20221012_1', '5.0.2');

COMMIT;

