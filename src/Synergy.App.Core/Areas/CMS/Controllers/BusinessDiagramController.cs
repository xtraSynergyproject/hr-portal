using Synergy.App.Business;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class BusinessDiagramController : ApplicationController
    {
        private readonly INoteBusiness _noteBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly ILOVBusiness _lovBusiness;
        private readonly IBusinessDiagramBusiness _businessDiagramBusiness;
        public BusinessDiagramController(INoteBusiness noteBusines, ITemplateBusiness templateBusiness, ILOVBusiness lovBusiness,
            IBusinessDiagramBusiness businessDiagramBusiness)
        {
            _noteBusiness = noteBusines;
            _templateBusiness = templateBusiness;
            _lovBusiness = lovBusiness;
            _businessDiagramBusiness = businessDiagramBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ManageBusinessDiagram()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ManageDiagram(NoteTemplateViewModel model)
        {
            model.TemplateCode = "MASTERDIAGRAM";
            var note = await _noteBusiness.GetNoteDetails(model);
            note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
            note.NoteSubject = "Diagram";
            note.StartDate = DateTime.Now;
            var lovMeduim = await _lovBusiness.GetSingle(x => x.Code == "NOTE_PRIORITY_MEDIUM");
            note.NotePriorityId = lovMeduim != null ? lovMeduim.Id : "";
            note.DataAction = Synergy.App.Common.DataActionEnum.Create;
            var templateDetails = await _templateBusiness.GetSingle(x => x.Code == model.TemplateCode);
            if (note.ColumnList.Count > 0)
            {
                foreach (var x in note.ColumnList)
                {
                    if (x.Name == "diagramJson")
                    {
                        x.Value = model.Json;
                    }
                }
            }
            note.TemplateId = templateDetails != null ? templateDetails.Id : "";
            var result = await _noteBusiness.ManageNote(note);
            return null;
        }
        public ActionResult BusinessDiagram(string id, string lo)
        {
            if (lo == "Popup")
            {
                ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
                ViewBag.lo = "Popup";
            }
            else
            {
                ViewBag.Layout = "/Areas/Cms/Views/Shared/_LayoutCms.cshtml";
            }

            if (id != null && id != "")
            {
                ViewBag.TempId = id;
            }
            return View();
        }

        public ActionResult ConfigBusinessDiagramNode(string nodeId)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SaveBusinessDiagram(BusinessDiagramViewModel model)
        {
            var res = await _businessDiagramBusiness.ManageBusinessDiagramTask(model);
            return Json(res);
        }
    }
}
