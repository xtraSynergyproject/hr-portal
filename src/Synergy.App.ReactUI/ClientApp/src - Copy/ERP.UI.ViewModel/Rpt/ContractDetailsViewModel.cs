using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class ContractDetailsViewModel
    {
       
        public string EmployeeNo { get; set; }
        public string Name { get; set; }
        public string Jan { get; set; }
        public string Feb { get; set; }
        public string Mar { get; set; }
        public string Apr { get; set; }
        public string May { get; set; }
        public string Jun { get; set; }
        public string Jul { get; set; }
        public string Aug { get; set; }
        public string Sep { get; set; }
        public string Oct { get; set; }
        public string Nov { get; set; }
        public string Dec { get; set; }
        public DateTime ContractEndDate { get; set; }
        public DateTime DateOfJoin { get; set; }
        [Display(Name = "Expired Department Name")]
        public string ExpiredOrganization { get; set; }
        public string ExpiredPosition { get; set; }
        public string ExpiredJob { get; set; }
        public long? BrandId { get; set; }
        [Display(Name = "Department Name")]
        public long? OrganizationId { get; set; }
        public string ContractRenewable { get; set; }
    }
}
