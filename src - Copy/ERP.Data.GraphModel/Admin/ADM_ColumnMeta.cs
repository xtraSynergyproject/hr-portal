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

    public partial class ADM_ColumnMeta : AdminBase
    {
        public ulong ColumnMetaId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public Nullable<long> ObjectMetadataId { get; set; }
        public Nullable<bool> IsNullable { get; set; }
        public string DataType { get; set; }
        public Nullable<int> MaximumLength { get; set; }
        public Nullable<int> NumericPrecision { get; set; }
        public Nullable<int> NumericScale { get; set; }
        public string Description { get; set; }

    }
}