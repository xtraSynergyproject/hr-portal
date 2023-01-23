using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
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
