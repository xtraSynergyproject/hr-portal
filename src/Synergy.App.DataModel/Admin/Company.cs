using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class Company : DataModelBase
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
        //public long RootPositionId { get; set; }
        //public long RootOrganizationId { get; set; }
        public string SmtpSenderName { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpFromId { get; set; }
        public string SmtpUserId { get; set; }
        public string SmtpPassword { get; set; }
        //public long? DefaultUserId { get; set; }
        public string DefaultEmailTemplate { get; set; }

        public string SmsGateway { get; set; }
        public string SmsUserId { get; set; }
        public string SmsPassword { get; set; }
        public string SmsSenderName { get; set; }
        public string LogoFileId { get; set; }
        public string LicenseKey { get; set; }
        public bool? SendCompanyWelcome { get; set; }

        public string CountryId { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonEmail { get; set; }
        public string ContactPersonMobile { get; set; }

        public long Count { get; set; }
        public DateTime? LastSyncDate { get; set; }

        public string LetterHeaderId { get; set; }
        public string LetterFooterId { get; set; }
        public string[] LicensedPortalIds { get; set; }

    }

    [Table("CompanyLog", Schema = "log")]
    public class CompanyLog : Company
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
