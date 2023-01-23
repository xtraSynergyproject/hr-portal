using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class EditorType : DataModelBase
    {
    
        public string Name { get; set; }

        public EditorCategoryEnum EditorCategory { get; set; }
        public ControlTypeEnum ControlType { get; set; }
        //public string IconCss { get; set; }
    }
    [Table("EditorTypeLog", Schema = "log")]
    public class EditorTypeLog : EditorType
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
