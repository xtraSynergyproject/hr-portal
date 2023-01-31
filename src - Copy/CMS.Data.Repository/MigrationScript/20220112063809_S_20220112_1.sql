START TRANSACTION;

ALTER TABLE log."CompanySettingLog" DROP CONSTRAINT "FK_CompanySettingLog_CompanySetting_Id";

ALTER TABLE log."NtsRatingLog" DROP CONSTRAINT "FK_NtsRatingLog_NtsRating_Id";

ALTER TABLE log."NtsRatingLog" ADD "CompanyId" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."NtsRatingLog" ADD "CreatedBy" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."NtsRatingLog" ADD "CreatedDate" timestamp without time zone NOT NULL DEFAULT TIMESTAMP '0001-01-01 00:00:00';

ALTER TABLE log."NtsRatingLog" ADD "DataAction" integer NOT NULL DEFAULT 0;

ALTER TABLE log."NtsRatingLog" ADD "IsDeleted" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE log."NtsRatingLog" ADD "LastUpdatedBy" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."NtsRatingLog" ADD "LastUpdatedDate" timestamp without time zone NOT NULL DEFAULT TIMESTAMP '0001-01-01 00:00:00';

ALTER TABLE log."NtsRatingLog" ADD "LegalEntityId" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."NtsRatingLog" ADD "NtsId" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."NtsRatingLog" ADD "NtsType" integer NOT NULL DEFAULT 0;

ALTER TABLE log."NtsRatingLog" ADD "PortalId" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."NtsRatingLog" ADD "RatedByUserId" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."NtsRatingLog" ADD "Rating" integer NOT NULL DEFAULT 0;

ALTER TABLE log."NtsRatingLog" ADD "RatingComment" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."NtsRatingLog" ADD "SequenceOrder" bigint NULL;

ALTER TABLE log."NtsRatingLog" ADD "Status" integer NOT NULL DEFAULT 0;

ALTER TABLE log."NtsRatingLog" ADD "VersionNo" bigint NOT NULL DEFAULT 0;

ALTER TABLE log."CompanySettingLog" ADD "Code" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."CompanySettingLog" ADD "CompanyId" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."CompanySettingLog" ADD "CreatedBy" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."CompanySettingLog" ADD "CreatedDate" timestamp without time zone NOT NULL DEFAULT TIMESTAMP '0001-01-01 00:00:00';

ALTER TABLE log."CompanySettingLog" ADD "DataAction" integer NOT NULL DEFAULT 0;

ALTER TABLE log."CompanySettingLog" ADD "GroupCode" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."CompanySettingLog" ADD "IsDeleted" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE log."CompanySettingLog" ADD "LastUpdatedBy" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."CompanySettingLog" ADD "LastUpdatedDate" timestamp without time zone NOT NULL DEFAULT TIMESTAMP '0001-01-01 00:00:00';

ALTER TABLE log."CompanySettingLog" ADD "LegalEntityId" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."CompanySettingLog" ADD "Name" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."CompanySettingLog" ADD "PortalId" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."CompanySettingLog" ADD "SequenceOrder" bigint NULL;

ALTER TABLE log."CompanySettingLog" ADD "Status" integer NOT NULL DEFAULT 0;

ALTER TABLE log."CompanySettingLog" ADD "Value" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."CompanySettingLog" ADD "VersionNo" bigint NOT NULL DEFAULT 0;

CREATE INDEX "IX_NtsRatingLog_RatedByUserId" ON log."NtsRatingLog" ("RatedByUserId");

ALTER TABLE log."NtsRatingLog" ADD CONSTRAINT "FK_NtsRatingLog_User_RatedByUserId" FOREIGN KEY ("RatedByUserId") REFERENCES public."User" ("Id") ON DELETE RESTRICT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220112063809_S_20220112_1', '5.0.2');

COMMIT;

