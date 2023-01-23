using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class SlavePullLogViewModel :  ViewModelBase
    {
        public PullTypeEnum PullType { get; set; }
        public string ResponseMessage { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string RequestQuery { get; set; }
        public bool? ApiResponseStatus { get; set; }

    }
}


