using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class WBSViewModel 
    {
        public string Id { get; set; }
        public string ItemNo { get; set; }       
        public string Subject { get; set; }        
        public string ParentId { get; set; }          
        public string Type { get; set; }          
        public DateTime? StartDate { get; set; }     
        public DateTime? DueDate { get; set; }
        public bool lazy { get; set; }
        public string title { get; set; }
        public string key { get; set; }

    }
   
}
