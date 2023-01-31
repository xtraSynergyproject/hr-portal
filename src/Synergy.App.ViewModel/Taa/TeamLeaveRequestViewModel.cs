using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
////using Kendo.Mvc.UI;
namespace Synergy.App.ViewModel
{
    public class TeamLeaveRequestViewModel //: ISchedulerEvent
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string PersonId { get; set; }
        public string UserName { get; set; }
        public string LeaveTypeId { get; set; }
        public string LeaveTypeCode { get; set; }
        public string LeaveStatus { get; set; }
        public string LeaveStatusCode { get; set; }
        private string _Title { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsAllDay { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }
        public string EmployeeName { get; set; }
        public string JobName { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationId { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }

    }
}
