START TRANSACTION;

CREATE TABLE public."CompanySetting" (
    "Id" text COLLATE cms_collation_ci NOT NULL,
    "Code" text COLLATE cms_collation_ci NULL,
    "Name" text COLLATE cms_collation_ci NULL,
    "Value" text COLLATE cms_collation_ci NULL,
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
    CONSTRAINT "PK_CompanySetting" PRIMARY KEY ("Id")
);

CREATE TABLE log."CompanySettingLog" (
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
    CONSTRAINT "PK_CompanySettingLog" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_CompanySettingLog_CompanySetting_Id" FOREIGN KEY ("Id") REFERENCES public."CompanySetting" ("Id") ON DELETE CASCADE
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220111104251_S_20220111_1', '5.0.2');

COMMIT;

