using System.ComponentModel.DataAnnotations;
namespace ERP.UI.ViewModel
{
    public class GradeWiseHeadCountViewModel
    {
        [Display(Name = "Grade")]
        public string GradeName { get; set; }
        [Display(Name = "Head Count")]
        public int HeadCount { get; set; }
    }
}
