using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{

    public class EGovBinBookingViewModel
    {
        public string ApplicantName { get; set; }
        public DateTime? BookingFromDate { get; set; }
        public DateTime? BookingToDate { get; set; }
        public string ContactNumber { get; set; }  
        public string Email { get; set; }
        public string WardId { get; set; }
        public string Address { get; set; }
        public string BinTypeId { get; set; }
        public string BinSizeId { get; set; }
        public long NumberOfBins { get; set; }
        public string BinClearanceCycleId { get; set; }
        public string BinBookingConsumerNo { get; set; }
        public string ParentageName { get; set; }        
        public string ServiceId { get; set; }
    }
}
