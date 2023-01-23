using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Synergy.App.Business
{
    public class BusinessRuleModelBusiness : BusinessBase<BusinessRuleModelViewModel, BusinessRuleModel>, IBusinessRuleModelBusiness
    {
        private IBreMetadataBusiness _breMetaDataBusiness;
        private IColumnMetadataBusiness _columnMetaDataBusiness;
        public BusinessRuleModelBusiness(IRepositoryBase<BusinessRuleModelViewModel, BusinessRuleModel> repo, IMapper autoMapper,
            IBreMetadataBusiness breMetaDataBusiness, IColumnMetadataBusiness columnMetaDataBusiness) : base(repo, autoMapper)
        {
            _breMetaDataBusiness = breMetaDataBusiness;
            _columnMetaDataBusiness = columnMetaDataBusiness;
        }

        public Task<List<BusinessRuleModelViewModel>> GetBusinessRuleModelTreeList(string companyId)
        {
            throw new NotImplementedException();
        }
        public async override Task<CommandResult<BusinessRuleModelViewModel>> Create(BusinessRuleModelViewModel model, bool autoCommit = true)
        {
            var result = await base.Create(model, autoCommit);
            if (result.IsSuccess)
            {
                await ManageOperationValue(model.BusinessRuleNodeId, model.DecisionScriptComponentId, model.BreMasterTableMetadataId);
            }
            return result;
        }

        public async Task ManageOperationValue(string businessRuleNodeId, string decisionScriptComponentId, string breMasterTableMetadataId)
        {
            var rules = new List<BusinessRuleModelViewModel>();
            if (businessRuleNodeId.IsNotNullAndNotEmpty())
            {
                var text = "";
                rules = await GetList(x => x.BusinessRuleNodeId == businessRuleNodeId);
                var rule = rules.FirstOrDefault(x => x.ParentId == null);
                if (rule != null)
                {
                    text = await GenerateDecisionRule(rule, rules);
                }
                var node = await _repo.GetSingleById<BusinessRuleNodeViewModel, BusinessRuleNode>(businessRuleNodeId);
                if (node != null)
                {
                    node.OperationValue = text;
                    await _repo.Edit<BusinessRuleNodeViewModel, BusinessRuleNode>(node);
                }

            }
            else if (decisionScriptComponentId.IsNotNullAndNotEmpty())
            {
                var text = "";
                rules = await GetList(x => x.DecisionScriptComponentId == decisionScriptComponentId);
                var rule = rules.FirstOrDefault(x => x.ParentId == null);
                if (rule != null)
                {
                    text = await GenerateDecisionRule(rule, rules);
                }
                var componennt = await _repo.GetSingleById<DecisionScriptComponentViewModel, DecisionScriptComponent>(decisionScriptComponentId);
                if (componennt != null)
                {
                    componennt.OperationValue = text;
                    await _repo.Edit<DecisionScriptComponentViewModel, DecisionScriptComponent>(componennt);
                }
            }
            else if (breMasterTableMetadataId.IsNotNullAndNotEmpty())
            {
                var text = "";
                rules = await GetList(x => x.BreMasterTableMetadataId == breMasterTableMetadataId);
                var rule = rules.FirstOrDefault(x => x.ParentId == null);
                if (rule != null)
                {
                    text = await GenerateDecisionRule(rule, rules);
                }
                var metadata = await _repo.GetSingleById<BreMasterTableMetadataViewModel, BreMasterTableMetadata>(breMasterTableMetadataId);
                if (metadata != null)
                {
                    metadata.OperationValue = text;
                    await _repo.Edit<BreMasterTableMetadataViewModel, BreMasterTableMetadata>(metadata);
                }
            }
        }
        private async Task<string> GenerateDecisionRule(BusinessRuleModelViewModel rule, List<BusinessRuleModelViewModel> rules)
        {
            bool? result = null;
            var text = "(";
            switch (rule.BusinessRuleType)
            {
                case BusinessRuleTypeEnum.Logical:
                    var childRules = rules.Where(x => x.ParentId == rule.Id).ToList();
                    var logic = rule.Condition.Value;
                    if (logic == LogicalEnum.And)
                    {
                        foreach (var item in childRules)
                        {
                            if (result == null || result.Value == true)
                            {
                                var andResult = await GenerateDecisionRule(item, rules);
                                text = $"{text}{andResult} && ";
                            }
                            else
                            {
                                break;
                            }
                        }
                        text = text.Trim(' ').Trim('&').Trim('&');
                    }
                    else
                    {
                        foreach (var item in childRules)
                        {
                            if (result == null || result.Value == false)
                            {
                                var orResult = await GenerateDecisionRule(item, rules);
                                text = $"{text}{orResult} ||";
                            }
                            else
                            {
                                break;
                            }
                        }
                        text = text.Trim(' ').Trim('|').Trim('|');
                    }
                    break;
                case BusinessRuleTypeEnum.Operational:
                    text = $"{text}{rule.OperationBackendValue}";
                    break;
                default:
                    break;
            }
            return $"{text.Trim()})";
            //return result ?? false;
        }


        public async override Task<CommandResult<BusinessRuleModelViewModel>> Edit(BusinessRuleModelViewModel model, bool autoCommit = true)
        {
            var result = await base.Edit(model, autoCommit);
            if (result.IsSuccess)
            {
                await ManageOperationValue(model.BusinessRuleNodeId, model.DecisionScriptComponentId, model.BreMasterTableMetadataId);
            }
            return result;
        }
        public async override Task Delete(string id, bool autoCommit = true)
        {
            var model = await _repo.GetSingleById(id);
            await base.Delete(id, autoCommit);
            if (model != null)
            {
                await ManageOperationValue(model.BusinessRuleNodeId, model.DecisionScriptComponentId, model.BreMasterTableMetadataId);
            }
        }
        public async Task<List<BusinessRuleModelViewModel>> GetBusinessRuleModelList(string nodeId)
        {
            var data = await GetList(x => x.BusinessRuleNodeId == nodeId);
            foreach (var item in data)
            {
                if (item.FieldSourceType == BreMetadataTypeEnum.InputData)
                {
                    // fetch the input data and get its name
                    var inputData = await _columnMetaDataBusiness.GetSingleById(item.FieldId);
                    if (inputData.IsNotNull())
                    {
                        item.Field = inputData.Alias;
                        item.FieldSourceTypeStr = "InputData";
                    }
                }
                else if (item.FieldSourceType == BreMetadataTypeEnum.MasterData)
                {
                    var masterData = await _breMetaDataBusiness.GetSingleById(item.FieldId);
                    if (masterData.IsNotNull())
                    {
                        item.Field = masterData.Name;
                        item.FieldSourceTypeStr = "MasterData";
                    }
                }
                else if (item.FieldSourceType == BreMetadataTypeEnum.Constant)
                {
                    item.FieldSourceTypeStr = "Constant";
                }
                if (item.ValueSourceType == BreMetadataTypeEnum.InputData)
                {
                    // fetch the input data and get its name
                    var inputData = await _columnMetaDataBusiness.GetSingleById(item.ValueId);
                    if (inputData.IsNotNull())
                    {
                        item.Value = inputData.Alias;
                        item.ValueSourceTypeStr = "InputData";
                    }
                }
                else if (item.ValueSourceType == BreMetadataTypeEnum.MasterData)
                {
                    var masterData = await _breMetaDataBusiness.GetSingleById(item.ValueId);
                    if (masterData.IsNotNull())
                    {
                        item.Value = masterData.Name;
                        item.ValueSourceTypeStr = "MasterData";
                    }
                }
                else if (item.ValueSourceType == BreMetadataTypeEnum.Constant)
                {
                    item.ValueSourceTypeStr = "Constant";
                }
            }
            return data;
        }

        public async Task<List<BusinessRuleModelViewModel>> GetMasterBusinessRuleModelList(string masterId)
        {
            var data = await GetList(x => x.BreMasterTableMetadataId == masterId);
            foreach (var item in data)
            {
                var collectionValue = await _breMetaDataBusiness.GetSingleById(item.FieldId);
                //if (collectionValue != null)
                //{
                //    var dbCollection = await _repo.GetSingleGlobal<DataIntegrationViewModel, DataIntegration>(x => x.Id == collectionValue.Id);
                //    var schemaList = JsonConvert.DeserializeObject<Dictionary<string, string>>(dbCollection.Schema);
                //    item.Field = schemaList.Where(x=>x.Key=="");
                //}

                if (item.ValueSourceType == BreMetadataTypeEnum.InputData)
                {
                    // fetch the input data and get its name                   
                    var inputData = await _breMetaDataBusiness.GetSingleById(item.ValueId);
                    item.Value = inputData.Name;
                }
            }
            return data;
        }

        public async Task<List<TreeViewViewModel>> ScanFolder(DirectoryInfo directory, string id, string type, string parentId, string templateType, string methodName, string MethodNamespace)
        {
            var model = new List<TreeViewViewModel>();

            var subDirectories = type != "Class" ? directory.GetDirectories() : null;
            if (id == null)
            {
                model.Add(new TreeViewViewModel { id = directory.Name, Name = directory.Name, ParentId = parentId, hasChildren = subDirectories.Length > 0, Type = "BusinessScript", Directory = directory.FullName });
            }
            else if (type == "BusinessScript")
            {
                foreach (var subdirectory in subDirectories)
                {
                    if (subdirectory.Name == templateType.ToString())
                    {
                        model.Add(new TreeViewViewModel { id = subdirectory.Name, Name = subdirectory.Name, ParentId = id, hasChildren = true, Type = "Nts", Directory = subdirectory.FullName });
                    }
                }
            }
            else if (type == "Nts")
            {
                foreach (var subdirectory in subDirectories)
                    model.Add(new TreeViewViewModel { id = subdirectory.Name, Name = subdirectory.Name, ParentId = id, hasChildren = true, Type = "CompanyName", Directory = subdirectory.FullName });
            }
            else if (type == "CompanyName")
            {
                foreach (var subdirectory in subDirectories)
                    model.Add(new TreeViewViewModel { id = subdirectory.Name, Name = subdirectory.Name, ParentId = id, hasChildren = true, Type = "Portal", Directory = subdirectory.FullName });
            }
            else if (type == "Portal")
            {
                foreach (var file in directory.GetFiles())
                {
                    model.Add(new TreeViewViewModel { id = file.Name, Type = "Class", Name = file.Name, ParentId = directory.Name, hasChildren = true, Directory = directory.FullName });
                }
            }
            else if (type == "Class")
            {
                foreach (var file in directory.GetFiles())
                {
                    if (id == file.Name)
                    {
                        var namespaceString = file.FullName.Split("src\\")[1].Replace("\\", ".").Replace(file.Extension, "");
                        Type tp = Type.GetType(namespaceString);
                        if (tp.IsNotNull())
                        {
                            MethodInfo[] methods = tp.GetMethods();
                            int i = 0;
                            foreach (MethodInfo mi in methods)
                            {
                                i++;
                                var isSelected = false;
                                if (mi.Name == methodName && namespaceString == MethodNamespace)
                                {
                                    isSelected = true;
                                    //model.Where(x => x.id == file.Name).FirstOrDefault().expanded = true;
                                }
                                model.Add(new TreeViewViewModel { id = mi.Name, Type = "Methods", Name = mi.Name, ParentId = file.Name, Checked = isSelected, hasChildren = false, Directory = namespaceString });
                            }
                        }
                    }
                }

            }
            return model.ToList();
        }

        public async Task<List<IdNameViewModel>> GetMethodParamName(string methodId, string namespaceString)
        {
            var model = new List<IdNameViewModel>();
            Type tp = Type.GetType(namespaceString);
            if (tp.IsNotNull())
            {
                var methodList = tp.GetMethods();
                if (methodList.IsNotNull())
                {
                    var method = methodList.Where(x => x.Name == methodId).FirstOrDefault();
                    var parameters = method.GetParameters();
                    var xd = new XmlDoc("..\\Synergy.App.Business\\CmsBusiness.xml");
                    Console.WriteLine("Method: " + XmlDoc.MethodSignature(method));
                    Console.WriteLine("XML Name: " + XmlDoc.XmlName(method));
                    Console.WriteLine("XML Summary: " + xd.XmlSummary(method));
                    Console.WriteLine("XML Param: " + xd.XmlParamSummary(method));
                    var paramList = new List<IdNameViewModel>();
                    paramList.Add(new IdNameViewModel
                    {
                        Id = "summary",
                        Name = xd.XmlSummary(method),
                    });
                    paramList.Add(new IdNameViewModel
                    {
                        Id = "xmlName",
                        Name = XmlDoc.XmlName(method),
                    });
                    paramList.Add(new IdNameViewModel
                    {
                        Id = "method",
                        Name = XmlDoc.MethodSignature(method),
                    });
                    paramList.AddRange(xd.XmlParamSummary(method));
                    return paramList;
                }
                else
                {
                    return model;
                }
            }
            else
            {
                return model;
            }
        }

        public async Task<List<TreeViewViewModel>> ScanFolderNew(string id, string type, string parentId, string templateType, string namespaces, string methodName, BusinessLogicExecutionTypeEnum? nodeType)
        {
            var model = new List<TreeViewViewModel>();

            var allNamespace = AllNameSpace().Where(x => x != null).ToList();
            if (id == null)
            {
                model.Add(new TreeViewViewModel
                {
                    id = "BusinessScript",
                    Name = "BusinessScript",
                    ParentId = parentId,
                    hasChildren = true,
                    Type = "BusinessScript",
                    Namespace = "Synergy.App.Business.Implementations.BusinessScript.",
                    HideCheckbox = false,
                });
            }
            else if (type == "BusinessScript")
            {
                model.Add(new TreeViewViewModel { HideCheckbox = false, id = templateType, Name = templateType, ParentId = parentId, hasChildren = true, Type = "Nts", Namespace = namespaces + templateType });
            }
            else if (type == "Nts")
            {
                var company = allNamespace.Where(x => x.Contains(namespaces)).Distinct().ToList();
                foreach (var a in company)
                {
                    var comp = a.Split(namespaces + ".")[1].Split(".")[0];
                    if (!model.Any(n => n.Name == comp))
                    {
                        model.Add(new TreeViewViewModel
                        {
                            id = comp,
                            Name = comp,
                            ParentId = parentId,
                            hasChildren = true,
                            Type = "CompanyName",
                            Namespace = namespaces + "." + comp,
                            HideCheckbox = false,
                        });
                    }

                }
            }
            else if (type == "CompanyName")
            {
                var company = allNamespace.Where(x => x.Contains(namespaces)).Distinct().ToList();
                foreach (var a in company)
                {
                    var comp = a.Split(namespaces + ".")[1].Split(".")[0];
                    model.Add(new TreeViewViewModel
                    {
                        id = comp,
                        Name = comp,
                        ParentId = parentId,
                        hasChildren = true,
                        Type = "Portal",
                        Namespace = namespaces + "." + comp,
                        HideCheckbox = false,
                    });

                }
            }
            else if (type == "Portal")
            {
                var company = allNamespace.Where(x => x.Contains(namespaces)).Distinct().ToList();
                var methods = GetTypesInNamespace(namespaces);
                if (nodeType == BusinessLogicExecutionTypeEnum.PreLoad)
                {
                    methods = methods.Where(x => x.Name.Contains("PreLoad")).ToList();
                }
                foreach (var a in methods.Where(x => x.MemberType == MemberTypes.TypeInfo).ToList())
                {
                    var comp = a.Name;
                    model.Add(new TreeViewViewModel
                    {
                        id = comp,
                        Name = comp,
                        ParentId = parentId,
                        hasChildren = true,
                        Type = "Class",
                        Namespace = namespaces,
                        HideCheckbox = false,
                    });

                }
            }
            else
            {
                var methods = GetTypesInNamespace(namespaces);
                foreach (var a in methods.Where(x => x.MemberType != MemberTypes.TypeInfo && !x.Name.Contains("<>")).ToList())
                {
                    var isSelected = false;
                    var name = a.Name.Split("<")[1].Split(">")[0];

                    if (name == methodName)
                    {
                        isSelected = true;
                    }
                    model.Add(new TreeViewViewModel { HideCheckbox = true, id = name, Type = "Methods", Name = name, ParentId = parentId, Checked = isSelected, hasChildren = false, Namespace = namespaces + "." + id });
                }
            }
            return model.Distinct().ToList();
        }

        public List<string> AllNameSpace()
        {
            try
            {
                return Assembly.GetExecutingAssembly().GetTypes().Select(x => x.Namespace).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private List<Type> GetTypesInNamespace(string nameSpace)
        {
            return
              Assembly.GetExecutingAssembly().GetTypes()
                      .Where(t => String.Equals(t.Namespace, nameSpace))
                      .ToList();
        }

        public async Task<bool> CopyBusinessRuleModelForLogics(List<BusinessRuleModelViewModel> oldBRModelList, List<dynamic> LogicIds)
        {
            //var _businessRuleModelBusiness = _serviceProvider.GetService<IBusinessRuleModelBusiness>();
            if (oldBRModelList.IsNotNull())
            {
                List<BusinessRuleModelViewModel> parentList = new();
                parentList = oldBRModelList.Where(x => x.BusinessRuleType == BusinessRuleTypeEnum.Logical).ToList();
                List<dynamic> parentIdList = new();
                foreach (var item in parentList)
                {
                    var idx = LogicIds.FindIndex(x => x.OldId == item.BusinessRuleNodeId);
                    string newSkipLogicId = LogicIds[idx].NewId.ToString();
                    BusinessRuleModelViewModel model = new();
                    model = _autoMapper.Map<BusinessRuleModelViewModel>(item);
                    model.Id = null;
                    model.BusinessRuleNodeId = newSkipLogicId;

                    var result = await Create(model);
                    if (result.IsSuccess)
                    {
                        var x = new { OldBRMId = item.Id, NewBRMId = result.Item.Id };
                        parentIdList.Add(x);
                        List<BusinessRuleModelViewModel> childList = new();
                        childList = oldBRModelList.Where(x => x.BusinessRuleType == BusinessRuleTypeEnum.Operational && x.ParentId == item.Id).ToList();
                        foreach (var child in childList)
                        {

                            BusinessRuleModelViewModel childModel = new();
                            childModel = _autoMapper.Map<BusinessRuleModelViewModel>(child);
                            childModel.Id = null;
                            childModel.BusinessRuleNodeId = newSkipLogicId;
                            childModel.ReferenceId = newSkipLogicId;
                            childModel.ParentId = result.Item.Id;

                            var childRes = await Create(childModel);
                            if (!childRes.IsSuccess)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }

                }
                // Updating parent id for sub parents under root
                var subParentList = oldBRModelList.Where(x => x.BusinessRuleType == BusinessRuleTypeEnum.Logical && x.ParentId != null).ToList();
                foreach (var item in subParentList)
                {
                    var idx = parentIdList.FindIndex(x => x.OldBRMId == item.Id);
                    string newId = parentIdList[idx].NewBRMId.ToString();
                    var subParentModel = await _repo.GetSingleById(newId);
                    var i = parentIdList.FindIndex(x => x.OldBRMId == item.ParentId);
                    subParentModel.ParentId = parentIdList[i].NewBRMId.ToString();
                    await Edit(subParentModel);
                }
                return true;
            }
            return true;
        }

        public async Task<bool> CopyBusinessRuleModelForDecisionScript(List<BusinessRuleModelViewModel> oldList, List<dynamic> decisionScriptCompIds)
        {
            //var _businessRuleModelBusiness = _serviceProvider.GetService<IBusinessRuleModelBusiness>();
            if (oldList.IsNotNull())
            {
                List<BusinessRuleModelViewModel> parentList = new();
                parentList = oldList.Where(x => x.BusinessRuleType == BusinessRuleTypeEnum.Logical).ToList();
                List<dynamic> parentIdList = new();
                foreach (var item in parentList)
                {
                    BusinessRuleModelViewModel model = new();
                    var idx = decisionScriptCompIds.FindIndex(x => x.OldId == item.DecisionScriptComponentId);
                    string newDSCId = decisionScriptCompIds[idx].NewId.ToString();
                    CommandResult<BusinessRuleModelViewModel> res;
                    //if (item.ParentId != null)
                    //{
                    // Create
                    model = _autoMapper.Map<BusinessRuleModelViewModel>(item);
                    model.Id = null;
                    model.DecisionScriptComponentId = newDSCId;
                    res = await Create(model);
                    //}
                    //else
                    //{
                    // Edit
                    //string BRMId = decisionScriptCompIds[idx].BRMRootId.ToString();
                    //model = await _businessRuleModelBusiness.GetSingleById(BRMId);
                    //var newId = model.Id;
                    //model = _autoMapper.Map<BusinessRuleModelViewModel>(item);
                    //model.Id = newId;
                    //model.DecisionScriptComponentId = newDSCId;
                    //res = await _businessRuleModelBusiness.Edit(model);

                    //}

                    if (res.IsSuccess)
                    {
                        var x = new { OldBRMId = item.Id, NewBRMId = res.Item.Id };
                        parentIdList.Add(x);
                        List<BusinessRuleModelViewModel> childList = new();
                        childList = oldList.Where(x => x.BusinessRuleType == BusinessRuleTypeEnum.Operational && x.ParentId == item.Id).ToList();
                        foreach (var child in childList)
                        {

                            BusinessRuleModelViewModel childModel = new();
                            childModel = _autoMapper.Map<BusinessRuleModelViewModel>(child);
                            childModel.Id = null;
                            childModel.ParentId = res.Item.Id;
                            childModel.DecisionScriptComponentId = newDSCId;

                            var childRes = await Create(childModel);
                            if (!childRes.IsSuccess)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                // Updating parent id for sub parents under root
                var subParentList = oldList.Where(x => x.BusinessRuleType == BusinessRuleTypeEnum.Logical && x.ParentId != null).ToList();
                foreach (var item in subParentList)
                {
                    // fetch entry
                    var idx = parentIdList.FindIndex(x => x.OldBRMId == item.Id);
                    string newId = parentIdList[idx].NewBRMId.ToString();

                    var subParentModel = await _repo.GetSingleById(newId);

                    // fetch parent id
                    var i = parentIdList.FindIndex(x => x.OldBRMId == item.ParentId);
                    subParentModel.ParentId = parentIdList[i].NewBRMId.ToString();

                    await Edit(subParentModel);
                }
                return true;
            }
            return true;
        }

    }
}
