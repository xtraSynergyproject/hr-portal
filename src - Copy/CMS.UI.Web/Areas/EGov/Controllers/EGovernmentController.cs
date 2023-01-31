using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using CMS.Common;
using CMS.Business;
using CMS.Data.Model;
using System.Data;

namespace CMS.UI.Web.Areas.EGov.Controllers
{
    [Area("EGov")]
    public class EGovernmentController : ApplicationController
    {
        private readonly ITemplateBusiness _templateBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly ITaskBusiness _taskBusiness;
        private readonly IUserContext _userContext;
        private readonly INoteBusiness _noteBusiness;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly IEGovernanceBusiness _eGovernanceBusiness;
        private readonly IPortalBusiness _portalBusiness;
        public EGovernmentController(ITemplateBusiness templateBusiness, IServiceBusiness serviceBusiness,
            IUserContext userContext, ITaskBusiness taskBusiness, INoteBusiness noteBusiness, ICmsBusiness cmsBusiness,
            IEGovernanceBusiness eGovernanceBusiness, IPortalBusiness portalBusiness)
        {
            _templateBusiness = templateBusiness;
            _serviceBusiness = serviceBusiness;
            _userContext = userContext;
            _taskBusiness = taskBusiness;
            _noteBusiness = noteBusiness;
            _cmsBusiness = cmsBusiness;
            _eGovernanceBusiness = eGovernanceBusiness;
            _portalBusiness = portalBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PayWaterBill()
        {
            return View();
        }

        public IActionResult PayElectricityBill()
        {
            return View();
        }
        public async Task<IActionResult> Login(string returnUrl = "")
        {
            var portal = await _cmsBusiness.GetSingleGlobal<PortalViewModel, Portal>(x => x.Name == "EGovCustomer");
            if (returnUrl.IsNullOrEmpty())
            {
                returnUrl = "/portal/EGovCustomer";
            }
            return RedirectToAction("login", "account", new { @area = "", @portalId = portal?.Id, @returnUrl = returnUrl });
        }

        public async Task<IActionResult> EGovLandingPage(string templateCode = null, string categoryCode = null)
        {
            var model = new EGovLandingPageViewModel();
            model.SilderBanner = await _eGovernanceBusiness.GetEGovSliderBannerData();
            model.Templates = await _templateBusiness.GetTemplateServiceList(templateCode, categoryCode, null, null, null, TemplateCategoryTypeEnum.Standard);
            var projectlist = await _eGovernanceBusiness.GetEGovProjectMasterData();
            if (projectlist != null)
            {
                model.ProjectCompleted = projectlist.Where(x => x.EndDate < System.DateTime.Today).ToList();
                model.ProjectOnGoing = projectlist.Where(x => x.StartDate <= System.DateTime.Today && x.EndDate >= System.DateTime.Today).ToList();
                model.ProjectUpComing = projectlist.Where(x => x.StartDate > System.DateTime.Today).ToList();
            }
            var newslist = await _eGovernanceBusiness.GetEGovLatestNewsData();
            if (newslist != null)
            {
                model.LatestNews = newslist.Where(x => x.EndDate >= System.DateTime.Today).ToList();
            }
            model.Notifications = await _eGovernanceBusiness.GetEGovNotificationMasterData();
            model.CorporatePhotos = await _eGovernanceBusiness.GetEGovCorporatePhotoData();
            model.Tenders = await _eGovernanceBusiness.GetEGovTenderMasterData();
            model.OtherWebsites = await _eGovernanceBusiness.GetEGovOtherWebsiteData();
            return View(model);
        }
        public async Task<ActionResult> ReadWaterBillData(string consumerNo)
        {
            var list = new List<PayBillViewModel>();
            list.Add(new PayBillViewModel
            {
                ConsumerName = "User 1",
                AccountId = "Id2021_1",
                BillMonth = "August",
                BillIssueDate = DateTime.Now,
                MinimumPayableAmount = 3000,
                BillAmount = 10000,
                PayByDate = DateTime.Now.AddDays(30),
                TotalAmount = 15000,
                ConsumerNo = "1"
            });

            list.Add(new PayBillViewModel
            {
                ConsumerName = "User 1",
                AccountId = "Id2021_1",
                BillMonth = "July",
                BillIssueDate = DateTime.Now.AddDays(-30),
                MinimumPayableAmount = 1200,
                BillAmount = 8000,
                PayByDate = DateTime.Now,
                TotalAmount = 13000,
                ConsumerNo = "1"
            });

            list.Add(new PayBillViewModel
            {
                ConsumerName = "User 2",
                AccountId = "Id2021_1",
                BillMonth = "July",
                BillIssueDate = DateTime.Now.AddDays(-30),
                MinimumPayableAmount = 1200,
                BillAmount = 8000,
                PayByDate = DateTime.Now,
                TotalAmount = 13000,
                ConsumerNo = "2"
            });

            list = list.Where(x => x.ConsumerNo == consumerNo).ToList();
            //return Json(list.ToDataSourceResult(request));
            return Json(list);
        }

        public async Task<ActionResult> ReadElectricityBillData(string consumerNo)
        {
            var list = new List<PayBillViewModel>();
            list.Add(new PayBillViewModel
            {
                ConsumerName = "User 1",
                AccountId = "Id2021_1",
                BillMonth = "August",
                BillIssueDate = DateTime.Now,
                MinimumPayableAmount = 3000,
                BillAmount = 10000,
                PayByDate = DateTime.Now.AddDays(30),
                TotalAmount = 15000,
                ConsumerNo = "1"
            });

            list.Add(new PayBillViewModel
            {
                ConsumerName = "User 1",
                AccountId = "Id2021_1",
                BillMonth = "July",
                BillIssueDate = DateTime.Now.AddDays(-30),
                MinimumPayableAmount = 1200,
                BillAmount = 8000,
                PayByDate = DateTime.Now,
                TotalAmount = 13000,
                ConsumerNo = "1"
            });

            list.Add(new PayBillViewModel
            {
                ConsumerName = "User 2",
                AccountId = "Id2021_1",
                BillMonth = "July",
                BillIssueDate = DateTime.Now.AddDays(-30),
                MinimumPayableAmount = 1200,
                BillAmount = 8000,
                PayByDate = DateTime.Now,
                TotalAmount = 13000,
                ConsumerNo = "2"
            });

            list = list.Where(x => x.ConsumerNo == consumerNo).ToList();
            //return Json(list.ToDataSourceResult(request));
            return Json(list);
        }

        public async Task<IActionResult> Dashboard(string categoryCodes, string requestby, string templateCodes = null)
        {
            var result = await _serviceBusiness.GetServiceList(_userContext.PortalId, null, null, categoryCodes, requestby, false);

            List<EgovDashboardViewModel> list = new List<EgovDashboardViewModel>();

            foreach (var item in result.GroupBy(x => x.TemplateCode))
            {
                list.Add(new EgovDashboardViewModel
                {
                    ServiceName = item.Select(x => x.TemplateDisplayName).FirstOrDefault(),
                    TemplateCode = item.Select(x => x.TemplateCode).FirstOrDefault(),
                    InProgressCount = item.Count(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE"),
                    CompletedCount = item.Count(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE"),
                    RejectedCount = item.Count(x => x.ServiceStatusCode == "SERVICE_STATUS_REJECT" || x.ServiceStatusCode == "SERVICE_STATUS_CANCEL")
                });
            }



            var model = new EGovDashboardViewModel()
            {
                ServiceList = list,
                TemplateCodes = templateCodes
            };

            return View(model);
        }


        //public async Task<IActionResult> ReadServiceList([DataSourceRequest] DataSourceRequest request)
        //{
        //    var categoryCode = "WATER_SERVICE,EGOV_ELECTRICITY,EGOV_WATER,EGOV_PROP_MGMT,EGOV_BIRTH_DEATH,EGOV_GRIEVANCE";

        //    var result = await _serviceBusiness.GetServiceList(_userContext.PortalId,null,null, categoryCode, "RequestedByMe", false);

        //    result = result.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE").ToList();

        //    List<EGovDashboardViewModel> list = new List<EGovDashboardViewModel>();

        //    foreach(var item in result)
        //    {
        //        list.Add(new EGovDashboardViewModel
        //        {
        //            ServiceName = item.TemplateDisplayName,
        //            Count = 1,
        //            DueDate = item.DisplayDueDate,
        //            BillDate = item.DisplayStartDate,
        //            Status = "Pending"
        //        });
        //    }
        //    //list.Add(new EGovDashboardViewModel
        //    //{
        //    //    ServiceName = "Death Registration",
        //    //    Count = 1,
        //    //    DueDate = "12.10.2021",
        //    //    BillDate = "01.10.2021",
        //    //    Status = "Pending"
        //    //});
        //    //list.Add(new EGovDashboardViewModel
        //    //{
        //    //    ServiceName = "Birth Registration",
        //    //    Count = 2,
        //    //    DueDate = "12.10.2021",
        //    //    BillDate = "01.10.2021",
        //    //    Status = "Pending"
        //    //});

        //    var j = Json(list.ToDataSourceResult(request));
        //    return j;
        //}

        public async Task<IActionResult> EmployeeDashboard(string categoryCodes)
        {
            //var templates = await _templateBusiness.GetTemplateServiceList(null, categoryCodes, null, null, null, TemplateCategoryTypeEnum.Standard);

            //string[] codes = templates.Select(x => x.Code).ToArray();
            //var templatecodes = string.Join(",", codes);
            //ViewBag.TemplateCodes = templatecodes;
            ViewBag.CategoryCodes = categoryCodes;
            return View();
        }

        public async Task<IActionResult> ReadTaskListCount(string categoryCodes)
        {
            var result = await _taskBusiness.GetTaskCountByServiceTemplateCodes(categoryCodes);
            var j = Json(result);
            return j;
        }

        public async Task<IActionResult> ReadTaskData(string categoryCodes, string taskStatus)
        {
            var list = await _taskBusiness.GetTaskListByServiceCategoryCodes(categoryCodes, taskStatus);
            //var j = Json(list.ToDataSourceResult(request));
            var j = Json(list);
            return j;
        }

        public IActionResult BookCommunityHall()
        {
            return View();
        }

        public IActionResult RentService()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ReadCommunityHallList(string wardId)
        {
            var where = "";
            if (wardId.IsNotNullAndNotEmpty())

            {
                var wardIds = string.Join("','", wardId);
                where = $@" and ""N_EGOV_MASTER_DATA_CommunityHallName"".""WardIds"" like ('%{wardId}%') ";
                var data = await _cmsBusiness.GetDataListByTemplate("EGOV_COMMUNITY_HALL_NAME", "", where);
                return Json(data);
            }
            else
            {
                var data = await _cmsBusiness.GetDataListByTemplate("EGOV_COMMUNITY_HALL_NAME", "", "");
                return Json(data);
            }

        }

        //[HttpPost]
        //public async Task<JsonResult> GetAvailableStatus(string hallId, DateTime? from, DateTime? to)
        //{
        //    var model = new EGovCommunityHallViewModel();
        //    var data = await _eGovernanceBusiness.GetCommunityHallBookingList(hallId);
        //    var result = data.Where(x => x.BookingFromDate == from || x.BookingFromDate == to || x.BookingToDate == from || x.BookingToDate == to).ToList();

        //        model.CommunityHallName = data.Select(x=>x.CommunityHallName).FirstOrDefault();
        //        model.Status = result.Count>0?"Booked":"Available";
        //        model.BookingFromDate = from.Value;
        //        model.BookingToDate = to.Value;         

        //    return Json(new { success = result.Count > 0 ?false:true,data=model });
        //}
        [HttpPost]
        public async Task<JsonResult> GetAvailableStatus(string hallId)
        {
            long basecharges = 0;
            long acchrs = 0;
            long elchrs = 0;
            long clchrs = 0;
            long total = 0;

            var model = new EGovCommunityHallViewModel();
            var data = await _eGovernanceBusiness.GetCommunityHallBookingList(hallId);

            var where = $@" and ""N_EGOV_MASTER_DATA_CommunityHallName"".""Id"" = '{hallId}'";
            var chdetails = await _cmsBusiness.GetDataListByTemplate("EGOV_COMMUNITY_HALL_NAME", "", where);
            foreach (DataRow dt in chdetails.Rows)
            {
                basecharges = dt["ChargesLeviedPerDay"] != System.DBNull.Value && dt["ChargesLeviedPerDay"].ToString() != "" ? long.Parse(dt["ChargesLeviedPerDay"].ToString()) : 0;
                acchrs = dt["ACCharges"] != System.DBNull.Value && dt["ACCharges"].ToString() != "" ? long.Parse(dt["ACCharges"].ToString()) : 0;
                elchrs = dt["ElectricityCharges"] != System.DBNull.Value && dt["ElectricityCharges"].ToString() != "" ? long.Parse(dt["ElectricityCharges"].ToString()) : 0;
                clchrs = dt["CleaningCharges"] != System.DBNull.Value && dt["CleaningCharges"].ToString() != "" ? long.Parse(dt["CleaningCharges"].ToString()) : 0;
                total = basecharges + acchrs + elchrs + clchrs;
            }

            var from = data.Select(x => x.BookingFromDate).ToArray();
            var to = data.Select(x => x.BookingToDate).ToArray();
            model.DisableDates = from.Union(to).ToArray();

            return Json(new { success = true, disdates = model.DisableDates, bscharges = basecharges, ac = acchrs, el = elchrs, cl = clchrs, tcharges = total });
        }

        public IActionResult AddMeterReading()
        {
            var model = new MeterReadingViewModel()
            {
                ConsumerNo = "WS/1013/2020-21-000011",
                CurrentReadingDate = DateTime.Now,
                LastReadingDate = DateTime.Now.AddDays(-30),
                LastReadingKL = 200,
            };
            return View(model);
        }

        public async Task<IActionResult> MyRequest(string moduleCodes, string templateCodes, string categoryCodes, string requestby, bool isDisableCreate = false, bool showAllOwnersService = false)
        {
            var result = await _serviceBusiness.GetServiceList(_userContext.PortalId, null, null, categoryCodes, requestby, false);
            ViewBag.UserId = _userContext.UserId;

            List<customIndexPageTemplateViewModel> list = new List<customIndexPageTemplateViewModel>();

            foreach (var item in result.GroupBy(x => x.TemplateCode))
            {
                list.Add(new customIndexPageTemplateViewModel
                {
                    ServiceName = item.Select(x => x.TemplateDisplayName).FirstOrDefault(),
                    TemplateCode = item.Select(x => x.TemplateCode).FirstOrDefault(),
                    InProgressCount = item.Count(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE"),
                    CompletedCount = item.Count(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE"),
                    RejectedCount = item.Count(x => x.ServiceStatusCode == "SERVICE_STATUS_REJECT" || x.ServiceStatusCode == "SERVICE_STATUS_CANCEL")
                });
            }



            var model = new CustomIndexPageTemplateViewModel()
            {
                ServiceList = list,
                TemplateCodes = templateCodes,
                ModuleCodes = moduleCodes,
                CategoryCodes = categoryCodes,
                IsDisableCreate = isDisableCreate,
                ShowAllOwnersService = showAllOwnersService,
                PortalId = _userContext.PortalId,
            };

            return View(model);
        }

        public async Task<IActionResult> GetMyRequestList(bool showAllOwnersService, string moduleCodes = null, string templateCodes = null, string categoryCodes = null, string search = null, DateTime? From = null, DateTime? To = null, string statusIds = null, string templateIds = null)
        {
            var dt = await _eGovernanceBusiness.GetMyRequestList(showAllOwnersService, moduleCodes, templateCodes, categoryCodes, search, From, To, statusIds, templateIds);
            return Json(dt);
        }

        public IActionResult ServiceList(string statusCodes, string templateCode)
        {
            ViewBag.UserId = _userContext.UserId;
            ViewBag.StatusCodes = statusCodes;
            return View(new CustomIndexPageTemplateViewModel
            {
                TemplateCodes = templateCode,
                PortalId = _userContext.PortalId,
            });
        }

        public async Task<IActionResult> ReadServiceList(string statusCodes, string templateCode)
        {
            var dt = await _eGovernanceBusiness.GetServiceList(statusCodes, templateCode);
            return Json(dt);
        }

        public async Task<IActionResult> ReadServiceTemplate(string templateCode)
        {
            var result = await _templateBusiness.GetTemplateServiceList(templateCode, null, null, null, null, TemplateCategoryTypeEnum.Standard, false);

            result.Add(new TemplateViewModel
            {
                DisplayName = "More Services"
            });

            var j = Json(result);
            return j;
        }

        public IActionResult KnowYourCorporator()
        {

            return View();
        }

        public IActionResult KnowYourWard()
        {

            return View();
        }

        public IActionResult KnowYourWardOfficers()
        {

            return View();
        }

        public async Task<IActionResult> GetCorporatorList(string wardNo = null, string wardName = null, string councillorName = null, string address = null, string phone = null)
        {
            var dt = await _eGovernanceBusiness.GetCorporatorList(wardNo, wardName, councillorName, address, phone);
            return Json(dt);
        }
        public async Task<IActionResult> GetAdminWard(string wardNo = null, string wardName = null, string electoralWardName = null, string location = null, string constituencyName = null, string latitude = null, string longitude = null)
        {
            var dt = await _eGovernanceBusiness.GetAdminWardList(wardNo, wardName, electoralWardName, location, constituencyName, latitude, longitude);
            return Json(dt);
        }
        public async Task<IActionResult> GetAdminWardOfficer(string wardNo = null, string wardName = null, string officerName = null, string location = null, string phone = null)
        {
            var dt = await _eGovernanceBusiness.GetAdminOfficersWardList(wardNo, wardName, officerName, location, phone);
            return Json(dt);
        }

        public IActionResult BinBooking()
        {
            return View();
        }

        public IActionResult SewerageConnection()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetExistingDetails(string consumerNo, string type)
        {
            if (type == "BinBooking")
            {
                var model = await _eGovernanceBusiness.GetExistingBinBookingDetails(consumerNo);
                if (model != null)
                {
                    return Json(new { success = true, data = model });
                }
            }
            else if (type == "Sewerage")
            {
                var model = await _eGovernanceBusiness.GetExistingSewerageDetails(consumerNo);
                if (model != null)
                {
                    return Json(new { success = true, data = model });
                }
            }

            return Json(new { success = false });
        }
        public IActionResult OnlineTaxPayment(string templateCode)
        {
            var model = new ServiceTemplateViewModel()
            {
                TemplateCode = templateCode,
            };
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> ReadServiceDetail(string templateCode)
        {
            if (templateCode == "SanitationTaxPaymentCommercialUsers")
            {
                var model = await _eGovernanceBusiness.GetCommercialTaxService();
                return Json(model);
            }
            else if (templateCode == "SanitationTaxPaymentResidentialUsers")
            {
                var model = await _eGovernanceBusiness.GetResidentialTaxService();
                return Json(model);
            }
            else if (templateCode == "PayTradeTax")
            {
                var model = await _eGovernanceBusiness.GetTradeTaxService();
                return Json(model);
            }

            return Json(new { success = false });
        }

        public IActionResult MyPayments(string portalNames = null)
        {
            ViewBag.PortalNames = portalNames;

            return View();
        }
        public async Task<IActionResult> ReadTaskDataInProgress(string portalNames = null)
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
            var result = await _eGovernanceBusiness.GetTaskList(ids);
            //var j = Json(result.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS").OrderByDescending(x => x.StartDate));
            var j = Json(result.Where(x => x.StatusGroupCode == "PENDING").OrderByDescending(x => x.StartDate));
            return j;


        }
        public async Task<IActionResult> ReadTaskDataOverdue(string portalNames = null)
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
            var result = await _eGovernanceBusiness.GetTaskList(ids);
            var j = Json(result.Where(x => x.TaskStatusCode == "TASK_STATUS_OVERDUE").OrderByDescending(x => x.StartDate));
            return j;
        }
        public async Task<IActionResult> ReadTaskDataCompleted(string portalNames = null)
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
            var result = await _eGovernanceBusiness.GetTaskList(ids);

            //var j = Json(result.Where(x => x.TaskStatusCode == "TASK_STATUS_COMPLETE").OrderByDescending(x => x.StartDate));
            var j = Json(result.Where(x => x.StatusGroupCode == "DONE").OrderByDescending(x => x.StartDate));
            return j;
        }
        public IActionResult PayTradeTax(string templateCode)
        {
            var model = new ServiceTemplateViewModel()
            {
                TemplateCode = templateCode,
            };
            return View(model);
        }

        public IActionResult RentalManagement()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetPropertyList(string wardId, string rentingType)
        {
            var data = await _eGovernanceBusiness.GetPropertyList(wardId, rentingType);
            return Json(data);
        }

        public async Task<IActionResult> GetPropertyDetails(string propertyId)
        {
            var amenities = "";
            var where = $@" and ""N_EGOV_MASTER_DATA_RentalProperty"".""Id"" = '{propertyId}'";
            var result = await _cmsBusiness.GetDataListByTemplate("RENTAL_PROPERTY", "", where);

            foreach (DataRow dt in result.Rows)
            {
                amenities = dt["PropertyAmenitiesId"].ToString();
                amenities = amenities.Replace("[", "");
                amenities = amenities.Replace("]", "");
                amenities = amenities.Replace("\"", "");
                amenities = String.Concat(amenities.Where(c => !Char.IsWhiteSpace(c)));
                var model = new EGovRentalViewModel()
                {
                    RentalAmount = (long)Convert.ToDouble(dt["RentalAmount"]),
                    DepositAmount = (long)Convert.ToDouble(dt["DepositAmount"]),
                    PropertyAmenitiesId = amenities,
                    AreaInSqFt = dt["AreaInSqFt"].ToString(),
                    WardId = dt["WardId"].ToString(),
                    BuildingNumber = dt["BuildingNumber"].ToString(),
                    Street = dt["Street"].ToString(),
                    LocalitySpecificLocation = dt["LocalitySpecificLocation"].ToString()

                };
                return Json(new { success = true, data = model });

            }
            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<JsonResult> GetAgreementDetails(string agreementNo)
        {
            var result = await _eGovernanceBusiness.GetAgreementDetails(agreementNo);

            return Json(new { success = true, data = result });
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

            var result = await _eGovernanceBusiness.UpdateOnlinePaymentDetails(model);
            if (result.IsSuccess)
            {
                return Json(new { success = true, requestURL = result.Item.RequestUrl/*, returnurl*/ });
            }
            return Json(new { success = false, error = result.Messages.ToHtmlError() });
        }

        public async Task<IActionResult> PaymentResponse(string msg)
        {
            if (msg.IsNullOrEmpty())
            {
                return View(new OnlinePaymentViewModel { PaymentStatusCode = "ERROR", UserMessage = "Invalid Message from Payment Gateway" });
            }
            var responseViewModel = await ValidatePaymentResponse(msg);
            ///Update Online payment
            await _eGovernanceBusiness.UpdateOnlinePayment(responseViewModel);
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
            var model = await _eGovernanceBusiness.GetOnlinePayment(id);
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

            var paymentStatus = await _eGovernanceBusiness.GetList<LOVViewModel, LOV>(x => x.LOVType == "PAYMENT_STATUS");
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
        public IActionResult CommunityHallHome()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ReadCommunityHallData()
        {
            var data = await _eGovernanceBusiness.GetCommunityHallList();
            return Json(data);
        }
    }
}
