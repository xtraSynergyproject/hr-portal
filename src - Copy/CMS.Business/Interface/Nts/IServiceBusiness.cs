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
    public interface IServiceBusiness : IBusinessBase<ServiceViewModel, NtsService>
    {
        Task<CommandResult<ServiceTemplateViewModel>> ManageService(ServiceTemplateViewModel model);
        Task<ServiceTemplateViewModel> GetServiceDetails(ServiceTemplateViewModel viewModel);
        Task<string> GetSelectQuery(TableMetadataViewModel tableMetaData, string where = null, string filtercolumns = null, string filter = null, bool isLog = false, string logId = null);
        Task<DataTable> GetServiceDataTableById(string serviceId, TableMetadataViewModel tableMetadata, bool isLog = false, string logId = null);
        Task<IList<NTSMessageViewModel>> GetServiceAttachedReplies(string userId, string serviceId);
        Task<CommandResult<ServiceTemplateViewModel>> DeleteService(ServiceTemplateViewModel model);
        Task GetServiceIndexPageCount(ServiceIndexPageTemplateViewModel model, PageViewModel page);
        Task<List<NtsLogViewModel>> GetVersionDetails(string serviceId);
        Task<DataTable> GetServiceIndexPageGridData(DataSourceRequest request, string indexPageTemplateId, NtsActiveUserTypeEnum ownerType, string serviceStatusCode);
        Task<ServiceIndexPageTemplateViewModel> GetServiceIndexPageViewModel(PageViewModel page);
        Task<IList<ServiceViewModel>> GetNtsServiceIndexPageGridData(DataSourceRequest request, NtsActiveUserTypeEnum ownerType, string taskStatusCode, string categoryCode, string templateCode, string moduleCode);
        Task GetNtsServiceIndexPageCount(NtsServiceIndexPageViewModel model);
        Task<List<ColumnMetadataViewModel>> GetViewableColumns(TableMetadataViewModel tableMetaData);
        Task<WorklistDashboardSummaryViewModel> GetWorklistDashboardCount(string userId, string moduleName = null, string templateCategoryCode = null, string taskTemplateIds = null, string serviceTemplateIds = null);
        Task<IList<ServiceViewModel>> GetSearchResult(ServiceSearchViewModel searchModel);
        Task<IList<ProjectDashboardChartViewModel>> GetDatewiseServiceSLA(ServiceSearchViewModel searchModel);
        Task<List<IdNameViewModel>> GetSharedList(string ServiceId);
        Task<bool> IsExitEntryFeeAvailed(string userId);
        Task<NoteViewModel> GetFBDashboardCount(string userId, string moduleId = null);
        Task<IList<ServiceViewModel>> GetServiceByUser(string userId);
        Task ReAssignTerminatedEmployeeServices(string userId, List<string> serviceList);
        Task<List<DashboardCalendarViewModel>> GetWorkPerformanceCount(string userId, string moduleCodes = null, DateTime? fromDate = null, DateTime? toDate = null);

        Task<List<IdNameViewModel>> GetCMSExternalRequest();
        Task<List<ServiceViewModel>> GetServiceList(string portalId, string moduleCodes, string templateCodes, string categoryCodes, string requestby = null, bool showAllOwnersService = false,string statusCodes=null, string parentServiceId = null);
        Task<List<ServiceViewModel>> GetInternalServiceListFromExternalRequestId(string serviceId);
        Task<List<ProjectDashboardChartViewModel>> GetExternalServiceChartByStatus();
        Task<List<ProjectDashboardChartViewModel>> GetInternalServiceChartByStatus();
        Task<List<ProjectDashboardChartViewModel>> GetInternalDashboardChartByStatus(ServiceSearchViewModel search);
        Task<IList<ProjectDashboardChartViewModel>> GetExternalServiceSLA(ServiceSearchViewModel searchModel);
        Task<IList<ProjectDashboardChartViewModel>> GetRequestSLA(ServiceSearchViewModel searchModel);
        Task<List<ServiceViewModel>> GetSEBIServiceList();
        Task<List<ServiceViewModel>> GetInternalDashboardServiceList(ServiceSearchViewModel searchModel);
        Task<List<ProjectDashboardChartViewModel>> GetInternalDashboardTaskChart(ServiceSearchViewModel search);
        Task<List<ServiceViewModel>> GetExternalSEBIServiceList();
        Task<List<ProjectDashboardChartViewModel>> GetExternalUserServiceChartByStatus();
        Task<List<ProjectDashboardChartViewModel>> GetExternalUserInternalServiceChartByStatus();
        Task<IList<ProjectDashboardChartViewModel>> GetExternalUserExternalServiceSLA(ServiceSearchViewModel searchModel);
        Task<List<ServiceViewModel>> GetSEBIExternalServiceList(string tasktemplatecode);
        Task<List<NtsLogViewModel>> GetServiceLog(string ServiceId, string TemplateCode);

        Task<List<IdNameViewModel>> GetServiceUserList(string serviceId);
        Task SendNotification(ServiceTemplateViewModel viewModel, NotificationTemplate item, string toUserId);
        Task<DataTable> GetDynamicService(string TemplateCode, string ServiceId, string TemplateType);
        Task<List<string>> GetDynamicServiceColumnLst(string TemplateCode, string TemplateType);
        Task<IList<TreeViewViewModel>> GetDocumentTreeviewList(string id, string type, string parentId, string serviceId, string expandingList);

        Task<IList<TreeViewViewModel>> GetDocumentIndexTreeviewList(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string batchId, string expandingList, string userRoleCodes, string docServiceId);
        Task<List<NtsViewModel>> GetBookList(string serviceId, string templateId, bool includeitemDetails = false);
        Task<NtsBookItemViewModel> GetBookById(string id);
        Task<List<NtsViewModel>> GetServiceBookDetails(string serviceId);
        Task<CommandResult<ServiceViewModel>> ManageMoveToParent(ServiceViewModel model);
        Task DeleteServiceBook(string serviceId);
        Task DeleteServiceBookItem(string serviceId, string parentId, NtsTypeEnum parentType);
        Task<List<ServiceViewModel>> ReadServiceBookData(string moduleCodes, string templateCodes, string categoryCodes);
        Task<List<ColumnMetadataViewModel>> GetServiceColumnById(string ServiceId);
        Task<ServiceTemplateViewModel> GetBookDetails(string serviceId);
        Task<List<ColumnMetadataViewModel>> GetServiceViewableColumns();
        Task<string> GetServiceSelectQuery(string where = null);
        Task UpdateServiceWorkflowStatus(string serviceId, string WorkflowStatus,string taskId);
        Task<DataTable> GetCustomServiceIndexPageGridData(DataSourceRequest request, string templateId, bool showAllOwnersService, string moduleCodes, string templateCodes, string categoryCodes);
        Task<ServiceTemplateViewModel> GetFormIoData(string templateId, string serviceId, string userId);
        Task<List<BookViewModel>> GetAllBook(string templateCodes, string templateIds, string bookIds, string search,string categoryIds,string permission);
        Task<List<IdNameViewModel>> GetProcessBookType(string templateCodes);
        Task<List<IdNameViewModel>> GetProcessBook(string templateCodes);
        Task<BookViewModel> GetBookDetail(string bookid);
        Task<BookViewModel> GetBookPageDetail(string bookid, string currentPageId);
        Task<List<IdNameViewModel>> GetBookAllPages(string bookid);
        Task<List<IdNameViewModel>> GetBookAllDirectPages(string bookid);
        Task<List<IdNameViewModel>> GetChildPageList(string pageId);
        Task<bool> ValidateProjectName(ServiceTemplateViewModel viewModel);
        Task<List<TreeViewViewModel>> GetBookTreeList(string id,string templateCode,string permission);
        Task<List<BookViewModel>> GetAllProcessBook(string templateCodes);
        Task<List<BookViewModel>> GetAllBookPages();
        Task<List<BookViewModel>> GetBookAllPagesByBookId(string bookid);
        Task<List<BookRealtionViewModel>> GetAllBookRelationBySourceId(string sourceId);
        Task CreateBookPageMapping(string pageId, string bookId, long? sequenceOrder);
        Task DeleteBokPageMapping(string bookId, string pageId);
        Task<List<BookViewModel>> GetGroupBookByCategoryId(string categoryId);
        Task UpdateCategorySequenceOrderOnDelete(string parentServiceId, long? sequenceOrder, string serviceId, string TemplateCode);
        Task<List<IdNameViewModel>> GetBookByPageIdPages(string pageid);
    }
}
