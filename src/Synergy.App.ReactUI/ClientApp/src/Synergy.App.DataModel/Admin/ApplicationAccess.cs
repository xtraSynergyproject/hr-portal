using Synergy.App.Common;
using System;

namespace Synergy.App.DataModel
{
    public class ApplicationAccess : DataModelBase
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Url { get; set; }
        public string ClientIP { get; set; }
        public AccessLogTypeEnum AccessType { get; set; }
        public string SessionId { get; set; }
        public DateTime LogDate { get; set; }
    }

}
