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
    public class ModuleBusiness : BusinessBase<ModuleViewModel, Module>, IModuleBusiness
    {
        public ModuleBusiness(IRepositoryBase<ModuleViewModel, Module> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<ModuleViewModel>> Create(ModuleViewModel model, bool autoCommit = true)
        {

            var data = _autoMapper.Map<ModuleViewModel>(model);
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<ModuleViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Create(data, autoCommit);

            return CommandResult<ModuleViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ModuleViewModel>> Edit(ModuleViewModel model, bool autoCommit = true)
        {

            var data = _autoMapper.Map<ModuleViewModel>(model);
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<ModuleViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Edit(data,autoCommit);

            return CommandResult<ModuleViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        private async Task<CommandResult<ModuleViewModel>> IsNameExists(ModuleViewModel model)
        {

            var errorList = new Dictionary<string, string>();

            if (model.Name != null || model.Name != "")
            {
                var name = await _repo.GetSingle(x => x.Name == model.Name && x.Id != model.Id);
                if (name != null)
                {
                    errorList.Add("Name", "Name already exist.");
                }
            }
            if (model.Code != null || model.Code != "")
            {
                var name = await _repo.GetSingle(x => x.Code == model.Code && x.Id != model.Id);
                if (name != null)
                {
                    errorList.Add("Code", "Code already exist.");
                }
            }
            if (errorList.Count > 0)
            {
                return CommandResult<ModuleViewModel>.Instance(model, false, errorList);
            }

            return CommandResult<ModuleViewModel>.Instance();
        }

    }
}
