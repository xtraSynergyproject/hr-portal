using Synergy.App.Common;

namespace Synergy.App.DataModel
{
    public class Sms : DataModelBase
    {
        public string SmsText { get; set; }
        public string Number { get; set; }
        public SmsSendingTypeEnum SendingType { get; set; }
        public string Response { get; set; }
        public string Error { get; set; }
        public string SmsGateway { get; set; }
        public string SmsUserId { get; set; }
        public string SmsPassword { get; set; }
        public string SmsSenderName { get; set; }
        public NotificationStatusEnum SmsStatus { get; set; }
    }


}
