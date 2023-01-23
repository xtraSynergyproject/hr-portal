using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class DashboardNoteViewModel
    {
        public long createdByMe { get; set; }
        public long createdByMeExpired { get; set; }
        public long createdByMeActive { get; set; }
        public long createdByMeDraft { get; set; }

        public long sharedByMe { get; set; }
        public long sharedByMeExpired { get; set; }
        public long sharedByMeActive { get; set; } 
        public long sharedByMeDraft { get; set; }

        public long shareWithMe { get; set; }
        public long sharedWithMeExpired { get; set; }
        public long sharedWithMeActive  { get; set; }
        public long sharedWithMeDraft { get; set; }

       
    }
}
