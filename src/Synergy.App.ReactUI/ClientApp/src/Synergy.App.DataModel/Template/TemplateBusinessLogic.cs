using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class TemplateBusinessLogic : DataModelBase
    {
        public BusinessLogicExecutionTypeEnum BusinessLogicExecutionType { get; set; }


        [ForeignKey("Action")]
        public string ActionId { get; set; }
        public LOV Action { get; set; }

    }
    [Table("TemplateBusinessLogicLog", Schema = "log")]
    public class TemplateBusinessLogicLog : TemplateBusinessLogic
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
