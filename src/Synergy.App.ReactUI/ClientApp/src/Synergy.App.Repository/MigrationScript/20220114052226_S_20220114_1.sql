START TRANSACTION;

CREATE TABLE public."ApplicationDocument" (
    "Id" text COLLATE cms_collation_ci NOT NULL,
    "DocumentName" text COLLATE cms_collation_ci NULL,
    "Code" text COLLATE cms_collation_ci NULL,
    "DocumentId" text COLLATE cms_collation_ci NULL,
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
    CONSTRAINT "PK_ApplicationDocument" PRIMARY KEY ("Id")
);

CREATE TABLE public."HybridHierarchy" (
    "Id" text COLLATE cms_collation_ci NOT NULL,
    "HierarchyMasterId" text COLLATE cms_collation_ci NULL,
    "ParentId" text COLLATE cms_collation_ci NULL,
    "ReferenceType" text COLLATE cms_collation_ci NULL,
    "ReferenceId" text COLLATE cms_collation_ci NULL,
    "LevelId" integer NOT NULL,
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
    CONSTRAINT "PK_HybridHierarchy" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_HybridHierarchy_HierarchyMaster_HierarchyMasterId" FOREIGN KEY ("HierarchyMasterId") REFERENCES public."HierarchyMaster" ("Id") ON DELETE RESTRICT
);

CREATE TABLE log."HybridHierarchyLog" (
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
    "HierarchyMasterId" text COLLATE cms_collation_ci NULL,
    "ParentId" text COLLATE cms_collation_ci NULL,
    "ReferenceType" text COLLATE cms_collation_ci NULL,
    "ReferenceId" text COLLATE cms_collation_ci NULL,
    "LevelId" integer NOT NULL,
    CONSTRAINT "PK_HybridHierarchyLog" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_HybridHierarchyLog_HierarchyMaster_HierarchyMasterId" FOREIGN KEY ("HierarchyMasterId") REFERENCES public."HierarchyMaster" ("Id") ON DELETE RESTRICT
);

CREATE INDEX "IX_HybridHierarchy_HierarchyMasterId" ON public."HybridHierarchy" ("HierarchyMasterId");

CREATE INDEX "IX_HybridHierarchyLog_HierarchyMasterId" ON log."HybridHierarchyLog" ("HierarchyMasterId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220114052226_S_20220114_1', '5.0.2');

COMMIT;

