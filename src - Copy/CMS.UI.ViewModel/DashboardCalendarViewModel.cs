
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CMS.Common;
using Kendo.Mvc.UI;

namespace CMS.UI.ViewModel
{
    public class DashboardCalendarViewModel : ISchedulerEvent
    {
        public string Id { get; set; }
        public string ParentID { get; set; }
        public string Title { get; set; }        
        public string Level { get; set; }        
        public string Description { get; set; }
        public bool IsAllDay { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }
        public string NtsId { get; set; }
        public string StatusName { get; set; }
        public string StatusCode { get; set; }
        public string UserId { get; set; }
        public string OwnerUserId { get; set; }
        public string AssigneeUserId { get; set; }
        public string TaskStatusId { get; set; }
        public string UserName { get; set; }

        public int? RecurrId { get; set; }
        public DateTime? DueDate { get; set; }

        public ModuleEnum? ModuleName { get; set; }
        public string TemplateCode { get; set; }

        public int? Count { get; set; }
        public int? BacklogCount { get; set; }
        public int? OpenCount { get; set; }
        public int? ClosedCount { get; set; }
        public int? NotStartedCount { get; set; }
        public int? NotificationCount { get; set; }
        public int? ReminderCount { get; set; }
        public string ServiceId { get; set; }
    }
}
