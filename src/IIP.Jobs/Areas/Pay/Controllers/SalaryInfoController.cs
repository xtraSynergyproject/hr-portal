using AutoMapper;
using Synergy.App.ViewModel;
using Synergy.App.Business;
using Synergy.App.Business.Interface.DMS;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Api.Areas.DMS.Models;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft;
//using Syncfusion.EJ2.FileManager.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synergy.App.Api.Areas.Pay.Controllers
{
    [Route("Pay/SalaryInfo")]
    [ApiController]
    public class SalaryInfoController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private IPayrollBatchBusiness _payrollBatchBusiness;
        private ITableMetadataBusiness _tableMetadataBusiness;
        private INoteBusiness _noteBusiness;
        private ICmsBusiness _cmsBusiness;

        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        public SalaryInfoController(AuthSignInManager<ApplicationIdentityUser> customUserManager, ICmsBusiness cmsBusiness,
        IServiceProvider serviceProvider, IPayrollBatchBusiness payrollBatchBusiness, ITableMetadataBusiness tableMetadataBusiness, INoteBusiness noteBusiness) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _payrollBatchBusiness = payrollBatchBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _noteBusiness = noteBusiness;
            _cmsBusiness = cmsBusiness;
        }


        [HttpGet]
        [Route("ReadSalaryElementInfoData")]
        public async Task<ActionResult> ReadSalaryElementInfoData(string ParentNoteId)
        {
            var model = await _payrollBatchBusiness.GetSalaryElementInfoDetails(null, ParentNoteId);//await _payrollBatchBusiness.GetSalaryInfoDetails(null);
            return Ok(model);
        }

        [HttpGet]
        [Route("ReadSalaryInfoData")]
        public async Task<ActionResult> ReadSalaryInfoData()
        {
            var model = await _cmsBusiness.GetDataListByTemplate("SalaryInfo", "");
            //var model =await _payrollBatchBusiness.GetSalaryInfoDetails(null);
            return Ok(model);
        }

        [HttpGet]
        [Route("CreateSalaryInfo")]
        public async Task<IActionResult> Create(string salaryInfoId, string layout, string personId)
        {
            var model = new SalaryInfoViewModel();
            if (salaryInfoId.IsNotNullAndNotEmpty())
            {
                var SalaryInfo = await _payrollBatchBusiness.GetSalaryInfoDetails(salaryInfoId);
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
            if (personId.IsNotNullAndNotEmpty())
            {
                model.PersonId = personId;
            }

            return Ok( model);
        }

        [HttpPost]
        [Route("ManageSalaryInfo")]
        public async Task<IActionResult> ManageSalaryInfo(SalaryInfoViewModel model)
        {
            await Authenticate(model.OwnerUserId,model.PortalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();


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
                        return Ok(new { success = true, note = result });
                    }
                    return Ok(new { success = false, error = ModelState });
                }
                else
                {
                    return Ok(new { success = false, error = "Salary Info is Already Created for this Person" });
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
                    return Ok(new { success = true, note = result });
                }
                return Ok(new { success = false, error = ModelState });
            }

        }

        [HttpPost]
        [Route("ManageSalaryElementInfo")]
        public async Task<IActionResult> ManageSalaryElementInfo(SalaryElementInfoViewModel model)
        {
            await Authenticate(model.OwnerUserId, model.PortalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

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
                            return Ok(new { success = false, error = "ElementName is Already Created for this Person" });
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
                                return Ok(new { success = true });
                            }
                            return Ok(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                        }

                    }

                    return Ok(new { success = true });

                }
                else
                {
                    var model1 = await _payrollBatchBusiness.GetSalaryElementInfoDetails(null, model.ParentNoteId, model.Id);

                    if (model1 != null)
                    {

                        model1 = model1.Where(x => x.ElementId == model.ElementId && x.Id != model.Id).ToList();
                        if (model1.Count() > 0)
                        {
                            return Ok(new { success = false, error = "ElementName is Already Created for this Person" });
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
                                return Ok(new { success = true });
                            }
                            return Ok(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                        }

                    }
                    return Ok(new { success = true });
                }

            }

            else
            {
                return Ok(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }



        }

        [HttpPost]
        [Route("DeleteSalaryElement")]
        public async Task<ActionResult> DeleteSalaryElement(string NoteId)
        {
            var result = await _payrollBatchBusiness.DeleteSalaryElement(NoteId);
            return Ok(new { success = result });
        }
    }
}
