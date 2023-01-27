using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class NoteNotificationTemplate : NotificationTemplate
    {
        [ForeignKey("NoteTemplate")]
        public string NoteTemplateId { get; set; }
        public NoteTemplate NoteTemplate { get; set; }
 

        [ForeignKey("ParentNoteNotificationTemplate")]
        public string ParentNoteNotificationTemplateId { get; set; }
        public NoteNotificationTemplate ParentNoteNotificationTemplate { get; set; }
    }
     
}
