using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface ITemplateCategoryBusiness : IBusinessBase<TemplateCategoryViewModel,TemplateCategory>
    {
        Task<List<TemplateCategoryViewModel>> GetCategoryList(string tCode, string tcCode, string mCodes, string templateIds, string categoryIds, TemplateTypeEnum templateType, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, bool allBooks = false, string portalNames=null);
        Task<List<TemplateCategoryViewModel>> GetModuleBasedCategory(string tCode, string tcCode, string mCodes, string templateIds, string categoryIds, TemplateTypeEnum templateType, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, bool allBooks = false, string portalNames=null);
    }
}
