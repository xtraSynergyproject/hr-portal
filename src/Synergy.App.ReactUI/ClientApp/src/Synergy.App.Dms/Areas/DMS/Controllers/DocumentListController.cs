using Synergy.App.Business;
using Synergy.App.Business.Interface;
using Synergy.App.Business.Interface.DMS;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
//using Kendo.Mvc.Extensions;
////using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.DMS.Controllers
{
    [Area("DMS")]
    public class DocumentListController : ApplicationController
    {
        private readonly IUserContext _userContext;
        private readonly INoteBusiness _noteBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly IColumnMetadataBusiness _columnMetadataBusiness;
        private readonly ITemplateCategoryBusiness _templateCategoryBusiness;
        private readonly IDMSDocumentBusiness _documentBusiness;
        private readonly INoteIndexPageTemplateBusiness _noteIndexPageTemplateBusiness;
        public DocumentListController(IUserContext userContext, INoteBusiness noteBusiness, IDMSDocumentBusiness documentBusiness
           ,ITemplateBusiness templateBusiness, ITemplateCategoryBusiness templateCategoryBusiness, IColumnMetadataBusiness columnMetadataBusiness
           , INoteIndexPageTemplateBusiness noteIndexPageTemplateBusiness)
        {

            _userContext = userContext;
            _noteBusiness = noteBusiness;
            _templateBusiness = templateBusiness;
            _templateCategoryBusiness = templateCategoryBusiness;
            _documentBusiness = documentBusiness;
            _noteIndexPageTemplateBusiness = noteIndexPageTemplateBusiness;
            _columnMetadataBusiness = columnMetadataBusiness;

          }

        public IActionResult Index()
        {
           
            return View();
        }

        public IActionResult DocumentReport()
        {
            var model = new DocumentListViewModel();
            return View(model);
        }

        public async Task<JsonResult> GetDocumentTemplateIdNameListByUser()
        {
            var tempCategoryId = await _templateCategoryBusiness.GetSingle(x => x.Code == "GENERAL_DOCUMENT");
            var templatlist = await _templateBusiness.GetList(x => x.TemplateCategoryId == tempCategoryId.Id && x.Code != "GENERAL_DOCUMENT");
            return Json(templatlist);
        }

        public async Task<ActionResult> ReadDocumentData([DataSourceRequest] DataSourceRequest request, string templateId, string projectNo, string noteNo, string docDescription, int docCount = 0)
        {
           // var list = new List<DocumentListViewModel>();

              var list =await _documentBusiness.DocumentReportDataWithFilter(templateId,noteNo,projectNo,docDescription);
     
            
            //if (noteNo.IsNotNullAndNotEmpty())
            //{
            //    list = list.Where(x => x.NoteNo.ToLower().Contains(noteNo.ToLower())).ToList();
            //}

            //if (docDescription.IsNotNullAndNotEmpty())
            //{
            //    list = list.Where(x => x.NoteDescription.ToLower().Contains(docDescription.ToLower())).ToList();
            //}
            //if (projectNo.IsNotNullAndNotEmpty())
            //{
            //    list = list.Where(x => x.ProjectNo.Contains(projectNo)).ToList();
            //}
            var j = Json(list);
           // var j = Json(list.ToDataSourceResult(request));
            return j;
        }

        public async Task<ActionResult> ReadDocumentDataGrid(string templateId, string projectNo, string noteNo, string docDescription, int docCount = 0)
        {
            // var list = new List<DocumentListViewModel>();

            var list = await _documentBusiness.DocumentReportDataWithFilter(templateId, noteNo, projectNo, docDescription);

            var j = Json(list);
            return j;
        }

        public async Task<ActionResult> DocumenReportList(string templateId, string noteNo, string documentId, int docCount = 0)
        {
            var model = new DocumentListViewModel { TemplateId = templateId, NoteNo = noteNo, DocumentId = documentId };
            //var noteIndex = await _noteIndexPageTemplateBusiness.GetSingle(x => x.TemplateId == templateId);
            // model.SelectedTableRows = await _noteIndexPageTemplateBusiness.GetList<NoteIndexPageColumnViewModel, NoteIndexPageColumn>(x => x.NoteIndexPageTemplateId == noteIndex.Id);
            var coldata = new List<ColumnMetadataViewModel>();
            var template = await _templateBusiness.GetSingleById(templateId);
            var tablemetaId = "";
            if (template.UdfTableMetadataId.IsNotNullAndNotEmpty())
            {
                tablemetaId = template.UdfTableMetadataId;
            }
            else if (template.TableMetadataId.IsNotNullAndNotEmpty())
            {
                tablemetaId = template.TableMetadataId;
            }
            if (template.IsNotNull() && tablemetaId.IsNotNullAndNotEmpty())
            {
                coldata = await _columnMetadataBusiness.GetList(x=>x.IsUdfColumn==true && x.TableMetadataId==tablemetaId);
                model.SelectedTableRows = coldata.Where(x => x.IsUdfColumn==true).ToList();

            }
            //var labelResult = _business.DynamicDocumentColumnLabel(templateId);
            //if (labelResult != null)
            //{
            //    model.DocumentNameLabel = labelResult.DocumentNameLabel;
            //    model.DocumentDescriptionLabel = labelResult.DocumentDescriptionLabel;
            //    model.NtsNoLabelName = labelResult.NtsNoLabelName;
            //    model.HideDescription = labelResult.HideDescription;
            //}
            //model.UdfList = _business.DynamicDocumentColumn(templateId).ToList();
            ViewBag.DocCount = docCount;
            return View("_DocumentReportList", model);
        }
        public async Task<IActionResult> ReadDocumentReportDynamicData( string documentId, string templateId, string noteNo)
        {
           // var Skip = request.PageSize * (request.Page - 1);
            //var Take = request.PageSize;
            var list = await _documentBusiness.DocumentReportDetailDataWithFilter(templateId, noteNo);

            var j = Json(list);

            return j;
        }
        //public async Task<IActionResult> ReadDocumentReportDynamicData([DataSourceRequest] DataSourceRequest request,string documentId, string templateId, string noteNo)
        //{
        //    var Skip = request.PageSize * (request.Page - 1);
        //    var Take = request.PageSize;
        //    var list =await _documentBusiness.DocumentReportDetailDataWithFilter(templateId, noteNo);

        //    var j = Json(list.ToDataSourceResult(request));

        //    return j;
        //}

    }
}
