using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class TaskNotificationTemplate : NotificationTemplate
    {
        

        [ForeignKey("TaskTemplate")]
        public string TaskTemplateId { get; set; }
        public TaskTemplate TaskTemplate { get; set; }


    

        [ForeignKey("ParentTaskNotificationTemplate")]
        public string ParentTaskNotificationTemplateId { get; set; }
        public TaskNotificationTemplate ParentTaskNotificationTemplate { get; set; }
    }
     
}
