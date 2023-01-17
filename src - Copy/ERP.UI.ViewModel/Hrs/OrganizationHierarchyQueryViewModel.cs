using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class OrganizationHierarchyQueryViewModel : BaseViewModel
    {

        public int Id { get; set; }

        public int HierarchyTypeId { get; set; }

        [Display(Name = "Hierarchy Type")]
        public string HierarchyTypeName { get; set; }

        public int OrganizationId { get; set; }

        [Display(Name = "Organization")]
        public string OrganizationName { get; set; }

        [Required]
        public int ParentOrganizationId { get; set; }

        [Display(Name = "Parent Organization")]
        public string ParentOrganizationName { get; set; }
        
    }
}
