
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Synergy.App.Common;

namespace Synergy.App.ViewModel
{
    public class TeamWorkloadViewModel
    {
        public string id { get; set; }
        public string parentId { get; set; }
        public string text { get; set; }
        public dynamic a_attr { get; set; }
        public string icon { get; set; }

        public string parent { get; set; }
        public bool children { get; set; }
        public string Id { get; set; }
        public string TaskId { get; set; }
        public string ProjectId { get; set; }
        public string ProjectOwnerId { get; set; }
        public string RequestedByUserId { get; set; }
        public string TaskName { get; set; }
        //public string TaskNameLimited { get { return TaskName.Substring(5); } }
        public string StageName { get; set; }
        public string StageId { get; set; }
        public string PerformanceStage { get; set; }
        public string PerformanceStageName { get; set; }
        public string PerformanceStageServiceId { get; set; }
        public string PerformanceStageOwner { get; set; }
        public string TaskType { get; set; }
        public string TaskStatus { get; set; }
        public string TaskStatusCode { get; set; }
        public string ServiceStatusCode { get; set; }
        public string TaskStatusId { get; set; }
        public string Priority { get; set; }
        public DateTime StartDate { get; set; }
        public string StartDateStr { get { return StartDate.ToString(); }  }
        public DateTime DueDate { get; set; }
        public Int64 SubTaskId { get; set; }
        public string UserId { get; set; }
        public string HierarchyUserId { get; set; }
        public string[] FilterUserId { get; set; }
        public string FilterTaskStatus { get; set; }
        public string UserName { get; set; }
        public string ProjectOwnerName { get; set; }
        public string PhotoId { get; set; }
        public string EmailId { get; set; }
        public bool Checkbox { get; set; }
        public bool hasChildren { get; set; }
        public bool IsAssignee { get; set; }
        public ICollection<long> Users { get; set; }
        public ICollection<string> UserNames { get; set; }
        public string TaskCount { get; set; }
        public long UserTaskCount { get; set; }
        public long SubTaskCount { get; set; }
        public Int32 Level { get; set; }
        public string CompletedCount { get; set; }
        public long PendingCount { get; set; }
        public long TotalCompletedCount { get; set; }
        public string DayLeft { get; set; }
        public string TemplateCode { get; set; }
        public string Code { get; set; }
        public string ServiceDescription { get; set; }
        public string NtsStatus { get; set; }
        public bool HasSubFolders { get; set; }
        public long Sequence { get; set; }

        public string PerformanceUser { get; set; }
        public string PerformanceUserYear { get; set; }
        ////  public NtsActionEnum TemplateAction { get; set; }

        //  public string StatusStyle
        //  {
        //      get
        //      {
        //          switch (TemplateAction)
        //          {
        //              case NtsActionEnum.Draft:
        //                  return "Color:#5bc0de";
        //              case NtsActionEnum.Submit:
        //                  return "Color:#f0ad4e";
        //              case NtsActionEnum.Complete:
        //                  return "Color:#5cb85c";
        //              case NtsActionEnum.Cancel:
        //                  return "Color:#999";
        //              case NtsActionEnum.Overdue:
        //                  return "Color:#d9534f";
        //              default:
        //                  return "Color:#999";
        //          }
        //      }
        //  }
        public string PTemplateCode { get; set; }        
        public int PInProgressCount { get; set; }
        public int PCompletedCount { get; set; }
        public int PRejectedCount { get; set; }
        public string GTemplateCode { get; set; }
        public int GInProgressCount { get; set; }
        public int GCompletedCount { get; set; }
        public int GRejectedCount { get; set; }
        public string CTemplateCode { get; set; }
        public int CInProgressCount { get; set; }
        public int CCompletedCount { get; set; }
        public int CRejectedCount { get; set; }
        public string DTemplateCode { get; set; }
        public int DInProgressCount { get; set; }
        public int DCompletedCount { get; set; }
        public int DRejectedCount { get; set; }
        public int GTotalCount { get; set; }
        public int CTotalCount { get; set; }
        public int DTotalCount { get; set; }
        public int TaskPendingCount { get; set; }
        public int TaskCompletedCount { get; set; }
        public int TaskCancelledCount { get; set; }
        public int TaskTotalCount { get; set; }
        public IList<PMSDashboardViewModel> ServiceList { get; set; }
        public string ManagerComment { get; set; }
        public string EmployeeComment { get; set; }
        public string ManagerRating { get; set; }
        public string EmployeeRating { get; set; }
        public string SuccessCriteria { get; set; }
        public string UserType { get; set; }
        public string ReviewId { get; set; }
        public string GoalId { get; set; }
        public string UdfNoteTableId { get; set; }
        public string CompetencyId { get; set; }
        public string NtsStatusId { get; set; }
        public long Weightage { get; set; }
    }

    public class PMSDashboardViewModel
    {
        public string ServiceName { get; set; }        
        public string Status { get; set; }       
        public int InProgressCount { get; set; }
        public int CompletedCount { get; set; }
        public int RejectedCount { get; set; }
        public string TemplateCode { get; set; }
        
    }
}
