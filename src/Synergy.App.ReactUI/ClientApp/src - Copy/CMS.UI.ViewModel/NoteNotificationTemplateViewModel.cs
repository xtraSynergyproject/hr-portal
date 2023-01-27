using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class NoteNotificationTemplateViewModel : NoteNotificationTemplate
    {
        public string NoteNotificationData { get; set; }
        public string TemplateId { get; set; }
    }
}
