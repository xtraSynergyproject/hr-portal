using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class TabViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public long ScreenId { get; set; }
        public long TabId { get; set; }
        public long SequenceNo { get; set; }
        public ICollection<BlockViewModel> Blocks { get; set; }

        public bool Checked { get; set; }
    }
}


