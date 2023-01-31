using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class RecruitmentPayElementViewModel : RecruitmentPayElement
    {
        public double? Bonus;

        public double? MobileAllowance;

        public double? Transportation;

        public long? AnnualLeave;
        public double? Gratuity;
      
        public double? Accommodation;
        public double? ProfessionalAllowance;
        public double? Food;
        public double? UtilityAllowance;
        public double? Laundry;
        public double? Basic;
        public double? HRA;
        public double? FRA;
        public double? Total;
        public string Grade;
        public long? GradeNumber;
        public double? FurnishingAllowance;
        public string Desigination;

      
        public string ApplicationId;
        public string Value;
        public string AccommodationName;
        public string ApplicantName;
        public string VisaCategoryName;
        public bool? SalaryRevision { get; set; }
        public double? SalaryRevisionAmount { get; set; }

        public string SalaryRevisionComment { get; set; }

        public string TransportationText;
        public bool IsEligibleAfterProbation { get; set; }
    }
}
