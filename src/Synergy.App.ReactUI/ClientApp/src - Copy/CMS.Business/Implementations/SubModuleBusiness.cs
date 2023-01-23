using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class SubModuleBusiness : BusinessBase<SubModuleViewModel, SubModule>, ISubModuleBusiness
    {
        private readonly IRepositoryQueryBase<SubModuleViewModel> _queryRepo;
        public SubModuleBusiness(IRepositoryBase<SubModuleViewModel, SubModule> repo, IRepositoryQueryBase<SubModuleViewModel> queryRepo, IMapper autoMapper) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<SubModuleViewModel>> Create(SubModuleViewModel model)
        {

            var data = _autoMapper.Map<SubModuleViewModel>(model);
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<SubModuleViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Create(data);

            return CommandResult<SubModuleViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<SubModuleViewModel>> Edit(SubModuleViewModel model)
        {

            var data = _autoMapper.Map<SubModuleViewModel>(model);
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<SubModuleViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Edit(data);

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
            string query = @$"select s.*,m.""Name"" as ""ModuleName"" from public.""SubModule"" as s
                             join public.""Module"" as m on m.""Id""=s.""ModuleId""
                              where s.""IsDeleted""=false";
            var list = await _queryRepo.ExecuteQueryList<SubModuleViewModel>(query, null);
            return list;
        }
        public async Task<IList<SubModuleViewModel>> GetPortalSubModuleList()
        {
            string query = @$"select s.*,m.""Name"" as ""ModuleName"" from public.""SubModule"" as s
                             join public.""Module"" as m on m.""Id""=s.""ModuleId""
                              where s.""IsDeleted""=false and s.""PortalId""='{_repo.UserContext.PortalId}' and s.""LegalEntityId""='{_repo.UserContext.LegalEntityId}'";
            var list = await _queryRepo.ExecuteQueryList<SubModuleViewModel>(query, null);
            return list;
        }

    }
}
