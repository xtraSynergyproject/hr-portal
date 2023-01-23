using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Synergy.App.Business;
using Synergy.App.ViewModel;
using Synergy.App.Common;
using System.Net.Http.Headers;
////using Kendo.Mvc.UI;
////using Kendo.Mvc.Extensions;
using System.IO;
using Synergy.App.WebUtility;

namespace CMS.UI.Web.Controllers
{
    public class CompanyController : ApplicationController
    {

        ICompanyBusiness _companyBusiness;
        IDocumentBusiness _documentBusiness;
        ICompanySettingBusiness _companySettingBusiness;
        IUserContext _userContext;
        public CompanyController(ICompanyBusiness companyBusiness, IDocumentBusiness documentBusiness
            , ICompanySettingBusiness companySettingBusiness
            , IUserContext userContext)
        {
            _companyBusiness = companyBusiness;
            _documentBusiness = documentBusiness;
            _companySettingBusiness = companySettingBusiness;
            _userContext = userContext;
        }

      
        // GET: CompanyController
        public async Task<IActionResult> Index()
        {
            var _company = await _companyBusiness.GetListGlobal(x => true);
            return View(_company);
        }

        public ActionResult Company_Read([DataSourceRequest] DataSourceRequest request)
        {
            var result = new List<CompanyViewModel>();
            var _company = _companyBusiness.GetListGlobal(x => true);
            result = _company.Result.ToList();
            var dsResult = result;
            //var dsResult = result.ToDataSourceResult(request);
            return Json(dsResult);
        }
        public ActionResult ReadCompanyData()
        {
            var result = new List<CompanyViewModel>();
            var _company = _companyBusiness.GetListGlobal(x => true);
            result = _company.Result.ToList();          
            return Json(result);
        }

        public IActionResult AddCompany()
        {
            var model = new CompanyViewModel();
            model.DataAction = DataActionEnum.Create;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddCompany(CompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _companyBusiness.Create(model);
                if (result.IsSuccess)
                {
                    return PopupRedirect("Company Added successfully");
                }
                else
                {
                    ModelState.AddModelErrors(result.Messages);
                }
            }
            return View(model);
        }
        public async Task<IActionResult> EditCompany(string id)
        {
            var model = await _companyBusiness.GetSingleById(id);
            model.DataAction = DataActionEnum.Edit;
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> EditCompany(CompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _companyBusiness.Edit(model);
                if (result.IsSuccess)
                {
                    return PopupRedirect("Company edited successfully");
                }
                else
                {
                    ModelState.AddModelErrors(result.Messages);
                }
            }
            return View(model);
        }
        // GET: CompanyController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CompanyController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CompanyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CompanyController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CompanyController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CompanyController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        public async Task<IActionResult> CompanyLogo(string id)
        {

            var image = await _documentBusiness.GetSingleById(id);
            if (image != null)
            {
                return File(image.Content, "image/jpeg");
            }
            return null;
        }

        // POST: CompanyController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        //public ActionResult ReadData([DataSourceRequest] DataSourceRequest request)
        //{
        //    var model = _companyBusiness.GetData();
        //    var data = model.Result;

        //    var dsResult = data.ToDataSourceResult(request);
        //    return Json(dsResult);
        //}
     
        public async Task<ActionResult> ReadData()
        {
            var model = await _companyBusiness.GetList();
            return Json(model);
        }

        public IActionResult CreateComp()
        {
            return View("ManageCompany", new CompanyViewModel
            {
                DataAction = DataActionEnum.Create,
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                //GroupPortals=
            });
        }
        public async Task<IActionResult> EditComp(string Id)
        {
            var module = await _companyBusiness.GetSingleById(Id);

            if (module != null)
            {
                if (module.SmtpPassword.IsNotNull())
                {
                    module.SmtpPassword = Helper.Decrypt(module.SmtpPassword);
                }
                if (module.SmsPassword.IsNotNull())
                {
                    module.SmsPassword = Helper.Decrypt(module.SmsPassword);
                }
                module.DataAction = DataActionEnum.Edit;
                return View("ManageCompany", module);
            }
            return View("ManageCompany", new CompanyViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Manage(CompanyViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _companyBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;

                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _companyBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;

                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }

            return View("ManageCompany", model);
        }
        public async Task<IActionResult> DeleteComp(string id)
        {
            await _companyBusiness.Delete(id);
            return View("AdminCompany", new CompanyViewModel());
        }
        public IActionResult AdminCompany()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Save(IList<IFormFile> files)
        {
            try
            {
                foreach (var file in files)
                {
                    var ms = new MemoryStream();
                    file.OpenReadStream().CopyTo(ms);
                    var result = await _documentBusiness.Create(new DocumentViewModel
                    {
                        Content = ms.ToArray(),
                        ContentType = file.ContentType,
                        Length = file.Length,
                        Name = file.FileName
                    }
                    );
                    if (result.IsSuccess)
                    {
                        Response.Headers.Add("fileId", result.Item.Id);
                        Response.Headers.Add("fileName", result.Item.Name);
                        return Json(new { success = true, fileId = result.Item.Id, filename = result.Item.Name });
                    }
                    else
                    {
                        Response.Headers.Add("status", "false");
                        return Content("");
                    }



                }
            }
            catch (Exception ex)
            {

            }
            return Content("");
        }

        public IActionResult CompanySettingIndex()
        {
            return View();
        }
        public IActionResult CreateCompanySetting()
        {
            var model = new CompanySettingViewModel();
            model.DataAction = DataActionEnum.Create;
            model.CompanyId = _userContext.CompanyId;
            return View("ManageCompanySetting", model);
        }
        public async Task<IActionResult> EditCompanySetting(string Id)
        {
            var model = await _companySettingBusiness.GetSingleById(Id);
            model.DataAction = DataActionEnum.Edit;
            return View("ManageCompanySetting", model);
        }
        public async Task<IActionResult> DeleteCompanySetting(string id)
        {
            await _companySettingBusiness.Delete(id);
            return View("CompanySettingIndex", new CompanyViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> ManageCompanySetting(CompanySettingViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _companySettingBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _companySettingBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            return View("ManageCompanySetting", model);
        }
        public async Task<ActionResult> ReadCompanySettingData()
        {
            var list = await _companySettingBusiness.GetList();
            return Json(list);
        }


    }

}
