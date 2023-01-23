using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class PositionHierarchyViewModel : DatedViewModelBase
    {
        //public long PositionHierarchyId { get; set; }  
        
        [Display(Name = "Hierarchy Name")]
        public long? HierarchyId { get; set; }

        [Display(Name = "Hierarchy Name")]
        public string HierarchyName { get; set; }
        [Required]
        [Display(Name = "Position")]
        public long? PositionId { get; set; }
       // [Required]
        [Display(Name = "Parent Position")]
        public long? ParentPositionId { get; set; }

        [Display(Name = "Position")]
        public string PositionName { get; set; }
        
       

        [Display(Name = "Proposed Parent Position")]
        public string ParentPositionName { get; set; }

        public string Remarks { get; set; }
        public DateTime AsOnDate { get; set; }
        public long? RelationshipId { get; set; }
        [Display(Name = "Legal Entity")]
        public long LegalEntityId { get; set; }
        [Display(Name = "Legal Entity")]
        public string LegalEntityName { get; set; }
    }
}
