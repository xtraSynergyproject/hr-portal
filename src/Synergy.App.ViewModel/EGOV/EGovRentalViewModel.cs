using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{

    public class EGovRentalViewModel
    {
        public string PropertyAmenitiesId { get; set; }
        public long RentalAmount { get; set; }
        public long DepositAmount { get; set; }
        public string AreaInSqFt { get; set; }
        public string PropertyId { get; set; }
        public string ServiceId { get; set; }
        public string ApplicantName { get; set; }
        public DateTime? AgreementStartDate { get; set; }
        public DateTime? AgreementEndDate { get; set; }
        public string RentFrequency { get; set; }
        public string Tenure { get; set; }
        public string TradeBusinessName { get; set; }
        public string RentalAgreementNumber { get; set; }
        public string PropertyRentingType { get; set; }
        public string WardId {get;set;}
        public string BuildingNumber {get;set;}
        public string Street {get;set;}
        public string LocalitySpecificLocation{get;set;}

}
}
