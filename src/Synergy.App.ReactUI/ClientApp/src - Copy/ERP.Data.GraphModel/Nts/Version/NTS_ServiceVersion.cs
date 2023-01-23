using ERP.Utility;
using System;
using System.Linq;

namespace ERP.Data.GraphModel
{
  
    public partial class NTS_ServiceVersion : NTS_Service
    {
        public long VersionedByUserId { get; set; }
        public DateTime VersionedDate { get; set; }
    }
    public class R_ServiceVersion_Service : RelationshipBase
    {

    }
    public class R_ServiceVersion_Template : RelationshipBase
    {

    }
    public class R_ServiceVersion_Step_ServicePlus : RelationshipBase
    {

    }
    public class R_ServiceVersion_ServicePlusServiceTemplate : RelationshipBase
    {

    }
    public class R_ServiceVersion_Adhoc_ServicePlus : RelationshipBase
    {

    }
    public class R_ServiceVersion_Owner_User : RelationshipBase
    {

    }
    public class R_ServiceVersion_RequestedBy_User : RelationshipBase
    {

    }
    public class R_ServiceVersion_Holder_User : RelationshipBase
    {

    }
   
    public class R_ServiceVersion_Status_ListOfValue : RelationshipBase
    {

    }
    public class R_ServiceVersion_Reference : RelationshipBase
    {
        public ReferenceTypeEnum ReferenceTypeCode { get; set; }
    }
    public class R_ServiceVersion_QuotationItem_Note : RelationshipBase
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
    }
    //public class R_ServiceVersion_SharedTo_User : RelationshipBase
    //{

    //}
    //public class R_ServiceVersion_SharedTo_Team : RelationshipBase
    //{

    //}

    //public class R_ServiceVersion_ParentReference : RelationshipBase
    //{

    //}
}
