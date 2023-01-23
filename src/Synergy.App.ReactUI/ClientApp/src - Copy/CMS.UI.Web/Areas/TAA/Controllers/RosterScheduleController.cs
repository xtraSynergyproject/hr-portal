using CMS.Business;
using CMS.Common;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
//using Telerik.Reporting.Processing;

namespace CMS.UI.Web.Areas.TAA.Controllers
{
    [Area("Taa")]
    public class RosterScheduleController : Controller
    {
        private readonly IAttendanceBusiness _business;
        private readonly IRosterScheduleBusiness _rosterScheduleBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly IHRCoreBusiness _hRCoreBusiness;
        private readonly ILeaveBalanceSheetBusiness _leaveBalanceSheetBusiness;
        public RosterScheduleController(IAttendanceBusiness business, ITableMetadataBusiness tableMetadataBusiness, ICmsBusiness cmsBusiness,
            IRosterScheduleBusiness rosterScheduleBusiness, IHRCoreBusiness hRCoreBusiness, ILeaveBalanceSheetBusiness leaveBalanceSheetBusiness)
        {
            _business = business;
            _tableMetadataBusiness = tableMetadataBusiness;
            _cmsBusiness = cmsBusiness;
            _leaveBalanceSheetBusiness = leaveBalanceSheetBusiness;
            _rosterScheduleBusiness = rosterScheduleBusiness;
            _hRCoreBusiness = hRCoreBusiness;
        }
        public ActionResult Index(string orgId = null, DateTime? date = null)
        {
            date = date ?? DateTime.Today;
            var model = new RosterScheduleViewModel { OrganizationId = orgId };
            model.WeekDateString = date.Value.ToString("yyyy-MM-dd");
            var firstDayOfweek = (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            var dayOfDate = (int)date.Value.DayOfWeek;
            var dateAdd = 0;
            if (firstDayOfweek > dayOfDate)
            {
                dateAdd = firstDayOfweek - dayOfDate - 7;
            }
            else
            {
                dateAdd = firstDayOfweek - dayOfDate;
            }

            var sunday = date.Value.AddDays(dateAdd);

            ViewBag.Sunday = sunday.ToString("yyyy-MM-dd");
            ViewBag.Sun = sunday.ToString("ddd, dd MMM yyyy");

            var monday = sunday.AddDays(1);
            ViewBag.Monday = monday.ToString("yyyy-MM-dd");
            ViewBag.Mon = monday.ToString("ddd, dd MMM yyyy");

            var tuesday = sunday.AddDays(2);
            ViewBag.Tuesday = tuesday.ToString("yyyy-MM-dd");
            ViewBag.Tue = tuesday.ToString("ddd, dd MMM yyyy");

            var wednesday = sunday.AddDays(3);
            ViewBag.Wednesday = wednesday.ToString("yyyy-MM-dd");
            ViewBag.Wed = wednesday.ToString("ddd, dd MMM yyyy");

            var thursday = sunday.AddDays(4);
            ViewBag.Thursday = thursday.ToString("yyyy-MM-dd");
            ViewBag.Thu = thursday.ToString("ddd, dd MMM yyyy");

            var friday = sunday.AddDays(5);
            ViewBag.Friday = friday.ToString("yyyy-MM-dd");
            ViewBag.Fri = friday.ToString("ddd, dd MMM yyyy");


            var saturday = sunday.AddDays(6);
            ViewBag.Saturday = saturday.ToString("yyyy-MM-dd");
            ViewBag.Sat = saturday.ToString("ddd, dd MMM yyyy");

            ViewBag.Previous = date.Value.AddDays(-7).ToString("yyyy-MM-dd");
            ViewBag.Next = date.Value.AddDays(7).ToString("yyyy-MM-dd");

            model.WeekDisplayName = string.Concat(sunday.ToString("dddd, dd MMM yyyy"), " - ", saturday.ToString("dddd, dd MMM yyyy"));
            if (orgId == null)
            {
                model.OrganizationId = "All";
            }
            return View(model);
        }

        public ActionResult CreateRoster(string orgId, string users, string dates)
        {
            var model = new RosterScheduleViewModel();
            model.UserIds = users;
            model.RosterDates = dates;
            model.DataAction = DataActionEnum.Create;
            model.OrganizationId = orgId;
            model.IsAttendanceCalculated = false;
            return View(model);
        }

        public async Task<JsonResult> GetDepartmentList()
        {
            var result = await _rosterScheduleBusiness.GetDepartmentList();
            //result.Insert(0,new IdNameViewModel()
            //{
            //    Name = "All",
            //    Id= "All"
            //});
            //result.OrderBy(x=>x.)
            return Json(result);
        }

        public async Task<ActionResult> ReadSearchData([DataSourceRequest] DataSourceRequest request, string orgId, DateTime? date = null)
        {
            var result =await _rosterScheduleBusiness.GetRosterSchedulerList(orgId, date);
            var json = Json(result.ToDataSourceResult(request));           
            return json;
        }

        [HttpGet]
        public async Task<JsonResult> GetCustomIdNameList()
        {
            var model = await _rosterScheduleBusiness.GetShiftPatternList();

            model.Add(new IdNameViewModel()
            {
                Id = "000",
                Name = "Custom",
            });
            model.Add(new IdNameViewModel()
            {
                Id = "001",
                Name = "DayOff",
            });
            return Json(model);
        }

        [HttpPost]
        public async Task<ActionResult> CorrectRoster(string startDate, string userId, string patternId, RosterDutyTypeEnum type = RosterDutyTypeEnum.Pattern)
        {
            var model = new RosterScheduleViewModel();

            if (type == RosterDutyTypeEnum.DayOff)
            {
                model.DraftRosterDutyType = RosterDutyTypeEnum.DayOff;
            }
            else
            {
                var data = await _rosterScheduleBusiness.GetRosterDutyTemplateById(patternId);
                if (data.IsNotNull())
                {
                    
                    model.DraftRosterDutyType = RosterDutyTypeEnum.Pattern;
                    model.DraftDuty1Enabled = data.Duty1Enabled;
                    model.DraftDuty1StartTime = data.Duty1StartTime;
                    model.DraftDuty1EndTime = data.Duty1EndTime;
                    model.DraftDuty1FallsNextDay = data.Duty1FallsNextDay;
                    model.DraftDuty2Enabled = data.Duty2Enabled;
                    model.DraftDuty2StartTime = data.Duty2StartTime;
                    model.DraftDuty2EndTime = data.Duty2EndTime;
                    model.DraftDuty2FallsNextDay = data.Duty2FallsNextDay;
                    model.DraftDuty3Enabled = data.Duty3Enabled;
                    model.DraftDuty3StartTime = data.Duty3StartTime;
                    model.DraftDuty3EndTime = data.Duty3EndTime;
                    model.DraftDuty3FallsNextDay = data.Duty3FallsNextDay;
                    model.ShiftPatternName = data.Name;
                }
                   

            }
            model.UserIds = userId + ",";
            model.RosterDates =startDate + ",";
            model.DataAction = DataActionEnum.Create;
            var res=await _rosterScheduleBusiness.CreateRosterSchedule(model);

            if (res.IsSuccess)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, error = res.Messages.ToHtmlError() });
            }
        }

      

        [HttpPost]
        public async Task<IActionResult> Manage(RosterScheduleViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var res = await _rosterScheduleBusiness.CreateRosterSchedule(model);
                    if (res.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false,error=res.Message });
                    }
                }
                if (model.DataAction == DataActionEnum.Edit)
                {
                    
                }

                else if (model.DataAction == DataActionEnum.Delete)
                {
                   
                }
                else
                {
                    ModelState.AddModelError("InvalidOperation", "Invalid Operation");
                    return Json(new { success = false, error = ModelState.SerializeErrors() });
                }
            }
            
            return Json(new { success = false, error = ModelState.SerializeErrors() });            
            
        }

        [HttpPost]
        public async Task<ActionResult> GetShiftPattern(string templateId)
        {
            var model = await _rosterScheduleBusiness.GetRosterDutyTemplateById(templateId);
            if (model.Duty1StartTime != null)
            {
                model.Duty1StartTimeVal = model.Duty1StartTime.ToString().Substring(0, 5);
            }
            if (model.Duty1EndTime != null)
            {
                model.Duty1EndTimeVal = model.Duty1EndTime.ToString().Substring(0, 5);
            }
            if (model.Duty2StartTime != null)
            {
                model.Duty2StartTimeVal = model.Duty2StartTime.ToString().Substring(0, 5);
            }
            if (model.Duty2EndTime != null)
            {
                model.Duty2EndTimeVal = model.Duty2EndTime.ToString().Substring(0, 5);
            }
            if (model.Duty3StartTime != null)
            {
                model.Duty3StartTimeVal = model.Duty3StartTime.ToString().Substring(0, 5);
            }
            if (model.Duty3EndTime != null)
            {
                model.Duty3EndTimeVal = model.Duty3EndTime.ToString().Substring(0, 5);
            }


            return Json(new { success = true, items = model });
        }

        public async Task<ActionResult> PublishRoster(string orgId, DateTime? date = null)
        {
            var model = new RosterScheduleViewModel();
            var result=await _rosterScheduleBusiness.PublishRoster(orgId, date);
            if (result.IsSuccess)
            {
                return Json(new { success = true });

            }
            return Json(new { success = false, errors = ModelState.SerializeErrors() });
        }

        [HttpGet]
        public async Task<ActionResult> GetPublishDate(string orgId, DateTime? date = null)
        {
            var publishData = await _rosterScheduleBusiness.GetPublishedDate(orgId, date);


            return Json(publishData);
        }

        public ActionResult CopyRoster(string orgId, string users, string dates, string date = null)
        {
            DateTime? date1 = date.IsNotNullAndNotEmpty()? Convert.ToDateTime(date) : DateTime.Today;
            var model = new RosterScheduleViewModel { OrganizationId = orgId, UserIds = users, RosterDates = dates };
            model.WeekDateString = date1.Value.ToString("yyyy-MM-dd");
            var firstDayOfweek = (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            var dayOfDate = (int)date1.Value.DayOfWeek;
            var dateAdd = 0;
            if (firstDayOfweek > dayOfDate)
            {
                dateAdd = firstDayOfweek - dayOfDate - 7;
            }
            else
            {
                dateAdd = firstDayOfweek - dayOfDate;
            }

            var sunday = date1.Value.AddDays(dateAdd);
            var saturday = sunday.AddDays(6);

            ViewBag.Previous = date1.Value.AddDays(-7).ToString("yyyy-MM-dd");
            ViewBag.Next = date1.Value.AddDays(7).ToString("yyyy-MM-dd");

            model.WeekDisplayName = string.Concat(sunday.ToString("dddd, dd MMM yyyy"), " - ", saturday.ToString("dddd, dd MMM yyyy"));
            ViewBag.EnablePrevious = true;
            if (sunday < DateTime.Today.AddDays(-7))
            {
                ViewBag.EnablePrevious = false;
            }
            model.RosterDate = sunday;
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> CopyRoster(RosterScheduleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =await _rosterScheduleBusiness.CopyRoster(model);
                if (!result.IsSuccess)
                {
                    result.Messages.Each(x => ModelState.AddModelError(x.Key, x.Value));
                    return Json(new { success = false, errors = ModelState.SerializeErrors() });
                }
                else
                {
                    return Json(new { success = true });
                }
            }
            else
            {
                return Json(new { success = false, errors = ModelState.SerializeErrors() });
            }
        }

        public ActionResult GetWeeks(DateTime date)
        {
            var list = new List<IdNameViewModel>();            
            date = date.AddDays(7);
            for (int i = 0; i < 19; i++)
            {
                list.Add(new IdNameViewModel
                {
                    Name = string.Concat(date.ToString("dddd, dd MMM yyyy"), " - ", date.AddDays(6).ToString("dddd, dd MMM yyyy")),
                    Code = string.Concat(date.ToString("yyyy/MM/dd"), "_", date.AddDays(6).ToString("yyyy/MM/dd"))

                });
                date = date.AddDays(7);
            }
            return Json(list);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteRoster(string users, string dates)
        {
            var del=await _rosterScheduleBusiness.DeleteRoster(users, dates);
            return Json(new { success = true });
        }

        public async Task<ActionResult> RosterTimeLineView(string orgId, DateTime date)
        {
            var model = new RosterScheduleViewModel { OrganizationId = orgId, RosterDate = date };
            model.userList = await _rosterScheduleBusiness.GetPersonListByOrganizationHerarchy(orgId);
            return View(model);
        }

        public async Task<ActionResult> GetRosterTimeData([DataSourceRequest] DataSourceRequest request, string orgId, DateTime? date = null)
        {
            var model = await _rosterScheduleBusiness.GetRosterTimeList(orgId, date);
            var j = Json(model.ToDataSourceResult(request));
            return j;
        }

        public async Task<ActionResult> LeaveCalendarView(string orgId, DateTime date)
        {
            var model = new UserHierarchyViewModel { Colleagues =await _rosterScheduleBusiness.GetPersonListByOrganizationHerarchy(orgId) };
            ViewBag.OrganizationId = orgId;
            return View(model);
        }

        public async Task<ActionResult> ReadLeaveCalendarView([DataSourceRequest] DataSourceRequest request, string orgId)
        {
            var model = await _leaveBalanceSheetBusiness.GetLeaveCalendar(orgId);
            var j = Json(model.ToDataSourceResult(request));
            return j;
        }
    }
}
