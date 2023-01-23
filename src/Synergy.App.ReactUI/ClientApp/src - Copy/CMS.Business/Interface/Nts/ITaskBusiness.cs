using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface ITaskBusiness : IBusinessBase<TaskViewModel, NtsTask>
    {
        Task<CommandResult<TaskTemplateViewModel>> ManageTask(TaskTemplateViewModel model);
        Task<CommandResult<TaskViewModel>> CreateEmail(TaskViewModel model);
        Task<TaskTemplateViewModel> GetTaskDetails(TaskTemplateViewModel viewModel);
        Task<string> GetSelectQuery(TableMetadataViewModel tableMetaData, string where = null, string filtercolumns = null, string filter = null, bool isLog = false, string logId = null);
        Task<DataTable> GetTaskDataTableById(string taskId, TableMetadataViewModel tableMetadata, bool isLog = false, string logId = null);
        Task<List<NTSMessageViewModel>> GetTaskMessageList(string userId, string taskId);
        Task<CommandResult<TaskTemplateViewModel>> DeleteTask(TaskTemplateViewModel model);
        Task GetTaskIndexPageCount(TaskIndexPageTemplateViewModel model, PageViewModel page);
        Task<DataTable> GetTaskIndexPageGridData(DataSourceRequest request, string indexPageTemplateId, NtsActiveUserTypeEnum ownerType, string taskStatusCode);
        Task<TaskIndexPageTemplateViewModel> GetTaskIndexPageViewModel(PageViewModel page);
        Task<IList<TaskViewModel>> GetNtsTaskIndexPageGridData(DataSourceRequest request, NtsActiveUserTypeEnum ownerType, string taskStatusCode, string categoryCode, string templateCode, string moduleCode);
        Task GetNtsTaskIndexPageCount(NtsTaskIndexPageViewModel model);
        Task<List<ColumnMetadataViewModel>> GetViewableColumns(TableMetadataViewModel tableMetaData);
        Task<IList<TaskViewModel>> GetServiceAdhocTaskGridData(DataSourceRequest request, string adhocTaskTemplateIds, string serviceId);
        Task<IList<TaskViewModel>> GetSearchResult(TaskSearchViewModel searchModel);
        Task<IList<ProjectDashboardChartViewModel>> GetDatewiseTaskSLA(TaskSearchViewModel searchModel);
        Task<List<IdNameViewModel>> GetSharedList(string TaskId);
        Task<IList<TaskViewModel>> GetTaskByUser(string userId);
        Task ReAssignTerminatedEmployeeTasks(string userId, List<string> taskList);
        Task<List<NtsLogViewModel>> GetVersionDetails(string taskId);
        Task<IList<TaskViewModel>> GetTaskList(string portalId, string moduleCodes = null, string templateCodes = null, string categoryCodes = null, string statusCodes = null, string parentServiceId = null, string userId = null, string parentNoteId = null);
        Task<IList<ProjectDashboardChartViewModel>> GetWorkPerformanceTaskList(TaskSearchViewModel search);
        Task<List<DashboardCalendarViewModel>> GetWorkPerformanceCount(string userId, string moduleCodes = null, DateTime? fromDate = null, DateTime? toDate = null);
        Task SendNotification(TaskTemplateViewModel viewModel, NotificationTemplate item, string toUserId);
        Task<List<IdNameViewModel>> GetTaskUserList(string taskId);
        Task<string> ChangeAssignee(string taskId, string userId);
        Task<string> ChangeLockStatus(string taskId);
        Task<string> UpdateActualStartDate(string taskId);
        Task<List<ColumnMetadataViewModel>> GetTaskViewableColumns();
        Task<TaskTemplateViewModel> GetFormIoData(string templateId, string taskId, string userId);
        Task<WorklistDashboardSummaryViewModel> TaskCountForDashboard(string userId,string bookId);
        Task<List<TaskViewModel>> TaskDashboardIndex(string userId, string statusFilter = null);
        Task<List<NoteViewModel>> LoadWorkBooks(string userId, string statusFilter = null);
        Task<List<NoteViewModel>> LoadProcessBooks(string userId, string statusFilter = null);
        Task<List<NoteViewModel>> LoadProcessStage(string userId, string statusFilter = null);
        Task<List<NotificationViewModel>> NotificationDashboardIndex(string userId,string bookId);
        Task<IList<NtsTaskIndexPageViewModel>> GetTaskCountByServiceTemplateCodes(string categoryCodes);
        Task<IList<TaskViewModel>> GetTaskListByServiceCategoryCodes(string categoryCodes, string status);
        Task<bool> UpdateStepTaskAssignee(string taskId, string ownerUserId);
        Task<IList<TaskViewModel>> GetWorkboardTaskList(string portalId, string moduleCodes = null, string templateCodes = null, string categoryCodes = null, string statusCodes = null, string parentServiceId = null, string userId = null, string parentNoteId = null);
    }
}
