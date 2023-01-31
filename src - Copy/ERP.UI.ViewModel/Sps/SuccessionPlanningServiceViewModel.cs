using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class SuccessionPlanningServiceViewModel : ViewModelBase
    {
        [Display(Name = "Id", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningService))]
        public override long Id { get; set; }
        [Display(Name = "SuccessionPlanningId", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningService))]
        public long SuccessionPlanningId { get; set; }
        [Display(Name = "ServiceTemplateMasterId", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningService))]
        public long ServiceTemplateMasterId { get; set; }
        [Display(Name = "ServiceId", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningService))]
        public long ServiceId { get; set; }
        [Display(Name = "OwnerUserId", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningService))]
        public long? OwnerUserId { get; set; }

        [Display(Name = "ServiceNo", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningService))]
        public string ServiceNo { get; set; }
        [Display(Name = "ServiceName", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningService))]
        public string ServiceName { get; set; }
        [Display(Name = "ServiceStatusName", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningService))]
        public string ServiceStatusName { get; set; }
        [Display(Name = "ServiceStatusCode", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningService))]
        public string ServiceStatusCode { get; set; }
        [Display(Name = "Subject", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningService))]
        public string Subject { get; set; }
        [Display(Name = "Description", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningService))]
        public string Description { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        [Display(Name = "StartDate", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningService))]
        public DateTime? StartDate { get; set; }
        [Display(Name = "StartDateDisplay", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningService))]
        public string StartDateDisplay { get { return StartDate.ToDD_MMM_YYYY(); } }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        [Display(Name = "DueDate", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningService))]
        public DateTime? DueDate { get; set; }
        [Display(Name = "DueDate", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningService))]
        public string DueDateDisplay { get { return DueDate.ToDD_MMM_YYYY(); } }

        [Display(Name = "CompletionDate", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningService))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? CompletionDate { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        [Display(Name = "SLA")]
        public int? SLA { get; set; }
        [Display(Name = "IsConfidential", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningService))]
        public bool IsConfidential { get; set; }
        [Display(Name = "PmsAccessType", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningService))]
        public PmsAccessType PmsAccessType { get; set; }
        [Display(Name = "ReferenceTypeCode", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningService))]
        public ReferenceTypeEnum ReferenceTypeCode { get; set; }
        [Display(Name = "RestaurantIds", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningService))]
        public string RestaurantIds { get; set; }
        [Display(Name = "PositionId", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningService))]
        public long PositionId { get; set; }
    }
}
