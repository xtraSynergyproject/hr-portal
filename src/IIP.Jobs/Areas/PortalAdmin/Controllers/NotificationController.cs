using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Synergy.App.Api.Areas.PortalAdmin.Controllers
{
    [Route("portaladmin/notification")]
    [ApiController]
    public class NotificationController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly INotificationBusiness _notificationBusiness;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public NotificationController(AuthSignInManager<ApplicationIdentityUser> customUserManager
            , IServiceProvider serviceProvider, INotificationBusiness notificationBusiness, IUserContext userContext) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _notificationBusiness = notificationBusiness;
        }

        [HttpGet]
        [Route("ReadNotificationData")]
        public async Task<IActionResult> ReadNotificationData( DateTime date, ReferenceTypeEnum? refType = null, bool read = false, bool archive = false, string refTypeId = null, bool completedStatus = false)
        {
            var list = await _notificationBusiness.GetNotificationList(date, refType, read, archive, refTypeId);
            foreach (var lst in list)
            {
                string pattern = @$"<a id='ext_url'.+?</a>";
                var r = new Regex(pattern, RegexOptions.Singleline);
                lst.Body = lst.Body.IsNotNullAndNotEmpty() ? r.Replace(lst.Body, "") : "";
            }
            if (!completedStatus)
            {
                list = list.Where(x => x.ActionStatus != NotificationActionStatusEnum.Completed).ToList();
            }
            var json = Ok(list);
            return json;
        }

        [HttpGet]
        [Route("ReadNotificationlist")]
        public async Task<IActionResult> ReadNotificationlist(string userId,string portalName,DateTime? date, ReferenceTypeEnum? refType = null, bool read = false, bool archive = false, string refTypeId = null, bool completedStatus = false)
        {
            await Authenticate(userId);
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var list = new List<NotificationViewModel>();
            if (refType.IsNotNull())
            {
                var nlist = await _notificationBusiness.GetNotificationList(_userContext.UserId, _userContext.PortalId, 20);
                list = nlist.Where(x => x.CreatedDate.Date == date.Value.Date && x.ReferenceType == refType).ToList();
            }
            else
            {
                var nlist = await _notificationBusiness.GetNotificationList(_userContext.UserId, _userContext.PortalId, 20, refTypeId);
                list = nlist.ToList();
            }

            if (read)
            {
                list = list.Where(x => x.ReadStatus != ReadStatusEnum.Read).ToList();
            }
            if (!archive)
            {
                list = list.Where(x => x.IsArchived == false).ToList();
            }
            if (!completedStatus)
            {
                list = list.Where(x => x.ActionStatus != NotificationActionStatusEnum.Completed).ToList();
            }
            foreach (var lst in list)
            {
                string pattern = @$"<a id='ext_url'.+?</a>";
                var r = new Regex(pattern, RegexOptions.Singleline);
                lst.Body = lst.Body.IsNotNullAndNotEmpty() ? r.Replace(lst.Body, "") : "";
            }
            var json = Ok(list);
            return json;
        }
    }
}
