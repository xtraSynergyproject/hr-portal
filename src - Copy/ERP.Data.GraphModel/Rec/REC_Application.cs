
using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{
    public partial class REC_Application : NodeBase
    {
        public string ApplicationNo { get; set; }
        public DateTime AppliedDate { get; set; }
        public RecruitmentApplicationStatusEnum ApplicationStatus { get; set; }
        public HiringStatusEnum HiringStatus { get; set; }
        public RecruitmentEmailStatusEnum? EmailSentStaus { get; set; }
        public DateTime? InterviewDate { get; set; }
        public TimeSpan? InterviewTime { get; set; }

        public bool Reference1Name { get; set; }
        public string Reference1JobTitle { get; set; }
        public string Reference1Address { get; set; }
        public string Reference1Company { get; set; }

        public bool Reference2Name { get; set; }
        public string Reference2JobTitle { get; set; }
        public string Reference2Address { get; set; }
        public string Reference2Company { get; set; }
    }
    public class R_Application_Candidate : RelationshipBase
    {

    }

    public class R_Application_JobRequest : RelationshipBase
    {

    }

    public class R_Application_Skill_ServiceFieldValue : RelationshipBase
    {
        public string Value { get; set; }
    }
    public class R_Application_SelectionCriteria_ServiceFieldValue : RelationshipBase
    {
        //public CriteriaTypeEnum Type { get; set; }
        public string Value { get; set; }
    }
    public class R_Application_DescriptiveQuestion_ServiceFieldValue : RelationshipBase
    {
        public string Value { get; set; }
    }
    public class R_Application_OtherInfo_ServiceFieldValue : RelationshipBase
    {
        //public OtherInfoTypeEnum Type { get; set; }
        public string Value { get; set; }

        //public string PreviousPosition { get; set; }
        //public string PreviousLocation { get; set; }
        //public string PreviousAppliedDate { get; set; }
    }
     
}
