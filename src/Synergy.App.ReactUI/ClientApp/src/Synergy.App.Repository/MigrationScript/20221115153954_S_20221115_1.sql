START TRANSACTION;

CREATE TABLE public."Captcha" (
    "Id" text COLLATE cms_collation_ci NOT NULL,
    "ReferenceType" text COLLATE cms_collation_ci NULL,
    "ReferenceId" text COLLATE cms_collation_ci NULL,
    "RetryCount" integer NOT NULL,
    "SubmitCount" integer NOT NULL,
    "IpAddress" text COLLATE cms_collation_ci NULL,
    "MacId" text COLLATE cms_collation_ci NULL,
    "CaptchaText" text COLLATE cms_collation_ci NULL,
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
    CONSTRAINT "PK_Captcha" PRIMARY KEY ("Id")
);

CREATE TABLE log."CaptchaLog" (
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
    "ReferenceType" text COLLATE cms_collation_ci NULL,
    "ReferenceId" text COLLATE cms_collation_ci NULL,
    "RetryCount" integer NOT NULL,
    "SubmitCount" integer NOT NULL,
    "IpAddress" text COLLATE cms_collation_ci NULL,
    "MacId" text COLLATE cms_collation_ci NULL,
    "CaptchaText" text COLLATE cms_collation_ci NULL,
    CONSTRAINT "PK_CaptchaLog" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20221115153954_S_20221115_1', '5.0.2');

COMMIT;

