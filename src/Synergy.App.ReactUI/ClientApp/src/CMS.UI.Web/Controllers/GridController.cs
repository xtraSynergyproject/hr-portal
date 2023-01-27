using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Synergy.App.WebUtility;

namespace CMS.UI.Web.Controllers
{
    public class GridController : ApplicationController
    {
        //public ActionResult Orders_Read([DataSourceRequest] DataSourceRequest request)
        //{
        //    var result = Enumerable.Range(0, 50).Select(i => new OrderViewModel
        //    {
        //        OrderID = i,
        //        Freight = i * 10,
        //        OrderDate = new DateTime(2016, 9, 15).AddDays(i % 7),
        //        ShipName = "ShipName " + i,
        //        ShipCity = "ShipCity " + i
        //    });

        //    var dsResult = result.ToDataSourceResult(request);
        //    return Json(dsResult);
        //}
    }
}
