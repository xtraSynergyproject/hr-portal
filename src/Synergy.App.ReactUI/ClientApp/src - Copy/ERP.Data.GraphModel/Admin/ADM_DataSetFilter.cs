//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ERP.Data.Model
{
    using System;
    using System.Collections.Generic;

    public partial class ADM_DataSetFilter : AdminBase
    {
        public virtual ulong DataSetId { get; set; }
        public virtual ulong DataId { get; set; }
        public virtual long? SequenceNo { get; set; }
        public virtual ADM_DataSet DataSet { get; set; }
    }
}