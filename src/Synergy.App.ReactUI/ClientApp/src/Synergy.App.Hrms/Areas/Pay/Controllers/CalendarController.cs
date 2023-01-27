using AutoMapper;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
//using Kendo.Mvc.Extensions;
//using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.Pay.Controllers
{
    [Area("Pay")]
    public class CalendarController : ApplicationController
    {
        IPayrollBusiness _payrollBusiness;
        ILOVBusiness _lovBusiness;
        INoteBusiness _noteBusiness;
        IUserContext _userContext;
        ITableMetadataBusiness _tableMetadataBusiness;
        private IMapper _autoMapper;
        ICmsBusiness _cmsBusiness;
        public CalendarController(IPayrollBusiness payrollBusiness,
            INoteBusiness noteBusiness,
            IUserContext userContext
            , IMapper autoMapper
            , ILOVBusiness lovBusiness , ITableMetadataBusiness tableMetadataBusiness
            , ICmsBusiness cmsBusiness) 
        {
            _payrollBusiness = payrollBusiness;
            _noteBusiness = noteBusiness;
            _userContext = userContext;
            _autoMapper = autoMapper;
            _lovBusiness = lovBusiness;
            _cmsBusiness = cmsBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> ManageCalendar(string CalendarId)
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
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageCalendar(CalendarViewModel model)
        {
            //var calList = await _payrollBusiness.GetCalendarListData();
            //var exist = calList.Where(x => x.Name == model.Name && x.Id != model.Id);
            //if (exist.Any())
            //{
            //    return Json(new { success = false, error = "Name Already Exist" });
            //}            

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
                        return Json(new { success = true, note = result });
                    }
                    return Json(new { success = false, error = result.HtmlError });                

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
                    return Json(new { success = true, note = result });
                }
                return Json(new { success = false, error = result.HtmlError });
            }
        }

        public async Task<IActionResult> ManageCalendarHoliday(string calendarId, string noteId, string calendarHolidayId)
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

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageCalendarHoliday(CalendarHolidayViewModel model)
        {
            //var calHolList = await _payrollBusiness.GetCalendarHolidayData(model.CalendarId);
            //var exist = calHolList.Where(x => x.HolidayName == model.HolidayName && x.Id!=model.Id);
            //if (exist.Any())
            //{
            //    return Json(new { success = false, error = "Holiday Name Already Exist" });
            //}
            int datediff = DateTime.Compare(model.FromDate.Value, model.ToDate.Value);

            if(datediff>0)
            {
                return Json(new { success = false, error = "From Date should be greater than To Date" });
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
                    return Json(new { success = true });
                }
                return Json(new { success = false, error = result.HtmlError });
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
                    return Json(new { success = true });
                }
                return Json(new { success = false, error = result.HtmlError });
            }

        }       
        
        public async Task<ActionResult> ReadCalendarData()
        {
            var model = await _cmsBusiness.GetDataListByTemplate("PayrollCalendar", "");            
            return Json(model);            
        }
        public async Task<ActionResult> ReadCalendarHolidayData(string CalendarId)
        {
            var model =await _payrollBusiness.GetCalendarHolidayData(CalendarId);
            return Json(model);
        }
        public async Task<ActionResult> DeleteCalendarHoliday(string Id)
        {
            var result = await _payrollBusiness.DeleteCalendarHoliday(Id);
            return Json(new { success = result });
        }
    }
}
