using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{

    public interface ICmsTaskBusiness : IBusinessBase<TemplateViewModel, Template>
    {
        Task ManageTable(string tableMetadataId);
        Task<DataTable> GetIndexPageData(string indexPageTemplateId, NtsTaskOwnerTypeEnum ownerType, DataSourceRequest request);
        Task<TaskIndexPageTemplateViewModel> GetIndexPageViewModel(PageViewModel page);
        Task<string> GetDataById(TemplateTypeEnum viewName, PageViewModel page, string recordId);
        Task<CommandResult<TaskTemplateViewModel>> Manage(TaskTemplateViewModel model);

    }
}
