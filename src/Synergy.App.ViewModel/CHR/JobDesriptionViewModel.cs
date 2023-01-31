using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
   public class JobDesriptionViewModel:NoteTemplateViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string NtsNoteId { get; set; }
        public string JobDescription { get; set; }
        public string Responsibility { get; set; }
        public string JobId { get; set; }
        public string jobCategory { get; set; }
        public string QualificationId { get; set; }
    
        
        public string Experience{ get; set; }
        public long Experience1 { get; set; }
        public string JobCriteria { get; set; }
        public string Skills { get; set; }

        public string OtherInformation { get; set; }
    }



}
