using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class HierarchyViewModel : ViewModelBase
    {
        [Display(Name = "Hierarchy Name")]
        public long HierarchyId { get; set; }
        [Required]
        [Display(Name = "Hierarchy Type")]
        public HierarchyTypeEnum HierarchyType { get; set; }
        [Required]
        [Display(Name = "Hierarchy Name")]
        public string Name { get; set; }
        public long? RootNodeId { get; set; }
        public string Description { get; set; }

        [Display(Name = "Level1 Name")]
        public string Level1Name { get; set; }
        [Display(Name = "Level2 Name")]
        public string Level2Name { get; set; }
        [Display(Name = "Level3 Name")]
        public string Level3Name { get; set; }
        [Display(Name = "Level4 Name")]
        public string Level4Name { get; set; }
        [Display(Name = "Level5 Name")]
        public string Level5Name { get; set; }
        [Required]
        [Display(Name = "Hierarchy Code")]
        public string Code { get; set; }

        public long? UserId { get; set; }
    }
}
