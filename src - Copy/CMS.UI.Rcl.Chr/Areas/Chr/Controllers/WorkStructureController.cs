using CMS.Business;
using CMS.Common;
using CMS.UI.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.CHR.Controllers
{
    [Area("CHR")]
    public class WorkStructureController : ApplicationController
    {
        IHRCoreBusiness _hRCoreBusiness;
        private readonly IUserContext _userContext;
        ITableMetadataBusiness _tableMetadataBusiness;

        public WorkStructureController(IHRCoreBusiness hrCoreBusiness, IUserContext userContext,
            ITableMetadataBusiness tableMetadataBusiness)
        {
            _hRCoreBusiness = hrCoreBusiness;
            _userContext = userContext;
            _tableMetadataBusiness = tableMetadataBusiness;
        }

        public async Task<IActionResult> Index()
        {
            var loggedInPerson = await _tableMetadataBusiness.GetTableDataByColumn("HRPerson", null, "UserId", _userContext.UserId);
            if(loggedInPerson.IsNotNull())
            {
                ViewBag.LoggedInPersonId = Convert.ToString(loggedInPerson["Id"]);
            } else
            {
                ViewBag.LoggedInPersonId = null;
            }
            
            return View();
        }
        public async Task<ActionResult> GetWorkStructureDiagram(string personId)
        {
            var list = await _hRCoreBusiness.GetWorkStructureDiagram(personId);
            return Json(list);
        }
    }
}
