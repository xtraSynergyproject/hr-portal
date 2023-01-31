START TRANSACTION;

ALTER TABLE log."CaptchaLog" ADD "EmailVerificationId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."Captcha" ADD "EmailVerificationId" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20221117055253_S_20221117_1', '5.0.2');

COMMIT;
