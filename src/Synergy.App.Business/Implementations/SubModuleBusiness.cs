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
    public class SubModuleBusiness : BusinessBase<SubModuleViewModel, SubModule>, ISubModuleBusiness
    {
        private readonly IRepositoryQueryBase<SubModuleViewModel> _queryRepo;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;
        public SubModuleBusiness(IRepositoryBase<SubModuleViewModel, SubModule> repo, IRepositoryQueryBase<SubModuleViewModel> queryRepo, IMapper autoMapper
            , ICmsQueryBusiness cmsQueryBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _cmsQueryBusiness = cmsQueryBusiness;
        }

        public async override Task<CommandResult<SubModuleViewModel>> Create(SubModuleViewModel model, bool autoCommit = true)
        {

            var data = _autoMapper.Map<SubModuleViewModel>(model);
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<SubModuleViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Create(data, autoCommit);

            return CommandResult<SubModuleViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<SubModuleViewModel>> Edit(SubModuleViewModel model, bool autoCommit = true)
        {

            var data = _autoMapper.Map<SubModuleViewModel>(model);
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<SubModuleViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Edit(data,autoCommit);

            return CommandResult<SubModuleViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        private async Task<CommandResult<SubModuleViewModel>> IsNameExists(SubModuleViewModel model)
        {

            var errorList = new Dictionary<string, string>();

            if (model.Name != null || model.Name != "")
            {
                var name = await _repo.GetSingle(x => x.Name == model.Name && x.Id != model.Id && x.ModuleId==model.ModuleId);
                if (name != null)
                {
                    errorList.Add("Name", "Name already exist.");
                }
            }
            if (errorList.Count > 0)
            {
                return CommandResult<SubModuleViewModel>.Instance(model, false, errorList);
            }

            return CommandResult<SubModuleViewModel>.Instance();
        }
        public async Task<IList<SubModuleViewModel>> GetSubModuleList()
        {
            var list = await _cmsQueryBusiness.GetSubModuleListData();
            return list;
        }
        public async Task<IList<SubModuleViewModel>> GetPortalSubModuleList()
        {
            var list = await _cmsQueryBusiness.GetPortalSubModuleListData();
            return list;
        }

    }
}
