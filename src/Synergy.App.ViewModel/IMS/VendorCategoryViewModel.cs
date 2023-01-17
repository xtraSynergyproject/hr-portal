using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class VendorCategoryViewModel:NoteTemplateViewModel
    {
        public string VendorName { get; set; }
        public string VendorId { get; set; }

        public string ItemCategoryName { get; set; }
        public string CategoryId { get; set; }
        public int SNo { get; set; }

    }
}
