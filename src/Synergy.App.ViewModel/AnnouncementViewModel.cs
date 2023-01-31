using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace Synergy.App.ViewModel
{
    public class AnnouncementViewModel : NoteTemplateViewModel
    {
        public string OrgId { get; set; }     
        [Required]
        [Display(Name = "Description")]
        public string Body { get; set; }    
        public bool IsNotifyByEmail { get; set; }
        public string UserId { get; set; }
        public string Attachment { get; set; }
        public NoteReferenceTypeEnum? ReferenceType { get; set; }
        public string ReferenceId { get; set; }
        public bool? EnableBroadcast { get; set; }
    }
}
