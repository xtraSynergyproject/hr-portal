using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
//using Kendo.Mvc.Extensions;
//using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.Sps.Controllers
{
    [Area("Sps")]
    public class SuccessionPlanningController : Controller
    {

        IModuleBusiness _Module;
        IUserContext _userContext;
        ISuccessionPlanningBusiness _SuccessionPlanning;
        IPayrollBusiness _PayrollBusiness;



        public SuccessionPlanningController(IModuleBusiness business, IUserContext userContext, ISuccessionPlanningBusiness SuccessionPlanning, IPayrollBusiness PayrollBusiness)
        {
            _Module = business;
            _userContext = userContext;
            _SuccessionPlanning = SuccessionPlanning;
            _PayrollBusiness = PayrollBusiness;


        }
        public async Task<IActionResult> Index(string Module, string Employee, string Department, int? Month, int? year)
        {
            SuccessionPlaningViewModel model = new SuccessionPlaningViewModel();



            var calendar = await _PayrollBusiness.GetCalendarListData();


            var dataCalendar = calendar.Where(x => x.Code == "UAE").ToList();

            int daysinMonth = 0, Monthint = 0, Yearint = 0;
            if (Month.IsNotNull() && year.IsNotNull())
            {
                daysinMonth = DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(Month));
            }
            else
            {
                daysinMonth = DateTime.DaysInMonth(Convert.ToInt32(DateTime.Now.Year), Convert.ToInt32(DateTime.Now.Month));
            }

            if (Month.IsNotNull())
            {
                Monthint = Convert.ToInt32(Month);
            }
            else { Monthint = DateTime.Now.Month; }


            Yearint = year.IsNotNull() ? Convert.ToInt32(year) : DateTime.Now.Year;
            var Leave = await _PayrollBusiness.GetCalendarHolidayDatawithmonthYear(dataCalendar[0].Id, Yearint, Monthint);

            string[] columns = new string[daysinMonth];
            string weekend = "";
            for (var i = 0; i < daysinMonth; i++)
            {


                weekend = "";
                DateTime date = new DateTime(Convert.ToInt32(Yearint), Convert.ToInt32(Monthint), i + 1);
                var dates = date.DayOfWeek;

                foreach (var item in dataCalendar)
                {
                    if (item.IsFridayWeekEnd == true && "Friday" == dates.ToString())
                    {

                        weekend = "Weekend";
                    }
                    else if (item.IsSaturdayWeekEnd == true && "Saturday" == dates.ToString())
                    {

                        weekend = "Weekend";
                    }
                    else if (item.IsMondayWeekEnd == true && "Monday" == dates.ToString())
                    {

                        weekend = "Weekend";
                    }
                    else if (item.IsTuesdayWeekEnd == true && "Tuesday" == dates.ToString())
                    {

                        weekend = "Weekend";
                    }
                    else if (item.IsWednesdayWeekEnd == true && "Wednesday" == dates.ToString())
                    {

                        weekend = "Weekend";
                    }
                    else if (item.IsThursdayWeekEnd == true && "Thursday" == dates.ToString())
                    {

                        weekend = "Weekend";
                    }
                }

                var lev = Leave.Where(x => x.ToDate == date).ToList();
                if (lev.Count > 0)
                {

                    if (string.IsNullOrEmpty(weekend))
                    {
                        weekend = "Holiday";
                    }
                }

                columns[i] = Convert.ToString(i + 1) + "," + weekend;
            }
            model.Columns = columns;
            model.Month = Convert.ToString(Monthint);
            model.Year = Yearint;
            if (Module.IsNotNullAndNotEmpty())
            {
                model.Module = Module;
            }

            if (Employee.IsNotNullAndNotEmpty())
            {
                model.Employee = Employee;
            }

            if (string.IsNullOrEmpty(Module))
            {
                model.Module = "60ec5b4a250bcb01854887d9";
            }
            if (string.IsNullOrEmpty(Department))
            {
                model.Department = "978913d3-730f-47ba-9c5f-b1215f453aff";
            }

            return View(model);
        }


        public async Task<JsonResult> GetModule()
        {

            var data = await _Module.GetList();
            return Json(data);


        }

        public async Task<ActionResult> GetDetails([DataSourceRequest] DataSourceRequest request, string Module, string Employee, string Department, int? Month, int? year)
        {
            var result = await _SuccessionPlanning.GetSuccessionPlanings(Module, Employee, Department, Month, year);
            //  var json = Json(result.ToDataSourceResult(request));



            //    var result1 = result.ToTreeDataSourceResult(request,
            //    e => e.ID,
            //    e => e.ParentId,
            //    e => e

            //);
            var result1 = result;

            return Json(result1);


        }



        public async Task<IActionResult> ViewAssessment(string UserId, Int32 day, Int32 Month, Int32 Year)
        {

            DateTime date = new DateTime(Convert.ToInt32(Year), Convert.ToInt32(Month), day - 3);
            var Model = await _SuccessionPlanning.GetAssessmentByDateuserid(UserId, date);

            if (Model.IsNotNull())
            {
                return View(Model);
            }
            else
            {

                Model = new SuccessionPlanningAssessmentViewModel();
                return View(Model);
            }
        }


        public async Task<IActionResult> ViewAssessmentSet(string UserId, Int32 day, Int32 Month, Int32 Year)
        {

            DateTime date = new DateTime(Convert.ToInt32(Year), Convert.ToInt32(Month), day - 3);
            var Model = await _SuccessionPlanning.GetAssessmenSetByDateuserid(UserId, date);
            if (Model.IsNotNull())
            {
                return View(Model);
            }
            else
            {

                Model = new SuccessionPlanningAssessmentViewModel();
                return View(Model);
            }
        }


    }
}
