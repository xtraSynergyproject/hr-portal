using System;

using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using ERP.Data.GraphModel;

namespace ERP.UI.ViewModel
{
    public class ServiceSearchViewModel : SearchViewModelBase
    {
        [Display(Name = "ServiceNo", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public string ServiceNo { get; set; }
        [Display(Name = "Subject", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public string Subject { get; set; }
        [Display(Name = "StartDate", ResourceType = typeof(ERP.Translation.Nts.Service))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? StartToDate { get; set; }
        [Display(Name = "DueDate", ResourceType = typeof(ERP.Translation.Nts.Service))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? DueDate { get; set; }
        public DateTime? DueToDate { get; set; }
        [Display(Name = "AssignedToType", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public GEN_ListOfValue AssignedToType { get; set; }
        [Display(Name = "CreationDate", ResourceType = typeof(ERP.Translation.Nts.Service))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? CreationDate { get; set; }
        [Display(Name = "CompletionDate", ResourceType = typeof(ERP.Translation.Nts.Service))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        public DateTime? CompletionDate { get; set; }
        public DateTime? CompletionToDate { get; set; }
        [Display(Name = "ServiceStatus", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public string ServiceStatus { get; set; }
        [Display(Name = "RequestedBy", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public string RequestedBy { get; set; }
        public string Mode { get; set; }
        public ModuleEnum? ModuleName { get; set; }
        public string ViewType { get; set; }
        public long? OwnerUserId { get; set; }
        public long? UserId { get; set; }
        public long? FilterUserId { get; set; }
        public string TemplateMasterCode { get; set; }
        public string TemplateCategoryCode { get; set; }
        public string Text { get; set; }
        public string RequestSource { get; set; }
        public string Layout { get; set; }
        public string ReturnUrl { get; set; }
        public long? TemplateMasterId { get; set; }
        public NtsTypeEnum? NTSType { get; set; }
    }
}
