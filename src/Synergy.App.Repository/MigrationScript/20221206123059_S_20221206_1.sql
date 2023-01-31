START TRANSACTION;

CREATE TABLE public."Sms" (
    "Id" text COLLATE cms_collation_ci NOT NULL,
    "SmsText" text COLLATE cms_collation_ci NULL,
    "Number" text COLLATE cms_collation_ci NULL,
    "SendingType" integer NOT NULL,
    "Response" text COLLATE cms_collation_ci NULL,
    "Error" text COLLATE cms_collation_ci NULL,
    "SmsGateway" text COLLATE cms_collation_ci NULL,
    "SmsUserId" text COLLATE cms_collation_ci NULL,
    "SmsPassword" text COLLATE cms_collation_ci NULL,
    "SmsSenderName" text COLLATE cms_collation_ci NULL,
    "SmsStatus" integer NOT NULL,
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
    CONSTRAINT "PK_Sms" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20221206123059_S_20221206_1', '5.0.2');

COMMIT;

