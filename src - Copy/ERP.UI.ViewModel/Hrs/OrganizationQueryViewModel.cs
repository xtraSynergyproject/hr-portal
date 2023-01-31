using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class OrganizationQueryViewModel : BaseViewModel
    {

        public int Id { get; set; }

        [Display(Name = "GRP Organization Id")]
        public int GRPOrganizationId { get; set; }

        [Required]
        [Display(Name = "Organization Name")]
        public string Name { get; set; }

        [Display(Name = "Organization Name (In Arabic)")]
        public string NameAr { get; set; }

        public string Description { get; set; }

        [Required]
        public int CostCenterId { get; set; }

        [Display(Name = "Cost Center Name")]
        public string CostCenterName { get; set; }

        [Required]
        public int LocationId { get; set; }

        [Display(Name = "Location")]
        public string LocationName { get; set; }

        [Required]
        public int OrganizationTypeId { get; set; }

        [Display(Name = "Organization Type")]
        public string OrganizationTypeName { get; set; }

    }
}
