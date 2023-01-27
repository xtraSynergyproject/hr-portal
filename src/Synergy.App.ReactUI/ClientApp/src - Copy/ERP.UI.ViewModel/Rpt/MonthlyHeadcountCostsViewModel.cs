using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class MonthlyHeadcountCostsViewModel
    {
        public string BrandName { get; set; }
        public string RestaurantName { get; set; }
        [Display(Name = "Department Name")]
        public long? OrganizationId { get; set; }
        public int HeadCount { get; set; }
        public string TotalCost { get; set; }
        public string AverageCost { get; set; }
        public string YearMonth { get; set; }
}
}
