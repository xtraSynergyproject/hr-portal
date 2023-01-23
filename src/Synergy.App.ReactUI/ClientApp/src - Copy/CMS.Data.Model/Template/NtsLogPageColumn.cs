﻿using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class NtsLogPageColumn : DataModelBase
    {
        [ForeignKey("Template")]
        public string TemplateId { get; set; }
        public Template Template { get; set; }

        [ForeignKey("ColumnMetadata")]
        public string ColumnMetadataId { get; set; }
        public ColumnMetadata ColumnMetadata { get; set; }
        public string ColumnName { get; set; }
        public string HeaderName { get; set; }
        public string  DisplayFormat { get; set; }
        public bool EnableSorting { get; set; }
        public bool EnableFiltering { get; set; }
        public bool IsForeignKeyTableColumn { get; set; }
        public string ForeignKeyTableAliasName { get; set; }
    }
    [Table("NtsLogPageColumnLog", Schema = "log")]
    public class NtsLogPageColumnLog : NtsLogPageColumn
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
