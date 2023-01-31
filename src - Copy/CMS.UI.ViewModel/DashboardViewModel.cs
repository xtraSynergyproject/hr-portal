
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CMS.Common;
using Kendo.Mvc.UI;

namespace CMS.UI.ViewModel
{
    public class DashboardViewModel
    {
        
        public string ModuleCodes { get; set; }
        public string UserId { get; set; }
        public string[] TaskTemplateIds { get; set; }
        public string[] NoteTemplateIds { get; set; }
        public string[] ServiceTemplateIds { get; set; }
    }
}
