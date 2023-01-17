START TRANSACTION;

CREATE TABLE public."UserPreference" (
    "Id" text COLLATE cms_collation_ci NOT NULL,
    "UserId" text COLLATE cms_collation_ci NULL,
    "PreferencePortalId" text COLLATE cms_collation_ci NULL,
    "DefaultLandingPageId" text COLLATE cms_collation_ci NULL,
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
    CONSTRAINT "PK_UserPreference" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_UserPreference_Page_DefaultLandingPageId" FOREIGN KEY ("DefaultLandingPageId") REFERENCES public."Page" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_UserPreference_Portal_PreferencePortalId" FOREIGN KEY ("PreferencePortalId") REFERENCES public."Portal" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_UserPreference_User_UserId" FOREIGN KEY ("UserId") REFERENCES public."User" ("Id") ON DELETE RESTRICT
);

CREATE TABLE log."UserPreferenceLog" (
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
    "UserId" text COLLATE cms_collation_ci NULL,
    "PreferencePortalId" text COLLATE cms_collation_ci NULL,
    "DefaultLandingPageId" text COLLATE cms_collation_ci NULL,
    CONSTRAINT "PK_UserPreferenceLog" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_UserPreferenceLog_Page_DefaultLandingPageId" FOREIGN KEY ("DefaultLandingPageId") REFERENCES public."Page" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_UserPreferenceLog_Portal_PreferencePortalId" FOREIGN KEY ("PreferencePortalId") REFERENCES public."Portal" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_UserPreferenceLog_User_UserId" FOREIGN KEY ("UserId") REFERENCES public."User" ("Id") ON DELETE RESTRICT
);

CREATE TABLE public."UserRolePreference" (
    "Id" text COLLATE cms_collation_ci NOT NULL,
    "UserRoleId" text COLLATE cms_collation_ci NULL,
    "PreferencePortalId" text COLLATE cms_collation_ci NULL,
    "DefaultLandingPageId" text COLLATE cms_collation_ci NULL,
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
    CONSTRAINT "PK_UserRolePreference" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_UserRolePreference_Page_DefaultLandingPageId" FOREIGN KEY ("DefaultLandingPageId") REFERENCES public."Page" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_UserRolePreference_Portal_PreferencePortalId" FOREIGN KEY ("PreferencePortalId") REFERENCES public."Portal" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_UserRolePreference_UserRole_UserRoleId" FOREIGN KEY ("UserRoleId") REFERENCES public."UserRole" ("Id") ON DELETE RESTRICT
);

CREATE TABLE log."UserRolePreferenceLog" (
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
    "PreferencePortalId" text COLLATE cms_collation_ci NULL,
    "DefaultLandingPageId" text COLLATE cms_collation_ci NULL,
    CONSTRAINT "PK_UserRolePreferenceLog" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_UserRolePreferenceLog_Page_DefaultLandingPageId" FOREIGN KEY ("DefaultLandingPageId") REFERENCES public."Page" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_UserRolePreferenceLog_Portal_PreferencePortalId" FOREIGN KEY ("PreferencePortalId") REFERENCES public."Portal" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_UserRolePreferenceLog_UserRole_UserRoleId" FOREIGN KEY ("UserRoleId") REFERENCES public."UserRole" ("Id") ON DELETE RESTRICT
);

CREATE INDEX "IX_UserPreference_DefaultLandingPageId" ON public."UserPreference" ("DefaultLandingPageId");

CREATE INDEX "IX_UserPreference_PreferencePortalId" ON public."UserPreference" ("PreferencePortalId");

CREATE INDEX "IX_UserPreference_UserId" ON public."UserPreference" ("UserId");

CREATE INDEX "IX_UserPreferenceLog_DefaultLandingPageId" ON log."UserPreferenceLog" ("DefaultLandingPageId");

CREATE INDEX "IX_UserPreferenceLog_PreferencePortalId" ON log."UserPreferenceLog" ("PreferencePortalId");

CREATE INDEX "IX_UserPreferenceLog_UserId" ON log."UserPreferenceLog" ("UserId");

CREATE INDEX "IX_UserRolePreference_DefaultLandingPageId" ON public."UserRolePreference" ("DefaultLandingPageId");

CREATE INDEX "IX_UserRolePreference_PreferencePortalId" ON public."UserRolePreference" ("PreferencePortalId");

CREATE INDEX "IX_UserRolePreference_UserRoleId" ON public."UserRolePreference" ("UserRoleId");

CREATE INDEX "IX_UserRolePreferenceLog_DefaultLandingPageId" ON log."UserRolePreferenceLog" ("DefaultLandingPageId");

CREATE INDEX "IX_UserRolePreferenceLog_PreferencePortalId" ON log."UserRolePreferenceLog" ("PreferencePortalId");

CREATE INDEX "IX_UserRolePreferenceLog_UserRoleId" ON log."UserRolePreferenceLog" ("UserRoleId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220822093029_S_20220822_1', '5.0.2');

COMMIT;

