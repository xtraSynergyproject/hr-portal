using System;
using ERP.Utility;

namespace ERP.Data.GraphModel
{

    public partial class SAL_TokenMoney : NodeBase
    {


        public string ReferenceNo { get; set; }

        public long? TokenAmount { get; set; }

        public PaymentModeEnum PaymentMode { get; set; }

        public string Attachment { get; set; }
        //public string BankName { get; set; }

        public Nullable<System.DateTime> PaymentDate { get; set; }
        //public long? LeadId { get; set; }

        //public string LeadName { get; set; }
        //public long? LeadPersonId { get; set; }
        //public string LeadPersonName { get; set; }
        //public long? ProjectId { get; set; }
        //public string ProjectName { get; set; }
        //public long? UnitTypeId { get; set; }
        //public string UnitTypeName { get; set; }
        //public long? UnitId { get; set; }
        //public string UnitName { get; set; }

        public string ConvertedTokenAmount { get; set; }
        public string DiscountAmount { get; set; }
        public string Others { get; set; }

    }
    public class R_TokenMoney_Unit : RelationshipBase
    {

    }
    public class R_TokenMoney_Lead : RelationshipBase
    {

    }
    public class R_TokenMoney_Currency : RelationshipBase
    {

    }
    public class R_TokenMoney_PaymentPlan : RelationshipBase
    {

    }

}
