using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IUserDataPermissionBusiness : IBusinessBase<UserDataPermissionViewModel, UserDataPermission>
    {
        Task<IList<IdNameViewModel>> GetColunmMetadataListByPage(string pageId);
        Task<IList<UserDataPermissionViewModel>> GetUserDataPermissionByPageId(string pageId, string portalId);
    }
}
