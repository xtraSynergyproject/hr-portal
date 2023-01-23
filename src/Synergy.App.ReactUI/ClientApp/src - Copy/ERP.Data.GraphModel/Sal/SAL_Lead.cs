using System;
using ERP.Utility;

namespace ERP.Data.GraphModel
{

    public partial class SAL_Lead : NodeBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // [CreateOnly]
        //public virtual long LeadPersonId { get; set; }
        public long LeadPersonId { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public SalSourceOfLeadEnum SourceOfLead { get; set; }
        //public string Location { get; set; }
        public SalLeadStatusEnum LeadStatus { get; set; }
        public string CampaignName { get; set; }
    }
    //public class R_Lead_User : RelationshipBase
    //{

    //}

    public class R_Lead_Assigned_User : RelationshipBase
    {

    }
    public class R_Lead_PreviousAssigned_User : RelationshipBase
    {
        public string Reason { get; set; }
    }
    public class R_Lead_CountryDailCode : RelationshipBase
    {

    }
    public class R_Lead_Country : RelationshipBase
    {

    }
    public class R_Lead_Service : RelationshipBase
    {

    }
}
