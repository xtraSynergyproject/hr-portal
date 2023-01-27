using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class UserRolePortalBusiness : BusinessBase<UserRolePortalViewModel, UserRolePortal>, IUserRolePortalBusiness
    {
        private readonly IRepositoryQueryBase<UserRolePortalViewModel> _queryRepo;

        public UserRolePortalBusiness(IRepositoryBase<UserRolePortalViewModel, UserRolePortal> repo, IMapper autoMapper,
            IRepositoryQueryBase<UserRolePortalViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<UserRolePortalViewModel>> Create(UserRolePortalViewModel model)
        {
            var data = _autoMapper.Map<UserRolePortalViewModel>(model);
            var result = await base.Create(data);

            return CommandResult<UserRolePortalViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<UserRolePortalViewModel>> Edit(UserRolePortalViewModel model)
        {
            var data = _autoMapper.Map<UserRolePortalViewModel>(model);
            var result = await base.Edit(data);

            return CommandResult<UserRolePortalViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<List<UserRolePortalViewModel>> GetPortalByUserRole(string userRoleId)
        {
            var query = @$"SELECT p.""Id"", p.""PortalId"" as PortalId FROM public.""UserRolePortal"" as p where p.""UserRoleId"" = '{userRoleId}' and p.""IsDeleted"" = false and p.""CompanyId""='{_repo.UserContext.CompanyId}'";
            var queryData = await _queryRepo.ExecuteQueryList<UserRolePortalViewModel>(query, null);
            return queryData;
        }
        public async Task<List<UserRoleViewModel>> GetUserRoleByPortal(string portalId)
        {
            var query = @$"SELECT u.*, p.""PortalId"" as PortalId
FROM public.""UserRolePortal"" as p
join public.""UserRole"" as u  on u.""Id""=p.""UserRoleId"" and  u.""IsDeleted"" = false and u.""CompanyId""='{_repo.UserContext.CompanyId}'  where p.""PortalId"" = '{portalId}' and p.""IsDeleted"" = false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
";
            var queryData = await _queryRepo.ExecuteQueryList<UserRoleViewModel>(query, null);
            return queryData;
        }
    }
}
