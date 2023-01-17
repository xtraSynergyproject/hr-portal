using System;
using System.Collections;
using System.Collections.Generic;
using ERP.Utility;

namespace ERP.Data.Model
{
    public partial class ADM_ScreenDataSetMeta : AdminBase
    {
       
        public ulong ScreenId { get; set; }
        public ADM_Screen Screen { get; set; }
        public ulong DataSetId { get; set; }
        public ADM_DataSet DataSet { get; set; }
        public long? SequenceNo { get; set; }

    }
}
