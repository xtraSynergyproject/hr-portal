
using ERP.Utility;
using System;

namespace ERP.Data.GraphModel
{
  
    public partial class PAY_BankLetter : NodeBase
    {
        public string BankName { get; set; }
        public string Description { get; set; }
        public string BankSwiftCode { get; set; }
        public string BranchName { get; set; }
        public string BankAccountNo { get; set; }
        public string BankIBanNo { get; set; }
        public DateTime? TransferDate { get; set; }
        public double NetAmount { get; set; }
        public DocumentStatusEnum PublishStatus { get; set; }
        public long? PayrollId { get; set; }
        public long? PayrollRunId { get; set; }

        public ExecutionStatusEnum ExecutionStatus { get; set; }
        public string Error { get; set; }

    }
    public class R_BankLetter_PayrollRun : RelationshipBase
    {

    }
}
