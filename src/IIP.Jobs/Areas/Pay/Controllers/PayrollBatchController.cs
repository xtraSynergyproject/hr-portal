using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Synergy.App.ViewModel.Pay;
using Synergy.App.ViewModel;
using System.Collections.Generic;

namespace Synergy.App.Api.Areas.Pay.Controllers
{
    [Route("pay/payrollbatch")]
    [ApiController]
    public class PayrollBatchController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private readonly ILegalEntityBusiness _legalEntityBusiness;

        private readonly INoteBusiness _noteBusiness;
        IPayrollBatchBusiness _payrollBatchBusiness;
        public PayrollBatchController(AuthSignInManager<ApplicationIdentityUser> customUserManager,
         IServiceProvider serviceProvider, IPayrollBatchBusiness payrollBatchBusiness, ILegalEntityBusiness legalEntityBusiness, INoteBusiness noteBusiness) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _legalEntityBusiness = legalEntityBusiness;
            _payrollBatchBusiness = payrollBatchBusiness;
            _noteBusiness = noteBusiness;
        }

        [HttpGet]
        [Route("GetPayGroupList")]
        public async Task<IActionResult> GetPayGroupList()
        {
            var result = await _payrollBatchBusiness.GetPayGroupList();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetPayrollGroupDetail")]
        public async Task<IActionResult> GetPayrollGroupDetail(string payGroupId, int monthval, int year)
        {
            var result = await _payrollBatchBusiness.GetPayrollGroupById(payGroupId);
            int[] monthNames = {1, 2, 3, 4, 5, 6,
                7, 8, 9, 10, 11, 12 };
            int sdmon;
            int sdyear;
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

            return Ok(new { success = true, data = result });
        }
        [HttpGet]
        [Route("GetPayBankBranchList")]
        public async Task<IActionResult> GetPayBankBranchList()
        {
            var result = await _payrollBatchBusiness.GetPayBankBranchList();

            return Ok(result);
        }
        [HttpGet]
        [Route("GetPayCalenderList")]
        public async Task<IActionResult> GetPayCalenderList()
        {
            var result = await _payrollBatchBusiness.GetPayCalenderList();

            return Ok(result);
        }

        [HttpGet]
        [Route("GetSalaryElementList")]
        public async Task<IActionResult> GetSalaryElementList()
        {
            var result = await _payrollBatchBusiness.GetSalaryElementIdName();

            return Ok(result);
        }

        [HttpGet]
        [Route("CreatePayrollBatch")]
        public async Task<IActionResult> CreatePayrollBatch(string userId,string portalName)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var model = new PayrollBatchViewModel();

            model.DataAction = DataActionEnum.Create;
            model.RunType = PayrollRunTypeEnum.Salary;
            model.LegalEntityId = _userContext.LegalEntityId;
            model.PayrollEndDate = DateTime.Now.Date;
            
            return Ok(model);
        }
        [HttpPost]
        [Route("ManagePayrollBatch")]
        public async Task<IActionResult> ManagePayrollBatch(PayrollBatchViewModel model)
        {
            await Authenticate(model.OwnerUserId, model.PortalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var ym = string.Concat(model.Year, model.Month);
                model.YearMonth = ym.ToSafeNullableInt();

                var exist = await _payrollBatchBusiness.IsPayrollExist(model.PayrollGroupId, ym);
                if (exist.IsNotNull())
                {
                    return Ok(new { success = false, error = "Payroll batch already created for selected month and year" });
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

                            return Ok(new { success = true });
                        }
                    
                }


            }
            return Ok( model);
        }


        [HttpGet]
        [Route("GetYear")]
        public ActionResult GetYear()
        {
            var cryear = DateTime.Today.Year;
            var pryear = DateTime.Today.AddYears(-1).Year;

            var data = new List<IdNameViewModel>();

            data.Add(new IdNameViewModel { Id = cryear.ToString(), Name = cryear.ToString() });
            data.Add(new IdNameViewModel { Id = pryear.ToString(), Name = pryear.ToString() });

            return Ok(data);
        }


        [HttpGet]
        [Route("ReadPayrollGroupList")]
        public async Task<ActionResult> ReadPayrollGroupList()
        {
            var leList = await _payrollBatchBusiness.GetPayrollGroupList();
            return Ok(leList);
        }
    }
}
