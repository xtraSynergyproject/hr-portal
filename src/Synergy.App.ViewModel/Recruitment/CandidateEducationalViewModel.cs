using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class CandidateEducationalViewModel : CandidateEducational
    {
        //public string CandidateProfileId { get; set; }
        //public QualificationTypeEnum QualificationType { get; set; }       
        //public string QualificationId { get; set; }
        public string Specialization { get; set; }
        //public string EducationType { get; set; }
        //public string Institute { get; set; }
        //public string CountryId { get; set; }
        //public string Duration { get; set; }
        //public string PassingYear { get; set; }

        //public string Marks { get; set; }

        //public string AttachmentId { get; set; }
        //public bool IsLatest { get; set; }

        public string QualificationName { get; set; }
        //public string OtherQualification { get; set; }
        public string SpecializationName { get; set; }
        public string EducationTypeName { get; set; }
        public string CountryName { get; set; }
        public string AttachmentName { get; set; }
        public string NoteId { get; set; }
        public string Json { get; set; }
        public string CandidateId { get; set; }
        public CandidateProfile CandidateProfile { get; set; }
        public string CandidateProfileId { get; set; }
        public QualificationTypeEnum QualificationTypeId { get; set; }
        
        public string QualificationId { get; set; }
        public string OtherQualification { get; set; }
       
        public string SpecializationId { get; set; }
        public string OtherSpecialization { get; set; }
     
        public string EducationTypeId { get; set; }
        public string OtherEducationType { get; set; }
        public string Institute { get; set; }
       
        public string CountryId { get; set; }
        public string Duration { get; set; }
        public string PassingYear { get; set; }

        public string Marks { get; set; }

        public string AttachmentId { get; set; }
        public bool IsLatest { get; set; }
    }
}
