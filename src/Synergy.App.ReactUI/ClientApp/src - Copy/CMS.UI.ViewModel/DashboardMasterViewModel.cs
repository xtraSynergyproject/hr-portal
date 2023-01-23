using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace CMS.UI.ViewModel
{
    public class DashboardMasterViewModel : NoteTemplateViewModel
    {
        public string layoutMetadata { get; set; }       
        public bool gridStack { get; set; }       

    }
}
