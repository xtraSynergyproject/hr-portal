using CMS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class Module : DataModelBase
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string Prefix { get; set; }
        public  SubModule[] SubModules { get; set; }

    }
    [Table("ModuleLog", Schema = "log")]
    public class ModuleLog : Module
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
