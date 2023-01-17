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
    public class UserPortalBusiness : BusinessBase<UserPortalViewModel, UserPortal>, IUserPortalBusiness
    {
        private readonly IRepositoryQueryBase<UserPortalViewModel> _queryRepo;
        private readonly IUserContext _userContext;

        public UserPortalBusiness(IRepositoryBase<UserPortalViewModel, UserPortal> repo, IMapper autoMapper,
            IRepositoryQueryBase<UserPortalViewModel> queryRepo, IUserContext userContext) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _userContext = userContext;
        }

        public async override Task<CommandResult<UserPortalViewModel>> Create(UserPortalViewModel model)
        {
            var data = _autoMapper.Map<UserPortalViewModel>(model);
            var result = await base.Create(data);

            return CommandResult<UserPortalViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<UserPortalViewModel>> Edit(UserPortalViewModel model)
        {
            var data = _autoMapper.Map<UserPortalViewModel>(model);
            var result = await base.Edit(data);

            return CommandResult<UserPortalViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<List<UserPortalViewModel>> GetPortalByUser(string userId)
        {
            var query = @$"SELECT p.""Id"", p.""PortalId"" as PortalId FROM public.""UserPortal"" as p where p.""UserId"" = '{userId}' and p.""IsDeleted"" = false and p.""CompanyId""='{_userContext.CompanyId}'";
            var queryData = await _queryRepo.ExecuteQueryList<UserPortalViewModel>(query, null);
            return queryData;
        }
        public async Task<List<UserViewModel>> GetUserByPortal(string portalId)
        {
            var query = @$"SELECT u.*, p.""PortalId"" as PortalId
FROM public.""UserPortal"" as p 
join public.""User"" as u  on u.""Id""=p.""UserId"" and  u.""IsDeleted"" = false and u.""CompanyId""='{_userContext.CompanyId}' where p.""PortalId"" = '{portalId}' and p.""IsDeleted"" = false and p.""CompanyId""='{_userContext.CompanyId}'
";
            var queryData = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return queryData;
        }

    }
}
