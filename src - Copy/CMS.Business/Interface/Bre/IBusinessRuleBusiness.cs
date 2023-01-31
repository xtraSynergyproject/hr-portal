using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IBusinessRuleBusiness : IBusinessBase<BusinessRuleViewModel, BusinessRule>
    {
        Task<List<BusinessRuleTreeViewModel>> GetBusinessRuleTreeList(string parentId);

        Task<BusinessRuleViewModel> GetDiagramDataByRuleId(string BussinessRuleId);
        Task<CommandResult<T>> ExecuteBusinessRule<T>(BusinessRuleViewModel businessRule, TemplateTypeEnum templateType, T viewModel, dynamic udf) where T : DataModelBase;

        Task<List<BusinessRuleViewModel>> GetBusinessRuleActionList(string templateId, int actionType);
    }
}