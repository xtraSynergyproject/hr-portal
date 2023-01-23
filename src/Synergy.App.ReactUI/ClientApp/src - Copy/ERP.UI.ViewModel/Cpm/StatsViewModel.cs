using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class StatsViewModel : ViewModelBase
    {

        public string Type { get; set; }
        public int Value { get; set; }

        public int[] Value1 { get; set; }
        public string Color { get; set; }


        public long TotalUnitCount { get; set; }
        public long ActiveContractCount { get; set; }
        public double TotalRentValue { get; set; }
        public double ActiveRentValue { get; set; }
        public double ReceivedRentValue { get; set; }
        public double PendingRentValue { get; set; }
        public long TotalMainCount { get; set; }
        public long PendingMainCount { get; set; }
        public long InprogressMainCount { get; set; }
        public long RejectMainCount { get; set; }
     
        public int CountryCount { get; set; }
        public string FromCountryName { get; set; }
 
    }
}
