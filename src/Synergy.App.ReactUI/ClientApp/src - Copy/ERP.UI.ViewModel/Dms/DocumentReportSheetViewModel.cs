using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class DocumentReportSheetViewModel
    {
        public string ProjectNumber { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentDescription { get; set; }
        public string Revision { get; set; }
        public string Discipline { get; set; }
        public string StageStatus { get; set; }
        public string TemplateOwner { get; set; }
    }
}
