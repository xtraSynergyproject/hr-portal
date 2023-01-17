using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace ERP.UI.ViewModel
{
    [Serializable]
    public class TemplateCategoryViewModel : ViewModelBase    {

        public long TemplateCategoryId { get; set; }
        [Display(Name = "Code", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string Code { get; set; }
        [Display(Name = "Name", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string Name { get; set; }
        [Display(Name = "Description", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string Description { get; set; }
        public long? ParentTemplateCategoryId { get; set; }
        [Display(Name = "NtsType", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public NtsTypeEnum NtsType { get; set; }
        [Display(Name = "NtsClassification", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public NtsClassificationEnum NtsClassification { get; set; } 
    }
} 
 