//using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synergy.App.ViewModel;
//using Kendo.Mvc.Extensions;
using Synergy.App.Common;
using Synergy.App.Business;
using Synergy.App.DataModel;
using System.Data;
using Synergy.App.WebUtility;
using static Humanizer.In;
using Elasticsearch.Net;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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
        ICompanySettingBusiness _companySettingBusiness;
        private readonly IPortalBusiness _portalBusiness;
        private readonly IStepTaskEscalationDataBusiness _stepTaskEscalationDataBusiness;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITemplateCategoryBusiness _templateCategoryBusiness;
        public EGovernmentController(ITemplateBusiness templateBusiness, IServiceBusiness serviceBusiness, ITemplateCategoryBusiness templateCategoryBusiness,
         IStepTaskEscalationDataBusiness stepTaskEscalationDataBusiness,   IUserContext userContext, ITaskBusiness taskBusiness, INoteBusiness noteBusiness, ICmsBusiness cmsBusiness,
            IEGovernanceBusiness eGovernanceBusiness, IPortalBusiness portalBusiness, ICompanySettingBusiness companySettingBusiness
             ,IServiceProvider serviceProvider)
        {
            _templateBusiness = templateBusiness;
            _serviceBusiness = serviceBusiness;
            _userContext = userContext;
            _taskBusiness = taskBusiness;
            _noteBusiness = noteBusiness;
            _cmsBusiness = cmsBusiness;
            _eGovernanceBusiness = eGovernanceBusiness;
            _portalBusiness = portalBusiness;
            _companySettingBusiness = companySettingBusiness;
            _stepTaskEscalationDataBusiness = stepTaskEscalationDataBusiness;
            _serviceProvider = serviceProvider;
            _templateCategoryBusiness = templateCategoryBusiness;
            
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
        public async Task<IActionResult> Login(string returnUrl = "",bool eGovLayoutNull=false,string portalName= "EGovCustomer")
        {
            var portal = await _cmsBusiness.GetSingleGlobal<PortalViewModel, Portal>(x => x.Name == portalName);
            if (returnUrl.IsNullOrEmpty())
            {
                returnUrl = $"/portal/{portalName}";
            }
            return RedirectToAction("login", "account", new { @area = "", @portalId = portal?.Id, @returnUrl = returnUrl, @eGovLayoutNull= eGovLayoutNull });
        }

        public async Task<IActionResult> EGovLandingPage(string templateCode = null, string categoryCode = null)
        {
            var model = new EGovLandingPageViewModel();
            model.SilderBanner = await _eGovernanceBusiness.GetEGovSliderBannerData();
            model.Templates = await _templateBusiness.GetTemplateServiceList(templateCode, categoryCode, null, null, null, TemplateCategoryTypeEnum.Standard,false,null,ServiceTypeEnum.StandardService);
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
        public IActionResult EGovSSCNotifications()
        {
            return View();
        }
        public async Task<IActionResult> ReadEGovSSCNotificationsData()
        {
            var model = await _eGovernanceBusiness.GetEGovSSCNotificationData();
            return Json(model);
        }
        public IActionResult EGovSSCTenders()
        {
            return View();
        }
        public async Task<IActionResult> ReadEGovSSCTendersData()
        {
            var model = await _eGovernanceBusiness.GetEGovSSCTenderData();
            return Json(model);
        }
        public IActionResult EGovSSCOrders()
        {
            return View();
        }
        public async Task<IActionResult> ReadEGovSSCOrdersData()
        {
            var model = await _eGovernanceBusiness.GetEGovSSCOrderData();
            return Json(model);
        }
        public IActionResult EGovSSCCirculars()
        {
            return View();
        }
        public async Task<IActionResult> ReadEGovSSCCircularsData()
        {
            var model = await _eGovernanceBusiness.GetEGovSSCCircularData();
            return Json(model);
        }
        public IActionResult EGovSSCDownloads()
        {
            return View();
        }
        public async Task<IActionResult> ReadEGovSSCDownloadsData()
        {
            var model = await _eGovernanceBusiness.GetEGovSSCDownloadsData();
            return Json(model);
        }
        public async Task<IActionResult> EGovSSCActnByeLaws()
        {
            var modelList = await _eGovernanceBusiness.GetSSCActnByeLawsData("Link");
            var imgList = await _eGovernanceBusiness.GetSSCActnByeLawsData("Image");
            ViewBag.ImageList = imgList.Select(x => x.FileId).ToList();
            return View(modelList);
        }

        public async Task<IActionResult> ReadEGovSSCOrderCircularsData()
        {
            var model = await _eGovernanceBusiness.GetEGovSSCOrderCircularData();
            return Json(model);
        }
        public IActionResult EGovSSCNews()
        {
            return View();
        }
        public async Task<IActionResult> ReadEGovSSCNewsData()
        {
            var model = await _eGovernanceBusiness.GetEGovSSCNewsData();
            return Json(model);
        }
        public IActionResult EGovSSCPublications()
        {
            return View();
        }
        public async Task<IActionResult> ReadEGovSSCPublicationsData()
        {
            var model = await _eGovernanceBusiness.GetEGovSSCPublicationData();
            return Json(model);
        }
        public IActionResult EGovSSCAdministrativeWards()
        {
            return View();
        }
        public async Task<IActionResult> GetAdministrativeWards()
        {
            var datalist = await _eGovernanceBusiness.GetAdministrativeWardsList();
            return Json(datalist);
        }
        public IActionResult EGovSSCAdminWardOfficers()
        {
            return View();
        }
        public IActionResult EGovSSCContactUs()
        {
            return View();
        }
        public IActionResult EGovSSCOurCity()
        {
            return View();
        }
        public IActionResult EGovSSCOrganisation()
        {
            return View();
        }
        public async Task<IActionResult> EGovSSCSmcJurisdiction()
        {
            ViewBag.SRO72 = "";
            ViewBag.SRO219 = "";
            var jsro72 = await _eGovernanceBusiness.GetSrinagarSettingData("JURISDICTION_SRO72");
            if (jsro72.IsNotNull())
            {
                if (jsro72.FileId.IsNotNullAndNotEmpty())
                {
                    ViewBag.SRO72 = jsro72.FileId;
                }
            }
            var jsro219 = await _eGovernanceBusiness.GetSrinagarSettingData("JURISDICTION_SRO219");
            if (jsro219.IsNotNull())
            {
                if (jsro219.FileId.IsNotNullAndNotEmpty())
                {
                    ViewBag.SRO219 = jsro219.FileId;
                }
            }
            return View();
        }
        public async Task<IActionResult> EGovSSCWardMaps()
        {
            var modellist = await _eGovernanceBusiness.GetEGovSSCWardMapsData();
            ViewBag.MetropolitianMap = ""; 
            ViewBag.CityMap = ""; 
            ViewBag.SanitationMap = ""; 
            ViewBag.ZoneNorth = ""; 
            ViewBag.ZoneSouth = ""; 
            ViewBag.ZoneEast = ""; 
            ViewBag.ZoneWest = "";
            var mapmetro = await _eGovernanceBusiness.GetSrinagarSettingData("MAP_METROPOLITIAN");
            if (mapmetro.IsNotNull())
            {
                if (mapmetro.FileId.IsNotNullAndNotEmpty())
                {
                    ViewBag.MetropolitianMap = mapmetro.FileId;
                }
            }
            var mapcity = await _eGovernanceBusiness.GetSrinagarSettingData("MAP_SMCCITY");
            if (mapcity.IsNotNull())
            {
                if (mapcity.FileId.IsNotNullAndNotEmpty())
                {
                    ViewBag.CityMap = mapcity.FileId;
                }
            }
            var mapzone = await _eGovernanceBusiness.GetSrinagarSettingData("MAP_SANITATION_ZONE");
            if (mapzone.IsNotNull())
            {
                if (mapzone.FileId.IsNotNullAndNotEmpty())
                {
                    ViewBag.SanitationMap = mapzone.FileId;
                }
            }
            var mapnorth = await _eGovernanceBusiness.GetSrinagarSettingData("MAP_SANITATION_ZONE_NORTH");
            if (mapnorth.IsNotNull())
            {
                if (mapnorth.FileId.IsNotNullAndNotEmpty())
                {
                    ViewBag.ZoneNorth = mapnorth.FileId;
                }
            }
            var mapsouth = await _eGovernanceBusiness.GetSrinagarSettingData("MAP_SANITATION_ZONE_SOUTH");
            if (mapsouth.IsNotNull())
            {
                if (mapsouth.FileId.IsNotNullAndNotEmpty())
                {
                    ViewBag.ZoneNorth = mapsouth.FileId;
                }
            }
            var mapeast = await _eGovernanceBusiness.GetSrinagarSettingData("MAP_SANITATION_ZONE_EAST");
            if (mapeast.IsNotNull())
            {
                if (mapeast.FileId.IsNotNullAndNotEmpty())
                {
                    ViewBag.ZoneEast = mapeast.FileId;
                }
            }
            var mapwest = await _eGovernanceBusiness.GetSrinagarSettingData("MAP_SANITATION_ZONE_WEST");
            if (mapwest.IsNotNull())
            {
                if (mapwest.FileId.IsNotNullAndNotEmpty())
                {
                    ViewBag.ZoneWest = mapwest.FileId;
                }
            }
            return View(modellist);
        }
        public IActionResult EGovSSCAchievements()
        {
            return View();
        }
        public IActionResult EGovSSCWhoisWho()
        {
            return View();
        }
        public IActionResult EGovSSCAboutUs()
        {
            return View();
        }
        public IActionResult EGovSSCHonourableMayor()
        {
            return View();
        }
        public IActionResult EGovSSCDeputyMayor()
        {
            return View();
        }
        public async Task<IActionResult> EGovSSCCorporators()
        {
            var modellist = await _eGovernanceBusiness.GetEGovSSCCorporatorsData();
            return View(modellist);
        }
        public async Task<IActionResult> EGovSSCCommitteeMaster(string committeeCode)
        {
            var model = await _eGovernanceBusiness.GetEGovSSCCommitteeMasterData(committeeCode);
            return View(model);
        }
        public async Task<IActionResult> EGovSSCExecutiveCommittee()
        {
            var modellist = await _eGovernanceBusiness.GetEGovSSCCommitteeMemberData("EGOV_SSC_COMMITTEE_EXECUTIVE");
            return View(modellist);
        }
        public async Task<IActionResult> EGovSSCFinancePlanningCommittee()
        {
            var modellist = await _eGovernanceBusiness.GetEGovSSCCommitteeMemberData("EGOV_SSC_COMMITTEE_FINPLAN");
            return View(modellist);
        }
        public async Task<IActionResult> EGovSSCSkillUpGradationCommittee()
        {
            var modellist = await _eGovernanceBusiness.GetEGovSSCCommitteeMemberData("EGOV_SSC_COMMITTEE_SKILL");
            return View(modellist);
        }
        public async Task<IActionResult> EGovSSCHousingCommittee()
        {
            var modellist = await _eGovernanceBusiness.GetEGovSSCCommitteeMemberData("EGOV_SSC_COMMITTEE_HOUSING");
            return View(modellist);
        }
        public async Task<IActionResult> EGovSSCPublicHealthCommittee()
        {
            var modellist = await _eGovernanceBusiness.GetEGovSSCCommitteeMemberData("EGOV_SSC_COMMITTEE_HEALTH");
            return View(modellist);
        }
        public IActionResult EGovSSCSocialJusticeCommittee()
        {
            return View();
        }
        public IActionResult EGovSSCSwachhBharatCommittee()
        {
            return View();
        }
        public async Task<IActionResult> EGovSSCSanitationReforms()
        {
            var data = await _eGovernanceBusiness.GetVideoGallery("SSC_VIDEO_SANITATION");
            return View(data);
        }
        public async Task<IActionResult> EGovSSCSwachhBharatMission()
        {
            var data = await _eGovernanceBusiness.GetVideoGallery("SSC_VIDEO_SWACHH");
            return View(data);
        }
        public async Task<IActionResult> EGovSSCCovid()
        {
            var data = await _eGovernanceBusiness.GetVideoGallery("SSC_VIDEO_COVID");
            return View(data);
        }
        public async Task<IActionResult> EGovSSCStreetLights()
        {
            var data = await _eGovernanceBusiness.GetVideoGallery("SSC_VIDEO_STREETLIGHT");
            return View(data);
        }
        public async Task<IActionResult> EGovSSCStrayDogsMgmt()
        {
            var data = await _eGovernanceBusiness.GetVideoGallery("SSC_VIDEO_DOGMGMT");
            return View(data);
        }
        public async Task<IActionResult> EGovSSCPostFlood()
        {
            var data = await _eGovernanceBusiness.GetVideoGallery("SSC_VIDEO_POSTFLOOD");
            return View(data);
        }

        public async Task<IActionResult> EGovSSCCityBiodiversity()
        {
            ViewBag.Biodiversity = "";
            var bio = await _eGovernanceBusiness.GetSrinagarSettingData("BIODIVERSITY_INDEX");
            if (bio.IsNotNull())
            {
                if (bio.FileId.IsNotNullAndNotEmpty())
                {
                    ViewBag.Biodiversity = bio.FileId;
                }
            }
            return View();
        }
        public IActionResult EGovSSCDepartmentIT()
        {
            return View();
        }
        public IActionResult JammuLandingPage()
        {
           
            return View();
        }
        public async Task<ActionResult> GetEGovDebrisRateData()
        {
            var data = await _eGovernanceBusiness.GetEGovDebrisRateData();
            if (data.IsNotNull())
            {
                return Json(new { success = true, amount=data }); 
            }
            return Json(new { success = false, error = "error" });
        }
        public async Task<ActionResult> GetEGovPoultryCostData()
        {
            var data = await _eGovernanceBusiness.GetEGovPoultryCostData();
            if (data.IsNotNull())
            {
                return Json(new { success = true, amount = data });
            }
            return Json(new { success = false, error = "error" });
        }
        public async Task<ActionResult> GetEGovSepticTankCostData()
        {
            var data = await _eGovernanceBusiness.GetEGovSepticTankCostData();
            if (data.IsNotNull())
            {
                return Json(new { success = true, amount = data });
            }
            return Json(new { success = false, error = "error" });
        }
        public async Task<ActionResult> GetEGovBinBookingCostData(string binSizeId)
        {
            var data = await _eGovernanceBusiness.GetEGovBinBookingCostData(binSizeId);
            if (data.IsNotNull())
            {
                return Json(new { success = true, amount = data });
            }
            return Json(new { success = false, error = "error" });
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
            var colorList = new List<string> { "bg-gradient-red-yellow", "bg-gradient-aqua", "bg-gradient-green", "bg-gradient-red", "bg-gradient-purple", "bg-gradient-blue" };
            var iconColorList = new List<string> { "bg-solid-yellow", "bg-solid-aqua", "bg-solid-green", "bg-solid-red", "bg-solid-purple", "bg-solid-blue" };
            int i = 0;
            foreach (var item in result.GroupBy(x => x.TemplateCode))
            {
                int idx = i % 6;
                list.Add(new customIndexPageTemplateViewModel
                {
                    ServiceName = item.Select(x => x.TemplateDisplayName).FirstOrDefault(),
                    TemplateCode = item.Select(x => x.TemplateCode).FirstOrDefault(),
                    InProgressCount = item.Count(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS" || x.ServiceStatusCode == "SERVICE_STATUS_OVERDUE"),
                    CompletedCount = item.Count(x => x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE"),
                    RejectedCount = item.Count(x => x.ServiceStatusCode == "SERVICE_STATUS_REJECT" || x.ServiceStatusCode == "SERVICE_STATUS_CANCEL"),
                    TemplateIconId = item.Select(x => x.IconFileId).FirstOrDefault(),
                    TemplateColorCss = colorList[idx],
                    TemplateIconColorCss = iconColorList[idx]
                }); ;
                i++;
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
            
                var cs = await _companySettingBusiness.GetSingle(x => x.Code == "SMART_CITY_URL");
                ViewBag.SmartCityUrl = cs?.Value;
            
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
        public async Task<IActionResult> DraftedServiceList(string statusCodes, string templateCode, string categoryCode)
        {
            ViewBag.UserId = _userContext.UserId;
            ViewBag.StatusCodes = statusCodes;
            //var portal = await _portalBusiness.GetSingle(x => x.Name == "EGovCustomer");
            return View(new CustomIndexPageTemplateViewModel
            {
                TemplateCodes = templateCode,
                CategoryCodes = categoryCode,
                /*PortalId = portal.Id,*///"42999855-8942-4bc4-8715-fc67b9df718a",
            }) ;
        }
        public async Task<IActionResult> ReadDraftedServiceList(string statusCodes, string templateCode,string categoryCode)
        {
            var dt = await _eGovernanceBusiness.GetDraftedServiceList(statusCodes, templateCode, categoryCode);
            return Json(dt);
        }

        public async Task<IActionResult> ReadServiceList(string statusCodes, string templateCode)
        {
            var dt = await _eGovernanceBusiness.GetServiceList(statusCodes, templateCode);
            return Json(dt);
        }

        public async Task<IActionResult> ReadServiceTemplate(string templateCode)
        {
            var result = await _templateBusiness.GetTemplateServiceList(templateCode, null, null, null, null, TemplateCategoryTypeEnum.Standard, false,null,ServiceTypeEnum.StandardService);

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

        public async Task<IActionResult> MyPayments(string portalNames = null)
        {
            ViewBag.PortalNames = portalNames;
            
                var cs = await _companySettingBusiness.GetSingle(x => x.Code == "SMART_CITY_URL");
                ViewBag.ReturnUrl = cs.Value;
            
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

        public IActionResult UpcomingProjects()
        {
            var userrole = _userContext.UserRoleCodes.IsNullOrEmpty() ? new string[] { } : _userContext.UserRoleCodes.Split(",");
            ViewBag.IsUserNeedsWants = userrole.Contains("EGOV_NEEDS_WANTS");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUpcomingProject(string id,string serviceId)
        {
            var data = await _eGovernanceBusiness.DeleteUpcomingProject(id);
            await _serviceBusiness.Delete(serviceId);
            return Json(new { success=true });
        }
        public async Task<IActionResult> ReadUpcomingProjectDepartmentData(string categoryId, string wardId)
        {
            var data = new List<EGovProjectViewModel>();
            //data.Add(new EGovProjectViewModel {Id="D1",ServiceNo="SS-INC0212",ProjectName="Road Pothholes",ProjectDescription="Description about the project goes here",RequestedBy="Mr Chenaa Reddy", RequestedDate= new DateTime(2022,3,11),Status="Active" });
            //data.Add(new EGovProjectViewModel {Id="D2",ServiceNo="SS-INC0213",ProjectName="Street Lights",ProjectDescription="Description about the project goes here",RequestedBy="Mr Chinmoy", RequestedDate= new DateTime(2022,3,11),Status="Active" });
            //data.Add(new EGovProjectViewModel {Id="D3",ServiceNo="SS-INC0214",ProjectName="SChool Building",ProjectDescription="Description about the project goes here",RequestedBy="Mr Binod Bhatia", RequestedDate= new DateTime(2022,3,11),Status="Active" });
            //data.Add(new EGovProjectViewModel {Id="D4",ServiceNo="SS-INC0215",ProjectName="Stray Dogs",ProjectDescription="Description about the project goes here",RequestedBy="Mr Vikram Singh", RequestedDate= new DateTime(2022,3,11),Status="Active" });
            data = await _eGovernanceBusiness.GetUpcomingProjectList(categoryId, wardId);
            var result = data.Where(x => x.IsProposedByCitizen == false);
            return Json(result);
        }
        public async Task<IActionResult> ReadUpcomingProjectCitizenData(string categoryId, string wardId)
        {
            var data = new List<EGovProjectViewModel>();
            //data.Add(new EGovProjectViewModel { Id = "C1", ServiceNo = "SS-INC0212", ProjectName = "Road Pothholes", ProjectDescription = "Description about the project goes here", RequestedBy = "Mr Chinmoy", RequestedDate = new DateTime(2022, 3, 1), Status = "Active",Like=26 });
            //data.Add(new EGovProjectViewModel { Id = "C2", ServiceNo = "SS-INC0213", ProjectName = "Street Lights", ProjectDescription = "Description about the project goes here", RequestedBy = "Mr Binod Bhatia", RequestedDate = new DateTime(2022, 3, 9), Status = "Active",Like=52 });
            //data.Add(new EGovProjectViewModel { Id = "C3", ServiceNo = "SS-INC0214", ProjectName = "School Building", ProjectDescription = "Description about the project goes here", RequestedBy = "Mr Vikram Singh", RequestedDate = new DateTime(2022, 3, 11), Status = "Active",Like=101 });
            data = await _eGovernanceBusiness.GetUpcomingProjectList(categoryId, wardId);
            var result = data.Where(x => x.IsProposedByCitizen == true);
            return Json(result);
        }
        public IActionResult CommunityHallHome()
        {
            return View();
        }
        public IActionResult CommunityHallJammu()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ReadCommunityHallData()
        {
            var data = await _eGovernanceBusiness.GetCommunityHallList();
            return Json(data);
        }
        public async Task<IActionResult> NewDashboard()
        {
            var model = new NewDashboardViewModel()
            {
                ProjectCount= 5009,
                FinishedCount = 809,
                LikeCount = 119,
                CommentCount = 27,
            };
           
          return View(model);
        }
        //public async Task<IActionResult> DashboardList()
        //{
        //    var model = new List<NewDashboardViewModel>();
        //    model.Add(new NewDashboardViewModel()
        //    {
        //        Date = "JAN 21 2021",
        //        DateTime = "Jan 21, 2021 @ 5:00 pm - 7:00 pm",
        //        Place = "Bagnan Koth",
        //        Description = "Project Description related text goes here along with brief details",
        //    });
           

        //    return Json(model);
        //}
        [HttpGet]
        public async Task<IActionResult> DashboardList()
        {
            var data = await _eGovernanceBusiness.GetDashboardList();
            return Json(data);
        }

        public async Task<IActionResult> CitizenProjectsHome()
        {
            var UserId = _userContext.UserId;
            var data = await _eGovernanceBusiness.GetWardData(UserId);
            var data1 = await _eGovernanceBusiness.GetNWTimeLineData();
            foreach (var item in data1)
            {
                if (DateTime.Now >= item.FromDate && DateTime.Now <= item.ToDate)
                {
                    ViewBag.Exist = true;
                    ViewBag.Message = item.IsNotNull() ? item.Message : null;
                    ViewBag.FromDate = item.FromDate;
                    ViewBag.ToDate = item.ToDate;

                }
                else { ViewBag.Exist = false; }
            }
            ViewBag.UserId = UserId;
            ViewBag.WardName = data.IsNotNull() ? data.WardName : null; 
            ViewBag.WardId = data.IsNotNull() ? data.WardId : null;
            ViewBag.IsAdminDeleteComment = false;
            if (_userContext.UserRoleCodes.IsNotNull() && _userContext.UserRoleCodes.Contains("EGOV_NEEDS_WANTS"))
            {
                ViewBag.IsAdminDeleteComment = true;
            }

            var portal = await _cmsBusiness.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
            if (portal?.Name == "EGovCustomer")
            {
                var cs = await _companySettingBusiness.GetSingle(x => x.Code == "SMART_CITY_URL");
                ViewBag.SmartCityUrl = cs.Value;
            }

            return View(data);
            //return View(new EGovDashboardViewModel() { UserId=_userContext.UserId });
        }

        public async Task<IActionResult> CititzenProjectsHomeList(string type,string userId, DateTime fromDate, DateTime toDate)
        {
            var model = await _eGovernanceBusiness.GetProposalProjectsList(type, userId);
            //var list = new List<EGovDashboardViewModel>();
            foreach (var i in model)
            {

                //if (i.RequestedDate >= fromDate && i.RequestedDate <= toDate)
                //{
                //    list.Add(i);

                //}
                i.DisplayRequestedDate = i.RequestedDate.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateFormat);

            }
            return Json(model);
        }

        public async Task<IActionResult> AllProjectsHomeList(DateTime fromDate, DateTime toDate)
        {
            var model = await _eGovernanceBusiness.GetAllProposalProjectsList();
            foreach (var i in model)
            {
                i.DisplayRequestedDate = i.RequestedDate.ToString(ApplicationConstant.DateAndTime.DefaultJqueryDateFormat);

            }
            return Json(model);
        }

        public async Task<IActionResult> UpdateProjectProposalLikes(string proposalId, ProjectPropsalResponseEnum type, string userId)
        {
            var exist = await _eGovernanceBusiness.GetProposalLikesData(proposalId, type, userId);

            if (exist.IsNotNull() && exist.ResponseType == type)
            {
                await _eGovernanceBusiness.UpdateProjectProposalLikes(proposalId,null, userId,DataActionEnum.Edit);
            }
            else if (exist.IsNotNull() && exist.ResponseType != type)
            {
                await _eGovernanceBusiness.UpdateProjectProposalLikes(proposalId, type, userId, DataActionEnum.Edit);
            }
            else
            {
                await _eGovernanceBusiness.UpdateProjectProposalLikes(proposalId, type, userId, DataActionEnum.Create);
            }

            return Json(new { success=true});
        }

        public IActionResult EmployeeProjectsHome()
        {
            return View(new EGovDashboardViewModel() { UserId = _userContext.UserId });
        }

        public IActionResult CitizenProjectsProposalDashboard()
        {
            return View();
        }
        public async Task<IActionResult> ViewProjectsUnderTaken(string serviceId)
        {
            var list = await _eGovernanceBusiness.ViewProjectsUnderTaken(serviceId);

            return View(list);
        }

        public async Task<IActionResult> GetChartByProjectCategory()
        {
            //var list = await _eGovernanceBusiness.GetProposalProjectsList(null,null);

            //var data = list.GroupBy(x => x.ProjectCategory).Select(g => g.Key).ToList();

            //var newlist = new List<EGovDashboardViewModel>();

            //foreach (var d in data)
            //{
            //    var count = list.Where(x => x.ProjectCategory == d).Count();
            //    var catName = list.Where(x => x.ProjectCategory == d).Select(x => x.ProjectCategoryName).FirstOrDefault();
            //    newlist.Add(new EGovDashboardViewModel() { LikesCount = count,ProjectCategoryName = catName });
            //}

            //var chartdata = new EGovDashboardViewModel
            //{
            //    ItemValueLabel = newlist.Select(x => x.ProjectCategoryName).ToList(),
            //    ItemValueSeries = newlist.Select(x => x.LikesCount).ToList()
            //};
            //return Json(chartdata);

            var list = await _eGovernanceBusiness.GetProposalProjectsCount("Category");
            list = list.OrderByDescending(x => x.ProjectsCount).Take(10).ToList();

            var chartdata = new EGovDashboardViewModel
            {
                ItemValueLabel = list.Select(x => x.ProjectCategoryName).ToList(),
                ItemValueSeries = list.Select(x => x.ProjectsCount).ToList()
            };
            return Json(chartdata);
        }

        public async Task<IActionResult> GetChartByPopularProjects()
        {
            var list = await _eGovernanceBusiness.GetProposalProjectsList(null, null);

            var data = list.GroupBy(x => x.ProjectCategory).Select(g => new EGovDashboardViewModel{ProjectCategory = g.Key,LikesCount=g.Sum(x=>x.LikesCount),ProjectCategoryName=g.FirstOrDefault().ProjectCategoryName } ).ToList();

            data = data.Where(x => x.LikesCount > 0).ToList();
            data = data.OrderByDescending(x => x.LikesCount).Take(10).ToList();

            var chartdata = new EGovDashboardViewModel
            {
                ItemValueLabel = data.Select(x => x.ProjectCategoryName).ToList(),
                ItemValueSeries = data.Select(x => x.LikesCount).ToList()
            };
            return Json(chartdata);
        }
        public async Task<IActionResult> GetChartByProjectsUndertaken()
        {
            var list = await _eGovernanceBusiness.GetProposalProjectsCount("undertaken");
            var chartdata = new EGovDashboardViewModel
            {
                ItemValueLabel = list.Select(x => x.ProjectCategoryName).ToList(),
                ItemValueSeries = list.Select(x => x.ProjectsCount).ToList()
            };
            return Json(chartdata);
        }
        public async Task<IActionResult> GetChartByProjectsStatus()
        {
            var list = await _eGovernanceBusiness.GetProposalProjectsCount();

            var chartdata = new EGovDashboardViewModel
            {
                ItemValueLabel = list.Where(x => x.ProjectStatusCode.IsNotNullAndNotEmpty()).Select(x => x.ProjectStatus).ToList(),
                ItemValueSeries = list.Where(x => x.ProjectStatusCode.IsNotNullAndNotEmpty()).Select(x => x.ProjectsCount).ToList(),
                ItemStatusColor = list.Where(x => x.ProjectStatusCode.IsNotNullAndNotEmpty()).Select(x => x.StatusColor).ToList()
            };
            return Json(chartdata);
        }

        public async Task<IActionResult> GetChartByProjectLocation()
        {
            var list = await _eGovernanceBusiness.GetProposalProjectsCount("Location");

            var chartdata = new EGovDashboardViewModel
            {
                ItemValueLabel = list.Select(x => x.LocationName).ToList(),
                ItemValueSeries = list.Select(x => x.ProjectsCount).ToList()
            };
            return Json(chartdata);
        }

        public async Task<IActionResult> GetChartByProjectCategories()
        {
            var list = await _eGovernanceBusiness.GetProposalProjectsCount("Category");

            var chartdata = new EGovDashboardViewModel
            {
                ItemValueLabel = list.Select(x => x.ProjectCategoryName).ToList(),
                ItemValueSeries = list.Select(x => x.ProjectsCount).ToList()
            };
            return Json(chartdata);
        }

        public async Task<IActionResult> NeedsAndWantsTaskList(string categoryCodes, string portal, bool showAllTaskForAdmin = true)
        {            
            if (portal == "EGOV")
            {
                var cs = await _companySettingBusiness.GetSingle(x => x.Code == "SMART_CITY_URL");
                ViewBag.ReturnUrl = cs.Value;
            }
            ViewBag.CategoryCodes = categoryCodes;
            ViewBag.ShowAllTaskForAdmin = showAllTaskForAdmin;

            return View();
        }

        public async Task<IActionResult> ReadNeedsAndWantsTaskListCount(string categoryCodes, bool showAllTaskForAdmin = true)
        {
            var result = await _eGovernanceBusiness.GetNeedsAndWantsTaskCount(categoryCodes, _userContext.PortalId, showAllTaskForAdmin);
            var j = Json(result);
            return j;
        }

        public async Task<IActionResult> ReadNeedsAndWantsTaskData(string categoryCodes, string taskStatus, bool showAllTaskForAdmin = true)
        {
            var list = await _eGovernanceBusiness.GetNeedsAndWantsTaskList(categoryCodes, taskStatus, _userContext.PortalId, showAllTaskForAdmin);
            
            var j = Json(list);
            return j;
        }

        public IActionResult EscalatedToMe(string portalNames)
        {
            ViewBag.PortalNames = portalNames;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCommentToEscalatedData(string id,string comment)
        {
            var data = await _stepTaskEscalationDataBusiness.GetSingleById(id);
            if(data.IsNotNull() && comment.IsNotNullAndNotEmpty())
            {
                data.EscalationComment = comment;
                data.DataAction = DataActionEnum.Edit;
                var res = await _stepTaskEscalationDataBusiness.Edit(data);
                if (res.IsSuccess)
                {
                    return Json(new { success = true });
                }
            }
            
            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<IActionResult> GetTaskListWithEscalationData(string portalNames)
        {
            //var portalId = _userContext.PortalId;
            var userId = _userContext.UserId;
            var dlist = await _stepTaskEscalationDataBusiness.GetPortalTaskListWithEscalationData(portalNames, userId);
            return Json(dlist);
        }

        public IActionResult MyTasksEscalated(string portalNames)
        {
            ViewBag.PortalNames = portalNames;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetMyTasksEscalated(string portalNames)
        {
           // var portalId = _userContext.PortalId;
            var userId = _userContext.UserId;
            var dlist = await _stepTaskEscalationDataBusiness.GetMyTasksEscalatedDataList(portalNames, userId);
            return Json(dlist);
        }

        public IActionResult AllEscalatedTasks(string portalNames)
        {
            ViewBag.PortalIds = portalNames;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEscalatedTasks(string portalIds)
        {
            var dlist = await _stepTaskEscalationDataBusiness.AllEscalatedTasks(portalIds);
            return Json(dlist);
        }

        public async Task<IActionResult> ExportAllEscalatedTasks(string portalIds)
        {

            var ms = await _stepTaskEscalationDataBusiness.GetExcelForTemplateData(portalIds);
            var report = string.Concat("AllEscalatedTasks", ".xlsx");
            return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", report);

        }


        public IActionResult PaymentCustom()
        {
            return View();
        }

        public IActionResult CustomerDashboardJammu()
        {
            return View();
        }
        public async Task<IActionResult> GetJSCAssetsDataByConsumer(string userId, string wardId = null,string assetTypeId = null)
        {
            var _sctBusiness = _serviceProvider.GetService<ISmartCityBusiness>();
            var data = await _sctBusiness.GetJSCAssetParcelListByUser(userId);
            //var data = await _eGovernanceBusiness.GetJSCAssetsDataByConsumer(userId);
            if (!wardId.IsNullOrEmpty())
            {
                data = data.Where(x => x.ward_no == wardId).ToList();
            }
            //if (!assetTypeId.IsNullOrEmpty())
            //{
            //    data = data.Where(x => x.AssetTypeId == assetTypeId).ToList();
            //}
            return Json(data);
        }

        public async Task<IActionResult> GetWardListForJammu()
        {
            var data = await _eGovernanceBusiness.GetWardListFromMaster();
            return Json(data);
        }
        
        public async Task<IActionResult> GeCollectorListForJammu()
        {
            var data = await _eGovernanceBusiness.GeCollectorListForJammu();
            return Json(data);
        }
        
        public async Task<IActionResult> GetAssetTypeListForJammu()
        {
            var data = await _eGovernanceBusiness.GetAssetTypeListForJammu();
            return Json(data);
        }

        public IActionResult CollectorAssignment()
        {
            return View();
        }

        public async Task<IActionResult> CreateCollectorWardAssignment(string id = null)
        {
            CollectorWardAssignmentViewModel model = new();
            if (id.IsNotNullAndNotEmpty())
            {
                model = await _eGovernanceBusiness.GetWardCollectorById(id);
                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
            }
            
            return View(model);
        }

        public async Task<IActionResult> ManageCollectorAssignmentToWard(CollectorWardAssignmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                if(model.DataAction == DataActionEnum.Create)
                {
                    var formTempModel = new FormTemplateViewModel();
                    formTempModel.DataAction = DataActionEnum.Create;
                    formTempModel.TemplateCode = "JSC_COLLECTOR_WARD_ASSIGNMENT";
                    var formmodel = await _cmsBusiness.GetFormDetails(formTempModel);
                    formmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var res = await _cmsBusiness.ManageForm(formmodel);
                    if (res.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
                else
                {
                    var formTempModel = new FormTemplateViewModel();
                    formTempModel.DataAction = DataActionEnum.Edit;
                    formTempModel.TemplateCode = "JSC_COLLECTOR_WARD_ASSIGNMENT";
                    //formTempModel.Id = 
                    var formmodel = await _cmsBusiness.GetFormDetails(formTempModel);
                    formmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var res = await _cmsBusiness.ManageForm(formmodel);
                    if (res.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
            }
            
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }

        public async Task<IActionResult> GetCollectorListForJammu()
        {
            var data = await _eGovernanceBusiness.GetJammuCollectorList();
            return Json(data);
        }
        
        public async Task<IActionResult> GetCollectorUnAssignedWardListForJammu()
        {
            var all = await _eGovernanceBusiness.GetWardListFromMaster();
            var assigned = await _eGovernanceBusiness.GetAssignedWardCollectorList();
            var unAssignedWardList = all.Where(p => !assigned.Any(p2 => p2.WardId == p.Id));
            return Json(unAssignedWardList);
        }

        public async Task<IActionResult> GetWardCollectAssignedList()
        {
            var list = await _eGovernanceBusiness.GetAssignedWardCollectorList();
            return Json(list);
        }

        public async Task<IActionResult> DeleteWardCollector(string id)
        {
            await _eGovernanceBusiness.DeleteWardCollector(id);
            return Json(new { success = true });
        }
        public IActionResult RevenueReport(string source)
        {
            ViewBag.Source = source;
            return View();
        }
        public IActionResult DefaulterReport(string source)
        {
            ViewBag.Source = source;
            return View("RevenueReport");
        }
        public async Task<IActionResult> GetRevenueReport(string source, string assetType, string ward, DateTime? From, DateTime? To)
        {
            var list = await _eGovernanceBusiness.GetAssetPaymentReport2(source,assetType, ward,From,To);
            return Json(list);
        }
       

        public async Task<IActionResult> GetUnallocationUserList()
        {
            var data = await _eGovernanceBusiness.GetUnallocationUserList();
            return Json(data);
        }

        public async Task<IActionResult> GetUnAllocatedAssetFilterByFromnToDate(string fromDate, string toDate)
        {
            var data = await _eGovernanceBusiness.GetUnAllocatedAssetFilterByFromnToDate(fromDate, toDate);
            return Json(data);
        }

        public async Task<IActionResult> GetUnAllocatedWard()
        {
            var data = await _eGovernanceBusiness.GetUnAllocatedWard();
            return Json(data);
        }
        public async Task<IActionResult> VideoGallery()
        {
            var data = await _eGovernanceBusiness.GetVideoGallery("SSC_VIDEO_GALLERY");
            return View(data);
        }
        public async Task<IActionResult> ServiceTemplateTilesHome(string templateCode, string categoryCode, string userId, string moduleCodes, string prms, string cbm, string templateIds, string categoryIds, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, string portalId = null)
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
            if (portalId.IsNotNullAndNotEmpty())
            {

                var portal = await _cmsBusiness.GetSingleById<PortalViewModel, Portal>(portalId);
                if (portal?.Name == "EGovCustomer")
                {
                    var cs = await _companySettingBusiness.GetSingle(x => x.Code == "SMART_CITY_URL");
                    ViewBag.SmartCityUrl = cs.Value;
                }
            }
            model.Notifications = await _eGovernanceBusiness.GetEGovSSCNotificationData();
            model.Tenders = await _eGovernanceBusiness.GetEGovSSCTenderData();
            model.OrderCirculars = await _eGovernanceBusiness.GetEGovSSCOrderCircularData();
            var modules = await _templateCategoryBusiness.GetModuleBasedCategory(templateCode, categoryCode, moduleCodes, templateIds, categoryIds, TemplateTypeEnum.Service, categoryType);
            model.ModuleList = modules.GroupBy(x=>x.ModuleId).Select((value,index) => new IdNameViewModel
            {
                 ClassName= index%2==0? "services-bg text-white" : "bg-cream text-theme-blue",
                 Name=value.Max(y=>y.ModuleName),
                 Code= value.Max(y => y.ModuleCode),
                Id = value.Max(y => y.ModuleId),
            }).ToList();
            return View(model);
        }
        public async Task<IActionResult> ServiceTemplateTiles(string templateCode, string categoryCode, string userId, string moduleCodes, string prms, string cbm, string templateIds, string categoryIds, TemplateCategoryTypeEnum categoryType = TemplateCategoryTypeEnum.Standard, string portalId = null)
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
            if (portalId.IsNotNullAndNotEmpty())
            {

                var portal = await _cmsBusiness.GetSingleById<PortalViewModel, Portal>(portalId);
                if (portal?.Name == "EGovCustomer")
                {
                    var cs = await _companySettingBusiness.GetSingle(x => x.Code == "SMART_CITY_URL");
                    ViewBag.SmartCityUrl = cs.Value;
                }
            }
            return View(model);
        }

        public async Task<IActionResult> RightToInformation()
        {
            var data = await _eGovernanceBusiness.GetSrinagarSettingData("RTI_ACT");
            ViewBag.FileId="";
            if (data.IsNotNull())
            {
                ViewBag.FileId = data.FileId;
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetSrinagarSettingWithCode(string code)
        {
            var data = await _eGovernanceBusiness.GetSrinagarSettingData(code);
            return Json(data);
        }

        public IActionResult EGovUserRegistration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EGovUserRegistration(EGovUserViewModel model)
        {

            if (!ModelState.IsValid)
            {

                return Json(new { success = false, error = "Please enter valid details" });
            }
            var result = await RegisterEGovUser(model);

            if (result.Item1)
            {
                return Json(new { success = true, url = result.Item2, ex = result.Item3, dataJson = result.Item4, signupUrl = result.Item5 });
            }
            return Json(new { success = false, error = result.Item2, ex = result.Item3, dataJson = result.Item4, signupUrl = result.Item5 });
        }

        private async Task<Tuple<bool, string, string, string , string>> RegisterEGovUser(EGovUserViewModel model)
        {

            var baseAuthUrlItem = await _companySettingBusiness.GetSingle(x => x.Code == "SMARTCITY_AUTH_BASE_URL");
            var baseAuthUrl = "";
            if (baseAuthUrlItem != null)
            {
                baseAuthUrl = baseAuthUrlItem.Value;
            }
            // signinurl to be changed
            var signUpUrl = $"{baseAuthUrl.Trim('/')}/createuser";
            //var loginData = new EGovUserViewModel { userId = model.UserId, password = model.Password };
            model.nationality = "Indian";
            model.departmentId = "CIVILIAN";
            string[] userRole = { "CITIZEN" };
            model.role = userRole;

            var registrationDataJson = Newtonsoft.Json.JsonConvert.SerializeObject(model/*loginData*/, Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });

            var response = "";
            try
            {
                response = await Helper.HttpRequest(signUpUrl, HttpVerb.Post, registrationDataJson, true);
            }
            catch (Exception e)
            {

                return new Tuple<bool, string, string, string, string>(false, "Error in Registration. Please contact Administrator", e.ToString(), "", "");
            }

            var token = JToken.Parse(response);
            var createUser = token.SelectToken("createuser");

            if (Convert.ToString(createUser) != "SUCCESS")
            {
                var reason = token.SelectToken("reason",false);
                return new Tuple<bool, string, string, string, string>(false, "User Registration Failed", response, registrationDataJson, signUpUrl);
            }
            return new Tuple<bool, string, string, string, string>(true, "User successfully registered", response, registrationDataJson, signUpUrl);

        }

    }
}
