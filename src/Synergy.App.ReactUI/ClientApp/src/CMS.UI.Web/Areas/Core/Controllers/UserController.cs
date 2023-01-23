using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synergy.App.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Synergy.App.ViewModel;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.WebUtility;

namespace CMS.UI.Web.Controllers
{
    public class UserController : ApplicationController
    {
        #region "Declarations"        
        IUserBusiness _business;
        IDocumentBusiness _documentBusiness;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private readonly IUserContext _userContext;

        #endregion

        #region "Constructor"
        public UserController(IUserBusiness business, IDocumentBusiness documentBusiness, AuthSignInManager<ApplicationIdentityUser> customUserManager,
            IUserContext userContext)
        {
            _business = business;
            _documentBusiness = documentBusiness;
            _customUserManager = customUserManager;
            _userContext = userContext;
        }
        #endregion

        // GET: UserController
        public async Task<IActionResult> Index()
        {
            var model = await _business.GetList();
            return View(model);
        }

        public ActionResult ReadUserData([DataSourceRequest] DataSourceRequest request)
        {
            var model = _business.GetList();
            var data = model.Result.ToList();
            var dsResult = data.ToDataSourceResult(request);
            return Json(dsResult);
        }

        public ActionResult ReadData()
        {
            var model = _business.GetList();
            var data = model.Result.ToList();           
            return Json(data);
        }
        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            var model = new UserViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _business.Create(model);
                if (result.IsSuccess)
                {
                    return PopupRedirect("User created successfully", true, true, "/user/create", "Create New User");
                }
                else
                {
                    ModelState.AddModelErrors(result.Messages);
                }
            }
            return View(model);
        }

        //// POST: UserController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: UserController/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var model = await _business.GetSingleById(id);
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _business.Edit(model);
                if (result.IsSuccess)
                {
                    return PopupRedirect("User edited successfully");
                }
                else
                {
                    ModelState.AddModelErrors(result.Messages);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> MyProfile(string id)
        {
            var model = await _business.GetSingleById(id);
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> MyProfile(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingItem = await _business.GetSingleById(model.Id);
                existingItem.Name = model.Name;
                existingItem.JobTitle = model.JobTitle;
                existingItem.Email = model.Email;
                existingItem.PhotoId = model.PhotoId;
                existingItem.ConfirmPassword = existingItem.Password;

                var result = await _business.Edit(existingItem);
                if (result.IsSuccess)
                {
                    var user = await _business.ValidateUser(_userContext.Email);

                    if (user != null)
                    {

                        var identity = new ApplicationIdentityUser
                        {
                            Id = user.Id,
                            UserName = user.Name,
                            IsSystemAdmin = user.IsSystemAdmin,
                            Email = user.Email,
                            UserUniqueId = user.Email,
                            CompanyId = user.CompanyId,
                            CompanyCode = user.CompanyCode,
                            CompanyName = user.CompanyName,
                            JobTitle = user.JobTitle,
                            PhotoId = user.PhotoId,
                            UserRoleCodes = string.Join(",", user.UserRoles.Select(x => x.Code)),
                            UserRoleIds = string.Join(",", user.UserRoles.Select(x => x.Id)),
                            PortalId = _userContext.PortalId,
                            UserPortals=user.UserPortals,
                            LegalEntityId = user.LegalEntityId,
                            LegalEntityCode = user.LegalEntityCode,
                            PersonId = user.PersonId,
                            PositionId = user.PositionId,
                            DepartmentId = user.DepartmentId,
                        };
                        if (_userContext.PortalId.IsNotNullAndNotEmpty())
                        {
                            var portal = await _business.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
                            if (portal != null)
                            {
                                identity.PortalTheme = portal.Theme.ToString();
                                identity.PortalName = portal.Name;
                            }
                        }
                        identity.MapClaims();                       
                        await _customUserManager.SignInAsync(identity, true);
                    }
                }
                return Json(new { success = true });
                //ViewBag.Success = true;
            }
            else
            {
                
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                //ModelState.AddModelErrors(result.Messages);
            }
            //return View(model);
          
        }

        // POST: UserController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
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

        public async Task<IActionResult> ProfilePhoto(string id)
        {

            var image = await _documentBusiness.GetSingleById(id);
            if (image != null)
            {
                return File(image.Content, "image/jpeg");
            }
            return null;
        }

        public async Task<IActionResult> SignaturePhoto(string id)
        {

            var image = await _documentBusiness.GetSingleById(id);
            if (image != null)
            {
                return File(image.Content, "image/jpeg");
            }
            return null;
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult LogOut()
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

        public async Task<IActionResult> ChangeUserProfilePhoto(string photoId, string userId)
        {

            await _business.ChangeUserProfilePhoto(photoId, userId);
            return Json(new { success = true });

        }

        public async Task<IActionResult> ChangeUserSignature(string photoId, string userId)
        {

            await _business.ChangeUserSignature(photoId, userId);
            return Json(new { success = true });

        }
    }
}
