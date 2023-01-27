using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class ContactsViewModel:NoteTemplateViewModel
    {
        public string VendorName { get; set; }
        public string VendorId { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string ContactPersonName { get; set; }
        public string MobileNo1 { get; set; }
        public string MobileNo2 { get; set; }        
        public string ContactNo1 { get; set; }
        public string ContactNo2 { get; set; }
        public string City { get; set; }
        public string CityName { get; set; }
        public string ContactNo3 { get; set; }
        public string Fax { get; set; }
        public string CityCode { get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }       
        public string TANNo { get; set; }
       
    }
}
