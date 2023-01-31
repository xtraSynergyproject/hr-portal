using ERP.Utility;
using System;
using System.Collections.Generic;

namespace ERP.Data.GraphModel
{
    public partial class CLK_UserInfo : NodeBase
    {
        public string BiometricId { get; set; }
        public string SponsorshipNo { get; set; }
        public string Name { get; set; }
        public int Privelage { get; set; }
        public string Password { get; set; }
        public bool Enabled { get; set; }
    }
    public class R_UserInfo_PersonRoot : RelationshipBase
    {

    }
    public class R_UserInfo_Device : RelationshipBase
    {
        public ProcessStatusEnum? RegistrationStatus { get; set; }
        public IncludeExclude? IncludeOrExclude { get; set; }
        public bool? IsRegisteredDevice { get; set; }
    }
}
