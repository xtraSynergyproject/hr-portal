using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class Editor : DataModelBase
    {
        public string LabelName { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; set; }
        public string Name { get; set; }
        public string EditorTypeId { get; set; }
        public string MinimumValue { get; set; }
        public string MaximumValue { get; set; }
        public string DefaultValue { get; set; }
        public int Length { get; set; }
        public int Format { get; set; }
        public ValidationTypeEnum ValidationType { get; set; }
        public string ValidationRegEx { get; set; }
        public string CustomValidationMessage { get; set; }
    }
    [Table("EditorLog", Schema = "log")]
    public class EditorLog : Editor
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
