using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using static Synergy.App.Common.ApplicationConstant;

namespace Synergy.App.DataModel
{
    public class BLSAppointmentSlot : DataModelBase
    {
        public string AppointmentId { get; set; }
        public string ApplicantNo { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string AppointmentTime { get; set; } 
        public BLSAppointmentStatusEnum AppointmentStatus { get; set; }

    }
    [Table("BLSAppointmentSlotLog", Schema = "log")]
    public class BLSAppointmentSlotLog : BLSAppointmentSlot
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
