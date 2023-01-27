using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class EmailComponent : DataModelBase
    {
        public EmailReceipientTypeEnum ReceipientType { get; set; }
        public string ReceipientUserId { get; set; }


        public string ReceipientEmail { get; set; }



        public string SenderName { get; set; }
        public PropertyBindingTypeEnum SenderNameBindingType { get; set; }
        public ProcessDesignVariableTypeEnum SenderNameVariableType { get; set; }
        public string SenderNameVariable { get; set; }


        public string SenderEmail { get; set; }
        public string CC { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string[] Attachments { get; set; }

        [ForeignKey("Component")]
        public string ComponentId { get; set; }
        public Component Component { get; set; }
    }
    [Table("EmailComponentLog", Schema = "log")]
    public class EmailComponentLog : EmailComponent
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
