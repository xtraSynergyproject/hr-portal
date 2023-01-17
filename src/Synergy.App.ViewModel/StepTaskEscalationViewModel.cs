using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class StepTaskEscalationViewModel : StepTaskEscalation
    {
       public string Assignee { get; set; }
        public string AssignedToType { get; set; }
        public string ParentId { get; set; }
        public string TemplateId { get; set; }
        public string ParentName { get; set; }
        public string NotificationtemplateName { get; set; }
        public string EscalatedNotificationtemplateName { get; set; }
        public string TaskStatusCode { get; set; }
        public string TaskId { get; set; }
        public string ParentServiceId { get; set; }
    }
}
