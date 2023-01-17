using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class CSCReportAcknowledgementViewModel
    {
        public string ApplicationReferenceNumber { get; set; }
        public DateTime? DateOfApplication { get; set; }
        public string DateOfApplicationText { get; set; }
        public string ApplicationServiceName { get; set; }
        public string ServiceChargeName { get; set; }
        public double? ServiceChargeAmount { get; set; }
        public string ServiceChargeKioskName { get; set; }
        public double? ServiceChargeKioskAmount { get; set; }
        public double? TotalFeesPaid { get; set; }
        public string PaymentDetails { get; set; }
        public DateTime? DeliveryDateOfService { get; set; }
        public string DeliveryDateOfServiceText { get; set; }
        public string DeliverableDetails { get; set; }
        public string OfficeType { get; set; }
        public string Distict { get; set; }
        public string Tehsil { get; set; }
        public string NagarPanchayatName { get; set; }
        public string NagarPanchayatNameHi { get; set; }
        public string MunicipalityName { get; set; }
        public string MunicipalityNameHi { get; set; }
        public string ZoneName { get; set; }
        public string ZoneNameHi { get; set; }
        public string RevenueVillage { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantAddress { get; set; }
        public string ApplicantDistict { get; set; }
        public string ApplicantState { get; set; }
        public string ApplicantEmail { get; set; }
        public string ApplicantMobile { get; set; }
        public string OfficerName { get; set; }
        public string OfficerAddress { get; set; }
        public string OfficerMobile { get; set; }
        public string OfficerSignature { get; set; }
        public string SealLogo { get; set; }
        public string GovtLogo { get; set; }
        public string ChipsLogo { get; set; }
        public List<ServiceChargeViewModel> ServiceChargeData { get; set; }
    }
}
