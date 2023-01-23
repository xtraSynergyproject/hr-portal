using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class BlockViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public long TabId { get; set; }
#pragma warning disable CS0108 // 'BlockViewModel.BlockId' hides inherited member 'ViewModelBase.BlockId'. Use the new keyword if hiding was intended.
        public long BlockId { get; set; }
#pragma warning restore CS0108 // 'BlockViewModel.BlockId' hides inherited member 'ViewModelBase.BlockId'. Use the new keyword if hiding was intended.
        public long SequenceNo { get; set; }
        public ICollection<ActionViewModel> Actions { get; set; }
        public ICollection<ScreenFieldViewModel> Fields { get; set; }
        public ICollection<FieldEditableViewModel> FieldEditable { get; set; }

        public bool Checked { get; set; }
    }
}


