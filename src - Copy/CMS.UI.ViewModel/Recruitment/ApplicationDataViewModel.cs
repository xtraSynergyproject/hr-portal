using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class ApplicationDataViewModel : ApplicationViewModel
    {
        public string ApplicationId { get; set; }
        public Application Application { get; set; }
        public string Program { get; set; }
        public ProficiencyEnum ProficiencyLevel { get; set; }
        //public string ApplicationId { get; set; }
        //public Application Application { get; set; }        
        public string DrivingLicenseCountryId { get; set; }
        public string Type { get; set; }
        public DateTime DrivingLicenseIssueDate { get; set; }
        public DateTime DrivingLicenseValidUpTo { get; set; }

        //public string ApplicationId { get; set; }
        //public Application Application { get; set; }
        public QualificationTypeEnum QualificationType { get; set; }
        public string QualificationId { get; set; }
        public string SpecializationId { get; set; }
        public EducationTypeEnum EducationType { get; set; }
        public string Institute { get; set; }
        public string EducationCountryId { get; set; }
        public string Duration { get; set; }
        public string PassingYear { get; set; }
        public double Marks { get; set; }

        //public string ApplicationId { get; set; }
        //public Application Application { get; set; }
        public string NatureOfWork { get; set; }
        public double NoOfYear { get; set; }

        //public string ApplicationId { get; set; }
        //public Application Application { get; set; }
        public string Employer { get; set; }
        public string LocationId { get; set; }
        public string PositionId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Responsibilities { get; set; }

        //public string ApplicationId { get; set; }
        //public Application Application { get; set; }
        public string ExperienceCountryId { get; set; }
        public double ExperienceNoOfYear { get; set; }

        //public string ApplicationId { get; set; }
        //public Application Application { get; set; }        
        public string JobId { get; set; }
        public double ExperienceByJobNoOfYear { get; set; }

        //public string ApplicationId { get; set; }
        //public Application Application { get; set; }       
        public string Sector { get; set; }        
        public string Industry { get; set; }        
        public string Category { get; set; }
        public double ExperienceBySectorNoOfYear { get; set; }

        //public string ApplicationId { get; set; }
        //public Application Application { get; set; }
        public string Language { get; set; }
        public LanguageProficiencyEnum LanguageProficiencyLevel { get; set; }

        //public string ApplicationId { get; set; }
        //public Application Application { get; set; }
        public string Currency { get; set; }
        public double Value { get; set; }
        public string Client { get; set; }
        public string Consultant { get; set; }
        public string ConstructionPeriodFrom { get; set; }
        public string ConstructionPeriodTo { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }

        //public string ApplicationId { get; set; }
        //public Application Application { get; set; }
        public string ReferenceName { get; set; }
        public string ReferencePosition { get; set; }
        public string ReferenceCompany { get; set; }
        public string ReferencePhone { get; set; }
        public string ReferenceEmail { get; set; }

        //public string ApplicationId { get; set; }
        //public Application Application { get; set; }
        public double OverseasSalary { get; set; }
        public double IndianSalary { get; set; }
        public double NetSalary { get; set; }
        public double OtherAllowances { get; set; }
    }
}
