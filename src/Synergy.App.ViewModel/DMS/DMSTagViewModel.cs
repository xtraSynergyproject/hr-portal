using Synergy.App.Common;
using Synergy.App.DataModel;
using Microsoft.AspNetCore.Http;
//using Syncfusion.EJ2.FileManager.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace Synergy.App.ViewModel
{
    public class DMSTagViewModel : NoteTemplateViewModel
    {
        public string TagName { get; set; }
        public string TagBackgroundColor { get; set; }
        public string TagForegroundColor { get; set; }
        public string TagPinned { get; set; }
        public string TagDescription { get; set; }
        public string TagParentId { get; set; }
        public string TagParentName { get; set; }

    }
   
}
