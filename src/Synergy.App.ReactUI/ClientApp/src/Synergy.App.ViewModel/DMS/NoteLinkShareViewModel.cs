using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
////using Kendo.Mvc.UI;
using Synergy.App.Common;
namespace Synergy.App.ViewModel
{
    public class NoteLinkShareViewModel : ViewModelBase
    {

        public string NoteId { get; set; }
        public string Link { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Key { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public string EmailTo { get; set; }

        public string ParentNoteId { get; set; }

        public string ReferenceId { get; set; }

        public ReferenceTypeEnum ReferenceType { get; set; }

        public string LogoId { get; set; }
    }
}
