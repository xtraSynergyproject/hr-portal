using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace Synergy.App.ViewModel
{
    public class FacebookViewModel 
    {
        public int fbid { get; set; }
        public string search_entity { get; set; }
        public string uploaded_by { get; set; }
        public string page_url { get; set; }
        public string pagename { get; set; }
        public string pagelike { get; set; }
        public string page_category { get; set; }
        public string date_time { get; set; }
        public string post_message { get; set; }
        public string no_of_people_reacted { get; set; }
        public string no_of_comment { get; set; }
        public string image_url { get; set; }
        public string video_url { get; set; }
        public string post_url { get; set; }
        public string embed_url { get; set; }
        public string trial622 { get; set; }
        public string count { get; set; }
        public bool is_notified { get; set; }
        public bool is_alerted { get; set; }

    }
    public class FacebookArrayViewModel
    {
        
        public string[] pagename { get; set; }        
        public string[] post_message { get; set; }

    }
    public class SocialMediaChartViewModel
    {

        public string MediaType { get; set; }
        public string  Count { get; set; }

    }
}
