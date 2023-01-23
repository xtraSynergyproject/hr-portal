START TRANSACTION;

ALTER TABLE log."NoteTemplateLog" ADD "OcrTemplateFileId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."NoteTemplate" ADD "OcrTemplateFileId" text COLLATE cms_collation_ci NULL;

CREATE TABLE public."OCRMapping" (
    "Id" text COLLATE cms_collation_ci NOT NULL,
    "TemplateId" text COLLATE cms_collation_ci NULL,
    "FieldName" text COLLATE cms_collation_ci NULL,
    "Cordinate1" text COLLATE cms_collation_ci NULL,
    "Cordinate2" text COLLATE cms_collation_ci NULL,
    "Cordinate3" text COLLATE cms_collation_ci NULL,
    "Cordinate4" text COLLATE cms_collation_ci NULL,
    "Cordinate5" text COLLATE cms_collation_ci NULL,
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
    CONSTRAINT "PK_OCRMapping" PRIMARY KEY ("Id")
);

CREATE TABLE log."OCRMappingLog" (
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
    "TemplateId" text COLLATE cms_collation_ci NULL,
    "FieldName" text COLLATE cms_collation_ci NULL,
    "Cordinate1" text COLLATE cms_collation_ci NULL,
    "Cordinate2" text COLLATE cms_collation_ci NULL,
    "Cordinate3" text COLLATE cms_collation_ci NULL,
    "Cordinate4" text COLLATE cms_collation_ci NULL,
    "Cordinate5" text COLLATE cms_collation_ci NULL,
    CONSTRAINT "PK_OCRMappingLog" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220127141009_20220127_T_2', '5.0.2');

COMMIT;

