using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class ServiceTaskViewModel : ViewModelBase
    {
        public long ServiceTaskTemplateId { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        [Display(Name = "Name", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public string Name { get; set; }        
        public long ServiceTemplateId { get; set; }
        public string ServiceTemplateName { get; set; }
        [Display(Name = "TaskTemplateId", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public long TaskTemplateId { get; set; }
        [Display(Name = "TaskTemplateName", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public string TaskTemplateName { get; set; }
        [Display(Name = "TriggeredByTemplateId", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public long? TriggeredByTemplateId { get; set; }
        [Display(Name = "TriggeredByTemplateName", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public string TriggeredByTemplateName { get; set; }
        [Display(Name = "ReturnedTo", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public long? ReturnedTo { get; set; }
        [Display(Name = "RatingTypeCode", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public string RatingTypeCode { get; set; }
        public string RatingTypeName { get; set; }
        public long? RatingTemplateId { get; set; }
        [Display(Name = "AssignToType", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public AssignToTypeEnum? AssignToType { get; set; }
        [Display(Name = "AssignedQueryType", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public AssignedQueryTypeEnum? AssignedQueryType { get; set; }
        [Display(Name = "AssignedTo", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public long AssignedTo { get; set; }
        [Display(Name = "AssignedByQuery", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public string AssignedByQuery { get; set; }
        public NtsServiceTaskTypeEnum? ServiceTaskType { get; set; }
        public long? ServiceTaskVersionId { get; set; }
        public long? ServiceVersionId { get; set; }
        public long? TaskVersionId { get; set; }
        public long? ServiceId { get; set; }
        public long? TaskId { get; set; }
        public string AssignedByMethodName { get; set; }
    }
}
