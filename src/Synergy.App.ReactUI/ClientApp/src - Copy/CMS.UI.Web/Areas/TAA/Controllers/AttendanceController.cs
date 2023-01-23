using CMS.Business;
using CMS.Common;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.TAA.Controllers
{
    [Area("Taa")]
    public class AttendanceController : Controller
    {
        private readonly IAttendanceBusiness _business;
        private readonly IUserContext _userContext;
        private readonly IHRCoreBusiness _hRCoreBusiness;
        private readonly IPayrollBatchBusiness _payrollBatchBusiness;
        public AttendanceController(IAttendanceBusiness business, IUserContext userContext, IPayrollBatchBusiness payrollBatchBusiness
            , IHRCoreBusiness hRCoreBusiness)
        {
            _business = business;
            _userContext = userContext;
            _payrollBatchBusiness = payrollBatchBusiness;
            _hRCoreBusiness = hRCoreBusiness;
        }
        public IActionResult Index(string orgId, DateTime? date)
        {
            var model = new AttendanceViewModel();

            if (date == null)
                model.SearchDate = DateTime.Now.ApplicationNow().Date;
            else
                model.SearchDate = date;

            model.OrganizationId = orgId;

            return View(model);
        }
        [HttpGet]
        public async Task<ActionResult> GetIdNameListWithLegalEntityAsAllOption()
        {
            var data = await _business.GetDepartmentIdNameList();
            //data.Insert(0, new IdNameViewModel { Id = "", Name = ApplicationConstant.PlaceHolder_AllOption });
            return Json(data);
        }
        public async Task<ActionResult> ReadSearchData([DataSourceRequest] DataSourceRequest request, string orgId, DateTime? date)
        {
            var result = await _business.GetAttendanceList(orgId, date);
            var json = Json(result.ToDataSourceResult(request));
            //json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public async Task<ActionResult> OverrideAttendance(string id, string userId, string empName, DateTime? date)
        {
            var model = new AttendanceViewModel();

            if (id.IsNotNullAndNotEmpty())
            {
                //model = await _business.GetSingleById(id);
                model = await _business.GetAttendanceDetailsById(id);
                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
                model.AttendanceDate = date.Value;
            }

            model.UserId = userId;
            model.EmployeeName = empName;
            model.Mode = "Override";

            return PartialView("_OverrideAttendance", model);
        }
        public async Task<ActionResult> UpdateAttendance(string id, string userId, string empName, DateTime? date)
        {
            var model = new AttendanceViewModel();

            if (id.IsNotNullAndNotEmpty())
            {
                //model = _business.GetSingleById(id);
                model = await _business.GetAttendanceDetailsById(id);
                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
                model.AttendanceDate = date.Value;
            }

            model.UserId = userId;
            model.EmployeeName = empName;
            model.Mode = "Update";

            return PartialView("_UpdateAttendance", model);
        }
        [HttpPost]
        public async Task<ActionResult> Manage(AttendanceViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DataAction == DataActionEnum.Create)
                {
                    return await Create(model);
                }
                if (model.DataAction == DataActionEnum.Edit)
                {
                    //var data = _business.GetSingleById(model.Id);
                    //var data = new AttendanceViewModel();
                    var data = await _business.GetAttendanceDetailsById(model.Id);
                    data.OverrideAttendance = model.OverrideAttendance;
                    data.OverrideAttendanceId = model.OverrideAttendanceId;
                    data.OverrideOTHours = model.OverrideOTHours;
                    data.OverrideDeductionHours = model.OverrideDeductionHours;
                    data.OverrideComments = model.OverrideComments;
                    data.EmployeeName = model.EmployeeName;
                    data.UserId = model.UserId;
                    data.Mode = model.Mode;
                    return await Correct(data);
                }

                else if (model.DataAction == DataActionEnum.Delete)
                {
                    return await Delete(model);
                }
                else
                {
                    ModelState.AddModelError("InvalidOperation", "Invalid Operation");
                    return Json(new { success = false, errors = ModelState.SerializeErrors() });
                }
            }
            else
            {
                return Json(new { success = false, errors = ModelState.SerializeErrors() });
            }
        }
        private async Task<ActionResult> Create(AttendanceViewModel model)
        {

            var result = await _business.CreateOverrideAttendance(model);
            if (!result.IsSuccess)
            {
                result.Messages.Each(x => ModelState.AddModelError(x.Key, x.Value));
                return Json(new { success = false, errors = ModelState.SerializeErrors() });
            }
            else
            {
                return Json(new { success = true, operation = model.DataAction.ToString(), ru = model.ReturnUrl, serviceId = model.ServiceId });
            }
        }
        private async Task<ActionResult> Correct(AttendanceViewModel model)
        {
            var result = await _business.CorrectOverrideAttendance(model);
            if (!result.IsSuccess)
            {
                result.Messages.Each(x => ModelState.AddModelError(x.Key, x.Value));
                return Json(new { success = false, errors = ModelState.SerializeErrors() });
            }
            else
            {
                return Json(new { success = true, operation = model.DataAction.ToString(), ru = model.ReturnUrl, serviceId = model.ServiceId });
            }
        }
        private async Task<JsonResult> Delete(AttendanceViewModel model)
        {
            var result = await _business.Delete(model);
            if (!result.IsSuccess)
            {
                result.Messages.Each(x => ModelState.AddModelError(x.Key, x.Value));
                return Json(new { success = false, errors = ModelState.SerializeErrors() });
            }
            else
            {
                return Json(new { success = true, operation = model.DataAction.ToString(), ru = model.ReturnUrl });
            }
        }
        public async Task<ActionResult> AttendancePostToPayroll(string payrollGroupId, string orgId, int? startMonth, int? year)
        {
            var viewModel = new AttendanceViewModel();

            var date = DateTime.Today;

            if (startMonth == null)
                startMonth = date.Month;

            if (year == null)
                year = date.Year;

            if (payrollGroupId != null)
                viewModel.PayrollGroupId = payrollGroupId;

            if (orgId != null)
                viewModel.OrganizationId = orgId;

            viewModel.SearchMonth = startMonth;
            viewModel.Year = year;
            var month = viewModel.SearchMonth.ToSafeInt();

            if (payrollGroupId != null && orgId != null && startMonth != null && year != null)
            {
                //var payGroup = _businessPaygroup.GetSingleById(payrollGroupId);
                var payGroup = await _payrollBatchBusiness.GetPayrollGroupById(payrollGroupId);


                if (viewModel.Year.IsNotNull() && viewModel.SearchMonth.HasValue)
                {

                    var attendanceStartDate = new DateTime(viewModel.Year.Value, month, 1);
                    if (attendanceStartDate.DaysInMonth() < payGroup.CutOffStartDay)
                    {
                        viewModel.SearchStart = new DateTime(viewModel.Year.Value, month, attendanceStartDate.DaysInMonth());
                    }
                    else
                    {
                        viewModel.SearchStart = new DateTime(viewModel.Year.Value, month, payGroup.CutOffStartDay);
                    }
                    var attendanceEndDate = new DateTime(viewModel.Year.Value, month, 1);
                    if (attendanceEndDate.DaysInMonth() < payGroup.CutOffEndDay)
                    {
                        viewModel.SearchEnd = new DateTime(viewModel.Year.Value, month, attendanceEndDate.DaysInMonth());
                    }
                    else
                    {
                        viewModel.SearchEnd = new DateTime(viewModel.Year.Value, month, payGroup.CutOffEndDay);
                    }
                    if (payGroup.IsCutOffStartDayPreviousMonth)
                    {
                        viewModel.SearchStart = viewModel.SearchStart.Value.AddMonths(-1);
                    }

                }


                viewModel.Different = (int)(viewModel.SearchEnd.Value.Date - viewModel.SearchStart.Value.Date).TotalDays;
                var first = viewModel.SearchStart.Value;

                int[] columns = new int[viewModel.Different.Value + 1];

                for (var i = 0; i <= viewModel.Different; i++)
                {
                    columns[i] = first.AddDays(i).Day;
                }
                viewModel.Columns = columns;
            }
            viewModel.PayrollGroupId = payrollGroupId;

            return View(viewModel);
        }
        public async Task<ActionResult> ReadAttendanceForPostPayrollList( string payrollGroupId, string orgId, int? startMonth, int? year)
        {
            var result = new List<AttendanceToPayrollViewModel>();
            if (payrollGroupId != null && orgId != null && startMonth != null && year != null)
            {
                result = await _business.GetAtendanceListForPostPayroll(payrollGroupId, startMonth.Value, year.Value, orgId);
            }
            var json = Json(result);
            //json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public async Task<ActionResult> PostAttendanceToPayroll(string personIds, DateTime startDate, DateTime endDate)
        {
            var result = await _business.PostAttendanceToPayroll(personIds, startDate, endDate);

            if (result.Equals("Success"))
            {
                return Json(new { success = true });

            }
            else
            {
                //ModelState.AddModelError("info", result);
                return Json(new { success = false, errors = result });
            }
        }

        public ActionResult EmployeeAttendence(string userId)
        {
            var model = new TimeinTimeoutDetailsViewModel { UserId = userId, SearchMonth = DateTime.Now.ApplicationNow().Date };
            return View(model);
        }


        public async Task<ActionResult> GetEmployeeAtendanceList([DataSourceRequest] DataSourceRequest request,TimeinTimeoutDetailsViewModel modelV)
        {


            if (modelV.UserId.IsNullOrEmpty())
            {
                modelV.UserId = _userContext.UserId;
            }

            var model = await _business.GetTimeinTimeOutDetails(modelV.UserId, modelV.SearchStart, modelV.SearchEnd, modelV.SearchMonth,modelV.SearchType);

            var j = Json(model.ToDataSourceResult(request));
            return j;

        }

        public ActionResult EmployeeAttendanceReport()
        {
            var model = new AttendanceViewModel();
            model.SearchFromDate = DateTime.Today.AddDays(-1);
            model.SearchToDate = DateTime.Today;
            model.EmployeeStatus = EmployeeStatusEnum.Active;
            return View(model);
        }

        public async Task<ActionResult> ReadAttendanceDetailsByDateData([DataSourceRequest] DataSourceRequest request, List<string> organisationId, List<string> personId, List<string> empStatus, DateTime? searchFromDate, DateTime? searchToDate, string payrollRunId = null)
        {

            var list = await _business.GetAttendanceListByDate(organisationId, personId, searchFromDate, searchToDate, empStatus, payrollRunId);
            var json = Json(list.ToDataSourceResult(request));

            return json;
        }


        public async Task<JsonResult> GetPersonListByOrganization(List<string> orgId)
        {
            var orgIds = "";
            if (orgId.Count>0)
            {
                orgIds = "'" + string.Join("','", orgId) + "'";

            } 
            
            var data = await _hRCoreBusiness.GetPersonListByOrgId(orgIds);
            return Json(data);
        }

        public IActionResult FaceDetectionAttendance()
        {
            return View();
        }
        
        public IActionResult RegisterFace()
        {
            var model = _userContext;
            return View(model);
        }

      
    }
}
