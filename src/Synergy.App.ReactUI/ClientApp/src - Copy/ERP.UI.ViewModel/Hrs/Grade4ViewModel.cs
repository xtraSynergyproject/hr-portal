using ERP.Utility;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class Grade4ViewModel : DatedViewModelBase
    {

        public long GradeId { get; set; }

        [Required]
        [StringLength(Constant.NameStringLength)]
        [Display(Name = "GradeName", ResourceType = typeof(Localize.Grade_en))]
        public string Name { get; set; }


        [Display(Name = "GradeName(Arabic)")]
        public string NameLocal { get; set; }

        [Display(Name = "SequenceNo")]
        public long? SequenceNo { get; set; }

        [Display(Name = "RankNo")]
        public long? RankNo { get; set; }

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
