using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class ActionViewModel :  ViewModelBase
    {
        public string Name { get; set; }
        //public long ActionId { get; set; }
        public long? ScreenId { get; set; }
        public long SequenceNo { get; set; }
        public bool Checked { get; set; }
        public long? ParentId { get; set; }
        public bool EnableIfLicenseIsNotValid { get; set; }

    }
}


