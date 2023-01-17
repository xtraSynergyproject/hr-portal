


using System;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using Kendo.Mvc.UI;

namespace ERP.UI.ViewModel
{
    public class TeamLeaveRequestViewModel : ISchedulerEvent
    {
        public long? Id { get; set; }
        public long? UserId { get; set; }
        public long? PersonId { get; set; }
        public string UserName { get; set; }
        public long? LeaveTypeId { get; set; }
        public string LeaveTypeCode { get; set; }
        public string LeaveStatus { get; set; }
        public string LeaveStatusCode { get; set; }
        private string _Title { get; set; }
        public string Title { get; set; }// { get { return string.Concat(_Title, "(", LeaveStatus, ")"); } set { _Title = value; } }
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
        public long? OrganizationId { get; set; }
        [Display(Name = "Department Name")]
        public string OrganizationName { get; set; }
    }
}
