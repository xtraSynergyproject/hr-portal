using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class JobCriteriaViewModel : JobCriteria
    {
       // [UIHint("JobAdvtCriteriaType")]
       // public string CriteriaType { get; set; }
        public string CriteriaTypeName { get; set; }
        public string LovTypeName { get; set; }
        public long RowId { get; set; }
        
    }
}
