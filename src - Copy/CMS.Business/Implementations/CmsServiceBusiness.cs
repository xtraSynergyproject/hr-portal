using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using Kendo.Mvc.UI;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class CmsServiceBusiness : BusinessBase<TemplateViewModel, Template>, ICmsServiceBusiness
    {
        public CmsServiceBusiness(IRepositoryBase<TemplateViewModel, Template> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public Task<string> GetDataById(TemplateTypeEnum viewName, PageViewModel page, string recordId)
        {
            throw new NotImplementedException();
        }

        public Task<DataTable> GetIndexPageData(string indexPageTemplateId, NtsServiceOwnerTypeEnum ownerType, DataSourceRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceIndexPageTemplateViewModel> GetIndexPageViewModel(PageViewModel page)
        {
            throw new NotImplementedException();
        }

        public Task<CommandResult<ServiceTemplateViewModel>> Manage(ServiceTemplateViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task ManageTable(string tableMetadataId)
        {
            throw new NotImplementedException();
        }
    }
}
