using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface ISubModuleBusiness : IBusinessBase<SubModuleViewModel,SubModule>
    {
        Task<IList<SubModuleViewModel>> GetSubModuleList();
        Task<IList<SubModuleViewModel>> GetPortalSubModuleList();
    }
}
