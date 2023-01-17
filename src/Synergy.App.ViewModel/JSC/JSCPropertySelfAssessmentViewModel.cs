using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
   public class JSCPropertySelfAssessmentViewModel : ServiceTemplateViewModel
    {
        public DateTime EffectFromDate { get; set; }
        public DateTime EffectToDate { get; set; }
        public string PropertyType { get; set; }
        public string DdnNo { get; set; }
        public string PropertyName { get; set; }
        public double PropertyRate { get; set; }
        public string BuildingCategory { get; set; }
        public string BuildingTypeName { get; set; }
        public string BuildingType { get; set; }
        public double BuildingRate { get; set; }
        public double LocationRate { get; set; }
        public string OccupancyId { get; set; }
        public string OccupancyName { get; set; }
        public double OccupancyRate { get; set; }
        public string LandValueId { get; set; }
        public string LandValue { get; set; }
        public string WardId { get; set; }
        public double TotalAmount { get; set; }
        public double TotalArea { get; set; }
        public double PlinthArea { get; set; }
        public List<JSCPropertyFloorViewModel> FloorDetail { get; set; }
        public double VacantArea { get; set; }
        public double VacantAmount { get; set; }
        public string Year { get; set; }
        public int BuildingAge { get; set; }

    }
}
