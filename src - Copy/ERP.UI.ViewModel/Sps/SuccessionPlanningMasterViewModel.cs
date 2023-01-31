using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class SuccessionPlanningMasterViewModel : ViewModelBase
    {
        [Display(Name = "Name", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningMaster))]
        public string Name { get; set; }
        [Display(Name = "Description", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningMaster))]
        public string Description { get; set; }
        [Display(Name = "DocumentMasterTargetType", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningMaster))]
        public SuccessionPlanningTargetEnum? DocumentMasterTargetType { get; set; }
        [Display(Name = "DocumentDurationType", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningMaster))]
        public SuccessionPlanningDurationEnum? DocumentDurationType { get; set; }
        [Display(Name = "Year", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningMaster))]
        public int? Year { get; set; }
        [Display(Name = "Month", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningMaster))]
        public int? Month { get; set; }
        [Display(Name = "Quarter", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningMaster))]
        public int? Quarter { get; set; }
        [Display(Name = "HalfYear", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningMaster))]
        public int? HalfYear { get; set; }
        [Display(Name = "StartDate", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningMaster))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StartDate { get; set; }
        [Display(Name = "EndDate", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningMaster))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? EndDate { get; set; }
        [Display(Name = "ClosedDate", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningMaster))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? ClosedDate { get; set; }
        [Display(Name = "DocumentMasterStatus", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningMaster))]
        public SuccessionPlanningStatusEnum? DocumentMasterStatus { get; set; }
        [Display(Name = "SuccessionPlanningServiceId", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningMaster))]
        public long SuccessionPlanningServiceId { get; set; }
        [Display(Name = "DevelopmentPlanningServiceId", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanningMaster))]
        public long DevelopmentPlanningServiceId { get; set; }
    }
}
