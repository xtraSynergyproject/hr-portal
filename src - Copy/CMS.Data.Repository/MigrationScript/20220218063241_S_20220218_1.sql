START TRANSACTION;

ALTER TABLE log."NtsTaskSharedLog" ADD "ContributionType" integer NOT NULL DEFAULT 0;

ALTER TABLE public."NtsTaskShared" ADD "ContributionType" integer NOT NULL DEFAULT 0;

ALTER TABLE log."NtsServiceSharedLog" ADD "ContributionType" integer NOT NULL DEFAULT 0;

ALTER TABLE public."NtsServiceShared" ADD "ContributionType" integer NOT NULL DEFAULT 0;

ALTER TABLE log."NtsNoteSharedLog" ADD "ContributionType" integer NOT NULL DEFAULT 0;

ALTER TABLE public."NtsNoteShared" ADD "ContributionType" integer NOT NULL DEFAULT 0;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220218063241_S_20220218_1', '5.0.2');

COMMIT;

