using CMS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class SubModule : DataModelBase
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        [ForeignKey("Module")]
        public string ModuleId { get; set; }
        public Module Module { get; set; }

        public MenuGroup[] MenuGroups { get; set; }

    }
    [Table("SubModuleLog", Schema = "log")]
    public class SubModuleLog : SubModule
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
