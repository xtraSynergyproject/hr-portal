﻿using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class NtsServiceStatusTrack : DataModelBase
    {

        [ForeignKey("NtsService")]
        public string NtsServiceId { get; set; }
        public NtsService NtsService { get; set; }

        [ForeignKey("ServiceStatus")]
        public string ServiceStatusId { get; set; }
        public LOV ServiceStatus { get; set; }

        public DateTime StatusChangedDate { get; set; }
        public string StatusChangedByUserId { get; set; }

    }
    [Table("NtsServiceStatusTrackLog", Schema = "log")]
    public class NtsServiceStatusTrackLog : NtsServiceStatusTrack
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
