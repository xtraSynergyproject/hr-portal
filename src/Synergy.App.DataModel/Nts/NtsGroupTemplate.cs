using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class NtsGroupTemplate : DataModelBase
    {
        [ForeignKey("NtsGroup")]
        public string NtsGroupId { get; set; }
        public NtsGroup NtsGroup { get; set; }

        [ForeignKey("Template")]
        public string TemplateId { get; set; }
        public Template Template { get; set; }
    }
    [Table("NtsGroupTemplateLog", Schema = "log")]
    public class NtsGroupTemplateLog : NtsGroupTemplate
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
