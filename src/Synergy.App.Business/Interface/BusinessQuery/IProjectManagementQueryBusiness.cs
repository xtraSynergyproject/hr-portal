using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IProjectManagementQueryBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<List<IdNameViewModel>> GetProjectsListData();
        Task<IList<ProjectEmailSetupViewModel>> GetEmailSetupListData();
        Task<List<ProjectEmailSetupViewModel>> GetProjectEmailSetupData();
        Task<ProjectEmailSetupViewModel> GetSingleProjectEmailSetupData();
        
        
        Task<List<WorkBoardViewModel>> GetWorkboardTaskListData();
        Task UpdateWorkBoardStatus(string id, WorkBoardstatusEnum status);
        Task<WorkBoardSectionViewModel> GetWorkBoardSectionData(string sectionId);
        Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionListByWorkbBoardId(string workboardId);
        Task<WorkBoardItemViewModel> GetWorkBoardItemDetails(string itemId);
        Task<IList<WorkBoardItemViewModel>> GetWorkBoardItemBySectionId(string sectionId);
        Task<List<WorkBoardItemViewModel>> GetItemBySectionId(string sectionId);
        Task DeleteItem(string itemId);
        Task DeleteSection(string itemId);
        Task UpdateWorkBoardJson(WorkBoardViewModel data);
        Task UpdateWorkBoardSectionSequenceOrder(WorkBoardSectionViewModel data);
        Task UpdateWorkBoardItemDetails(WorkBoardItemViewModel data);
        Task UpdateWorkBoardItemSequenceOrder(WorkBoardItemViewModel data);
        Task UpdateWorkBoardItemSectionId(WorkBoardItemViewModel data);
        Task<WorkBoardViewModel> GetWorkBoardDetails(string workBoradId);
        Task<WorkBoardViewModel> GetWorkBoardDetailsByIdKey(string workBoardUniqueId, string shareKey);
        Task<List<WorkBoardTemplateViewModel>> GetTemplateList();
        Task<List<LOVViewModel>> GetTemplateCategoryList();
        Task<List<WorkBoardTemplateViewModel>> GetSearchResults(string values);
        Task<List<WorkBoardViewModel>> GetSharedWorkboardList(WorkBoardstatusEnum status, string sharedWithUserId);
        Task<IList<UserViewModel>> GetUserList(string noteId);
        Task UpdateWorkboardItem(string workboardId, string sectionId, string workboardItemId);
        Task<WorkBoardItemViewModel> GetWorkboardItemByNoteId(string id);
        Task<WorkBoardItemViewModel> GetWorkboardItemById(string id);
        Task<WorkBoardItemViewModel> GetWorkBoardItemByNtsNoteId(string ntsNoteId);
        Task<List<WorkBoardSectionViewModel>> GetWorkboardSectionList(string id);
        Task<List<WorkBoardViewModel>> GetOtherWorkboardList(WorkBoardstatusEnum status, string id);
        Task<List<WorkBoardSectionViewModel>> GetSectionListOrderBySequenceNo(string workBoardId);
        Task<List<WorkBoardItemViewModel>> GetItemsInSection(string sectionId);
        Task<WorkBoardTemplateViewModel> GetWorkBoardTemplateById(string templateTypeId);
        Task<List<WorkBoardSectionViewModel>> GetParticularNumberOfSections(int number);
        Task<List<WorkBoardSectionViewModel>> GetSectionsIdOrderBySequenceNo(string workBoardId);
        Task UpdateSectionsforBasicTemplate(List<WorkBoardSectionViewModel> sections, string workBoardId);
        Task UpdateSectionsforMonthlyTemplate(List<WorkBoardSectionViewModel> sections, string workBoardId, DateTime date);
        Task UpdateSectionsforWeeklyTemplate(List<WorkBoardSectionViewModel> sections, string workBoardId);
        Task UpdateSectionsforYearlyTemplate(List<WorkBoardSectionViewModel> sections, string workBoardId);








        Task<IList<ProgramDashboardViewModel>> GetProjectData();
        Task<IList<ProgramDashboardViewModel>> GetProjectSharedData();
        Task<IList<ProjectGanttTaskViewModel>> ReadProjectTask(string userId, string projectId, bool isProjectManager = false, string projectIds = null, string ownerIds = null, string assigneeIds = null, string tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null);
        Task<List<ProjectGanttTaskViewModel>> GetProjectUserIdNameList(string projectId);
        Task<IList<MailViewModel>> ReadEmailTaskData(string userId);
        Task<IList<WBSViewModel>> ReadProjectTaskForEmailList(string projectId);
        Task<IList<IdNameViewModel>> ReadProjectTaskUserData(string projectId);
        Task<List<ProjectGanttTaskViewModel>> GetTaskStatusByTemplate(string templateId, string userId);
        Task<List<ProjectGanttTaskViewModel>> GetTaskByUsers(string templateId, string userId);
        Task<List<ProjectGanttTaskViewModel>> GetSLADetails(string templateId, string userId, DateTime? FromDate = null, DateTime? ToDate = null);
        Task<IList<ProjectGanttTaskViewModel>> ReadTaskGridViewData(string templateId, string userId, string tasksStatus = null, string ownerIds = null);
        Task<IList<IdNameViewModel>> GetTaskOwnerUsersList(string templateId, string userId);
        Task<IList<IdNameViewModel>> GetTaskUsersList(string templateId);
        Task<List<ProjectGanttTaskViewModel>> GetPMTaskStatusByTemplate(string templateId);
        Task<List<ProjectGanttTaskViewModel>> GetPMTaskByUsers(string templateId);
        Task<List<ProjectGanttTaskViewModel>> GetPMSLADetails(string templateId, DateTime? FromDate = null, DateTime? ToDate = null);
        Task<IList<ProjectGanttTaskViewModel>> ReadPMTaskGridViewData(string templateId, string tasksStatus = null, string userIds = null);
        Task<List<ProjectGanttTaskViewModel>> GetGroupTemplate();
        Task<List<ProjectGanttTaskViewModel>> GetTaskStatusRequestedByMeGroup(string TemplateID, string UserID, string StatusLOV);
        Task<List<ProjectGanttTaskViewModel>> GetTaskStatusAssigneduseridGroup(string TemplateID, string UserID, string StatusTemplateId, string StatusLOV);
        Task<List<ProjectGanttTaskViewModel>> MdlassignUserGroup(string TemplateID, string UserID);
        Task<List<NtsTaskChartList>> GetGridListGroup(string TemplateID, string UserID, string assigneeIds = null, string StatusIDs = null);
        Task<List<ProjectGanttTaskViewModel>> GetDatewiseTaskGroup(string TemplateID, string UserID, DateTime? FromDate = null, DateTime? ToDate = null);
        Task<List<ProjectGanttTaskViewModel>> GetTaskStatusRequestedByMeProjectGroup(string TemplateID, string StatusLOV = null);
        Task<List<ProjectGanttTaskViewModel>> GetChartByAssigneduserProjectGroup(string templateId, string StatusTemplateID = null, string StatusLOV = null);
        Task<IList<ProjectGanttTaskViewModel>> GetGridListProjectGroup(string templateId, string tasksStatus = null, string ownerIds = null);
        Task<IList<ProjectGanttTaskViewModel>> GetTaskStatusAssigneduseridProjectGroup(string templateId);
        Task<IList<ProjectGanttTaskViewModel>> GetDatewiseTaskProjectGroup(string templateId, DateTime? FromDate, DateTime? ToDate);
        Task<List<ProjectGanttTaskViewModel>> GetTaskStatusByTemplateGroup(string templateId, string userId, string StatusLOV = null);
        Task<List<ProjectGanttTaskViewModel>> GetTaskByUsersGroup(string templateId, string userId, string StatusTemplateID = null, string StatusLOV = null);
        Task<IList<ProjectGanttTaskViewModel>> ReadTaskGridViewDataGroup(string templateId, string userId, string tasksStatus = null, string ownerIds = null);
        Task<IList<IdNameViewModel>> GetTaskOwnerUsersListGroup(string templateId, string userId);
        Task<IList<ProjectGanttTaskViewModel>> GetDatewiseSingleGroup(string templateId, string userId, DateTime? FromDate, DateTime? ToDate);
        Task<ProgramDashboardViewModel> GetPerformanceDashboard();
        Task<List<ProjectDashboardChartViewModel>> GetProjectStatus();
        Task<IList<ProjectDashboardChartViewModel>> GetTopfiveProject();
        Task<IList<ProjectGanttTaskViewModel>> GetTimeLog();
        Task<IList<ProgramDashboardViewModel>> GetProjectwiseUsers();
        Task<IList<ProgramDashboardViewModel>> GetTaskDetails();
        Task<List<ProjectDashboardChartViewModel>> GetProjecTaskStatus();
        Task<List<TaskWorkTimeViewModel>> GetHourReportTaskData(string serviceId);
        Task<List<TaskWorkTimeViewModel>> GetHourReportProjectData(string projectId, string assigneeId, string sdate, string edate);
        Task<IList<ProjectGanttTaskViewModel>> ReadProjectTimelineData();
        Task<List<UserHierarchyChartViewModel>> GetProjectHierarchyDataForLevelUpto0();
        Task<List<UserHierarchyChartViewModel>> GetProjectHierarchyDataForLevel1(string parentId);
        Task<List<UserHierarchyChartViewModel>> GetProjectHierarchyDataForLevel2(string parentId);
        Task<List<UserHierarchyChartViewModel>> GetProjectHierarchyDataForNodeTypeProjectnStage(string parentId);
        Task<List<UserHierarchyChartViewModel>> GetProjectHierarchyDataForNodeTypeTaskUser(string parentId);
        Task<List<UserHierarchyChartViewModel>> GetProjectHierarchyDataForNodeTypeTaskStatus(string parentId);
        Task<List<TreeViewViewModel>> GetUserRoleData(TreeViewViewModel l, string userId);
        Task<List<TreeViewViewModel>> GetProjectStageData(string userId, TreeViewViewModel p);
        Task<List<TreeViewViewModel>> GetProjectData(string userId, TreeViewViewModel pr);
        Task<List<TreeViewViewModel>> GetStageData(string userId, TreeViewViewModel ps);
        Task<List<TreeViewViewModel>> GetUserRoleList(string userId, string roleText);
        Task<IList<TeamWorkloadViewModel>> ReadProjectTeamDataByUser(string projectId, string userId);
        Task<IList<TeamWorkloadViewModel>> ReadProjectTeamDateData(string projectId);
        Task<IList<TeamWorkloadViewModel>> ReadProjectTeamDataByDate(string projectId, DateTime startDate, bool isProjectManager = false);
        Task<IList<TeamWorkloadViewModel>> ReadProjectTeamWorkloadData(string projectId, string userId, bool isProjectManager = false);
        Task<ServiceViewModel> GetProjectDetails(string projectId);
        Task<IList<IdNameViewModel>> GetProjectsList(string userId, bool isProjectManager);
        Task<IList<ProjectGanttTaskViewModel>> ReadWBSTimelineGanttChartData(string userId, string projectId, string userRole, List<string> projectIds = null, List<string> ownerIds = null, List<string> assigneeIds = null, List<string> tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null);
        Task<List<IdNameViewModel>> GetPrjLevel3userIfAdmin(string monthYear, string status);
        Task<List<IdNameViewModel>> GetPrjLevel3userIfProjectManager(string monthYear, string status, string userId, string userIds);
        Task<List<IdNameViewModel>> GetPrjLevel3(string monthYear, string status, string userId);
        Task<List<IdNameViewModel>> GetPrjLevel2userIfAdmin(string monthYear);
        Task<List<IdNameViewModel>> GetPrjLevel2userIfProjectManager(string monthYear, string userId, string userIds);
        Task<List<IdNameViewModel>> GetPrjLevel2(string monthYear, string userId);
        Task<List<IdNameViewModel>> GetPrjLevel1userIfAdmin();
        Task<List<IdNameViewModel>> GetPrjLevel1userIfProjectManager(string userId, string userIds);
        Task<List<IdNameViewModel>> GetPrjLevel1(string userId);
        Task<IList<TaskViewModel>> ReadTaskOverdueData(string projectId);
        Task<IList<TeamWorkloadViewModel>> ReadProjectSubTaskViewData(string taskId, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null);
        Task<ProgramDashboardViewModel> ReadProjectTotalTaskDataOld(string projectId);
        Task<ProgramDashboardViewModel> ReadProjectTotalTaskData(string projectId);
        Task<IList<TeamWorkloadViewModel>> ReadManagerProjectTaskViewData(string projectId, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null);
        Task<IList<TeamWorkloadViewModel>> ReadProjectTaskViewData(string projectId, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null);
        Task<IList<TeamWorkloadViewModel>> ReadManagerProjectStageViewData(string projectId);
        Task<IList<TeamWorkloadViewModel>> ReadProjectStageViewData(string projectId);
        Task<IList<TeamWorkloadViewModel>> ReadProjectTaskAssignedDataOld(string projectId);
        Task<IList<TeamWorkloadViewModel>> ReadManagerProjectTaskAssignedData(string projectId);
        Task<IList<TeamWorkloadViewModel>> ReadProjectTaskAssignedData(string projectId);
        Task<IList<TeamWorkloadViewModel>> ReadManagerProjectTaskOwnerData(string projectId);
        Task<IList<TeamWorkloadViewModel>> ReadProjectTaskOwnerData(string projectId);
        Task<IList<DashboardCalendarViewModel>> ReadProjectCalendarViewData(string projectId, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null);
        Task<IList<DashboardCalendarViewModel>> ReadManagerProjectCalendarViewData(string projectId, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null);
        Task<List<IdNameViewModel>> GetSubordinatesUserIdNameListuserIfAdmin();
        Task<List<IdNameViewModel>> GetSubordinatesUserIdNameList();
        Task<List<ProjectGanttTaskViewModel>> GetTaskStatusRequestedByMe(string TemplateID, string UserID);
        Task<List<ProjectGanttTaskViewModel>> GetTaskStatusAssigneduserid(string TemplateID, string UserID);
        Task<List<ProjectGanttTaskViewModel>> MdlassignUser(string TemplateID, string UserID);
        Task<List<NtsTaskChartList>> GetGridList(string TemplateID, string UserID, string assigneeIds = null, string StatusIDs = null);
        Task<List<ProjectGanttTaskViewModel>> GetDatewiseTask(string TemplateID, string UserID, DateTime? FromDate = null, DateTime? ToDate = null);
        Task<List<ProjectGanttTaskViewModel>> GetTaskType(string projectId);
        Task<List<ProjectGanttTaskViewModel>> ReadProjectStageChartData(string userId, string projectId);
        Task<List<ProjectGanttTaskViewModel>> GetProjectStageIdNameList(string userId, string projectId);
        Task<List<ProjectGanttTaskViewModel>> GetTaskStatus(string projectId);
        Task<ProjectDashboardViewModel> GetPrjDashboardDetails(string projectId);
        Task<List<ProjectGanttTaskViewModel>> GetPrjTaskDetails(string projectId);
        Task<IList<IdNameViewModel>> GetProjectSharedList(string userId);
        Task<IList<ProjectGanttTaskViewModel>> ReadProjectUserWorkloadGridViewData(string userId, List<UserHierarchyViewModel> users, string projectIds = null, string ownerIds = null, string assigneeIds = null, string tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null);
        Task<IList<ProjectGanttTaskViewModel>> ReadProjectTaskGridViewData(string userId, string projectId, string userRole, string projectIds = null, string ownerIds = null, string assigneeIds = null, string tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null);
        Task<IList<ProjectGanttTaskViewModel>> ReadMindMapData(string projectId);
        Task<long?> GetInboxMenuItemByUsercount(string roleText, string userId);
        Task<List<TreeViewViewModel>> GetInboxMenuItemByUser(string roleText, string userId);
        Task<List<TreeViewViewModel>> GetProjectStageData2(string id, string userRoleId, string userId);
        Task<List<TreeViewViewModel>> GetProjectData2(string id, string userId);
        Task<long?> GetInboxMenuItemcount(string roleText, string userId);
        Task<List<TreeViewViewModel>> GetInboxMenuItem(string roleText, string userId);
        Task<List<TreeViewViewModel>> GetProjectStageData3(string id, string userRoleId, string userId);
        Task<List<TreeViewViewModel>> GetProjectData3(string id, string userId);














    }
}
