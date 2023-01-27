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
    public class UserPreferenceBusiness : BusinessBase<UserPreferenceViewModel, UserPreference>, IUserPreferenceBusiness
    {
        public UserPreferenceBusiness(IRepositoryBase<UserPreferenceViewModel, UserPreference> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }
    }
}
