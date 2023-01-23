using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class NoteLinkShareViewModel : ViewModelBase
    {
        public long NoteId { get; set; }
        public string Link { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime? ExpiryDate { get; set; }


    }
}
