using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace Synergy.App.ViewModel
{
    public class InstagramPostViewModel
    {
        public string url { get; set; }
        public DateTime created_date { get; set; }
        public string keyword { get; set; }
    }
   
}
