using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IUserDataPermissionBusiness : IBusinessBase<UserDataPermissionViewModel, UserDataPermission>
    {
        Task<IList<IdNameViewModel>> GetColunmMetadataListByPage(string pageId);
        Task<IList<UserDataPermissionViewModel>> GetUserDataPermissionByPageId(string pageId, string portalId);
    }
}
