using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
////using Kendo.Mvc.UI;
using System.Data;
using System.Linq.Expressions;


namespace Synergy.App.Business
{
    public interface ICmsQueryBusiness : IBusinessBase<TemplateViewModel, Template>
    {
        #region CmsBusiness
        Task<bool> ManageTableExists(TableMetadataViewModel existingTableMetadata);
        Task<bool?> ManageTableRecordExists(TableMetadataViewModel existingTableMetadata);
        Task EditTableSchema(TableMetadataViewModel tableMetadata);
        Task CreateTableSchema(TableMetadataViewModel tableMetadata, bool dropTable);
        Task TableQueryExecute(string tableQuery);
        Task<DataTable> GetQueryDataTable(string selectQuery);
        Task<DataTable> GetEditNoteTableExistColumn(TableMetadataViewModel tableMetadata);
        Task<DataTable> GetEditNoteTableConstraints(TableMetadataViewModel tableMetadata);
        Task<DataTable> GetEditTaskTableExistColumn(TableMetadataViewModel tableMetadata);
        Task<DataTable> GetEditTaskTableConstraints(TableMetadataViewModel tableMetadata);
        Task<DataTable> GetEditServiceTableExistColumn(TableMetadataViewModel tableMetadata);
        Task<DataTable> GetEditServiceTableConstraints(TableMetadataViewModel tableMetadata);
        Task<DataTable> GetEditFormTableExistColumn(TableMetadataViewModel tableMetadata);
        Task<DataTable> GetEditFormTableConstraints(TableMetadataViewModel tableMetadata);
        Task<List<ColumnMetadataViewModel>> GetForeignKeyColumnByTableMetadata(TableMetadataViewModel tableMetaData);
        Task<DataTable> GetDataByColumn(ColumnMetadataViewModel column, object columnValue, TableMetadataViewModel tableMetaData, string excludeId);
        Task DeleteFrom(FormTemplateViewModel model, TableMetadataViewModel tableMetaData);
        Task<List<EmailTaskViewModel>> ReadEmailTaskData(string refId, ReferenceTypeEnum refType, TaskViewModel taskdetails);
        Task<List<IdNameViewModel>> GetActivePersonList();
        Task<List<IdNameViewModel>> GetPayrollElementList();
        Task<TestViewModel> Test();
        Task<List<TreeViewViewModel>> GetInboxTreeviewList(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string batchId, string expandingList, string userRoleCodes, string inboxCode);
        Task<List<TreeViewViewModel>> GetInboxMenuItem(string id, string type, string templateCode);
        Task<List<TaskTemplateViewModel>> ReadInboxData(string id, string type, string templateCode, string userId = null);
        Task<List<TASTreeViewViewModel>> GetInboxMenuItem(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string projectId, string expandingList, string userroleCode);
        Task<List<TASTreeViewViewModel>> GetInboxMenuItemByUser(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string projectId, string expandingList, string userroleCode);
        Task<string> GetLatestMigrationScript();
        Task<TemplateViewModel> ExecuteMigrationScript(string script);
        Task<List<string>> GetAllMigrationsList();
        Task<List<TemplateViewModel>> GetTemplateListByTemplateType(int i);
        #endregion


        #region TemplateBusiness

        Task<List<TemplateViewModel>> GetTemplateDeleteList();
        Task SetOcrTemplateFileId(string templateId, string fileId);
        Task<List<TemplateViewModel>> GetTemplateServiceListbyTeam(string tCode, string teamId);
        Task<List<WorkflowViewModel>> GetTemplateDetail(string id);
        Task<List<WorkflowViewModel>> GetWorkflowDiagramDetailsByTemplate(string id);
        Task<List<WorkflowViewModel>> GetServiceDetail(string id);
        Task<List<WorkflowViewModel>> GetWorkflowDiagramDetails(string id);
        Task<List<TemplateViewModel>> GetTemplateServiceList(string tCode, string tcCode, string mCodes, string templateIds, string categoryIds, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, bool allBooks = false, string portalNames = null, ServiceTypeEnum? serviceType = null, string groupCodes = null);
        Task<List<TemplateViewModel>> GetTemplateListByTaskTemplate(string taskTemplateId);
        Task<List<BusinessDiagramNodeViewModel>> GetTemplateBusinessDiagram(string templateId);
        Task<List<IdNameViewModel>> GetComponentsList(string templateId);
        Task<List<TemplateViewModel>> GetAllowedTemplateList(string categoryCode, string userId, TemplateTypeEnum? templateType, TaskTypeEnum? taskType, string portalCode = null);
        Task<List<TemplateViewModel>> GetNoteTemplateList(string tCode, string tcCode, string mCodes, string templateIds = null, string categoryIds = null, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, string portalNames = null);
        Task<List<TemplateViewModel>> GetTemplateList(string tCode, string tcCode, string mCodes, string templateIds, string categoryIds, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, string portalNames = null);
        Task<List<TemplateViewModel>> GetAdhocTemplateList(string tCode, string tcCode, string moduleId);
        Task<TemplateViewModel> CheckTemplate(TemplateViewModel model);
        Task<TemplateViewModel> SelectTemplateByName(TemplateViewModel viewModel);
        Task<TemplateViewModel> SelectTemplateByCode(TemplateViewModel viewModel);

        #endregion


        #region PageBusiness

        Task<List<PageViewModel>> PortalDetails();
        Task<MenuGroupViewModel> GetMenuGroup(string language, PageViewModel page);
        Task<PageViewModel> GetUserPagePermission(string portalId, string pageId);

        #endregion
        Task<List<ApplicationDocumentViewModel>> GetApplicationDocumentListData();
        Task<ApplicationDocumentViewModel> GetApplicationDocumentData(string documentCode);
        Task<List<ApplicationErrorViewModel>> GetApplicationErrorListData();
        Task<TableMetadataViewModel> GetViewableColumnMetadataListData(string schemaName, string tableName);
        Task<List<ColumnMetadataViewModel>> GetViewableForiegnKeyColumnListForFormData(string tableMetadataId);
        Task<List<ColumnMetadataViewModel>> GetViewableForiegnKeyColumnListForNoteData(string tableMetadataId);
        Task<List<ColumnMetadataViewModel>> GetViewableForiegnKeyColumnListForTaskData(string tableMetadataId);
        Task<List<ColumnMetadataViewModel>> GetViewableForiegnKeyColumnListForTaskData2();
        Task<List<ColumnMetadataViewModel>> GetViewableForiegnKeyColumnListForServiceData(string tableMetadataId);
        Task<List<ColumnMetadataViewModel>> GetViewableForiegnKeyColumnListForServiceData1(string tableMetadataId);
        Task<List<ComponentParentViewModel>> GetComponentParentData(string componentId);
        Task<List<ComponentParentViewModel>> GetComponentChildData(string componentId);
        Task RemoveParentsData(string componentId);
        Task<List<ComponentResultViewModel>> GetChildComponentResultListData(ComponentResultViewModel componentResult);
        Task<List<ComponentResultViewModel>> GetParentListData(string ntsServiceId, string componentId);
        Task<List<ComponentResultViewModel>> GetComponentResultListData(string serviceId);
        Task<List<TaskViewModel>> GetStepTaskListData(string serviceId);
        Task<List<TemplateViewModel>> GetStepTaskTemplateListData(string serviceTemplateId);
        Task<List<AssignmentViewModel>> GetUserListOnNtsBasisData(string id);
        Task<List<AssignmentViewModel>> GetUserListOnNtsBasisData1(string comp);
        Task<List<AssignmentViewModel>> GetUserListOnNtsBasisData2(string id);
        Task<List<AssignmentViewModel>> GetUserListOnNtsBasisData3(string id);
        Task<MessageEmailViewModel> GetTaskMessageIdData(string MessageId);
        Task<List<FileViewModel>> GetFileListData();
        Task<List<IdNameViewModel>> GetFileLogsDetailsData(string fileId);
        Task<List<FileViewModel>> GetFileLogsDetailsByIdData(string Id);
        Task<FileViewModel> GetFileLogsDetailsByFileIdAndVersionData(string FileId, long versionNo);
        Task<IList<GrantAccessViewModel>> GetGrantAccessListData(string userId);
        Task<IList<TaskViewModel>> GetBHServiceData(bool showAllService);
        Task<IList<TaskViewModel>> GetBHTaskData();
        Task<List<BusinessHierarchyPermissionViewModel>> GetBusinessHierarchyPermissionData(string groupCode);
        Task<List<MenuGroupViewModel>> GetParentMenuGroups(List<string> list);
        Task<string> GetBusinessHierarchyPermissionData2(string users);
        Task<bool> DeleteBusinessHierarchyPermissionData(string id, string noteId);
        Task<List<HybridHierarchyViewModel>> GetHierarchyPathData(string hierarchyItemId);
        Task<List<HybridHierarchyViewModel>> GetHierarchyParentDetailsData(string hierarchyItemId);
        Task<List<HybridHierarchyViewModel>> GetBusinessHierarchyChildListData(string parentId, int level, int levelupto, bool enableAOR, string bulkRequestId, bool includeParent, string userId, bool isAdmin);
        Task UpdateHierarchyPathData(HybridHierarchyViewModel hybridmodel, string values);
        Task<List<HybridHierarchyViewModel>> GetBusinessHierarchyListData(string referenceType = null, string searckKey = null, bool bindPath = false);
        Task<List<UserHierarchyPermissionViewModel>> GetBusinessHierarchyRootPermissionData(string PermissionId = null, string UserId = null);
        Task<List<BusinessHierarchyAORViewModel>> GetAllAORBusinessHierarchyListData();
        Task<List<LegalEntityViewModel>> GetData();
        Task<LegalEntityViewModel> GetFinancialYearData();
        Task<List<MenuGroupViewModel>> GetMenuGroupListData();
        Task<List<MenuGroupViewModel>> GetMenuGroupListPortalAdminData(string PortalId, string LegalEntityId);
        Task<List<MenuGroupViewModel>> GetMenuGroupListparentPortalAdminData(string PortalId, string LegalEntityId, string Id);
        Task<List<TagCategoryViewModel>> GetNtsTagData(NtsTypeEnum ntstype, string ntsId);
        Task<TagCategoryViewModel> GetTagCategoryDataByIdData(string categoryId);
        Task<TagCategoryViewModel> GetTagCategoryDataByNoteIdData(string NoteId);
        Task<TagViewModel> GetTagByNoteIdData(string NoteId);
        Task<List<TagViewModel>> TagListByCategoryNoteIdData(string NoteId);
        Task<List<PageViewModel>> GetMenuItemsData(Portal portal, string userId, string companyId, string groupCode = null);
        Task<List<MenuGroupDetailsViewModel>> GetMenuItemsData1(string ids);
        Task<List<PageDetailsViewModel>> GetMenuItemsData2(string ids);
        Task<List<PageViewModel>> GetPortalDiagramData(PortalViewModel portal, string userId, string companyId);
        Task<List<IdNameViewModel>> GetPortalForUserData(string portalIds);
        Task<List<IdNameViewModel>> GetPortalsData(string id);
        Task<List<PortalViewModel>> GetPortalListByPortalNames(string portalNames);
        Task<List<IdNameViewModel>> GetAllowedPortalsData(string portalIds);
        Task<List<PageViewModel>> GetMenuGroupOfPortalData(string portalIds);
        Task<List<PageViewModel>> GetMenuGroupOfPortalData1(string portalIds, string item);
        Task SetAllTaskNotificationReadData(string userId, string taskId);
        Task<IList<NotificationViewModel>> GetNotificationListData(string userId);
        Task SetAllNotificationReadData(string userId);
        Task<IList<IdNameViewModel>> GetComponentListData();
        Task<IList<SubModuleViewModel>> GetSubModuleListData();
        Task<IList<SubModuleViewModel>> GetPortalSubModuleListData();
        Task<DataTable> GetTableData(string tableMetadataId, string recordId, string name, string schema);
        Task<DataTable> GetTableDataByColumnData(string schema, string name, string udfName, string udfValue);
        Task<DataTable> GetTableDataByHeaderIdData(string schema, string name, string fieldName, string headerId);
        Task<DataTable> DeleteTableDataByHeaderIdData(string schema, string name, string fieldName, string headerId);
        Task EditTableDataByHeaderIdData(string schema, string name, List<string> columnKeys, string fieldName, string headerId);
        Task<DataTable> GetViewColumnByTableNameData(string schema, string tableName);
        Task<IList<TeamViewModel>> GetTeamByOwnerData(string userId);
        Task<IList<TeamViewModel>> GetTeamByUserData(string userId);
        Task<IList<TeamViewModel>> GetTeamMemberListData(string teamId);
        Task<TeamViewModel> GetTeamOwnerData(string teamId);
        Task<IList<IdNameViewModel>> GetTeamByGroupCodeData(string groupCode);
        Task<IList<IdNameViewModel>> GetTeamUsersByCodeData(string Code);
        Task<List<TeamViewModel>> GetTeamWithPortalIdsData();
        Task<List<TeamViewModel>> GetTeamsBasedOnAllowedPortalsData(string portalId);
        Task<List<TemplateCategoryViewModel>> GetCategoryListData(string tCode, string tcCode, string mCodes, string categoryIds, string templateIds, TemplateTypeEnum templateType, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, bool allBooks = false, string portalNames = null);
        Task<List<TemplateCategoryViewModel>> GetModuleBasedCategory(string tCode, string tcCode, string mCodes, string categoryIds, string templateIds, TemplateTypeEnum templateType, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, bool allBooks = false, string portalNames = null);
        Task<AssignmentViewModel> GetUserDetailsData(string user);
        Task<IList<UserViewModel>> GetUserListData();
        Task<IList<UserViewModel>> GetUserListForPortalData();
        Task<IList<UserViewModel>> GetActiveUserListData();
        Task<PagedList<UserViewModel>> GetActiveUserListForSwitchData(string filter, int pageSize, int pageNumber, string indexedItemId = null);
        Task<IList<UserViewModel>> GetActiveUserListForSwitchProfileData();
        Task<PagedList<UserViewModel>> GetActiveUsersListForSwitchProfileData(string filter, int pageSize, int pageNumber, string indexedItemId = null);
        Task<IList<UserViewModel>> GetActiveUserListForPortalData();
        Task<IList<UserViewModel>> GetUserTeamListData(string id);
        Task<IList<UserViewModel>> GetUserTeamListForPortalData();
        Task<IList<UserViewModel>> GetUserListForEmailSummaryData();
        Task<IList<UserViewModel>> GetSwitchToUserListData(string userId);
        Task<IdNameViewModel> GetPersonWithSponsorData(string userId);
        Task<IList<UserListOfValue>> ViewModelListData(string userId);
        Task<List<UserViewModel>> GetSwitchUserListData(string userId);
        Task<string> GetHierarchyRootIdData(string userHierarchyId, string hierarchy);
        Task<string> GetHierarchyRootIdData1(string userHierarchyId, string hierarchy);
        Task<string> GetHierarchyRootIdData2(string userHierarchyId, string hierarchy);
        Task<List<double>> GetUserNodeLevelData(string userId);
        Task<List<PositionChartViewModel>> GetUserHierarchyData(string parentId, int levelUpto, string hierarchyId, int level = 1, int option = 1);
        Task<string> GetPersonDateOfJoiningData(string userId);
        Task<List<LegalEntityViewModel>> GetEntityByIdsData(string legalEntity);
        Task<List<UserPermissionViewModel>> ViewUserPermissionsData(string userId);
        Task<IList<UserViewModel>> GetUserListWithEmailTextData();
        Task<List<UserViewModel>> GetAllUsersWithPortalIdData(string PortalId);
        Task<List<UserViewModel>> GetUsersWithPortalIdsData();
        Task<List<PortalViewModel>> GetAllowedPortalListData(string userId);
        Task<List<UserViewModel>> GetDMSPermiisionusersListData(string PortalId, string LegalEntityId);
        Task<string> GetUserHierarchyRootIdData(string userHierarchyId, string hierarchy);
        Task<IList<IdNameViewModel>> GetColunmMetadataListByPageData(string pageId);
        Task<IList<UserDataPermissionViewModel>> GetUserDataPermissionByPageIdData(string pageId, string portalId);
        Task<List<StepTaskEscalationDataViewModel>> AllEscalatedTasks(string portalids);
        #region Workboard

        Task<List<WorkBoardViewModel>> GetWorkboardList(WorkBoardstatusEnum status, string id = null);
        Task<List<WorkBoardViewModel>> GetWorkboardListData(WorkBoardstatusEnum status, string id = null);
        Task<List<WorkBoardViewModel>> GetWorkboardTaskList();
        Task<bool> UpdateWorkBoardStatus(string id, WorkBoardstatusEnum status);
        Task<WorkBoardSectionViewModel> GetWorkBoardSectionDetails(string sectionId);
        Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionListByWorkbBoardId(string workboardId);
        Task<WorkBoardItemViewModel> GetWorkBoardItemDetails(string itemId);
        Task<IList<WorkBoardItemViewModel>> GetWorkBoardItemBySectionId(string sectionId);
        Task<IList<WorkBoardItemViewModel>> GetWorkBoardItemByParentId(string parentId);
        Task<List<WorkBoardItemViewModel>> GetItemBySectionId(string sectionId);
        Task DeleteItem(string itemId);
        Task DeleteSection(string itemId);
        Task UpdateWorkBoardJson(WorkBoardViewModel data);
        Task UpdateWorkBoardSectionSequenceOrder(WorkBoardSectionViewModel data);

        Task UpdateWorkBoardItemDetails(WorkBoardItemViewModel data);
        Task UpdateWorkBoardItemSequenceOrder(WorkBoardItemViewModel data);
        Task UpdateWorkBoardItemSectionId(WorkBoardItemViewModel data);

        Task<List<WorkBoardInviteDetailsViewModel>> GetWorkBoardInvites(string workBoradId);
        Task<WorkBoardViewModel> GetWorkBoardDetails(string workBoradId);

        Task<WorkBoardViewModel> GetWorkBoardDetail(string workBoradId);
        Task<WorkBoardViewModel> GetWorkBoardDetailsByIdKey(string workBoardUniqueId, string shareKey);
        Task<List<WorkBoardTemplateViewModel>> GetTemplateList();
        Task<List<LOVViewModel>> GetTemplateCategoryList();
        Task<List<WorkBoardTemplateViewModel>> GetSearchResults(string[] values);
        Task<List<WorkBoardSectionViewModel>> GetJsonContent(string workBoardId, List<WorkBoardSectionViewModel> list);
        Task<List<WorkBoardItemViewModel>> GetJsonContentData(List<WorkBoardSectionViewModel> list);
        Task<List<WorkBoardItemViewModel>> GetWorkBoardSectionForIndex(WorkBoardItemViewModel item);
        Task<WorkBoardTemplateViewModel> GetWorkBoardTemplateById(string templateTypeId);
        Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForBasic();
        Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForBasicData();
        Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForBasicDataUpdate(string workBoardId, List<WorkBoardSectionViewModel> sections);

        Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForMonthly(int days);
        Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForMonthlyData(int days);

        Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForMonthlyDataUpdate(string workBoardId, List<WorkBoardSectionViewModel> sections, DateTime date);

        Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForWeekly();
        Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForWeeklyData();
        Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForWeeklyDataUpdate(string workBoardId, List<WorkBoardSectionViewModel> sections);

        Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForYearly();
        Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForYearlyData();

        Task<List<WorkBoardSectionViewModel>> GetWorkBoardSectionForYearlyDataUpdate(string workBoardId, List<WorkBoardSectionViewModel> sections);

        //        GetWorkBoardSectionForBasic
        //GetWorkBoardSectionForMonthly
        //GetWorkBoardSectionForWeekly
        //GetWorkBoardSectionForYearly
        Task<List<WorkBoardSectionViewModel>> GetWorkboardSectionList(string id);
        Task<WorkBoardItemViewModel> GetWorkBoardItemByNtsNoteId(string ntsNoteId);
        Task<WorkBoardItemViewModel> GetWorkboardItemById(string id);
        Task<WorkBoardItemViewModel> GetWorkboardItemByNoteId(string id);
        Task<bool> UpdateWorkboardItem(string workboardId, string sectionId, string workboardItemId);

        Task<IList<UserViewModel>> GetUserList(string noteId);
        Task<List<WorkBoardViewModel>> GetSharedWorkboardList(WorkBoardstatusEnum status, string sharedWithUserId);
        #endregion
        #region UserRoleUserBusiness

        Task<List<UserRoleUserViewModel>> GetUserRoleByUser(string userId);

        #endregion
        #region UserRoleStageParentBusiness

        Task<List<UserRoleStageParentViewModel>> GetUserRoleStageParentList();

        #endregion
        #region UserRolePortalBusiness

        Task<List<UserRolePortalViewModel>> GetPortalByUserRole(string userRoleId);
        Task<List<UserRoleViewModel>> GetUserRoleByPortal(string portalId);
        #endregion
        #region UserRoleDataPermissionBusiness

        Task<IList<IdNameViewModel>> GetColunmMetadataListByPageForUserRole(string pageId);
        Task<IList<UserRoleDataPermissionViewModel>> GetUserRoleDataPermissionByPageId(string pageId, string portalId);
        #endregion


        #region UserRoleBusiness
        Task<List<UserViewModel>> GetUserRoleForUser(string id);
        Task<List<UserRoleViewModel>> GetUserRolesWithPortalIds();

        #endregion
        #region UserPortalBusiness

        Task<List<UserPortalViewModel>> GetPortalByUser(string userId);
        Task<List<UserViewModel>> GetUserByPortal(string portalId);

        #endregion
        #region UserHierarchyPermissionBusiness
        Task<List<UserHierarchyPermissionViewModel>> GetUserPermissionHierarchy(string userId);
        Task<List<UserHierarchyPermissionViewModel>> GetUserPermissionHierarchyForPortal(string userId);
        Task<List<IdNameViewModel>> GetUserNotInPermissionHierarchy(string UserId);

        #endregion
        #region UserRoleHierarchyPermissionBusiness
        Task<List<UserRoleHierarchyPermissionViewModel>> GetUserRolePermissionHierarchy(string userRoleId);
        Task<List<UserRoleHierarchyPermissionViewModel>> GetUserRolePermissionHierarchyForPortal(string userRoleId);
        Task<List<IdNameViewModel>> GetUserRoleNotInPermissionHierarchy(string UserRoleId);

        #endregion
        #region UserHierarchyBusiness
        Task<IList<UserHierarchyViewModel>> GetHierarchyList(string HierarchyId, string userId);
        Task<IList<UserHierarchyViewModel>> GetHierarchyListForAllPortals(string HierarchyId);
        Task<IList<UserHierarchyViewModel>> GetHierarchyListForPortal(string HierarchyId, string userId);
        Task<UserHierarchyViewModel> GetHierarchyUser(string HierarchyId, string userId);
        Task<List<UserViewModel>> GetHierarchyUsers(string hierarchyCode, string parentUserId, int level, int option);
        Task<List<UserHierarchyViewModel>> GetPerformanceHierarchyUsers(string parentUserId);
        Task<List<UserHierarchyViewModel>> GetUserHierarchyByCode(string code, string parentUserId);
        Task<List<IdNameViewModel>> GetNonExistingUser(string hierarchyId, string userId);
        Task<List<IdNameViewModel>> GetHierarchyData(HierarchyTypeEnum userId);
        Task<List<UserViewModel>> GetAllCSCUsersList();
        Task<List<PositionChartViewModel>> GetUserHierarchyChartData(string parentId, int levelUpto, string hierarchyId, int level = 1, int option = 1);
        Task<List<IdNameViewModel>> GetStepTaskTemplateList(string serviceTemplateId);
        #endregion


        #region UserGroupBusiness
        Task<List<TagCategoryViewModel>> GetAllTagCategory();
        Task<TagCategoryViewModel> GetTagCategoryDetails(string Id);
        Task<List<IdNameViewModel>> GetAllSourceID();
        Task DeleteTagCategory(string Id);
        Task DeleteTag(string Id);
        Task<TagCategoryViewModel> IsTagCategoryNameExist(string TagCategoryName, string Id);
        Task<TagCategoryViewModel> IsTagCategoryCodeExist(string TagCategoryCode, string Id);
        Task<TagCategoryViewModel> IsParentAssignTosourceTagExist(string ParentId, string TagSourceId, string Id);
        Task<List<IdNameViewModel>> GetParentTagCategory();
        Task<List<TagViewModel>> GetTagList(string CategoryId);
        Task<TagViewModel> GetTagEdit(string NoteId);
        Task<TagViewModel> GetNoteId(string Id);
        Task<TagViewModel> IsTagNameExist(string Parentid, string TagName, string Id);
        Task<List<UserGroupViewModel>> GetTeamWithPortalIds();

        #endregion

        Task<List<TemplateViewModel>> GetMasterList(string groupCode);
        Task<List<TemplateViewModel>> GetTemplatesList(string portalId, TemplateTypeEnum templateType);
        Task<List<IdNameViewModel>> GetCSCOfficeType(string templateCode);
        Task<List<IdNameViewModel>> GetCSCSubfficeType(string officeId, string districtId);
        Task<List<IdNameViewModel>> GetRevenueVillage(string officeId, string subDistrictId);
        Task UpdateMarriageCerfificateFile(string referenceId, string fileId);
        Task<List<DynamicGridViewModel>> GetDataGridValue(string parentId);

        Task<List<TeamViewModel>> GetTeamWithUsersByPortalId(string portalId, string teamIds);
        Task<PageViewModel> GetUserLandingPage(string userId, string portalId);
        Task<IList<UserEntityPermissionViewModel>> GetUserPermissionList(string userId, string userRole, string permission);
        Task<PageViewModel> GetUserRoleLandingPage(string userId, string portalId);
        Task<List<StepTaskEscalationViewModel>> GetStepTaskEscalation(string stepTaskComponentId);
        Task<List<PositionChartIndexViewModel>> GetCMDBHierarchyData(string parentId, int levelUpto);
        Task<List<StepTaskEscalationViewModel>> GetTaskListWithEscalation();
        Task<List<StepTaskEscalationDataViewModel>> GetPortalTaskListWithEscalationData(string portal, string escalatUser);
        Task<List<StepTaskEscalationDataViewModel>> MyTasksEscalatedDataList(string portal, string assigneeUser);
        Task<List<NtsEmailViewModel>> GetNtsEmailList(string serTempCodes, string statusCodes, string userId, string portalNames, string templateCodes = null, string catCodes = null, string groupCodes = null, string categoryId = null, string templateId = null, string serviceId = null, string departmetId = null, DateTime? fromDate = null, DateTime? toDate = null);
        //Task<List<NtsEmailViewModel>> GetNtsEmailList(string serviceId, string targetId, string targetType);
        //Task<List<NtsEmailViewModel>> GetNtsEmailList(string serTempCodes, string statusCodes, string userId, string portalNames, string templateCodes = null, string catCodes = null, string groupCodes = null, string categoryId = null, string templateId = null);
        Task<List<PropertyViewModel>> GetPropertyData(string userId);
        Task<List<StepTaskEscalationViewModel>> GetOverDueTaskListForEscalation();
        Task<List<StepTaskEscalationDataViewModel>> EscalatedTaskDataByTaskId(string taskId);
        Task<IList<IdNameViewModel>> GetTriggeringStepTaskComponentList(string templateId);
        Task<FormTemplateViewModel> GetSelectQueryData(TableMetadataViewModel tableMetaData);
        Task<List<RuntimeWorkflowDataViewModel>> GetRuntimeWorkflowDataList(string runtimeWorkflowDataId);
        Task<List<UserRolePreferenceViewModel>> GetUserRolePreferences(string userRoleId);
        Task<List<UserPreferenceViewModel>> GetUserPreferences(string userId);
        Task<List<LegalEntityViewModel>> GetLegalEntityByLocationData();
        Task<string> GetForeignKeyId(ColumnMetadataViewModel col, string val);
    }
}
