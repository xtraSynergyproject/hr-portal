using ERP.Utility;
using System;

namespace ERP.Data.GraphModel
{

    public partial class TAA_BiometricData : NodeBase
    {
        public long BiometricUserId { get; set; }
        public string BadgeNumber { get; set; }
        public string SponsorhipNo { get; set; }
        public string SSN { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime EventTime { get; set; }
        public string CheckType { get; set; }
        public string VerifyCode { get; set; }
        public string SensorId { get; set; }
        public string WorkCode { get; set; }
        public bool? IsManual { get; set; }
        public string MachineName { get; set; }
        public string IP { get; set; }
        public string Port { get; set; }
        public long UserId { get; set; }
    }

    //public class R_BiometricData_User : RelationshipBase
    //{

    //}
}
