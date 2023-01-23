using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class PayrollRunPersonViewModel : NoteTemplateViewModel
    {    
        public string PersonId { get; set; }       
        public string PayrollRunId { get; set; }
       

    }
}
