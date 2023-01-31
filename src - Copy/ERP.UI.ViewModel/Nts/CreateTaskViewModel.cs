using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class CreateTaskViewModel
    {
        //[Display (Name = "Template Category")]
        [Display(Name = "TemplateCategoryId", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public long TemplateCategoryId { get; set; }
        //[Required]
        //[Display(Name = "Task Template")]        
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ERP.Translation.Validation))]
        [Display(Name = "TemplateId", ResourceType = typeof(ERP.Translation.Nts.Task))]
        public long TemplateMasterId { get; set; }
    }
}
