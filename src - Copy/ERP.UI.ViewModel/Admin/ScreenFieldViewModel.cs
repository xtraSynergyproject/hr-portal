using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class ScreenFieldViewModel :  ViewModelBase
    {
        public string Name { get; set; }       
        public long ScreenId { get; set; }
        public long SequenceNo { get; set; }
        public bool Checked { get; set; }
        public string DisplayName { get; set; }
        public bool IsVisible { get; set; }
        public bool IsEditable { get; set; }

        public string VisibleName { get; set; }
        public string EditableName { get; set; }

    }
}


