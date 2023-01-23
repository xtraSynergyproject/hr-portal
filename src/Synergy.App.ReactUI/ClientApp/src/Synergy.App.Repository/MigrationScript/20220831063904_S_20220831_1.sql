START TRANSACTION;

ALTER TABLE log."QRCodeDataLog" RENAME COLUMN "Url" TO "QrCodeUrl";

ALTER TABLE log."QRCodeDataLog" RENAME COLUMN "TargetUrl" TO "QRCodeImageId";

ALTER TABLE public."QRCodeData" RENAME COLUMN "Url" TO "QrCodeUrl";

ALTER TABLE public."QRCodeData" RENAME COLUMN "TargetUrl" TO "QRCodeImageId";

ALTER TABLE log."QRCodeDataLog" ADD "Data" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."QRCodeDataLog" ADD "IsPopup" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE log."QRCodeDataLog" ADD "QRCodeDataType" integer NOT NULL DEFAULT 0;

ALTER TABLE log."QRCodeDataLog" ADD "QRCodeType" integer NOT NULL DEFAULT 0;

ALTER TABLE public."QRCodeData" ADD "Data" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."QRCodeData" ADD "IsPopup" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."QRCodeData" ADD "QRCodeDataType" integer NOT NULL DEFAULT 0;

ALTER TABLE public."QRCodeData" ADD "QRCodeType" integer NOT NULL DEFAULT 0;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220831063904_S_20220831_1', '5.0.2');

COMMIT;

