
using ERP.Utility;

namespace ERP.Data.GraphModel
{

    public partial class PAY_SalaryInfoRoot : RootNodeBase
    {

    }

    public partial class PAY_SalaryInfo : NodeDatedBase
    {
        [NotMapped]
        public long SalaryInfoId { get; set; }
        public string BankAccountNo { get; set; }
        public string BankIBanNo { get; set; }
        public bool? SalaryTransferLetterProvided { get; set; }
        public PaymentModeEnum PaymentMode { get; set; }
        public bool TakeAttendanceFromTAA { get; set; }
        public bool IsEligibleForOT { get; set; }
        public OTPaymentTypeEnum OTPaymentType { get; set; }

        public bool? IsEligibleForAirTicketForSelf { get; set; }
        public bool? IsEligibleForAirTicketForDependant { get; set; }      
        public bool? ValidateDependentDocumentForBenefit { get; set; }
        public int AirTicketInterval { get; set; }
        public bool? IsEligibleForEOS { get; set; }
        public double? UnpaidLeavesNotInSystem { get; set; }
        public bool? DisableProcessingTicket { get; set; }
    }

    public class R_SalaryInfoRoot : RelationshipDatedBase
    {

    }
    public class R_SalaryInfoRoot_PersonRoot : RelationshipBase
    {

    }
    public class R_SalaryInfo_BankBranch : RelationshipBase
    {

    }
    public class R_SalaryInfo_PayrollGroup : RelationshipBase
    {

    }
    public class R_SalaryInfo_PayCalendar : RelationshipBase
    {

    }

}
