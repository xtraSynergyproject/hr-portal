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

    public interface ICmsBusiness : IBusinessBase<TemplateViewModel, Template>
    {
        Task ManageTable(TableMetadataViewModel tableMetadata);
        Task<TestViewModel> Test();
        Task<DataTable> GetFormIndexPageGridData(string indexPageTemplateId, DataSourceRequest request);
        Task<FormIndexPageTemplateViewModel> GetFormIndexPageViewModel(PageViewModel page);

        Task<DataRow> GetDataById(TemplateTypeEnum viewName, PageViewModel page, string recordId, bool isLog = false, string logId = null);
        Task<CommandResult<FormTemplateViewModel>> ManageForm(FormTemplateViewModel model);
        Task<DataTable> GetData(string schemaName, string tableName, string columns = null, string filter = null, string orderbyColumns = null, OrderByEnum orderby = OrderByEnum.Ascending, string where = null);
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
        Task<List<TaskTemplateViewModel>> ReadInboxData(string id, string type, string templateCode,string userId=null);
        Task<IList<TASTreeViewViewModel>> GetInboxMenuItem(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string projectId, string expandingList, string userroleCode);

        Task<IList<TASTreeViewViewModel>> GetInboxMenuItemByUser(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string projectId, string expandingList, string userroleCode);
        Task<string> GetLatestMigrationScript();
        Task<string> ExecuteMigrationScript(string script);
    }
}
