using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public class UserGroupUserBusiness : BusinessBase<UserGroupUserViewModel, UserGroupUser>, IUserGroupUserBusiness
    {
        public UserGroupUserBusiness(IRepositoryBase<UserGroupUserViewModel, UserGroupUser> repo, IMapper autoMapper) : base(repo, autoMapper)
        {


        }

        public async override Task<CommandResult<UserGroupUserViewModel>> Create(UserGroupUserViewModel model)
        {

            // var data = _autoMapper.Map<UserRoleViewModel>(model);

            var result = await base.Create(model);
            return CommandResult<UserGroupUserViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<UserGroupUserViewModel>> Edit(UserGroupUserViewModel model)
        {
            var result = await base.Edit(model);
            return CommandResult<UserGroupUserViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
    }
}
