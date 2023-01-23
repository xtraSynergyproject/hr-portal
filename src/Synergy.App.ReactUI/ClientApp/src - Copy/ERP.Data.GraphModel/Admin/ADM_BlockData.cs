using System;
using System.Collections;
using System.Collections.Generic;
using ERP.Utility;

namespace ERP.Data.Model
{
    public partial class ADM_BlockData : AdminBase
    {
        public string DataTypeCode { get; set; }
        public ulong BlockId { get; set; }
        public ADM_Block Block { get; set; }
        public ulong DataId { get; set; }
        public ADM_Data Data { get; set; }
        public long? SequenceNo { get; set; }

    }
}
