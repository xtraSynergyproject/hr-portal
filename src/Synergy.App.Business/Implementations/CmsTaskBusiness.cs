using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using Kendo.Mvc.UI;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class CmsTaskBusiness : BusinessBase<TemplateViewModel, Template>, ICmsTaskBusiness
    {
        public CmsTaskBusiness(IRepositoryBase<TemplateViewModel, Template> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public Task<string> GetDataById(TemplateTypeEnum viewName, PageViewModel page, string recordId)
        {
            throw new NotImplementedException();
        }

        public Task<DataTable> GetIndexPageData(string indexPageTemplateId, NtsTaskOwnerTypeEnum ownerType, DataSourceRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<TaskIndexPageTemplateViewModel> GetIndexPageViewModel(PageViewModel page)
        {
            throw new NotImplementedException();
        }

        public Task<CommandResult<TaskTemplateViewModel>> Manage(TaskTemplateViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task ManageTable(string tableMetadataId)
        {
            throw new NotImplementedException();
        }
    }
}
