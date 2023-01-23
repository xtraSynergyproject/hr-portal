using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
////using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{

    public interface ICmsBusiness : IBusinessBase<TemplateViewModel, Template>
    {
        Task ManageTable(TableMetadataViewModel tableMetadata);
        Task<TestViewModel> Test();
        Task<DataTable> GetFormIndexPageGridData(string indexPageTemplateId, DataSourceRequest request);
        Task<FormIndexPageTemplateViewModel> GetFormIndexPageViewModel(PageViewModel page);

        Task<DataRow> GetDataById(TemplateTypeEnum viewName, PageViewModel page, string recordId, bool isLog = false, string logId = null);
        Task<CommandResult<FormTemplateViewModel>> ManageForm(FormTemplateViewModel model);
        Task<DataTable> GetData(string schemaName, string tableName, string columns = null, string filter = null, string orderbyColumns = null, OrderByEnum orderby = OrderByEnum.Ascending, string where = null, bool ignoreJoins = false, string returnColumns = null, int? limit = null, int? skip = null, bool enableLocalization = false, string lang = null);
        Task<IList<TreeViewViewModel>> GetInboxTreeviewList(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string batchId, string expandingList, string userRoleCodes, string inboxCode);
        Task<IList<IdNameViewModel>> GetActivePersonList();
        Task<IList<IdNameViewModel>> GetPayrollElementList();
        void GetFormUdfDetails(FormTemplateViewModel model);
        Task<FormTemplateViewModel> GetFormDetails(FormTemplateViewModel viewModel);
        Task<DataTable> GetDataListByTemplate(string templateCode, string templateId, string where = null);
        Task<IList<EmailTaskViewModel>> ReadEmailTaskData(string refId, ReferenceTypeEnum refType);
        Task<Tuple<bool, string>> CreateForm(string data, string pageId, string templateCode = null);
        Task<Tuple<bool, string>> EditForm(string recordId, string data, string pageId, string templateCode = null);
        Task<IList<TreeViewViewModel>> GetInboxMenuItem(string id, string type, string templateCode);
        Task<List<TaskTemplateViewModel>> ReadInboxData(string id, string type, string templateCode, string userId = null);
        Task<IList<TASTreeViewViewModel>> GetInboxMenuItem(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string projectId, string expandingList, string userroleCode);

        Task<IList<TASTreeViewViewModel>> GetInboxMenuItemByUser(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string projectId, string expandingList, string userroleCode);
        Task<string> GetLatestMigrationScript();
        Task<string> ExecuteMigrationScript(string script);
        Task<List<IdNameViewModel>> GetCSCOfficeType(string templateCode);
        Task<List<IdNameViewModel>> GetCSCSubfficeType(string officeId, string districtId);
        Task<List<IdNameViewModel>> GetRevenueVillage(string officeId, string subDistrictId);
        Task<List<DynamicGridViewModel>> GetDataGridValue(string parentId);
        Task<List<string>> GetAllMigrationsList();
        Task<List<TemplateViewModel>> GetTemplateListByTemplateType(int i);

        Task<List<TemplateViewModel>> GetStepTaskTemplateListData(string serviceTemplateId);
        Task<List<IdNameViewModel>> GetStepTaskTemplateList(string serviceTemplateId);

        // Copy Template Methods
        Task<CommandResult<ServiceTemplateViewModel>> CopyServiceTemplate(ServiceTemplateViewModel oldModel, string newTempId, CopyTemplateViewModel copyModel = null);
        Task<CommandResult<ServiceIndexPageTemplateViewModel>> CopyServiceTempltaeIndexPageData(ServiceIndexPageTemplateViewModel model, string newTempId, CopyTemplateViewModel copyModel = null);
        Task<bool> CopyLanguage(List<ResourceLanguageViewModel> items, string newTemplateId);
        Task<CommandResult<NoteTemplateViewModel>> CopyNoteTemplate(NoteTemplateViewModel oldModel, string newTempId);
        Task<CommandResult<NoteIndexPageTemplateViewModel>> CopyNoteTempltaeIndexPageData(NoteIndexPageTemplateViewModel model, string newTempId);
        Task<CommandResult<FormTemplateViewModel>> CopyFormTemplate(FormTemplateViewModel oldModel, string newTempId);
        Task<CommandResult<FormIndexPageTemplateViewModel>> CopyFormTemplateIndexPageData(FormIndexPageTemplateViewModel model, string newTempId);
        Task<CommandResult<TaskTemplateViewModel>> CopyTaskTemplate(TaskTemplateViewModel oldModel, string newTempId);
        Task<CommandResult<TaskIndexPageTemplateViewModel>> CopyTaskTempltaeIndexPageData(TaskIndexPageTemplateViewModel model, string newTempId);
        Task<CommandResult<PageTemplateViewModel>> CopyPageTemplate(PageTemplateViewModel oldModel, string newTempId);

        Task<List<dynamic>> CopyProcessDesign(List<ProcessDesignViewModel> list, string newTemplateId);
        Task<List<dynamic>> CopyComponents(string oldPdId, string newPdId, List<ComponentViewModel> oldList);
        Task<List<dynamic>> CreateStepTask(List<TemplateViewModel> oldStepTaskList, string jsonStr);
        Task<List<dynamic>> CopyStepTaskComponent(List<dynamic> compList, List<dynamic> stepTaskIdList, string newServiceId, string oldServiceId, List<StepTaskComponentViewModel> oldStepTaskCompList, CopyTemplateViewModel copyModel = null);
        Task<List<DecisionScriptComponentViewModel>> GetOldDecisionScriptComponentData(List<ComponentViewModel> ComponentList);
        Task<List<dynamic>> CopyStepTaskLogic(List<StepTaskSkipLogicViewModel> oldStepTaskSkipLogic, List<dynamic> StepTaskCompIdList);
        Task<List<BusinessRuleModelViewModel>> GetOldBusinessRuleModelDataForStepTaskSkipLogic(List<StepTaskSkipLogicViewModel> oldStepTaskSkipLogic);
        Task<bool> CopyBusinessRuleModelForLogics(List<BusinessRuleModelViewModel> oldBRModelList, List<dynamic> skipLogicIds);
        Task<List<StepTaskSkipLogicViewModel>> GetOldStepTaskSkipLogics(List<StepTaskComponentViewModel> StepTaskCompIdList);
        Task<List<BusinessRuleModelViewModel>> GetOldBusinessRuleModelDataForStepTaskAssigneeLogic(List<StepTaskAssigneeLogicViewModel> oldStepTaskAssigneeLogic);
        Task<List<dynamic>> CopyStepTaskAssignee(List<StepTaskAssigneeLogicViewModel> oldStepTaskAssigneeLogic, List<dynamic> StepTaskCompIdList);
        Task<List<StepTaskAssigneeLogicViewModel>> GetOldStepTaskAssigneeLogics(List<StepTaskComponentViewModel> StepTaskCompList);
        // Business Logic copy
        Task<List<BusinessRuleViewModel>> GetOldBusinessRuleList(string oldServiceId);
        Task<List<BusinessRuleNodeViewModel>> GetOldBusinessRuleNodeList(List<BusinessRuleViewModel> oldBusinessRules);
        Task<List<BreResultViewModel>> GetOldBreResultList(List<BusinessRuleNodeViewModel> nodes);
        Task<List<BusinessRuleConnectorViewModel>> GetOldBRConnectorList(List<BusinessRuleViewModel> businessRuleList);
        Task<List<dynamic>> CopyBusinessRules(List<BusinessRuleViewModel> oldList, string newServiceId);
        Task<List<dynamic>> CopyBusinessRuleNodes(List<dynamic> BRIds, List<BusinessRuleNodeViewModel> oldNodesList);
        Task<List<dynamic>> CopyBreResults(List<dynamic> nodeIds, List<BreResultViewModel> oldList);
        Task<bool> CopyBusinessRuleConnector(List<dynamic> BRIds, List<dynamic> nodeIds, List<BusinessRuleConnectorViewModel> oldList);
        Task<List<dynamic>> CopyDecisionScriptComponent(List<dynamic> ComponentList, List<DecisionScriptComponentViewModel> oldList);

        Task<List<ComponentViewModel>> GetOldComponentsList(ProcessDesignViewModel PdList);
        Task UpdateUdfPermission(List<UdfPermissionViewModel> oldUdfList, List<ColumnMetadataViewModel> columnMetadataList, string newStepTaskTempId, List<ColumnMetadataViewModel> oldColumnMetadataList);
        Task<List<StepTaskComponentViewModel>> GetOldStepTaskComponents(List<ComponentViewModel> compList);

        Task<List<BusinessRuleModelViewModel>> GetOldBusinessRuleModelForDecisionScript(List<DecisionScriptComponentViewModel> decisionScriptCompIds);
        Task<bool> CopyBusinessRuleModelForDecisionScript(List<BusinessRuleModelViewModel> oldList, List<dynamic> decisionScriptCompIds);
        Task<List<BusinessRuleModelViewModel>> GetOldBRMListforBusinessDecision(List<BusinessRuleNodeViewModel> nodes);
        Task<bool> CopyStepTaskScripts(List<dynamic> stepTaskIds, List<TemplateViewModel> oldStepTaskList);


        Task<Dictionary<string, dynamic>> GetTemplateCompleteDataById(string id);

        Task<bool> CopyDevTemplates(CopyTemplateViewModel model);
        //----------------------------------------------------------------------------
        Task<bool> CopyForm(string oldTempId, string newTemplateId, bool devImport = false, CopyTemplateViewModel copyModel = null);
        Task<bool> CopyPage(string oldTempId, string newTemplateId, bool devImport = false, CopyTemplateViewModel copyModel = null);
        Task<bool> CopyNote(string oldTempId, string newTemplateId, bool devImport = false, CopyTemplateViewModel copyModel = null);
        Task<bool> CopyTask(string oldTempId, string newTemplateId);
        Task<bool> CopyService(string oldTempId, string newTemplateId, TemplateViewModel model);
        Task<bool> CopyCustomTemplate(string oldTempId, string newTemplateId, CopyTemplateViewModel copyModel = null);
        Task<List<PropertyViewModel>> GetPropertyData(string userId);
    }
}
