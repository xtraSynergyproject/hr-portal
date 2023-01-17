using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
namespace ERP.UI.ViewModel
{
    public class CompanySearchViewModel : SearchViewModelBase
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
        public long RootPositionId { get; set; }
        [Display(Name = "Root Department Name")]
        public long RootOrganizationId { get; set; }

    }
}

