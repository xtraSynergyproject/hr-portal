START TRANSACTION;

ALTER TABLE log."CaptchaLog" ADD "ActualImages" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."CaptchaLog" ADD "DisplayImages" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."Captcha" ADD "ActualImages" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."Captcha" ADD "DisplayImages" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20221117073746_S_20221117_2', '5.0.2');

COMMIT;

