using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
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
