using CMS.UI.ViewModel;
using CMS.Common;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Business.Interface.DMS;
using Kendo.Mvc.Extensions;
using CMS.Business;
using CMS.UI.Utility;

namespace CMS.UI.Web.Areas.DMS.Controllers
{
    [Area("DMS")]

    public class DMSReportController : ApplicationController
    {
        private IDMSDocumentBusiness _dmsDocumentBusiness;
        private ITemplateCategoryBusiness _templateCategoryBusiness;
        private ITemplateBusiness _templateBusiness;
        private IUserContext _userContext;
        public DMSReportController(IDMSDocumentBusiness dmsDocumentBusiness, IUserContext userContext,
            ITemplateCategoryBusiness templateCategoryBusiness, ITemplateBusiness templateBusiness)
        {
            _dmsDocumentBusiness = dmsDocumentBusiness;
            _userContext = userContext;
            _templateCategoryBusiness = templateCategoryBusiness;
            _templateBusiness = templateBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DocumentPendingStatusReport()
        {
            return View();
        }

        public async Task<IActionResult> ReadDocumentStageData([DataSourceRequest] DataSourceRequest request, string stageStatus, string discipline, bool isOverdue)
        {
            var result = await _dmsDocumentBusiness.DocumentStageReportData(_userContext.UserId, stageStatus, discipline, isOverdue);
            return Json(result.ToDataSourceResult(request));
        }

        public async Task<IActionResult> ReadDocumentStageDataGrid(string stageStatus, string discipline, bool isOverdue)
        {
            var result = await _dmsDocumentBusiness.DocumentStageReportData(_userContext.UserId, stageStatus, discipline, isOverdue);
            return Json(result);
        }


        public async Task<List<TemplateViewModel>> GetDocumentList(string code)
        {            
            var template = await _templateBusiness.GetNoteTemplateList(null,code,null);                     

            return template;
            
        }

        public IActionResult DocumentSubmittedStatusReport()
        {
            //var model = new DocumentListViewModel { FromDate = DateTime.Now, ToDate = DateTime.Now.AddDays(1) };
            return View();
        }
        public async Task<ActionResult> ReadDocumentSubmittedData([DataSourceRequest] DataSourceRequest request, string stageStatus, string discipline, string revesion, DateTime? fromDate, DateTime? toDate)
        {
        
            var list =await _dmsDocumentBusiness.DocumentSubmittedReportData(_userContext.UserId, stageStatus, discipline, revesion, fromDate, toDate);

            var j = Json(list.ToDataSourceResult(request));
           
            return j;
        }

        public async Task<ActionResult> ReadDocumentSubmittedDataGrid(string stageStatus, string discipline, string revision, DateTime? fromDate, DateTime? toDate)
        {

            var list = await _dmsDocumentBusiness.DocumentSubmittedReportData(_userContext.UserId, stageStatus, discipline, revision, fromDate, toDate);

            var j = Json(list);

            return j;
        }

        public async Task<ActionResult> ReadDocumentReceivedData([DataSourceRequest] DataSourceRequest request, string stageStatus, string discipline, string revesion, DateTime? fromDate, DateTime? toDate)
        {
            var list = await _dmsDocumentBusiness.DocumentReceivedReportData(_userContext.UserId, stageStatus, discipline, revesion, fromDate, toDate);

            var j = Json(list.ToDataSourceResult(request));
            // j.MaxJsonLength = int.MaxValue;
            return j;
        }

        public async Task<ActionResult> ReadDocumentReceivedDataGrid(string stageStatus, string discipline, string revision, DateTime? fromDate, DateTime? toDate)
        {
            var list = await _dmsDocumentBusiness.DocumentReceivedReportData(_userContext.UserId, stageStatus, discipline, revision, fromDate, toDate);

            var j = Json(list);
            // j.MaxJsonLength = int.MaxValue;
            return j;
        }

        public IActionResult DocumentReceivedStatusReport()
        {
            //var model = new DocumentListViewModel { FromDate = DateTime.Now, ToDate = DateTime.Now.AddDays(1) };
            return View();
        }

        public IActionResult DocumentReceivedCommentsReport()
        {
            //var model = new DocumentListViewModel { FromDate = DateTime.Now, ToDate = DateTime.Now.AddDays(1) };
            return View();
        }

        public async Task<IActionResult> ReadDocumentReceivedCommentsData([DataSourceRequest] DataSourceRequest request, string templateId, string discipline, string revision, DateTime fromDate, DateTime toDate)
        {
            var result = await _dmsDocumentBusiness.DocumentReceivedCommentsReportData(_userContext.UserId, templateId, discipline, revision, fromDate, toDate);
            return Json(result.ToDataSourceResult(request));
        }

        public async Task<IActionResult> ReadDocumentReceivedCommentsDataGrid(string templateId, string discipline, string revision, DateTime fromDate, DateTime toDate)
        {
            var result = await _dmsDocumentBusiness.DocumentReceivedCommentsReportData(_userContext.UserId, templateId, discipline, revision, fromDate, toDate);
            return Json(result);
        }
    }
}
