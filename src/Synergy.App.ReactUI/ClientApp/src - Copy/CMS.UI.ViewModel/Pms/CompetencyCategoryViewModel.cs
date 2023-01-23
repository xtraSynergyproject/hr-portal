using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class CompetencyCategoryViewModel: NoteTemplateViewModel
    {
       public string CategoryName { get; set; }

        public string CategoryCode { get; set; }

        public string CategoryDescription { get; set; }


        public string NtsNoteId { get; set; }
        public string MasterId { get; set; }
        public string[] MasterIds { get; set; }
        public string CompetencyName { get; set; }
        public string JobId { get; set; }
        public string[] JobIds { get; set; }
        public string JobTitle { get; set; }
        public string ProficiencyLevelId { get; set; }
        public string ProficiencyLevelName { get; set; }
        public string key { get; set; }
        public string title { get; set; }
        public bool lazy { get; set; }
    }
}
