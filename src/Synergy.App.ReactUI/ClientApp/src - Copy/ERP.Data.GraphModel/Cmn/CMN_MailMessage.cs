//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using ERP.Utility;
using System;


namespace ERP.Data.GraphModel
{
    public partial class CMN_MailMessage : NodeBase
    {
        public string MessageId { get; set; }
        public string From { get; set; }
        public string SenderName { get; set; }

        public string To { get; set; }
        public string CC { get; set; }

        public string BCC { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        //public NotificationStatusEnum EmailStatus { get; set; }

        //public string SmtpHost { get; set; }

        //public int SmtpPort { get; set; }

        //public string SmtpUserId { get; set; }
     
        public Guid? EmailUniqueId { get; set; }
        public NotificationStatusEnum? EmailStatus { get; set; }
    }

    public class R_MailMessage_Task : RelationshipBase
    {

    }

}
