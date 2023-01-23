using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace Synergy.App.ViewModel
{
    public class FacebookPostViewModel : polairty
    {
        public string post_url { get; set; }
        public string uploaded_by { get; set; }
        public string page_url { get; set; }
        public string page_name { get; set; }
        public string no_of_likes { get; set; }
        public string no_of_comments { get; set; }
        public string no_of_shares { get; set; }
        public string post_msg { get; set; }  
        public DateTime post_date { get; set; }
        public string keyword { get; set; }        

    }
    public class FacebookUserViewModel
    {
        public string profile_url { get; set; }        
        public string profile_pic_url { get; set; }
        public string no_of_friends { get; set; }
        public string user_id { get; set; }
        public string scrape_date { get; set; }
        public string join_date { get; set; }
        public string scrape_tag { get; set; }
                

    }
    public class FacebookFriendViewModel
    {
        public string friend_name { get; set; }
        public string friend_detail { get; set; }
        public string friend_profile_pic_url { get; set; }
        public string friend_profile_url { get; set; }
        public string parent_id { get; set; }

    }
    public class FacebookUserPostViewModel : polairty
    {
        public string post_title { get; set; }
        public string post_msg { get; set; }
        public string post_status { get; set; }
        public string post_likes { get; set; }
        public string no_of_comment { get; set; }
        public string no_of_share { get; set; }
        public string parent_id { get; set; }
        public string post_date { get; set; }
        public string post_media_url { get; set; }

    }
    public class FacebookCommonFriendViewModel
    {
        public string FacebookFriend_friend_profile_url { get; set; }
        public string FacebookFriend_friend_name { get; set; }
        public int FacebookFriend_count { get; set; }
    }
}
