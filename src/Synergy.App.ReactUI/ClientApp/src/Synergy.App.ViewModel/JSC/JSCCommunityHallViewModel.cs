using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{

    public class JSCCommunityHallViewModel
    {
        public string CommunityHallId { get; set; }
        public string CommunityType { get; set; }   
        public string CommunityTypeName { get; set; }   
        public string Name { get; set; }
        public double StandardRate { get; set; }
        public double SpecialRate { get; set; }
        public double JMCDiscountPercentage { get; set; }
        public bool IsAvailable { get; set; }
        public string WardName { get; set; }
        public string Ward { get; set; }
        public string PhotoId { get; set; }
        public string Details { get; set; }
        public DateTime CommunityBookingFromDate { get; set; }
        public DateTime CommunityBookingToDate { get; set; }
        public string CommunityBookingFromDateText { get; set; }
        public string CommunityBookingToDateText { get; set; }
        public string Disclaimer_Terms_Conditions { get; set; }
        public List<string> CommunityHallPhotoId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string MapArea { get; set; }
        public long NoOfDays { get; set; }
        public double Rate { get; set; }
        public double TotalAmount { get; set; }
    }
}
