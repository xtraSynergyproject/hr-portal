﻿using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace Synergy.App.ViewModel
{
    public class TwitterViewModel
    {
        public string id { get; set; }
        public string created_at { get; set; }
        public string text { get; set; }
        public string source { get; set; }
        public string type { get; set; }
        public string possibly_sensitive { get; set; }
        public string hashtags { get; set; }
        public string url { get; set; }
        public string location { get; set; }
        public int retweet_count { get; set; }
        public int reply_count { get; set; }
        public int quote_count { get; set; }
        public string img_link { get; set; }
        public string video_link { get; set; }
        public string retweet_img_link { get; set; }
        public string retweet_video_link { get; set; }       
        public string trial756 { get; set; }
        public string count { get; set; }
        public bool is_notified { get; set; }
        public bool is_alerted { get; set; }
        public DateTime CreateDate { get; set; }


    }
    public class TwitterArrayViewModel
    {
        public string[] text { get; set; }
        public string[] hashtags { get; set; }
        public string[] location { get; set; }
    }
}