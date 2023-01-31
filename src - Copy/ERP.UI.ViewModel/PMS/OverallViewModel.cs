using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class OverallViewModel : ViewModelBase    {

        public string PmsDocumentName { get; set; }
        public int PmsDocumentYear { get; set; }
        public long PerformanceDocumentId { get; set; }
    }
}
