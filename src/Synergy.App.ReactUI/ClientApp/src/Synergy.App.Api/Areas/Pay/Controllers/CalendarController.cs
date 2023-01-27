using AutoMapper;
using Synergy.App.ViewModel;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
//using Syncfusion.EJ2.FileManager.Base;
using System;
using System.Threading.Tasks;

namespace Synergy.App.Api.Areas.Pay.Controllers
{
    [Route("pay/calendar")]
    [ApiController]
    public class CalendarController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        IPayrollBusiness _payrollBusiness;
        ILOVBusiness _lovBusiness;
        INoteBusiness _noteBusiness;
        IUserContext _userContext;
        ITableMetadataBusiness _tableMetadataBusiness;
        private IMapper _autoMapper;
        ICmsBusiness _cmsBusiness;
        public CalendarController(AuthSignInManager<ApplicationIdentityUser> customUserManager,
         IServiceProvider serviceProvider, IPayrollBusiness payrollBusiness,
            INoteBusiness noteBusiness,
            IUserContext userContext
            , IMapper autoMapper
            , ILOVBusiness lovBusiness, ITableMetadataBusiness tableMetadataBusiness
            , ICmsBusiness cmsBusiness):base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _payrollBusiness = payrollBusiness;
            _noteBusiness = noteBusiness;
            _userContext = userContext;
            _autoMapper = autoMapper;
            _lovBusiness = lovBusiness;
            _cmsBusiness = cmsBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
        }

        [HttpGet]
        [Route("CreateCalendarDetails")]
        public async Task<IActionResult> CreateCalendarDetails(string CalendarId)
        {
            var model = new CalendarViewModel();
            if (CalendarId.IsNotNullAndNotEmpty())
            {
                var calendar = await _payrollBusiness.GetCalendarDetailsById(CalendarId);
                if (calendar != null)
                {
                    model = calendar;
                }
                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
            }

            return Ok(model);
        }

        [HttpPost]
        [Route("ManageCalendar")]
        public async Task<IActionResult> ManageCalendar(CalendarViewModel model)
        {
            await Authenticate(model.OwnerUserId, model.PortalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

            if (model.DataAction == DataActionEnum.Create)
            {

                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = model.DataAction;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.TemplateCode = "PayrollCalendar";
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";

                var result = await _noteBusiness.ManageNote(notemodel);
                if (result.IsSuccess)
                {
                    return Ok(new { success = true, note = result });
                }
                return Ok(new { success = false, error = result.HtmlError });

            }

            else
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Edit;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.NoteId = model.NoteId;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                var result = await _noteBusiness.ManageNote(notemodel);
                if (result.IsSuccess)
                {
                    return Ok(new { success = true, note = result });
                }
                return Ok(new { success = false, error = result.HtmlError });
            }
        }

        [HttpGet]
        [Route("CreateCalendarHoliday")]
        public async Task<IActionResult> CreateCalendarHoliday(string calendarId, string noteId, string calendarHolidayId)
        {

            var model = new CalendarHolidayViewModel();
            if (calendarHolidayId.IsNotNullAndNotEmpty())
            {
                //var legalentityid = _userContext.LegalEntityId;
                var calHoliday = await _payrollBusiness.GetCalendarHolidayDetailsById(calendarHolidayId);
                if (calHoliday != null)
                {
                    model = calHoliday;
                }
                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
                model.CalendarId = calendarId;
                model.ParentNoteId = noteId;
            }

            return Ok(model);
        }

        [HttpPost]
        [Route("ManageCalendarHoliday")]
        public async Task<IActionResult> ManageCalendarHoliday(CalendarHolidayViewModel model)
        {
            await Authenticate(model.OwnerUserId, model.PortalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

            int datediff = DateTime.Compare(model.FromDate.Value, model.ToDate.Value);

            if (datediff > 0)
            {
                return Ok(new { success = false, error = "From Date should be greater than To Date" });
            }
            if (model.DataAction == DataActionEnum.Create)
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = model.DataAction;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.TemplateCode = "CALENDAR_HOLIDAY";
                noteTempModel.ParentNoteId = model.ParentNoteId;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";

                var result = await _noteBusiness.ManageNote(notemodel);
                if (result.IsSuccess)
                {
                    return Ok(new { success = true });
                }
                return Ok(new { success = false, error = result.HtmlError });
            }
            else
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Edit;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.NoteId = model.NoteId;
                noteTempModel.ParentNoteId = model.ParentNoteId;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                var result = await _noteBusiness.ManageNote(notemodel);
                if (result.IsSuccess)
                {
                    return Ok(new { success = true });
                }
                return Ok(new { success = false, error = result.HtmlError });
            }

        }

        [HttpGet]
        [Route("ReadCalendarData")]
        public async Task<ActionResult> ReadCalendarData()
        {
            var model = await _cmsBusiness.GetDataListByTemplate("PayrollCalendar", "");
            return Ok(model);
        }

        [HttpGet]
        [Route("ReadCalendarHolidayData")]
        public async Task<ActionResult> ReadCalendarHolidayData(string CalendarId)
        {
            var model = await _payrollBusiness.GetCalendarHolidayData(CalendarId);
            return Ok(model);
        }

        [HttpGet]
        [Route("DeleteCalendarHoliday")]
        public async Task<ActionResult> DeleteCalendarHoliday(string Id)
        {
            var result = await _payrollBusiness.DeleteCalendarHoliday(Id);
            return Ok(new { success = result });
        }


    }
}
