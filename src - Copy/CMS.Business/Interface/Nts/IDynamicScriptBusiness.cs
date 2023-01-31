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
    public interface IDynamicScriptBusiness
    {
        Task<CommandResult<T>> ExecuteScript<T>(string script, T viewModel, TemplateTypeEnum templateType, dynamic inputData, IUserContext uc, IServiceProvider sp);

    }
}
