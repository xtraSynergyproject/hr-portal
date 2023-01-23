START TRANSACTION;

ALTER TABLE public."Page" ADD "IsPageRedirect" boolean NOT NULL DEFAULT FALSE;

CREATE TABLE public."NtsStaging" (
    "Id" text COLLATE cms_collation_ci NOT NULL,
    "BatchId" text COLLATE cms_collation_ci NULL,
    "ReferenceId" text COLLATE cms_collation_ci NULL,
    "ReferenceType" integer NOT NULL,
    "TemplateId" text COLLATE cms_collation_ci NULL,
    "UserId" text COLLATE cms_collation_ci NULL,
    "FileId" text COLLATE cms_collation_ci NULL,
    "StageStatus" integer NOT NULL,
    "Error" text COLLATE cms_collation_ci NULL,
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
    CONSTRAINT "PK_NtsStaging" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220204051118_20220204_T_1', '5.0.2');

COMMIT;

