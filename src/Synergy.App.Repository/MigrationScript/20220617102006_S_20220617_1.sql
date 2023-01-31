START TRANSACTION;

ALTER TABLE public."LOV" ADD "ReferenceId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."LOV" ADD "ReferenceType" integer NOT NULL DEFAULT 0;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220505122327_20220505_N_2', '5.0.2');

COMMIT;

START TRANSACTION;

ALTER TABLE rec."CandidateEducational" RENAME COLUMN "QualificationType" TO "QualificationTypeId";

ALTER TABLE rec."CandidateEducational" RENAME COLUMN "EducationType" TO "EducationTypeId";

ALTER TABLE log."TaskTemplateLog" ADD "LocalizedColumnId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."TaskTemplate" ADD "LocalizedColumnId" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."StepTaskComponentLog" ADD "OpenSameTaskOnServiceSubmit" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE log."ServiceTemplateLog" ADD "EnableIntroPage" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE log."ServiceTemplateLog" ADD "EnablePreviewPage" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE log."ServiceTemplateLog" ADD "IntroPageAction" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."ServiceTemplateLog" ADD "IntroPageArea" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."ServiceTemplateLog" ADD "IntroPageController" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."ServiceTemplateLog" ADD "IntroPageParams" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."ServiceTemplateLog" ADD "LocalizedColumnId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ServiceTemplate" ADD "EnableIntroPage" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."ServiceTemplate" ADD "EnablePreviewPage" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."ServiceTemplate" ADD "IntroPageAction" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ServiceTemplate" ADD "IntroPageArea" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ServiceTemplate" ADD "IntroPageController" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ServiceTemplate" ADD "IntroPageParams" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ServiceTemplate" ADD "LocalizedColumnId" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."NoteTemplateLog" ADD "LocalizedColumnId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."NoteTemplate" ADD "LocalizedColumnId" text COLLATE cms_collation_ci NULL;

ALTER TABLE rec."JobAdvertisement" ALTER COLUMN "AgencyId" TYPE text COLLATE cms_collation_ci;

ALTER TABLE log."FormTemplateLog" ADD "LocalizedColumnId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."FormTemplate" ADD "LocalizedColumnId" text COLLATE cms_collation_ci NULL;

CREATE INDEX "IX_TaskTemplateLog_LocalizedColumnId" ON log."TaskTemplateLog" ("LocalizedColumnId");

CREATE INDEX "IX_TaskTemplate_LocalizedColumnId" ON public."TaskTemplate" ("LocalizedColumnId");

CREATE INDEX "IX_ServiceTemplateLog_LocalizedColumnId" ON log."ServiceTemplateLog" ("LocalizedColumnId");

CREATE INDEX "IX_ServiceTemplate_LocalizedColumnId" ON public."ServiceTemplate" ("LocalizedColumnId");

CREATE INDEX "IX_NoteTemplateLog_LocalizedColumnId" ON log."NoteTemplateLog" ("LocalizedColumnId");

CREATE INDEX "IX_NoteTemplate_LocalizedColumnId" ON public."NoteTemplate" ("LocalizedColumnId");

CREATE INDEX "IX_FormTemplateLog_LocalizedColumnId" ON log."FormTemplateLog" ("LocalizedColumnId");

CREATE INDEX "IX_FormTemplate_LocalizedColumnId" ON public."FormTemplate" ("LocalizedColumnId");

ALTER TABLE public."FormTemplate" ADD CONSTRAINT "FK_FormTemplate_ColumnMetadata_LocalizedColumnId" FOREIGN KEY ("LocalizedColumnId") REFERENCES public."ColumnMetadata" ("Id") ON DELETE RESTRICT;

ALTER TABLE log."FormTemplateLog" ADD CONSTRAINT "FK_FormTemplateLog_ColumnMetadata_LocalizedColumnId" FOREIGN KEY ("LocalizedColumnId") REFERENCES public."ColumnMetadata" ("Id") ON DELETE RESTRICT;

ALTER TABLE public."NoteTemplate" ADD CONSTRAINT "FK_NoteTemplate_ColumnMetadata_LocalizedColumnId" FOREIGN KEY ("LocalizedColumnId") REFERENCES public."ColumnMetadata" ("Id") ON DELETE RESTRICT;

ALTER TABLE log."NoteTemplateLog" ADD CONSTRAINT "FK_NoteTemplateLog_ColumnMetadata_LocalizedColumnId" FOREIGN KEY ("LocalizedColumnId") REFERENCES public."ColumnMetadata" ("Id") ON DELETE RESTRICT;

ALTER TABLE public."ServiceTemplate" ADD CONSTRAINT "FK_ServiceTemplate_ColumnMetadata_LocalizedColumnId" FOREIGN KEY ("LocalizedColumnId") REFERENCES public."ColumnMetadata" ("Id") ON DELETE RESTRICT;

ALTER TABLE log."ServiceTemplateLog" ADD CONSTRAINT "FK_ServiceTemplateLog_ColumnMetadata_LocalizedColumnId" FOREIGN KEY ("LocalizedColumnId") REFERENCES public."ColumnMetadata" ("Id") ON DELETE RESTRICT;

ALTER TABLE public."TaskTemplate" ADD CONSTRAINT "FK_TaskTemplate_ColumnMetadata_LocalizedColumnId" FOREIGN KEY ("LocalizedColumnId") REFERENCES public."ColumnMetadata" ("Id") ON DELETE RESTRICT;

ALTER TABLE log."TaskTemplateLog" ADD CONSTRAINT "FK_TaskTemplateLog_ColumnMetadata_LocalizedColumnId" FOREIGN KEY ("LocalizedColumnId") REFERENCES public."ColumnMetadata" ("Id") ON DELETE RESTRICT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220617102006_S_20220617_1', '5.0.2');

COMMIT;

