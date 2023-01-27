using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class PayrollRunPersonViewModel : NoteTemplateViewModel
    {    
        public string PersonId { get; set; }       
        public string PayrollRunId { get; set; }
       

    }
}
