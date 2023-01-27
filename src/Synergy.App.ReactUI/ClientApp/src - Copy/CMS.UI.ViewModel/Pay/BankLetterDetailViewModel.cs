using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class BankLetterDetailViewModel : ViewModelBase
    {
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string BankAccountNo { get; set; }
        public string BankIBanNo { get; set; }
        public string BankSwiftCode { get; set; }



        public double NetAmount { get; set; }

        public string PersonId { get; set; }
        [Display(Name = "Employee Name /Iqama No")]
        public string PersonName { get; set; }
        public string SponsorshipNo { get; set; }

        public string BankLetterId { get; set; }
        public string SalaryEntryId { get; set; }



        public ExecutionStatusEnum ExecutionStatus { get; set; }
        public string Error { get; set; }
        public string PayrollId { get; set; }
        public string PayrollRunId { get; set; }
        public string Description { get; set; }
        public DateTime? TransferDate { get; set; }
        public DocumentStatusEnum PublishStatus { get; set; }
        public long TotalProcessed { get; set; }
        public long TotalSucceeded { get; set; }
    }
}
