using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class UserRoleUserBusiness : BusinessBase<UserRoleUserViewModel, UserRoleUser>, IUserRoleUserBusiness
    {
        private readonly IRepositoryQueryBase<UserRoleUserViewModel> _queryRepo;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;

        public UserRoleUserBusiness(IRepositoryBase<UserRoleUserViewModel, UserRoleUser> repo, IMapper autoMapper, ICmsQueryBusiness cmsQueryBusiness,
            IRepositoryQueryBase<UserRoleUserViewModel> queryRepo): base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _cmsQueryBusiness = cmsQueryBusiness;
        }

        public async override Task<CommandResult<UserRoleUserViewModel>> Create(UserRoleUserViewModel model, bool autoCommit = true)
        {
           
           // var data = _autoMapper.Map<UserRoleViewModel>(model);
           
            var result = await base.Create(model,autoCommit);           
            return CommandResult<UserRoleUserViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<UserRoleUserViewModel>> Edit(UserRoleUserViewModel model, bool autoCommit = true)
        {  
            var result = await base.Edit(model,autoCommit);           
            return CommandResult<UserRoleUserViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<List<UserRoleUserViewModel>> GetUserRoleByUser(string userId)
        {
            var queryData = await _cmsQueryBusiness.GetUserRoleByUser(userId);
            return queryData;
        }



    }
}
