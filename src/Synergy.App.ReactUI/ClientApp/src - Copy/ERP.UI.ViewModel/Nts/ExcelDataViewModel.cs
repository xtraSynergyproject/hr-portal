using ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class ExcelDataViewModel: ViewModelBase
    {        
        public string NoteNo { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public long FileId { get; set; }
        public string Data { get; set; }
        public string Extension { get; set; }
    }
}
