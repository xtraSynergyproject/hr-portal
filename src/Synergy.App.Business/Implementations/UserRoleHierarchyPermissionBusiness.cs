using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
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


namespace Synergy.App.Business
{
    public class UserRoleHierarchyPermissionBusiness : BusinessBase<UserRoleHierarchyPermissionViewModel, UserRoleHierarchyPermission>, IUserRoleHierarchyPermissionBusiness
    {
        private readonly IRepositoryQueryBase<UserRoleHierarchyPermissionViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo1;
        private IUserContext _userContext;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;
        public UserRoleHierarchyPermissionBusiness(IRepositoryBase<UserRoleHierarchyPermissionViewModel, UserRoleHierarchyPermission> repo, IMapper autoMapper, ICmsQueryBusiness cmsQueryBusiness,
            IRepositoryQueryBase<UserRoleHierarchyPermissionViewModel> queryRepo,
            IRepositoryQueryBase<IdNameViewModel> queryRepo1, IUserContext userContext) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _queryRepo1 = queryRepo1;
            _userContext = userContext;
            _cmsQueryBusiness = cmsQueryBusiness;
        }

        public async override Task<CommandResult<UserRoleHierarchyPermissionViewModel>> Create(UserRoleHierarchyPermissionViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<UserRoleHierarchyPermissionViewModel>(model);
            var result = await base.Create(data, autoCommit);

            return CommandResult<UserRoleHierarchyPermissionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<UserRoleHierarchyPermissionViewModel>> Edit(UserRoleHierarchyPermissionViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<UserRoleHierarchyPermissionViewModel>(model);
            var result = await base.Edit(data,autoCommit);

            return CommandResult<UserRoleHierarchyPermissionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<List<UserRoleHierarchyPermissionViewModel>> GetUserPermissionHierarchy(string userRoleId)
        {
            var queryData = await _cmsQueryBusiness.GetUserRolePermissionHierarchy(userRoleId);
            return queryData;
        }
        public async Task<List<IdNameViewModel>> GetHierarchyData(HierarchyTypeEnum userId)
        {
            var queryData = await _cmsQueryBusiness.GetHierarchyData(userId);
            return queryData;
        }
        public async Task<List<UserRoleHierarchyPermissionViewModel>> GetUserPermissionHierarchyForPortal(string userRoleId)
        {
            var queryData = await _cmsQueryBusiness.GetUserRolePermissionHierarchyForPortal(userRoleId);
            return queryData;
        }
        public async Task<List<IdNameViewModel>> GetUserNotInPermissionHierarchy(string UserId)
        {

            var queryData = await _cmsQueryBusiness.GetUserRoleNotInPermissionHierarchy(UserId);
            return queryData;
        }

    }
}
