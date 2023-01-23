using Microsoft.AspNetCore.Mvc;
using Synergy.App.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IronBarCode;
using System.Drawing;
using Synergy.App.ViewModel;
using Synergy.App.Common;
using Newtonsoft.Json;

namespace CMS.UI.Rcl.Rec.Areas.Rec.Controllers
{
    [Area("Rec")]
    public class RecruitmentMasterController : Controller
    {
        private readonly IRecruitmentTransactionBusiness _recruitmentTransactionBusiness;
        private readonly IFileBusiness _fileBusiness;
        private readonly IApplicationBusiness _applicationBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly IUserContext _userContext;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly ICmsBusiness _cmsBusiness;
        public RecruitmentMasterController(INoteBusiness noteBusiness, IUserContext userContext,
           ITableMetadataBusiness tableMetadataBusiness, IApplicationBusiness applicationBusiness, IRecruitmentTransactionBusiness recruitmentTransactionBusiness, IFileBusiness fileBusiness, ICmsBusiness cmsBusiness)
        {
            _recruitmentTransactionBusiness = recruitmentTransactionBusiness;
            _fileBusiness = fileBusiness;
            _applicationBusiness = applicationBusiness;
            _noteBusiness = noteBusiness;
            _userContext = userContext;
            _tableMetadataBusiness = tableMetadataBusiness;
            _cmsBusiness = cmsBusiness;
        }



        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<JsonResult> GetIdNameList(string type, string viewData = null)
        {
            var data = await _recruitmentTransactionBusiness.GetIdNameList(type);
            if (viewData != null)
            {
                ViewData[viewData] = data;
            }
            return Json(data);
        }
        public async Task<JsonResult> GetExamCenter()
        {
            var list = await _recruitmentTransactionBusiness.GetExamCenter();
            return Json(list);
        }

        public async Task<JsonResult> GetCountryIdNameList()
        {
            var list = await _recruitmentTransactionBusiness.GetCountryIdNameList();
            return Json(list);
        }

        public async Task<IActionResult> AdmitCard(string appid)
        {
            // appid = "ffde7c01-de9d-40f0-a906-6b928e3597b3";
            //GeneratedBarcode barcode = QRCodeWriter.CreateQrCode(appid, 500, QRCodeWriter.QrErrorCorrectionLevel.Medium);
            //var x = barcode.ToJpegBinaryData();

            var qrCodeData = appid;
            var qrCode = await _fileBusiness.GenerateBarCodeFile(qrCodeData, QRCodeDataTypeEnum.Text, QRCodeTypeEnum.QR_CODE, ReferenceTypeEnum.NTS_Service, appid);

            var existingadmitCard = await _tableMetadataBusiness.GetTableDataByColumn("ADMITCARD", null, "ApplicationId", appid);
            var model = new AdmitCardViewModel(); if (existingadmitCard != null)
            {
                ViewBag.Mode = "View";
                model.RollNo = Convert.ToString(existingadmitCard["RollNo"]);
                model.CandidateName = Convert.ToString(existingadmitCard["CandidateName"]);
                model.ExamDate = Convert.ToDateTime(existingadmitCard["ExamDate"]).Date;
                var examCenterId = Convert.ToString(existingadmitCard["ExamCenter"]);
                var examCenter = await _tableMetadataBusiness.GetTableDataByColumn("ExamCenter", null, "Id", examCenterId);
                if (examCenter != null)
                {
                    model.ExamCenter = Convert.ToString(examCenter["Name"]);
                }
                model.ApplicationId = appid;
            }
            else
            {
                ViewBag.Mode = "Create";
                var appDetil = await _recruitmentTransactionBusiness.GetApplicationDetailsById(appid);
                if (appDetil != null)
                {
                    model.RollNo = appDetil.ApplicationNo;
                    model.CandidateName = appDetil.FirstName + " " + appDetil.MiddleName + " " + appDetil.LastName;
                    // model.ExamDate = DateTime.Now;
                    model.ApplicationId = appid;
                }
            }

            ViewBag.FileId = qrCode.Item1;
            return View(model);
        }
        [HttpPost]
        public async Task<JsonResult> AdmitCard(AdmitCardViewModel model)
        {
            var noteTemp = new NoteTemplateViewModel();
            noteTemp.TemplateCode = "ADMITCARD";
            var note = await _noteBusiness.GetNoteDetails(noteTemp);

            note.OwnerUserId = _userContext.UserId;
            note.StartDate = DateTime.Now;
            note.Json = "{}";
            note.DataAction = DataActionEnum.Create;

            //var list = new List<System.Dynamic.ExpandoObject>();
            dynamic exo = new System.Dynamic.ExpandoObject();

            ((IDictionary<String, Object>)exo).Add("RollNo", model.RollNo);
            ((IDictionary<String, Object>)exo).Add("CandidateName", model.CandidateName);
            ((IDictionary<String, Object>)exo).Add("ExamCenter", model.ExamCenter);
            ((IDictionary<String, Object>)exo).Add("ExamDate", model.ExamDate);
            ((IDictionary<String, Object>)exo).Add("ApplicationId", model.ApplicationId);

            note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
            note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
            var res = await _noteBusiness.ManageNote(note);
            if (res.IsSuccess)
            {
                var examCenter = await _tableMetadataBusiness.GetTableDataByColumn("ExamCenter", null, "Id", model.ExamCenter);
                if (examCenter != null)
                {
                    model.ExamCenter = Convert.ToString(examCenter["Name"]);
                }
                return Json(new { success = true, examCenter = model.ExamCenter });
            }

            return Json(new { success = false });
        }
        public async Task<IActionResult> AppointmentLetter(string appid)
        {
            var appmodel = await _recruitmentTransactionBusiness.GetApplicationDetailsById(appid);
            var model = new AdmitCardViewModel();
            if (appmodel != null)
            {
                model.CandidateName = appmodel.FirstName + ' ' + appmodel.MiddleName + ' ' + appmodel.LastName;
                model.CompleteAddress = appmodel.PermanentAddress;
                if (appmodel.ContractStartDate.HasValue)
                {
                    model.AppointmentLetterDate = appmodel.ContractStartDate.Value;
                }

                model.StateCountry = appmodel.PermanentAddressState + ", " + appmodel.PermanentAddressCountryName;
                model.CandidateElementData = await _recruitmentTransactionBusiness.GetElementPayData(appid);

            }
            return View(model);
        }
        public async Task<IActionResult> AppointmentLetterRecruitmentElement(string appId)
        {
            var model = new RecCandidateElementInfoViewModel();
            model.ReferenceId = appId;
            if (appId.IsNotNull())
            {
                var appmodel = await _recruitmentTransactionBusiness.GetApplicationDetailsById(model.ReferenceId);

                model.ApplicantName = appmodel.FirstName + ' ' + appmodel.MiddleName + ' ' + appmodel.LastName;

                if (appmodel.JobId.IsNotNullAndNotEmpty())
                {
                    model.OfferDesigination = appmodel.JobId;
                }
                model.OfferGrade = appmodel.OfferGrade;
                model.OfferDesigination = appmodel.OfferDesigination ?? appmodel.JobId;
                model.JoiningDate = appmodel.JoiningDate;
                model.OfferSignedBy = appmodel.OfferSignedBy;
                model.AnnualLeave = appmodel.AnnualLeave;
                model.AnnualLeave = appmodel.AnnualLeave;
                model.SalaryRevision = appmodel.SalaryRevision;
                model.SalaryRevisionAmount = appmodel.SalaryRevisionAmount;
                model.SalaryOnAppointment = appmodel.SalaryOnAppointment.IsNotNullAndNotEmpty() ? Int32.Parse(appmodel.SalaryOnAppointment) : 0;
                model.AccommodationId = appmodel.AccommodationId;
                model.OrganizationId = appmodel.OrganizationId;
            }

            return View(model);
        }

        public async Task<JsonResult> ReadPayElementData(string appid)
        {
            var model = await _recruitmentTransactionBusiness.GetElementData(appid);
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageApplicationElement(RecCandidateElementInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appmodel = await _recruitmentTransactionBusiness.GetApplicationDetailsById(model.ReferenceId);
                appmodel.OfferDesigination = model.OfferDesigination;
                appmodel.OfferGrade = model.OfferGrade;
                appmodel.JoiningDate = model.JoiningDate;
                appmodel.OfferSignedBy = model.OfferSignedBy;
                appmodel.AnnualLeave = model.AnnualLeave;
                appmodel.AccommodationId = model.AccommodationId;
                appmodel.SalaryRevision = model.SalaryRevision;
                appmodel.SalaryRevisionAmount = model.SalaryRevisionAmount;
                appmodel.SalaryOnAppointment = model.SalaryOnAppointment.ToString();
                appmodel.ContractStartDate = model.ContractStartDate;
                appmodel.JoiningNotLaterThan = model.JoiningNotLaterThan;
                appmodel.IsTrainee = model.IsTrainee;
                appmodel.ServiceCompletion = model.ServiceCompletion;
                appmodel.TravelOriginAndDestination = model.TravelOriginAndDestination;
                appmodel.VehicleTransport = model.VehicleTransport;
                appmodel.IsLocalCandidate = model.IsLocalCandidate;
                appmodel.OrganizationId = model.OrganizationId;
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.NoteId = appmodel.ApplicationNoteId;
                noteTempModel.DataAction = DataActionEnum.Edit;
                noteTempModel.ActiveUserId = _userContext.UserId;

                var noteModel = await _noteBusiness.GetNoteDetails(noteTempModel);

                noteModel.Json = JsonConvert.SerializeObject(appmodel);

                var noteresult = await _noteBusiness.ManageNote(noteModel);
                //await _applicationBusiness.Edit(appmodel);

                if (model.JsonPayElement.IsNotNull())
                {
                    var JsonPayElement = JsonConvert.DeserializeObject<List<dynamic>>(model.JsonPayElement);

                    foreach (var a in JsonPayElement)
                    {
                        var jc = new RecCandidateElementInfoViewModel();
                        jc.ReferenceId = model.ReferenceId;
                        jc.Value = a["Value"];
                        jc.ElementId = a["PayId"];
                        jc.Comment = a["Comment"];
                        if (a["ElementId"] != null)
                        {
                            jc.Id = a["Id"];

                            var noteTempModel1 = new NoteTemplateViewModel();
                            noteTempModel1.NoteId = a["NoteId"];
                            noteTempModel1.DataAction = DataActionEnum.Edit;
                            noteTempModel1.ActiveUserId = _userContext.UserId;

                            var noteModel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);

                            noteModel.Json = JsonConvert.SerializeObject(jc);

                            var noteresult1 = await _noteBusiness.ManageNote(noteModel1);

                            //var res = await _recruitmentElementBusiness.Edit(jc);
                            if (noteresult1.IsSuccess)
                            {
                                ViewBag.Success = true;
                            }

                        }
                        else if (a["Value"] != null || a["Comment"] != null)
                        {
                            var noteTempModel1 = new NoteTemplateViewModel();
                            noteTempModel1.TemplateCode = "REC_PAY_ELEMENT_CANDIDATE";
                            noteTempModel1.DataAction = DataActionEnum.Create;
                            noteTempModel1.ActiveUserId = _userContext.UserId;

                            var noteModel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);

                            noteModel1.Json = JsonConvert.SerializeObject(jc);

                            var noteresult1 = await _noteBusiness.ManageNote(noteModel1);

                            //var res = await _recruitmentElementBusiness.Create(jc);  
                            if (noteresult1.IsSuccess)
                            {
                                ViewBag.Success = true;
                            }
                        }
                    }
                }
            }


            return RedirectToAction("AppointmentLetter", new { appid = model.ReferenceId });
            //return View("RecruitmentElement", model);
        }
        public IActionResult MedicalFitnessCertificate(string appId)
        {
            var model = new MedFitCertificateViewModel();
            model.ApplicationId = appId;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageMedicalFitnessCertificate(MedFitCertificateViewModel model)
        {

            var formTempModel = new FormTemplateViewModel();
            formTempModel.DataAction = DataActionEnum.Create;
            formTempModel.TemplateCode = "MEDICAL_FITNESS_CERTIFICATE";
            var formmodel = await _cmsBusiness.GetFormDetails(formTempModel);
            formmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            var result = await _cmsBusiness.ManageForm(formmodel);
            if (result.IsSuccess)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, error = result.Messages.ToHtmlError() });
        }
        public async Task<IActionResult> ViewMedicalFitnessCertificate(string appId)
        {
            //var model = new MedFitCertificateViewModel();
            //appId = model.ApplicationId;

            if (appId.IsNotNull())
            {
                // model.ApplicationId = appId;
                var mfmodel = await _recruitmentTransactionBusiness.GetMedicalFitnessData(appId);
                var appmodel = await _recruitmentTransactionBusiness.GetApplicationDetailsById(appId);
                mfmodel.FirstName = appmodel.FirstName;
                mfmodel.Age = appmodel.Age;
                mfmodel.SignatureId = appmodel.SignatureId;
                return View(mfmodel);
            }

            else
            {
                return View("ViewMedicalFitnessCertificate", new MedFitCertificateViewModel());
            }
        }
        public IActionResult PublishResult()
        {
            return View();
        }
        public async Task<IActionResult> GetResultData(string eId)
        {
            var data = await _recruitmentTransactionBusiness.GetResultData(eId);
            return Json(data);
        }
    }
}
