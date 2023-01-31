using System;
using System.Collections;
using System.Collections.Generic;
using ERP.Utility;

namespace ERP.Data.Model
{
    public partial class ADM_ScreenDataMeta : AdminBase
    {
       
        public ulong ScreenId { get; set; }
        public ADM_Screen Screen { get; set; }
        public string DataTypeCode { get; set; }
        public ulong DataId { get; set; }
        public ADM_Data Data { get; set; }
        public long? SequenceNo { get; set; }

    }
}
