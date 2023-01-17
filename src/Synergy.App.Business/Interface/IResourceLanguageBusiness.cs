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
    public interface IResourceLanguageBusiness : IBusinessBase<ResourceLanguageViewModel, ResourceLanguage>
    {
        Task<ResourceLanguageViewModel> GetExistingResourceLanguage(ResourceLanguageViewModel model);
        Task<Dictionary<string, string>> GetResourceByTemplate(TemplateTypeEnum templateType, string templateId,string languageCode);
        Task<bool> CopyLanguage(List<ResourceLanguageViewModel> items, string newTemplateId);
    }
}
