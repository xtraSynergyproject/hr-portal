using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class LegalEntity : DataModelBase
    {
        public string Name { get; set; }
        public string NameLocal { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string PrimaryContactPerson { get; set; }
        public string SecondaryContactPerson { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string SecondaryContactEmail { get; set; }
        public string PrimaryContactMobile { get; set; }
        public string SecondaryContactMobile { get; set; }
        public string PrimaryContactPhone { get; set; }
        public string SecondaryContactPhone { get; set; }
        //public long CountryId { get; set; }
        public string CultureInfo { get; set; }
        public string DateFormat { get; set; }
        public string DateTimeFormat { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyName { get; set; }
        public string TimeZone { get; set; }

        public string LogoFileId { get; set; }
        public string LicenseKey { get; set; }
        public bool? SendCompanyWelcome { get; set; }

        public string CountryId { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonEmail { get; set; }
        public string ContactPersonMobile { get; set; }
        public double BasicSalaryPercentage { get; set; }
        public double HousingAllowancePercentage { get; set; }
        public double TransportAllowancePercentage { get; set; }

        public string MobileNo { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string PinCode { get; set; }
        public string PANNo { get; set; }
        public string TANNo { get; set; }
        public string GSTNo { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Street { get; set; }
    }
    [Table("LegalEntityLog", Schema = "log")]
    public class LegalEntityLog : LegalEntity
    {
        public string RecordId { get; set; }
        public long LogVersionNo { get; set; }
        public bool IsLatest { get; set; }
        public DateTime LogStartDate { get; set; }
        public DateTime LogEndDate { get; set; }
        public DateTime LogStartDateTime { get; set; }
        public DateTime LogEndDateTime { get; set; }
        public bool IsDatedLatest { get; set; }
        public bool IsVersionLatest { get; set; }
    }
}
