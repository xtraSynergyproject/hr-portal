using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;


namespace CMS.UI.ViewModel
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
