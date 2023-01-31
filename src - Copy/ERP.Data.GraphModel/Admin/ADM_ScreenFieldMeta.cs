using System;
using System.Collections;
using System.Collections.Generic;
using ERP.Utility;

namespace ERP.Data.Model
{
    public partial class ADM_ScreenFieldMeta : AdminBase
    {
        public ulong ScreenId { get; set; }
        public ADM_Screen Screen { get; set; }
        public ulong FieldId { get; set; }
        public ADM_Field Field { get; set; }
        public long? SequenceNo { get; set; }

    }
}
