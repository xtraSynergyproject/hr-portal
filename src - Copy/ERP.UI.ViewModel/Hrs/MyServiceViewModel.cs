using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class MyServiceViewModel
    {
        //[Display(Name = "Template Category")]
        [Display(Name = "TemplateCategoryId", ResourceType = typeof(Translation.Nts.Service))]
        public long TemplateCategoryId { get; set; }
        //[Required]
        //[Display(Name = "Service Template")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        [Display(Name = "TemplateId", ResourceType = typeof(ERP.Translation.Nts.Service))]
        public long TemplateId { get; set; }
        public long UserId { get; set; }
    }

}
