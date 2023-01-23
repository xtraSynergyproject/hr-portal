using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{

    public class EGovCommunityHallViewModel
    {
        public string CommunityHallName { get; set; }
        public DateTime? BookingFromDate { get; set; }
        public DateTime? BookingToDate { get; set; }
        public DateTime? NextAvailableDate { get; set; }
        public DateTime?[] DisableDates { get; set; }
        public string Status { get; set; }  
        public double ChargesLeviedPerDay { get; set; }
        public double ACCharges { get; set; }
        public double ElectricityCharges { get; set; }
        public double CleaningCharges { get; set; }
        public string WardName { get; set; }
        public string Ward { get; set; }
    }
}
