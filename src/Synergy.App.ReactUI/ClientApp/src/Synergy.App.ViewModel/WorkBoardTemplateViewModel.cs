using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class WorkBoardTemplateViewModel : NoteTemplateViewModel
    {
        public string TemplateCategoryId { get; set; }
        public string TemplateTypeName { get; set; }
        public string TemplateDescription { get; set; }
        public string  SampleContent { get; set; }
        public string  ContentImage { get; set; }
        public string TemplateTypeCode { get; set; }

    }
}
