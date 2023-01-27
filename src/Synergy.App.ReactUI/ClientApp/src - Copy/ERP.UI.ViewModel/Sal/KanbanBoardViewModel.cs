using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class KanbanBoardViewModel 
    {
        public long ListID { get; set; }
        //public string name { get; set; }
        public string ColorClass { get; set; }
        //public long cardID { get; set; }
        //public string TemplateName { get; set; }
        public string LeadStatus { get; set; }
        public string LeadStatusActual { get; set; }
        public long LeadCount { get; set; }
        
        
                
                 
                
    }
}

