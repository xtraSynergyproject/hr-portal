START TRANSACTION;

CREATE TABLE public."UserRoleHierarchyPermission" (
    "Id" text COLLATE cms_collation_ci NOT NULL,
    "UserRoleId" text COLLATE cms_collation_ci NULL,
    "HierarchyId" text COLLATE cms_collation_ci NULL,
    "HierarchyPermission" integer NOT NULL,
    "CustomRootId" text COLLATE cms_collation_ci NULL,
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
    CONSTRAINT "PK_UserRoleHierarchyPermission" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_UserRoleHierarchyPermission_HierarchyMaster_HierarchyId" FOREIGN KEY ("HierarchyId") REFERENCES public."HierarchyMaster" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_UserRoleHierarchyPermission_UserRole_UserRoleId" FOREIGN KEY ("UserRoleId") REFERENCES public."UserRole" ("Id") ON DELETE RESTRICT
);

CREATE TABLE log."UserRoleHierarchyPermissionLog" (
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
    "UserRoleId" text COLLATE cms_collation_ci NULL,
    "HierarchyId" text COLLATE cms_collation_ci NULL,
    "HierarchyPermission" integer NOT NULL,
    "CustomRootId" text COLLATE cms_collation_ci NULL,
    CONSTRAINT "PK_UserRoleHierarchyPermissionLog" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_UserRoleHierarchyPermissionLog_HierarchyMaster_HierarchyId" FOREIGN KEY ("HierarchyId") REFERENCES public."HierarchyMaster" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_UserRoleHierarchyPermissionLog_UserRole_UserRoleId" FOREIGN KEY ("UserRoleId") REFERENCES public."UserRole" ("Id") ON DELETE RESTRICT
);

CREATE INDEX "IX_UserRoleHierarchyPermission_HierarchyId" ON public."UserRoleHierarchyPermission" ("HierarchyId");

CREATE INDEX "IX_UserRoleHierarchyPermission_UserRoleId" ON public."UserRoleHierarchyPermission" ("UserRoleId");

CREATE INDEX "IX_UserRoleHierarchyPermissionLog_HierarchyId" ON log."UserRoleHierarchyPermissionLog" ("HierarchyId");

CREATE INDEX "IX_UserRoleHierarchyPermissionLog_UserRoleId" ON log."UserRoleHierarchyPermissionLog" ("UserRoleId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220430135600_S_20220430_2', '5.0.2');

COMMIT;

