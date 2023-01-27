using CMS.Business;
using CMS.Common;
using CMS.UI.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.Pms.Controllers
{
    [Area("Pms")]
    public class PerformanceDiagramController : ApplicationController
    {
        private readonly IPerformanceManagementBusiness _performanceDiagram;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly IHRCoreBusiness _hrCoreBusiness;
        private readonly IPerformanceManagementBusiness _performanceManagementBusiness;
        private readonly IUserContext _userContext;

        public PerformanceDiagramController(IPerformanceManagementBusiness performanceDiagram, ICmsBusiness cmsBusiness, IHRCoreBusiness hrCoreBusiness,
            IPerformanceManagementBusiness performanceManagementBusiness, IUserContext userContext)
        {
            _performanceDiagram = performanceDiagram;
            _cmsBusiness = cmsBusiness;
            _hrCoreBusiness = hrCoreBusiness;
            _performanceManagementBusiness = performanceManagementBusiness;
            _userContext = userContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetPerformanceDiagram(string performanceDocumentId)
        {
            var model = await _performanceDiagram.GetPerformanceDiagram(performanceDocumentId);
            return Json(model);
        }

        public async Task<IActionResult> GetUserList()
        {
            var position = await _hrCoreBusiness.GetPostionHierarchyParentId(null);
            if (position != null && position.Id.IsNotNullAndNotEmpty())
            {
                var positionchildList = await _hrCoreBusiness.GetPositionHierarchy(position.Id, 1);
                if (positionchildList != null && positionchildList.Count > 0)
                {
                    var list = positionchildList.ConvertAll(x => new IdNameViewModel
                    {
                        Id = x.UserId,
                        Name = x.DisplayName
                    }).ToList();

                    list.Add(new IdNameViewModel
                    {
                        Id = _userContext.UserId,
                        Name = "Self",
                    });
                    return Json(list.Where(x=>x.Id.IsNotNull()).GroupBy(x => x.Id)
                                  .Select(g => g.First())
                                  .ToList());
                }
            }
            return Json("");
        }
        
        public async Task<IActionResult> GetPerformaceListByUser(string userId)
        {
            var data = await _performanceManagementBusiness.GetPerformanceSharedList(userId);
            return Json(data);
        }
    }
}
