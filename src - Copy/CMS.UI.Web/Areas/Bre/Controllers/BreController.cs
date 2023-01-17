using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CMS.Business;
using CMS.Common;
using CMS.Data.Model;
using CMS.UI.Utility;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CMS.UI.Web.Controllers
{
    [Area("Bre")]
    public class BreController : ApplicationController
    {
        private readonly IBreMasterMetadataBusiness _breMasterMetadataBusiness;
        IBusinessRuleModelBusiness _ruleBusiness;
        IBreMasterMetadataBusiness _masterMetaBusiness;
        private readonly IBreMetadataBusiness _breMetadataBusiness;
        private readonly IBusinessRuleBusiness _breRuleBusiness;
        private readonly IBusinessRuleModelBusiness _breRuleModelBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly IColumnMetadataBusiness _columnMetadataBusiness;
        IBreResultBusiness _breResultBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly IUserContext _userContext;
        private readonly IBreMasterColumnMetadataBusiness _masterColumnMetadataBusiness;
        private readonly IBreMasterTableMetadataBusiness _masterTableMetadataBusiness;
        public BreController(IBreMasterMetadataBusiness breMasterMetadataBusiness,
            IBusinessRuleModelBusiness ruleBusiness,
            ITemplateBusiness templateBusiness,
            IBusinessRuleModelBusiness breRuleModelBusiness,
            IColumnMetadataBusiness columnMetadataBusiness,
            IBreMasterMetadataBusiness masterMetaBusiness,
            IBreMetadataBusiness breMetadataBusiness,
            IBusinessRuleBusiness breRuleBusiness
            , IBreResultBusiness breResultBusiness,
            ITableMetadataBusiness tableMetadataBusiness
            , IUserContext userContext
            , IBreMasterColumnMetadataBusiness masterColumnMetadataBusiness
            , IBreMasterTableMetadataBusiness masterTableMetadataBusiness)
        {
            _breMasterMetadataBusiness = breMasterMetadataBusiness;
            _ruleBusiness = ruleBusiness;
            _templateBusiness = templateBusiness;
            _columnMetadataBusiness = columnMetadataBusiness;
            _masterMetaBusiness = masterMetaBusiness;
            _breMetadataBusiness = breMetadataBusiness;
            _breRuleModelBusiness = breRuleModelBusiness;
            _breRuleBusiness = breRuleBusiness;
            _breResultBusiness = breResultBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _userContext = userContext;
            _masterColumnMetadataBusiness = masterColumnMetadataBusiness;
            _masterTableMetadataBusiness = masterTableMetadataBusiness;
        }
        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetBusinessRuleModelListNew(string id, string nodeId, string decisionScriptComponentId, bool? isWorkFlow)
        {

            var list = new List<BusinessRuleModelViewModel>();
            var data = new List<BusinessRuleModelViewModel>();
            if (isWorkFlow == true)
            {
                data = await _ruleBusiness.GetList(x => x.DecisionScriptComponentId == decisionScriptComponentId);
            }
            else
            {
                data = await _ruleBusiness.GetList(x => x.BusinessRuleNodeId == nodeId);
            }
            list = data;
            if (id == null)
            {
                //data = data.Where(x => x.ParentId == null).ToList();
                if (data.Count == 0)
                {

                    if (isWorkFlow == true)
                    {
                        await _ruleBusiness.Create(new BusinessRuleModelViewModel()
                        {
                            BusinessRuleType = BusinessRuleTypeEnum.Logical,
                            Condition = LogicalEnum.And,
                            DecisionScriptComponentId = decisionScriptComponentId.IsNotNullAndNotEmpty() ? decisionScriptComponentId : null,
                            BusinessRuleSource = BusinessRuleSourceEnum.BusinessRule
                        });
                        data = await _ruleBusiness.GetList(x => x.DecisionScriptComponentId == decisionScriptComponentId);
                    }
                    else
                    {
                        await _ruleBusiness.Create(new BusinessRuleModelViewModel()
                        {
                            BusinessRuleType = BusinessRuleTypeEnum.Logical,
                            Condition = LogicalEnum.And,
                            BusinessRuleNodeId = nodeId,
                            BusinessRuleSource = BusinessRuleSourceEnum.BusinessRule
                        });
                        data = await _ruleBusiness.GetList(x => x.BusinessRuleNodeId == nodeId);
                    }
                }
            }
            //else
            //{
            //    data = data.Where(x => x.ParentId == id).ToList();
            //}
            data.ForEach(x => x.HasChildren = list.Any(y => y.ParentId == x.Id));
            var result = data.Select(x => new
            {
                id = x.Id,
                Field = x.Field,
                Condition = x.Condition.ToString(),
                hasChildren = x.HasChildren,
                Value = x.Value,
                OperatorType = x.OperatorType.ToString(),
                expanded = true,
                ParentId = x.ParentId,
                FieldSourceTypeStr = x.FieldSourceType.ToString(),
                ValueSourceTypeStr = x.ValueSourceType.ToString(),
                DecisionScriptComponentId = x.DecisionScriptComponentId,
                OperationValue = x.OperationValue

            }).ToList();

            return Json(result.ToList());
        }
        public async Task<JsonResult> GetBusinessRuleModelList(string id, string nodeId,string decisionScriptComponentId,bool? isWorkFlow)
        {
            //var data = await _ruleBusiness.GetList();
            //data = data.Where(x => x.DecisionNodeId == nodeId).ToList();
            //if (data.Count == 0)
            //{
            //    var temp = new BusinessRuleModelViewModel { DecisionNodeId = nodeId, Condition = LogicalEnum.And };
            //    await _ruleBusiness.Create(temp);
            //    data.Add(temp);
            //}
            //data.ForEach(x => x.HasChildren = data.Any(y => y.ParentId == x.Id));

            //var result = data.Select(x => new
            //{
            //    id = x.Id,
            //    Field = x.Field,
            //    Condition = x.Condition.ToString(),
            //    hasChildren = x.HasChildren,
            //    BusinessRuleType = x.BusinessRuleType.ToString(),
            //    Value = x.Value,
            //    OperatorType = x.OperatorType.ToString(),
            //    expanded = true,
            //    IsConditionChecked = x.Condition == LogicalEnum.And ? "checked" : ""

            //}).ToList();

            var list = new List<BusinessRuleModelViewModel>();
            var data = new List<BusinessRuleModelViewModel>();
            //var data = await _ruleBusiness.GetBusinessRuleModelList(nodeId);
            if (isWorkFlow == true)
            {
                data = await _ruleBusiness.GetList(x => x.DecisionScriptComponentId == decisionScriptComponentId);
            }            
            else
            {
                data = await _ruleBusiness.GetList(x => x.BusinessRuleNodeId == nodeId);
            }
            list = data;
            if (id == null)
            {
                data = data.Where(x => x.ParentId == null).ToList();
                if (data.Count == 0)
                {
                  
                    if (isWorkFlow == true)
                    {
                        await _ruleBusiness.Create(new BusinessRuleModelViewModel()
                        {
                            BusinessRuleType = BusinessRuleTypeEnum.Logical,
                            Condition = LogicalEnum.And,                          
                            DecisionScriptComponentId = decisionScriptComponentId.IsNotNullAndNotEmpty() ? decisionScriptComponentId : null,
                            BusinessRuleSource = BusinessRuleSourceEnum.BusinessRule
                        });
                        data = await _ruleBusiness.GetList(x => x.DecisionScriptComponentId == decisionScriptComponentId);
                    }
                    else 
                    {
                        await _ruleBusiness.Create(new BusinessRuleModelViewModel()
                        {
                            BusinessRuleType = BusinessRuleTypeEnum.Logical,
                            Condition = LogicalEnum.And,
                            BusinessRuleNodeId = nodeId,
                            BusinessRuleSource = BusinessRuleSourceEnum.BusinessRule
                        });
                        data = await _ruleBusiness.GetList(x => x.BusinessRuleNodeId == nodeId);
                    }
                }
            }
            else
            {
                data = data.Where(x => x.ParentId == id).ToList();
            }
            data.ForEach(x => x.HasChildren = list.Any(y => y.ParentId == x.Id));
            var result = data.Select(x => new
            {
                id = x.Id,
                Field = x.Field,
                Condition = x.Condition.ToString(),
                hasChildren = x.HasChildren,
                Value = x.Value,
                OperatorType = x.OperatorType.ToString(),
                expanded = true,
                ParentId = x.ParentId,
                FieldSourceTypeStr=x.FieldSourceType.ToString(),
                ValueSourceTypeStr = x.ValueSourceType.ToString(),
                DecisionScriptComponentId=x.DecisionScriptComponentId,
                OperationValue=x.OperationValue

            }).ToList();

            return Json(result.ToList());
        }
        // GET: BreController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        public async Task<ActionResult> BreMasterData()
        {
            return View();
        }

        public async Task<JsonResult> GetBreMasterDataTreeList(string id)
        {
            var model = await _breMasterMetadataBusiness.GetBreMasterDataTreeList(id);
            var result = model.Select(x => new
            {
                id = x.Name,
                Name = x.DisplayName,
                hasChildren = x.HasSubFolders,

            }).ToList();
            return Json(result.ToList());
        }
        // GET: BreController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BreController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BreController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BreController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BreController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BreController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<IActionResult> GetMasterMetaData()
        {
            var data = await _masterMetaBusiness.GetAllBreMasterMetaData();
            return Json(data);
        }

        public async Task<IActionResult> GetBusinessRule([DataSourceRequest] DataSourceRequest request, string nodeId)
        {
            var data = await _ruleBusiness.GetList();
            data = data.Where(x => x.BusinessRuleNodeId == nodeId).ToList();
            if (data.Count == 0)
            {
                var temp = new BusinessRuleModelViewModel { BusinessRuleNodeId = nodeId, Condition = LogicalEnum.And };
                await _ruleBusiness.Create(temp);
                data.Add(temp);
            }
            var j = Json(data.ToTreeDataSourceResult(request));
            return j;

        }

        public async Task<IActionResult> UpdateBusinessRule(string nodeId, string ruleId, string id, string field, string value, EqualityOperationEnum operatorType, string parentId, LogicalEnum? condition)
        {
            if (id == null || id == "")
            {
                var model = new BusinessRuleModelViewModel
                {
                    BusinessRuleNodeId = nodeId,
                    Field = field,
                    Value = value,
                    OperatorType = operatorType,
                    ParentId = parentId,
                    Condition = condition,
                };
                await _ruleBusiness.Create(model);

                if (condition != null)
                {
                    var temp = new BusinessRuleModelViewModel { BusinessRuleNodeId = nodeId, ParentId = model.Id };
                    await _ruleBusiness.Create(temp);
                }
            }
            else
            {
                var data = await _ruleBusiness.GetSingleById(id);
                data.Field = field;
                data.Value = value;
                data.OperatorType = operatorType;
                data.Condition = condition;
                await _ruleBusiness.Edit(data);
            }

            var res = await _ruleBusiness.GetList();
            return Json(res);
        }

        public async Task<IActionResult> DeleteRule(string id)
        {
            await _ruleBusiness.Delete(id);
            return Json(new { success = true });
        }

        public async Task<IActionResult> ChangeCondition(string id)
        {
            var data = await _ruleBusiness.GetSingleById(id);
            if (data.Condition == LogicalEnum.And)
            {
                data.Condition = LogicalEnum.Or;
            }
            else
            {
                data.Condition = LogicalEnum.And;
            }
            await _ruleBusiness.Edit(data);
            return Json(new { success = true });
        }

        public async Task<IActionResult> BreInputData(string ruleId,string templateId, string id)
        {
            //var data = await _breMetadataBusiness.GetBreMetaData(ruleId, id);
            var data = new List<TreeViewViewModel>();
            if (templateId != null)
            {
               
                if (id != null)
                {
                   
                    if (id == "Input")
                    {
                        data.Add(new TreeViewViewModel
                        {
                            expanded = true,
                            id = "Data",
                            Name = "Data",
                            Type = "Parent",
                            ParentId = id,
                            // BreInputDataType = BreInputDataTypeEnum.Root,
                            hasChildren = true,
                            // DataType = DataTypeEnum.Object,
                        });
                        data.Add(new TreeViewViewModel
                        {
                            expanded = true,
                            id = "Context",
                            Name = "Context",
                            Type = "Parent",
                            ParentId = id,
                            // BreInputDataType = BreInputDataTypeEnum.Root,
                            hasChildren = true,
                            // DataType = DataTypeEnum.Object,
                        });
                    }

                   else if (id == "Data")
                    {
                        var coldata = new List<ColumnMetadataViewModel>();
                        var template = await _templateBusiness.GetSingleById(templateId);
                        var tablemetaId = "";
                        if (template.UdfTableMetadataId.IsNotNullAndNotEmpty())
                        {
                            tablemetaId = template.UdfTableMetadataId;
                        }
                        else if (template.TableMetadataId.IsNotNullAndNotEmpty())
                        {
                            tablemetaId = template.TableMetadataId;
                        }
                        if (template.IsNotNull() && tablemetaId.IsNotNullAndNotEmpty())
                        {
                            coldata = await _columnMetadataBusiness.GetViewableColumnMetadataList(tablemetaId, template.TemplateType);

                        }
                        data.AddRange(coldata.Select(x => new TreeViewViewModel
                        {
                            id = x.Id,
                            Name = x.Alias,
                            hasChildren = false,
                            Type = "Property",
                            //ItemType = x.ty.ToString(),
                            //DataType = DataTypeEnum.Object,
                            FieldDataType=x.DataType,
                            ParentId = id,
                            //IconCss = x.IconCss,
                            //IsCollection = x.IsCollection,
                            expanded = true
                        }));
                    }
                    else if (id == "Context")
                    {
                        PropertyInfo[] properties = _userContext.GetType().GetProperties();

                        foreach (PropertyInfo property in properties)
                        {
                            //var attribute = Attribute.GetCustomAttribute(property, typeof(KeyAttribute))
                            //    as KeyAttribute;
                            data.Add( new TreeViewViewModel
                            {
                                id = property.Name,
                                Name = property.Name,
                                hasChildren = false,
                                Type = "Property",
                                //ItemType = x.ty.ToString(),
                                // DataType = DataTypeEnum.Object,
                                ParentId = id,
                                //IconCss = x.IconCss,
                                //IsCollection = x.IsCollection,
                                expanded = false
                            });

                        }
                        
                    }

                }
                else
                {
                    data.Add(new TreeViewViewModel
                    {
                        expanded = true,
                        id = "Input",
                        Name = "Input",
                        Type = "Root",
                        ParentId = null,
                        // BreInputDataType = BreInputDataTypeEnum.Root,
                        hasChildren = true,
                        // DataType = DataTypeEnum.Object,
                    });
                   
                }

            }
            data.Where(x => x.ParentId == id).ToList();
            
            return Json(data);
        }

        public async Task<object> BreInputDataFancyTree(string ruleId, string templateId, string id)
        {
            //var data = await _breMetadataBusiness.GetBreMetaData(ruleId, id);
            var data = new List<TreeViewViewModel>();
            if (templateId != null)
            {

                if (id != null)
                {

                    if (id == "Input")
                    {
                        data.Add(new TreeViewViewModel
                        {
                            expanded = true,
                            id = "Data",
                            Name = "Data",
                            Type = "Parent",
                            ParentId = id,
                            // BreInputDataType = BreInputDataTypeEnum.Root,
                            hasChildren = true,
                            // DataType = DataTypeEnum.Object,
                        });
                        data.Add(new TreeViewViewModel
                        {
                            expanded = true,
                            id = "Context",
                            Name = "Context",
                            Type = "Parent",
                            ParentId = id,
                            // BreInputDataType = BreInputDataTypeEnum.Root,
                            hasChildren = true,
                            // DataType = DataTypeEnum.Object,
                        });
                    }

                    else if (id == "Data")
                    {
                        var coldata = new List<ColumnMetadataViewModel>();
                        var template = await _templateBusiness.GetSingleById(templateId);
                        var tablemetaId = "";
                        if (template.UdfTableMetadataId.IsNotNullAndNotEmpty())
                        {
                            tablemetaId = template.UdfTableMetadataId;
                        }
                        else if (template.TableMetadataId.IsNotNullAndNotEmpty())
                        {
                            tablemetaId = template.TableMetadataId;
                        }
                        if (template.IsNotNull() && tablemetaId.IsNotNullAndNotEmpty())
                        {
                            coldata = await _columnMetadataBusiness.GetViewableColumnMetadataList(tablemetaId, template.TemplateType);

                        }
                        data.AddRange(coldata.Select(x => new TreeViewViewModel
                        {
                            id = x.Id,
                            Name = x.Alias,
                            hasChildren = false,
                            Type = "Property",
                            //ItemType = x.ty.ToString(),
                            //DataType = DataTypeEnum.Object,
                            FieldDataType = x.DataType,
                            ParentId = id,
                            //IconCss = x.IconCss,
                            //IsCollection = x.IsCollection,
                            expanded = true
                        }));
                    }
                    else if (id == "Context")
                    {
                        PropertyInfo[] properties = _userContext.GetType().GetProperties();

                        foreach (PropertyInfo property in properties)
                        {
                            //var attribute = Attribute.GetCustomAttribute(property, typeof(KeyAttribute))
                            //    as KeyAttribute;
                            data.Add(new TreeViewViewModel
                            {
                                id = property.Name,
                                Name = property.Name,
                                hasChildren = false,
                                Type = "Property",
                                //ItemType = x.ty.ToString(),
                                // DataType = DataTypeEnum.Object,
                                ParentId = id,
                                //IconCss = x.IconCss,
                                //IsCollection = x.IsCollection,
                                expanded = false
                            });

                        }

                    }

                }
                else
                {
                    data.Add(new TreeViewViewModel
                    {
                        expanded = true,
                        id = "Input",
                        Name = "Input",
                        Type = "Root",
                        ParentId = null,
                        // BreInputDataType = BreInputDataTypeEnum.Root,
                        hasChildren = true,
                        // DataType = DataTypeEnum.Object,
                    });

                }

            }
            data.Where(x => x.ParentId == id).ToList();

            var newList = new List<FileExplorerViewModel>();
            newList.AddRange(data.ToList().Select(x => new FileExplorerViewModel { key = x.id, title = x.Name, lazy = true,
                FieldDataType = x.FieldDataType.ToString(),ParentId = id
            }));
            var json = JsonConvert.SerializeObject(newList);
            return json;

            //   return Json(data);
        }
        public async Task<JsonResult> BreMasterTreeData(string ruleId, string id)
        {
            var data = await _breMasterMetadataBusiness.GetBreMasterMetaData(ruleId,id);
            var result = data.Select(x => new
            {
                id = x.Id,
                Name = x.Name,
                hasChildren = x.HasSubFolders,
                ItemType = x.BreInputDataType.ToString(),
                DataType = x.DataType.ToString(),
                ParentId = x.ParentId,
                ParentName=x.ParentName,
                IconCss = x.IconCss,
                expanded = false,
                FieldDataType = x.DataType,
            }).ToList();
            return Json(result);
        }

        public async Task<object> BreMasterFancyTreeData(string ruleId, string id)
        {
            var data = await _breMasterMetadataBusiness.GetBreMasterMetaData(ruleId,id);
            //var result = data.Select(x => new
            //{
            //    id = x.Id,
            //    Name = x.Name,
            //    hasChildren = x.HasSubFolders,
            //    ItemType = x.BreInputDataType.ToString(),
            //    DataType = x.DataType.ToString(),
            //    ParentId = x.ParentId,
            //    ParentName=x.ParentName,
            //    IconCss = x.IconCss,
            //    expanded = false,
            //    FieldDataType = x.DataType,
            //}).ToList();

            var newList = new List<FileExplorerViewModel>();
            newList.AddRange(data.ToList().Select(x => new FileExplorerViewModel
            {
                key = x.Id,
                title = x.Name,
                lazy = true,
                ItemType = x.BreInputDataType.ToString(),
                ParentId = x.ParentId,
                ParentName = x.ParentName,
                FieldDataType = x.DataType.ToString(),
            }));
            var json = JsonConvert.SerializeObject(newList);
            return json;
        }

        public async Task<IActionResult> BreTableData( string tableMetadataId, string id, TemplateTypeEnum templateType)
        {
            //var data = await _breMetadataBusiness.GetBreMetaData(ruleId, id);
            var data = new List<TreeViewViewModel>();
            if (id == "Data")
            {
                if (tableMetadataId != null)
                {
                    var column = await _columnMetadataBusiness.GetViewableColumnMetadataList(tableMetadataId,templateType);
                    if (column.Count>0)
                    {
                        data.AddRange(column.Select(x => new TreeViewViewModel
                        {
                            id = x.Id,
                            Name = x.Alias,
                            hasChildren = false,
                            Type = "Property",
                            //ItemType = x.ty.ToString(),
                            // DataType = DataTypeEnum.Object,
                            ParentId = id,
                            //IconCss = x.IconCss,
                            //IsCollection = x.IsCollection,
                            expanded = true
                        }));
                    }
                }
            }
            else if(id == "Input")
            {
                data.Add(new TreeViewViewModel
                {
                    expanded = true,
                    id = "Data",
                    Name = "Data",
                    Type = "Parent",
                    ParentId = id,
                    // BreInputDataType = BreInputDataTypeEnum.Root,
                    hasChildren = true,
                    // DataType = DataTypeEnum.Object,
                });

            }
            else
            {
                data.Add(new TreeViewViewModel
                {
                    expanded = true,
                    id = "Input",
                    Name = "TableMeta",
                    Type = "Root",
                    ParentId = null,
                    // BreInputDataType = BreInputDataTypeEnum.Root,
                    hasChildren = true,
                    // DataType = DataTypeEnum.Object,
                });

            }
           
            data.Where(x => x.ParentId == id).ToList();

            return Json(data);
        }


        public async Task<IActionResult> ManageBusinessRuleInputData(string ruleId, string parentId, string id)
        {
            var model = new BreMetadataViewModel();
            if (id != null && id != "")
            {
                model = await _breMetadataBusiness.GetSingleById(id);
                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
                model.BusinessRuleId = ruleId;
            }
            model.ParentId = parentId;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageBusinessRuleInputData(BreMetadataViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _breMetadataBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        return PopupRedirect("Input data created successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _breMetadataBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        return PopupRedirect("Input data edited successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteMasterData(string bussinessRuleId, string id)
        {
            var Allrecords = await _breMetadataBusiness.GetList(x => x.BusinessRuleId == bussinessRuleId);
            var inputData = await _breMetadataBusiness.GetSingleById(id);
            await _breMasterMetadataBusiness.Delete(inputData.Id);
            //var childList = Allrecords.Where(x => x.ParentId == inputData.Id).ToList();
            //var Detail = new List<string>();
            //Detail.Add(inputData.Id);
            //Detail = getChilds(Allrecords, childList, Detail);          
            //foreach (var d in Detail)
            //{
            //    await _breMasterMetadataBusiness.Delete(d);
            //}
            return Json(new { success = true });
        }
        public async Task<IActionResult> DeleteMasterColumnData(string id)
        {
            //var columnMetaData = await _masterColumnMetadataBusiness.GetSingleById(id); 
            var rulemodel = await _breRuleModelBusiness.GetList(x => x.FieldId == id || x.ValueId == id);
            if (rulemodel.IsNotNull() && rulemodel.Count > 0)
            {
                foreach (var item in rulemodel)
                {
                    await _breRuleModelBusiness.Delete(item.Id);
                }
            }
            await _masterColumnMetadataBusiness.Delete(id);
           
            return Json(new { success = true });
        }
        public async Task<IActionResult> DeleteMasterTableData(string bussinessRuleId, string id)
        {
            var tableMetaData = await _masterTableMetadataBusiness.GetSingle(x=>x.BusinessRuleId==bussinessRuleId && x.Id==id);
            if (tableMetaData.IsNotNull())
            {
                var columnMetaDatalist = await _masterColumnMetadataBusiness.GetList(x => x.BreMasterTableMetadataId == tableMetaData.Id);
                foreach (var columnMetaData in columnMetaDatalist)
                {
                  
                    var rulemodel = await _breRuleModelBusiness.GetList(x => x.FieldId == columnMetaData.Id || x.ValueId == columnMetaData.Id);
                    if (rulemodel.IsNotNull() && rulemodel.Count > 0)
                    {
                        foreach (var item in rulemodel)
                        {
                            await _breRuleModelBusiness.Delete(item.Id);
                        }
                    }
                    await _masterColumnMetadataBusiness.Delete(columnMetaData.Id);
                }
               
                await _masterTableMetadataBusiness.Delete(tableMetaData.Id);
            }
            

            return Json(new { success = true });
        }
        public async Task<IActionResult> DeleteInputData(string bussinessRuleId, string id)
        {
            var Allrecords = await _breMetadataBusiness.GetList(x => x.BusinessRuleId == bussinessRuleId);
            var inputData = await _breMetadataBusiness.GetSingleById(id);
            var childList = Allrecords.Where(x => x.ParentId == inputData.Id).ToList();
            var Detail = new List<string>();
            Detail.Add(inputData.Id);
            Detail = getChilds(Allrecords, childList, Detail);
            // Detail.AddRange(childid);
            foreach (var d in Detail)
            {
                await _breMetadataBusiness.Delete(d);
            }
            return Json(new { success = true });
        }

        public List<string> getChilds(List<BreMetadataViewModel> total, List<BreMetadataViewModel> childList, List<string> ids)
        {
            foreach (var child in childList)
            {
                ids.Add(child.Id);
                if (total.Any(x => x.ParentId == child.Id))
                {
                    var grandchildList = total.Where(x => x.ParentId == child.Id).ToList();
                    getChilds(total, grandchildList, ids);
                }
            }
            return ids;
        }

        public async Task<IActionResult> BreResultViewDetails(string businessRuleNodeId,string templateId,string parentId,string ruleId,bool? isBusinessDiagram)
        {
            var model = new BreResultViewModel();
            if (businessRuleNodeId.IsNotNullAndNotEmpty())
            {               
                model.BusinessRuleNodeId = businessRuleNodeId;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
                model.BusinessRuleNodeId = Guid.NewGuid().ToString() ;
            }
            
           var templateDetails = await _templateBusiness.GetSingleById(templateId);
            if(templateDetails.IsNotNull())
            {
                model.TemplateTypeText = templateDetails.TemplateType.ToString();
            }

            var detailModel = new List<IdNameViewModel>();
            if (businessRuleNodeId != null && businessRuleNodeId != "")
            {
                var modelData = await _breResultBusiness.GetSingle(x => x.BusinessRuleNodeId == businessRuleNodeId);
                if (modelData != null)
                {
                    model = modelData;
                    model.TemplateTypeText = templateDetails.TemplateType.ToString();
                    model.MethodReturn = model.MethodReturnValue == true ? TrueOrFalseEnum.True : TrueOrFalseEnum.False;
                    foreach (var m in model.Message)
                    {
                        detailModel.Add(new IdNameViewModel { Name = m });
                    }
                }
            }
            ViewBag.DataSource = detailModel;
            ViewBag.IsBusinessDiagram = isBusinessDiagram;
            model.NodeParentId = parentId;
            model.BusinessRuleId = ruleId;
            model.TemplateId = templateId;
            var node = await _breResultBusiness.GetSingleById<BusinessRuleNodeViewModel, BusinessRuleNode>(businessRuleNodeId);
            if (node != null)
            {
                model.ProcessName = node.Name;
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> BreResultViewDetails(BreResultViewModel model)
        {
            //if (model.Detail != null)
            //{
            //    var detailData = JsonConvert.DeserializeObject<List<IdNameViewModel>>(model.Detail);
            //    var detailItems = new Dictionary<string, string>();
            //    foreach (var item in detailData)
            //    {
            //        detailItems.Add(item.Id, item.Name);
            //    }
            //    // model.Detail = detailItems;
            //}

            if (ModelState.IsValid)
            {

                if (model.Id.IsNullOrEmpty())
                {
                    var result = await _breResultBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        return Json(new { sucess = true, item = result.Item});
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }

                }
                else
                {
                    var result = await _breResultBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        return Json(new { sucess = true, item = result.Item });
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }

                }
            }
            return View(model);
        }

        public async Task<JsonResult> Destroy([DataSourceRequest] DataSourceRequest request, BusinessRuleModelViewModel rule)
        {
            if (ModelState.IsValid)
            {
                //employeeDirectory.Delete(employee, ModelState);
                await _ruleBusiness.Delete(rule.Id);
            }

            return Json(new[] { rule }.ToTreeDataSourceResult(request, ModelState));
        }

        public async Task<JsonResult> Create([DataSourceRequest] DataSourceRequest request, BusinessRuleModelViewModel rule)
        {
            if (ModelState.IsValid)
            {
                await _ruleBusiness.Create(rule);
            }

            return Json(new[] { rule }.ToTreeDataSourceResult(request, ModelState));
        }

        public async Task<JsonResult> Update([DataSourceRequest] DataSourceRequest request, BusinessRuleModelViewModel rule)
        {
            if (ModelState.IsValid)
            {
                await _ruleBusiness.Edit(rule);
            }

            return Json(new[] { rule }.ToTreeDataSourceResult(request, ModelState));
        }
        public async Task<ActionResult> AddBreMasterMetaData(string ruleId, string parentId, string id,string templateId)
        {
            //BreMasterMetadataViewModel model = new BreMasterMetadataViewModel();
            BreMasterTableMetadataViewModel model = new BreMasterTableMetadataViewModel();
            if (id != null)
            {
                //model = await _breMasterMetadataBusiness.GetSingleById(id);
                model = await _masterTableMetadataBusiness.GetSingle(x=>x.Id==id && x.BusinessRuleId==ruleId);
                if (model.IsNotNull())
                {
                    var table=await _tableMetadataBusiness.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == model.TableMetadataId);
                    model.DataAction = DataActionEnum.Edit;
                    model.TemplateType = table.TemplateType;
                }
               
            }
            else
            {
                model.Id = Guid.NewGuid().ToString();
                model.BusinessRuleId = ruleId;
                model.ParentId = parentId;
                model.DataAction = DataActionEnum.Create;
            }
            ViewBag.templateId = templateId;
            return View(model);
        }

        public async Task<ActionResult> GetPropertiesFromMasterDataCollection(string tableMetaDateId, TemplateTypeEnum templateType,string ruleId)
        {
            List<BreMasterColumnMetadataViewModel> columnMetaData = new List<BreMasterColumnMetadataViewModel>();
            var masterMetaData = await _masterTableMetadataBusiness.GetSingle(x=>x.BusinessRuleId==ruleId&&x.TableMetadataId==tableMetaDateId);
            if (masterMetaData.IsNotNull())
            {
              columnMetaData = await _masterColumnMetadataBusiness.GetList<BreMasterColumnMetadataViewModel, BreMasterColumnMetadata>(x => x.BreMasterTableMetadataId == masterMetaData.Id);

            }
            var data = await _columnMetadataBusiness.GetViewableColumnMetadataList(tableMetaDateId, templateType);
            if (columnMetaData.Count > 0)
            {
                //data.ForEach(x => x.IsChecked = columnMetaData.Any(y => y.ColumnMetadataId == x.Id));
                data.ForEach(x => x.IsChecked = columnMetaData.Any(y => y.Alias == x.Alias));
            }
            return Json(data/*.ToDataSourceResult(request)*/);
        }
        public async Task<JsonResult> GetMasterMetaDataColletionList(string id)
        {
            //var model = await _breMasterMetadataBusiness.GetMasterDataCollectionList();  
            var list = new List<TableMetadataViewModel>();
            if (id.IsNullOrEmpty())
            {
                list = await _tableMetadataBusiness.GetList<TableMetadataViewModel, TableMetadata>(x => x.CompanyId == _userContext.CompanyId);
            }
            else
            {
                list = await _tableMetadataBusiness.GetList<TableMetadataViewModel, TableMetadata>(x => x.CompanyId == _userContext.CompanyId && x.Id==id);
            }
            
            return Json(list);
        }

        [HttpPost]
        public async Task<IActionResult> ManageBusinessRuleMasterData(BreMasterMetadataViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _breMasterMetadataBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        return PopupRedirect("Master data created successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _breMasterMetadataBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        return PopupRedirect("Master data edited successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageMasterColumnMetaData(BreMasterColumnMetadataViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var columnMetaData = await _masterColumnMetadataBusiness.GetList<BreMasterColumnMetadataViewModel, BreMasterColumnMetadata>(x => x.ColumnMetadataId == model.ColumnMetadataId);
                    if (columnMetaData.Count > 0)
                    {
                        return PopupRedirect("Master data Already Exist");
                    }
                    var result = await _masterColumnMetadataBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        return PopupRedirect("Master data created successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _masterColumnMetadataBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        return PopupRedirect("Master data edited successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageMasterTableMetaData(BreMasterTableMetadataViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _masterTableMetadataBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true, result = result.Item });
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _masterTableMetadataBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true, result = result.Item });
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return Json(new { success = false, error=ModelState.ToHtmlError() });
        }
   
        public async Task<JsonResult> GetMasterBusinessRuleModelList(string id, string masterId)
        {
            var list = new List<BusinessRuleModelViewModel>();
          //  var data = await _ruleBusiness.GetMasterBusinessRuleModelList(masterId);
            var data = await _ruleBusiness.GetList(x => x.BreMasterTableMetadataId == masterId);
            list = data;
            if (id == null)
            {
                data = data.Where(x => x.ParentId == null).ToList();
                if (data.Count == 0)
                {
                    await _ruleBusiness.Create(new BusinessRuleModelViewModel()
                    {
                        BusinessRuleType = BusinessRuleTypeEnum.Logical,
                        Condition = LogicalEnum.And,
                        BreMasterTableMetadataId = masterId,
                        BusinessRuleSource = BusinessRuleSourceEnum.MasterData
                    });
                    data = await _ruleBusiness.GetList(x => x.BreMasterTableMetadataId == masterId);
                }
            }
            else
            {
                data = data.Where(x => x.ParentId == id).ToList(); 
            }
            data.ForEach(x => x.HasChildren = list.Any(y => y.ParentId == x.Id));
            var result = data.Select(x => new
            {
                id = x.Id,
                Field = x.Field,
                Condition = x.Condition.ToString(),
                hasChildren = x.HasChildren,
                Value = x.Value,
                OperatorType = x.OperatorType.ToString(),
                expanded = true,
                ParentId = x.ParentId,
                FieldSourceTypeStr = x.FieldSourceType.ToString(),
                ValueSourceTypeStr = x.ValueSourceType.ToString(),
                OperationValue = x.OperationValue
            }).ToList();

            return Json(result.ToList());
        }

        public IActionResult FolderStructure()
        {
            return View("BusinessScriptFolder");
        }
        
        public async Task<IActionResult> GetFolderStructure(string id, string type, string parentId, string templateType, string namespaces, string methodName)
        {
            var result = await _ruleBusiness.ScanFolderNew(id, type, parentId, templateType, namespaces, methodName);
            return Json(result);
        }
        
        public async Task<object> GetFolderStructureFancyTree(string id, string type, string parentId, string templateType, string namespaces, string methodName)
        {
            var result = await _ruleBusiness.ScanFolderNew(id, type, parentId, templateType, namespaces, methodName);
            var newList = new List<FileExplorerViewModel>();
            newList.AddRange(result.ToList().Select(x => new FileExplorerViewModel { key = x.id, title = x.Name, lazy = true, type = x.Type,
                parentId = x.ParentId,
                namespaces = x.Namespace,
                methodName = methodName,
                templateType = templateType,
                checkbox = x.HideCheckbox,
            }));
            var json = JsonConvert.SerializeObject(newList);
            return json;
           // return Json(result);
        }

        public async Task<IActionResult> GetMethodParameters(string methodId, string namespaceString)
        {
            var result = await _ruleBusiness.GetMethodParamName(methodId, namespaceString);
            return Json(result);
        }

    }
}
