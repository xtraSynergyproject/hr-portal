using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
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
    }
}
