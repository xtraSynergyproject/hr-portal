START TRANSACTION;

ALTER TABLE log."TaskTemplateLog" ADD "DisplayColumnId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."TaskTemplate" ADD "DisplayColumnId" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."ServiceTemplateLog" ADD "DisplayColumnId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ServiceTemplate" ADD "DisplayColumnId" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."NoteTemplateLog" ADD "DisplayColumnId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."NoteTemplate" ADD "DisplayColumnId" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."FormTemplateLog" ADD "DisplayColumnId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."FormTemplate" ADD "DisplayColumnId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."BusinessRuleModel" ADD "ReferenceId" text COLLATE cms_collation_ci NULL;

CREATE TABLE public."StepTaskAssigneeLogic" (
    "Id" text COLLATE cms_collation_ci NOT NULL,
    "Name" text COLLATE cms_collation_ci NULL,
    "ExecutionLogicDisplay" text COLLATE cms_collation_ci NULL,
    "ExecutionLogic" text COLLATE cms_collation_ci NULL,
    "SuccessResult" boolean NOT NULL,
    "AssignedToTypeId" text COLLATE cms_collation_ci NULL,
    "AssignedToUserId" text COLLATE cms_collation_ci NULL,
    "AssignedToTeamId" text COLLATE cms_collation_ci NULL,
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
    CONSTRAINT "PK_StepTaskAssigneeLogic" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_StepTaskAssigneeLogic_HierarchyMaster_AssignedToHierarchyMa~" FOREIGN KEY ("AssignedToHierarchyMasterId") REFERENCES public."HierarchyMaster" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StepTaskAssigneeLogic_LOV_AssignedToTypeId" FOREIGN KEY ("AssignedToTypeId") REFERENCES public."LOV" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StepTaskAssigneeLogic_Team_AssignedToTeamId" FOREIGN KEY ("AssignedToTeamId") REFERENCES public."Team" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StepTaskAssigneeLogic_User_AssignedToUserId" FOREIGN KEY ("AssignedToUserId") REFERENCES public."User" ("Id") ON DELETE RESTRICT
);

CREATE TABLE log."StepTaskAssigneeLogicLog" (
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
    "Name" text COLLATE cms_collation_ci NULL,
    "ExecutionLogicDisplay" text COLLATE cms_collation_ci NULL,
    "ExecutionLogic" text COLLATE cms_collation_ci NULL,
    "SuccessResult" boolean NOT NULL,
    "AssignedToTypeId" text COLLATE cms_collation_ci NULL,
    "AssignedToUserId" text COLLATE cms_collation_ci NULL,
    "AssignedToTeamId" text COLLATE cms_collation_ci NULL,
    "AssignedToHierarchyMasterId" text COLLATE cms_collation_ci NULL,
    "AssignedToHierarchyMasterLevelId" integer NOT NULL,
    CONSTRAINT "PK_StepTaskAssigneeLogicLog" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_StepTaskAssigneeLogicLog_HierarchyMaster_AssignedToHierarch~" FOREIGN KEY ("AssignedToHierarchyMasterId") REFERENCES public."HierarchyMaster" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StepTaskAssigneeLogicLog_LOV_AssignedToTypeId" FOREIGN KEY ("AssignedToTypeId") REFERENCES public."LOV" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StepTaskAssigneeLogicLog_Team_AssignedToTeamId" FOREIGN KEY ("AssignedToTeamId") REFERENCES public."Team" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StepTaskAssigneeLogicLog_User_AssignedToUserId" FOREIGN KEY ("AssignedToUserId") REFERENCES public."User" ("Id") ON DELETE RESTRICT
);

CREATE TABLE public."StepTaskSkipLogic" (
    "Id" text COLLATE cms_collation_ci NOT NULL,
    "Name" text COLLATE cms_collation_ci NULL,
    "ExecutionLogicDisplay" text COLLATE cms_collation_ci NULL,
    "ExecutionLogic" text COLLATE cms_collation_ci NULL,
    "SuccessResult" boolean NOT NULL,
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
    CONSTRAINT "PK_StepTaskSkipLogic" PRIMARY KEY ("Id")
);

CREATE TABLE log."StepTaskSkipLogicLog" (
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
    "Name" text COLLATE cms_collation_ci NULL,
    "ExecutionLogicDisplay" text COLLATE cms_collation_ci NULL,
    "ExecutionLogic" text COLLATE cms_collation_ci NULL,
    "SuccessResult" boolean NOT NULL,
    CONSTRAINT "PK_StepTaskSkipLogicLog" PRIMARY KEY ("Id")
);

CREATE INDEX "IX_TaskTemplateLog_DisplayColumnId" ON log."TaskTemplateLog" ("DisplayColumnId");

CREATE INDEX "IX_TaskTemplate_DisplayColumnId" ON public."TaskTemplate" ("DisplayColumnId");

CREATE INDEX "IX_ServiceTemplateLog_DisplayColumnId" ON log."ServiceTemplateLog" ("DisplayColumnId");

CREATE INDEX "IX_ServiceTemplate_DisplayColumnId" ON public."ServiceTemplate" ("DisplayColumnId");

CREATE INDEX "IX_NoteTemplateLog_DisplayColumnId" ON log."NoteTemplateLog" ("DisplayColumnId");

CREATE INDEX "IX_NoteTemplate_DisplayColumnId" ON public."NoteTemplate" ("DisplayColumnId");

CREATE INDEX "IX_FormTemplateLog_DisplayColumnId" ON log."FormTemplateLog" ("DisplayColumnId");

CREATE INDEX "IX_FormTemplate_DisplayColumnId" ON public."FormTemplate" ("DisplayColumnId");

CREATE INDEX "IX_StepTaskAssigneeLogic_AssignedToHierarchyMasterId" ON public."StepTaskAssigneeLogic" ("AssignedToHierarchyMasterId");

CREATE INDEX "IX_StepTaskAssigneeLogic_AssignedToTeamId" ON public."StepTaskAssigneeLogic" ("AssignedToTeamId");

CREATE INDEX "IX_StepTaskAssigneeLogic_AssignedToTypeId" ON public."StepTaskAssigneeLogic" ("AssignedToTypeId");

CREATE INDEX "IX_StepTaskAssigneeLogic_AssignedToUserId" ON public."StepTaskAssigneeLogic" ("AssignedToUserId");

CREATE INDEX "IX_StepTaskAssigneeLogicLog_AssignedToHierarchyMasterId" ON log."StepTaskAssigneeLogicLog" ("AssignedToHierarchyMasterId");

CREATE INDEX "IX_StepTaskAssigneeLogicLog_AssignedToTeamId" ON log."StepTaskAssigneeLogicLog" ("AssignedToTeamId");

CREATE INDEX "IX_StepTaskAssigneeLogicLog_AssignedToTypeId" ON log."StepTaskAssigneeLogicLog" ("AssignedToTypeId");

CREATE INDEX "IX_StepTaskAssigneeLogicLog_AssignedToUserId" ON log."StepTaskAssigneeLogicLog" ("AssignedToUserId");

ALTER TABLE public."FormTemplate" ADD CONSTRAINT "FK_FormTemplate_ColumnMetadata_DisplayColumnId" FOREIGN KEY ("DisplayColumnId") REFERENCES public."ColumnMetadata" ("Id") ON DELETE RESTRICT;

ALTER TABLE log."FormTemplateLog" ADD CONSTRAINT "FK_FormTemplateLog_ColumnMetadata_DisplayColumnId" FOREIGN KEY ("DisplayColumnId") REFERENCES public."ColumnMetadata" ("Id") ON DELETE RESTRICT;

ALTER TABLE public."NoteTemplate" ADD CONSTRAINT "FK_NoteTemplate_ColumnMetadata_DisplayColumnId" FOREIGN KEY ("DisplayColumnId") REFERENCES public."ColumnMetadata" ("Id") ON DELETE RESTRICT;

ALTER TABLE log."NoteTemplateLog" ADD CONSTRAINT "FK_NoteTemplateLog_ColumnMetadata_DisplayColumnId" FOREIGN KEY ("DisplayColumnId") REFERENCES public."ColumnMetadata" ("Id") ON DELETE RESTRICT;

ALTER TABLE public."ServiceTemplate" ADD CONSTRAINT "FK_ServiceTemplate_ColumnMetadata_DisplayColumnId" FOREIGN KEY ("DisplayColumnId") REFERENCES public."ColumnMetadata" ("Id") ON DELETE RESTRICT;

ALTER TABLE log."ServiceTemplateLog" ADD CONSTRAINT "FK_ServiceTemplateLog_ColumnMetadata_DisplayColumnId" FOREIGN KEY ("DisplayColumnId") REFERENCES public."ColumnMetadata" ("Id") ON DELETE RESTRICT;

ALTER TABLE public."TaskTemplate" ADD CONSTRAINT "FK_TaskTemplate_ColumnMetadata_DisplayColumnId" FOREIGN KEY ("DisplayColumnId") REFERENCES public."ColumnMetadata" ("Id") ON DELETE RESTRICT;

ALTER TABLE log."TaskTemplateLog" ADD CONSTRAINT "FK_TaskTemplateLog_ColumnMetadata_DisplayColumnId" FOREIGN KEY ("DisplayColumnId") REFERENCES public."ColumnMetadata" ("Id") ON DELETE RESTRICT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220618061059_S_20220618_2', '5.0.2');

COMMIT;

