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
    public class WorkspaceViewModel : NoteTemplateViewModel
    {
        public string LegalEntityName { get; set; }
        public string CreatedbyName { get; set; }
        public string WorkspaceName { get; set; }
        public string Code { get; set; }
        public string ParentName { get; set; }
        //public string NoteSubject { get; set; }
        //public int? SequenceOrder { get; set; }
        //public string LegalEntityId { get; set; }
        //public string CreatedBy { get; set; }
        public string[] DocumentTypeId { get; set; }
        public string WorkspaceId { get; set; }
        //public string NoteId { get; set; }
        public string DocumentTypeIds { get; set; }
        public string DocumentTypeNoteId { get; set; }
        
        public string Type { get; set; }



    }
   
}
