using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace Synergy.App.ViewModel
{
    public class TwitterTrendingViewModel
    {
        public string name { get; set; }
        public string url { get; set; }
        public string promoted_content { get; set; }
        public string query { get; set; }
        public string tweet_volume { get; set; }       
        public DateTime created_date { get; set; }       
        public string location { get; set; }       

    }    
}
