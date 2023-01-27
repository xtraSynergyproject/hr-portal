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
    public class UserHierarchyPermissionBusiness : BusinessBase<UserHierarchyPermissionViewModel, UserHierarchyPermission>, IUserHierarchyPermissionBusiness
    {
        private readonly IRepositoryQueryBase<UserHierarchyPermissionViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo1;
        private IUserContext _userContext;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;
        public UserHierarchyPermissionBusiness(IRepositoryBase<UserHierarchyPermissionViewModel, UserHierarchyPermission> repo, IMapper autoMapper, ICmsQueryBusiness cmsQueryBusiness,
            IRepositoryQueryBase<UserHierarchyPermissionViewModel> queryRepo,
            IRepositoryQueryBase<IdNameViewModel> queryRepo1, IUserContext userContext) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _queryRepo1 = queryRepo1;
            _userContext = userContext;
            _cmsQueryBusiness = cmsQueryBusiness;
        }

        public async override Task<CommandResult<UserHierarchyPermissionViewModel>> Create(UserHierarchyPermissionViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<UserHierarchyPermissionViewModel>(model);
            var result = await base.Create(data, autoCommit);

            return CommandResult<UserHierarchyPermissionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<UserHierarchyPermissionViewModel>> Edit(UserHierarchyPermissionViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<UserHierarchyPermissionViewModel>(model);
            var result = await base.Edit(data,autoCommit);

            return CommandResult<UserHierarchyPermissionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<List<UserHierarchyPermissionViewModel>> GetUserPermissionHierarchy(string userId)
        {
            var queryData = await _cmsQueryBusiness.GetUserPermissionHierarchy(userId);
            return queryData;
        }
        public async Task<List<IdNameViewModel>> GetHierarchyData(HierarchyTypeEnum userId)
        {
            var queryData = await _cmsQueryBusiness.GetHierarchyData(userId);
            return queryData;
        }
        public async Task<List<UserHierarchyPermissionViewModel>> GetUserPermissionHierarchyForPortal(string userId)
        {
            var queryData = await _cmsQueryBusiness.GetUserPermissionHierarchyForPortal(userId);
            return queryData;
        }
        public async Task<List<IdNameViewModel>> GetUserNotInPermissionHierarchy(string UserId)
        {

            var queryData = await _cmsQueryBusiness.GetUserNotInPermissionHierarchy(UserId);
            return queryData;
        }

    }
}
