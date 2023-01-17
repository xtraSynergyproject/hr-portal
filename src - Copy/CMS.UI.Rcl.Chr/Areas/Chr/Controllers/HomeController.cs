using CMS.Business;
using CMS.UI.Utility;
using CMS.UI.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.CHR.Controllers
{
    [Area("CHR")]
    public class HomeController : ApplicationController
    {
        private IHRCoreBusiness _hrCoreBusiness;
        public HomeController(IHRCoreBusiness hrCoreBusiness
           )
        {
            _hrCoreBusiness = hrCoreBusiness;
           
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> EmployeeDashboard()
        {
          //  var AgeList = await _hrCoreBusiness.GetAllPersonAgeList();
          //  var SalaryList = await _hrCoreBusiness.GetAllPersonSalaryList();
          //  if (AgeList!=null && AgeList.Count()>0) 
          //  {
          //      ViewBag.MaxAge = AgeList.Max(t => t.Age);
          //      ViewBag.MinAge = AgeList.Where(x => x.Age != 0).Min(t => t.Age);
          //      ViewBag.AvgAge = AgeList.GroupBy(i => i.Age).OrderByDescending(grp => grp.Count())
          //.Select(grp => grp.Key).First();
          //  }
          //  if (SalaryList != null && SalaryList.Count() > 0)
          //  {
          //      ViewBag.HeighestSalary = SalaryList.Max(t => t.Salary);
          //      ViewBag.LowestSalary = SalaryList.Min(t => t.Salary);
          //      ViewBag.AverageSalary = SalaryList.GroupBy(i => i.Salary).OrderByDescending(grp => grp.Count())
          //.Select(grp => grp.Key).First();
          //  }
               
            return View();
        }
        public async Task<ActionResult> GetEmployeeByGender(string typeId)
        {
            //Perso search = new ServiceSearchViewModel();
           // search.UserId = _userContext.UserId;
            var viewModel = await _hrCoreBusiness.GetAllPersonList(typeId);
            var list1 = viewModel.Where(x=>x.GenderId!=null).GroupBy(x => x.Gender).Select(group => new { Value = group.Count(), Type = group.Select(x => x.Gender).FirstOrDefault(), Id = group.Select(x => x.Gender).FirstOrDefault() }).ToList();
            var list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type.ToString(), Value = x.Value, Id = x.Id.ToString() }).ToList();
            return Json(list);
        }
        public async Task<ActionResult> GetEmployeeByRole(string typeId)
        {
            //Perso search = new ServiceSearchViewModel();
            // search.UserId = _userContext.UserId;
            var viewModel = await _hrCoreBusiness.GetAllPersonOnRoleBasisList(typeId);
            var list1 = viewModel.GroupBy(x => x.IqamahJobTitle).Select(group => new { Value = group.Count(), Type = group.Select(x => x.IqamahJobTitle).FirstOrDefault(), Id = group.Select(x => x.IqamahJobTitle).FirstOrDefault() }).ToList();
            //var list1 = viewModel.GroupBy(x => x.UserRole).Select(group => new { Value = group.Count(), Type = group.Select(x => x.UserRole).FirstOrDefault(), Id = group.Select(x => x.UserRole).FirstOrDefault() }).ToList();
            var list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type.ToString(), Value = x.Value, Id = x.Id.ToString() }).ToList();
            return Json(list);
        }
        public async Task<ActionResult> GetEmployeeByAge(string typeId)
        {
           
            var viewModel = await _hrCoreBusiness.GetAllPersonAgeList(typeId);
            var list1 = viewModel.Where(x => x.AgeGroup != "").GroupBy(x => x.AgeGroup).Select(group => new { Value = group.Count(), Type = group.Select(x => x.AgeGroup).FirstOrDefault(), Id = group.Select(x => x.AgeGroup).FirstOrDefault() }).ToList();
            var list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type.ToString(), Value = x.Value, Id = x.Id.ToString() }).ToList();
            return Json(list.OrderBy(x=>x.Type));
        }
        public async Task<ActionResult> GetEmployeeBySalary(string typeId)
        {

            var viewModel = await _hrCoreBusiness.GetAllPersonSalaryList(typeId);
            var list1 = viewModel.Where(x => x.SalaryRange != "").GroupBy(x => x.SalaryRange).Select(group => new { Value = group.Count(), Type = group.Select(x => x.SalaryRange).FirstOrDefault(), Id = group.Select(x => x.SalaryRange).FirstOrDefault() }).ToList();
            var list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type.ToString(), Value = x.Value, Id = x.Id.ToString() }).ToList();
            return Json(list);
        }
        public async Task<ActionResult> GetEmployeeByNationality(string typeId)
        {

            var viewModel = await _hrCoreBusiness.GetAllPersonByCountryList(typeId);
            var list1 = viewModel.Where(x => x.NationalityName != "").GroupBy(x => x.NationalityName).Select(group => new { Value = group.Count(), Type = group.Select(x => x.NationalityName).FirstOrDefault(), Id = group.Select(x => x.SalaryRange).FirstOrDefault() }).ToList();
            var list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type.ToString(), Value = x.Value, Id = x.Id.ToString() }).ToList();
            return Json(list);
        }
        [HttpPost]
        public IActionResult Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }
    }
}
