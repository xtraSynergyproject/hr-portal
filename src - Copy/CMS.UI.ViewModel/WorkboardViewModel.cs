using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class WorkboardViewModel : NoteViewModel
    {
        public string WorkboardId { get; set; }
        public string TemplateTypeId { get; set; }
        public string TemplateTypeName { get; set; }
        public string WorkBoardName { get; set; }
        public string WorkBoardDescription { get; set; }
        public string IconFileId { get; set; }
        public string WorkBoardStatus { get; set; }
    }
}
