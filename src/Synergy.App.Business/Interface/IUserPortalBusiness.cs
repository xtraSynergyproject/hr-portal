using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IUserPortalBusiness : IBusinessBase<UserPortalViewModel, UserPortal>
    {
        Task<List<UserPortalViewModel>> GetPortalByUser(string userId);
        Task<List<UserViewModel>> GetUserByPortal(string portalId);
    }
}
