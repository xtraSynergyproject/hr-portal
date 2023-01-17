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
    public class UserRoleUserBusiness : BusinessBase<UserRoleUserViewModel, UserRoleUser>, IUserRoleUserBusiness
    {
        private readonly IRepositoryQueryBase<UserRoleUserViewModel> _queryRepo;

        public UserRoleUserBusiness(IRepositoryBase<UserRoleUserViewModel, UserRoleUser> repo, IMapper autoMapper,
            IRepositoryQueryBase<UserRoleUserViewModel> queryRepo): base(repo, autoMapper)
        {
            _queryRepo = queryRepo;

        }

        public async override Task<CommandResult<UserRoleUserViewModel>> Create(UserRoleUserViewModel model)
        {
           
           // var data = _autoMapper.Map<UserRoleViewModel>(model);
           
            var result = await base.Create(model);           
            return CommandResult<UserRoleUserViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<UserRoleUserViewModel>> Edit(UserRoleUserViewModel model)
        {  
            var result = await base.Edit(model);           
            return CommandResult<UserRoleUserViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<List<UserRoleUserViewModel>> GetUserRoleByUser(string userId)
        {
            var query = @$"SELECT ur.""Id"", ur.""UserRoleId"" as UserRoleId FROM public.""UserRoleUser"" as ur where ur.""UserId"" = '{userId}' and ur.""IsDeleted"" = false and ur.""CompanyId""='{_repo.UserContext.CompanyId}'";
            var queryData = await _queryRepo.ExecuteQueryList<UserRoleUserViewModel>(query, null);
            return queryData;
        }



    }
}
