using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class TemplateCategoryBusiness : BusinessBase<TemplateCategoryViewModel, TemplateCategory>, ITemplateCategoryBusiness
    {
        private readonly IRepositoryQueryBase<TemplateCategoryViewModel> _queryRepo;
        public TemplateCategoryBusiness(IRepositoryBase<TemplateCategoryViewModel, TemplateCategory> repo, IMapper autoMapper,
            IRepositoryQueryBase<TemplateCategoryViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<TemplateCategoryViewModel>> Create(TemplateCategoryViewModel model)
        {
            var data = _autoMapper.Map<TemplateCategoryViewModel>(model);
            var result = await base.Create(data);
            return CommandResult<TemplateCategoryViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<TemplateCategoryViewModel>> Edit(TemplateCategoryViewModel model)
        {
            var data = _autoMapper.Map<TemplateCategoryViewModel>(model);
            var result = await base.Edit(data);
            return CommandResult<TemplateCategoryViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<List<TemplateCategoryViewModel>> GetCategoryList(string tCode, string tcCode, string mCodes, string categoryIds, string templateIds, TemplateTypeEnum templateType, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, bool allBooks = false, string portalNames=null)
        {
            var query = $@"select tc.*,t.""ViewType"" as ViewType from public.""TemplateCategory"" as tc
                            join public.""Template"" as t on tc.""Id""=t.""TemplateCategoryId"" and t.""IsDeleted""=false
                            left join public.""Module"" as m on tc.""ModuleId""=m.""Id"" and m.""IsDeleted""=false
                            join public.""Portal"" as p on t.""PortalId""=p.""Id"" #PORTALWHERE#
                            where tc.""TemplateCategoryType""={(int)categoryType} and tc.""IsDeleted""=false --and t.""PortalId""='{_repo.UserContext.PortalId}'
                            #templatetype# #templatecodesearch# #categorycodesearch# #modulecodeSearch# #templateIdsearch# #categoryIdSearch#
                            group by tc.""Id"",t.""ViewType"" order by tc.""SequenceOrder"" ";

            var portalWhere = "";
            if (portalNames.IsNotNullAndNotEmpty())
            {
                portalWhere = $@" and p.""Name"" in ('{portalNames.Replace(",","','")}') ";
            }
            else
            {
                portalWhere = $@" and p.""Id"" = '{_repo.UserContext.PortalId}' ";
            }
            query = query.Replace("#PORTALWHERE#", portalWhere);

            var ttypesearch = "";
            if(templateType == TemplateTypeEnum.Service)
            {
                ttypesearch = $@" and tc.""TemplateType"" in ({(int)TemplateTypeEnum.Service},{(int)TemplateTypeEnum.Custom}) ";
            }
            else
            {
                ttypesearch = $@" and tc.""TemplateType""={(int)templateType} ";
            }
            query = query.Replace("#templatetype#", ttypesearch);

            var templatecodeSearch = "";            
            if (!tCode.IsNullOrEmpty())
            {
                tCode = tCode.Replace(",", "','");
                tCode = String.Concat("'", tCode, "'");
                templatecodeSearch = $@" and t.""Code"" in (" + tCode + ") ";
            }
            query = query.Replace("#templatecodesearch#", templatecodeSearch);

            var modulecodeSearch = "";
            if (!mCodes.IsNullOrEmpty())
            {
                mCodes = mCodes.Replace(",", "','");
                mCodes = String.Concat("'", mCodes, "'");
                modulecodeSearch = @" and m.""Code"" in (" + mCodes + ") ";
            }            
            query = query.Replace("#modulecodeSearch#", modulecodeSearch);

            var categorycodeSearch = "";
            if (!tcCode.IsNullOrEmpty())
            {
                tcCode = tcCode.Replace(",", "','");
                tcCode = String.Concat("'", tcCode, "'");
                categorycodeSearch = @" and tc.""Code"" in (" + tcCode + ") ";
            }
            query = query.Replace("#categorycodesearch#", categorycodeSearch);

            var templateIdSearch = "";
            if (!templateIds.IsNullOrEmpty())
            {
                templateIds = templateIds.Replace(",", "','");
                templateIds = String.Concat("'", templateIds, "'");
                templateIdSearch = @" and t.""Id"" in (" + templateIds + ") ";
            }
            query = query.Replace("#templateIdsearch#", templateIdSearch);

            var categoryIdSearch = "";
            if (!categoryIds.IsNullOrEmpty())
            {
                categoryIds = categoryIds.Replace(",", "','");
                categoryIds = String.Concat("'", categoryIds, "'");
                categoryIdSearch = @" and tc.""Id"" in (" + categoryIds + ") ";
            }
            query = query.Replace("#categoryIdSearch#", categoryIdSearch);

            var list = await _queryRepo.ExecuteQueryList<TemplateCategoryViewModel>(query, null);
            if (allBooks)
            {
                list = list.Where(x => x.ViewType == NtsViewTypeEnum.Book).ToList();
            }
            return list;
        }
    }
}
