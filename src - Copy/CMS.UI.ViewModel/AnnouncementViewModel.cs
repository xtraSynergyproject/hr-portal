using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace CMS.UI.ViewModel
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
