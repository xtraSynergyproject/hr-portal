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
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CMS.UI.Web.Areas.CHR.Controllers
{
    [Area("CHR")]
    public class LeaveController : ApplicationController
    {
        private readonly IHRCoreBusiness _hrCoreBusiness;
        private readonly IUserContext _userContext;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private IUserHierarchyBusiness _userHierarchyBusiness;
        private readonly ILeaveBalanceSheetBusiness _leaveBalanceSheetBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly ISalaryInfoBusiness _salaryInfoBusiness;
        public LeaveController(IHRCoreBusiness hrCoreBusiness, IUserContext userContext, IUserHierarchyBusiness userHierarchyBusiness
            , ITableMetadataBusiness tableMetadataBusiness, ILeaveBalanceSheetBusiness leaveBalanceSheetBusiness, IServiceBusiness serviceBusiness,
            ISalaryInfoBusiness salaryInfoBusiness)
        {
            _hrCoreBusiness = hrCoreBusiness;
            _userContext = userContext;
            _userHierarchyBusiness = userHierarchyBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _leaveBalanceSheetBusiness = leaveBalanceSheetBusiness;
            _serviceBusiness = serviceBusiness;
            _salaryInfoBusiness = salaryInfoBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> LeaveDetail(string userId, string personId)
        {
            //userId = userId.IsNullOrEmpty() ? _userContext.UserId : userId;
            var model = new LeaveDetailViewModel { UserId = userId };
            if (userId.IsNotNullAndNotEmpty())
            {
                await _leaveBalanceSheetBusiness.UpdateLeaveBalance(DateTime.Now, "", userId);
                var leavebalance = await _tableMetadataBusiness.GetTableDataByColumn("LeaveBalanceSheet", null, "UserId", userId);
                if (leavebalance != null)
                {

                    model.LeaveBalance = Convert.ToDouble(leavebalance["ClosingBalance"]);
                }
            }


            var contract = await _tableMetadataBusiness.GetTableDataByColumn("HRContract", null, "EmployeeId", personId);
            if (contract != null)
            {
                model.YearlyEntitlement = Convert.ToString(contract["AnnualLeaveEntitlement"]);
            }
            //var per = await _hrCoreBusiness.GetPersonDetail(userId);
            //_leavebalanceBusiness.UpdateLeaveBalance(DateTime.Today, "", userId);
            //var prms = new Dictionary<string, object>
            //{
            //    { "PersonId", per!=null?per.PersonId:0 },
            //    { "userId", userId }
            //};

            //var leavebal = await _hrCoreBusiness.GetLeaveBalance(userId);

            //if (leavebal != null)
            //{
            //    model.LeaveBalance =  leavebal.ClosingBalance;
            //}

            //var entitlement = await _hrCoreBusiness.GetContractDetail(per != null ? per.PersonId : "");

            //if (entitlement != null)
            //{
            //    model.YearlyEntitlement = entitlement.AnnualLeaveEntitlement;
            //}
            // ViewBag.ReturnToWorkTemplateCode = LegalEntityHelper.GetReturnToWorkTemplateCode(LoggedInUserLegalEntityCode);
            // ViewBag.LeaveCancelTemplateCode = LegalEntityHelper.GetLeaveCancelTemplateCode(LoggedInUserLegalEntityCode);
            return PartialView(model);
        }
        public async Task<ActionResult> ReadLeaveDetailData( string userId)
        {
            //userId = userId.IsNullOrEmpty() ? _userContext.UserId : userId;
            var model = await _hrCoreBusiness.GetLeaveDetail(userId);
            var j = Json(model);
            return j;
        }

        public async Task<ActionResult> AnnualLeaveBalanceProjections(string userId)
        {
            await _leaveBalanceSheetBusiness.UpdateLeaveBalance(DateTime.Now, "", userId);
            var LoggedInUserLegalEntityCode = _userContext.LegalEntityCode;

            var leavebal = await _tableMetadataBusiness.GetTableDataByColumn("LeaveBalanceSheet", null, "UserId", userId);

            var model = new LeaveDetailViewModel { UserId = userId };
            model.StartDate = DateTime.Today;
            if (leavebal != null)
            {
                model.LeaveBalance = Convert.ToDouble(leavebal["ClosingBalance"]);
            }
            model.LeaveType = Helper.GetAnnualLeaveTemplateCode(LoggedInUserLegalEntityCode);
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> FutureAnnualLeaveBalance(DateTime date, string leaveTypeCode, string userId)
        {
            var futureBalance = await _leaveBalanceSheetBusiness.GetLeaveBalanceWithFutureEntitlement(date, leaveTypeCode, userId);
            if (futureBalance.IsNotNull())
            {
                futureBalance = Math.Round(futureBalance, 2);
                return Json(new { success = true, balance = futureBalance });
            }
            else
            {
                ModelState.AddModelError("LeaveBalance", "On given projection date, Annual leave balance failed.");
                return Json(new { success = false, errors = ModelState.SerializeErrors() });
            }

        }
        public async Task<ActionResult> GetAnnualLeaveEntitlement(ServiceViewModel viewModel)
        {
            var entitlement = await _leaveBalanceSheetBusiness.GetEntitlement("ANNUAL_LEAVE", viewModel.OwnerUserId);
            return Json(new { Code = entitlement, Name = entitlement });
        }
        [HttpGet]
        public async Task<ActionResult> GetAnnualLeaveDetails(string userId, DateTime vacationStart, DateTime vacationEnd)
        {

            var _leaveRequired = await _leaveBalanceSheetBusiness.GetLeaveDuration(userId, vacationStart, vacationEnd);
            var _leaveDuration = (vacationEnd - vacationStart).TotalDays + 1;
            var _isEligibleExitReEntryVisa = await _serviceBusiness.IsExitEntryFeeAvailed(userId);
            var _salaryInfoModel = await _salaryInfoBusiness.GetEligiblityForTickets(userId);
            bool _IsEligibleForAirTicketForSelf = false;
            bool _IsEligibleForAirTicketForDependant = false;
            if (_salaryInfoModel.IsNotNull())
            {
                _IsEligibleForAirTicketForSelf = _salaryInfoModel.IsEmployeeEligibleForFlightTicketsForSelf ?? false;
                _IsEligibleForAirTicketForDependant = _salaryInfoModel.IsEmployeeEligibleForFlightTicketsForDependants ?? false;
            }
            var _ticketCost = await _hrCoreBusiness.GetAirTicketCostByUser(userId);
            var j = Json(new
            {
                leaveDuration = _leaveDuration,
                leaveRequired = _leaveRequired,
                isEligibleExitEntryFee = _isEligibleExitReEntryVisa,
                isEligibleTicketSelf = _IsEligibleForAirTicketForSelf,
                isEligibleTicketDependent = _IsEligibleForAirTicketForDependant,
                ticketCost = _ticketCost

            });
            return j;
        }




        public ActionResult LeaveCancel(string UserId, string ParentServiceId)
        {
            var model = new LeaveCanceViewModel();
            model.UserId = UserId;
            model.ParentServiceId = ParentServiceId;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LeaveCancelSave(string leaveReason, string userId, string ParentSeriveId)
        {
            var model = new LeaveCanceViewModel();
            var serviceTemplate = new ServiceTemplateViewModel();
            serviceTemplate.ActiveUserId = _userContext.UserId;
            serviceTemplate.TemplateCode = "LEAVE_CANCEL";
            var service = await _serviceBusiness.GetServiceDetails(serviceTemplate);

            service.OwnerUserId = userId;

            service.ActiveUserId = _userContext.UserId;
            service.DataAction = DataActionEnum.Create;
            service.ParentServiceId = ParentSeriveId;

            model.CancelReason = leaveReason;



            service.Json = JsonConvert.SerializeObject(model);

            var res = await _serviceBusiness.ManageService(service);

            if (res.IsSuccess)
            { }
            return Json(new { success = false });
        }
        public async Task<ActionResult> EmployeeLeaveSummaryData(ManpowerLeaveSummaryViewModel search = null)
        {

            if (search.PersonId.IsNotNull())
            {
                search.PersonId = search.PersonId;
            }
            var model = await _hrCoreBusiness.GetLeaveRequestDetails(search);
            var j = Json(model);
            return j;
        }

        public ActionResult EmployeeLeaveSummary(string personId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var model = new ManpowerLeaveSummaryViewModel();
            ViewBag.Title = "Employee Leave Summary Report";
            if (personId.IsNotNull())
            {
                model.PersonId = personId;
            }
            model.StartDate = startDate != null ? startDate : DateTime.Today.AddMonths(-1);
            model.EndDate = endDate != null ? endDate : DateTime.Today;
            return View(model);
        }

        public ActionResult LeaveSummary(string userId)
        {
            var model = new LeaveSummaryViewModel { UserId = userId };
            return View(model);
        }

        public async Task<ActionResult> ReadLeaveSummaryData([DataSourceRequest] DataSourceRequest request, string userId)
        {
            var count = await _hrCoreBusiness.GetLeaveSummary(userId);
            var j = Json(count);
            return j;
        }

        public async Task<ActionResult> ReadLeaveSummaryPieData([DataSourceRequest] DataSourceRequest request, string userId)
        {

            var count = await _hrCoreBusiness.GetLeaveSummary(userId);
           var res=count.Where(x => x.Type != null);

            var list = (from doc in res
                        group doc by doc.Type into g
                        select new LeaveSummaryViewModel
                        {
                            Type = g.FirstOrDefault().Type,
                            Count = Convert.ToInt64(g.Sum(x => x.Count))
                        }).ToList();

            var j = Json(list);
            return j;
        }
    }

}
