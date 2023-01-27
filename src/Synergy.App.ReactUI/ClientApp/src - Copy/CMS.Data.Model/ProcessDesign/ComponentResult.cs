using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class ComponentResult : DataModelBase
    {

        public ProcessDesignComponentTypeEnum ComponentType { get; set; }

        [ForeignKey("ProcessDesignResult")]
        public string ProcessDesignResultId { get; set; }
        public ProcessDesignResult ProcessDesignResult { get; set; }

        [ForeignKey("ProcessDesign")]
        public string ProcessDesignId { get; set; }
        public ProcessDesign ProcessDesign { get; set; }

        [ForeignKey("Component")]
        public string ComponentId { get; set; }
        public Component Component { get; set; }


        [ForeignKey("NtsService")]
        public string NtsServiceId { get; set; }
        public NtsService NtsService { get; set; }

        [ForeignKey("NtsTask")]
        public string NtsTaskId { get; set; }
        public NtsTask NtsTask { get; set; }


        [ForeignKey("ComponentStatus")]
        public string ComponentStatusId { get; set; }
        public LOV ComponentStatus { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm:ss}")]
        public DateTime? StartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm:ss}")]
        public DateTime? EndDate { get; set; }
        public string Error { get; set; }
    }
    [Table("ComponentResultLog", Schema = "log")]
    public class ComponentResultLog : ComponentResult
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
