using ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Data.GraphModel
{
  
    public partial class GEN_Email : NodeBase
    {        
        public string From { get; set; }
        public string SenderName { get; set; }

        public string To { get; set; }
        public string CC { get; set; }

        public string BCC { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public NotificationStatusEnum EmailStatus { get; set; }

        public int RetryCount { get; set; }
        public bool IsAsync { get; set; }

        public string SmtpHost { get; set; }

        public int SmtpPort { get; set; }

        public string SmtpUserId { get; set; }

        public string Error { get; set; }
        public bool ShowOriginalSender { get; set; }
        public bool DisableDefaultEmailTemplate { get; set; }
        public bool? SendToOriginalRecipient { get; set; }

        public string Source { get; set; }
        public long? SequenceNo { get; set; }
        public Guid? EmailUniqueId { get; set; }
        public NodeEnum? ReferenceNode { get; set; }
      //  public ReferenceTypeEnum? ReferenceType { get; set; }
        public long? ReferenceId { get; set; }
        public CalendarInvitationTypeEnum? CalendarInvitationType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
