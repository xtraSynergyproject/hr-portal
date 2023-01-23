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
    public class MenuGroupBusiness : BusinessBase<MenuGroupViewModel, MenuGroup>, IMenuGroupBusiness
    {
        IRepositoryQueryBase<MenuGroupViewModel> _repoQuery;
        private IResourceLanguageBusiness _resourceLanguageBusiness;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;
        public MenuGroupBusiness(IRepositoryBase<MenuGroupViewModel, MenuGroup> repo, IMapper autoMapper
            , IRepositoryQueryBase<MenuGroupViewModel> repoQuery
            , IResourceLanguageBusiness resourceLanguageBusiness, ICmsQueryBusiness cmsQueryBusiness) : base(repo, autoMapper)
        {
            _repoQuery = repoQuery;
            _resourceLanguageBusiness = resourceLanguageBusiness;
            _cmsQueryBusiness = cmsQueryBusiness;
        }

        public async override Task<CommandResult<MenuGroupViewModel>> Create(MenuGroupViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<MenuGroupViewModel>(model);
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<MenuGroupViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Create(data, autoCommit);
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

        public async override Task<CommandResult<MenuGroupViewModel>> Edit(MenuGroupViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<MenuGroupViewModel>(model);
            var validateName = await IsNameExists(data);
            if (!validateName.IsSuccess)
            {
                return CommandResult<MenuGroupViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Edit(data,autoCommit);
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
                            await base.Create<ResourceLanguageViewModel, ResourceLanguage>(Ldata);
                        }

                    }
                }
               
            }
            return CommandResult<MenuGroupViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<List<MenuGroupViewModel>> GetMenuGroupList()
        {
            return await _cmsQueryBusiness.GetMenuGroupListData();
        }


        public async Task<List<MenuGroupViewModel>> GetMenuGroupListPortalAdmin(string PortalId,string LegalEntityId)
        {
            return await _cmsQueryBusiness.GetMenuGroupListPortalAdminData(PortalId, LegalEntityId);
        }


        public async Task<List<MenuGroupViewModel>> GetMenuGroupListparentPortalAdmin(string PortalId, string LegalEntityId,string Id)
        {
            return await _cmsQueryBusiness.GetMenuGroupListparentPortalAdminData(PortalId, LegalEntityId, Id);
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
                var name = await _repo.GetSingle(x => x.Name.ToLower() == model.Name.ToLower() && x.Id != model.Id && x.SubModuleId == model.SubModuleId && x.PortalId == model.PortalId);
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
