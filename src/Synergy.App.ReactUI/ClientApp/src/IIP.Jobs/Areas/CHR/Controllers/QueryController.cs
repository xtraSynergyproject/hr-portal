﻿using Synergy.App.Api.Areas.Cms.Models;
using Synergy.App.Api.Controllers;
//using CMS.UI.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Synergy.App.Common.ApplicationConstant;

namespace Synergy.App.Api.Areas.CHR.Controllers
{
    [Route("CHR/query")]
    [ApiController]
    public class QueryController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _iConfiguration;

        public QueryController(AuthSignInManager<ApplicationIdentityUser> customUserManager
             , IServiceProvider serviceProvider, IConfiguration iConfiguration) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _iConfiguration = iConfiguration;

        }
        [HttpGet]
        [Route("GetLeaveReport/{serviceId}")]
        public async Task<IActionResult> GetLeaveReport(string serviceId)
        {
            var _hRCoreBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
            var _performanceManagementBusiness = _serviceProvider.GetService<IPerformanceManagementBusiness>();
            var _leaveBalanceSheetBusiness = _serviceProvider.GetService<ILeaveBalanceSheetBusiness>();
            try
            {
                var user = await _hRCoreBusiness.GetUserFullInfo(serviceId);
                var report = new LeaveReportingViewModel();
                if (user != null)
                {
                    report.Email = user.Email;
                    report.OwnerEmployeeNo = user.PersonNo;
                    report.HireDate = user.DateOfJoin;
                    report.Grade = user.GradeName;
                    report.JobName = user.JobName;
                    report.OwnerDepartmentName = user.DepartmentName;
                    report.OwnerLocationtName = user.LocationName;
                    report.OwnerDisplayName = user.PersonFullName;
                    report.Mobile = user.WorkPhone;
                    report.Vacation = user.AnnualLeaveEntitlement;
                    var leavedetail = await _hRCoreBusiness.GetLeaveDetail(user.UserId);
                    if (leavedetail != null)
                    {
                        var leave = leavedetail.Where(x => x.ServiceId == serviceId).FirstOrDefault();
                        if (leave != null)
                        {
                            report.LeaveType = leave.LeaveType;
                            report.LeaveDuration = leave.DurationText;
                            report.LeaveStartDate = leave.StartDate.ToDefaultDateFormat();
                            report.LeaveEndDate = leave.EndDate.ToDefaultDateFormat();
                            report.Status = leave.LeaveStatus;
                            report.RequestTime = leave.AppliedDate.ToDefaultDateTimeFormat();
                            report.TelephoneNumber = leave.TelephoneNumber;
                            report.AddressDetail = leave.AddressDetail;
                            report.OtherInformation = leave.OtherInformation;
                            var lvbal = await _leaveBalanceSheetBusiness.GetLeaveBalance(leave.AppliedDate.Value, "ANNUAL_LEAVE", user.UserId);
                            report.LeaveBalance = lvbal.ToString();
                        }
                    }

                }
                return Ok(report);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpGet]
        [Route("GetLeaveServiceStepTask/{serviceId}")]
        public async Task<IActionResult> GetLeaveServiceStepTask(string serviceId)
        {
            try
            {
                var _componentResultBusiness = _serviceProvider.GetService<IComponentResultBusiness>();
                var stepTaskList = await _componentResultBusiness.GetStepTaskList(serviceId);
                return Ok(stepTaskList);
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet]
        [Route("GetPositionHierarchyByUserLoggedInUser")]
        public async Task<ActionResult> GetPositionHierarchyByUserLoggedInUser(String userId)
        {
            var _hRCoreBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
            List<IdNameViewModel> list = new List<IdNameViewModel>();
            var model = await _hRCoreBusiness.GetPostionHierarchyParentId(userId);
            if (model != null)
            {
                var data = await _hRCoreBusiness.GetPositionHierarchyUsers(model.Id, 100);
                if (data != null && data.Count() > 0)
                {
                    list = data.Select(e => new IdNameViewModel()
                    {
                        Id = e.Id,
                        Name = e.Name
                    }).ToList();
                }
                return Ok(list);
            }
            return Ok(list);
        }



        [HttpGet]
        [Route("ReadPayrollData")]
        public async Task<ActionResult> ReadPayrollData(string userId)
        {
            await Authenticate(userId);
            var _context = _serviceProvider.GetService<IUserContext>();

            var _business = _serviceProvider.GetService<IPayrollRunBusiness>();
            var list = await _business.GetPayrollRunList();
            //var list = await _business.GetPayrollRunList(legalEntityId);

            return Ok(list);

        }

        [HttpGet]
        [Route("GetPersonListWithDetails")]
        public async Task<ActionResult> GetPersonListWithDetails(string userId)
        {
            await Authenticate(userId);
            var _context = _serviceProvider.GetService<IUserContext>();
            var _hRCoreBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
            var result = await _hRCoreBusiness.GetAssignmentDetails(null, null, _context.LegalEntityId);

            return Ok(result);
        }

        [HttpGet]
        [Route("ReadPersonDocumentRequestList")]
        public async Task<ActionResult> ReadPersonDocumentRequestList(string userId)
        {
            var _hRCoreBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
            var depenList = await _hRCoreBusiness.GetPersonRequestDocumentList(userId);

            return Ok(depenList);
        }

        [HttpGet]
        [Route("EmployeeDocument")]
        public async Task<ActionResult> EmployeeDocument(string userId)
        {
            var _templateCategoryBusiness = _serviceProvider.GetService<ITemplateCategoryBusiness>();
            var _templateBusiness = _serviceProvider.GetService<ITemplateBusiness>();
            var _noteIndexPageTemplateBusiness = _serviceProvider.GetService<INoteIndexPageTemplateBusiness>();
            await Authenticate(userId);
            var _context = _serviceProvider.GetService<IUserContext>();
            var userRole = "";
            var role = _context.UserRoleCodes;
            if (role.Contains("HR"))
            {
                userRole = "HR";
            }

            var person = new PersonProfileViewModel();
            var category = await _templateCategoryBusiness.GetSingle(x => x.Code == "PersonDocuments");
            var templatelist = await _templateBusiness.GetList(x => x.TemplateCategoryId == category.Id);
            person.NoteTableRows = new List<NoteIndexPageTemplateViewModel>();
            foreach (var template in templatelist)
            {
                var noteIndex = await _noteIndexPageTemplateBusiness.GetSingle(x => x.TemplateId == template.Id);
                if (noteIndex != null)
                {
                    noteIndex.SelectedTableRows = await _noteIndexPageTemplateBusiness.GetList<NoteIndexPageColumnViewModel, NoteIndexPageColumn>(x => x.NoteIndexPageTemplateId == noteIndex.Id);
                    noteIndex.TemplateName = template.DisplayName;
                    person.NoteTableRows.Add(noteIndex);
                }
            }

            return Ok(person);
        }


        [HttpGet]
        [Route("LoadNoteIndexPageGrid")]
        public async Task<ActionResult> LoadNoteIndexPageGrid(string indexPageTemplateId, NtsActiveUserTypeEnum ownerType, string noteStatusCode, string userId)
        {

            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            var dt = await _noteBusiness.GetNoteIndexPageGridData(null, indexPageTemplateId, ownerType, noteStatusCode, userId);

            return Ok(dt);
        }

        [HttpGet]
        [Route("ReadDependantList")]
        public async Task<ActionResult> ReadDependantList(string userId, string ntsstatus)
        {
            await Authenticate(userId);
            var _context = _serviceProvider.GetService<IUserContext>();
            var _hRCoreBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
            var depenList = await _hRCoreBusiness.GetDependentList(_context.PersonId, ntsstatus);
            return Ok(depenList);
        }

        [HttpGet]
        [Route("ReadDependantDocumentRequestList")]
        public async Task<ActionResult> ReadDependantDocumentRequestList(string userId)
        {
            await Authenticate(userId, portalName: "HR");
            var _context = _serviceProvider.GetService<IUserContext>();
            var _hRCoreBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
            var depenList = await _hRCoreBusiness.GetDependentRequestDocumentList(userId);
            return Ok(depenList);
        }

        [HttpGet]
        [Route("EmployeeProfile")]
        public async Task<ActionResult> EmployeeProfile(string userId)
        {
            await Authenticate(userId);
            var _context = _serviceProvider.GetService<IUserContext>();
            var _hRCoreBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
            // var model = new PersonProfileViewModel();
            //model.PersonId = personId;
            var person = await _hRCoreBusiness.GetEmployeeProfile(_context.PersonId);
            // model.PersonId = person.PersonId;
            return Ok(person);
        }


        [HttpPost]
        [Route("UploadAttachment")]
        public async Task<ActionResult> UploadAttachment(AttachmentSet model)
        {
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            var _fileBusiness = _serviceProvider.GetService<IFileBusiness>();
            string fileId = "";
            //var result = "";
            try
            {
                var sb = new StringBuilder();
                //result = "1";
                sb.AppendFormat("Received image set {0}: ", model.Name);
                //result = "11";

                model.Images.ForEach(i =>
                    sb.AppendFormat("Got image {0} of type {1} and size {2} bytes,", i.FileName, i.MimeType,
                        i.StringData.Length)
                    );
                //result = "111";

                //result = sb.ToString();

                var m = model.Images[0];

                fileId = Guid.NewGuid().ToString();
                string path = AppSettings.UploadPath(_iConfiguration);
                string fullpath = @"" + path + fileId + model.FileType;
                string contentType=model.Images[0].MimeType;
                //new FileExtensionContentTypeProvider().TryGetContentType(fullpath, out contentType);

                var fmodel = new FileViewModel
                {
                    Id = fileId,
                    DataAction = DataActionEnum.Create,
                    AttachmentType = AttachmentTypeEnum.File.ToString(),
                    FileName = m.FileName,
                    FileExtension = model.FileType,
                    ContentLength = 0,
                    ContentType = contentType,
                    //IsInPhysicalPath = false,
                    ContentByte = Convert.FromBase64String(m.StringData),
                    CreatedBy = model.UserId,
                    ReferenceTypeId = model.NtsId,
                    ReferenceTypeCode = model.NtsType
                };
                var result = await _fileBusiness.Create(fmodel);
                //if (model.IsNtsComments)
                //{
                //    LoadNtsComment(model.Comment, model.UserId, model.NtsId, fileId, model.NtsType);
                //}

                //}
            }
            catch (Exception e)
            {
                //result = e.ToString();
                return Ok(fileId);
            }


            return Ok(fileId);

        }

        [HttpPost]
        [Route("UploadNtsAttachment")]
        public async Task<ActionResult> UploadNtsAttachment(AttachmentSet model)
        {
            var _fileBusiness = _serviceProvider.GetService<IFileBusiness>();
            string fileId = "";
            try
            {
                var m = model.Images[0];
                var fId = Guid.NewGuid().ToString();

                var fmodel = new FileViewModel
                {
                    Id=fId,
                    ContentByte = Convert.FromBase64String(m.StringData),
                    ContentType = m.MimeType,
                    ContentLength = m.StringData.Length,
                    FileName = m.FileName,
                    ReferenceTypeId = model.NtsId,
                    ReferenceTypeCode = model.NtsType,
                    FileExtension = model.FileType
                };
                var result = await _fileBusiness.Create(fmodel);
                if (result.IsSuccess)
                {
                    fileId = result.Item.Id;
                }
            }
            catch (Exception e)
            {
                //result = e.ToString();
                return Ok(e.ToString());
            }
            return Ok(fileId);

        }
        [HttpGet]
        [Route("ViewAttachment")]
        public async Task<ActionResult> ViewAttachment(string fileId)
        {
            var _fileBusiness = _serviceProvider.GetService<IFileBusiness>();
            var detail = await _fileBusiness.GetSingleById(fileId);
            var doc = await _fileBusiness.GetFileByte(fileId);
            if (doc != null)
            {
                //return File(doc, "application/oc-stream", detail.FileName);

                return new FileStreamResult(new MemoryStream(doc), detail.ContentType);
            }
            return null;
        } 


        [HttpGet]
        [Route("DownloadAttachment")]
        public async Task<ActionResult> DownloadAttachment(string fileId)
        {
            var _fileBusiness = _serviceProvider.GetService<IFileBusiness>();
            var doc = await _fileBusiness.GetFileByte(fileId);
            var detail = await _fileBusiness.GetSingleById(fileId);
            if (doc != null)
            {
                return File(doc, "application/oc-stream", detail.FileName);
            }
            return null;
        }


        [HttpPost]
        [Route("AddUploadedFile")]
        public async Task<ActionResult> AddUploadedFile(NoteTemplateViewModel model)
        {
            var _documentBusiness = _serviceProvider.GetService<IDMSDocumentBusiness>();

            if (model.FileIds != null && model.FileIds != "")
            {
                var result = await _documentBusiness.AddUploadedFiles(model);
                if (!result.IsSuccess)
                {
                    result.Messages.ToString();
                    return Ok(new { success = false, errors = ModelState });
                }
            }
            return Ok(new { success = true, dataAction = model.DataAction.ToString(), id = model.Id });
        }
    }



}
