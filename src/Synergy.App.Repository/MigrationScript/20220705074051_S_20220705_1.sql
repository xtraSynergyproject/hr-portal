START TRANSACTION;

CREATE TABLE public."DocumentESign" (
    "Id" text COLLATE cms_collation_ci NOT NULL,
    "DocumentFileId" text COLLATE cms_collation_ci NULL,
    "DocumentReferenceNo" text COLLATE cms_collation_ci NULL,
    "Key" text COLLATE cms_collation_ci NULL,
    "Transaction" text COLLATE cms_collation_ci NULL,
    "ESignUrl" text COLLATE cms_collation_ci NULL,
    "ReferenceId" text COLLATE cms_collation_ci NULL,
    "ReferenceType" integer NOT NULL,
    "SignedDocumentFileId" text COLLATE cms_collation_ci NULL,
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
    CONSTRAINT "PK_DocumentESign" PRIMARY KEY ("Id")
);

CREATE TABLE log."DocumentESignLog" (
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
    "DocumentFileId" text COLLATE cms_collation_ci NULL,
    "DocumentReferenceNo" text COLLATE cms_collation_ci NULL,
    "Key" text COLLATE cms_collation_ci NULL,
    "Transaction" text COLLATE cms_collation_ci NULL,
    "ESignUrl" text COLLATE cms_collation_ci NULL,
    "ReferenceId" text COLLATE cms_collation_ci NULL,
    "ReferenceType" integer NOT NULL,
    "SignedDocumentFileId" text COLLATE cms_collation_ci NULL,
    CONSTRAINT "PK_DocumentESignLog" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220705074051_S_20220705_1', '5.0.2');

COMMIT;

