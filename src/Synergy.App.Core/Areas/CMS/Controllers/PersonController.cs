using Synergy.App.Business;
using Synergy.App.WebUtility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class PersonController : ApplicationController
    {
        private readonly ICmsBusiness _cmsBusiness;
        private readonly IHRCoreBusiness _hrCoreBusiness;
        public PersonController(ICmsBusiness cmsBusiness, IHRCoreBusiness hrCoreBusiness)
        {
            _cmsBusiness = cmsBusiness;
            _hrCoreBusiness = hrCoreBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetActivePersonList()
        {            
            var personList = await _hrCoreBusiness.GetPersonListByOrgId(null);
            //var personList = await _cmsBusiness.GetActivePersonList();
            return Json(personList);
        }
        public async Task<ActionResult> GetActivePersonValueMapper(string value, string filters)
        {
            long dataItemIndex = -1;

            if (value != null)
            {
                var list = await _hrCoreBusiness.GetFilteredPersonListByOrgId(filters, 0, 0, value);
                //await _userBusiness.GetSwitchUserList(null, null, false, filters, 0, 0);
                dataItemIndex = list.ItemIndex;
            }
            return Json(dataItemIndex);
        }
        public async Task<ActionResult> GetPersonVirtualData(int page, int pageSize, string filters, string hierarchyId, string parentId = null)
        {
            var list = await _hrCoreBusiness.GetFilteredPersonListByOrgId(filters, pageSize, page, null);
            //await _userBusiness.GetSwitchUserList(_userContext.UserId, _userContext.LoggedInAsByUserId, hasLoggedinAsPermission, filters, pageSize, page);

            return Json(new { Data = list.Data, Total = list.Total });

        }
    }
}
