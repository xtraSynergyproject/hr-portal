using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class SubModuleViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public long ModuleId { get; set; }
       // public long SubModuleId { get; set; }
        public long SequenceNo { get; set; }
        public ICollection<ScreenViewModel> Screens { get; set; }
        public bool Checked { get; set; }
    }
}


