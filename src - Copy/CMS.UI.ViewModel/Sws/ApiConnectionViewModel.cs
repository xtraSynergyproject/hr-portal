using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace CMS.UI.ViewModel
{
    public class ApiConnectionViewModel : NoteTemplateViewModel
    {
        public string restApiUrl { get; set; }
        
        public string parameters { get; set; }
        
        public string userName { get; set; }
        
        public string password { get; set; }
        
        public string apiKey { get; set; }       
        public string pollingTime { get; set; }
    }
}
