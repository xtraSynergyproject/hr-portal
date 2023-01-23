using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class SuccessionPlanningViewModel : ViewModelBase
    {
        [Display(Name = "SuccessionPlanningId", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanning))]
        public long SuccessionPlanningId { get; set; }
        [Display(Name = "SuccessionPlanningMasterId", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanning))]
        public long SuccessionPlanningMasterId { get; set; }
        [Display(Name = "DocumentOwnerUserId", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanning))]
        public long DocumentOwnerUserId { get; set; }
        [Display(Name = "CycleServiceTemplateMasterId", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanning))]
        public long? CycleServiceTemplateMasterId { get; set; }
        [Display(Name = "DevelopmentServiceTemplateMasterId", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanning))]
        public long? DevelopmentServiceTemplateMasterId { get; set; }
        [Display(Name = "ServiceId", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanning))]
        public long? ServiceId { get; set; }
        [Display(Name = "Name", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanning))]
        public string Name { get; set; }
        [Display(Name = "Description", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanning))]
        public string Description { get; set; }
        [Display(Name = "successions", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanning))]
        public IList<SuccessionPlanningServiceViewModel> successions { get; set; }
        [Display(Name = "Year", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanning))]
        public int? Year { get; set; }
        [Display(Name = "StartDate", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanning))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "EndDate", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanning))]
        public DateTime? EndDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "ClosedDate", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanning))]
        public DateTime? ClosedDate { get; set; }
        [Display(Name = "DocumentStatus", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanning))]
        public SuccessionPlanningStatusEnum? DocumentStatus { get; set; }
        [Display(Name = "base64Img", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanning))]
        public string base64Img { get; set; }
        [Display(Name = "JobTitle", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanning))]
        public string JobTitle { get; set; }
        [Display(Name = "Grade", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanning))]
        public string Grade { get; set; }
        [Display(Name = "Organization", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanning))]
        public string Organization { get; set; }
        [Display(Name = "EmployeeName", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanning))]
        public string EmployeeName { get; set; }
        [Display(Name = "DisplayName", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanning))]
        public string DisplayName { get { return Name + " - " + Year ; } }
        [Display(Name = "FocusServiceId", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanning))]
        public long FocusServiceId { get; set; }
        [Display(Name = "CurrentStageId", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanning))]
        public long? CurrentStageId { get; set; }
        [Display(Name = "LineManagerJobTitle", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanning))]
        public string LineManagerJobTitle { get; set; }
        [Display(Name = "LineManager", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanning))]
        public string LineManager { get; set; }
        [Display(Name = "LineManagerUserId", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanning))]
        public long LineManagerUserId { get; set; }
        [Display(Name = "AccessType", ResourceType = typeof(ERP.Translation.Sps.SuccessionPlanning))]
        public PmsAccessType AccessType { get; set; }
    }
}
