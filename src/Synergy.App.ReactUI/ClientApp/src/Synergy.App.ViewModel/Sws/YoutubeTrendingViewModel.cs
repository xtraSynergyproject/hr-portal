using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace Synergy.App.ViewModel
{
    public class YoutubeTrendingViewModel
    {
        public string videoId { get; set; }
        public DateTime publishedAt { get; set; }
        public string channelId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string channelTitle { get; set; }
        public string viewCount { get; set; }       
        public string likeCount { get; set; }       
        public string favoriteCount { get; set; }       
        public string commentCount { get; set; }       
        public string thumbnailUrl { get; set; }       
        public string location { get; set; }       

    }    
}
