START TRANSACTION;

ALTER TABLE log."UserLog" ADD "DepartmentName" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."User" ADD "DepartmentName" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."NtsTaskCommentLog" ADD "CommentSubject" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."NtsTaskComment" ADD "CommentSubject" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."NtsServiceCommentLog" ADD "CommentSubject" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."NtsServiceComment" ADD "CommentSubject" text COLLATE cms_collation_ci NULL;

CREATE TABLE public."StepTaskEscalation" (
    "Id" text COLLATE cms_collation_ci NOT NULL,
    "Name" text COLLATE cms_collation_ci NULL,
    "StepTaskComponentId" text COLLATE cms_collation_ci NULL,
    "ParentStepTaskEscalationId" text COLLATE cms_collation_ci NULL,
    "StepTaskEscalationType" integer NOT NULL,
    "AssignedToTypeId" text COLLATE cms_collation_ci NULL,
    "AssignedToUserId" text COLLATE cms_collation_ci NULL,
    "AssignedToTeamId" text COLLATE cms_collation_ci NULL,
    "AssignedToHierarchyMasterId" text COLLATE cms_collation_ci NULL,
    "AssignedToHierarchyMasterLevelId" integer NULL,
    "NewPriorityId" text COLLATE cms_collation_ci NULL,
    "NotificationTemplateId" text COLLATE cms_collation_ci NULL,
    "TriggerDaysAfterOverDue" integer NOT NULL,
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
    CONSTRAINT "PK_StepTaskEscalation" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_StepTaskEscalation_LOV_AssignedToTypeId" FOREIGN KEY ("AssignedToTypeId") REFERENCES public."LOV" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StepTaskEscalation_LOV_NewPriorityId" FOREIGN KEY ("NewPriorityId") REFERENCES public."LOV" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StepTaskEscalation_NotificationTemplate_NotificationTemplat~" FOREIGN KEY ("NotificationTemplateId") REFERENCES public."NotificationTemplate" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StepTaskEscalation_StepTaskComponent_StepTaskComponentId" FOREIGN KEY ("StepTaskComponentId") REFERENCES public."StepTaskComponent" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StepTaskEscalation_StepTaskEscalation_ParentStepTaskEscalat~" FOREIGN KEY ("ParentStepTaskEscalationId") REFERENCES public."StepTaskEscalation" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StepTaskEscalation_Team_AssignedToTeamId" FOREIGN KEY ("AssignedToTeamId") REFERENCES public."Team" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StepTaskEscalation_User_AssignedToUserId" FOREIGN KEY ("AssignedToUserId") REFERENCES public."User" ("Id") ON DELETE RESTRICT
);

CREATE TABLE log."StepTaskEscalationLog" (
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
    "StepTaskComponentId" text COLLATE cms_collation_ci NULL,
    "ParentStepTaskEscalationId" text COLLATE cms_collation_ci NULL,
    "StepTaskEscalationType" integer NOT NULL,
    "AssignedToTypeId" text COLLATE cms_collation_ci NULL,
    "AssignedToUserId" text COLLATE cms_collation_ci NULL,
    "AssignedToTeamId" text COLLATE cms_collation_ci NULL,
    "AssignedToHierarchyMasterId" text COLLATE cms_collation_ci NULL,
    "AssignedToHierarchyMasterLevelId" integer NULL,
    "NewPriorityId" text COLLATE cms_collation_ci NULL,
    "NotificationTemplateId" text COLLATE cms_collation_ci NULL,
    "TriggerDaysAfterOverDue" integer NOT NULL,
    CONSTRAINT "PK_StepTaskEscalationLog" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_StepTaskEscalationLog_LOV_AssignedToTypeId" FOREIGN KEY ("AssignedToTypeId") REFERENCES public."LOV" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StepTaskEscalationLog_LOV_NewPriorityId" FOREIGN KEY ("NewPriorityId") REFERENCES public."LOV" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StepTaskEscalationLog_NotificationTemplate_NotificationTemp~" FOREIGN KEY ("NotificationTemplateId") REFERENCES public."NotificationTemplate" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StepTaskEscalationLog_StepTaskComponent_StepTaskComponentId" FOREIGN KEY ("StepTaskComponentId") REFERENCES public."StepTaskComponent" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StepTaskEscalationLog_StepTaskEscalation_ParentStepTaskEsca~" FOREIGN KEY ("ParentStepTaskEscalationId") REFERENCES public."StepTaskEscalation" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StepTaskEscalationLog_Team_AssignedToTeamId" FOREIGN KEY ("AssignedToTeamId") REFERENCES public."Team" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_StepTaskEscalationLog_User_AssignedToUserId" FOREIGN KEY ("AssignedToUserId") REFERENCES public."User" ("Id") ON DELETE RESTRICT
);

CREATE INDEX "IX_StepTaskEscalation_AssignedToTeamId" ON public."StepTaskEscalation" ("AssignedToTeamId");

CREATE INDEX "IX_StepTaskEscalation_AssignedToTypeId" ON public."StepTaskEscalation" ("AssignedToTypeId");

CREATE INDEX "IX_StepTaskEscalation_AssignedToUserId" ON public."StepTaskEscalation" ("AssignedToUserId");

CREATE INDEX "IX_StepTaskEscalation_NewPriorityId" ON public."StepTaskEscalation" ("NewPriorityId");

CREATE INDEX "IX_StepTaskEscalation_NotificationTemplateId" ON public."StepTaskEscalation" ("NotificationTemplateId");

CREATE INDEX "IX_StepTaskEscalation_ParentStepTaskEscalationId" ON public."StepTaskEscalation" ("ParentStepTaskEscalationId");

CREATE INDEX "IX_StepTaskEscalation_StepTaskComponentId" ON public."StepTaskEscalation" ("StepTaskComponentId");

CREATE INDEX "IX_StepTaskEscalationLog_AssignedToTeamId" ON log."StepTaskEscalationLog" ("AssignedToTeamId");

CREATE INDEX "IX_StepTaskEscalationLog_AssignedToTypeId" ON log."StepTaskEscalationLog" ("AssignedToTypeId");

CREATE INDEX "IX_StepTaskEscalationLog_AssignedToUserId" ON log."StepTaskEscalationLog" ("AssignedToUserId");

CREATE INDEX "IX_StepTaskEscalationLog_NewPriorityId" ON log."StepTaskEscalationLog" ("NewPriorityId");

CREATE INDEX "IX_StepTaskEscalationLog_NotificationTemplateId" ON log."StepTaskEscalationLog" ("NotificationTemplateId");

CREATE INDEX "IX_StepTaskEscalationLog_ParentStepTaskEscalationId" ON log."StepTaskEscalationLog" ("ParentStepTaskEscalationId");

CREATE INDEX "IX_StepTaskEscalationLog_StepTaskComponentId" ON log."StepTaskEscalationLog" ("StepTaskComponentId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220905103604_N_20220905_1', '5.0.2');

COMMIT;

