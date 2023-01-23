using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Synergy.App.Business;
using Synergy.App.ViewModel;
using Synergy.App.Common;
using System.Net.Http.Headers;
//using Kendo.Mvc.UI;
//using Kendo.Mvc.Extensions;
using System.IO;
using Synergy.App.DataModel;
using System.Data;
using Synergy.App.WebUtility;

namespace CMS.UI.Web.Areas.Property.Controllers
{
    [Area("Property")]
    public class PropertyOwnershipController : ApplicationController
    {
        private readonly IServiceBusiness _serviceBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private static IUserContext _userContext;
        public PropertyOwnershipController(IUserContext userContext, IServiceBusiness serviceBusiness, ITableMetadataBusiness tableMetadataBusiness, INoteBusiness noteBusiness)
        {
            _serviceBusiness = serviceBusiness;
             _userContext = userContext;
            _tableMetadataBusiness = tableMetadataBusiness;
            _noteBusiness = noteBusiness;
        }

      
        // GET: CompanyController
        public async Task<IActionResult> Index(string serviceId,string propertyId)
        {
            PropertyOwnershipViewModel model = new PropertyOwnershipViewModel();
            if (propertyId.IsNotNullAndNotEmpty()) 
            {
             
                var udftabledata = await _tableMetadataBusiness.GetTableDataByColumn("PROPERTY_MASTER", "", "Id", propertyId);                
                var ownerudftabledata = await _tableMetadataBusiness.GetTableDataByColumn("PROPERTY_OWNER", "", "Id", Convert.ToString(udftabledata["OwnerNameId"]));
               var note= await _noteBusiness.GetSingleById(udftabledata["NtsNoteId"].ToString());                
                model.PropertyNo = note.NoteNo;
                model.OldOwnerName = ownerudftabledata.Table.Columns.Contains("FirstName") ? ownerudftabledata["FirstName"].ToString():null;
                model.OldMiddleNameOfOwner = ownerudftabledata.Table.Columns.Contains("MiddleName") ? ownerudftabledata["MiddleName"].ToString():null;
                model.OldSurname = ownerudftabledata.Table.Columns.Contains("LastName") ? ownerudftabledata["LastName"].ToString():null;
                model.OldFatherOrHusbandName = ownerudftabledata.Table.Columns.Contains("FatherOrHusbandName") ? ownerudftabledata["FatherOrHusbandName"].ToString():null;
                model.OldFatherMiddleName = ownerudftabledata.Table.Columns.Contains("FatherMiddleName") ? ownerudftabledata["FatherMiddleName"].ToString():null;
                model.OldFatherOrHusbandSurname = ownerudftabledata.Table.Columns.Contains("FatherOrHusbandSurname") ? ownerudftabledata["FatherOrHusbandSurname"].ToString():null;
                model.OldHouseNo = udftabledata.Table.Columns.Contains("HouseNo") ? udftabledata["HouseNo"].ToString():null;
                model.WardNo = udftabledata.Table.Columns.Contains("WardNoId") ? udftabledata["WardNoId"].ToString():null;
                model.Zone = udftabledata.Table.Columns.Contains("Zone") ? udftabledata["Zone"].ToString():null;
                model.OldPinCode = udftabledata.Table.Columns.Contains("PostalCode") ? udftabledata["PostalCode"].ToString():null;
                model.OldLocalit = udftabledata.Table.Columns.Contains("Locality") ? udftabledata["Locality"].ToString():null;
                model.OldCity = udftabledata.Table.Columns.Contains("City") ? udftabledata["City"].ToString():null;
                model.OldColony = udftabledata.Table.Columns.Contains("ColonyId") ? udftabledata["ColonyId"].ToString():null;
                model.RegistryNo = udftabledata.Table.Columns.Contains("RegistryNo") ? udftabledata["RegistryNo"].ToString():null;
                model.ContactAccount = udftabledata.Table.Columns.Contains("ContractAccount") ? udftabledata["ContractAccount"].ToString():null;
                model.MobileNo = ownerudftabledata.Table.Columns.Contains("Mobile") ? ownerudftabledata["Mobile"].ToString():null;
            }
            
            if (serviceId.IsNotNullAndNotEmpty())
            {
                model.ServiceId = serviceId;
                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManagePropertyOwnershiphange(PropertyOwnershipViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var noteTempModel = new ServiceTemplateViewModel();

                    noteTempModel.DataAction = model.DataAction;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "PROPERTY_OWNERSHIPCHANGE";                  
                    var notemodel = await _serviceBusiness.GetServiceDetails(noteTempModel);
                   
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    notemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                   
                    var result = await _serviceBusiness.ManageService(notemodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                   
                }
                else
                {
                    
                        var noteTempModel = new ServiceTemplateViewModel();
                        noteTempModel.DataAction = DataActionEnum.Edit;
                        noteTempModel.ActiveUserId = _userContext.UserId;
                        noteTempModel.ServiceId = model.ServiceId;
                        noteTempModel.ParentNoteId = model.ParentNoteId;
                        var notemodel = await _serviceBusiness.GetServiceDetails(noteTempModel);
                        // model = _autoMapper.Map<NoteTemplateViewModel, SalaryInfoViewModel>(notemodel, model);
                        notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                        var result = await _serviceBusiness.ManageService(notemodel);
                        if (result.IsSuccess)
                        {
                            return Json(new { success = true });
                        }
                        return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                        // }

                        // }
                        // return Json(new { success = true });
                    //}

                }
            }

            else
            {
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }



        }
        public IActionResult PropertyOwnershipChange(string moduleCodes, string templateCodes, string categoryCodes, bool isDisableCreate = false, bool showAllOwnersService = false)
        {
            ViewBag.IsDisableCreate = isDisableCreate;
            ViewBag.ShowAllOwnersService = showAllOwnersService;
            ViewBag.UserId = _userContext.UserId;
            var model = new ServiceViewModel { ModuleCode = moduleCodes, TemplateCode = templateCodes, TemplateCategoryCode = categoryCodes };
            return View(model);            
        }

        public async Task<IActionResult> ReadPOCServiceDataInProgress(string moduleCodes, string templateCodes, string categoryCodes, string requestby = null, bool showAllOwnersService = false, bool isDisableCreate = false, string mode = null, string templatecode = null)
        {
            if (mode.IsNotNullAndNotEmpty())
            {
                var result = await _serviceBusiness.GetSEBIExternalServiceList(templatecode);

                var j = Json(result.Where(x => x.ServiceStatusCode == "TASK_STATUS_INPROGRESS").OrderByDescending(x => x.CreatedDate));
                return j;
            }
            else
            {
                var result = await _serviceBusiness.GetServiceList(_userContext.PortalId, moduleCodes, templateCodes, categoryCodes, requestby, showAllOwnersService);
                
                    if (isDisableCreate)
                    {
                        var j = Json(result.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS").OrderByDescending(x => x.CreatedDate));
                        return j;
                    }
                    else
                    {
                        var j = Json(result.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_DRAFT").OrderByDescending(x => x.CreatedDate));
                        return j;
                    }                
            }

        }

        public async Task<IActionResult> ReadPOCServiceDataOverdue(string moduleCodes, string templateCodes, string categoryCodes, string requestby = null, bool showAllOwnersService = false, string mode = null, string templatecode = null)
        {
            if (mode.IsNotNullAndNotEmpty())
            {
                var result = await _serviceBusiness.GetSEBIExternalServiceList(templatecode);
                var j = Json(result.Where(x => x.ServiceStatusCode == "TASK_STATUS_OVERDUE").OrderBy(x => x.CreatedDate));
                return j;
            }
            else
            {
                var result = await _serviceBusiness.GetServiceList(_userContext.PortalId, moduleCodes, templateCodes, categoryCodes, requestby, showAllOwnersService);
                var j = Json(result.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE").OrderBy(x => x.CreatedDate));
                return j;
            }
        }

        public async Task<IActionResult> ReadPOCServiceDataCompleted(string moduleCodes, string templateCodes, string categoryCodes, string requestby = null, bool showAllOwnersService = false, string mode = null, string templatecode = null)
        {
            if (mode.IsNotNullAndNotEmpty())
            {
                var result = await _serviceBusiness.GetSEBIExternalServiceList(templatecode);
                var j = Json(result.Where(x => x.ServiceStatusCode == "TASK_STATUS_COMPLETE" || x.ServiceStatusCode == "TASK_STATUS_CANCEL" || x.ServiceStatusCode == "TASK_STATUS_REJECT").OrderByDescending(x => x.LastUpdatedDate));
                return j;
            }
            else
            {
                var result = await _serviceBusiness.GetServiceList(_userContext.PortalId, moduleCodes, templateCodes, categoryCodes, requestby, showAllOwnersService);
                var j = Json(result.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE" || x.ServiceStatusCode == "SERVICE_STATUS_CANCEL" || x.ServiceStatusCode == "SERVICE_STATUS_REJECT").OrderByDescending(x => x.LastUpdatedDate));
                return j;
            }
        }

        public async Task<IActionResult> ReadPOCServiceDataClosed(string moduleCodes, string templateCodes, string categoryCodes, string requestby = null, bool showAllOwnersService = false, string mode = null, string templatecode = null)
        {
            if (mode.IsNotNullAndNotEmpty())
            {
                var result = await _serviceBusiness.GetSEBIExternalServiceList(templatecode);
                var j = Json(result.Where(x => x.ServiceStatusCode == "TASK_STATUS_CLOSE").OrderByDescending(x => x.LastUpdatedDate));
                return j;
            }
            else
            {
                var result = await _serviceBusiness.GetServiceList(_userContext.PortalId, moduleCodes, templateCodes, categoryCodes, requestby, showAllOwnersService);
                var j = Json(result.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_CLOSE").OrderByDescending(x => x.LastUpdatedDate));
                return j;
            }
        }
    }

}
