﻿using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class BusinessExecutionComponent : DataModelBase
    {
        [ForeignKey("Component")]
        public string ComponentId { get; set; }
        public Component Component { get; set; }
    }
    [Table("BusinessExecutionComponentLog", Schema = "log")]
    public class BusinessExecutionComponentLog : BusinessExecutionComponent
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