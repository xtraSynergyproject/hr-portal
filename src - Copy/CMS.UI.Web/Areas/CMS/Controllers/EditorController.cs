using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CMS.Business;
using Microsoft.AspNetCore.Mvc;
using CMS.UI.ViewModel;
using CMS.Common;
using Kendo.Mvc.Extensions;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Kendo.Mvc.UI;
using CMS.Data.Model;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Net.Http;
using Syncfusion.EJ2.Base;
using System.Text;
using Npgsql;
using FastMember;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using Syncfusion.EJ2.Maps;
using System.Data;
using CMS.UI.Utility;

namespace CMS.UI.Web.Areas.CMS.Controllers


{
    //[Authorize]
    [Area("Cms")]

    public class EditorController : ApplicationController
    {

        private readonly IUserContext _userContext;
        private readonly INoteBusiness _noteBusiness;
        private readonly ITaskBusiness _taskBusiness;
        private readonly IWebHelper _webApi;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly IFileBusiness _fileBusiness;
        private readonly ILOVBusiness _lovBusiness;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public EditorController(IUserContext userContext, INoteBusiness noteBusiness, ITaskBusiness taskBusiness, IWebHelper webApi,
            ITableMetadataBusiness tableMetadataBusiness, ICmsBusiness cmsBusiness, IFileBusiness fileBusiness, ILOVBusiness lovBusiness, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {

            _userContext = userContext;
            _noteBusiness = noteBusiness;
            _taskBusiness = taskBusiness;
            _webApi = webApi;
            _tableMetadataBusiness = tableMetadataBusiness;
            _cmsBusiness = cmsBusiness;
            _fileBusiness = fileBusiness;
            _lovBusiness = lovBusiness;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            //ViewBag.Websites= await _noteBusiness.GetAllSynergyWebsite();
            var charts = await _noteBusiness.GetAllDashboardItemDetailsWithDashboard();
            var chartjson = JsonConvert.SerializeObject(charts.Select(x => new ChartListData { id = x.Id, name = x.Name }).ToArray());
            ViewBag.Charts = chartjson;
            return View("Editor");
        }
        public IActionResult BookEditor(string df = null, string hf = null, string jf = null, string cbm = null, string sf = null, string cf = null, string noteId = null)
        {
            ViewBag.DisplayFiled = df;
            ViewBag.HtmlField = hf;
            ViewBag.JsonField = jf;
            ViewBag.StyleField = sf;
            ViewBag.CssField = cf;
            ViewBag.cbm = cbm;
            ViewBag.Id = noteId;
            return View();
        }
        public IActionResult HtmlEditor(string displayfield = null, string htmlfield = null, string jsonfield = null, string cbm = null)
        {
            ViewBag.DisplayFiled = displayfield;
            ViewBag.HtmlField = htmlfield;
            ViewBag.JsonField = jsonfield;
            ViewBag.cbm = cbm;
            return View();
        }
        public async Task<IActionResult> Editor()
        {
            ViewBag.Websites = await _noteBusiness.GetAllSynergyWebsite();            
            return View("Index");
        }
        public async Task<IActionResult> Render(string id)
        {
            ViewBag.Id = id;
            ViewBag.Page = await _noteBusiness.GetSynergyWebsite(id);
            return View();
        }
        public async Task<IActionResult> Gallery(string id)
        {
            ViewBag.Id = id;
            return View();
        }
        public async Task<IActionResult> Manage()
        {
            return View();
        }

        public async Task<IActionResult> CopyLink(string url)
        {
            ViewBag.URL = url;
            return View();
        }
        public async Task<ActionResult> AddExistingPage(string categoryId, string bookId, string compId)
        {
            ViewBag.CategoryId = categoryId;
            ViewBag.BookId = bookId;
            ViewBag.CompId = compId;
            return View();
        }
        public async Task<ActionResult> AddAnalyticsChart(string compId)
        {            
            ViewBag.CompId = compId;
            return View();
        }
        public async Task<ActionResult> AddPageTemplate(string compId)
        {
            ViewBag.CompId = compId;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ManageSynergyWebsite(SynergyWebsiteViewModel model)
        {
            if (model.IsNotNull())
            {
                try
                {

                    var noteTempModel = new NoteTemplateViewModel();
                    if (model.NoteId.IsNotNullAndNotEmpty())
                    {
                        noteTempModel.DataAction = DataActionEnum.Edit;
                        noteTempModel.NoteId = model.NoteId;
                    }
                    else
                    {
                        noteTempModel.DataAction = DataActionEnum.Create;
                        if (model.Name.IsNullOrEmpty())
                        {
                            return Json(new { success = false, error = "Page Name is required !" });
                        }
                    }
                    model.HtmlContent = model.HtmlContent.IsNotNullAndNotEmpty() ? model.HtmlContent.Replace("'", "^^") : null;
                    model.Style = model.Style.IsNotNullAndNotEmpty() ? model.Style.Replace("'", "^^") : null;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "SYNERGY_WEBSITE";
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    notemodel.Json = JsonConvert.SerializeObject(model);
                    notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                    notemodel.NoteSubject = model.Name.IsNotNullAndNotEmpty() ? model.Name : notemodel.NoteSubject;
                    var result = await _noteBusiness.ManageNote(notemodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, error = "Saving Failed Contact To Administor" });
                }


            }
            return Json(new { success = false, error = "Saving Failed" });
        }
        [HttpPost]
        public async Task<IActionResult> ManageBookEditor(SynergyWebsiteViewModel model)
        {
            if (model.IsNotNull())
            {
                try
                {

                    var noteTempModel = new NoteTemplateViewModel();
                    if (model.NoteId.IsNotNullAndNotEmpty())
                    {
                        noteTempModel.DataAction = DataActionEnum.Edit;
                        noteTempModel.NoteId = model.NoteId;
                    }
                    else
                    {
                        noteTempModel.DataAction = DataActionEnum.Create;
                        if (model.Name.IsNullOrEmpty())
                        {
                            return Json(new { success = false, error = "Page Name is required !" });
                        }
                    }
                    model.HtmlContent = model.HtmlContent.IsNotNullAndNotEmpty() ? model.HtmlContent.Replace("'", "^^") : null;
                    model.Style = model.Style.IsNotNullAndNotEmpty() ? model.Style.Replace("'", "^^") : null;
                    model.Html = model.Html.IsNotNullAndNotEmpty() ? model.Html.Replace("'", "^^") : null;
                    model.Css = model.Css.IsNotNullAndNotEmpty() ? model.Css.Replace("'", "^^") : null;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "SYNERGY_WEBSITE";
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    notemodel.Json = JsonConvert.SerializeObject(model);
                    notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                    notemodel.NoteSubject = model.Name.IsNotNullAndNotEmpty() ? model.Name : notemodel.NoteSubject;
                    var result = await _noteBusiness.ManageNote(notemodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, error = "Saving Failed Contact To Administor" });
                }


            }
            return Json(new { success = false, error = "Saving Failed" });
        }
        public async Task<string> GetSynergyWebsite(string id)
        {
            var data = await _noteBusiness.GetSynergyWebsite(id);
            return data.HtmlContent;
        }
        public async Task<SynergyWebsiteViewModel> GetSynergyWebsiteData(string id)
        {
            var data = await _noteBusiness.GetSynergyWebsite(id);
            data.HtmlContent = data.HtmlContent.IsNotNullAndNotEmpty() ? data.HtmlContent.Replace("^^", "'") : null;
            data.Style = data.Style.IsNotNullAndNotEmpty() ? data.Style.Replace("^^", "'") : null;
            return data;
        }
        public async Task<SynergyWebsiteViewModel> GetBookEditorData(string id)
        {
            var data = await _noteBusiness.GetSynergyWebsite(id);
            if (data.IsNotNull())
            {
                data.HtmlContent = data.HtmlContent.IsNotNullAndNotEmpty() ? data.HtmlContent.Replace("^^", "'") : null;
                data.Style = data.Style.IsNotNullAndNotEmpty() ? data.Style.Replace("^^", "'") : null;
                data.Html = data.Html.IsNotNullAndNotEmpty() ? data.Html.Replace("^^", "'") : null;
                data.Css = data.Css.IsNotNullAndNotEmpty() ? data.Css.Replace("^^", "'") : null;
                return data;
            }
            return new SynergyWebsiteViewModel();
        }
        public async Task<string> GetSynergyWebsiteStyle(string id)
        {
            var data = await _noteBusiness.GetSynergyWebsite(id);
            return data.Style;
        }
        public async Task<List<SynergyWebsiteViewModel>> GetAllSynergyWebsite()
        {
            var data = await _noteBusiness.GetAllSynergyWebsite();
            return data;
        }
        public async Task<List<SynergyWebsiteViewModel>> GetAllSynergyWebsiteNote()
        {
            var data = await _noteBusiness.GetAllSynergyWebsiteNote();
            return data;
        }
        public async Task<ActionResult> GetImages(string referenceId)
        {
            if (referenceId.IsNotNullAndNotEmpty())
            {
                var list = await _fileBusiness.GetList(x => x.ReferenceTypeId == referenceId);
                return Json(list);
            }
            return null;
        }
    }
    public class ChartListData
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}
