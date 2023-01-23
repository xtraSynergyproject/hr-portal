using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class SLAChangeRequestViewModel : ViewModelBase
    {
        public long TaskId { get; set; }
        [Display(Name = "StartDate", ResourceType = typeof(ERP.Translation.Nts.Service))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StartDate { get; set; }
        [Display(Name = "CurrentDueDate", ResourceType = typeof(ERP.Translation.Nts.Service))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DueDate { get; set; }
        [Display(Name = "CurrentSLA", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public TimeSpan? SLA { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ProposedStartDate { get; set; }
        [Display(Name = "ProposedDueDate", ResourceType = typeof(ERP.Translation.Nts.Service))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        public DateTime? ProposedDueDate { get; set; }
        [Display(Name = "ProposedSLA", ResourceType = typeof(ERP.Translation.Nts.Service))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        public TimeSpan? ProposedSLA { get; set; }
        [Display(Name = "RequestReason", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public string RequestReason { get; set; }
        [Display(Name = "RequestStatus", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public SLARequestStatusEnum? RequestStatus { get; set; }
        [Display(Name = "RejectReason", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public string RejectReason { get; set; }
        [Display(Name = "ApprovalComment", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public string ApprovalComment { get; set; }
        [Display(Name = "ApprovedSLA", ResourceType = typeof(ERP.Translation.Nts.Service))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        public TimeSpan? ApprovedSLA { get; set; }
        [Display(Name = "ApprovedDueDate", ResourceType = typeof(ERP.Translation.Nts.Service))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ApprovedDueDate { get; set; }
    }
}
