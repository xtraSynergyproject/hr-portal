using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class ScreenViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public long SubModuleId { get; set; }
        //public long ScreenId { get; set; }
        public long SequenceNo { get; set; }
        public ICollection<ActionViewModel> Actions { get; set; }
        public ICollection<ScreenFieldViewModel> Fields { get; set; }
        public ICollection<ScreenFieldViewModel> FieldEditable { get; set; }
        public bool Checked { get; set; }
    }
}


