using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Common;
namespace CMS.UI.ViewModel
{
    public class ServiceBookHtmlItemViewModel
    {
        public  NtsTypeEnum  NtsType { get; set; }
        public string NtsId { get; set; }

        public long SequenceOrder { get; set; }

        public NoteTemplateViewModel NoteTemplateViewModel { get; set; }

        public ServiceTemplateViewModel ServiceTemplateViewModel { get; set; }

        public TaskTemplateViewModel TaskTemplateViewModel { get; set; }

        public string ParentId { get; set; }
        public NtsViewModel NtsViewModel { get; set; }

    }
}
