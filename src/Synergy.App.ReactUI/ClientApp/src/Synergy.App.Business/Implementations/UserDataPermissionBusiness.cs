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
    public class UserDataPermissionBusiness : BusinessBase<UserDataPermissionViewModel, UserDataPermission>, IUserDataPermissionBusiness
    {
        private readonly IRepositoryQueryBase<UserDataPermissionViewModel> _apstqueryRepo;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;

        public UserDataPermissionBusiness(IRepositoryBase<UserDataPermissionViewModel, UserDataPermission> repo, IMapper autoMapper,
            IRepositoryQueryBase<UserDataPermissionViewModel> apstqueryRepo, ICmsQueryBusiness cmsQueryBusiness) : base(repo, autoMapper)
        {
            _apstqueryRepo = apstqueryRepo;
            _cmsQueryBusiness = cmsQueryBusiness;
        }

        public async Task<IList<IdNameViewModel>> GetColunmMetadataListByPage(string pageId)
        {
            var list = await _cmsQueryBusiness.GetColunmMetadataListByPageData(pageId);
            return list;
        }

        public async override Task<CommandResult<UserDataPermissionViewModel>> Create(UserDataPermissionViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<UserDataPermissionViewModel>(model);
            var result = await base.Create(data, autoCommit);

            return CommandResult<UserDataPermissionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<UserDataPermissionViewModel>> Edit(UserDataPermissionViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<UserDataPermissionViewModel>(model);
            var result = await base.Edit(data,autoCommit);

            return CommandResult<UserDataPermissionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<IList<UserDataPermissionViewModel>> GetUserDataPermissionByPageId(string pageId,string portalId)
        {
            var list = await _cmsQueryBusiness.GetUserDataPermissionByPageIdData(pageId, portalId);
            return list;
        }

    }
}
