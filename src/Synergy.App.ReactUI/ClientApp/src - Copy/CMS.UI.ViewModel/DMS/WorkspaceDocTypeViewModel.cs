using CMS.Common;
using CMS.Data.Model;
using Microsoft.AspNetCore.Http;
using Syncfusion.EJ2.FileManager.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace CMS.UI.ViewModel
{
    public class WorkspaceDocTypeViewModel : NoteTemplateViewModel
    {
        public string DocumentTypeId { get; set; }
        public string WorkspaceId { get; set; }


    }
}
