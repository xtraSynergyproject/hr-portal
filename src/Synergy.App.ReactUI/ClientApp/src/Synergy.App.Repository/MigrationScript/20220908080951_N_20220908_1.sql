START TRANSACTION;

ALTER TABLE public."MenuGroup" DROP CONSTRAINT "FK_MenuGroup_SubModule_SubModuleId";

ALTER TABLE log."MenuGroupLog" DROP CONSTRAINT "FK_MenuGroupLog_SubModule_SubModuleId";

ALTER TABLE log."MenuGroupLog" ALTER COLUMN "SubModuleId" DROP NOT NULL;

ALTER TABLE public."MenuGroup" ALTER COLUMN "SubModuleId" DROP NOT NULL;

ALTER TABLE public."MenuGroup" ADD CONSTRAINT "FK_MenuGroup_SubModule_SubModuleId" FOREIGN KEY ("SubModuleId") REFERENCES public."SubModule" ("Id") ON DELETE RESTRICT;

ALTER TABLE log."MenuGroupLog" ADD CONSTRAINT "FK_MenuGroupLog_SubModule_SubModuleId" FOREIGN KEY ("SubModuleId") REFERENCES public."SubModule" ("Id") ON DELETE RESTRICT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220908080951_N_20220908_1', '5.0.2');

COMMIT;