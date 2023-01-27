using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class NtsNoteIndexPageViewModel : NoteIndexPageTemplateViewModel
    {
        public string CategoryCode { get; set; }
        public string TemplateCode { get; set; }
        public string ModuleCode { get; set; }
    }
}
