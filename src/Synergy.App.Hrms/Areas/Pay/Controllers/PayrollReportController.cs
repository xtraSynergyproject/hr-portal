using Synergy.App.Business;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Synergy.App.Common;
using Synergy.App.ViewModel;
//using Kendo.Mvc.Extensions;
//using Kendo.Mvc.UI;

namespace CMS.UI.Web.Areas.Pay.Controllers
{
    [Area("Pay")]
    public class PayrollReportController : Controller
    {

        IPayrollRunBusiness _payrollRunBusiness;
        IPayrollBusiness _payrollBusiness;
        ITableMetadataBusiness _tableMetadataBusiness;
        ISalaryInfoBusiness _businessSalaryInfo;
        IExcelReportBusiness _excelReportBusiness;
        public PayrollReportController(IPayrollBusiness payrollBusiness, ITableMetadataBusiness tableMetadataBusiness,
            ISalaryInfoBusiness businessSalaryInfo, IPayrollRunBusiness payrollRunBusiness, IExcelReportBusiness excelReportBusiness)
        {
            _payrollBusiness = payrollBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _businessSalaryInfo = businessSalaryInfo;
            _payrollRunBusiness = payrollRunBusiness;
            _excelReportBusiness = excelReportBusiness;
            
        }
        public ActionResult FlightTicketAccrual(string payrollRunId = null, LayoutModeEnum? layoutMode = null, bool disableSearch = false)
        {

            var year = DateTime.Today.Year;
            var month = (MonthEnum)DateTime.Today.Month;
            var yearmonth = string.Concat(year, DateTime.Today.Month.ToString().PadLeft(2, '0')).ToSafeInt();
            var model = new PayrollReportViewModel { Year = year, Month = month, YearMonth = yearmonth };
            if (layoutMode == LayoutModeEnum.Popup)
            {
                ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
            }
            return View(model);
        }
        public async Task<IActionResult> SickLeaveAccrualReportData(PayrollReportViewModel search = null)
        {
            search.Year = search.Year ?? DateTime.Today.Year;
            search.Month = search.Month ?? (MonthEnum)DateTime.Today.Month;
            var date = new DateTime(search.Year.ToSafeInt(), (int)search.Month.Value, 1);
            search.StartDate = date.FirstDateOfMonth();
            search.EndDate = date.LastDateOfMonth();

            search.ElementId = search.ElementId ?? null;
            if (search.PersonId.IsNotNull())
            {
                search.PersonId = search.PersonId;
            }
            if (search.PayrollId.IsNotNull())
            {
                var pay = await _tableMetadataBusiness.GetTableDataByColumn("PayrollBatch", "", "Id", search.PayrollId);
                if (pay.IsNotNull())
                {
                    search.StartDate = Convert.ToDateTime(pay["PayrollStartDate"]);
                    search.EndDate = Convert.ToDateTime(pay["PayrollEndDate"]);

                }
            }

            search.ElementCode = "'MONTHLY_SICK_LEAVE_ACCRUAL'";
            var model =await  _businessSalaryInfo.GetAccuralDetails(search);
            var j = Json(model.OrderBy(o => o.PersonNo));
            return j;
        }
        public async Task<IActionResult> SickLeaveAccrual(string payrollRunId = null, LayoutModeEnum? layoutMode = null, bool disableSearch = false)
        {
            ViewBag.Title = "Sick Leave Accrual";

            var year = DateTime.Today.Year;
            var month = (MonthEnum)DateTime.Today.Month;
            var yearmonth = string.Concat(year, DateTime.Today.Month.ToString().PadLeft(2, '0')).ToSafeInt();
            if (payrollRunId.IsNotNullAndNotEmpty())
            {
                var payrollRun =await _payrollRunBusiness.GetPayrollRunById(payrollRunId);
                if (payrollRun != null && payrollRun.YearMonth.HasValue)
                {
                    yearmonth = payrollRun.YearMonth.Value;
                    year = yearmonth.ToString().Substring(0, 4).ToSafeInt();
                    month = (MonthEnum)yearmonth.ToString().Substring(4, 2).ToSafeInt();
                }
            }
            if (layoutMode==LayoutModeEnum.Popup) 
            {
                ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
            }
            var model = new PayrollReportViewModel { /*LayoutMode = layoutMode,*/ PayrollRunId = payrollRunId, Year = year, Month = month, YearMonth = yearmonth, DisableSearch = disableSearch };

            return View(model);
            // return PartialView("_SickLeaveAccrual", model);
        }
        public async Task<ActionResult> FlightTicketAccrualReportData(PayrollReportViewModel search = null)
        {
            search.Year = search.Year ?? DateTime.Today.Year;
            search.Month = search.Month ?? (MonthEnum)DateTime.Today.Month;
            var date = new DateTime(search.Year.ToSafeInt(), (int)search.Month.Value, 1);
            search.StartDate = date.FirstDateOfMonth();
            search.EndDate = date.LastDateOfMonth();

            search.ElementId = search.ElementId ?? null;
            if (search.PersonId.IsNotNull())
            {
                search.PersonId = search.PersonId;
            }
            if (search.PayrollId.IsNotNull())
            {
                var pay = await _tableMetadataBusiness.GetTableDataByColumn("PayrollBatch", "", "Id", search.PayrollId);
                if (pay.IsNotNull())
                {
                    search.StartDate = Convert.ToDateTime(pay["PayrollStartDate"]);
                    search.EndDate = Convert.ToDateTime(pay["PayrollEndDate"]);

                }

            }
            search.ElementCode = "'MONTHLY_SELF_TICKET_ACCRUAL','MONTHLY_DEPENDENT_INFANT_TICKET_ACCRUAL','MONTHLY_DEPENDENT_CHILD_TICKET_ACCRUAL','MONTHLY_DEPENDENT_ADULT_TICKET_ACCRUAL','MONTHLY_WIFE_TICKET_ACCRUAL','MONTHLY_HUSBAND_TICKET_ACCRUAL'";
            var model = await _businessSalaryInfo.GetAccuralDetails(search);
            return Json(model);

        }

        public async Task<ActionResult> VacationAccrualReportData(PayrollReportViewModel search = null)
        {
            search.Year = search.Year ?? DateTime.Today.Year;
            search.Month = search.Month ?? (MonthEnum)DateTime.Today.Month;
            var date = new DateTime(search.Year.ToSafeInt(), (int)search.Month.Value, 1);
            search.StartDate = date.FirstDateOfMonth();
            search.EndDate = date.LastDateOfMonth();

            search.ElementId = search.ElementId ?? null;
            if (search.PersonId.IsNotNull())
            {
                search.PersonId = search.PersonId;
            }
            if (search.PayrollId.IsNotNull())
            {
                var pay = await _tableMetadataBusiness.GetTableDataByColumn("PayrollBatch", "", "Id", search.PayrollId);
                if (pay.IsNotNull())
                {
                    search.StartDate = Convert.ToDateTime(pay["PayrollStartDate"]);
                    search.EndDate = Convert.ToDateTime(pay["PayrollEndDate"]);

                }

            }
            search.ElementCode = "'MONTHLY_VACATION_ACCRUAL'";
            var model = await _businessSalaryInfo.GetAccuralDetails(search);
            var j = Json(model);
            return j;
        }

        public async Task<ActionResult> EosAccrualReportData(PayrollReportViewModel search = null)
        {
            search.Year = search.Year ?? DateTime.Today.Year;
            search.Month = search.Month ?? (MonthEnum)DateTime.Today.Month;
            var date = new DateTime(search.Year.ToSafeInt(), (int)search.Month.Value, 1);
            search.StartDate = date.FirstDateOfMonth();
            search.EndDate = date.LastDateOfMonth();

            search.ElementId = search.ElementId ?? null;
            if (search.PersonId.IsNotNull())
            {
                search.PersonId = search.PersonId;
            }
            if (search.PayrollId.IsNotNull())
            {
                var pay = await _tableMetadataBusiness.GetTableDataByColumn("PayrollBatch", "", "Id", search.PayrollId);
                if (pay.IsNotNull())
                {
                    search.StartDate = Convert.ToDateTime(pay["PayrollStartDate"]);
                    search.EndDate = Convert.ToDateTime(pay["PayrollEndDate"]);

                }

            }
            if (search.StartDate.IsNotNull())
            {
                search.AttendanceStartDate = search.StartDate;
            }
            if (search.EndDate.IsNotNull())
            {
                search.AttendanceEndDate = search.EndDate;
            }
            search.ElementCode = "'MONTHLY_EOS_ACCRUAL'";
            var model = await _businessSalaryInfo.GetAccuralDetails(search);
            return Json(model);

        }

        public async Task<ActionResult> LoanAccrualReportData(PayrollReportViewModel search = null)
        {
            search.Year = search.Year ?? DateTime.Today.Year;
            search.Month = search.Month ?? (MonthEnum)DateTime.Today.Month;
            var date = new DateTime(search.Year.ToSafeInt(), (int)search.Month.Value, 1);
            search.StartDate = date.FirstDateOfMonth();
            search.EndDate = date.LastDateOfMonth();

            search.ElementId = search.ElementId ?? null;
            if (search.PersonId.IsNotNull())
            {
                search.PersonId = search.PersonId;
            }
            if (search.PayrollId.IsNotNull())
            {
                var pay = await _tableMetadataBusiness.GetTableDataByColumn("PayrollBatch", "", "Id", search.PayrollId);
                if (pay.IsNotNull())
                {
                    search.StartDate = Convert.ToDateTime(pay["PayrollStartDate"]);
                    search.EndDate = Convert.ToDateTime(pay["PayrollEndDate"]);

                }

            }
            search.ElementCode = "'MONTHLY_LOAN_ACCRUAL'";
            var model = await _businessSalaryInfo.GetLoanAccuralDetails(search);
            return Json(model);

        }


        public ActionResult EosAccrual(string payrollRunId = null, LayoutModeEnum? layoutMode = null, bool disableSearch = false)
        {
            var year = DateTime.Today.Year;
            var month = (MonthEnum)DateTime.Today.Month;
            var yearmonth = string.Concat(year, DateTime.Today.Month.ToString().PadLeft(2, '0')).ToSafeInt();
            var model = new PayrollReportViewModel { Year = year, Month = month, YearMonth = yearmonth };
            if (layoutMode == LayoutModeEnum.Popup)
            {
                ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
            }
            return View(model);
        }


        public ActionResult LoanAccrual(string payrollRunId = null, LayoutModeEnum? layoutMode = null, bool disableSearch = false)
        {


            var year = DateTime.Today.Year;
            var month = (MonthEnum)DateTime.Today.Month;
            var yearmonth = string.Concat(year, DateTime.Today.Month.ToString().PadLeft(2, '0')).ToSafeInt();
            var model = new PayrollReportViewModel { Year = year, Month = month, YearMonth = yearmonth };
            if (layoutMode == LayoutModeEnum.Popup)
            {
                ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
            }
            return View(model);
        }


        public ActionResult VacationAccrual(string payrollRunId = null, LayoutModeEnum? layoutMode = null, bool disableSearch = false)
        {
            var year = DateTime.Today.Year;
            var month = (MonthEnum)DateTime.Today.Month;
            var yearmonth = string.Concat(year, DateTime.Today.Month.ToString().PadLeft(2, '0')).ToSafeInt();
            var model = new PayrollReportViewModel { Year = year, Month = month, YearMonth = yearmonth };
            if (layoutMode == LayoutModeEnum.Popup)
            {
                ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
            }
            return View(model);
        }

        public async Task<ActionResult> FlightTicketAccrualDetailsByDateDataExcel([DataSourceRequest] DataSourceRequest request, int Year, MonthEnum Month, string PersonId, PayrollReportViewModel search = null)
        {

            var search2 = new PayrollReportViewModel();
            search2.Year = Year;
            search2.Month = Month;

            var date = new DateTime(search2.Year.ToSafeInt(), (int)search2.Month.Value, 1);
            search2.StartDate = date.FirstDateOfMonth();
            search2.EndDate = date.LastDateOfMonth();

            search2.ElementId = search2.ElementId ?? null;
            if (PersonId.IsNotNullAndNotEmpty())
            {
                search2.PersonId = search.PersonId;
            }


            search2.ElementCode = "'MONTHLY_SELF_TICKET_ACCRUAL','MONTHLY_DEPENDENT_INFANT_TICKET_ACCRUAL','MONTHLY_DEPENDENT_CHILD_TICKET_ACCRUAL','MONTHLY_DEPENDENT_ADULT_TICKET_ACCRUAL','MONTHLY_WIFE_TICKET_ACCRUAL','MONTHLY_HUSBAND_TICKET_ACCRUAL'";
            var list =await _businessSalaryInfo.GetAccuralDetails(search2);


             var ms =await _excelReportBusiness.GetFlightAccrualDetails(list.ToList());
            return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FlightTicketAccrual.xlsx");
            //return View();
        }

        public async Task<ActionResult> VacationAccuralDetailsByDateDataExcel([DataSourceRequest] DataSourceRequest request, string PayrollId, int? Year, MonthEnum? Month, string personId)
        {

            var search = new PayrollReportViewModel();
            search.Year = Year ?? DateTime.Today.Year;
            search.Month = Month ?? (MonthEnum)DateTime.Today.Month;

            var date = new DateTime(search.Year.ToSafeInt(), (int)search.Month.Value, 1);
            search.StartDate = date.FirstDateOfMonth();
            search.EndDate = date.LastDateOfMonth();
            if (personId.IsNotNullAndNotEmpty())
            {
                search.PersonId = personId;
            }

            if (search.StartDate.IsNotNull())
            {
                search.AttendanceStartDate = search.StartDate;
            }
            if (search.EndDate.IsNotNull())
            {
                search.AttendanceEndDate = search.EndDate;
            }

            if (PayrollId.IsNotNullAndNotEmpty())
            {
                var pay = await _tableMetadataBusiness.GetTableDataByColumn("PayrollBatch", "", "Id", PayrollId);
                if (pay.IsNotNull())
                {
                    search.StartDate = Convert.ToDateTime(pay["PayrollStartDate"]);
                    search.EndDate = Convert.ToDateTime(pay["PayrollEndDate"]);
                    search.AttendanceStartDate = Convert.ToDateTime(pay["AttendanceStartDate"]); 
                    search.AttendanceEndDate = Convert.ToDateTime(pay["AttendanceEndDate"]); 

                }
            }

            search.ElementCode = "'MONTHLY_VACATION_ACCRUAL'";
            var model = await _businessSalaryInfo.GetAccuralDetails(search);
            foreach (var item in model)
            {
                if (!item.SickLeaveDays.HasValue)
                {
                    item.SickLeaveDays = 0.00;
                    item.SickLeaveAmount = 0.00;
                }
                if (!item.UnpaidLeaveDays.HasValue)
                {
                    item.UnpaidLeaveDays = 0.00;
                    item.UnpaidLeaveAmount = 0.00;
                }
                if (!item.NonWorkingDays.HasValue)
                {
                    item.NonWorkingDays = 0.00;
                    item.NonWorkingAmount = 0.00;
                }
            }
            var ms =await _excelReportBusiness.GetVacationAccuralDetails(model.ToList());
              return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "VacationAccuralReport.xlsx");
            //return View();
        }

        public async Task<ActionResult> LoanAccrualDetailsByDateDataExcel([DataSourceRequest] DataSourceRequest request, string personId, int? Year, MonthEnum? month = null)
        {
            var search = new PayrollReportViewModel();
            search.Year = Year ?? DateTime.Today.Year;
            search.Month = month ?? (MonthEnum)DateTime.Today.Month;

            var date = new DateTime(search.Year.ToSafeInt(), (int)search.Month.Value, 1);
            search.StartDate = date.FirstDateOfMonth();
            search.EndDate = date.LastDateOfMonth();
            if (personId.IsNotNullAndNotEmpty())
            {
                search.PersonId = personId;
            }

            search.ElementCode = "'MONTHLY_LOAN_ACCRUAL'";
            var list =await _businessSalaryInfo.GetLoanAccuralDetails(search);//.GetAccuralDetailsExcel(personId, Year, month).ToList();
              var ms =await _excelReportBusiness.GetLoanAccrualDetails(list.ToList());
             return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "LoanAccuralReport.xlsx");
           // return View();
        }

        public async Task<ActionResult> AccrualDetailsByDateDataExcel([DataSourceRequest] DataSourceRequest request, string personId, int? Year, MonthEnum? month = null)
        {
            var search = new PayrollReportViewModel();
            search.Year = Year ?? DateTime.Today.Year;
            search.Month = month ?? (MonthEnum)DateTime.Today.Month;

            var date = new DateTime(search.Year.ToSafeInt(), (int)search.Month.Value, 1);
            search.StartDate = date.FirstDateOfMonth();
            search.EndDate = date.LastDateOfMonth();


            if (personId.IsNotNullAndNotEmpty())
            {
                search.PersonId = personId;
            }

            if (search.StartDate.IsNotNull())
            {
                search.AttendanceStartDate = search.StartDate;
            }
            if (search.EndDate.IsNotNull())
            {
                search.AttendanceEndDate = search.EndDate;
            }

            search.ElementCode = "'MONTHLY_EOS_ACCRUAL'";


            var list = await _businessSalaryInfo.GetAccuralDetails(search);
             var ms =await _excelReportBusiness.GetEndOfServiceAccrualDetails(list.ToList());
            return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "EOSAccuralReport.xlsx");
            //return View();
        }

    }


}
