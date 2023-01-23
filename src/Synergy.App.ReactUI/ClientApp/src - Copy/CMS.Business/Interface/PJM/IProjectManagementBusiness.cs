using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IProjectManagementBusiness : IBusinessBase<ServiceViewModel, NtsService>
    {
        Task<IList<ProjectGanttTaskViewModel>> ReadMindMapData(string projectId);
        Task<IList<TreeViewViewModel>> GetInboxMenuItem(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string projectId, string expandingList, string userroleCode);
        Task<IList<TreeViewViewModel>> GetInboxMenuItemByUser(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string projectId, string expandingList, string userroleCode);
        Task<IList<ProgramDashboardViewModel>> GetProjectData();
        Task<IList<ProgramDashboardViewModel>> GetProjectSharedData();
        Task<IList<IdNameViewModel>> GetProjectsLevel1(string userId, bool isProjectManager);
        Task<IList<IdNameViewModel>> GetProjectsLevel2(string userId, bool isProjectManager, string monthYear);
        Task<IList<IdNameViewModel>> GetProjectsLevel3(string userId, bool isProjectManager, string monthYear, string status);
        Task<IList<IdNameViewModel>> GetProjectsList(string userId, bool isProjectManager);
        Task<ServiceViewModel> GetProjectDetails(string projectId);        
        Task<IList<ProjectGanttTaskViewModel>> ReadWBSTimelineGanttChartData(string userId, string projectId, string userRole, List<string> projectIds = null, List<string> ownerIds = null, List<string> assigneeIds = null, List<string> tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null);
        Task<IList<TeamWorkloadViewModel>> ReadManagerProjectTaskViewData(string projectId, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null);
        Task<IList<TeamWorkloadViewModel>> ReadProjectTaskViewData(string projectId, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null);
        Task<ProgramDashboardViewModel> ReadProjectTotalTaskData(string projectId);
        Task<IList<TeamWorkloadViewModel>> ReadManagerProjectStageViewData(string projectId);
        Task<IList<TeamWorkloadViewModel>> ReadProjectStageViewData(string projectId);
        Task<IList<DashboardCalendarViewModel>> ReadManagerProjectCalendarViewData(string projectId, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null);
        Task<IList<DashboardCalendarViewModel>> ReadProjectCalendarViewData(string projectId, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null);
        Task<IList<TeamWorkloadViewModel>> ReadProjectSubTaskViewData(string taskId, List<string> filteruser = null, List<string> filterstatus = null, List<string> ownerIds = null);
        Task<IList<TeamWorkloadViewModel>> ReadProjectTeamWorkloadData(string projectId, string userId, bool isProjectManager = false);
        Task<IList<TeamWorkloadViewModel>> ReadProjectTeamDataByUser(string projectId, string userId);
        Task<IList<TeamWorkloadViewModel>> ReadManagerProjectTaskAssignedData(string projectId);
        Task<IList<TeamWorkloadViewModel>> ReadProjectTaskAssignedData(string projectId);
        Task<IList<TeamWorkloadViewModel>> ReadManagerProjectTaskOwnerData(string projectId);
        Task<IList<TeamWorkloadViewModel>> ReadProjectTaskOwnerData(string projectId);
        Task<IList<TreeViewViewModel>> GetWBSItemData(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string projectId, string expandingList, string userroleCode);
        Task<IList<TeamWorkloadViewModel>> ReadProjectTeamDateData(string projectId);
        Task<IList<TeamWorkloadViewModel>> ReadProjectTeamDataByDate(string projectId, DateTime startDate, bool isProjectManager = false);
        Task<IList<ProjectGanttTaskViewModel>> ReadProjectTaskGridViewData(string userId, string projectId,string userRole, string projectIds = null, string ownerIds = null, string assigneeIds = null, string tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null);
        Task<IList<IdNameViewModel>> GetProjectSharedList(string userId);
        Task<ProjectDashboardViewModel> GetProjectDashboardDetails(string projectId);
        Task<List<ProjectDashboardChartViewModel>> GetTaskStatus(string projectId);
        #region
        /// <summary>
        /// code for request by me chart
        /// </summary>
        /// <param name="TemplateID"></param>
        /// /// <param name="UserID"></param>
        /// <returns></returns>
        /// 
        Task<List<ProjectDashboardChartViewModel>> GetTaskStatusRequestedByMe(string TemplateID, string UserID);

        Task<List<ProjectDashboardChartViewModel>> GetDatewiseTask(string TemplateID, string UserID,DateTime? FromDate=null,DateTime? ToDate=null);

        Task<List<ProjectDashboardChartViewModel>> GetTaskStatusAssigneduserid(string TemplateID, string UserID);

        Task<List<ProjectDashboardChartViewModel>> MdlassignUser(string TemplateID, string UserID);

        Task<List<NtsTaskChartList>> GetGridList(string TemplateID, string UserID, string assigneeIds = null, string StatusIDs = null);

        #endregion

        Task<IList<ProjectDashboardChartViewModel>> GetTaskType(string projectId);
        Task<IList<TaskViewModel>> ReadTaskOverdueData(string projectId);
        Task<IList<ProjectGanttTaskViewModel>> ReadProjectUserWorkloadGridViewData(string userIds, List<string> projectIds = null, List<string> ownerIds = null, List<string> assigneeIds = null, List<string> tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null);
        //Task<IList<ProjectGanttTaskViewModel>> ReadProjectTaskGrid(string projectId);
        Task<bool> CreateMindMap(string model);
        Task<IList<ProjectDashboardChartViewModel>> ReadProjectStageChartData(string userId, string projectId);
        Task<IList<ProjectGanttTaskViewModel>> ReadProjectTask(string userId, string projectId, bool isProjectManager = false, string projectIds = null, string ownerIds = null, string assigneeIds = null, string tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null);
        Task<IList<IdNameViewModel>> GetProjectStageIdNameList(string userId, string projectId);
        Task<IList<IdNameViewModel>> GetProjectUserIdNameList(string projectId);
        Task<IList<MailViewModel>> ReadEmailTaskData(string userId);
        Task<IList<WBSViewModel>> ReadProjectTaskForEmailList(string projectId);
        Task<IList<IdNameViewModel>> ReadProjectTaskUserData(string projectId);
        Task<List<ProjectDashboardChartViewModel>> GetTaskStatusByTemplate(string templateId, string userId);
        Task<List<ProjectDashboardChartViewModel>> GetTaskByUsers(string templateId, string userId);
        Task<IList<ProjectGanttTaskViewModel>> ReadTaskGridViewData(string templateId, string userId, string tasksStatus = null, string ownerIds = null);
        Task<IList<IdNameViewModel>> GetTaskOwnerUsersList(string templateId, string userId);
        Task<List<ProjectDashboardChartViewModel>> GetSLADetails(string templateId, string userId, DateTime? FromDate = null, DateTime? ToDate = null);
        Task<List<ProjectDashboardChartViewModel>> GetPMTaskStatusByTemplate(string templateId);
        Task<List<ProjectDashboardChartViewModel>> GetPMTaskByUsers(string templateId);
        Task<IList<ProjectGanttTaskViewModel>> ReadPMTaskGridViewData(string templateId, string tasksStatus = null, string ownerIds = null);
        Task<List<ProjectDashboardChartViewModel>> GetPMSLADetails(string templateId, DateTime? FromDate = null, DateTime? ToDate = null);

        Task<IList<IdNameViewModel>> GetTaskUsersList(string templateId);
        Task<List<ProjectDashboardChartViewModel>> GetTaskStatusRequestedByMeGroup(string TemplateID, string UserID, string StatusLOV);

        Task<List<ProjectDashboardChartViewModel>> GetDatewiseTaskGroup(string TemplateID, string UserID, DateTime? FromDate = null, DateTime? ToDate = null);

        Task<List<ProjectDashboardChartViewModel>> GetTaskStatusAssigneduseridGroup(string TemplateID, string UserID, string StatusTemplateId, string StatusLOV);

        Task<List<ProjectDashboardChartViewModel>> MdlassignUserGroup(string TemplateID, string UserID);

        Task<List<NtsTaskChartList>> GetGridListGroup(string TemplateID, string UserID, string assigneeIds = null, string StatusIDs = null);

        Task<List<ProjectDashboardChartViewModel>> GetGroupTemplate();

        Task<List<ProjectDashboardChartViewModel>> GetTaskStatusRequestedByMeProjectGroup(string TemplateID, string StatusLOV = null);
        Task<List<ProjectDashboardChartViewModel>> GetChartByAssigneduserProjectGroup(string templateId, string StatusTemplateID = null, string StatusLOV = null);

        Task<IList<ProjectGanttTaskViewModel>> GetGridListProjectGroup(string templateId, string tasksStatus = null, string ownerIds = null);
        Task<IList<ProjectDashboardChartViewModel>> GetTaskStatusAssigneduseridProjectGroup(string templateId);
        Task<IList<ProjectDashboardChartViewModel>> GetDatewiseTaskProjectGroup(string templateId, DateTime? FromDate, DateTime? ToDate);
        Task<List<ProjectDashboardChartViewModel>> GetTaskStatusByTemplateGroup(string templateId, string userId, string StatusLOV = null);
        Task<List<ProjectDashboardChartViewModel>> GetTaskByUsersGroup(string templateId, string userId, string StatusTemplateID = null, string StatusLOV = null);
        Task<IList<ProjectGanttTaskViewModel>> ReadTaskGridViewDataGroup(string templateId, string userId, string tasksStatus = null, string ownerIds = null);

        Task<IList<IdNameViewModel>> GetTaskOwnerUsersListGroup(string templateId, string userId);

        Task<IList<ProjectDashboardChartViewModel>> GetDatewiseSingleGroup(string templateId, string userId, DateTime? FromDate, DateTime? ToDate);
        Task<ProgramDashboardViewModel> GetPerformanceDashboard();
         Task<List<ProjectDashboardChartViewModel>> GetProjectStatus();

        Task<IList<ProjectDashboardChartViewModel>> GetTopfiveProject();
        Task<IList<ProjectDashboardChartViewModel>> GetTimeLog();

        Task<IList<ProgramDashboardViewModel>> GetProjectwiseUsers();

        Task<IList<ProgramDashboardViewModel>> GetTaskDetails();

        Task<List<ProjectDashboardChartViewModel>> GetProjecTaskStatus();
        Task<List<TaskWorkTimeViewModel>> GetHourReportTaskData(string serviceId);
        Task<List<TaskWorkTimeViewModel>> GetHourReportProjectData(string projectId, string assigneeId, string sdate, string edate);

        Task<IList<ProjectGanttTaskViewModel>> ReadProjectTimelineData();
        Task<MemoryStream> GetHourSpentReportDataExcel(List<TaskWorkTimeViewModel> model, string projectId, string assigneeId, string sdate, string edate);
        Task<IList<IdNameViewModel>> GetSubordinatesUserIdNameList();
        Task<List<UserHierarchyChartViewModel>> GetProjectHierarchy(string parentId, int levelUpto, string nodeType);
    }
}
