using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    [Table("EmailSetting", Schema = "rec")]
    public class EmailSetting : DataModelBase
    {
        public string SmtpSenderName { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUserId { get; set; }
        public string SmtpPassword { get; set; }
    }
}
