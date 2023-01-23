using CMS.Business;
using CMS.UI.Utility;
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
    }
}
