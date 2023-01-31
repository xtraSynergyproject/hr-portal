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
    public class UserPagePreferenceBusiness : BusinessBase<UserPagePreferenceViewModel, UserPagePreference>, IUserPagePreferenceBusiness
    {
        public UserPagePreferenceBusiness(IRepositoryBase<UserPagePreferenceViewModel, UserPagePreference> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }
    }
}
