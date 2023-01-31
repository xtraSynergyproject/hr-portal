
using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class PersonBankDocumentViewModel : PersonDocumentViewModel
    {
        [Display(Name = "Bank Name")]
        public long? BankBranchId { get; set; }
        [Display(Name = "Bank Name")]
        public string BankBranchName { get; set; }
        [Display(Name = "Bank Account No")]
        public string BankAccountNo { get; set; }
        [Display(Name = "Bank IBAN No")]
        public string BankIbanNo { get; set; }
        [Display(Name = "Salary Transfer Letter Provided By Company")]
        public string SalaryTransferLetter { get; set; }
        public string Attachment { get; set; }

    }
}
