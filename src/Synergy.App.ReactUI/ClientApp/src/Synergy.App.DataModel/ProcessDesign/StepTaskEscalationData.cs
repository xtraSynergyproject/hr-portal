using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class StepTaskEscalationData : DataModelBase
    {
        [ForeignKey("StepTaskEscalation")]
        public string StepTaskEscalationId { get; set; }
        public StepTaskEscalation StepTaskEscalation { get; set; }

        [ForeignKey("StepTaskComponent")]
        public string StepTaskComponentId { get; set; }
        public StepTaskComponent StepTaskComponent { get; set; }

        [ForeignKey("NtsTask")]
        public string NtsTaskId { get; set; }
        public NtsTask NtsTask { get; set; }


        [ForeignKey("NtsService")]
        public string NtsServiceId { get; set; }
        public NtsService NtsService { get; set; }


        [ForeignKey("EscalatedToUser")]
        public string EscalatedToUserId { get; set; }
        public User EscalatedToUser { get; set; }
        public DateTime EscalatedDate { get; set; }
        public string EscalationComment { get; set; }


    }
    [Table("StepTaskEscalationDataLog", Schema = "log")]
    public class StepTaskEscalationDataLog : StepTaskEscalationData
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
