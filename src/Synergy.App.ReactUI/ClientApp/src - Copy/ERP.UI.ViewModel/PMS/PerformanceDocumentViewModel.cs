using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ERP.UI.ViewModel
{
    public class PerformanceDocumentViewModel : ViewModelBase
    {

      //  public long PerformanceDocumentId { get; set; }
        public long PerformanceDocumentMasterId { get; set; }
        public long DocumentOwnerUserId { get; set; }
        public long? GoalServiceTemplateMasterId { get; set; }
        public long? CompetencyServiceTemplateMasterId { get; set; }
        public long? PeerReviewTaskTemplateMasterId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<GoalViewModel> goals { get; set; }
        public int? Year { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "Closed Date")]
        public DateTime? ClosedDate { get; set; }
        [Display(Name = "Document Status")]
        public PerformanceDocumentStatusEnum? DocumentStatus { get; set; }
        public string base64Img { get; set; }
        public string JobTitle { get; set; }
        public string Grade { get; set; }
        [Display(Name = "Department Name")]
        public string Organization { get; set; }
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        public string DisplayName { get { return Name + " - " + Year ; } }
        public long FocusServiceId { get; set; }
        public long? CurrentStageId { get; set; }
        [Display(Name = "Line Manager Job Title")]
        public string LineManagerJobTitle { get; set; }
        [Display(Name = "Line Manager")]
        public string LineManager { get; set; }
        public long LineManagerUserId { get; set; }
        public PmsAccessType AccessType { get; set; }
    }
}
