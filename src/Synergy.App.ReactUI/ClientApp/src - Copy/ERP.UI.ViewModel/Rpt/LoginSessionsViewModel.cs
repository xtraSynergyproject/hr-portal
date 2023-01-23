using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel.General
{
    public class LoginSessionsViewModel
    {
        public string UserId { get; set; }
        public string PersonFullName { get; set; }
        public int Jan { get; set; }
        public int Feb { get; set; }
        public int Mar { get; set; }
        public int Apr { get; set; }
        public int May { get; set; }
        public int Jun { get; set; }
        public int Jul { get; set; }
        public int Aug { get; set; }
        public int Sep { get; set; }
        public int Oct { get; set; }
        public int Nov { get; set; }
        public int Dec { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        [Display(Name = "Department Name")]
        public long OrganizationId { get; set; }

    }
}
