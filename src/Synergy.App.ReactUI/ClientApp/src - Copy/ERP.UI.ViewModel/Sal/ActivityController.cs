using ERP.Authorization;
using ERP.Business.Base;
using ERP.Data.Model;
using ERP.UI.ViewModel;
using ERP.UI.Web.Controllers;
using ERP.Utility;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ERP.UI.Web.Areas.Sal.Controllers
{
    public class ActivityController : Controller
    {
        #region "Declarations"

        private IActivityBusiness _business;

        #endregion

        #region "Constructor"

        public ActivityController(IActivityBusiness business)
        {
            _business = business;

        }

        #endregion

        #region "Method"



        #endregion

        #region "Action"

        [BasicAuthorize(2240)]
        public ActionResult Index()
        {
            ViewBag.Title = "Manage Activity";
            return View(new ActivityViewModel());
        }

        [BasicAuthorize(2240)]
        public ActionResult Create()
        {
            ViewBag.Title = "Create Activity";
            return View("Manage", new ActivityViewModel { Operation = DataOperation.Create });
        }

        [BasicAuthorize(2240)]
        public ActionResult Correct(long id)
        {
            ViewBag.Title = "Correct Activity";
            var model = _business.GetSingle(x => x.Id == id);
            model.Operation = DataOperation.Correct;
            return View("Manage", model);
        }

        public ActionResult Delete(long id)
        {
            ViewBag.Title = "Delete Activity";
            var model = _business.GetSingle(x => x.Id == id);
            model.Operation = DataOperation.Delete;
            return View("Manage", model);
        }
        [HttpPost]
        public ActionResult Manage(ActivityViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Operation == DataOperation.Create)
                {
                    return Create(model);
                }
                if (model.Operation == DataOperation.Correct)
                {
                    return Correct(model);
                }
                else if (model.Operation == DataOperation.Delete)
                {
                    return Delete(model);
                }
                else
                {
                    ModelState.AddModelError("InvalidOperation", "Invalid Operation");
                    return Json(new { success = false, errors = ModelState.SerializeErrors() });
                }
            }
            else
            {
                return Json(new { success = false, errors = ModelState.SerializeErrors() });
            }
        }


        [BasicAuthorize(2240)]
        private ActionResult Create(ActivityViewModel model)
        {
            var result = _business.Create(model);
            if (!result.IsSuccess)
            {
                result.Messages.Each(x => ModelState.AddModelError(x.Key, x.Value));
                return Json(new { success = false, errors = ModelState.SerializeErrors() });
            }
            else
            {
                return Json(new { success = true, operation = model.Operation.ToString(), id = model.Id });
            }
        }



        private ActionResult Correct(ActivityViewModel model)
        {
            var result = _business.Correct(model);
            if (!result.IsSuccess)
            {
                result.Messages.Each(x => ModelState.AddModelError(x.Key, x.Value));
                return Json(new { success = false, errors = ModelState.SerializeErrors() });
            }
            else
            {
                return Json(new { success = true, operation = model.Operation.ToString(), id = model.Id });
            }
        }
        private ActionResult Delete(ActivityViewModel model)
        {
            var result = _business.Delete(model);
            if (!result.IsSuccess)
            {
                result.Messages.Each(x => ModelState.AddModelError(x.Key, x.Value));
                return Json(new { success = false, errors = ModelState.SerializeErrors() });
            }
            else
            {
                return Json(new { success = true, operation = model.Operation.ToString(), id = model.Id });
            }
        }

        public ActionResult ReadActivityData([DataSourceRequest] DataSourceRequest request, ActivityViewModel search = null)
        {
            var model = _business.GetSearchResult(search);
            var j = Json(model.ToDataSourceResult(request));
            return j;
        }
        [HttpGet]
        public ActionResult GetIdNameList()
        {
            var data = _business.GetActiveIdNameList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}