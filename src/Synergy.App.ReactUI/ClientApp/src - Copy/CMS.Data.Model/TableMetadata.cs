using CMS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class TableMetadata : DataModelBase
    {
        public TableTypeEnum TableType { get; set; }
        public TemplateTypeEnum TemplateType { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public string Schema { get; set; }

        [ForeignKey("Module")]
        public string ModuleId { get; set; }
        public Module Module { get; set; }
        public bool CreateTable { get; set; }
        public string Query { get; set; }

        public string DefaultDisplayColumnId { get; set; }

        public bool EnableLegalEntityPermission { get; set; }
        public string ParentTableMetadataId { get; set; }
    }
    [Table("TableMetadataLog", Schema = "log")]
    public class TableMetadataLog : TableMetadata
    {
        public string RecordId { get; set; }
        public long LogVersionNo { get; set; }
        public bool IsLatest { get; set; }
        public DateTime LogStartDate { get; set; }
        public DateTime LogEndDate { get; set; }
        public DateTime LogStartDateTime { get; set; }
       public DateTime LogEndDateTime { get; set; } 
        public bool IsDatedLatest { get; set; } 
        public bool IsVersionLatest { get; set; }
    }
}
