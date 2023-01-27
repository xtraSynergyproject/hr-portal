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
    public class UserRoleDataPermissionBusiness : BusinessBase<UserRoleDataPermissionViewModel, UserRoleDataPermission>, IUserRoleDataPermissionBusiness
    {
        private readonly IRepositoryQueryBase<UserDataPermissionViewModel> _apstqueryRepo;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;

        public UserRoleDataPermissionBusiness(IRepositoryBase<UserRoleDataPermissionViewModel, UserRoleDataPermission> repo, IMapper autoMapper, ICmsQueryBusiness cmsQueryBusiness,
            IRepositoryQueryBase<UserDataPermissionViewModel> apstqueryRepo) : base(repo, autoMapper)
        {
            _apstqueryRepo = apstqueryRepo;
            _cmsQueryBusiness = cmsQueryBusiness;
        }

        public async Task<IList<IdNameViewModel>> GetColunmMetadataListByPageForUserRole(string pageId)
        {

            var list = await _cmsQueryBusiness.GetColunmMetadataListByPageForUserRole(pageId);
            return list;
        }

        public async Task<IList<UserRoleDataPermissionViewModel>> GetUserRoleDataPermissionByPageId(string pageId,string portalId)
        {

            var list = await _cmsQueryBusiness.GetUserRoleDataPermissionByPageId(pageId, portalId);
            return list;
        }

        public async override Task<CommandResult<UserRoleDataPermissionViewModel>> Create(UserRoleDataPermissionViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<UserRoleDataPermissionViewModel>(model);
            var result = await base.Create(data, autoCommit);

            return CommandResult<UserRoleDataPermissionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<UserRoleDataPermissionViewModel>> Edit(UserRoleDataPermissionViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<UserRoleDataPermissionViewModel>(model);
            var result = await base.Edit(data,autoCommit);

            return CommandResult<UserRoleDataPermissionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
    }
}
