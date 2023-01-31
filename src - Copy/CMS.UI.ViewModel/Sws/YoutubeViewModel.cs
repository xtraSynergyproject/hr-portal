using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace CMS.UI.ViewModel
{
    public class YoutubeViewModel
    {
        public int ytid { get; set; }
        public string search_entity { get; set; }
        public string videoid { get; set; }
        public string youtube_video_url { get; set; }
        public string publishedat { get; set; }
        public string channelid { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string channeltitle { get; set; }
        public string publishtime { get; set; }
        public string thumbnail_urls { get; set; }
        public string comments { get; set; } 
        public string count { get; set; }
        public bool is_notified { get; set; }
        public bool is_alerted { get; set; }

    }
    public class YoutubeArrayViewModel
    {
        
        public string[] title { get; set; }
        public string[] description { get; set; }
        


    }
}
