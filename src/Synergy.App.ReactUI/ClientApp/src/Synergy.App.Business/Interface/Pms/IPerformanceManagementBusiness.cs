using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IPerformanceManagementBusiness : IBusinessBase<ServiceViewModel, NtsService>
    {
        Task<IList<ProjectGanttTaskViewModel>> ReadPerformanceDashboardTaskData(string userId, string performanceId, string userRole, string projectIds = null, string ownerIds = null, string assigneeIds = null, string tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null, string type = null, string stageId = null);
        Task<IList<ProjectGanttTaskViewModel>> ReadPerformanceDashboardData(string userId, string performanceId, string userRole, string projectIds = null,string ownerIds = null, string assigneeIds = null, string tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null, string type = null, string stageId = null);
        Task<IList<ProjectGanttTaskViewModel>> ReadMindMapData(string projectId);
        Task<IList<TreeViewViewModel>> GetInboxMenuItem(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string projectId, string expandingList, string userroleCode);
        Task<IList<TreeViewViewModel>> GetInboxMenuItemByUser(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string performanceId, string expandingList, string userroleCode);
        Task<IList<PerformanceDiagramViewModel>> GetPerformanceDiagram(string performanceDocumentId);
        Task<IList<ProgramDashboardViewModel>> GetPerformanceData(string userId);
        Task<IList<StageViewModel>> GetPerformanceStageData(string userId, string performanceDocumentId);
        Task<IList<ProgramDashboardViewModel>> GetPerformanceSharedData();
        Task<List<IdNameViewModel>> GetPerformanceRatingsList();
        Task updateGoalWeightaged(string Id, string Weightage);
        Task updateCompentancyWeightaged(string Id, string Weightage);
        // mirza
        Task<IList<CompetencyViewModel>> GetCompetencyData(string parentNoteId, string udfNoteId=null, string noteId=null);
        Task<CommandResult<CompetencyViewModel>> CreateComp(CompetencyViewModel model);
        Task<CommandResult<CompetencyViewModel>> EditComp(CompetencyViewModel model);
        Task<bool> DeleteComp(string Id);
        Task<CommandResult<CompetencyViewModel>> IsCompNameExists(CompetencyViewModel model);
        Task<CommandResult<PerformanceDocumentViewModel>> CreatePerGradeRating(PerformanceDocumentViewModel model);
        Task<CommandResult<PerformanceDocumentViewModel>> EditPerGradeRating(PerformanceDocumentViewModel model);
        Task<IList<GoalViewModel>> GetGoalWeightageByPerformanceId(string Id, string stageId, string userId);
        Task<IList<GoalViewModel>> GetCompentencyWeightageByPerformanceId(string Id, string stageId, string userId);
        Task<IList<IdNameViewModel>> GetPerformanceList(string userId, bool isProjectManager,string year=null);
        Task<ServiceViewModel> GetPerformanceDetails(string projectId);        
        Task<IList<ProjectGanttTaskViewModel>> ReadWBSTimelineGanttChartData(string userId, string projectId, string userRole, List<string> projectIds = null, List<string> ownerIds = null, List<string> assigneeIds = null, List<string> tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null);
        Task<IList<TeamWorkloadViewModel>> ReadManagerProjectTaskViewData(string projectId, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null);
        Task<IList<TeamWorkloadViewModel>> ReadPerformanceTaskViewData(string projectId, string performanceUser, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null,string type=null);
        Task<IList<ServiceViewModel>> GetPerformanceDocumentStageDataByServiceId(string performanceServiceId, string ownerUserId, string stageId = null);
        Task<IList<TeamWorkloadViewModel>> ReadGoalServiceData(string performanceId);
        Task<ProgramDashboardViewModel> ReadProjectTotalTaskData(string projectId, string templatecode);
        Task<IList<TeamWorkloadViewModel>> ReadManagerPerformanceGoalViewData(string performanceId, string stageId, string userId);
        Task<IList<TeamWorkloadViewModel>> GetAllApprovedGoalForManager(string performanceId);
        Task<IList<ServiceViewModel>> GetPerDocMasterStageDataByServiceId(string performanceServiceId, string ownerUserId, string stageId = null);
        Task<IList<ServiceViewModel>> ReadPerformanceDocumentStagesData(string performanceId, string stageId = null);
        Task<IList<TeamWorkloadViewModel>> ReadPerformanceCompetencyStageViewData(string performanceId, string stageId, string userId);
        Task<IList<TeamWorkloadViewModel>> ReadPerformanceDevelopmentViewData(string performanceId, string stageId, string userId);
        Task<IList<TeamWorkloadViewModel>> ReadPerformanceAllData(string performanceId,string stageId, string userId);
        Task<IList<TeamWorkloadViewModel>> ReadProjectStageViewData(string projectId);
       // Task<IList<DashboardCalendarViewModel>> ReadManagerPerformanceCalendarViewData(string projectId, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null);
        Task<IList<DashboardCalendarViewModel>> ReadPerformanceCalendarViewData(string projectId, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null, string performanceStageId = null);
        Task<IList<TeamWorkloadViewModel>> ReadProjectSubTaskViewData(string taskId, string id, string status);
        Task<IList<TeamWorkloadViewModel>> ReadProjectTeamWorkloadData(string projectId, string userId, bool isProjectManager = false);
        Task<IList<TeamWorkloadViewModel>> ReadProjectTeamDataByUser(string projectId, string userId);
        Task<IList<TeamWorkloadViewModel>> ReadManagerProjectTaskAssignedData(string projectId, string templatecode);
        Task<IList<TeamWorkloadViewModel>> ReadManagerPerformanceTaskAssignedData(string projectId, string templatecode);
        Task<IList<TeamWorkloadViewModel>> ReadProjectTaskAssignedData(string projectId, string templatecode);
        Task<IList<TeamWorkloadViewModel>> ReadManagerProjectTaskOwnerData(string projectId, string templatecode);
        Task<IList<TeamWorkloadViewModel>> ReadProjectTaskOwnerData(string projectId, string templatecode);
        Task<IList<TreeViewViewModel>> GetWBSItemData(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string projectId, string expandingList, string userroleCode);
        Task<IList<TeamWorkloadViewModel>> ReadProjectTeamDateData(string projectId);
        Task<IList<TeamWorkloadViewModel>> ReadProjectTeamDataByDate(string projectId, DateTime startDate, bool isProjectManager = false);
        Task<IList<ProjectGanttTaskViewModel>> ReadPerformanceTaskGridViewData(string userId, string performanceId,string objectiveId,string userRole, List<string> projectIds = null, List<string> ownerIds = null, List<string> assigneeIds = null, List<string> tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null, List<string> type =null, InboxTypeEnum? inboxType=null);
        Task<IList<IdNameViewModel>> GetPerformanceSharedList(string userId);
        Task<PerformanceDashboardViewModel> GetPerformanceDashboardDetails(string projectId, string stageId = null);
        Task<List<ProjectDashboardChartViewModel>> GetTaskStatus(string userId,string projectId,string stageId=null);
        Task<List<ProjectDashboardChartViewModel>> GetPerformanceServicesStatus(string UserId,string projectIds,string type,string stageId=null);
        Task<List<ProjectDashboardChartViewModel>> GetPerformanceServiceStatus(string userId, string projectId, string type, string stageId = null);
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

        Task<List<NtsTaskChartList>> GetGridList(string TemplateID, string UserID, List<string> assigneeIds = null, List<string> StatusIDs = null);

        #endregion
        Task<PerformanceDocumentViewModel> GetPerformanceDocumentDetails(string Id);
        Task<IList<PerformanceDocumentStageViewModel>> GetPerformanceDocumentStageData(string perDocId, string perDocStageId=null,string udfNoteId=null, bool isEnableReview = false);
        Task<List<PerformanceDocumentViewModel>> GetPerformanceDocumentsList();
        Task<PerformanceDocumentStageViewModel> GetPerformanceDocumentStage(string Id);
        
        Task<IList<ProjectDashboardChartViewModel>> GetTaskType(string userId, string projectId,string stageId=null);
        Task<IList<TaskViewModel>> ReadTaskOverdueData(string projectId);
        Task<IList<ProjectGanttTaskViewModel>> ReadPerformanceUserWorkloadGridViewData(string userIds, List<string> projectIds = null, List<string> ownerIds = null, List<string> assigneeIds = null, List<string> tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null);
        //Task<IList<ProjectGanttTaskViewModel>> ReadProjectTaskGrid(string projectId);
        Task<bool> CreateMindMap(string model);
        Task<IList<ProjectDashboardChartViewModel>> ReadPerformanceStageChartData(string userId, string projectId);
        Task<IList<ProjectGanttTaskViewModel>> ReadProjectTask(string userId, string projectId, bool isProjectManager = false, List<string> projectIds = null, List<string> ownerIds = null, List<string> assigneeIds = null, List<string> tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null);
        Task<IList<IdNameViewModel>> GetPerformanceStageIdNameList(string userId, string projectId, List<string> ownerIds = null, List<string> types = null);
        Task<IList<IdNameViewModel>> GetPerformanceObjectiveList(string userId, string projectId);
        Task<IList<IdNameViewModel>> GetPerformanceUserIdNameList(string projectId);
        Task<IList<MailViewModel>> ReadEmailTaskData(string userId);
        Task<IList<WBSViewModel>> ReadProjectTaskForEmailList(string projectId);
        Task<IList<IdNameViewModel>> ReadPerformanceTaskUserData(string projectId);
        Task<List<ProjectDashboardChartViewModel>> GetTaskStatusByTemplate(string templateId, string userId);
        Task<List<ProjectDashboardChartViewModel>> GetTaskByUsers(string templateId, string userId);
        Task<IList<ProjectGanttTaskViewModel>> ReadTaskGridViewData(string templateId, string userId, List<string> tasksStatus = null, List<string> ownerIds = null);
        Task<IList<IdNameViewModel>> GetTaskOwnerUsersList(string templateId, string userId);
        Task<List<ProjectDashboardChartViewModel>> GetSLADetails(string templateId, string userId, DateTime? FromDate = null, DateTime? ToDate = null);
        Task<List<ProjectDashboardChartViewModel>> GetPMTaskStatusByTemplate(string templateId);
        Task<List<ProjectDashboardChartViewModel>> GetPMTaskByUsers(string templateId);
        Task<IList<ProjectGanttTaskViewModel>> ReadPMTaskGridViewData(string templateId, List<string> tasksStatus = null, List<string> ownerIds = null);
        Task<List<ProjectDashboardChartViewModel>> GetPMSLADetails(string templateId, DateTime? FromDate = null, DateTime? ToDate = null);
        Task<IList<PerformanceDocumentViewModel>> GetPerformanceGradeRatingData(string parentNoteId, string noteId=null, string udfNoteId=null);
        Task<IList<IdNameViewModel>> GetTaskUsersList(string templateId);
        Task<List<ProjectDashboardChartViewModel>> GetTaskStatusRequestedByMeGroup(string TemplateID, string UserID, string StatusLOV);
        Task<PerformanceDocumentViewModel> GetPDMByServiceId(string serviceId);
        Task<PerformanceDocumentViewModel> GetPerformanceDocumentMasterByDocServiceId(string serviceId);
        Task<List<ProjectDashboardChartViewModel>> GetDatewiseTaskGroup(string TemplateID, string UserID, DateTime? FromDate = null, DateTime? ToDate = null);
        Task<List<ProjectGanttTaskViewModel>> GetTaskListByType(string templateCodes, string pdmId = null, string ownerId = null, string stageId = null);
        Task<List<ProjectGanttTaskViewModel>> GetStageTaskList(string templateCodes, string pdmId = null, string ownerId = null, string stageId = null);
        Task<List<ProjectDashboardChartViewModel>> GetTaskStatusAssigneduseridGroup(string TemplateID, string UserID, string StatusTemplateId, string StatusLOV);

        Task<List<ProjectDashboardChartViewModel>> MdlassignUserGroup(string TemplateID, string UserID);

        Task<List<NtsTaskChartList>> GetGridListGroup(string TemplateID, string UserID, List<string> assigneeIds = null, List<string> StatusIDs = null);

        Task<List<ProjectDashboardChartViewModel>> GetGroupTemplate();
        Task<IList<IdNameViewModel>> GetPDMList(string year = null);
        Task<ServiceViewModel> GetPDMDetails(string pdmId);
        Task<PerformanceDocumentViewModel> IsDocNameExist(string docName, string docId);
        Task<IList<IdNameViewModel>> GetPerformanceGradeRatingList(string perRatingId);
        Task<List<ProjectDashboardChartViewModel>> GetTaskStatusRequestedByMeProjectGroup(string TemplateID, string StatusLOV = null);
        Task<List<ProjectDashboardChartViewModel>> GetChartByAssigneduserProjectGroup(string templateId, string StatusTemplateID = null, string StatusLOV = null);

        Task<IList<ProjectGanttTaskViewModel>> GetGridListProjectGroup(string templateId, List<string> tasksStatus = null, List<string> ownerIds = null);
        Task<IList<ProjectDashboardChartViewModel>> GetTaskStatusAssigneduseridProjectGroup(string templateId);
        Task<IList<ProjectDashboardChartViewModel>> GetDatewiseTaskProjectGroup(string templateId, DateTime? FromDate, DateTime? ToDate);
        Task<IList<IdNameViewModel>> GetSubordinatesIdNameList();

        Task<List<ProjectDashboardChartViewModel>> GetTaskStatusByTemplateGroup(string templateId, string userId, string StatusLOV = null);
        Task<List<ProjectDashboardChartViewModel>> GetTaskByUsersGroup(string templateId, string userId, string StatusTemplateID = null, string StatusLOV = null);
        Task<IList<ProjectGanttTaskViewModel>> ReadTaskGridViewDataGroup(string templateId, string userId, List<string> tasksStatus = null, List<string> ownerIds = null);

        Task<IList<IdNameViewModel>> GetTaskOwnerUsersListGroup(string templateId, string userId);

        Task<IList<ProjectDashboardChartViewModel>> GetDatewiseSingleGroup(string templateId, string userId, DateTime? FromDate, DateTime? ToDate);
        Task<IList<ProjectGanttTaskViewModel>> ReadPerformanceTaskUserGridViewData(string userId, List<string> projectIds = null, List<string> ownerIds = null, List<string> assigneeIds = null, List<string> tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null, List<string> pmsTypes = null,List<string> stageIds=null);
        Task<IList<ProjectGanttTaskViewModel>> ReadPerformanceObjectivesGridViewData(string userId, List<string> projectIds = null, List<string> ownerIds = null, List<string> assigneeIds = null, List<string> tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null, List<string> pmsTypes = null,List<string> stageIds=null,string statusCodes=null);
        Task<IList<ProjectGanttTaskViewModel>> ReadEmployeePerformanceObjectivesData(string userId, List<string> projectIds = null, List<string> ownerIds = null, List<string> assigneeIds = null, List<string> tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null, List<string> pmsTypes = null, List<string> stageIds = null, string statusCodes = null);
        Task<CommandResult<PerformanceDocumentViewModel>> GeneratePerformanceDocument(string pdmId);
        Task<CommandResult<PerformanceDocumentViewModel>> PublishDocumentMaster(string pdmId);
        Task<IList<PerformanceDocumentViewModel>> GetPerformanceDocumentMappedUserData(string perDocId, string userid);
        Task<IList<IdNameViewModel>> GetPerDocMasMappedUserData(string perDocId);
        Task<IList<TreeViewViewModel>> GetDiagramMenuItem(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string projectId, string expandingList, string userroleCode);
        Task<CommandResult<PerformanceDocumentViewModel>> CreatePerDoc(PerformanceDocumentViewModel model);
        Task<CommandResult<PerformanceDocumentViewModel>> EditPerDoc(PerformanceDocumentViewModel model);       
        Task<CommandResult<PerformanceDocumentStageViewModel>> CreatePerDocStage(PerformanceDocumentStageViewModel model);
        Task<CommandResult<PerformanceDocumentStageViewModel>> EditPerDocStage(PerformanceDocumentStageViewModel model);
        Task<bool> DeleteDocumentStage(string noteId);
        Task<List<IdNameViewModel>> GetPerformanceDocumentGoalTemplates();
        Task GetInboxMenuItem(string id, string userRoleCodes);
        Task<List<IdNameViewModel>> GetPerformanceDocumentCompetencyTemplates();
         Task<List<IdNameViewModel>> GetEmployeeReviewTemplate();
        Task<List<IdNameViewModel>> GetManagerReviewTemplate();
        Task<IList<ServiceViewModel>> ReadPerformanceDocumentGoalData(string performanceId, string userId, string masterStageId);
        Task GetInboxMenuItemByUser(string id, string userRoleCodes);
        Task<IList<ServiceViewModel>> ReadPerformanceDocumentCompetencyData(string performanceId, string userId, string masterStageId);
        Task TriggerReviewGoal(ServiceTemplateViewModel viewModel);
        Task<IList<PerformanceDocumentViewModel>> GetPerformanceDocumentList(string userId);
        Task<IList<ProjectGanttTaskViewModel>> GetPerformanceDocumentTaskList();
        Task CalculatePerformanceRating(string documentMasterId,string masterStageId);
        Task<IList<PerformanceDocumentViewModel>> GetPerformanceDocumentDetailsData(string PerformanceDocumentId,string deptId=null,string userId=null,string pdmStageId=null);

        Task updateGoalRating(string Id, string RatingId,string type);
        Task updateCompentancyRating(string Id, string RatingId,string type);

        Task<PerformaceRatingViewModel> GetPerformanceRatingDetails(string Id);

        Task DeletePerformanceRating(string Id);

        Task<List<PerformanceRatingItemViewModel>> GetPerformanceRatingItemList(string ParentNodeId);

        Task<PerformanceRatingItemViewModel> GetPerformanceRatingItemDetails(string Id);

        Task DeletePerformanceRatingItem(string Id);

        Task<PerformanceRatingItemViewModel> IsRatingItemExist(string Parentid, string Name, string Id);
        Task<PerformanceRatingItemViewModel> IsRatingItemCodeExist(string Parentid, string code, string Id);
        Task<List<ServiceViewModel>> GetServiceListByPDMId(string pdmId, string templateCodes, string ownerUserId);
       Task<PerformaceRatingViewModel> IsRatingNameExist(string Name, string Id);

        Task<List<PerformaceRatingViewModel>> GetPerformanceRatingList();
        Task<IList<CompetencyCategoryViewModel>> GetCompetencyMaster();
        Task<IList<CompetencyCategoryViewModel>> ReadCompetencyMasterJob(string noteid);
        
        Task<List<CompetencyCategoryViewModel>> GetPerformanceTaskCompetencyCategory();
        Task<List<CompetencyViewModel>> GetPerformanceTaskCompetencyMaster(string templateCode, string categoryCode);

        Task<CompetencyCategoryViewModel> IsCompetencyCategoryNameExist(string Name, string Id);
        Task<CompetencyCategoryViewModel> IsCompetencyCategoryCodeExist(string Code, string Id);
        Task<CompetencyCategoryViewModel> GetcompetencyCategoryDetails(string Id);
        Task<List<IdNameViewModel>> GetParentCompatencyCategory();

        Task<CompetencyCategoryViewModel> IsParentAssignToCompetencyCategoryExist(string ParentId, string Id);
        Task DeleteCompetencyCategory(string Id);
        Task DeleteCompetency(string Id);
        Task DeleteGoal(string Id);
        Task DeleteDevelopment(string Id);
        Task DeletService(string Id);
        Task<List<IdNameViewModel>> GetDepartmentListByOrganization(string organizationId);
        Task<List<IdNameViewModel>> GetAllYearFromPerformanceMaster(string departmentId);
        Task<PerformanceDocumentViewModel> GetPerformanceDocumentMasterByNoteId(string noteId);
        Task<bool?> UpdatePerformanceDocumentMasterStageStatus(string id, PerformanceDocumentStatusEnum status);
        Task<bool?> UpdatePerformanceDocumentMasterStatus(string id, PerformanceDocumentStatusEnum status);

        Task<List<CompetencyCategoryViewModel>> GetCompotencyDetails();

        Task<List<IdNameViewModel>> GetAllPerformanceDocument();
        Task<List<IdNameViewModel>> GetAllDepartment();
        Task<PerformanceDocumentViewModel> GetPerformanceDocumentMasterByServiceId(string serviceId);
        Task<IList<PerformanceDashboardViewModel>> GetPerformanceSummaryData(string filter);
        Task<IList<ServiceViewModel>> GetPerformanceSummaryDetail(string filter, string status, string serviceId);
        Task<List<IdNameViewModel>> GetYearByUserId(string userId);
        Task<List<IdNameViewModel>> GetRatingDetailsFromDocumentMaster(string Id);
        Task<IList<PerformanceDocumentViewModel>> GetPerformanceFinalReport(string documentMasterId, string departmentId = null, string userId = null, string stageId = null);
        Task TriggerAdhocTasksGoals(ServiceTemplateViewModel viewModel);
        Task TriggerAdhocTasksCompetency(ServiceTemplateViewModel viewModel);
        //Task TriggerReviewAdhocTasks(PerformanceDocumentStageViewModel masterStageId,string userId);
        Task<List<IdNameViewModel>> GetParentGoalByDepartment(string departmentId);
        Task<List<IdNameViewModel>> GetPerformanceMasterByDepatment(string departmentId, string year);
        Task<List<GoalViewModel>> GetDepartmentGoalByDepartment(string departmentId, string masterId);
        Task<List<IdNameViewModel>> GetYearByDepartment(string departmentId);
        Task<IList<IdNameViewModel>> GetDepartmentList();
        Task MapDepartmentUser(NoteTemplateViewModel viewModel);
        Task<List<IdNameViewModel>> GetDepartmentGoal(string departmentId);
        Task<string> GetParentGoal(string goalId);
        Task<List<IdNameViewModel>> GetDepartmentBasedOnUser();
        Task<IList<TeamWorkloadViewModel>> ReadPerformanceCompetencyServiceData(string performanceId);
        Task<IList<TeamWorkloadViewModel>> GetAllApprovedCompetenciesForManager(string performanceId);
        Task TriggerReviewAdhocTasks(ServiceTemplateViewModel viewModel);
        Task<bool> GeneratePerformanceDocumentStages(string masterStageId);
       // Task<CommandResult<PerformanceDocumentStageViewModel>> ChangeStatusforDocumentMasterStage(PerformanceDocumentStatusEnum status, PerformanceDocumentStageViewModel model);
        Task<PerformanceDocumentViewModel> GetPerformanceDocumentByMaster(string ownerUserId, string docMasterId);
        Task<List<TeamWorkloadViewModel>> GetAllApprovedGoals(string ownerUserId, string docServiceId,string stageId);
        Task<List<TeamWorkloadViewModel>> GetAllApprovedCompetencies(string ownerUserId, string docServiceId,string stageId);
        Task<List<TeamWorkloadViewModel>> GetAllStageCompetencies(string ownerUserId, string docServiceId,string stageId, DateTime? startDate, DateTime? endDate);
        Task<List<TeamWorkloadViewModel>> GetAllStageGoals(string ownerUserId, string docServiceId,string stageId, DateTime? startDate, DateTime? endDate);
        Task<PerformanceDocumentStageViewModel> GetPerformanceDocumentMasterStageById(string noteId);
        Task<List<ServiceViewModel>> LoadWorkBooks(string userIds, string docId = null);

        Task<ServiceTemplateViewModel> GetBookDetails(string serviceId);
        Task<GoalViewModel> GetGoalDataById(string id);
        Task<CompetencyViewModel> GetCompetencyDataById(string id);
        Task<CompetencyViewModel> GetDevelopmentDataById(string id);
        Task<List<NtsViewModel>> GetBookList(string serviceId, string templateId, bool includeitemDetails = false);
        Task<IList<NotificationViewModel>> GetNotificationsList(string refIds);

    }
}
