using AutoMapper;
using CMS.Business.Interface;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public class UserPagePreferenceBusiness : BusinessBase<UserPagePreferenceViewModel, UserPagePreference>, IUserPagePreferenceBusiness
    {
        public UserPagePreferenceBusiness(IRepositoryBase<UserPagePreferenceViewModel, UserPagePreference> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }
    }
}
