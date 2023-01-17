using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class BankLetterViewModel : ViewModelBase
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
        public string PayrollId { get; set; }
        public string PayrollRunId { get; set; }

        public ExecutionStatusEnum ExecutionStatus { get; set; }
        public string Error { get; set; }
        public DateTime? PayrollStartDate { get; set; }
        public DateTime? PayrollEndDate { get; set; }
        public int? YearMonth { get; set; }
    }
}
