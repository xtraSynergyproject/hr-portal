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
    public class UserHierarchyPermissionBusiness : BusinessBase<UserHierarchyPermissionViewModel, UserHierarchyPermission>, IUserHierarchyPermissionBusiness
    {
        private readonly IRepositoryQueryBase<UserHierarchyPermissionViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo1;
        private IUserContext _userContext;

        public UserHierarchyPermissionBusiness(IRepositoryBase<UserHierarchyPermissionViewModel, UserHierarchyPermission> repo, IMapper autoMapper,
            IRepositoryQueryBase<UserHierarchyPermissionViewModel> queryRepo,
            IRepositoryQueryBase<IdNameViewModel> queryRepo1, IUserContext userContext) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _queryRepo1 = queryRepo1;
            _userContext = userContext;
        }

        public async override Task<CommandResult<UserHierarchyPermissionViewModel>> Create(UserHierarchyPermissionViewModel model)
        {
            var data = _autoMapper.Map<UserHierarchyPermissionViewModel>(model);
            var result = await base.Create(data);

            return CommandResult<UserHierarchyPermissionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<UserHierarchyPermissionViewModel>> Edit(UserHierarchyPermissionViewModel model)
        {
            var data = _autoMapper.Map<UserHierarchyPermissionViewModel>(model);
            var result = await base.Edit(data);

            return CommandResult<UserHierarchyPermissionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<List<UserHierarchyPermissionViewModel>> GetUserPermissionHierarchy(string userId)
        {
            var query = @$"SELECT up.*,h.""Name"" as HierarchyName
                   FROM public.""UserHierarchyPermission"" as up 
                
                   left join public.""HierarchyMaster"" as h on h.""Id""= up.""HierarchyId""
                   where up.""UserId"" = '{userId}' and up.""IsDeleted"" = false";
            var queryData = await _queryRepo.ExecuteQueryList<UserHierarchyPermissionViewModel>(query, null);
            return queryData;
        }
        public async Task<List<UserHierarchyPermissionViewModel>> GetUserPermissionHierarchyForPortal(string userId)
        {
            var query = @$"SELECT up.*,h.""Name"" as HierarchyName
                   FROM public.""UserHierarchyPermission"" as up 
                
                   left join public.""HierarchyMaster"" as h on h.""Id""= up.""HierarchyId"" and up.""CompanyId"" = '{_repo.UserContext.CompanyId}' and h.""CompanyId"" = '{_repo.UserContext.CompanyId}' and h.""IsDeleted""=false
                   where up.""UserId"" = '{userId}' and up.""IsDeleted"" = false and h.""PortalId"" = '{_repo.UserContext.PortalId}' and h.""LegalEntityId"" = '{_repo.UserContext.LegalEntityId}'";
            var queryData = await _queryRepo.ExecuteQueryList<UserHierarchyPermissionViewModel>(query, null);
            return queryData;
        }
        public async Task<List<IdNameViewModel>> GetUserNotInPermissionHierarchy(string UserId)
        {
            var query = @$"SELECT distinct u.*
                   FROM public.""User"" as u
                   join public.""UserPortal"" as up on up.""UserId""=u.""Id"" and up.""IsDeleted""=false
                   left join public.""UserHierarchyPermission"" as uhp on uhp.""UserId""=u.""Id"" and uhp.""IsDeleted""=false
                   left join public.""HierarchyMaster"" as h on h.""Id""= uhp.""HierarchyId"" and h.""Code""='BUSINESS_HIERARCHY' and h.""IsDeleted""=false
                   where u.""IsDeleted"" = false and up.""PortalId""='{_userContext.PortalId}'  #WHERE# ";
            var where = "";
            if (UserId.IsNotNullAndNotEmpty()) {
                where = $@" and u.""Id"" NOT IN (select distinct uhp.""UserId"" from public.""UserHierarchyPermission"" as uhp

                    join public.""UserPortal"" as up on up.""UserId""=uhp.""UserId"" and up.""IsDeleted""=false	 WHERE uhp.""UserId"" IS NOT NULL and uhp.""UserId""<>'{UserId}' 
                        and uhp.""IsDeleted""=false and up.""PortalId""='{_userContext.PortalId}')";
            }
            else
            {
             where =$@" and u.""Id"" NOT IN (select distinct uhp.""UserId"" from public.""UserHierarchyPermission"" as uhp

                    join public.""UserPortal"" as up on up.""UserId""=uhp.""UserId"" and up.""IsDeleted""=false																										 
				WHERE uhp.""UserId"" IS NOT NULL and uhp.""IsDeleted""=false and up.""PortalId""='{_userContext.PortalId}')";
            }
            query = query.Replace("#WHERE#", where); 
            var queryData = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }

    }
}
