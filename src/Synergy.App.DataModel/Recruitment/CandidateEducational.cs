using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    [Table("CandidateEducational", Schema = "rec")]
    public class CandidateEducational : DataModelBase
    {
        [ForeignKey("CandidateProfile")]
        public string CandidateProfileId { get; set; }
        public CandidateProfile CandidateProfile { get; set; }
        public QualificationTypeEnum QualificationTypeId { get; set; }
        [ForeignKey("ListOfValue")]
        public string QualificationId { get; set; }
        public string OtherQualification { get; set; }
        [ForeignKey("ListOfValue")]
        public string SpecializationId { get; set; }
        public string OtherSpecialization { get; set; }
        [ForeignKey("ListOfValue")]
        public string EducationTypeId { get; set; }
        public string OtherEducationType { get; set; }
        public string Institute { get; set; }
        [ForeignKey("Country")]
        public string CountryId { get; set; }
        public string Duration { get; set; }
        public string PassingYear { get; set; }

        public string Marks { get; set; }

        public string AttachmentId { get; set; }
        public bool IsLatest { get; set; }


    }
}
