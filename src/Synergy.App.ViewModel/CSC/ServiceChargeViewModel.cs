using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class ServiceChargeViewModel
    {        
        public string ChargeName { get; set; }  
        public string ChargeCode { get; set; }
        public long Amount { get; set; }     
        public int FeeExcemptionQuantity { get; set; }
        public string Document1Id { get; set; }
        public string Document2Id { get; set; }
        public string Document3Id { get; set; }
        public string Document4Id { get; set; }
        public string Document5Id { get; set; }
        public string Document6Id { get; set; }
         
    }
}
