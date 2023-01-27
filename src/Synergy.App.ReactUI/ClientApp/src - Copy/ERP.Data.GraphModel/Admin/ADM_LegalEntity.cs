
using ERP.Utility;

namespace ERP.Data.GraphModel
{

    public partial class ADM_LegalEntity : NodeBase
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string LegalEntityCode { get; set; }
        public string Town { get; set; }
        public string PostalCode { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNo { get; set; }
        public string BankIBanNo { get; set; }
        public string ContactPerson { get; set; }
        public double BasicSalaryPercentage { get; set; }
        public double HousingAllowancePercentage { get; set; }
        public double TransportAllowancePercentage { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public int? FiscalYearStartMonth { get; set; }
        public int? FiscalYearEndMonth { get; set; }
        public FiscalYearTypeEnum? FiscalYearType { get; set; }

        public string TravelAgentEmail { get; set; }
        public bool? IsBiometricOTEnabled { get; set; }
        public bool? IsBiometricDeductionEnabled { get; set; }
        public double? VatPercentage { get; set; }
        public string VatRegistrationNo { get; set; }
        public AssessmentReportTemplateEnum? AssessmentReportTemplate { get; set; }
        public string AssessmentReportSummary { get; set; }
        public string AssessmentReportColor { get; set; }
    }


    public class R_LegalEntity_OrganizationRoot : RelationshipDatedBase
    {

    }
    public class R_LegalEntity_Address_Country : RelationshipDatedBase
    {

    }
    public class R_LegalEntity_BankBranch : RelationshipDatedBase
    {

    }
}
