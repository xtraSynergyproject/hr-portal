using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IHiringManagerBusiness : IBusinessBase<HiringManagerViewModel, HiringManager>
    {


        Task<List<HiringManagerViewModel>> GetUserIdList();
        Task<List<HiringManagerViewModel>> GetHMListByOrgId(string orgId);
        Task<IList<HiringManagerViewModel>> GetHiringManagersList();
        Task<List<HiringManagerViewModel>> GetHODListByOrgId(string orgId);
        Task<List<IdNameViewModel>> GetHmOrg(string userId);
        Task<List<IdNameViewModel>> GetHODOrg(string userId);
    }
}
