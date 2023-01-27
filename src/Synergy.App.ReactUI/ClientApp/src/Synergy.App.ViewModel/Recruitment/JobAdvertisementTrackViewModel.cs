using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;


namespace Synergy.App.ViewModel
{
    public class JobAdvertisementTrackViewModel : JobAdvertisementTrack
    {
        public string JobName { get; set; }
        public string JobCriteria { get; set; }
        public string Skills { get; set; }
        public string OtherInformation { get; set; }
        public string SaveType { get; set; }
        
    }
}
