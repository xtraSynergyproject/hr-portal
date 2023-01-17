using Synergy.App.Common;
using Synergy.App.DataModel;
using System;

namespace Synergy.App.ViewModel
{
    public class ComponentResultViewModel : ComponentResult
    {
        public string ComponentStatusName { get; set; }
        public string TaskNo { get; set; }
        public string ServiceSubject { get; set; }
        public string TaskSubject { get; set; }
        public string TemplateMasterCode { get; set; }
        public string TaskId { get; set; }
        public string Assignee { get; set; }
        public string AssigneeId { get; set; }
        public string AssigneePhotoId { get; set; }
        public string Email { get; set; }
        public string ComponentStatusCode { get; set; }
        public string StageId { get; set; }
        public string StageName { get; set; }
        public string ItemSubjectLimited
        {
            get
            {
                return TaskSubject.LimitSize(40);
            }
        }
        public string ItemStatus
        {
            get
            {
                if (ComponentStatusCode.IsNullOrEmpty())
                {
                    return "Completed";
                }
                return ComponentStatusCode.Replace("COMPONENT_STATUS_", "").Replace("SERVICE_STATUS_", "").Replace("TASK_STATUS_", "").ToLower();
            }
        }
        public string ItemStepStatus
        {
            get
            {
                if (ComponentStatusCode.IsNullOrEmpty())
                {
                    return "step-info";
                }
                var st = ComponentStatusCode.Replace("COMPONENT_STATUS_", "").Replace("SERVICE_STATUS_", "").Replace("TASK_STATUS_", "").ToLower();
                switch (st)
                {
                    case "draft":
                        return "step-info";
                    case "complete":
                    case "completed":
                        return "step-success";
                    case "inprogress":
                        return "step-primary";
                    case "overdue":
                        return "step-warning";
                    case "reject":
                    case "rejected":
                        return "step-danger";
                    case "cancel":
                    case "canceled":
                        return "step-secondary";
                    default:
                        return "step-secondary";
                }
            }
        }
        public string ItemDateRange
        {
            get
            {
                if (StartDate != null && EndDate != null)
                {
                    return $"{StartDate.ToDefaultDateTimeFormat()} - {EndDate.ToDefaultDateTimeFormat()}";
                }
                else if (StartDate != null)
                {
                    return StartDate.ToDefaultDateTimeFormat();
                }
                if (EndDate != null)
                {
                    return EndDate.ToDefaultDateTimeFormat();
                }
                return "";
            }
        }
        public string StatusAndNo
        {
            get
            {
                if (ComponentStatusName.IsNotNullAndNotEmpty() && TaskNo.IsNotNullAndNotEmpty())
                {
                    return $"{TaskNo} | <span class='font-{ItemStatus}'>{ComponentStatusName}</span>";
                }
                else if (ComponentStatusName.IsNotNullAndNotEmpty())
                {
                    return $"<span class='font-{ItemStatus}'>{ComponentStatusName}</span>";
                }
                if (TaskNo.IsNotNullAndNotEmpty())
                {
                    return TaskNo;
                }
                return "";
            }
        }
    }
}
