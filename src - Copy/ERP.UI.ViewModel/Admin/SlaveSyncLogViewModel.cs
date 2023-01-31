using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class SlaveSyncLogViewModel :  ViewModelBase
    {
        public string NtsNo { get; set; }
        public string TemplateName { get; set; }
        public NtsTypeEnum NtsType { get; set; }
        public bool ApiResponseStatus { get; set; }
        public string ResponseMessage { get; set; }
        public long? NtsId { get; set; }
        public long SlaveNtsId { get; set; }

    }
}


