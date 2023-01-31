using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
   public class beneficiaryViewModel
    {
        public string AccountNumber1 { get; set; }
        public string Iban1 { get; set; }
        public string SwiftCode1 { get; set; }
        public string Branch1 { get; set; }

    }


    public class JobDetViewModel
    {
       public string Id { get; set; }
        public string OrganizationId { get; set; }
        public string JobId { get; set; }
        
        public string DepartmentName { get; set; }
        public string JobTitle { get; set; }

        public string JobDescription { get; set; }
        public string Responsibility { get; set; }

    }
}
