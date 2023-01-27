using AutoMapper;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class HelpController : ApplicationController
    {
        private IMapper _autoMapper;
        private readonly IUserContext _userContext;
        private readonly IHRCoreBusiness _hrCoreBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly ITeamBusiness _teamBusiness;
        public HelpController(IUserContext userContext, IServiceBusiness serviceBusiness, IUserBusiness userBusiness,
             IHRCoreBusiness hrCoreBusiness, IMapper autoMapper, INoteBusiness noteBusiness, ITeamBusiness teamBusiness)
        {
            _userContext = userContext;
            _serviceBusiness = serviceBusiness;
            _userBusiness = userBusiness;
            _hrCoreBusiness = hrCoreBusiness;
            _autoMapper = autoMapper;
            _noteBusiness = noteBusiness;
            _teamBusiness = teamBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> HelpPost(string type)
        {
            //ViewBag.Title = ERP.Translation.Nts.Master.Dashboard;// "Dashboard";
            //var fbDashboard = _serviceBusiness.GetFBDashboardCount(LoggedInUserId);
            //fbDashboard.OwnerDisplayName = LoggedInUserName;
            ////  if (fbDashboard.base64Img == "")
            ////  {
            ////      fbDashboard.base64Img = GenerateAvatar(LoggedInUserName);
            ////  }    CompanyOrgId: Int @cypher(statement: "match (this)-[:R_User_LegalEntity_OrganizationRoot{IsPrimary:true}]->(leo:HRS_OrganizationRoot) optional match (leo)<-[:R_LegalEntity_OrganizationRoot]->(le:ADM_LegalEntity) return leo.Id as LegalEntityId")

            //var companyOrg = _orgBusiness.LegalEntityId;
            //fbDashboard.OrganizationId = _orgBusiness.LegalEntityId;
            //fbDashboard.Operation = DataOperation.Read;
            //fbDashboard.PositionId = LoggedInUserPositionId ?? 0;
            //_business.LogActivity(LoggedInUserId, DataOperationEvent.View, "Visited help library", null, null, Helper.GetUrlLocalpath(), Helper.GetQueryString());
            //return View(fbDashboard);

            //await _attendanceBusiness.UpdateAttendanceTable(DateTime.Now);
            ViewBag.Title = "Dashboard";// "Dashboard";
            var LoggedInUser = await _userBusiness.GetSingleById(_userContext.UserId);
            var fbDashboard = await _serviceBusiness.GetFBDashboardCount(_userContext.UserId);
            fbDashboard.OwnerDisplayName = LoggedInUser.Name;

            var companyOrg = await _hrCoreBusiness.GetCompanyOrganization(_userContext.UserId);
            fbDashboard.DepartmentId = companyOrg != null ? (companyOrg.Id ?? "") : "";
            fbDashboard.DataAction = DataActionEnum.Read;
            //fbDashboard.PositionId = LoggedInUserPositionId ?? 0;
            return View(fbDashboard);
        }

        public async Task<IActionResult> ReadManagePostTile(EndlessScrollingRequest param)
        {
            if (param.UserId.IsNullOrEmpty())
            {
                param.UserId = _userContext.UserId;
            }
            param.LoggendInUserId = _userContext.UserId;
            var list = await _hrCoreBusiness.GetGroupMessage(param);
            var distinct = list.GroupBy(x => x.Id)
                                  .Select(g => g.First())
                                  .ToList();
            //if (param.ModuleName != null && param.HomeType == "UserGuide")
            //{
            //    distinct = distinct.Where(x => x.ModuleName == param.ModuleName).ToList();
            //}

            foreach (var x in distinct)
            {
                x.SequenceOrder = x.SequenceOrder ?? 0;
            }

            //if (param.SearchParam.IsNotNullAndNotEmpty())
            //{
            //    distinct = distinct.Where(x => x.NoteDescription.Contains(param.SearchParam)).ToList();
            //}

            distinct = distinct.OrderBy(x => x.SequenceOrder).ToList();

            var j = Json(distinct);

            return j;
        }        
        
    }
}
