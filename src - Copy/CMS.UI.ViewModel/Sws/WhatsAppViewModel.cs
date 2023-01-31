using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace CMS.UI.ViewModel
{
    public class WhatsAppViewModel
    {
        public int? id { get; set; }
        public string owner { get; set; }
        public string user { get; set; }
        public string messages { get; set; }       
        public string trial782 { get; set; }
        public string count { get; set; }
        public bool is_notified { get; set; }
        public bool is_alerted { get; set; }


    }
    public class WhatsAppArrayViewModel
    {
        
        public string[] user { get; set; }
        public string[] messages { get; set; }
       


    }
}
