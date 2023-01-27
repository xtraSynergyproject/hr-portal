using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class ProcessDesignResult : DataModelBase
    {

        [ForeignKey("ProcessDesign")]
        public string ProcessDesignId { get; set; }
        public ProcessDesign ProcessDesign { get; set; }


        [ForeignKey("NtsService")]
        public string NtsServiceId { get; set; }
        public NtsService NtsService { get; set; }


        [ForeignKey("ProcessDesignStatus")]
        public string ProcessDesignStatusId { get; set; }
        public LOV ProcessDesignStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


    }
    [Table("ProcessDesignResultLog", Schema = "log")]
    public class ProcessDesignResultLog : ProcessDesignResult
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
