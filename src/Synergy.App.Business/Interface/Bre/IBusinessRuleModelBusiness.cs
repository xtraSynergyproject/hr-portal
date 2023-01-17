using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IBusinessRuleModelBusiness : IBusinessBase<BusinessRuleModelViewModel, BusinessRuleModel>
    {
        Task<List<BusinessRuleModelViewModel>> GetBusinessRuleModelTreeList(string companyId);
        Task<List<BusinessRuleModelViewModel>> GetBusinessRuleModelList(string nodeId);
        Task<List<BusinessRuleModelViewModel>> GetMasterBusinessRuleModelList(string masterId);
        Task<List<TreeViewViewModel>> ScanFolder(DirectoryInfo directory, string id, string type, string parentId, string templateType, string methodName, string MethodNamespace);
        Task<List<IdNameViewModel>> GetMethodParamName(string methodId, string namespaceString);
        Task ManageOperationValue(string businessRuleNodeId, string decisionScriptComponentId, string breMasterTableMetadataId);
        Task<List<TreeViewViewModel>> ScanFolderNew(string id, string type, string parentId, string templateType, string namespaces, string methodName, BusinessLogicExecutionTypeEnum? nodeType);
        Task<bool> CopyBusinessRuleModelForLogics(List<BusinessRuleModelViewModel> oldBRModelList, List<dynamic> LogicIds);

        Task<bool> CopyBusinessRuleModelForDecisionScript(List<BusinessRuleModelViewModel> oldList, List<dynamic> decisionScriptCompIds);

    }
}
