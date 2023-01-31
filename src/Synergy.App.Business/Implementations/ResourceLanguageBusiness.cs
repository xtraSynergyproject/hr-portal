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
    public class ResourceLanguageBusiness : BusinessBase<ResourceLanguageViewModel, ResourceLanguage>, IResourceLanguageBusiness
    {
        public ResourceLanguageBusiness(IRepositoryBase<ResourceLanguageViewModel, ResourceLanguage> repo, IMapper autoMapper)
            : base(repo, autoMapper)
        {

        }
        public async Task<ResourceLanguageViewModel> GetExistingResourceLanguage(ResourceLanguageViewModel model)
        {
            var existingrecord = await _repo.GetSingle(x => x.Code.ToLower() == model.Code.ToLower()
            && x.TemplateType == model.TemplateType && x.TemplateId == model.TemplateId && x.GroupCode == model.GroupCode
            );
            return existingrecord;
            
        }
        private async Task<CommandResult<ResourceLanguageViewModel>> IsNameExists(ResourceLanguageViewModel model)
        {

            var errorList = new Dictionary<string, string>();       
                var existingrecord = await _repo.GetSingle(x => x.Code.ToLower() == model.Code.ToLower()
                && x.TemplateType==model.TemplateType && x.TemplateId==model.TemplateId && x.GroupCode==model.GroupCode
                && x.Id != model.Id);
                if (existingrecord != null)
                {
                    errorList.Add("Code", "Code already exist.");
                }
            var existingrecord1 = await _repo.GetSingle(x => x.English.ToLower() == model.English.ToLower()
                && x.TemplateType == model.TemplateType && x.TemplateId == model.TemplateId && x.GroupCode == model.GroupCode
                && x.Id != model.Id);
            if (existingrecord != null)
            {
                errorList.Add("English", "English record already exist.");
            }

            if (errorList.Count > 0)
            {
                return CommandResult<ResourceLanguageViewModel>.Instance(model, false, errorList);
            }

            return CommandResult<ResourceLanguageViewModel>.Instance();
        }
        public async override Task<CommandResult<ResourceLanguageViewModel>> Create(ResourceLanguageViewModel model, bool autoCommit = true)
        {
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<ResourceLanguageViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Create(model,autoCommit);

            if (result.IsSuccess)
            {
                if (model.Code == "PortalPageMenuName")
                {
                    var page = await _repo.GetSingleById<PageViewModel, Page>(model.TemplateId);
                    if (page.IsNotNull())
                    {
                        page.MenuName = model.English;
                        await base.Edit<PageViewModel, Page>(page);
                    }
                }
                else if (model.Code == "PortalPageTitle") 
                {
                    var page = await _repo.GetSingleById<PageViewModel, Page>(model.TemplateId);
                    if (page.IsNotNull())
                    {
                        page.Title = model.English;
                        await base.Edit<PageViewModel, Page>(page);
                    }
                }
                else if (model.Code == "MenuGroupName")
                {
                    var menuGroup = await _repo.GetSingleById<MenuGroupViewModel, MenuGroup>(model.TemplateId);
                    if (menuGroup.IsNotNull())
                    {
                        menuGroup.Name = model.English;
                        await base.Edit<MenuGroupViewModel, MenuGroup>(menuGroup);
                    }
                }
            }
            return CommandResult<ResourceLanguageViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async override Task<CommandResult<ResourceLanguageViewModel>> Edit(ResourceLanguageViewModel model, bool autoCommit = true)
        {

            
            //var validateName = await IsNameExists(model);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<ResourceLanguageViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Edit(model,autoCommit);
            if (result.IsSuccess)
            {
                if (model.Code == "PortalPageMenuName")
                {
                    var page = await _repo.GetSingleById<PageViewModel, Page>(model.TemplateId);
                    if (page.IsNotNull())
                    {
                        page.MenuName = model.English;
                        await base.Edit<PageViewModel, Page>(page);
                    }
                }
                else if (model.Code == "PortalPageTitle")
                {
                    var page = await _repo.GetSingleById<PageViewModel, Page>(model.TemplateId);
                    if (page.IsNotNull())
                    {
                        page.Title = model.English;
                        await base.Edit<PageViewModel, Page>(page);
                    }
                }
                else if (model.Code == "MenuGroupName")
                {
                    var menuGroup = await _repo.GetSingleById<MenuGroupViewModel, MenuGroup>(model.TemplateId);
                    if (menuGroup.IsNotNull())
                    {
                        menuGroup.Name = model.English;
                        await base.Edit<MenuGroupViewModel, MenuGroup>(menuGroup);
                    }
                }
            }
            return CommandResult<ResourceLanguageViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public Task<Dictionary<string, string>> GetResourceByTemplate(TemplateTypeEnum templateType, string templateId, string languageCode)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CopyLanguage(List<ResourceLanguageViewModel> items, string newTemplateId)
        {
            foreach (var item in items)
            {
                item.Id = null;
                item.TemplateId = newTemplateId;

                if (item.Id.IsNullOrEmpty())
                {
                    var res = await Create(item);
                    if (!res.IsSuccess)
                    {
                        return false;
                    }
                }
                else
                {
                    var res = await Edit(item);
                    if (!res.IsSuccess)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }
}
