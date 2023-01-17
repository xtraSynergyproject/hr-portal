using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class HRJobCriteriaViewModel
    {
        
        public string Id { get; set; }
        
        public string Criteria { get; set; }
        public string Type { get; set; }
        public int? Weightage { get; set; }
      
        public string CriteriaType { get; set; }
     
        public string ListOfValueTypeId { get; set; }

        public string CriteriaTypeName { get; set; }
        public string LovTypeName { get; set; }
        public long RowId { get; set; }

        public string NoteId { get; set; }
    }
}
