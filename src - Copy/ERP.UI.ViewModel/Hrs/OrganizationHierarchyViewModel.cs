using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class OrganizationHierarchyViewModel : DatedViewModelBase
    {
        //public long OrganizationHierarchyId { get; set; }
        [Required]
        [Display(Name = "Hierarchy Name")]
        public Nullable<long> HierarchyId { get; set; }

        public string HierarchyName { get; set; }
        [Required]
        [Display(Name = "Department Name")]
        public long? OrganizationId { get; set; }
        
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
        [Required]
        [Display(Name = "Parent Department")]
        public long? ParentOrganizationId { get; set; }

        [Display(Name = "Proposed Parent Department")]
        public string ParentOrganizationName { get; set; }

        [Display(Name = "Relationship")]
        public Nullable<long> RelationshipId { get; set; }      

        public string Remarks { get; set; }
        public DateTime AsOnDate { get; set; }

    }
}