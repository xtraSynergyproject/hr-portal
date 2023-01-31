using ERP.Utility;
using System;
using System.Collections.Generic;

namespace ERP.Data.GraphModel
{
    public partial class CLK_AccessLog : NodeBase
    {
        public long? DeviceId { get; set; }
        public string BiometricId { get; set; }
        public DateTime PunchingTime { get; set; }
        public string DeviceName { get; set; }
        public int? DeviceMachineNo { get; set; }
        public string DeviceIpAddress { get; set; }
        public int DevicePortNo { get; set; }
        public string DeviceSerialNo { get; set; }
        public PunchingTypeEnum? DevicePunchingType { get; set; }
        public long? UserInfoId { get; set; }
        public long? UserId { get; set; }
        public long? PersonId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string SponsorshipNo { get; set; }
        public string SignInLocation { get; set; }

        public AccessLogSourceEnum? AccessLogSource { get; set; }

    }
    public class R_AccessLog_GeoLocation : RelationshipBase
    {

    }
}
