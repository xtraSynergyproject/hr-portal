START TRANSACTION;

CREATE TABLE public."NtsNoteUserReaction" (
    "Id" text COLLATE cms_collation_ci NOT NULL,
    "LikeOrDisLike" boolean NULL,
    "ReactedDate" timestamp without time zone NOT NULL,
    "ReactedByByUserId" text COLLATE cms_collation_ci NULL,
    "NtsNoteId" text COLLATE cms_collation_ci NULL,
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
    CONSTRAINT "PK_NtsNoteUserReaction" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_NtsNoteUserReaction_NtsNote_NtsNoteId" FOREIGN KEY ("NtsNoteId") REFERENCES public."NtsNote" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_NtsNoteUserReaction_User_ReactedByByUserId" FOREIGN KEY ("ReactedByByUserId") REFERENCES public."User" ("Id") ON DELETE RESTRICT
);

CREATE TABLE log."NtsNoteUserReactionLog" (
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
    "LikeOrDisLike" boolean NULL,
    "ReactedDate" timestamp without time zone NOT NULL,
    "ReactedByByUserId" text COLLATE cms_collation_ci NULL,
    "NtsNoteId" text COLLATE cms_collation_ci NULL,
    CONSTRAINT "PK_NtsNoteUserReactionLog" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_NtsNoteUserReactionLog_NtsNote_NtsNoteId" FOREIGN KEY ("NtsNoteId") REFERENCES public."NtsNote" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_NtsNoteUserReactionLog_User_ReactedByByUserId" FOREIGN KEY ("ReactedByByUserId") REFERENCES public."User" ("Id") ON DELETE RESTRICT
);

CREATE TABLE public."QRCodeData" (
    "Id" text COLLATE cms_collation_ci NOT NULL,
    "Url" text COLLATE cms_collation_ci NULL,
    "ReferenceType" integer NULL,
    "ReferenceTypeId" text COLLATE cms_collation_ci NULL,
    "TargetUrl" text COLLATE cms_collation_ci NULL,
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
    CONSTRAINT "PK_QRCodeData" PRIMARY KEY ("Id")
);

CREATE TABLE log."QRCodeDataLog" (
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
    "Url" text COLLATE cms_collation_ci NULL,
    "ReferenceType" integer NULL,
    "ReferenceTypeId" text COLLATE cms_collation_ci NULL,
    "TargetUrl" text COLLATE cms_collation_ci NULL,
    CONSTRAINT "PK_QRCodeDataLog" PRIMARY KEY ("Id")
);

CREATE INDEX "IX_NtsNoteUserReaction_NtsNoteId" ON public."NtsNoteUserReaction" ("NtsNoteId");

CREATE INDEX "IX_NtsNoteUserReaction_ReactedByByUserId" ON public."NtsNoteUserReaction" ("ReactedByByUserId");

CREATE INDEX "IX_NtsNoteUserReactionLog_NtsNoteId" ON log."NtsNoteUserReactionLog" ("NtsNoteId");

CREATE INDEX "IX_NtsNoteUserReactionLog_ReactedByByUserId" ON log."NtsNoteUserReactionLog" ("ReactedByByUserId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220820060134_S_20220820_1', '5.0.2');

COMMIT;

