using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class PerformanceDocumentMasterViewModel : ViewModelBase
    {

        public long PerformanceDocumentMasterId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "Goal Service")]
        public long? GoalServiceTemplateMasterId { get; set; }
        [Display(Name = "Competency Service")]
        public long? CompetencyServiceTemplateMasterId { get; set; }
        [Display(Name = "Peer Review Task")]
        public long? PeerReviewTaskTemplateMasterId { get; set; }
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ClosedDate { get; set; }
        [Display(Name = "Document Status")]
        public PerformanceDocumentStatusEnum? DocumentMasterStatus { get; set; }
        public PerformanceDocumentTargetEnum? DocumentMasterTargetType { get; set; }
        public long SequenceNo { get; set; }
        public int Year { get; set; }
    }
}
