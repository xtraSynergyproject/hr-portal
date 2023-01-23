using CMS.Business;
using CMS.Common;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using CMS.UI.Utility;

namespace CMS.UI.Web.Areas.CHR.Controllers
{
    [Area("CHR")]
    public class HRCoreController : ApplicationController
    {

        private readonly IHRCoreBusiness _hrCoreBusiness;
        private readonly IUserContext _userContext;
        private readonly IListOfValueBusiness _ListOfValueBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IServiceBusiness _serviceBusiness;
        private IMapper _autoMapper;
        private readonly ILOVBusiness _lovBusiness;
        private readonly IHybridHierarchyBusiness _hybridHierarchyBusiness;

        public HRCoreController(IHRCoreBusiness hrCoreBusiness,IUserContext userContext, INoteBusiness noteBusiness, IListOfValueBusiness ListOfValueBusiness
            , IWebHostEnvironment webHostEnvironment, IServiceBusiness serviceBusiness, IMapper autoMapper, ILOVBusiness lovBusiness, IHybridHierarchyBusiness hybridHierarchyBusiness)
        {
            _hrCoreBusiness = hrCoreBusiness;
            _userContext = userContext;
            _noteBusiness = noteBusiness;
            _ListOfValueBusiness = ListOfValueBusiness;
            _webHostEnvironment = webHostEnvironment;
            _serviceBusiness = serviceBusiness;
            _autoMapper = autoMapper;
            _lovBusiness = lovBusiness;
            _hybridHierarchyBusiness = hybridHierarchyBusiness;
        }        
        
        public ActionResult Index()
        {            
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GeneratePosition(string JobId, string OrgId)
        {
            if(JobId.IsNotNullAndNotEmpty() && OrgId.IsNotNullAndNotEmpty())
            {
                var jobName = await _hrCoreBusiness.GetJobNameById(JobId);
                var OrgName = await _hrCoreBusiness.GetOrgNameById(OrgId);
                if (jobName.IsNotNull() && OrgName.IsNotNull())
                {
                    var Name = await _hrCoreBusiness.GenerateNextPositionName(OrgName.Name + "_" + jobName.Name + "_");
                    return Json(new { success = true, result = Name });
                }                    
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<IActionResult> GetBeneficiaryDEt(string Id)
        {
            if (Id.IsNotNullAndNotEmpty())
            {                
                {
                    var Name = await _hrCoreBusiness.GetBeneficiaryDEt(Id);
                    var AccountNo = Name.AccountNumber1;
                    var IbanNo = Name.Iban1;
                    var SwiftCode = Name.SwiftCode1;
                    var branch = Name.Branch1;
                    return Json(new { success = true, result = AccountNo, Iban=IbanNo,swiftcode=SwiftCode,branch=branch });                
                }
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<IActionResult> GetJobDesc(string Departmentid,string JobId)
        {
            if (Departmentid.IsNotNullAndNotEmpty())
            {
                {
                    var data = await _hrCoreBusiness.GetJobDescription(Departmentid,JobId);
                    var Id = data.Id;
                    
                    return Json(new { success = true, result = Id});
                }
            }
            return Json(new { success = false });
        }

        public async Task<IActionResult> ResignationTermination(string EmpId,string UserId)
        {
            var model = new ResignationTerminationViewModel();
            
            if (_userContext.UserRoleCodes.IsNotNullAndNotEmpty() && _userContext.UserRoleCodes.Contains("HR"))
            {
                ViewBag.Role = "HR";
            }else if (_userContext.UserRoleCodes.IsNotNullAndNotEmpty() &&  _userContext.UserRoleCodes.Contains("Employee"))
            {
                ViewBag.Role = "Employee";
            }          
           
            if (EmpId.IsNotNullAndNotEmpty())
            {
                string[] spl = EmpId.Split("?Userid=");
                model.Id = spl[0];
                model.UserId = spl[1];
            }
            var regter = await _hrCoreBusiness.GetResignationTerminationList(model.UserId);
            var exist = regter.Where(x => x.ServiceStatus != "Canceled" && x.ServiceStatus != "Rejected").ToList();
            
            ViewBag.ShowSeperation = exist.Count > 0 ? false:true;
            
            return View(model);
        }      

        public async Task<ActionResult> GetGridData(string Id = null,string UserId=null)
        {
            if (Id.IsNullOrEmpty())
            {
                Id = _userContext.UserId;
            }
            else { Id = UserId; }
            var model = await _hrCoreBusiness.GetResignationTerminationList(Id);

            var j = Json(model);
            return j;   
        }

        public async Task<IActionResult> ViewJobDescription(string Departmentid, string JobId)
        {
            var model = new JobDesriptionViewModel();
            model = await _hrCoreBusiness.GetHRJobDesciption(JobId);
            if (model.IsNotNull())
            {
                model.JobDescription = WebUtility.HtmlDecode(model.JobDescription);
                model.Responsibility = WebUtility.HtmlDecode(model.Responsibility);
                model.DataAction = DataActionEnum.Edit;
                if (model.Experience.IsNotNullAndNotEmpty())
                {
                    model.Experience1 = Convert.ToInt32(model.Experience);
                }
            }
            else
            {
                var model1 = new JobDesriptionViewModel();
                model1.DataAction = DataActionEnum.Create;
                model1.JobId = JobId;
                return View("EmployeeJobDescription",model1);
            }
            return View("EmployeeJobDescription",model);
            // var data = await _hrCoreBusiness.GetJobDescription(Departmentid, JobId);
            //return View(data);
        }

        /// <summary>
        /// Time Permission Reques Code 
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns></returns>
        public IActionResult TimePermissionRequest(string Userid)
        {
            var model = new TimePermisssionViewModel();
            model.UserId = Userid;

            return View(model);
        }

        public async Task<ActionResult> GetTimePermissionGridData(string UserId = null)
        {
            if (UserId.IsNullOrEmpty())
            {
                UserId = _userContext.UserId;
            }
          
            var model = await _hrCoreBusiness.GetTimePermissionDetailsList(UserId);

            var j = Json(model);
            return j;
        }

        public IActionResult signinsignout(string UserId,string Portalid)
        {
            var model = new PunchingViewModel();
            model.UserId = UserId;
           
            return View(model);
        }


        public async Task<ActionResult> JobDescription(string JObId)
        {
            var model = new JobDesriptionViewModel();
            model = await _hrCoreBusiness.GetHRJobDesciption(JObId);
            if (model.IsNotNull())
            {
                model.JobDescription= WebUtility.HtmlDecode(model.JobDescription);
                model.Responsibility = WebUtility.HtmlDecode(model.Responsibility);
                model.DataAction = DataActionEnum.Edit;
                if (model.Experience.IsNotNullAndNotEmpty())
                {   
                    model.Experience1 =Convert.ToInt32(model.Experience);
                }
            }
            else 
            {
                var model1 = new JobDesriptionViewModel();
                model1.DataAction = DataActionEnum.Create;
                model1.JobId = JObId;
                return View(model1);
            }
            return View(model);
        }

        public async Task<IActionResult> GetAlljobs()
        {
            var model =await  _hrCoreBusiness.GetAllJobs();
            return Json(model);
        }

        public async Task<IActionResult> ReadJobCriteriaData(string parentid, string jobdesc)
        {
            //var data = new List<HRJobCriteriaViewModel>();
            var model = await _hrCoreBusiness.GetJobCriteriabyParentID(parentid);
            var result = model.ToList();
            return Json(result);            
        }

        public async Task<IActionResult> ReadJobSkillData( string parentid, string jobdesc)
        {
            //var data = new List<HRJobCriteriaViewModel>();
            var model = await _hrCoreBusiness.GetJobSkillabyParentID(parentid);
            var result = model.ToList();
            return Json(result);
        }

        public async Task<IActionResult> ReadJobOtherInformationData( string parentid, string jobdesc)
        {
            //var data = new List<HRJobCriteriaViewModel>();
            var model = await _hrCoreBusiness.GetJobOthernformationParentID(parentid);
            var result = model.ToList();
            return Json(result);
        }

        public async Task<IActionResult> ReadJobOtherInformationData1([DataSourceRequest] DataSourceRequest request,string parentid, string jobdesc)
        {
            //var data = new List<HRJobCriteriaViewModel>();
            var model = await _hrCoreBusiness.GetJobOthernformationParentID(parentid);
            var result = model.ToList().ToDataSourceResult(request);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> ManageJobDescription(JobDesriptionViewModel model)
        {
            model.JobDescription = model.JobDescription.Replace("'", "");
            model.Responsibility = model.Responsibility.Replace("'", "");
            if (model.DataAction == DataActionEnum.Create)
            {
                if (model.Experience1.IsNotNull())
                {
                    model.Experience =Convert.ToString(model.Experience1);
                }
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = model.DataAction;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.TemplateCode = "JobDescription";
                //noteTempModel.ParentNoteId = model.ParentNoteId;
                //noteTempModel.NoteSubject = model.NoteSubject;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                notemodel.Json = JsonConvert.SerializeObject(model);
                notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                //notemodel.NoteSubject = model.NoteSubject;

                var result = await _noteBusiness.ManageNote(notemodel);

                if (result.IsSuccess)
                {
                    if (model.JobCriteria.IsNotNull())
                    {
                        var jobcriteria = JsonConvert.DeserializeObject<List<HRJobCriteriaViewModel>>(model.JobCriteria);

                        var jc = new HRJobCriteriaViewModel();
                        foreach (var a in jobcriteria)
                        {
                            var noteTempModelCriteria = new NoteTemplateViewModel();
                            noteTempModelCriteria.DataAction = model.DataAction;
                            noteTempModelCriteria.ActiveUserId = _userContext.UserId;
                            noteTempModelCriteria.TemplateCode = "JOB_CRITERIA";
                            //noteTempModel.ParentNoteId = model.ParentNoteId;
                            //noteTempModel.NoteSubject = model.NoteSubject;
                            var notemodelCriteria = await _noteBusiness.GetNoteDetails(noteTempModelCriteria);

                            notemodelCriteria.Json = JsonConvert.SerializeObject(a);
                            notemodelCriteria.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                            notemodelCriteria.ParentNoteId = result.Item.NoteId;
                            //notemodel.NoteSubject = model.NoteSubject;

                            var result1 = await _noteBusiness.ManageNote(notemodelCriteria);                          
                        }
                    }
                    if (model.Skills.IsNotNull())
                    {
                        var jobskills = JsonConvert.DeserializeObject<List<HRJobCriteriaViewModel>>(model.Skills);

                        var jc = new HRJobCriteriaViewModel();
                        foreach (var a in jobskills)
                        {
                            var noteTempModelCriteria = new NoteTemplateViewModel();
                            noteTempModelCriteria.DataAction = model.DataAction;
                            noteTempModelCriteria.ActiveUserId = _userContext.UserId;
                            noteTempModelCriteria.TemplateCode = "SKILLS";
                            //noteTempModel.ParentNoteId = model.ParentNoteId;
                            //noteTempModel.NoteSubject = model.NoteSubject;
                            var notemodelCriteria = await _noteBusiness.GetNoteDetails(noteTempModelCriteria);
                            jc = a;
                            notemodelCriteria.Json = JsonConvert.SerializeObject(jc);
                            notemodelCriteria.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                            notemodelCriteria.ParentNoteId = result.Item.NoteId;
                            //notemodel.NoteSubject = model.NoteSubject;

                            var result1 = await _noteBusiness.ManageNote(notemodelCriteria);
                        }
                    }
                    if (model.OtherInformation.IsNotNull())
                    {
                        var jobOtherInformation = JsonConvert.DeserializeObject<List<HRJobCriteriaViewModel>>(model.OtherInformation);

                        var jc = new HRJobCriteriaViewModel();
                        foreach (var a in jobOtherInformation)
                        {
                            var noteTempModelCriteria = new NoteTemplateViewModel();
                            noteTempModelCriteria.DataAction = model.DataAction;
                            noteTempModelCriteria.ActiveUserId = _userContext.UserId;
                            noteTempModelCriteria.TemplateCode = "OTHER_INFORMATION";
                            //noteTempModel.ParentNoteId = model.ParentNoteId;
                            //noteTempModel.NoteSubject = model.NoteSubject;
                            var notemodelCriteria = await _noteBusiness.GetNoteDetails(noteTempModelCriteria);
                            jc = a;
                            notemodelCriteria.Json = JsonConvert.SerializeObject(jc);
                            notemodelCriteria.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                            notemodelCriteria.ParentNoteId = result.Item.NoteId;
                            //notemodel.NoteSubject = model.NoteSubject;

                            var result1 = await _noteBusiness.ManageNote(notemodelCriteria);
                        }
                    }
                    return Json(new { success = true });
                }
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
            else
            {
                // var exist = await _pmtBusiness.IsDocNameExist(model.Name, model.Id);
                // if (exist != null)
                // {
                //     return Json(new { success = false, error = "The given name already exist" });
                // }

                if (model.Experience1.IsNotNull())
                {
                    model.Experience = Convert.ToString(model.Experience1);
                }
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.DataAction = DataActionEnum.Edit;
                //noteTempModel.ParentNoteId = model.ParentNoteId;
                noteTempModel.ActiveUserId = _userContext.UserId;
                noteTempModel.NoteId = model.NoteId;
                //noteTempModel.NoteSubject = model.NoteSubject;

                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                notemodel.Json = JsonConvert.SerializeObject(model);
                //notemodel.NoteSubject = model.NoteSubject;
                var result = await _noteBusiness.ManageNote(notemodel);
                if (result.IsSuccess)
                {
                    if (model.JobCriteria.IsNotNull())
                    {
                        var jobcriteria = JsonConvert.DeserializeObject<List<HRJobCriteriaViewModel>>(model.JobCriteria);
                        List<string> list = new List<string>();

                        foreach (var item in jobcriteria)
                        {
                            if (item.Id.IsNotNull())
                            {
                                list.Add(item.Id);
                            }
                        }

                        _hrCoreBusiness.DeleteCriteria(result.Item.NoteId, list);

                        var jc = new HRJobCriteriaViewModel();
                        foreach (var a in jobcriteria)
                        {
                            var noteTempModelCriteria = new NoteTemplateViewModel();
                            if (a.NoteId.IsNullOrEmpty())
                            {
                                noteTempModelCriteria.DataAction = DataActionEnum.Create;
                                noteTempModelCriteria.ActiveUserId = _userContext.UserId;
                                noteTempModelCriteria.TemplateCode = "JOB_CRITERIA";
                                //noteTempModel.ParentNoteId = model.ParentNoteId;
                                //noteTempModel.NoteSubject = model.NoteSubject;
                                var notemodelCriteria = await _noteBusiness.GetNoteDetails(noteTempModelCriteria);

                                notemodelCriteria.Json = JsonConvert.SerializeObject(a);
                                notemodelCriteria.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                                notemodelCriteria.ParentNoteId = result.Item.NoteId;
                                //notemodel.NoteSubject = model.NoteSubject;

                                var result1 = await _noteBusiness.ManageNote(notemodelCriteria);
                            }
                            else
                            {
                                noteTempModelCriteria.DataAction = DataActionEnum.Edit;
                                noteTempModelCriteria.ActiveUserId = _userContext.UserId;
                                noteTempModelCriteria.NoteId = a.NoteId;
                                //noteTempModel.ParentNoteId = model.ParentNoteId;
                                //noteTempModel.NoteSubject = model.NoteSubject;
                                var notemodelCriteria = await _noteBusiness.GetNoteDetails(noteTempModelCriteria);

                                notemodelCriteria.Json = JsonConvert.SerializeObject(a);
                                //notemodelCriteria.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                                notemodelCriteria.ParentNoteId = result.Item.NoteId;
                                //notemodel.NoteSubject = model.NoteSubject;

                                var result1 = await _noteBusiness.ManageNote(notemodelCriteria);
                            }
                        }
                    }
                    else { _hrCoreBusiness.DeleteCriteria(result.Item.NoteId, null); }
                    if (model.Skills.IsNotNull())
                    {
                        var jobskills = JsonConvert.DeserializeObject<List<HRJobCriteriaViewModel>>(model.Skills);

                        List<string> list = new List<string>();

                        foreach (var item in jobskills)
                        {
                            if (item.Id.IsNotNull())
                            {
                                list.Add(item.Id);
                            }
                        }

                        _hrCoreBusiness.DeleteSkill(result.Item.NoteId, list);

                        var jc = new HRJobCriteriaViewModel();
                        foreach (var a in jobskills)
                        {
                            var noteTempModelCriteria = new NoteTemplateViewModel();

                            if (a.NoteId.IsNullOrEmpty())
                            {
                                noteTempModelCriteria.DataAction = DataActionEnum.Create;
                                noteTempModelCriteria.ActiveUserId = _userContext.UserId;
                                noteTempModelCriteria.TemplateCode = "SKILLS";
                                //noteTempModel.ParentNoteId = model.ParentNoteId;
                                //noteTempModel.NoteSubject = model.NoteSubject;
                                var notemodelCriteria = await _noteBusiness.GetNoteDetails(noteTempModelCriteria);
                                jc = a;
                                notemodelCriteria.Json = JsonConvert.SerializeObject(jc);
                                notemodelCriteria.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                                notemodelCriteria.ParentNoteId = result.Item.NoteId;
                                //notemodel.NoteSubject = model.NoteSubject;

                                var result1 = await _noteBusiness.ManageNote(notemodelCriteria);
                            }
                            else
                            {
                                noteTempModelCriteria.DataAction = DataActionEnum.Edit;
                                noteTempModelCriteria.ActiveUserId = _userContext.UserId;
                                noteTempModelCriteria.NoteId = a.NoteId;
                                //noteTempModel.ParentNoteId = model.ParentNoteId;
                                //noteTempModel.NoteSubject = model.NoteSubject;
                                var notemodelCriteria = await _noteBusiness.GetNoteDetails(noteTempModelCriteria);
                                jc = a;
                                notemodelCriteria.Json = JsonConvert.SerializeObject(jc);
                                //notemodelCriteria.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                                notemodelCriteria.ParentNoteId = result.Item.NoteId;
                                //notemodel.NoteSubject = model.NoteSubject;

                                var result1 = await _noteBusiness.ManageNote(notemodelCriteria);
                            }
                        }
                    }
                    else { _hrCoreBusiness.DeleteSkill(result.Item.NoteId, null); }
                    if (model.OtherInformation.IsNotNull())
                    {
                        var jobOtherInformation = JsonConvert.DeserializeObject<List<HRJobCriteriaViewModel>>(model.OtherInformation);

                        List<string> list = new List<string>();

                        foreach (var item in jobOtherInformation)
                        {
                            if (item.Id.IsNotNull())
                            {
                                list.Add(item.Id);
                            }
                        }

                        _hrCoreBusiness.DeleteOtherInformation(result.Item.NoteId, list);

                        var jc = new HRJobCriteriaViewModel();
                        foreach (var a in jobOtherInformation)
                        {
                            var noteTempModelCriteria = new NoteTemplateViewModel();

                            if (a.NoteId.IsNullOrEmpty())
                            {
                                noteTempModelCriteria.DataAction = DataActionEnum.Create;
                                noteTempModelCriteria.ActiveUserId = _userContext.UserId;
                                noteTempModelCriteria.TemplateCode = "OTHER_INFORMATION";
                                //noteTempModel.ParentNoteId = model.ParentNoteId;
                                //noteTempModel.NoteSubject = model.NoteSubject;
                                var notemodelCriteria = await _noteBusiness.GetNoteDetails(noteTempModelCriteria);
                                jc = a;
                                notemodelCriteria.Json = JsonConvert.SerializeObject(jc);
                                notemodelCriteria.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                                notemodelCriteria.ParentNoteId = result.Item.NoteId;
                                //notemodel.NoteSubject = model.NoteSubject;

                                var result1 = await _noteBusiness.ManageNote(notemodelCriteria);
                            }
                            else
                            {
                                noteTempModelCriteria.DataAction = DataActionEnum.Edit;
                                noteTempModelCriteria.ActiveUserId = _userContext.UserId;
                                noteTempModelCriteria.NoteId = a.NoteId;
                                //noteTempModel.ParentNoteId = model.ParentNoteId;
                                //noteTempModel.NoteSubject = model.NoteSubject;
                                var notemodelCriteria = await _noteBusiness.GetNoteDetails(noteTempModelCriteria);
                                jc = a;
                                notemodelCriteria.Json = JsonConvert.SerializeObject(jc);
                                //notemodelCriteria.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                                notemodelCriteria.ParentNoteId = result.Item.NoteId;
                                //notemodel.NoteSubject = model.NoteSubject;

                                var result1 = await _noteBusiness.ManageNote(notemodelCriteria);
                            }
                        }
                    }
                    else { _hrCoreBusiness.DeleteOtherInformation(result.Item.NoteId, null); }

                    return Json(new { success = true });
                }
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetOtherCriteriaListOfValue(ReferenceTypeEnum type, string jobid, string ddlvalue, string viewData = null)
        {

            var data = new List<ListOfValueViewModel>();
            //if(ddlvalue=="List of Value")
            //{
            data = await _ListOfValueBusiness.GetList(x => x.ReferenceTypeCode == type && x.ReferenceTypeId == jobid && x.Status != StatusEnum.Inactive && x.ListOfValueType == "LOV_TYPE");

            if (viewData != null)
            {
                ViewData[viewData] = data.OrderBy(x => x.SequenceOrder);
            }
            // return Json(data.OrderBy(x => x.SequenceOrder));
            //}
            return Json(data.OrderBy(x => x.SequenceOrder));
        }
        public async Task<IActionResult> GetAllOrganisation()
        {
            var model = await _hrCoreBusiness.GetAllOrganisation();
            return Json(model);
        }

        [HttpPost]
        public async Task<JsonResult> AddSignSignout(SigninSignoutTypeEnum Punching, string UserID, string locationId)
        {
            PunchingTypeEnum punchingType;
            if (Punching == SigninSignoutTypeEnum.Checkin)
            {
                punchingType = PunchingTypeEnum.Checkin;
            }
            else
            {
                punchingType = PunchingTypeEnum.Checkout;
            }
            var result = await _hrCoreBusiness.UpdateAccessLogDetail(UserID, DateTime.Now, punchingType, DeviceTypeEnum.RemoteLogin, locationId);
            //var exist = await _hrCoreBusiness.GetAccessLogData(UserID, punchingType);
            //if (exist == null)
            //{
            //    var result = await _hrCoreBusiness.UpdateAccessLogDetail(UserID, DateTime.Now, punchingType, DeviceTypeEnum.RemoteLogin, locationId);
            //}
            //else
            //{
            //    return Json(new { success=false, message = "Already Punched In" });
            //}

            //var model = new PunchingViewModel();
            //var checkmdel = await _hrCoreBusiness.GetPunchingDetails(Punching, UserID, DateTime.Now, "");

            //if (checkmdel.IsNotNull())
            //{
            //    _hrCoreBusiness.Updatesigninsingnout(Punching, checkmdel.Id, DateTime.Now.ToString("HH:mm:ss"));
            //}
            //else
            //{
            //    model.AttendanceDate = DateTime.Now.ToString();
            //    if (SigninSignoutTypeEnum.Checkin == Punching)
            //    {
            //        model.Duty1StartTime = DateTime.Now.ToString("HH:mm:ss");
            //    }
            //    else { model.Duty1EndTime = DateTime.Now.ToString("HH:mm:ss"); }
            //    model.UserId = UserID;
            //    var noteTempModel = new NoteTemplateViewModel();
            //    noteTempModel.DataAction = DataActionEnum.Create;
            //    noteTempModel.ActiveUserId = _userContext.UserId;
            //    noteTempModel.TemplateCode = "Attendance";
            //    //noteTempModel.ParentNoteId = model.ParentNoteId;
            //    //noteTempModel.NoteSubject = model.NoteSubject;
            //    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

            //    notemodel.Json = JsonConvert.SerializeObject(model);
            //    notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
            //    //notemodel.NoteSubject = model.NoteSubject;

            //    var result = await _noteBusiness.ManageNote(notemodel);
            //}
            return Json(new { success = true});
        }

        public async Task<ActionResult> TeamAttendance(string userId)
        {
            var model = new AttendanceViewModel();
            model.SearchFromDate = DateTime.Today.AddDays(-1);
            model.SearchToDate = DateTime.Today;
            model.EmployeeStatus = EmployeeStatusEnum.Active;
            if (userId.IsNotNull())
            {
                model.UserId = userId;
            }
            else
            {
                model.UserId = _userContext.UserId;
            }

            var position = await _hrCoreBusiness.GetPositionID(model.UserId);
            
            ViewBag.PosId = position.IsNotNull()? position.Id:"0" ;
            
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetReporteePersonList(string userid)
        {
            var model =await _hrCoreBusiness.GetAllHierarchyUsers(userid,_userContext.UserId);
            return Json(model);
        }

        public async Task<ActionResult> ReadTeamAttendanceDetailsByDateData([DataSourceRequest] DataSourceRequest request, string personId, DateTime? searchFromDate, DateTime? searchToDate,string UserId)
        {
            var list =await _hrCoreBusiness.GetAttendanceListByDate( personId, searchFromDate, searchToDate,UserId);
            var result = list.ToList().ToDataSourceResult(request);
            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetLocationByid (string PersonId)
        {
            var data = await _hrCoreBusiness.GetPersonLocationId(PersonId);
            return Json(data);
        }

        [HttpGet]
        public async Task<JsonResult> GetAssignmentDetails(string personId)
        {
            var data = await _hrCoreBusiness.GetAssignmentByPerson(personId);
            return Json(new {success=true, assdata=data});
        }
        //[HttpPost]
        //public async Task<ActionResult> ImportHrBusinessData(IList<IFormFile> files1)
        //{
        //    if (files1 != null)
        //    {
        //        var fileName = "";
        //        var physicalPath = "";
        //        foreach (var file in files1)
        //        {
        //            fileName = Path.GetFileName(file.FileName);
        //            string contentRootPath = _webHostEnvironment.ContentRootPath;

        //            physicalPath = Path.Combine(contentRootPath, "wwwroot", fileName);
        //            using (FileStream stream = new FileStream(physicalPath, FileMode.Create))
        //            {
        //                file.CopyTo(stream);
        //            }
        //        }
              
        //        var result = await ManageQuestionnaireExcel(physicalPath);
        //        if (System.IO.File.Exists(physicalPath))
        //        {
        //            System.IO.File.Delete(physicalPath);
        //        }

        //        if (result.Count == 0)
        //        {
        //            //return Json(new { success = true, operation = DataOperation.Create.ToString() });
        //            return Json(new { success = true, errors = result });
        //        }
        //        else
        //        {
        //            return Json(new { success = false, errors = result });
        //        }

        //    }

        //    return Content("");
        //}
        [HttpGet]
        public async Task<JsonResult> GetPersonDetails(string personId)
        {
            var data = await _hrCoreBusiness.GetPersonDetailsByPersonId(personId);
            return Json(new { success = true, perdata = data });
        }

        public ActionResult DataUploadIndex(DataUploadViewModel model)
        {
            var dataUploadViewModel = new DataUploadViewModel()
            {
                UploadTypeId = model.UploadTypeId,
            };
            return View(dataUploadViewModel);
        }
        public async Task<IActionResult> ReadUploadData()
        {
            var model = await _hrCoreBusiness.GetUploadData();
            return Json(model);
        }
        public async Task<IActionResult> CreateUploadData(string id, DataActionEnum dataAction)
        {
            var model = new DataUploadViewModel();
                 var templateModel = new ServiceTemplateViewModel();
                templateModel.ActiveUserId = _userContext.UserId;
                templateModel.DataAction = dataAction;
                templateModel.TemplateCode = "DATA_UPLOAD";
                var newmodel = await _serviceBusiness.GetServiceDetails(templateModel);
            model.CompanyId = newmodel.CompanyId;
            model.DataAction = DataActionEnum.Create;
            model.DataUploadId= id;
              return View(model);
        }
        public async Task<IActionResult> Error(string Id)
        {
            var model = await _hrCoreBusiness.GetUploadData();
            List<DataUploadViewModel> data = model.ToList();
            var x = data.Where(x => x.DataUploadId == Id).FirstOrDefault();
            ViewBag.Err = x.Error;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ManageUploadData(DataUploadViewModel model)
        {
            //var exist = await _hrCoreBusiness.ValidateTemplate(model);
            //if (!exist.IsSuccess)
            //{
            //    return Json(new { success = false, error = exist.HtmlError });
            //}
            //else
            //{
                var serviceTempModel = new ServiceTemplateViewModel();
                serviceTempModel.DataAction = DataActionEnum.Create;
                serviceTempModel.ActiveUserId = _userContext.UserId;
                serviceTempModel.TemplateCode = "DATA_UPLOAD";
                serviceTempModel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                //serviceTempModel.UdfNoteTableId = model.DataUploadId;

                var servicemodel = await _serviceBusiness.GetServiceDetails(serviceTempModel);
                servicemodel.DataAction = DataActionEnum.Create;
                servicemodel.ActiveUserId = _userContext.UserId;
                servicemodel.TemplateCode = "DATA_UPLOAD";
                servicemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";

                var dataUploadViewModel = new DataUploadViewModel()
                {
                    UploadTypeId = model.UploadTypeId,
                    AttachmentFileId = model.AttachmentFileId,
                    DataUploadId = model.DataUploadId,
                    ExecutionStatus = HrDataExecutionStatus.InProgress,
                    ExecutionStartTime = DateTime.Now,

                };

                servicemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(dataUploadViewModel);

                model = _autoMapper.Map<ServiceTemplateViewModel, DataUploadViewModel>(servicemodel, model);
                var result = await _serviceBusiness.ManageService(servicemodel);
                if (result.IsSuccess)
                {
                    var uploadtype = await _lovBusiness.GetSingleById(model.UploadTypeId);
                    if (uploadtype != null)
                    {
                        if (uploadtype.Code == "Employee_Data")
                        {
                            await _hrCoreBusiness.BulkUploadEmployeeAssignmentData(model.AttachmentFileId, result.Item.UdfNoteId);
                        }
                        else if (uploadtype.Code == "Person_Data")
                        {
                            await _hrCoreBusiness.BulkUploadEmployeeData(model.AttachmentFileId, result.Item.UdfNoteId);
                        }
                        else if (uploadtype.Code == "Assignment_Data")
                        {
                            await _hrCoreBusiness.BulkUploadAssignmentData(model.AttachmentFileId, result.Item.UdfNoteId);
                        }
                        else if (uploadtype.Code == "Job_Data")
                        {
                            await _hrCoreBusiness.BulkUploadJobData(model.AttachmentFileId, result.Item.UdfNoteId);
                        }
                        else if (uploadtype.Code == "Department_Data")
                        {
                            await _hrCoreBusiness.BulkUploadDepartmentData(model.AttachmentFileId, result.Item.UdfNoteId);
                        }
                        else if(uploadtype.Code == "BUSINESS_HIERARCHY_DATA")
                        {
                            await _hrCoreBusiness.BulkUploadBusinessHierarchyData(model.AttachmentFileId, result.Item.UdfNoteId);
                            
                        }
                    }
                    return Json(new { success = true });
                }
                return Json(new { success = false, error = ModelState.SerializeErrors() });

            //}
        }      

        public IActionResult ReimbursementRequestItem(string reimbursmentRequestId)
        {
            ViewBag.ReimbursmentRequestId = reimbursmentRequestId;
            return View();
        }
        public async Task<JsonResult> ReadReimbursementRequestItem(string reimbursmentRequestId)
        {
            var depenList = await _hrCoreBusiness.GetReimbursementRequestItemList(reimbursmentRequestId);
            var dsResult = depenList;
            return Json(dsResult);
        }

        public async Task<IActionResult> DeleteReimbursementRequestItem(string id)
        {
            var result = await _hrCoreBusiness.DeleteReimbursementRequestItem(id);
            return Json(new { success = true });
        }
        
        public async Task<IActionResult> ManageReimbursementRequestItem(string reimbursmentRequestId, string id)
        {
            var model = new ReimbursementRequestViewModel
            {
                Id = id,
                ReimbursementRequestId = reimbursmentRequestId
            };
            if (id.IsNotNullAndNotEmpty())
            {
                model = await _hrCoreBusiness.GetReimbursementRequestItemData(id);
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageReimbursementRequestItem(ReimbursementRequestViewModel model)
        {
            if(model.Id.IsNullOrEmptyOrWhiteSpace())
            {
                var res = await _hrCoreBusiness.CreateReimbursementRequestItem(model);
                return Json(new { success = true, item = res });
            }
            else
            {
                var res = await _hrCoreBusiness.UpdateReimbursementRequestItem(model);
                return Json(new { success = true, item = res });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetPositionDetails(string Id)
        {
            var data = await _hrCoreBusiness.GetPositionDetailsById(Id);
            return Json(new { success = true, posdata = data });
        }

    }
}