using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class PerformaceRatingViewModel: NoteTemplateViewModel
    {
       public string Name { get; set; }
       public string Code { get; set; }

        public string NtsNoteIds { get; set; }

    }

    public class PerformanceRatingItemViewModel : NoteTemplateViewModel
    {
        public string Name { get; set; }
        public Int32? code { get; set; }
    }
}
