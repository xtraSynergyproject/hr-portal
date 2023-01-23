START TRANSACTION;

ALTER TABLE log."BLSAppointmentLog" ADD "ApplicantPhotoId" text COLLATE cms_collation_ci NULL;

ALTER TABLE public."BLSAppointment" ADD "ApplicantPhotoId" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20221123135826_S_20221123_1', '5.0.2');

COMMIT;