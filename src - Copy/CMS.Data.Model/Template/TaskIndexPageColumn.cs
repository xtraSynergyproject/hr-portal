﻿using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class TaskIndexPageColumn : DataModelBase
    {
        [ForeignKey("TaskIndexPageTemplate")]
        public string TaskIndexPageTemplateId { get; set; }
        public TaskIndexPageTemplate TaskIndexPageTemplate { get; set; }

        [ForeignKey("ColumnMetadata")]
        public string ColumnMetadataId { get; set; }
        public ColumnMetadata ColumnMetadata { get; set; }

        public string ColumnName { get; set; }
        public string HeaderName { get; set; }
        public string DisplayFormat { get; set; }
        public bool EnableSorting { get; set; }
        public bool EnableFiltering { get; set; }
        public bool IsForeignKeyTableColumn { get; set; }

        public string ForeignKeyTableAliasName { get; set; }
    }
    [Table("TaskIndexPageColumnLog", Schema = "log")]
    public class TaskIndexPageColumnLog : TaskIndexPageColumn
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