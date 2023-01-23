using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel.Sal
{
    public class BrokerIndividualViewModel : ViewModelBase
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Nationality")]
        public long? NationalityId { get; set; }
        [Display(Name = "Nationality")]
        public string NationalityName { get; set; }
        public string Mobile { get; set; }
        [Display(Name = "E-Mail")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Address { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        [Display(Name = "ID Type")]
        public string IDType { get; set; }
        [Display(Name = "ID Number")]
        public string IDNumber { get; set; }
        [Display(Name = "Passport Number")]
        public string PassportNumber { get; set; }
        [Display(Name = "Swift Code")]
        public string SwiftCode { get; set; }
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public string Branch { get; set; }
        public string IBAN { get; set; }
        public string ServiceStatus { get; set; }
        public string ServiceId { get; set; }
        public string RERA { get; set; }
        public long? Lead { get; set; }

    }
}
