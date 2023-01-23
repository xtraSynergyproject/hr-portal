using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class JobDescriptionCriteriaViewModel : JobDescriptionCriteria
    {
        //[UIHint("JobAdvtCriteriaType")]
        //public string CriteriaType { get; set; }
        public string CriteriaTypeName { get; set; }
        public string LovTypeName { get; set; }
        public long RowId { get; set; }
        public string NoteId { get; set; }
        
    }
}
