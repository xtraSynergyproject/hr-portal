using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class BankLetterViewModel : ViewModelBase
    {
        public string BankName { get; set; }
        public string BankSwiftCode { get; set; }
        public string BranchName { get; set; }
        public string BankAccountNo { get; set; }
        public string BankIBanNo { get; set; }
        public DateTime? TransferDate { get; set; }
        public double NetAmount { get; set; }
        public DocumentStatusEnum PublishStatus { get; set; }

        public ExecutionStatusEnum ExecutionStatus { get; set; }
        public string Error { get; set; }
        [Display(Name = "Department Name")]
        public long? OrganizationId { get; set; }
        [Display(Name = "Company")]
        public string OrganizationName { get; set; }

        [Display(Name = "Legal Entity")]
        public long? LegalEntityId { get; set; }
        [Display(Name = "Legal Entity")]
        public string LegalEntityName { get; set; }

        public DateTime? PayrollStartDate { get; set; }
        public DateTime? PayrollEndDate { get; set; }
        public int? YearMonth { get; set; }

        public long? PayrollId { get; set; }
        public long? PayrollRunId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DefaultDateFormat)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }


    }
}
