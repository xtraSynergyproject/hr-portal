using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
   public class PropertyPriceViewModel : ViewModelBase
    {
        [Required]
        [Display(Name = "Project Name")]
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
        [Required]
        [Display(Name = "Unit Type")]
        public long UnitTypeId { get; set; }
        public string UnitType { get; set; }
        public string Area { get; set; }
        [Required]
        public long? Price { get; set; }
        public string FloorPlan { get; set; }
        public string PreviousFloorPlan { get; set; }
    }
}
