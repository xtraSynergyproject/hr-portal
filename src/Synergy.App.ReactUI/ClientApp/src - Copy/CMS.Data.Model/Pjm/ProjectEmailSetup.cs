using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class ProjectEmailSetup : DataModelBase
    {
        public string UserName { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpFromId { get; set; }
        public string SmtpUserId { get; set; }
        public string SmtpPassword { get; set; }
        public long Count { get; set; }
        public string ServiceId { get; set; }
        public NtsService Service { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string Signature { get; set; }

        public string Pop3Host { get; set; }
        public int Pop3Port { get; set; }
        public DateTime? LastSyncDate { get; set; }

        public string Message { get; set; }

    }
    [Table("ProjectEmailSetupLog", Schema = "log")]
    public class ProjectEmailSetupLog : ProjectEmailSetup
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
