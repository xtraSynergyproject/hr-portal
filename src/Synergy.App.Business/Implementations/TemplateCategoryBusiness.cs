using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class TemplateCategoryBusiness : BusinessBase<TemplateCategoryViewModel, TemplateCategory>, ITemplateCategoryBusiness
    {
        private readonly IRepositoryQueryBase<TemplateCategoryViewModel> _queryRepo;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;
        public TemplateCategoryBusiness(IRepositoryBase<TemplateCategoryViewModel, TemplateCategory> repo, IMapper autoMapper,
            IRepositoryQueryBase<TemplateCategoryViewModel> queryRepo, ICmsQueryBusiness cmsQueryBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _cmsQueryBusiness = cmsQueryBusiness; 
        }

        public async override Task<CommandResult<TemplateCategoryViewModel>> Create(TemplateCategoryViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<TemplateCategoryViewModel>(model);
            var validateCode = await IsCodeExists(data);
            if (!validateCode.IsSuccess)
            {
                return CommandResult<TemplateCategoryViewModel>.Instance(model, false, validateCode.Messages);
            }
            var result = await base.Create(data, autoCommit);
            return CommandResult<TemplateCategoryViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<TemplateCategoryViewModel>> Edit(TemplateCategoryViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<TemplateCategoryViewModel>(model);
            var validateCode = await IsCodeExists(data);
            if (!validateCode.IsSuccess)
            {
                return CommandResult<TemplateCategoryViewModel>.Instance(model, false, validateCode.Messages);
            }
            var result = await base.Edit(data,autoCommit);
            return CommandResult<TemplateCategoryViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<List<TemplateCategoryViewModel>> GetCategoryList(string tCode, string tcCode, string mCodes, string categoryIds, string templateIds, TemplateTypeEnum templateType, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, bool allBooks = false, string portalNames=null)
        {
            var list = await _cmsQueryBusiness.GetCategoryListData(tCode, tcCode, mCodes, categoryIds, templateIds, templateType, categoryType, allBooks, portalNames);
            if (allBooks)
            {
                list = list.Where(x => x.ViewType == NtsViewTypeEnum.Book).ToList();
            }
            return list;
        }

        public async Task<List<TemplateCategoryViewModel>> GetModuleBasedCategory(string tCode, string tcCode, string mCodes, string categoryIds, string templateIds, TemplateTypeEnum templateType, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, bool allBooks = false, string portalNames = null)
        {
            var list = await _cmsQueryBusiness.GetModuleBasedCategory(tCode, tcCode, mCodes, categoryIds, templateIds, templateType, categoryType, allBooks, portalNames);
            if (allBooks)
            {
                list = list.Where(x => x.ViewType == NtsViewTypeEnum.Book).ToList();
            }
            return list;
        }

        private async Task<CommandResult<TemplateCategoryViewModel>> IsCodeExists(TemplateCategoryViewModel model)
        {
            var errorList = new Dictionary<string, string>();
            if (model.Name.IsNullOrEmpty())
            {
                errorList.Add("Name", "Name is required");
            }
            else if (model.Code.IsNullOrEmpty())
            {
                errorList.Add("Code", "Code is required");
            }
            else
            {
                var name = await _repo.GetSingle(x => x.Code == model.Code && x.Id != model.Id && x.TemplateType == model.TemplateType);
                if (name != null)
                {
                    errorList.Add("Code", "Code already exist");
                }
            }
            if (errorList.Count > 0)
            {
                return CommandResult<TemplateCategoryViewModel>.Instance(model, false, errorList);
            }

            return CommandResult<TemplateCategoryViewModel>.Instance();
        }
    }
}
