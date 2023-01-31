using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class NtsNoteIndexPageViewModel : NoteIndexPageTemplateViewModel
    {
        public string CategoryCode { get; set; }
        public string TemplateCode { get; set; }
        public string ModuleCode { get; set; }
    }
}
