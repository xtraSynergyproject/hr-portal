
using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{
    public partial class REC_JobRequest : NodeBase
    {
        public string JobCode { get; set; }
        public int NoOfPositions { get; set; }
        public string JobDescription { get; set; }
        public string SkillSet { get; set; }
        public DateTime? PostedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Experience { get; set; }
        public string Qualification { get; set; }
        //public string Nationality { get; set; }
        //public string JobLocation { get; set; }
        public JobTypeEnum JobType { get; set; }
        public JobRequestStateEnum JobRequestState { get; set; }
        public JobRequestStatusEnum JobRequestStatus { get; set; }
    }

    public class R_JobRequest_JobRoot : RelationshipBase
    {

    }

    public class R_JobRequest_Recruiter_User : RelationshipBase
    {

    }
    public class R_JobRequest_Nationality : RelationshipBase
    {

    }
    public class R_JobRequest_Location : RelationshipBase
    {

    }
    public class R_JobRequest_Skill_ServiceFieldValue : RelationshipBase
    {

    }
    public class R_JobRequest_SelectionCriteria_ServiceFieldValue : RelationshipBase
    {

    }
    public class R_JobRequest_DescriptiveQuestion_ServiceFieldValue : RelationshipBase
    {

    }
    public class R_JobRequest_OtherInfo_ServiceFieldValue : RelationshipBase
    {

    }
}
