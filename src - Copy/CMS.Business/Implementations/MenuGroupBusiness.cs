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
    public class MenuGroupBusiness : BusinessBase<MenuGroupViewModel, MenuGroup>, IMenuGroupBusiness
    {
        IRepositoryQueryBase<MenuGroupViewModel> _repoQuery;
        private IResourceLanguageBusiness _resourceLanguageBusiness;
        public MenuGroupBusiness(IRepositoryBase<MenuGroupViewModel, MenuGroup> repo, IMapper autoMapper
            , IRepositoryQueryBase<MenuGroupViewModel> repoQuery
            , IResourceLanguageBusiness resourceLanguageBusiness) : base(repo, autoMapper)
        {
            _repoQuery = repoQuery;
            _resourceLanguageBusiness = resourceLanguageBusiness;
        }

        public async override Task<CommandResult<MenuGroupViewModel>> Create(MenuGroupViewModel model)
        {
            var data = _autoMapper.Map<MenuGroupViewModel>(model);
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<MenuGroupViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Create(data);
            if (result.IsSuccess)
            {
                if (model.Name.IsNotNullAndNotEmpty())
                {
                    var Ldata = new ResourceLanguageViewModel
                    {
                        TemplateId = result.Item.Id,
                        TemplateType = TemplateTypeEnum.MenuGroup,
                        English = result.Item.Name,
                        Code = "MenuGroupName"
                    };
                    await base.Create<ResourceLanguageViewModel, ResourceLanguage>(Ldata);
                }
            }
            return CommandResult<MenuGroupViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<MenuGroupViewModel>> Edit(MenuGroupViewModel model)
        {
            var data = _autoMapper.Map<MenuGroupViewModel>(model);
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<MenuGroupViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Edit(data);
            if (result.IsSuccess)
            {
                var name = await _repo.GetSingle(x => x.Name.ToLower() != model.Name.ToLower() && x.Id==model.Id);
                if (name == null)
                {
                    if (model.Name.IsNotNullAndNotEmpty())
                    {
                        var Ldata = new ResourceLanguageViewModel
                        {
                            TemplateId = result.Item.Id,
                            TemplateType = TemplateTypeEnum.MenuGroup,
                            English = result.Item.Name,
                            Code = "MenuGroupName"
                        };
                        var record = await _resourceLanguageBusiness.GetExistingResourceLanguage(Ldata);
                        if (record != null)
                        {
                            record.English = result.Item.Name;
                            await _resourceLanguageBusiness.Edit(record);
                        }
                        else
                        {
                            await base.Edit<ResourceLanguageViewModel, ResourceLanguage>(Ldata);
                        }

                    }
                }
               
            }
            return CommandResult<MenuGroupViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<List<MenuGroupViewModel>> GetMenuGroupList()
        {
            var query = @$"select mg.*,p.""Name"" as ""SubModule"",p.""Name"" as ""PortalName"" from public.""MenuGroup"" as mg
            left join public.""SubModule"" as sm on mg.""SubModuleId""=sm.""Id"" and sm.""IsDeleted""=false
            left join public.""Portal"" as p on mg.""PortalId""=p.""Id"" and p.""IsDeleted""=false
            where mg.""IsDeleted""=false";
            return await _repoQuery.ExecuteQueryList<MenuGroupViewModel>(query, null);
        }


        public async Task<List<MenuGroupViewModel>> GetMenuGroupListPortalAdmin(string PortalId,string LegalEntityId)
        {
            var query = @$"select mg.*,sm.""Name"" as ""SubModule"",p.""Name"" as ""PortalName"" from public.""MenuGroup"" as mg
            left join public.""SubModule"" as sm on mg.""SubModuleId""=sm.""Id"" and sm.""IsDeleted""=false and mg.""CompanyId"" = '{_repo.UserContext.CompanyId}' and sm.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            left join public.""Portal"" as p on mg.""PortalId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            where mg.""IsDeleted""=false  and mg.""PortalId""='{PortalId}' and mg.""LegalEntityId""='{LegalEntityId}'
             and sm.""PortalId""='{PortalId}' and sm.""LegalEntityId""='{LegalEntityId}'";
            return await _repoQuery.ExecuteQueryList<MenuGroupViewModel>(query, null);
        }


        public async Task<List<MenuGroupViewModel>> GetMenuGroupListparentPortalAdmin(string PortalId, string LegalEntityId,string Id)
        {
            var query = @$"select mg.*,p.""Name"" as ""SubModule"",p.""Name"" as ""PortalName"" from public.""MenuGroup"" as mg
            left join public.""SubModule"" as sm on mg.""SubModuleId""=sm.""Id"" and sm.""IsDeleted""=false and mg.""CompanyId"" = '{_repo.UserContext.CompanyId}' and sm.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            left join public.""Portal"" as p on mg.""PortalId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            where mg.""IsDeleted""=false  and mg.""PortalId""='{PortalId}' and mg.""LegalEntityId""='{LegalEntityId}'
             and sm.""PortalId""='{PortalId}' and sm.""LegalEntityId""='{LegalEntityId}' and mg.""Id""!='{Id}'";
            return await _repoQuery.ExecuteQueryList<MenuGroupViewModel>(query, null);
        }

        private async Task<CommandResult<MenuGroupViewModel>> IsNameExists(MenuGroupViewModel model)
        {
            var errorList = new Dictionary<string, string>();
            if (model.Name.IsNullOrEmpty())
            {
                errorList.Add("Name", "Name is required.");
            }
            else if (model.ShortName.IsNullOrEmpty())
            {
                errorList.Add("ShortName", "Short Name is required.");
            }
            else if (model.PortalId.IsNullOrEmpty())
            {
                errorList.Add("Portal", "Portal is required.");
            }
            else
            {
                var name = await _repo.GetSingle(x => x.Name.ToLower() == model.Name.ToLower() && x.Id != model.Id && x.SubModuleId == model.SubModuleId);
                if (name != null)
                {
                    errorList.Add("Name", "Name already exist.");
                }
            }
            if (errorList.Count > 0)
            {
                return CommandResult<MenuGroupViewModel>.Instance(model, false, errorList);
            }

            return CommandResult<MenuGroupViewModel>.Instance();
        }
    }
}
