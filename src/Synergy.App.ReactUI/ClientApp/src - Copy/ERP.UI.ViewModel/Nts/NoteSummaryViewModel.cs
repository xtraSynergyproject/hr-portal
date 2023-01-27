using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class NoteSummaryViewModel
    {
            //[Display(Name = "PersonNo")]
        [Display(Name = "PersonId", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public string PersonId { get; set; }
            //[Display(Name = "Archive")]
        [Display(Name = "Expired", ResourceType = typeof(ERP.Translation.Nts.Note))]
        public long Expired { get; set; }
            public long Active { get; set; }
            public long Draft { get; set; }
            public string DocumentType { get; set; }
            public string DocumentName { get; set; }
            public string Category { get; set; }
            public long Total { get; set; }
            public long TemplateMasterId { get; set; }
            public string Code { get; set; }
            public long Count { get; set; }
    }
}
