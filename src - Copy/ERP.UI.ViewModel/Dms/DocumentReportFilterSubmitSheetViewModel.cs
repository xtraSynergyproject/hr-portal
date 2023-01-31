using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class DocumentReportFilterSubmitSheetViewModel
    {
        public List<DocumentReportSheetViewModel> Created { get; set; }
        public List<DocumentReportSheetViewModel> Updated { get; set; }
        public List<DocumentReportSheetViewModel> Destroyed { get; set; }
    }
}
