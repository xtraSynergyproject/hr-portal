using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class PartnerRegistrationViewModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string LandlineNo { get; set; }
        public string DateOfBirth { get; set; }
        public bool Gender { get; set; }
        public string Profession { get; set; }
        public string CountryId { get; set; }
        public string StateId { get; set; }
        public string OrganisationName { get; set; }
        public string ContactPerson { get; set; }
        public string PinCode { get; set; }
        public string Designation { get; set; }
        public string Gstin { get; set; }
        public string Pan { get; set; }
        public string Captcha { get; set; }
        public string Tan { get; set; }
       
        public string City { get; set; }
        public string Town { get; set; }
        public string Address { get; set; }
        public DataActionEnum DataAction { get; set; }
        public string Json { get; set; }
        public string PageId { get; set; }



    }
}
