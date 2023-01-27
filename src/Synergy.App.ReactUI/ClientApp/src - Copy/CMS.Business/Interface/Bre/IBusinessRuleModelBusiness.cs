using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IBusinessRuleModelBusiness : IBusinessBase<BusinessRuleModelViewModel, BusinessRuleModel>
    {
        Task<List<BusinessRuleModelViewModel>> GetBusinessRuleModelTreeList(string companyId);
        Task<List<BusinessRuleModelViewModel>> GetBusinessRuleModelList(string nodeId);
        Task<List<BusinessRuleModelViewModel>> GetMasterBusinessRuleModelList(string masterId);
        Task<List<TreeViewViewModel>> ScanFolder(DirectoryInfo directory, string id, string type, string parentId, string templateType, string methodName, string MethodNamespace);
        Task<List<IdNameViewModel>> GetMethodParamName(string methodId, string namespaceString);
        Task ManageOperationValue(string businessRuleNodeId, string decisionScriptComponentId, string breMasterTableMetadataId);
        Task<List<TreeViewViewModel>> ScanFolderNew(string id, string type, string parentId, string templateType, string namespaces, string methodName);
    }
}
