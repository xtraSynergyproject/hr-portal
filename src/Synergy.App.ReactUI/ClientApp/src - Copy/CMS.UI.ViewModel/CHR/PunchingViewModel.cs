using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class PunchingViewModel: NoteTemplateViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string NtsNoteId { get; set; }
        public string LocationId { get; set; }
        public string AttendanceDate { get; set; }
        public string Duty1StartTime { get; set; }
        public string Duty1EndTime { get; set; }

    }
}
