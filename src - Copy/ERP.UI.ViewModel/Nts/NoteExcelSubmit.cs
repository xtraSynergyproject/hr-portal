using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class NoteExcelSubmit
    {
        public  List<NoteExcelViewModel> Created { get; set; }
        public  List<NoteExcelViewModel> Updated { get; set; }
        public  List<NoteExcelViewModel> Destroyed { get; set; }
    }
}
