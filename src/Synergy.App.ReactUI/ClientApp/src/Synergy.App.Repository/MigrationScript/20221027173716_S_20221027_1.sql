START TRANSACTION;

ALTER TABLE log."ComponentResultLog" ADD "ParentComponentResultId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."ComponentResult" ADD "ParentComponentResultId" text COLLATE cms_collation_ci NULL;

CREATE INDEX "IX_ComponentResultLog_ParentComponentResultId" ON log."ComponentResultLog" ("ParentComponentResultId");

CREATE INDEX "IX_ComponentResult_ParentComponentResultId" ON public."ComponentResult" ("ParentComponentResultId");

ALTER TABLE public."ComponentResult" ADD CONSTRAINT "FK_ComponentResult_ComponentResult_ParentComponentResultId" FOREIGN KEY ("ParentComponentResultId") REFERENCES public."ComponentResult" ("Id") ON DELETE RESTRICT;

ALTER TABLE log."ComponentResultLog" ADD CONSTRAINT "FK_ComponentResultLog_ComponentResult_ParentComponentResultId" FOREIGN KEY ("ParentComponentResultId") REFERENCES public."ComponentResult" ("Id") ON DELETE RESTRICT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20221027173716_S_20221027_1', '5.0.2');

COMMIT;