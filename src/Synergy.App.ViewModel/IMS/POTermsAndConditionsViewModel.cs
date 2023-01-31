using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class POTermsAndConditionsViewModel:NoteTemplateViewModel
    {        
        public string POTCID { get; set; }
        public string POID { get; set; }
        public string TermsId { get; set; }
        public string TermsTitle { get; set; }
        public string TermsDescription { get; set; }
        public string TermsList { get; set; }
        public string Title { get; set; }


    }
}

