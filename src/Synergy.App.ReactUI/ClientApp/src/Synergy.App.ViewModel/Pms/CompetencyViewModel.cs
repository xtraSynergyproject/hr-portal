using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class CompetencyViewModel: NoteTemplateViewModel
    {
        public string CompetencyName { get; set; }
        public string CompetencyCode { get; set; }
        public string CompetencyDescription { get; set; }
        public string ProficiencyLevelId { get; set; }
        public string CompetencyType { get; set; }
        public string CompetencyTypeId { get; set; }
       
        public DateTime? DueDate { get; set; }
    }
}
