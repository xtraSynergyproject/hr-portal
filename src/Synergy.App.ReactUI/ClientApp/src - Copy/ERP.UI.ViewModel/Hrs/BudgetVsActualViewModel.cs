using System.ComponentModel.DataAnnotations;
namespace ERP.UI.ViewModel
{
    public class BudgetVsActualViewModel
    {
        [Display(Name = "Grade")]
        public string GradeName { get; set; }
        [Display(Name = "Head Count")]
        public int HeadCount { get; set; }
    }
}
