using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synergy.App.ViewModel;
using Synergy.App.Common;
using Synergy.App.Business;
using Synergy.App.DataModel;
using System.Data;
//using Kendo.Mvc.Extensions;

namespace CMS.UI.Web.Areas.Csc.Controllers
{
    [Area("CSC")]
    public class CommonServiceController : Controller
    {
        private readonly ILearningBusiness _learningBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly ITaskBusiness _taskBusiness;
        private readonly IUserContext _userContext;
        private readonly INoteBusiness _noteBusiness;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly ICommonServiceBusiness _commonServiceBusiness;
        ICompanySettingBusiness _companySettingBusiness;
        private readonly IPortalBusiness _portalBusiness;
        private readonly IUserBusiness _userBusiness;

        public CommonServiceController(ITemplateBusiness templateBusiness, IServiceBusiness serviceBusiness,
            IUserContext userContext, ITaskBusiness taskBusiness, INoteBusiness noteBusiness, ICmsBusiness cmsBusiness,
            ICommonServiceBusiness commonServiceBusiness, IPortalBusiness portalBusiness, IUserBusiness userBusiness,
            ICompanySettingBusiness companySettingBusiness, ILearningBusiness learningBusiness)
        {
            _templateBusiness = templateBusiness;
            _serviceBusiness = serviceBusiness;
            _userContext = userContext;
            _taskBusiness = taskBusiness;
            _noteBusiness = noteBusiness;
            _cmsBusiness = cmsBusiness;
            _commonServiceBusiness = commonServiceBusiness;
            _portalBusiness = portalBusiness;
            _companySettingBusiness = companySettingBusiness;
            _learningBusiness = learningBusiness;
            _userBusiness = userBusiness;
        }
        public IActionResult Index()
        {
            var model = new LearningPlanViewModel();
            return View(model);
        }
        public async Task<IActionResult> CSCLoginPage()
        {
            return View();
        }
        public async Task<IActionResult> CSCTrackStatus()
        {
            return View();
        }
        public async Task<ActionResult> ReadTrackApplicationData(string appNumber)
        {
            var data = new List<CSCTrackApplicationViewModel>();
            if (appNumber.IsNotNullAndNotEmpty())
            {
                data = await _commonServiceBusiness.GetTrackApplicationList(appNumber);
            }
            return Json(data);
        }
        public async Task<ActionResult> ReadData([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _learningBusiness.GetLearningPlanData();
            var json = Json(result);
            //var json = Json(result.ToDataSourceResult(request));
            return json;
        }
        public async Task<IActionResult> ServiceIntroduction(string templateCode, string pageName, string cbm)
        {

            if (_userContext.CultureName=="hi-IN")
            {
                pageName = $"{pageName}Hindi";
            }
            var model = new TemplateViewModel
            {
                Code = templateCode,
                PageName = pageName,
                PortalNames = _userContext.PortalName,
                PortalId = _userContext.PortalId,
                CallBackMethodName = cbm
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageServiceIntroduction(TemplateViewModel model)
        {
            return Json(new { success = true });

        }
        public async Task<IActionResult> CSCMyPayments(string portalNames = null)
        {
            ViewBag.PortalNames = portalNames;

            //var cs = await _companySettingBusiness.GetSingle(x => x.Code == "SMART_CITY_URL");
            //ViewBag.ReturnUrl = cs.Value;

            return View();
        }

        public async Task<IActionResult> ReadCSCTaskDataInProgress(string portalNames = null)
        {
            var ids = "";
            if (portalNames.IsNotNullAndNotEmpty())
            {
                string[] names = portalNames.Split(",").ToArray();
                var portals = await _portalBusiness.GetList(x => names.Contains(x.Name));
                string[] portalIds = portals.Select(x => x.Id).ToArray();
                ids = String.Join("','", portalIds);
            }
            else
            {
                ids = _userContext.PortalId;
            }
            var result = await _commonServiceBusiness.GetCSCTaskList(ids);
            //var j = Json(result.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS").OrderByDescending(x => x.StartDate));
            var j = Json(result.Where(x => x.StatusGroupCode == "PENDING").OrderByDescending(x => x.StartDate));
            return j;

        }

        public async Task<IActionResult> ReadCSCTaskDataCompleted(string portalNames = null)
        {
            var ids = "";
            if (portalNames.IsNotNullAndNotEmpty())
            {
                string[] names = portalNames.Split(",").ToArray();
                var portals = await _portalBusiness.GetList(x => names.Contains(x.Name));
                string[] portalIds = portals.Select(x => x.Id).ToArray();
                ids = String.Join("','", portalIds);
            }
            else
            {
                ids = _userContext.PortalId;
            }
            var result = await _commonServiceBusiness.GetCSCTaskList(ids);

            //var j = Json(result.Where(x => x.TaskStatusCode == "TASK_STATUS_COMPLETE").OrderByDescending(x => x.StartDate));
            var j = Json(result.Where(x => x.StatusGroupCode == "DONE").OrderByDescending(x => x.StartDate));
            return j;
        }

        [HttpPost]
        public async Task<JsonResult> OnlinePayment(string ntsId, string noteTableId, long amount, NtsTypeEnum ntsType, string assigneeUserId, string returnUrl)
        {
            var model = new OnlinePaymentViewModel()
            {
                NtsId = ntsId,
                UdfTableId = noteTableId,
                Amount = amount,
                NtsType = ntsType,
                UserId = assigneeUserId,
                ReturnUrl = returnUrl
            };

            var result = await _commonServiceBusiness.UpdateOnlinePaymentDetails(model);
            if (result.IsSuccess)
            {
                return Json(new { success = true, requestURL = result.Item.RequestUrl/*, returnurl*/ });
            }
            return Json(new { success = false, error = result.Messages.ToHtmlError() });
        }

        public async Task<IActionResult> CSCPaymentResponse(string msg)
        {
            if (msg.IsNullOrEmpty())
            {
                return View(new OnlinePaymentViewModel { PaymentStatusCode = "ERROR", UserMessage = "Invalid Message from Payment Gateway" });
            }
            var responseViewModel = await ValidatePaymentResponse(msg);
            ///Update Online payment
            await _commonServiceBusiness.UpdateOnlinePayment(responseViewModel);
            if (responseViewModel.PaymentStatusCode == "SUCCESS")
            {
                responseViewModel.UserMessage = $"Your payment has been completed successfully. Please note the reference number: {responseViewModel.PaymentReferenceNo} for further communication.";
                // update udf payment status id,payment reference and then complete the task
                if (responseViewModel.NtsType == NtsTypeEnum.Task)
                {
                    TaskTemplateViewModel model = new TaskTemplateViewModel();
                    model.TaskId = responseViewModel.NtsId;
                    model.TaskTemplateType = TaskTypeEnum.StepTask;
                    model.DataAction = DataActionEnum.Edit;
                    model.ActiveUserId = _userContext.UserId;
                    model.SetUdfValue = true;
                    var taskModel = await _taskBusiness.GetTaskDetails(model);
                    if (taskModel != null)
                    {
                        if (taskModel.ColumnList != null && taskModel.ColumnList.Any())
                        {
                            var rowData = taskModel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                            if (rowData.ContainsKey("PaymentStatusId"))
                            {
                                rowData["PaymentStatusId"] = responseViewModel.PaymentStatusId;
                            }
                            if (rowData.ContainsKey("PaymentReferenceNo"))
                            {
                                rowData["PaymentReferenceNo"] = responseViewModel.PaymentReferenceNo;
                            }

                            taskModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                        }
                        taskModel.TaskStatusCode = "TASK_STATUS_COMPLETE";
                        taskModel.TaskTemplateType = TaskTypeEnum.StepTask;
                        taskModel.DataAction = DataActionEnum.Edit;
                        taskModel.ActiveUserId = _userContext.UserId;
                        var update = await _taskBusiness.ManageTask(taskModel);
                    }

                }
                else if (responseViewModel.NtsType == NtsTypeEnum.Service)
                {
                    ServiceTemplateViewModel model = new ServiceTemplateViewModel();
                    model.ServiceId = responseViewModel.NtsId;                    
                    model.DataAction = DataActionEnum.Edit;
                    model.ActiveUserId = _userContext.UserId;
                    model.SetUdfValue = true;
                    var serviceModel = await _serviceBusiness.GetServiceDetails(model);
                    if (serviceModel != null)
                    {
                        if (serviceModel.ColumnList != null && serviceModel.ColumnList.Any())
                        {
                            var rowData = serviceModel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                            if (rowData.ContainsKey("PaymentStatusId"))
                            {
                                rowData["PaymentStatusId"] = responseViewModel.PaymentStatusId;
                            }
                            if (rowData.ContainsKey("PaymentReferenceNo"))
                            {
                                rowData["PaymentReferenceNo"] = responseViewModel.PaymentReferenceNo;
                            }
                            if (rowData.ContainsKey("Amount"))
                            {
                                rowData["Amount"] = responseViewModel.Amount;
                            }
                            if (rowData.ContainsKey("Acknowledgment"))
                            {
                                rowData["Acknowledgment"] = "true";
                            }                            

                            serviceModel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                        }
                        serviceModel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                        serviceModel.DataAction = DataActionEnum.Edit;
                        serviceModel.ActiveUserId = _userContext.UserId;
                        serviceModel.AllowPastStartDate = true;
                        var update = await _serviceBusiness.ManageService(serviceModel);
                    }
                }
            }
            else
            {
                responseViewModel.UserMessage = responseViewModel.ResponseError;
            }
            return View(responseViewModel);
        }

        private async Task<OnlinePaymentViewModel> ValidatePaymentResponse(string msg)
        {
            var values = msg.Split('|');
            var id = values[1];
            var model = await _commonServiceBusiness.GetOnlinePayment(id);
            if (model == null)
            {
                return new OnlinePaymentViewModel
                {
                    PaymentStatusCode = "ERROR",
                    ResponseError = "Invalid Transaction"
                };
            }
            model.AuthStatus = values[14];
            model.ResponseErrorCode = values[23];
            model.ResponseError = values[24];
            model.PaymentReferenceNo = values[2];
            if (model.PaymentReferenceNo == "NA")
            {
                model.PaymentReferenceNo = "";
            }
            model.ResponseChecksumValue = values[25];
            model.ResponseMessage = msg.Replace($"|{model.ResponseChecksumValue}", "");

            model.ResponseUrl = Request.Path;

            var paymentStatus = await _commonServiceBusiness.GetList<LOVViewModel, LOV>(x => x.LOVType == "PAYMENT_STATUS");
            switch (model.AuthStatus)
            {
                case "0300":
                    var success = paymentStatus.FirstOrDefault(x => x.Code == "SUCCESS");
                    if (success != null)
                    {
                        model.PaymentStatusId = success.Id;
                        model.PaymentStatusCode = success.Code;
                        model.PaymentStatusName = success.Name;
                    }
                    break;
                default:
                    var fail = paymentStatus.FirstOrDefault(x => x.Code == "ERROR");
                    if (fail != null)
                    {
                        model.PaymentStatusId = fail.Id;
                        model.PaymentStatusCode = fail.Code;
                        model.PaymentStatusName = fail.Name;
                    }
                    if (model.ResponseError.IsNullOrEmpty() || model.ResponseError == "NA")
                    {
                        model.ResponseError = "An error occured while processing your request.";
                    }
                    if (model.PaymentReferenceNo.IsNotNullAndNotEmpty())
                    {
                        model.ResponseError = $"{model.ResponseError} Please note the reference number: {model.PaymentReferenceNo} for further communication.";
                    }
                    break;
            }
            return model;
        }

        public IActionResult CSCServiceListDashboard(string categoryCodes, bool enableCreate = false)
        {
            //var templates = await _templateBusiness.GetTemplateServiceList(null, categoryCodes, null, null, null, TemplateCategoryTypeEnum.Standard);

            //string[] codes = templates.Select(x => x.Code).ToArray();
            //var templatecodes = string.Join(",", codes);
            //ViewBag.TemplateCodes = templatecodes;
            ViewBag.CategoryCodes = categoryCodes;
            ViewBag.EnableCreate = enableCreate;
            ViewBag.UserId = _userContext.UserId;
            //var cs = await _companySettingBusiness.GetSingle(x => x.Code == "SMART_CITY_URL");
            //ViewBag.ReturnUrl = cs.Value;
            return View();
        }

        public async Task<IActionResult> CSCSelectServiceTemplate(string templateCode, string categoryCode, string userId, string moduleCodes, string prms, string cbm, string templateIds, string categoryIds, bool allBooks = false, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, ServiceTypeEnum serviceType = ServiceTypeEnum.StandardService, string portalNames = null)
        {
            var model = new TemplateViewModel();
            model.Code = templateCode;
            model.CategoryCode = categoryCode;
            model.UserId = userId;
            model.ModuleCodes = moduleCodes;
            model.Prms = prms;
            model.CallBackMethodName = cbm;
            model.TemplateIds = templateIds;
            model.CategoryIds = categoryIds;
            model.TemplateCategoryType = categoryType;
            model.AllBooks = allBooks;
            model.PortalNames = portalNames;
            model.ServiceType = serviceType;

            if (_userContext.UserRoleCodes.Contains("CSC_AGENT"))
            {
                ViewBag.IsAgent = true;

            }
            else
            {
                ViewBag.IsAgent = false;
                ViewBag.UserId = _userContext.UserId;
                ViewBag.UserName = _userContext.Name;
                var details = await _userBusiness.GetSingleById(_userContext.UserId);
                ViewBag.MobileNo = details.Mobile;
                ViewBag.Email = details.Email;
            }

            return View(model);
        }

        public async Task<IActionResult> ReadServiceListCount(string categoryCodes, bool isIncluded)
        {
            var result = await _serviceBusiness.GetServiceCountByServiceTemplateCodes(categoryCodes, _userContext.PortalId, isIncluded);
            var j = Json(result);
            return j;
        }
        public async Task<ActionResult> GetCasteDetails(string casteId)
        {
            var result = await _learningBusiness.GetCasteDetails(casteId);
            var json = Json(result);

            return json;
        }

        public async Task<ActionResult> ChargeDetails(string serviceId= "6a6306ae-82ac-4426-b83f-9921be8457af")
        {
            var data = await _commonServiceBusiness.GetServiceChargeDetails(serviceId);

            IList<ServiceChargeViewModel> data1 = new List<ServiceChargeViewModel>();                                                   

            var service = await _serviceBusiness.GetSingleById(serviceId);

            

            if(service.TemplateCode == "OBC_CERTIFICATE")
            {
                var docCount = await _commonServiceBusiness.GetDocumentsCount(service.UdfNoteTableId);

                foreach (var item in data)
                {
                    if (item.ChargeCode == "49")
                    {
                        item.Amount = (docCount - item.FeeExcemptionQuantity) > 0 ? item.Amount * (docCount - item.FeeExcemptionQuantity) : item.Amount;
                    }
                }                
            }

            if(service.TemplateCode == "CSC_MARRIAGE_CERTIFICATE")
            {
                var where = $@" and ""N_SNC_CSC_SERVICES_Csc_Marriage_Certificate"".""Id"" = '{service.UdfNoteTableId}'";
                var udfdetails = await _cmsBusiness.GetDataListByTemplate(service.TemplateCode,"",where);
                int days=0;
                foreach (DataRow row in udfdetails.Rows)
                {
                    days = Convert.ToInt32(row["noOfMarriageDaysFromTodaysDate"]);
                }

                if(days <= 30)
                {
                    data = data.Where(x => x.ChargeCode != "50").ToList();
                }
            }
            

            ViewBag.TotalAmount = data.Sum(x => x.Amount).ToString();
            ViewBag.ServiceId = serviceId;
            if (service.IsNotNull())
            {
                ViewBag.TableId = service.UdfNoteTableId;
                ViewBag.UserId = service.OwnerUserId;
            }            

            return View(data);
        }
    }
}
