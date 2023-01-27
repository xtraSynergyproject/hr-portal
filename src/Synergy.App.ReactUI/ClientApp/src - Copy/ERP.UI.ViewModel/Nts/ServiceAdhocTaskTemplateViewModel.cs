using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    [Serializable]
    public class ServiceAdhocTaskTemplateViewModel : ViewModelBase
    {
        public long ServiceAdhocTaskTemplateId { get; set; }
        public long ServiceTemplateId { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        [Display(Name = "TaskTemplateId", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public long TaskTemplateId { get; set; }
        [Display(Name = "TaskTemplateTemplateMasterName", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string TaskTemplateTemplateMasterName { get; set; }

    }
}
