using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class TemplateMasterSearchViewModel : DatedSearchViewModelBase
    {
        [Display(Name = "TemplateTypeCode", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string TemplateTypeCode { get; set; }
        [Display(Name = "TemplateCategoryCode", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string TemplateCategoryCode { get; set; }
        [Display(Name = "TemplateClassificationCode", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string TemplateClassificationCode { get; set; }
        [Display(Name = "Name", ResourceType = typeof(ERP.Translation.Nts.Template))]
        public string Name { get; set; }
        public bool IsSystemTemplate { get; set; }
    }
}
