
namespace ERP.Data.GraphModel
{
    using ERP.Utility;
    using System;
    using System.Linq;

    public partial class NTS_Service : NTSBase
    {
        public string ServiceNo { get; set; }
        public string Subject { get; set; }
        public long? VersionNo { get; set; }
        public string Description { get; set; }
        //public long? HolderUserId { get; set; }
        //public long? RequestedByUserId { get; set; }
        public TimeSpan? SLA { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public DateTime? DelegatedDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime? RejectedDate { get; set; }
        public DateTime? CanceledDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsConfidential { get; set; }
        //public long? SystemRatingId { get; set; }       

        public decimal CalculatedRating { get; set; }

        // public string ServiceStatusCode { get; set; }      

        public string RejectionReason { get; set; }
        public string ReturnReason { get; set; }
        public string CompleteReason { get; set; }
        public string DelegateReason { get; set; }
        public string CancelReason { get; set; }

        public bool? IsCreatingInBackGround { get; set; }
        public double? PercentageCompleted { get; set; }
        public string XmlData { get; set; }
    }
    public class R_Service_Template : RelationshipBase
    {

    }
    public class R_Service_Step_ServicePlus : RelationshipBase
    {

    }
    public class R_Service_Adhoc_ServicePlus : RelationshipBase
    {

    }
    public class R_Service_ServicePlusServiceTemplate : RelationshipBase
    {

    }
    public class R_Service_Owner_User : RelationshipBase
    {

    }
    public class R_Service_RequestedBy_User : RelationshipBase
    {

    }
    public class R_Service_Holder_User : RelationshipBase
    {

    }
    public class R_Service_Status_ListOfValue : RelationshipBase
    {

    }
    //public class R_Service_System_Rating : RelationshipBase
    //{

    //}

    public class R_Service_SharedTo_User : RelationshipBase
    {

    }
    public class R_Service_SharedTo_Team : RelationshipBase
    {

    }
    public class R_Service_SharedTo_OrganizationRoot : RelationshipBase
    {
        public bool IncludeAllChild { get; set; }
    }
    public class R_Service_Reference : RelationshipBase
    {
        public ReferenceTypeEnum ReferenceTypeCode { get; set; }
    }
    //public class R_Service_Multiple_Reference : RelationshipBase
    //{
    //    public ReferenceTypeEnum ReferenceTypeCode { get; set; }
    //}


    public class R_Service_QuestionAnswer_NoteMultipleFieldValue : RelationshipBase
    {
        public string Comment { get; set; }
        public string InterviewerComment { get; set; }
        public double? Score { get; set; }
    }
    //public class R_Service_ParentReference : RelationshipBase
    //{
    //    public string Code { get; set; }
    //}

    public class R_Service_Parent_Service : RelationshipBase
    {
        public ParentServiceTypeEnum ParentServiceType { get; set; }
    }

    public class R_Service_Recurrence_Service : RelationshipBase
    {

    }
    public class R_Service_Multiple_Reference : RelationshipBase
    {

    }
    public class R_Service_QuotationItem_Note : RelationshipBase
    {
        public QuotationItemTypeEnum? QuotationItemType { get; set; }
        public double? QuotationItemPrice { get; set; }
        public double? QuotationItemQuantity { get; set; }
        public double? QuotationItemTotal { get; set; }
        public string QuotationItemNote { get; set; }
        public long? QuotationItemSequenceNo { get; set; }
        public double? QuotationItemQuantityReceived { get; set; }
        public double? QuotationItemQuantityRejected { get; set; }
        public string QuotationItemRejectionReason { get; set; }
        public long? QuotationItemVersionNo { get; set; }
        public double? QuotationItemQuantityDelivered { get; set; }
    }
}
