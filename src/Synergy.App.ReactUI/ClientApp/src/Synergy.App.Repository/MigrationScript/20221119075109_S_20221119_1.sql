START TRANSACTION;

ALTER TABLE log."CaptchaLog" DROP COLUMN "EmailVerificationId";

ALTER TABLE log."CaptchaLog" DROP COLUMN "MacId";

ALTER TABLE public."Captcha" DROP COLUMN "EmailVerificationId";

ALTER TABLE public."Captcha" DROP COLUMN "MacId";

CREATE TABLE public."BLSAppointment" (
    "Id" text COLLATE cms_collation_ci NOT NULL,
    "ServiceId" text COLLATE cms_collation_ci NULL,
    "CaptchaId" text COLLATE cms_collation_ci NULL,
    "EmailVerificationCode" text COLLATE cms_collation_ci NULL,
    "PhotoId" text COLLATE cms_collation_ci NULL,
    "IpAddress" text COLLATE cms_collation_ci NULL,
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
    CONSTRAINT "PK_BLSAppointment" PRIMARY KEY ("Id")
);

CREATE TABLE log."BLSAppointmentLog" (
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
    "ServiceId" text COLLATE cms_collation_ci NULL,
    "CaptchaId" text COLLATE cms_collation_ci NULL,
    "EmailVerificationCode" text COLLATE cms_collation_ci NULL,
    "PhotoId" text COLLATE cms_collation_ci NULL,
    "IpAddress" text COLLATE cms_collation_ci NULL,
    CONSTRAINT "PK_BLSAppointmentLog" PRIMARY KEY ("Id")
);

CREATE TABLE public."BLSAppointmentSlot" (
    "Id" text COLLATE cms_collation_ci NOT NULL,
    "AppointmentId" text COLLATE cms_collation_ci NULL,
    "ApplicantNo" text COLLATE cms_collation_ci NULL,
    "AppointmentDate" timestamp without time zone NULL,
    "AppointmentTime" text COLLATE cms_collation_ci NULL,
    "AppointmentStatus" integer NOT NULL,
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
    CONSTRAINT "PK_BLSAppointmentSlot" PRIMARY KEY ("Id")
);

CREATE TABLE log."BLSAppointmentSlotLog" (
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
    "AppointmentId" text COLLATE cms_collation_ci NULL,
    "ApplicantNo" text COLLATE cms_collation_ci NULL,
    "AppointmentDate" timestamp without time zone NULL,
    "AppointmentTime" text COLLATE cms_collation_ci NULL,
    "AppointmentStatus" integer NOT NULL,
    CONSTRAINT "PK_BLSAppointmentSlotLog" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20221119075109_S_20221119_1', '5.0.2');

COMMIT;

