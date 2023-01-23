﻿using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
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
        public string PhotoId { get; set; }
    }
}
