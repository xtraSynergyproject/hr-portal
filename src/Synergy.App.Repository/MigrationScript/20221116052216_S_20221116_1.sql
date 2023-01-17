START TRANSACTION;

ALTER TABLE log."ServiceTemplateLog" ADD "DisableSubmitButton" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."ServiceTemplate" ADD "DisableSubmitButton" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE log."CaptchaLog" ADD "IsVerified" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."Captcha" ADD "IsVerified" boolean NOT NULL DEFAULT FALSE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20221116052216_S_20221116_1', '5.0.2');

COMMIT;

