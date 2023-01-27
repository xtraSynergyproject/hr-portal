START TRANSACTION;

ALTER TABLE rec."JobDescriptionCriteria" RENAME COLUMN "CriteriaType" TO "CriteriaTypeId";

ALTER TABLE rec."JobDescription" RENAME COLUMN "Qualification" TO "QualificationId";

ALTER TABLE rec."JobCriteria" RENAME COLUMN "CriteriaType" TO "CriteriaTypeId";

ALTER TABLE rec."JobAdvertisement" RENAME COLUMN "Qualification" TO "QualificationId";

ALTER TABLE rec."JobAdvertisement" RENAME COLUMN "NeededDate" TO "RequiredDate";

ALTER TABLE rec."JobAdvertisement" RENAME COLUMN "LocationId" TO "JobLocationId";

ALTER TABLE rec."Application" ADD "CandidateId" text COLLATE cms_collation_ci NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220430121304_S_20220430_1', '5.0.2');

COMMIT;

