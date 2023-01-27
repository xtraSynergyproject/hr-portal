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
    public interface IResourceLanguageBusiness : IBusinessBase<ResourceLanguageViewModel, ResourceLanguage>
    {
        Task<ResourceLanguageViewModel> GetExistingResourceLanguage(ResourceLanguageViewModel model);
        Task<Dictionary<string, string>> GetResourceByTemplate(TemplateTypeEnum templateType, string templateId,string languageCode);
    }
}
