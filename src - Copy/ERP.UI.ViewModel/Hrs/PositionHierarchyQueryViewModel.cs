using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class PositionHierarchyQueryViewModel : BaseViewModel
    {

        public int Id { get; set; }

        public int HierarchyTypeId { get; set; }

        [Display(Name = "Hierarchy Type")]
        public string HierarchyTypeName { get; set; }

        public int PositionId { get; set; }

        [Display(Name = "Position")]
        public string PositionName { get; set; }

        [Required]
        public int ParentPositionId { get; set; }

        [Display(Name = "Parent Position")]
        public string ParentPositionName { get; set; }
        
    }
}
