using ERP.Utility;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERP.UI.ViewModel
{
    public class LegalEntityViewModel : ViewModelBase
    {
        [Required]
        [Display(Name = "Legal Entity Name")]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Town { get; set; }

        [Display(Name = "Postal Code")]

        public string PostalCode { get; set; }
        [Display(Name = "Legal Entity Code")]
        public string LegalEntityCode { get; set; }

        public string Telephone { get; set; }

        public string Fax { get; set; }

        public string Mobile { get; set; }


        [Display(Name = "Bank Account Name")]
        public string BankAccountName { get; set; }

        public string BankName { get; set; }
        public string BankBranchName { get; set; }

        [Display(Name = "Bank Account No")]
        public string BankAccountNo { get; set; }

        public string BankSwiftCode { get; set; }

        [Display(Name = "Bank IBAN No")]
        public string BankIBanNo { get; set; }
        [Required]
        [Display(Name = "Department Name")]
        public long? OrganizationId { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        //[Required]
        [Display(Name = "Country")]
        public long? CountryId { get; set; }

        [Display(Name = "Country")]
        public string CountryName { get; set; }

        [Display(Name = "Bank Branch")]
        public long? BankBranchId { get; set; }
        [Display(Name = "Bank Branch")]
        public string BankBranch { get; set; }

        [Display(Name = "Basic Salary Percentage")]
        public double? BasicSalaryPercentage { get; set; }
        [Display(Name = "Housing Allowance Percentage")]
        public double? HousingAllowancePercentage { get; set; }
        [Display(Name = "Transport Allowance Percentage")]
        public double? TransportAllowancePercentage { get; set; }
        [Display(Name = "Currency Code")]
        public string CurrencyCode { get; set; }
        [Display(Name = "Currency Name")]
        public string CurrencyName { get; set; }


        public MonthEnum? FiscalYearStartMonth { get; set; }
        public MonthEnum? FiscalYearEndMonth { get; set; }
        public FiscalYearTypeEnum? FiscalYearType { get; set; }

       // [Required]
        [Display(Name = "Travel Agent Email")]
        public string TravelAgentEmail { get; set; }

        public long? LegalEntityFolderId { get; set; }

        public long? userId { get; set; }
        [Display(Name = "Vat Percentage")]
        public double? VatPercentage { get; set; }
        [Display(Name = "Vat Registration No")]
        public string VatRegistrationNo { get; set; }
        [Display(Name = "Assessment Report Template")]
        public AssessmentReportTemplateEnum? AssessmentReportTemplate { get; set; }
        [Display(Name = "Assessment Report Summary")]
        public string AssessmentReportSummary { get; set; }
        [Display(Name = "Assessment Report Color")]
        public string AssessmentReportColor { get; set; }
    }
}
