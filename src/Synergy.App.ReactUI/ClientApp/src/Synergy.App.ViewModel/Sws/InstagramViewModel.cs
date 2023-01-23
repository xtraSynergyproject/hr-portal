using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace Synergy.App.ViewModel
{
    public class InstagramViewModel
    {
        public int id { get; set; }
        public string search_keyword { get; set; }
        public string post_links { get; set; }
        public string image_links { get; set; }          
        public string likes { get; set; }          
        public string age { get; set; }          
        public string caption { get; set; }          
        public string hashtags { get; set; } 
        public string count { get; set; }
        public bool is_notified { get; set; }
        public bool is_alerted { get; set; }


    }
    public class InstagramArrayViewModel
    {
        
        public string[] hashtags { get; set; }
        public string[] caption { get; set; }
        


    }
    public class ElasticSerachViewModel
    {
        public string updateDate { get; set; }
        public string metadata { get; set; }
        public string extension { get; set; }
        public string documentType { get; set; }
        public string subject { get; set; }
        public string description { get; set; }
        public string userId { get; set; }
        public string templateCategory { get; set; }
        public string noteno { get; set; }
        public string name { get; set; }
        public string versionNo { get; set; }
        public string extracttext { get; set; }
        public string fileId { get; set; }
        public string status { get; set; }


    }
    public class ElasticSerach1ViewModel
    {
        public string[] subject { get; set; }
        public string[] description { get; set; }
        public string[] metadata { get; set; }
    }
}
