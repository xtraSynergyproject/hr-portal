using AutoMapper;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
//using Kendo.Mvc.Extensions;
//using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.Pay.Controllers
{
    [Area("Pay")]
    public class SalaryInfoController : ApplicationController
    {
        IPayrollBatchBusiness _payrollBatchBusiness;
        ILOVBusiness _lovBusiness;
        INoteBusiness _noteBusiness;
        IUserContext _userContext;
        ITableMetadataBusiness _tableMetadataBusiness;
        IPayrollElementBusiness _payrollElementBusiness;
        private IMapper _autoMapper;
        ICmsBusiness _cmsBusiness;
        public SalaryInfoController(IPayrollBatchBusiness payrollBatchBusiness,
            INoteBusiness noteBusiness,
            IUserContext userContext
            , IMapper autoMapper
            , ILOVBusiness lovBusiness , ITableMetadataBusiness tableMetadataBusiness
            , ICmsBusiness cmsBusiness, IPayrollElementBusiness payrollElementBusiness) 
        {
            _payrollBatchBusiness = payrollBatchBusiness;
            _noteBusiness = noteBusiness;
            _userContext = userContext;
            _autoMapper = autoMapper;
            _lovBusiness = lovBusiness;
            _cmsBusiness = cmsBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _payrollElementBusiness = payrollElementBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SalaryElementInfoIndex(string salaryInfoId)
        {
            var model = new SalaryElementInfoViewModel();
            model.ParentNoteId = salaryInfoId;//salaryInfoId;
            return View(model);
        }
        public async Task<IActionResult> CreateSalaryElement(string noteId,string salaryInfoId,string salaryElementId)
        
        {
            var model = new SalaryElementInfoViewModel();
           
            if (salaryElementId.IsNotNullAndNotEmpty())
            {
               
                var SalaryInfo = await _payrollBatchBusiness.GetSalaryElementInfoDetails(noteId, salaryInfoId, salaryElementId);
                if (SalaryInfo != null)
                {
                    model = SalaryInfo.FirstOrDefault();
                }
                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
            }
            model.ParentNoteId = salaryInfoId;
            return View("ManageSalaryElementInfo", model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageSalaryElementInfo(SalaryElementInfoViewModel model)
        {
            if (ModelState.IsValid)
            {



                if (model.DataAction == DataActionEnum.Create)
                {

                    var model1 = await _payrollBatchBusiness.GetSalaryElementInfoDetails(null, model.ParentNoteId);

                    if (model1 != null)
                    {

                        model1 = model1.Where(x => x.ElementId == model.ElementId && x.Id != model.Id).ToList();
                        if (model1.Count() > 0)
                        {
                            return Json(new { success = false, error = "ElementName is Already Created for this Person" });
                        }
                        else
                        {
                            var noteTempModel = new NoteTemplateViewModel();

                            noteTempModel.DataAction = model.DataAction;
                            noteTempModel.ActiveUserId = _userContext.UserId;
                            noteTempModel.TemplateCode = "SalaryElementInfo";
                            noteTempModel.ParentNoteId = model.ParentNoteId;
                            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                            // model = _autoMapper.Map<NoteTemplateViewModel, SalaryInfoViewModel>(notemodel, model);
                            notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                            notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                            //notemodel.StartDate = DateTime.Now;
                            // var prioritylov = await _lovBusiness.GetSingle(x => x.LOVType == "NOTE_PRIORITY" && x.Code == "NOTE_PRIORITY_LOW");
                            // notemodel.NotePriorityId = prioritylov.Id;
                            var result = await _noteBusiness.ManageNote(notemodel);
                            if (result.IsSuccess)
                            {
                                return Json(new { success = true });
                            }
                            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                        }

                    }

                    return Json(new { success = true });

                }
                else
                {
                    var model1 = await _payrollBatchBusiness.GetSalaryElementInfoDetails(null, model.ParentNoteId,model.Id);

                    if (model1 != null)
                    {

                        model1 = model1.Where(x => x.ElementId == model.ElementId && x.Id != model.Id).ToList();
                        if (model1.Count() > 0)
                        {
                            return Json(new { success = false, error = "ElementName is Already Created for this Person" });
                        }
                        else
                        {
                            var noteTempModel = new NoteTemplateViewModel();
                            noteTempModel.DataAction = DataActionEnum.Edit;
                            noteTempModel.ActiveUserId = _userContext.UserId;
                            noteTempModel.NoteId = model.NoteId;
                            noteTempModel.ParentNoteId = model.ParentNoteId;
                            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                            // model = _autoMapper.Map<NoteTemplateViewModel, SalaryInfoViewModel>(notemodel, model);
                            notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                            var result = await _noteBusiness.ManageNote(notemodel);
                            if (result.IsSuccess)
                            {
                                return Json(new { success = true });
                            }
                            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                        }

                    }
                    return Json(new { success = true });
                }

            }

            else
            {
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
           


        }
        public async Task<IActionResult>  Create(string salaryInfoId,string layout,string personId)
        {
            var model = new SalaryInfoViewModel();
            if (salaryInfoId.IsNotNullAndNotEmpty())
            {
                var SalaryInfo = await _payrollBatchBusiness.GetSalaryInfoDetails(salaryInfoId);
                if (SalaryInfo!=null) 
                {
                    model = SalaryInfo.FirstOrDefault();
                }
                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                model.DataAction = DataActionEnum.Create; 
            }
            if (layout.IsNotNullAndNotEmpty())
            {
                ViewBag.Layout = null;
            }
            else
            {
                ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
            }
            if (personId.IsNotNullAndNotEmpty())
            {
                model.PersonId = personId;
            }
            
            return View("ManageSalaryInfo", model);
        }
        [HttpPost]
        public async Task<IActionResult>  ManageSalaryInfo(SalaryInfoViewModel model)
        {
          
           
             if (model.DataAction == DataActionEnum.Create)
            {
                var existing = await _tableMetadataBusiness.GetTableDataByColumn("SalaryInfo", null, "PersonId", model.PersonId);
                if (existing == null)
                {
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.DataAction = model.DataAction;
                    noteTempModel.ActiveUserId = _userContext.UserId;
                    noteTempModel.TemplateCode = "SalaryInfo";
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    // model = _autoMapper.Map<NoteTemplateViewModel, SalaryInfoViewModel>(notemodel, model);
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                    //notemodel.StartDate = DateTime.Now;
                    // var prioritylov = await _lovBusiness.GetSingle(x => x.LOVType == "NOTE_PRIORITY" && x.Code == "NOTE_PRIORITY_LOW");
                    // notemodel.NotePriorityId = prioritylov.Id;
                    var result = await _noteBusiness.ManageNote(notemodel);
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true, note = result });
                    }
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
                else
                {
                    return Json(new { success = false, error ="Salary Info is Already Created for this Person" });
                }

            }
           
            else 
            {
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Edit;
                noteTempModel.ActiveUserId = _userContext.UserId;             
                noteTempModel.NoteId = model.NoteId;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                // model = _autoMapper.Map<NoteTemplateViewModel, SalaryInfoViewModel>(notemodel, model);
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                var result = await _noteBusiness.ManageNote(notemodel);
                if (result.IsSuccess)
                {
                    return Json(new { success = true, note = result });
                }
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
            
        }
        public async Task<ActionResult> ReadSalaryInfoData(/*[DataSourceRequest] DataSourceRequest request,*/ SalaryInfoViewModel search = null)
        {
            var model = await _cmsBusiness.GetDataListByTemplate("SalaryInfo", "");
            //var model =await _payrollBatchBusiness.GetSalaryInfoDetails(null);
            return Json(model);            
        }
        public async Task<ActionResult> ReadSalaryElementInfoData(string ParentNoteId)
        {
            var model =await _payrollBatchBusiness.GetSalaryElementInfoDetails(null, ParentNoteId) ;//await _payrollBatchBusiness.GetSalaryInfoDetails(null);
            return Json(model);
        }
        public async Task<ActionResult> DeleteSalaryElement(string NoteId)
        {
            var result = await _payrollBatchBusiness.DeleteSalaryElement(NoteId);
            return Json(new { success= result });
        }
    }
}
