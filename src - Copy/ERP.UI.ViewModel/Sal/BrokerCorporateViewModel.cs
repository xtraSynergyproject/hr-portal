using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel

{
    public class BrokerCorporateViewModel : ViewModelBase
    {
        [Display(Name = "CR Number")]
        public string CRNumber { get; set; }

        [Display(Name = "ID(Attach)")]
        public int ID { get; set; }

        [Display(Name = "CR Date")]
        public DateTime CRDate { get; set;}

        public string VAT { get; set;}

        [Display(Name = "Company Name")]
        public string CRAttachment { get; set; }

        [Display(Name = "Contact Person")]
        public long PersonId { get; set; }

        public string PersonName { get; set; }

        public string Nationality { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        [Display(Name = "Company Address")]
        public string Address { get; set; }

        public string Street { get; set; }
        public string City { get; set; }
       

        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }


        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [Display(Name = "Swift Code")]
        public string SwiftCode { get; set; }
        public string AccountNo { get; set; }
        public string Branch { get; set; }
        public string IBAN { get; set; }
        public string ServiceStatus { get; set; }
        public string serviceId { get; set; }
        public string RERA { get; set; }
    }
}

