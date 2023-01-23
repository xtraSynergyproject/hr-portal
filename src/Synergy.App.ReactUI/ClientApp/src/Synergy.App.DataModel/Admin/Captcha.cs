using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using static Synergy.App.Common.ApplicationConstant;

namespace Synergy.App.DataModel
{
    public class Captcha : DataModelBase
    {
        public string ReferenceType { get; set; }
        public string ReferenceId { get; set; }
        public int RetryCount { get; set; }
        public int SubmitCount { get; set; }
        public string IpAddress { get; set; }
        public bool IsVerified { get; set; }
        public string CaptchaText { get; set; }
        public string DisplayImages { get; set; }
        public string ActualImages { get; set; }
    }
    [Table("CaptchaLog", Schema = "log")]
    public class CaptchaLog : Captcha
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
