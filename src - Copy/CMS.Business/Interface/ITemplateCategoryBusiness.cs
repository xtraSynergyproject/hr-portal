using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface ITemplateCategoryBusiness : IBusinessBase<TemplateCategoryViewModel,TemplateCategory>
    {
        Task<List<TemplateCategoryViewModel>> GetCategoryList(string tCode, string tcCode, string mCodes, string templateIds, string categoryIds, TemplateTypeEnum templateType, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, bool allBooks = false, string portalNames=null);
    }
}
