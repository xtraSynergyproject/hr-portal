using System;
using ERP.Utility;

namespace ERP.Data.GraphModel
{

    public partial class SAL_CustomerBooking : NodeBase
    {
        public string FullName { get; set; }
        public string NationalID { get; set; }
        public string PassportNumber { get; set; }
        public DateTime DOB { get; set; }
        public DateTime IssueDate { get; set; }
        public string IssuePlace { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string POBox { get; set; }
        public string City { get; set; }
        public string HouseNo { get; set; }
        public string StreetAddress { get; set; }
        public string District { get; set; }
       // public DepositProofEnum DepositProof { get; set; }
        public string PaymentReference { get; set; }
        public string IBAN { get; set; }
        public string AccountNumber { get; set; }
        public BookingStatusEnum BookingStatus { get; set; }
       
        public string BankName { get; set; }
        public string Branch { get; set; }

    }

    public class R_CustomerBooking_Lead : RelationshipBase
    {

    }
    public class R_CustomerBooking_Project : RelationshipBase
    {

    }
    public class R_CustomerBooking_UnitType : RelationshipBase
    {

    }
    public class R_CustomerBooking_Unit : RelationshipBase
    {

    }
    public class R_CustomerBooking_PaymentPlan : RelationshipBase
    {

    }
    public class R_CustomerBooking_Nationality : RelationshipBase
    {

    }

}
