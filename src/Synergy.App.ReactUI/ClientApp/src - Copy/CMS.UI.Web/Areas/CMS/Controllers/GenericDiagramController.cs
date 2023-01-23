using CMS.Business;
using CMS.Common;
using CMS.UI.Utility;
using CMS.UI.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class GenericDiagramController : ApplicationController
    {
        INoteBusiness _noteBusiness;
        ILOVBusiness _lovBusiness;
        ITemplateBusiness _templateBusiness;
        ITableMetadataBusiness _tableMetadataBusiness;
        ITaskBusiness _taskBusiness;
        IUserContext _userContext;
        IBusinessDiagramBusiness _businessDiagramBusiness;
        IPortalBusiness _portalBusiness;

        public GenericDiagramController(INoteBusiness noteBusiness, ILOVBusiness lovBusiness, ITemplateBusiness templateBusiness,
            IUserContext userContext, IBusinessDiagramBusiness businessDiagramBusiness,
            ITableMetadataBusiness tableMetadataBusiness, IPortalBusiness portalBusiness,
            ITaskBusiness taskBusiness)
        {
            _noteBusiness = noteBusiness;
            _lovBusiness = lovBusiness;
            _templateBusiness = templateBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _taskBusiness = taskBusiness;
            _userContext = userContext;
            _businessDiagramBusiness = businessDiagramBusiness;
            _portalBusiness = portalBusiness;
        }
        public async Task<IActionResult> Index()
        {
            var portalDetails = await _portalBusiness.GetSingle(x => x.Name == "CMS");
            ViewBag.PortalId = "";
            if(portalDetails.IsNotNull())
            {
                ViewBag.PortalId = portalDetails.Id;
            }
            return View();
        }
        public async Task<JsonResult> GetCategoryTreeList(string id, string parentId)
        {
            var list = new List<TreeViewViewModel>();
            if (id.IsNullOrEmpty())
            {
                list.Add(new TreeViewViewModel
                {
                    id = "Category",
                    Name = "Category",
                    DisplayName = "Category",
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = "Root",
                    children = true,
                    text = "Category",
                    parent = "#",
                    a_attr = new { data_id = "Category", data_name = "Category", data_type = "Root" },
                });
            }
            else if (id == "Category")
            {
                var noteList = await _noteBusiness.GetList(x=>x.TemplateCode == "GENERIC_DIAGRAM_CATEGORY" && x.ParentNoteId == null);
                var treelist = noteList.Select(x => new TreeViewViewModel()
                {
                    id = x.Id,
                    Name = x.NoteSubject,
                    DisplayName = x.NoteSubject,
                    ParentId = x.ParentNoteId.IsNotNull() ? x.ParentNoteId : "Category",
                    hasChildren = true,
                    expanded = true,
                    Type = "generic",
                    children = true,
                    text = x.NoteSubject,
                    parent = x.ParentNoteId.IsNotNull() ? x.ParentNoteId : "Category",
                    a_attr = new { data_id = x.Id, data_name = x.NoteSubject, data_type = "generic" },
                });
                list.AddRange(treelist);
            } else
            {
                var noteList = await _noteBusiness.GetList(x => x.ParentNoteId == id);
                var treelist = noteList.Select(x => new TreeViewViewModel()
                {
                    id = x.Id,
                    Name = x.NoteSubject,
                    DisplayName = x.NoteSubject,
                    ParentId = x.ParentNoteId.IsNotNull() ? x.ParentNoteId : "Category",
                    hasChildren = true,
                    expanded = true,
                    Type = "generic",
                    children = true,
                    text = x.NoteSubject,
                    parent = x.ParentNoteId.IsNotNull() ? x.ParentNoteId : "Category",
                    a_attr = new { data_id = x.Id, data_name = x.NoteSubject, data_type = "generic" },
                });
                list.AddRange(treelist);
            }
            return Json(list.ToList());
        }

        public IActionResult ManageCategory(string id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ManageCategory(string subject, string parentId)
        {
            var model = new NoteTemplateViewModel
            {
                TemplateCode = "GENERIC_DIAGRAM_CATEGORY"
            };
            var note = await _noteBusiness.GetNoteDetails(model);
            note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
            note.NoteSubject = subject;
            note.StartDate = DateTime.Now;
            var lovMeduim = await _lovBusiness.GetSingle(x => x.Code == "NOTE_PRIORITY_MEDIUM");
            note.NotePriorityId = lovMeduim != null ? lovMeduim.Id : "";
            note.DataAction = Common.DataActionEnum.Create;
            note.ParentNoteId = parentId.IsNullOrEmpty() ? null : parentId;
            var result = await _noteBusiness.ManageNote(note);
            return Json(result);
        }

        public async Task<ActionResult> GetGenericBusinessDiagram(string templateId)
        {
            var busDiagram = new TaskTemplateViewModel();
            string businessDiagramJson = null;
            var data = await _tableMetadataBusiness.GetTableDataByColumn("BUSINESS_DIAGRAM", null, "diagramTemplateId", templateId);
            if (data != null)
            {
                var noteId = Convert.ToString(data["NtsNoteId"]);
                if (noteId.IsNotNullAndNotEmpty())
                {
                    var task = await _taskBusiness.GetSingle(x => x.UdfNoteId == noteId);
                    if (task.IsNotNull())
                    {
                        var bDiagram = new TaskTemplateViewModel
                        {
                            TaskId = task.Id
                        };
                        busDiagram = await _taskBusiness.GetTaskDetails(bDiagram);
                    }
                }
                businessDiagramJson = Convert.ToString(data["diagramJson"]);
            }
            else
            {
                var bDiagram = new TaskTemplateViewModel
                {
                    ActiveUserId = _userContext.UserId,
                    TemplateCode = "BUSINESS_DIAGRAM",
                };
                var bDiagramTask = await _taskBusiness.GetTaskDetails(bDiagram);
                bDiagramTask.TaskSubject = "Business Diagram";
                bDiagramTask.OwnerUserId = _userContext.UserId;
                bDiagramTask.StartDate = DateTime.Now;
                bDiagramTask.DueDate = DateTime.Now;
                bDiagramTask.AssignedToUserId = _userContext.UserId;
                bDiagramTask.DataAction = DataActionEnum.Create;
                bDiagramTask.CreatedBy = _userContext.UserId;
                bDiagramTask.CompanyId = _userContext.UserId;
                bDiagramTask.CreatedDate = DateTime.Now;
                bDiagramTask.LastUpdatedBy = _userContext.UserId;
                bDiagramTask.LastUpdatedDate = DateTime.Now;
                var busDiagramFull = await _taskBusiness.ManageTask(bDiagramTask);
                busDiagram = busDiagramFull.Item;
            }
            return Json(new
            {
                standard = businessDiagramJson,
                diagramDetails = busDiagram
            });
        }

        [HttpPost]
        public async Task<IActionResult> SaveBusinessDiagram(BusinessDiagramViewModel model)
        {
            var res = await _businessDiagramBusiness.ManageGenericDiagramTask(model);
            return Json(res);
        }
    }
}
