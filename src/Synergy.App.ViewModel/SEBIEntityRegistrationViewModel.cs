using Synergy.App.Common;
using Synergy.App.DataModel;
using System;

namespace Synergy.App.ViewModel
{
    public class SEBIEntityRegistrationViewModel
    {
        public string ServiceId { get; set; }
        public string ServiceNo { get; set; }
        public string EntityTypeName { get; set; }
        public string NameOfTheCompany { get; set; }
        public string RegisteredOfficeAddressOfCompany { get; set; }
        public string SelectState { get; set; }
        public string SelectStatusOfTheCompany { get; set; }
        public string IssueSize { get; set; }
        public string NameOfRegistrarsTransferAgentIfThroughRta { get; set; }
        public string StockExchangeForComplaints { get; set; }
        public string PreviousNamesOfTheCompany { get; set; }
        public bool BombayStockExchangeLtd { get; set; }
        public bool CalcuttaStockExchangeLtd { get; set; }
        public bool MetroplitanStockExchangeLimitedPreviousNameMcxStockExchangeLtd { get; set; }
        public bool NationalStockExchangeOfIndiaLtd { get; set; }
        public string PANOfTheCompany { get; set; }
        public string CINOfTheCompany { get; set; }
        public string Designation { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddressPrimary { get; set; }
        public string EmailAddressAlternate { get; set; }
        public DateTime? DateOfIncorporationOfTheCompany { get; set; }
        public string Pincode { get; set; }
        public string CompliancesDealingOfficer { get; set; }

    }
}
