using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.PJM.Controllers
{
    [Area("PJM")]
    public class ProjectReportController : ApplicationController
    {
        IUserContext _userContext;
        IProjectManagementBusiness _projectManagementBusiness;
        public ProjectReportController(IUserContext userContext, IProjectManagementBusiness projectManagementBusiness)
        {
            _userContext = userContext;
            _projectManagementBusiness = projectManagementBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult HourSpentReport()
        {
            return View();
        }
        public async Task<ActionResult> HourSpentReportDataExcel(string projectId, string assigneeId, string sdate,string edate)
        {
            DateTime sd = sdate.ToSafeDateTime();
            DateTime ed = edate.ToSafeDateTime();
            var result = await _projectManagementBusiness.GetHourReportTaskData(null);   
            if (assigneeId.IsNotNullAndNotEmpty())
            {
                result = result.Where(x => x.AssigneeId == assigneeId).ToList();
            }
            if (projectId.IsNotNull())
            {
                result = result.Where(x => x.ProjectId == projectId).ToList();
            }
            //if (date.IsNotNullAndNotEmpty() && date != "null")
            //{
            //    sd = Convert.ToDateTime(date);
            //    result = result.Where(x => x.WSDate.Value.Date == sd.Date).ToList();
            //    var d = result.Select(x => x.WSDate).ToList();
            //    result = result.Where(x => x.WSDate.HasValue ? x.WSDate.Value.ToShortDateString() == sd.ToShortDateString() : x.WSDate == sd).ToList();
            //}
            if (sdate.IsNotNullAndNotEmpty() && sdate != "null")
            {
                sd = Convert.ToDateTime(sdate);
                result = result.Where(x => x.StartDate.Date >= sd.Date).ToList();
                // var d = result.Select(x => x.WSDate).ToList();
               // result = result.Where(x => x.WSDate.HasValue ? x.WSDate.Value/*.ToShortDateString()*/ >= sd/*.ToShortDateString()*/ : x.WSDate >= sd).ToList();
            }
            if (edate.IsNotNullAndNotEmpty() && edate != "null")
            {
                ed = Convert.ToDateTime(edate);
                ed = ed.AddDays(1).Date;
                result = result.Where(x => x.EndDate.Date < ed.Date).ToList();
                // var d = result.Select(x => x.WSDate).ToList();
               // result = result.Where(x => x.WEDate.HasValue ? x.WEDate.Value/*.ToShortDateString()*/ < ed/*.ToShortDateString()*/ : x.WEDate < sd).ToList();
            }
            var sStdate = new DateTime(sdate.ToSafeDateTime().Year, sdate.ToSafeDateTime().Month, sdate.ToSafeDateTime().Day, 0, 0, 0).ToString();
            var sEtdate = new DateTime(edate.ToSafeDateTime().Year, edate.ToSafeDateTime().Month, edate.ToSafeDateTime().Day, 0, 0, 0).ToString();

            var report = string.Concat("HoursSpentReport_"+sd.ToDefaultDateFormat()+"-"+ed.AddDays(-1).Date.ToDefaultDateFormat(), ".xlsx");
            var ms = await _projectManagementBusiness.GetHourSpentReportDataExcel(result,projectId,assigneeId, sStdate,sEtdate);

            return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", report);
        }
        public async Task<ActionResult> ReadData(string projectId,string assigneeId,string sdate, string edate)
        {
            
            //if (assigneeId.IsNullOrEmpty()) 
            //{
            //    assigneeId = _userContext.UserId;
            //}
               // search.UserId = _userContext.UserId;
            
            DateTime sd =  DateTime.Now;
            DateTime ed = DateTime.Now;
            //var result = _business.GetSearchResult(search).OrderByDescending(x => x.LastUpdatedDate);
            // var list = new List<TaskViewModel>();
            var result =await  _projectManagementBusiness.GetHourReportTaskData(null);
            //list.Add(result);


            if (assigneeId.IsNotNullAndNotEmpty())
            {
                result = result.Where(x => x.AssigneeId == assigneeId).ToList();
            }

            if (projectId.IsNotNull())
            {
                result = result.Where(x => x.ProjectId == projectId).ToList();
            }

            //if (search.Taskfilter.IsNotNull())
            //{
            //    result = result.Where(x => x.TaskGroup == search.Taskfilter).ToList();
            //}

            if (sdate.IsNotNullAndNotEmpty() && sdate!="null")
            {
                sd = Convert.ToDateTime(sdate);
                result = result.Where(x => x.WSDate.Value.Date >= sd.Date).ToList();
                // var d = result.Select(x => x.WSDate).ToList();
               // result = result.Where(x => x.WSDate.HasValue ? x.WSDate.Value/*.ToShortDateString()*/ >= sd/*.ToShortDateString()*/ : x.WSDate >= sd).ToList();
            }
            if (edate.IsNotNullAndNotEmpty() && edate != "null")
            {
                ed = Convert.ToDateTime(edate);
                ed = ed.AddDays(1).Date;
                //var d = result.Select(x => x.WEDate).ToList();
                result = result.Where(x => x.WEDate.Value.Date < ed.Date).ToList();
                
               // result = result.Where(x => x.WEDate.HasValue ? x.WEDate.Value/*.ToShortDateString()*/ < ed/*.ToShortDateString()*/ : x.WEDate < sd).ToList();
            }

            var j = Json(result);
          
            return j;
        }
    }
}
