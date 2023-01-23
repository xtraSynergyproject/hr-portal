using System;
using System.Collections;
using System.Collections.Generic;
using ERP.Utility;

namespace ERP.Data.Model
{
    public partial class ADM_BlockField : AdminBase
    {
        public ulong BlockId { get; set; }
        public ADM_Block Block { get; set; }
        public ulong FieldId { get; set; }
        public ADM_Field Field { get; set; }
        public long? SequenceNo { get; set; }

    }
}
