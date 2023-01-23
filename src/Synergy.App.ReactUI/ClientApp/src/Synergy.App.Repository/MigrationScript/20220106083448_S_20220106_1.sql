START TRANSACTION;

CREATE TABLE public."ApplicationAccess" (
    "Id" text COLLATE cms_collation_ci NOT NULL,
    "UserId" text COLLATE cms_collation_ci NULL,
    "Email" text COLLATE cms_collation_ci NULL,
    "UserName" text COLLATE cms_collation_ci NULL,
    "Url" text COLLATE cms_collation_ci NULL,
    "ClientIP" text COLLATE cms_collation_ci NULL,
    "AccessType" integer NOT NULL,
    "SessionId" text COLLATE cms_collation_ci NULL,
    "LogDate" timestamp without time zone NOT NULL,
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
    CONSTRAINT "PK_ApplicationAccess" PRIMARY KEY ("Id")
);

CREATE TABLE public."UserSession" (
    "Id" text COLLATE cms_collation_ci NOT NULL,
    "UserId" text COLLATE cms_collation_ci NULL,
    "Email" text COLLATE cms_collation_ci NULL,
    "UserName" text COLLATE cms_collation_ci NULL,
    "Url" text COLLATE cms_collation_ci NULL,
    "ClientIP" text COLLATE cms_collation_ci NULL,
    "SessionId" text COLLATE cms_collation_ci NULL,
    "SessionStartDate" timestamp without time zone NOT NULL,
    "SessionEndDate" timestamp without time zone NULL,
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
    CONSTRAINT "PK_UserSession" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220106083448_S_20220106_1', '5.0.2');

COMMIT;

