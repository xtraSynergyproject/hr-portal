using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class UdfPermissionSettingsViewModel : UdfPermissionHeader
    {
        
        public string UserName { get; set; }
        public string TeamName { get; set; }       
        public string PageName { get; set; }             
        public string PrimaryTemplateName { get; set; }      
        public string Category { get; set; }
        public string Template { get; set; }
    }
}
