using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Business;
using CMS.Common;
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
    public class BreRuleModelController : ApplicationController
    {
        private readonly IBreMasterMetadataBusiness _breMasterMetadataBusiness;
        IBusinessRuleModelBusiness _ruleBusiness;
        IBreMasterMetadataBusiness _masterMetaBusiness;
        private readonly IBreMetadataBusiness _breMetadataBusiness;
        private readonly IBusinessRuleBusiness _breRuleBusiness;
        IBreResultBusiness _breResultBusiness;
        public BreRuleModelController(IBreMasterMetadataBusiness breMasterMetadataBusiness,
            IBusinessRuleModelBusiness ruleBusiness,
            IBreMasterMetadataBusiness masterMetaBusiness,
            IBreMetadataBusiness breMetadataBusiness,
            IBusinessRuleBusiness breRuleBusiness
            , IBreResultBusiness breResultBusiness)
        {
            _breMasterMetadataBusiness = breMasterMetadataBusiness;
            _ruleBusiness = ruleBusiness;
            _masterMetaBusiness = masterMetaBusiness;
            _breMetadataBusiness = breMetadataBusiness;
            _breRuleBusiness = breRuleBusiness;
            _breResultBusiness = breResultBusiness;
        }
        public ActionResult Index()
        {
            return View();
        }


        public async Task<ActionResult> AddMasterCondition(string masterId, string templateId, string parentId, DataActionEnum dataAction, LogicalEnum? condition, string ruleId, string id, string collectionId, TemplateTypeEnum templateType)
        {
            if (!id.IsNullOrEmpty())
            {
                var model = await _ruleBusiness.GetSingleById(id);
                model.DataAction = DataActionEnum.Edit;
                model.RuleId = ruleId;
                model.TableMetaId = collectionId;
                model.TemplateId = templateId;
                return View(model);
            }
            else
            {
                var model = new BusinessRuleModelViewModel
                {
                    BreMasterTableMetadataId = masterId,
                    ParentId = parentId,
                    DataAction = dataAction,
                    Condition = condition,
                    RuleId = ruleId,
                    TableMetaId = collectionId,
                    BusinessRuleType = BusinessRuleTypeEnum.Operational,
                    BusinessRuleSource = BusinessRuleSourceEnum.MasterData

                };
                // model.FieldId = collectionId;
                ViewBag.TemplateType = templateType;
                model.TemplateId = templateId;
                return View(model);
            }
        }
        public async Task<ActionResult> AddMasterGroup(string masterId, string parentId, DataActionEnum dataAction, LogicalEnum? condition, string ruleId, string id)
        {
            if (!id.IsNullOrEmpty())
            {
                var model = await _ruleBusiness.GetSingleById(id);
                model.DataAction = DataActionEnum.Edit;
                model.RuleId = ruleId;
                return View(model);
            }
            else
            {
                var model = new BusinessRuleModelViewModel
                {
                    //MasterMetaDataId = masterId,
                    ParentId = parentId,
                    DataAction = dataAction,
                    Condition = condition,
                    RuleId = ruleId,
                    BusinessRuleType = BusinessRuleTypeEnum.Logical,
                    BusinessRuleSource = BusinessRuleSourceEnum.MasterData
                };
                ViewBag.ruleId = ruleId;
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateBusinessRuleBuilderGroup(DataActionEnum dataAction, string tableMetaId, string parentId, LogicalEnum condition, string ruleId, string id)
        {
            if (dataAction == DataActionEnum.Create)
            {
                var model = new BusinessRuleModelViewModel { DataAction = dataAction, BreMasterTableMetadataId = tableMetaId, ParentId = parentId, Condition = condition, RuleId = ruleId, BusinessRuleType = BusinessRuleTypeEnum.Logical, BusinessRuleSource = BusinessRuleSourceEnum.MasterData };

                var result = await _ruleBusiness.Create(model);
                if (result.IsSuccess)
                {
                    //return PopupRedirect("Group created successfully");
                    return Json(new { success = true });
                }
                else
                {
                    ModelState.AddModelErrors(result.Messages);
                }
            }
            else if (dataAction == DataActionEnum.Edit)
            {
                var model = await _ruleBusiness.GetSingleById(id);
                if (model.Condition == LogicalEnum.And)
                {
                    model.Condition = LogicalEnum.Or;
                }
                else
                {
                    model.Condition = LogicalEnum.And;
                }
                //model.Condition = condition;
                model.DataAction = DataActionEnum.Edit;
                var result = await _ruleBusiness.Edit(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true });
                }
                else
                {
                    ModelState.AddModelErrors(result.Messages);
                }
            }

            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
        [HttpPost]
        public async Task<IActionResult> CreateBusinessRuleCondition(BusinessRuleModelViewModel model)
        {
            if (ModelState.IsValid)
            {
                var left = "";
                var leftB = "";
                var right = "";
                var symbol = "";
                var rightB = "";
                if (model.FieldSourceType == BreMetadataTypeEnum.InputData)
                {
                    left = "Input" + "." + model.ParentFieldId + "." + model.Field;
                    leftB = "$!Input" + "." + model.ParentFieldId + "." + model.Field;
                }
                else if (model.FieldSourceType == BreMetadataTypeEnum.Constant)
                {
                    left = model.Field;
                    leftB = model.Field;
                }

                if (model.ValueSourceType == BreMetadataTypeEnum.InputData)
                {
                    right = "Input" + "." + model.ParentValueId + "." + model.Value;
                    rightB = "$!Input" + "." + model.ParentValueId + "." + model.Value;
                }
                else if (model.ValueSourceType == BreMetadataTypeEnum.TableMeta)
                {
                    right = "TableMeta" + "." + model.ParentValueId + "." + model.Value;
                    rightB = "TableMeta" + "." + model.ParentValueId + "." + model.Value;
                }
                else if (model.ValueSourceType == BreMetadataTypeEnum.Constant)
                {
                    right = model.Value;
                    rightB = model.Value;
                }

                if (model.OperatorType == EqualityOperationEnum.Equal)
                {
                    symbol = "==";
                }
                else if (model.OperatorType == EqualityOperationEnum.GreaterThan)
                {
                    symbol = ">";
                }
                else if (model.OperatorType == EqualityOperationEnum.GreaterThanOrEqual)
                {
                    symbol = ">=";
                }
                else if (model.OperatorType == EqualityOperationEnum.LessThan)
                {
                    symbol = "<";
                }
                else if (model.OperatorType == EqualityOperationEnum.LessThanOrEqual)
                {
                    symbol = "<=";
                }
                else if (model.OperatorType == EqualityOperationEnum.NotEqual)
                {
                    symbol = "!=";
                }
                model.OperationValue = $"{left} {symbol} {right}";
                model.OperationBackendValue = $"{leftB} {symbol} {rightB}";
                if (model.DataAction == DataActionEnum.Create)
                {

                    var result = await _ruleBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _ruleBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return View("AddMasterCondition", model);
        }

        public async Task<IActionResult> RemoveGroup(string id)
        {
            var Allrecords = await _ruleBusiness.GetList();
            var groupData = await _ruleBusiness.GetSingleById(id);
            var childList = Allrecords.Where(x => x.ParentId == groupData.Id).ToList();
            var Detail = new List<string>();
            Detail.Add(groupData.Id);
            Detail = getChilds(Allrecords, childList, Detail);
            foreach (var d in Detail)
            {
                await _ruleBusiness.Delete(d);
            }
            return Json(new { success = true });
        }
        public List<string> getChilds(List<BusinessRuleModelViewModel> total, List<BusinessRuleModelViewModel> childList, List<string> ids)
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
        public async Task<IActionResult> RemoveCondition(string id)
        {
            var Condition = await _ruleBusiness.GetSingleById(id);
            await _ruleBusiness.Delete(Condition.Id);
            return Json(new { success = true });
        }
    }
}
