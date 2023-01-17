using AutoMapper;
using Synergy.App.Business.Interface;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public class UserRolePreferenceBusiness : BusinessBase<UserRolePreferenceViewModel, UserRolePreference>, IUserRolePreferenceBusiness
    {
        public UserRolePreferenceBusiness(IRepositoryBase<UserRolePreferenceViewModel, UserRolePreference> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }
    }
}
