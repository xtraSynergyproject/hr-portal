using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface ISubModuleBusiness : IBusinessBase<SubModuleViewModel,SubModule>
    {
        Task<IList<SubModuleViewModel>> GetSubModuleList();
        Task<IList<SubModuleViewModel>> GetPortalSubModuleList();
    }
}
