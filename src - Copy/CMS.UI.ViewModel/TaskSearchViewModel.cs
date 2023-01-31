using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class TaskSearchViewModel 
    {
        public string Id { get; set; }
        public virtual StatusEnum? Status { get; set; }
        public DataOperation? Operation { get; set; }
        public string TaskId { get; set; }        
        public string EmailTaskId { get; set; }        
        public string TaskNo { get; set; }        
        public string Subject { get; set; }
        public DateTime? StartDate { get; set; }      
        public DateTime? DueDate { get; set; }     
        public LOVViewModel AssignedToType { get; set; }        
        public DateTime? CreationDate { get; set; }        
        public DateTime? CompletionDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public DateTime? ReminderDate { get; set; }
        public string TaskStatus { get; set; }
        public string TaskStatusId { get; set; }
        public string Mode { get; set; }
        public string Text { get; set; }
        public long? AssignTo { get; set; }
       
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleCode { get; set; }
        public string UserId { get; set; }
        public string OwnerName { get; set; }
        public string AssignedToUserName { get; set; }
        public string UserRole { get; set; }
        public long? ServiceId { get; set; }
        public string TemplateMasterCode { get; set; }
        public string TemplateCategoryCode { get; set; }
        
        public string Description { get; set; }
        public string PortalNames { get; set; }
        public string RequestSource { get; set; }
        public string Layout { get; set; }
        public string ReturnUrl { get; set; }
        public long? TemplateMasterId { get; set; }
        public NtsTypeEnum? NTSType { get; set; }
        public string[] Userfilter { get; set; }
        public string Projectfilter { get; set; }
        public int Days { get; set; }
        public int ActualSLA { get; set; }
        public int ChartFilter { get; set; }
        public int PieChartFilter { get; set; }
        public string Period { get; set; }
        public string Taskfilter { get; set; }
        public DateTime? Datefilter { get; set; }
        public DateTime[] Datefilter1 { get; set; }
        public List<long> UserfilterCVR { get; set; }
        public List<ModuleViewModel> ModuleList { get; set; }
        public List<string> TaskStatusIds { get; set; }
        public List<string> TaskAssigneeIds { get; set; }
        public List<string> TaskOwnerIds { get; set; }
        public string TrendType { get; set; }
    }
   public class TaskViewModelComparer : IEqualityComparer<TaskViewModel>
    {
        public bool Equals(TaskViewModel x, TaskViewModel y)
        {
            if (x.Id == y.Id)
                return true;

            return false;
        }

        public int GetHashCode(TaskViewModel obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
