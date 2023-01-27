using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.Recruitment.Controllers
{
    [Area("Recruitment")]
    public class MasterController : ApplicationController
    {

        private IMasterBusiness _MasterBusiness;
        private readonly IUserContext _userContext;
        private readonly IHiringManagerBusiness _hmBusiness;
        public MasterController(IMasterBusiness MasterBusiness, IUserContext userContext,IHiringManagerBusiness hmBusiness)
        {
            _MasterBusiness = MasterBusiness;
            _userContext = userContext;
            _hmBusiness = hmBusiness;
        }
        [HttpGet]
        public async Task<JsonResult> GetIdNameList(string type, string viewData = null)
        {
            var data = await _MasterBusiness.GetIdNameList(type);
            if (viewData != null)
            {
                ViewData[viewData] = data;
            }
            return Json(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetOrganizationIdNameByRecruitmentList(string jobAddId)
        {
            IList<IdNameViewModel> list = new List<IdNameViewModel>();
            var data = await _MasterBusiness.GetOrgByJobAddId(jobAddId);
            var Role =  _userContext.UserRoleCodes;
            if (Role.Contains("HM")) 
            {
                var orglist=await _hmBusiness.GetHmOrg(_userContext.UserId);
                foreach (var obj in data) 
                {
                    if (orglist.Any(x=>x.Id==obj.Id)) 
                    {
                        list.Add(obj);
                    }
                }
                return Json(list);
            }
            else if (Role.Contains("ORG_UNIT"))
            {
                var orglist = await _hmBusiness.GetHODOrg(_userContext.UserId);
                foreach (var obj in data)
                {
                    if (orglist.Any(x => x.Id == obj.Id))
                    {
                        list.Add(obj);
                    }
                }
                return Json(list);
            }
            return Json(data);
        }

    }
}