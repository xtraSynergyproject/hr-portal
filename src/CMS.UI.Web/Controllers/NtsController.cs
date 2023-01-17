using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
using CMS.Web;
using Hangfire;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CMS.UI.Web
{
    public class NtsController : ApplicationController
    {
        private readonly IUserBusiness _userBusiness;
        private readonly IUserContext _userContext;
        private readonly IPortalBusiness _portalBusiness;
        private readonly IPushNotificationBusiness _notifyBusiness;
        private readonly INtsBusiness _ntsBusiness;

        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public NtsController(IUserBusiness userBusiness,
            AuthSignInManager<ApplicationIdentityUser> customUserManager, IUserContext userContext
            , IPortalBusiness portalBusiness, IPushNotificationBusiness notifyBusiness,
            INtsBusiness ntsBusiness)
        {
            _userBusiness = userBusiness;
            _customUserManager = customUserManager;
            _userContext = userContext;
            _portalBusiness = portalBusiness;
            _notifyBusiness = notifyBusiness;
            _ntsBusiness = ntsBusiness;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MessageBox()
        {
            return View();
        }

        public async Task<IActionResult> GetNtsMenuList(string id, string userId)
        {
            var result = await _ntsBusiness.GetNtsMenuList(id, _userContext.UserId, _userContext.Email);
            var model = result.ToList();
            return Json(model);
        }
        public async Task<IActionResult> UpdateRating(NtsTypeEnum ntsType, string ntsId, string userId, int rating, string ratingComment)
        {
            await _ntsBusiness.UpdateRating(ntsType, ntsId, userId, rating, ratingComment);
            return Json(new { success = true });
        }
        public async Task<IActionResult> RemoveRating(NtsTypeEnum ntsType, string ntsId, string userId)
        {
            await _ntsBusiness.RemoveRating(ntsType, ntsId, userId);
            return Json(new { success = true });
        }
        public async Task<IActionResult> ReadNtsList([DataSourceRequest] DataSourceRequest request)
        {
            //var result = await _taskBusiness.GetActiveListByUserId(_userContext.UserId);
            //var j = Json(result.Where(x => x.TaskStatusCode == "COMPLETED").OrderByDescending(x => x.StartDate).ToDataSourceResult(request));
            var a = new List<TaskViewModel>();
            a.Add(new TaskViewModel
            {
                TaskSubject = "Approval Task 1",
                AssignedToUserId = "Admin@synergy.com",
                DueDate = DateTime.Now
            }); a.Add(new TaskViewModel
            {
                TaskSubject = "Approval Task 2",
                AssignedToUserId = "Admin@synergy.com",
                DueDate = DateTime.Now
            }); a.Add(new TaskViewModel
            {
                TaskSubject = "Approval Task 3",
                AssignedToUserId = "Admin@synergy.com",
                DueDate = DateTime.Now
            }); a.Add(new TaskViewModel
            {
                TaskSubject = "Approval Task 4",
                AssignedToUserId = "Admin@synergy.com",
                DueDate = DateTime.Now
            }); a.Add(new TaskViewModel
            {
                TaskSubject = "Approval Task 5",
                AssignedToUserId = "Admin@synergy.com",
                DueDate = DateTime.Now
            }); a.Add(new TaskViewModel
            {
                TaskSubject = "Approval Task 6",
                AssignedToUserId = "Admin@synergy.com",
                DueDate = DateTime.Now
            });
            var j = Json(a.ToDataSourceResult(request));

            return Json(j);
        }

        public async Task<IActionResult> ReadAttachedReplies()
        {
            var result = await _ntsBusiness.GetAttachedReplies(_userContext.UserId, null);
            return Json(result);
        }
    }
}
