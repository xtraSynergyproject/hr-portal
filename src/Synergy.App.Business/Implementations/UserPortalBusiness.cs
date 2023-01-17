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
    public class UserPortalBusiness : BusinessBase<UserPortalViewModel, UserPortal>, IUserPortalBusiness
    {
        private readonly IRepositoryQueryBase<UserPortalViewModel> _queryRepo;
        private readonly IUserContext _userContext;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;

        public UserPortalBusiness(IRepositoryBase<UserPortalViewModel, UserPortal> repo, IMapper autoMapper, ICmsQueryBusiness cmsQueryBusiness,
            IRepositoryQueryBase<UserPortalViewModel> queryRepo, IUserContext userContext) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _userContext = userContext;
            _cmsQueryBusiness = cmsQueryBusiness; 
        }

        public async override Task<CommandResult<UserPortalViewModel>> Create(UserPortalViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<UserPortalViewModel>(model);
            var result = await base.Create(data, autoCommit);

            return CommandResult<UserPortalViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<UserPortalViewModel>> Edit(UserPortalViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<UserPortalViewModel>(model);
            var result = await base.Edit(data,autoCommit);

            return CommandResult<UserPortalViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<List<UserPortalViewModel>> GetPortalByUser(string userId)
        {
            var queryData = await _cmsQueryBusiness.GetPortalByUser(userId);
            return queryData;
        }
        public async Task<List<UserViewModel>> GetUserByPortal(string portalId)
        {
            var queryData = await _cmsQueryBusiness.GetUserByPortal(portalId);
            return queryData;
        }

    }
}
