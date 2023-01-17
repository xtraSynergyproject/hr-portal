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
    public class UserRolePortalBusiness : BusinessBase<UserRolePortalViewModel, UserRolePortal>, IUserRolePortalBusiness
    {
        private readonly IRepositoryQueryBase<UserRolePortalViewModel> _queryRepo;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;

        public UserRolePortalBusiness(IRepositoryBase<UserRolePortalViewModel, UserRolePortal> repo, IMapper autoMapper, ICmsQueryBusiness cmsQueryBusiness,
            IRepositoryQueryBase<UserRolePortalViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _cmsQueryBusiness = cmsQueryBusiness;
        }

        public async override Task<CommandResult<UserRolePortalViewModel>> Create(UserRolePortalViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<UserRolePortalViewModel>(model);
            var result = await base.Create(data, autoCommit);

            return CommandResult<UserRolePortalViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<UserRolePortalViewModel>> Edit(UserRolePortalViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<UserRolePortalViewModel>(model);
            var result = await base.Edit(data,autoCommit);

            return CommandResult<UserRolePortalViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<List<UserRolePortalViewModel>> GetPortalByUserRole(string userRoleId)
        {
            var queryData = await _cmsQueryBusiness.GetPortalByUserRole(userRoleId);
            return queryData;
        }
        public async Task<List<UserRoleViewModel>> GetUserRoleByPortal(string portalId)
        {
            var queryData = await _cmsQueryBusiness.GetUserRoleByPortal(portalId);
            return queryData;
        }
    }
}
