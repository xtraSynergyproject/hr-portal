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
    public class WorkspaceDocTypeViewModel : NoteTemplateViewModel
    {
        public string DocumentTypeId { get; set; }
        public string WorkspaceId { get; set; }


    }
}
