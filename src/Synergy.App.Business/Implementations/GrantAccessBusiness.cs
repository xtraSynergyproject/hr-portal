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
    public class GrantAccessBusiness : BusinessBase<GrantAccessViewModel, GrantAccess>, IGrantAccessBusiness
    {
        private readonly IRepositoryQueryBase<GrantAccessViewModel> _queryRepo;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;
        public GrantAccessBusiness(IRepositoryBase<GrantAccessViewModel, GrantAccess> repo,
            IRepositoryQueryBase<GrantAccessViewModel> queryRepo,
            IMapper autoMapper, ICmsQueryBusiness cmsQueryBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _cmsQueryBusiness = cmsQueryBusiness;
        }

        public async override Task<CommandResult<GrantAccessViewModel>> Create(GrantAccessViewModel model, bool autoCommit = true)
        {

            var data = _autoMapper.Map<GrantAccessViewModel>(model);           
            var result = await base.Create(data, autoCommit);

            return CommandResult<GrantAccessViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<GrantAccessViewModel>> Edit(GrantAccessViewModel model, bool autoCommit = true)
        {

            var data = _autoMapper.Map<GrantAccessViewModel>(model);           
            var result = await base.Edit(data,autoCommit);

            return CommandResult<GrantAccessViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<IList<GrantAccessViewModel>> GetGrantAccessList(string userId)
        {
            var list = await _cmsQueryBusiness.GetGrantAccessListData(userId);
            return list;
        }

    }
}
