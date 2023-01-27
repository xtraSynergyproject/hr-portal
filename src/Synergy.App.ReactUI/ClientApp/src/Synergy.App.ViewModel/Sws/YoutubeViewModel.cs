using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace Synergy.App.ViewModel
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
    public class Youtube1ViewModel :polairty
    {

        public string Id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string channelTitle { get; set; }
        public DateTime publishTime { get; set; }
        public string keyword { get; set; }

    }
    public class YoutubeCommentViewModel
    {

        public string Id { get; set; }
        public string videoId { get; set; }
        public string textDisplay { get; set; }
        public string textOriginal { get; set; }
        public string authorDisplayName { get; set; }
        public DateTime publishedAt { get; set; }
        public DateTime updatedAt { get; set; }


    }
    public class Twitter1ViewModel : public_metrics 
    {

        public string id { get; set; }
        public string text { get; set; }        
        public string source { get; set; }        
        public bool possibly_sensitive { get; set; }        
        public DateTime created_at { get; set; }                
        public string keyword { get; set; }                


    }
    public class referenced_tweets : polairty
    {  
        public string type { get; set; }
    }
    public class public_metrics : referenced_tweets
    {
        public int retweet_count { get; set; }
        public int reply_count { get; set; }
        public int quote_count { get; set; }
        public int like_count { get; set; }
    }
    public class polairty
    {
        public double pos { get; set; }
        public double neg { get; set; }
        public double neu { get; set; }
        public double compound { get; set; }
    }
    public class RoipViewModel
    {
        ////public string Id { get; set; }
        //public bool heard { get; set; }
        //public string f_type { get; set; }
        //public bool archive_info { get; set; }
        public string channel_no { get; set; }
        public string channel_name { get; set; }
        public string date_value { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string dur_time { get; set; }

        public DateTime start_datetime { get; set; }       


        //public TimeSpan start_timeDisplay
        //{
        //    get
        //    {
        //        var d = TimeSpan.FromMilliseconds(start_time);
        //        return d;
        //    }
        //}
        //public TimeSpan end_timeDisplay
        //{
        //    get
        //    {
        //        var d = TimeSpan.FromMilliseconds(end_time);
        //        return d;
        //    }
        //}
        //public TimeSpan dur_timeDisplay
        //{
        //    get
        //    {
        //        var d = TimeSpan.FromMilliseconds(dur_time);
        //        return d;
        //    }
        //}
        //public string phone_no { get; set; }
        //public string in_out { get; set; }
        //public string impcall { get; set; }
        public string file_name { get; set; }
        //public string remarks1 { get; set; }
        //public string remarks2 { get; set; }
        //public string remarks3 { get; set; }
        //public string remarks4 { get; set; }
        //public string remarks5 { get; set; }
        //public string remarks6 { get; set; }
        //public string remarks7 { get; set; }
    }
}
