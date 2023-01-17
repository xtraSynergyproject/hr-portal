using ERP.Utility;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class GradeViewModel : DatedViewModelBase
    {

        public long GradeId { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType =typeof(ERP.Translation.Validation))]
        [StringLength(Constant.NameStringLength, ErrorMessageResourceName = "StringLength", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        [Display(Name = "Name", ResourceType = typeof(ERP.Translation.Hrs.Grade))]
        public string Name { get; set; }


        [Display(Name = "NameLocal", ResourceType = typeof(ERP.Translation.Hrs.Grade))]
        public string NameLocal { get; set; }

        [Display(Name = "SequenceNo", ResourceType = typeof(ERP.Translation.Hrs.Grade))]
        public long? SequenceNo { get; set; }

        [Display(Name = "RankNo",ResourceType =typeof(ERP.Translation.Hrs.Grade))]
        public long? RankNo { get; set; }

        [Display(Name = "Medical Insurance Cost")]
        public double? MedicalInsuranceCost { get; set; }

        [Display(Name = "Medical Card Type")]
        public MedicalCardTypeEnum? MedicalCardType { get; set; }

        [Display(Name = "Travel Class")]
        public TravelClassEnum? TravelClass { get; set; }

        [Display(Name = "Ticket Allowance Interval")]
        public int? TicketAllowanceInterval { get; set; }

        [Display(Name = "PerDiem Cost")]
        public double? PerDiemCost { get; set; }

        //[Display(Name = "Standard Salary(Min)")]
        //public byte[] StandardSalaryEncrypted { get; set; }
        //[Display(Name = "Standard Salary(Lower Min)")]
        //public byte[] MinimumSalaryEncrypted { get; set; }
        //[Display(Name = "Standard Salary(Max)")]
        //public byte[] MaximumSalaryEncrypted { get; set; }

        //[Display(Name = "Standard Salary(Min)")]
        //public decimal? StandardSalary { get; set; }
        //[Display(Name = "Standard Salary(Lower Min)")]
        //public decimal? MinimumSalary { get; set; }
        //[Display(Name = "Standard Salary(Max)")]
        //public decimal? MaximumSalary { get; set; }
    }
}
