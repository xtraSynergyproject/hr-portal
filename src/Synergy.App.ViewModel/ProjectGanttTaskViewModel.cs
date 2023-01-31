using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
////using Kendo.Mvc.UI;
using Synergy.App.Common;

namespace Synergy.App.ViewModel
{
    public class ProjectGanttTaskViewModel //: IGanttTask
    {
        public string Id { get; set; }
        public string ServiceId { get; set; }
        public string TemplateCode { get; set; }
        public string ProjectName { get; set; }
        public string ProjectId { get; set; }
        public string PerformanceName { get; set; }
        public string MasterName { get; set; }
        public string JobName { get; set; }
        public string DepartmentName { get; set; }
        public string EmployeeNo { get; set; }
        public string UserName { get; set; }
        public string OwnerName { get; set; }
        public string TaskOwnerName { get; set; }
        public bool IsManager { get; set; }
        public string TaskNo { get; set; }
        public string ProjectNo { get; set; }
        public string ParentId { get; set; }
        public string ServiceStage { get; set; }
        public string UserId { get; set; }
        public string OwnerUserId { get; set; }
        public string Title { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime Start { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime End { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime PlannedStart { get; set; }
        [DisplayFormat(DataFormatString = ApplicationConstant.DateAndTime.DefaultDateFormat)]
        public DateTime PlannedEnd { get; set; }
        public decimal PercentComplete { get; set; }
        public bool Summary { get; set; }
        public bool Expanded { get; set; }
        public int OrderId { get; set; }
        public string TaskType { get; set; }
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        public long? PredecessorId { get; set; }
        public long? TaskGroupId { get; set; }

        public string NtsStatus { get; set; }
        public string NtsStatusId { get; set; }
        public string Priority { get; set; }
        public string Type { get; set; }
        public string TypeCode { get; set; }
        public string RefId { get; set; }
        public string AssigneeUserId { get; set; }
        public string TaskStatusId { get; set; }
        public List<String> Predeccessor { get; set; }
        public string diagramTaskId { get; set; }

        public long Value { get; set; }

        public TimeSpan Days { get; set; }
        public TimeSpan ActualSLA { get; set; }
        public string Code { get; set; }
        public string TemplateMasterCode { get; set; }
        public string NtsStatusCode { get; set; }
        public string Year { get; set; }
        public string StatusGroupCode { get; set; }
        public string TaskStatusCode { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
    }
}
