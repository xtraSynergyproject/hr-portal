using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel.Hrs
{
    public class DocumentViewModel
    {
            [Display(Name = "PersonNo")]
            public string PersonId { get; set; }
            [Display(Name = "Archive")]
            public long Expired { get; set; }
            public long Active { get; set; }
            public long Draft { get; set; }
            public string DocumentType { get; set; }
            public string Category { get; set; }
            public long Total { get; set; }
    }
}
