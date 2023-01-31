using CMS.Business;
using CMS.Common;
using CMS.UI.Utility;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.PJM.Controllers
{
    [Area("PJM")]
    public class ProjectEmailSetupController : ApplicationController
    {
        private readonly IProjectEmailSetupBusiness _projectEmailSetupBusiness;
        private readonly IEmailBusiness _emailBusiness;
        private readonly IUserBusiness _userBusiness;
        IUserContext _userContext;
        public ProjectEmailSetupController(IProjectEmailSetupBusiness projectEmailSetupBusiness, IEmailBusiness emailBusiness
            , IUserBusiness userBusiness, IUserContext userContext)
        {
            _projectEmailSetupBusiness = projectEmailSetupBusiness;
            _emailBusiness = emailBusiness;
            _userBusiness = userBusiness;
            _userContext = userContext;
         }

        public async Task<ActionResult> Index(string module)
        {
            var model = new ProjectEmailSetupViewModel();
            ViewBag.Module = module;
            return View(model);

        }
        public async Task<ActionResult> ReceiveEmail()
        {
            await _emailBusiness.ReceiveMail();
            return View("Index");
        }

        public async Task<ActionResult> ReadEmailData()
        {            
            var list = await _projectEmailSetupBusiness.GetEmailSetupList();
            return Json(list);
        }

        public async Task<ActionResult> GetUsersIdNameList()
        {
            List<UserViewModel> list = new List<UserViewModel>();
            list = await _userBusiness.GetList();
            if (_userContext.IsSystemAdmin == false)
            {
                list = list.Where(x => x.Id == _userContext.UserId).ToList();
            }
            return Json(list);
        }

        public IActionResult Create(string module)
        {
            ViewBag.Module = module;
            return View("ManageEmailSetup", new ProjectEmailSetupViewModel
            {
                DataAction = DataActionEnum.Create,
            });
        }
        public async Task<IActionResult> Edit(string Id, string module)
        {
            ViewBag.Module = module;
            var module1 = await _projectEmailSetupBusiness.GetSingleById(Id);

            if (module1 != null)
            {
                module1.SmtpPassword = Helper.Decrypt(module1.SmtpPassword);
                module1.DataAction = DataActionEnum.Edit;
                return View("ManageEmailSetup", module1);
            }
            return View("ManageEmailSetup", new ProjectEmailSetupViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Manage(ProjectEmailSetupViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _projectEmailSetupBusiness.Create(model);
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
                    var result = await _projectEmailSetupBusiness.Edit(model);
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

            return View("ManageEmailSetup", model);
        }
        public async Task<IActionResult> Delete(string id)
        {
            await _projectEmailSetupBusiness.Delete(id);
            return View("Index", new ProjectEmailSetupViewModel());
        }

        public async Task<ActionResult> TestEmail(string id)
        {
           var res= await _projectEmailSetupBusiness.TestEmail(id);
            return Json(new { success = res });
        }

        public async Task<ActionResult> GetProjectsList()
        {
           

            var projectList = await _projectEmailSetupBusiness.GetProjectsList();
            var j = Json(projectList);
            return j;
        }


    }
}