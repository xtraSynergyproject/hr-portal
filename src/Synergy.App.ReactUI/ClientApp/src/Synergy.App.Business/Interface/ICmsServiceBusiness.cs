﻿using Synergy.App.Common;
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

    public interface ICmsServiceBusiness : IBusinessBase<TemplateViewModel, Template>
    {
        Task ManageTable(string tableMetadataId);
        Task<DataTable> GetIndexPageData(string indexPageTemplateId, NtsServiceOwnerTypeEnum ownerType, DataSourceRequest request);
        Task<ServiceIndexPageTemplateViewModel> GetIndexPageViewModel(PageViewModel page);
        Task<string> GetDataById(TemplateTypeEnum viewName, PageViewModel page, string recordId);
        Task<CommandResult<ServiceTemplateViewModel>> Manage(ServiceTemplateViewModel model);

    }
}
