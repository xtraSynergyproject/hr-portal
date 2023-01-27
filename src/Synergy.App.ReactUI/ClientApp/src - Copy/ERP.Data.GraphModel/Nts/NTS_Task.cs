using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{
  
    public partial class NTS_Task : NTSBase
    {
        public string TaskNo { get; set; }
        //public NtsServiceTaskTypeEnum? ServiceTaskType { get; set; }
        public long? VersionNo { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }

        public TimeSpan? SLA { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public DateTime? DelegatedDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime? RejectedDate { get; set; }
        public DateTime? CanceledDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public DateTime? ReopenedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public DateTime? ReminderDate { get; set; }
        public string RatingComment { get; set; }
        public AssignToTypeEnum? AssignToType { get; set; }
        public string RejectionReason { get; set; }
        public string ReturnReason { get; set; }
        public string ReopenReason { get; set; }
        public string CancelReason { get; set; }
        public string CompleteReason { get; set; }
        public string DelegateReason { get; set; }
        public bool IsTaskAutoComplete { get; set; }
        public bool IsAssignedInTemplate { get; set; }
        //public decimal? SequenceNo { get; set; }

        public long? CompletedByUserId { get; set; }
        public long? RejectedByUserId { get; set; }

        public bool IsTaskAutoCompleteIfSameAssignee { get; set; }
        public NtsLockStatusEnum? LockStatus { get; set; }


        public long? PlanOrder { get; set; }

        public bool? IsRead { get; set; }
        public bool? IsLocked { get; set; }
        public TriggerTypeEnum? TriggerType { get; set; }
    }


    public class R_Task_Template : RelationshipBase
    {

    }
    public class R_Task_Owner_User : RelationshipBase
    {

    }
    public class R_Task_Owner_Team : RelationshipBase
    {

    }
    public class R_Task_Holder_User : RelationshipBase
    {

    }
    public class R_Task_RequestedBy_User : RelationshipBase
    {

    }
    public class R_Task_AssignedTo_User : RelationshipBase
    {

    }
    public class R_Task_AssignedTo_Team : RelationshipBase
    {

    }
    public class R_Task_Status_ListOfValue : RelationshipBase
    {

    }
    public class R_Task_Parent_Task : RelationshipBase
    {

    }
    //public class R_Task_Team : RelationshipBase
    //{

    //}

    public class R_Task_Step_Service : RelationshipBase
    {
        public TaskCreationMode? CreationMode { get; set; }
    }
    public class R_Task_Adhoc_Service : RelationshipBase
    {

    }
    public class R_Task_ServiceTaskTemplate : RelationshipBase
    {

    }

    public class R_Task_SharedTo_User : RelationshipBase
    {

    }
    public class R_Task_SharedTo_Team : RelationshipBase
    {

    }
    public class R_Task_SharedTo_OrganizationRoot : RelationshipBase
    {
        public bool ExcludeAllChild { get; set; }
    }
    public class R_Task_Reference : RelationshipBase
    {
        public ReferenceTypeEnum? ReferenceTypeCode { get; set; }
    }
    public class R_Task_PlannedBy_User : RelationshipBase
    {

    }
    public class R_Task_To_Task : RelationshipBase
    {

    }
    public class R_Task_CC_Task : RelationshipBase
    {

    }
    public class R_Task_NTS_Reference : RelationshipBase
    {
        public NtsTypeEnum? NtsType { get; set; }
    }
    public class R_Task_ParentReference : RelationshipBase
    {
        public ReferenceTypeEnum? ReferenceTypeCode { get; set; }
    }
    
    public class R_Email_Tag_Note : RelationshipBase
    {

    } 
    public class R_Email_Tag_Task : RelationshipBase
    {

    }
    public class R_Email_Tag_Service : RelationshipBase
    {

    }
    public class R_Task_Predecessor_Task : RelationshipBase
    {
        public DependencyTypeEnum Type { get; set; }
    }
    public class R_Task_Successor_Task : RelationshipBase
    {

    }

}
