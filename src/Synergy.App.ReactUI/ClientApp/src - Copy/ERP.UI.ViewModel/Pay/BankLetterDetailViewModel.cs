using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class BankLetterDetailViewModel : ViewModelBase
    {
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string BankAccountNo { get; set; }
        public string BankIBanNo { get; set; }
        public string BankSwiftCode { get; set; }



        public double NetAmount { get; set; }

        public long PersonId { get; set; }
        [Display(Name = "Employee Name /Iqama No")]
        public string PersonName { get; set; }
        public string SponsorshipNo { get; set; }

        public long BankLetterId { get; set; }
        public long SalaryEntryId { get; set; }



        public ExecutionStatusEnum ExecutionStatus { get; set; }
        public string Error { get; set; }


        public long? PayrollId { get; set; }
        public long? PayrollRunId { get; set; }


    }
}
