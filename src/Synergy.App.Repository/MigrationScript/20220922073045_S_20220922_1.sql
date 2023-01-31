START TRANSACTION;

ALTER TABLE log."UserLog" ADD "DisableCaptcha" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE log."UserLog" ADD "OverrideCaptchaSettings" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE log."UserLog" ADD "OverrideTwoFactorAuthentication" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."User" ADD "DisableCaptcha" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."User" ADD "OverrideCaptchaSettings" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."User" ADD "OverrideTwoFactorAuthentication" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE log."StepTaskEscalationLog" ADD "EscalatedToNotificationTemplateId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."StepTaskEscalation" ADD "EscalatedToNotificationTemplateId" text COLLATE cms_collation_ci NULL;

ALTER TABLE log."PortalLog" ADD "DisableCaptcha" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE log."PortalLog" ADD "EnableTwoFactorAuth" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE log."PortalLog" ADD "TwoFactorAuthType" integer NULL;

ALTER TABLE public."Portal" ADD "DisableCaptcha" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."Portal" ADD "EnableTwoFactorAuth" boolean NOT NULL DEFAULT FALSE;

ALTER TABLE public."Portal" ADD "TwoFactorAuthType" integer NULL;

CREATE INDEX "IX_StepTaskEscalationLog_EscalatedToNotificationTemplateId" ON log."StepTaskEscalationLog" ("EscalatedToNotificationTemplateId");

CREATE INDEX "IX_StepTaskEscalation_EscalatedToNotificationTemplateId" ON public."StepTaskEscalation" ("EscalatedToNotificationTemplateId");

ALTER TABLE public."StepTaskEscalation" ADD CONSTRAINT "FK_StepTaskEscalation_NotificationTemplate_EscalatedToNotifica~" FOREIGN KEY ("EscalatedToNotificationTemplateId") REFERENCES public."NotificationTemplate" ("Id") ON DELETE RESTRICT;

ALTER TABLE log."StepTaskEscalationLog" ADD CONSTRAINT "FK_StepTaskEscalationLog_NotificationTemplate_EscalatedToNotif~" FOREIGN KEY ("EscalatedToNotificationTemplateId") REFERENCES public."NotificationTemplate" ("Id") ON DELETE RESTRICT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220922073045_S_20220922_1', '5.0.2');

COMMIT;

