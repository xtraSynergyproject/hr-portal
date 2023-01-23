using CMS.Business;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CMS.Common;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;


namespace CMS.UI.Web.Areas.Pay.Controllers
{
    [Area("Pay")]
    public class PayrollBatchController : Controller
    {
        private readonly ILegalEntityBusiness _legalEntityBusiness;
        
        private readonly INoteBusiness _noteBusiness;
        private readonly IUserContext _userContext;
        IPayrollBatchBusiness _payrollBatchBusiness;
        public PayrollBatchController(IPayrollBatchBusiness payrollBatchBusiness, ILegalEntityBusiness legalEntityBusiness,             INoteBusiness noteBusiness, IUserContext userContext)
        {
            _legalEntityBusiness = legalEntityBusiness;
            _payrollBatchBusiness = payrollBatchBusiness;
            _noteBusiness = noteBusiness;
            _userContext = userContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetPayGroupList()
        {
            var result = await _payrollBatchBusiness.GetPayGroupList();

            return Json(result);
        }
        public async Task<JsonResult> GetPayrollGroupDetail(string payGroupId,int monthval, int year)
        {
            var result = await _payrollBatchBusiness.GetPayrollGroupById(payGroupId);
           // monthval = monthval.Replace("/(^|-)0 +/g", "$1");

            int [] monthNames= {1, 2, 3, 4, 5, 6,
                7, 8, 9, 10, 11, 12 };
            int sdmon ;
            int sdyear ;
            if (result != null && result.IsStartDayPreviousMonth)
            {
                if (monthval == 1)
                {
                     sdmon = monthNames[11];
                     sdyear = year - 1;
                }
                else
                {
                    sdmon = monthNames[monthval - 2];
                    sdyear = year;
                }
            }
            else
            {
                sdmon = monthNames[monthval - 1];
                sdyear = year;
            }
            var lastDate = DateTime.DaysInMonth(year, monthval); 
            var stDate = result.StartDay >= lastDate ? lastDate : result.StartDay;
            var endDate = result.EndDay >= lastDate ? lastDate : result.EndDay;
            var edmon = monthNames[monthval - 1];
            var sdate = new DateTime(sdyear, sdmon, stDate);//stDate + " " + sdmon + " " + sdyear;
            var edate = new DateTime(year, edmon, endDate);//endDate + " " + edmon + " " + year;
            result.PayrollStartDate = sdate;
            result.PayrollEndDate = edate;
                       
            return Json(new { success=true, data = result });
        }
        public async Task<JsonResult> GetPayBankBranchList()
        {
            var result = await _payrollBatchBusiness.GetPayBankBranchList();

            return Json(result);
        }
        public async Task<JsonResult> GetPayCalenderList()
        {
            var result = await _payrollBatchBusiness.GetPayCalenderList();

            return Json(result);
        }
        public async Task<JsonResult> GetSalaryElementList()
        {
            var result = await _payrollBatchBusiness.GetSalaryElementIdName();

            return Json(result);
        }

        public IActionResult ManagePayrollBatch()
        {
            var model = new PayrollBatchViewModel()
            {
                DataAction = DataActionEnum.Create,
                RunType = PayrollRunTypeEnum.Salary,
                LegalEntityId = _userContext.LegalEntityId,
                PayrollEndDate=DateTime.Now.Date,
               // PayrollStartDate= DateTime.Now.Date
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManagePayrollBatch(PayrollBatchViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ym = string.Concat(model.Year, model.Month);
                model.YearMonth = ym.ToSafeNullableInt();

                var exist = await _payrollBatchBusiness.IsPayrollExist(model.PayrollGroupId, ym);
                if (exist.IsNotNull())
                {
                    return Json(new { success = false, error = "Payroll batch already created for selected month and year" });
                }
                else
                {
                    
                    if (model.DataAction == DataActionEnum.Create)
                    {
                        var noteTemplate = new NoteTemplateViewModel();

                        noteTemplate.ActiveUserId = _userContext.UserId;
                        noteTemplate.TemplateCode = "PayrollBatch";
                        noteTemplate.DataAction = model.DataAction;
                        var note = await _noteBusiness.GetNoteDetails(noteTemplate);

                        model.PayrollStatus = PayrollStatusEnum.NotStarted;

                        if (model.Year.IsNotNull() && model.Month.IsNotNull())
                        {
                            var payGroup = await _payrollBatchBusiness.GetPayrollGroupById(model.PayrollGroupId);
                            var month = model.Month.ToSafeInt();

                            var startDate = new DateTime(model.Year.Value, month, 1);
                            if (startDate.DaysInMonth() < payGroup.StartDay)
                            {
                                model.PayrollStartDate = new DateTime(model.Year.Value, month, startDate.DaysInMonth());
                            }
                            else
                            {
                                model.PayrollStartDate = new DateTime(model.Year.Value, month, payGroup.StartDay);
                            }
                            var endDate = new DateTime(model.Year.Value, month, 1);
                            if (endDate.DaysInMonth() < payGroup.EndDay)
                            {
                                model.PayrollEndDate = new DateTime(model.Year.Value, month, endDate.DaysInMonth());
                            }
                            else
                            {
                                model.PayrollEndDate = new DateTime(model.Year.Value, month, payGroup.EndDay);
                            }
                            if (payGroup.IsStartDayPreviousMonth)
                            {
                                model.PayrollStartDate = model.PayrollStartDate.AddMonths(-1);
                            }

                            var attendanceStartDate = new DateTime(model.Year.Value, month, 1);
                            if (attendanceStartDate.DaysInMonth() < payGroup.CutOffStartDay)
                            {
                                model.AttendanceStartDate = new DateTime(model.Year.Value, month, attendanceStartDate.DaysInMonth());
                            }
                            else
                            {
                                model.AttendanceStartDate = new DateTime(model.Year.Value, month, payGroup.CutOffStartDay);
                            }
                            var attendanceEndDate = new DateTime(model.Year.Value, month, 1);
                            if (attendanceEndDate.DaysInMonth() < payGroup.CutOffEndDay)
                            {
                                model.AttendanceEndDate = new DateTime(model.Year.Value, month, attendanceEndDate.DaysInMonth());
                            }
                            else
                            {
                                model.AttendanceEndDate = new DateTime(model.Year.Value, month, payGroup.CutOffEndDay);
                            }
                            if (payGroup.IsCutOffStartDayPreviousMonth)
                            {
                                model.AttendanceStartDate = model.AttendanceStartDate.Value.AddMonths(-1);
                            }
                            //var ym = string.Concat(model.Year, model.Month);
                            //model.YearMonth = ym.ToSafeNullableInt();
                        }

                        note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                        note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                        var result = await _noteBusiness.ManageNote(note);

                        if (result.IsSuccess)
                        {
                            ViewBag.Success = true;
                            var payrun = new PayrollRunViewModel
                            {
                                Name = model.Name,
                                Description = model.Description,
                                PayrollBatchId = result.Item.UdfNoteTableId,
                                YearMonth = model.YearMonth,
                                PayrollStartDate = model.PayrollStartDate,
                                PayrollEndDate = model.PayrollEndDate,
                                PayrollStateStart = PayrollStateEnum.NotStarted,
                                PayrollStateEnd = PayrollStateEnum.NotStarted,
                                ExecutionStatus = PayrollExecutionStatusEnum.NotStarted,
                                ExecutedBy = _userContext.UserId
                            };
                            noteTemplate.TemplateCode = "PayrollRun";
                            noteTemplate.ParentNoteId = result.Item.NoteId;
                            note = await _noteBusiness.GetNoteDetails(noteTemplate);

                            note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(payrun);
                            result = await _noteBusiness.ManageNote(note);

                            return Json(new { success = true });
                        }
                        else
                        {
                            ModelState.AddModelErrors(result.Messages);
                        }
                    }
                    //else if (model.DataAction == DataActionEnum.Edit)
                    //{
                    //    var result = await _payrollBatchBusiness.Edit(model);
                    //    if (result.IsSuccess)
                    //    {
                    //        ViewBag.Success = true;
                    //    }
                    //    else
                    //    {
                    //        ModelState.AddModelErrors(result.Messages);
                    //    }
                    //}
                }


            }
            return View("ManagePayrollBatch", model);
        }
        

        public ActionResult GetYear()
        {
            var cryear = DateTime.Today.Year;
            var pryear = DateTime.Today.AddYears(-1).Year;

            var data = new List<IdNameViewModel>();

            data.Add(new IdNameViewModel { Id = cryear.ToString(), Name = cryear.ToString() });
            data.Add(new IdNameViewModel { Id = pryear.ToString(), Name = pryear.ToString() });

            return Json(data);
        }

        public async Task<ActionResult> GetLegalEntityList()
        {
            var leList = await _legalEntityBusiness.GetList();
            return Json(leList);
        }
        public async Task<ActionResult> ReadPayrollGroupList()
        {
            var leList = await _payrollBatchBusiness.GetPayrollGroupList();
            return Json(leList);
        }
    }
}
