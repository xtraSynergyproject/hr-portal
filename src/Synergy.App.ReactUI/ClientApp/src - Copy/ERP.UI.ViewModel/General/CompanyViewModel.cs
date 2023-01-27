using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class CompanyViewModel : ViewModelBase
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
        public long CountryId { get; set; }
        public string CultureInfo { get; set; }
        public string DateFormat { get; set; }
        public string DateTimeFormat { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyName { get; set; }
        public string TimeZone { get; set; }
        public long? RootPositionId { get; set; }
        [Display(Name = "Root Department Name")]
        public long? RootOrganizationId { get; set; }
        public string SmtpSenderName { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpFromId { get; set; }
        public string SmtpUserId { get; set; }
        public string SmtpPassword { get; set; }
        public long DefaultUserId { get; set; }
        public string DefaultEmailTemplate { get; set; }
        public string SmsGateway { get; set; }
        public string SmsUserId { get; set; }
        public string SmsPassword { get; set; }
        public string SmsSenderName { get; set; }
        public long? LogoFileId { get; set; }
        public FileViewModel SelectedLogoFile { get; set; }
        public string LicenseKey { get; set; }
        public bool? SendCompanyWelcome { get; set; }
        [Display(Name = "Person Number Generation Type")]
        public PersonNumberGenerationTypeEnum? PersonNumberGenerationType { get; set; }

    }
}
