using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using static Synergy.App.Common.ApplicationConstant;

namespace Synergy.App.DataModel
{
    public class BLSAppointment : DataModelBase
    {
        public string ServiceId { get; set; }
        public string CaptchaId { get; set; }
        public string EmailVerificationCode { get; set; }
        public string ApplicantPhotoId { get; set; }
        public string PhotoId { get; set; }
        public string IpAddress { get; set; }
         
    }
    [Table("BLSAppointmentLog", Schema = "log")]
    public class BLSAppointmentLog : BLSAppointment
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
