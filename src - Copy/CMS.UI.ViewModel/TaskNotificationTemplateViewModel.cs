using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class TaskNotificationTemplateViewModel : TaskNotificationTemplate
    {
        public string TaskNotificationData { get; set; }
        public string TemplateId { get; set; }
    }
}
