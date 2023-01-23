﻿using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class ComponentParent : DataModelBase
    {

        [ForeignKey("Component")]
        public string ComponentId { get; set; }
        public Component Component { get; set; }

        [ForeignKey("Parent")]
        public string ParentId { get; set; }
        public Component Parent { get; set; }
    }
    [Table("ComponentParentLog", Schema = "log")]
    public class ComponentParentLog : ComponentParent
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