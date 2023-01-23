using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
using CMS.Web;
using Hangfire;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class TaskController : ApplicationController
    {
        private readonly IRecTaskBusiness _taskBusiness;
        private readonly IRecTaskTemplateBusiness _taskTemplateBusiness;
        private readonly IFileBusiness _fileBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IApplicationBusiness _applicationBusiness;
        private readonly IBatchBusiness _batchBusiness;
        private readonly IUserContext _userContext;
        private readonly IPushNotificationBusiness _notiificationBusiness;
        private readonly IConfiguration _configuration;
        private IMapper _autoMapper;
        private IManpowerRecruitmentSummaryBusiness _manpowerRecruitmentSummaryBusiness;
        private readonly IApplicationStateCommentBusiness _applicationStateCommentBusiness;
        private readonly IListOfValueBusiness _lovBusiness;
        private readonly INtsBusiness _ntsBusiness;
        private readonly IJobDescriptionBusiness _jobDescriptionBusiness;
        private readonly IMasterBusiness _MasterBusiness;
        private readonly IWebHelper _webApi;
        private readonly ITaskBusiness _tBusiness;
        //private readonly IBusinessDiagramBusiness _bus;

        public TaskController(IRecTaskBusiness taskBusiness, IFileBusiness fileBusiness, IUserContext userContext, IMapper autoMapper,
            IPushNotificationBusiness notiificationBusiness, IConfiguration configuration, IUserBusiness userBusiness, IApplicationBusiness applicationBusiness, IBatchBusiness batchBusiness
            , IManpowerRecruitmentSummaryBusiness manpowerRecruitmentSummaryBusiness
            , IApplicationStateCommentBusiness applicationStateCommentBusiness, IListOfValueBusiness lovBusiness, INtsBusiness ntsBusiness
            , IJobDescriptionBusiness jobDescriptionBusiness, IRecTaskTemplateBusiness taskTemplateBusiness, IMasterBusiness MasterBusiness
            , IWebHelper webApi, ITaskBusiness tBusiness)
        {
            _taskBusiness = taskBusiness;
            _fileBusiness = fileBusiness;
            _userContext = userContext;
            _notiificationBusiness = notiificationBusiness;
            _configuration = configuration;
            _userBusiness = userBusiness;
            _applicationBusiness = applicationBusiness;
            _autoMapper = autoMapper;
            _batchBusiness = batchBusiness;
            _manpowerRecruitmentSummaryBusiness = manpowerRecruitmentSummaryBusiness;
            _applicationStateCommentBusiness = applicationStateCommentBusiness;
            _lovBusiness = lovBusiness;
            _ntsBusiness = ntsBusiness;
            _jobDescriptionBusiness = jobDescriptionBusiness;
            _taskTemplateBusiness = taskTemplateBusiness;
            _MasterBusiness = MasterBusiness;
            _webApi = webApi;
            _tBusiness = tBusiness;
        }
        public async Task<IActionResult> Index(string taskId, string assignTo, string teamId, string templateCode1, ReferenceTypeEnum referenceTypeCode, string referenceId, bool isPopUp = false, long? versionId = null, string udf1 = null, string udf2 = null, string udfValue1 = null, string udfValue2 = null)
        {

            var model = await _taskBusiness.GetTaskDetails(new RecTaskViewModel { Id = taskId, TemplateCode = templateCode1, ActiveUserId = _userContext.UserId, ReferenceTypeCode = referenceTypeCode, ReferenceTypeId = referenceId, TaskVersionId = versionId });
            if (!taskId.IsNotNullAndNotEmpty())
            {
                model.TemplateCode = templateCode1;
                model.Id = Guid.NewGuid().ToString();
                model.DataAction = DataActionEnum.Create;
                model.ActiveUserId = _userContext.UserId;
                model.AssigneeUserId = assignTo.IsNotNullAndNotEmpty() ? assignTo : model.AssigneeUserId;
                model.AssigneeTeamId = teamId.IsNotNullAndNotEmpty() ? teamId : model.AssigneeTeamId;
                model.AssignToType = model.AssigneeUserId.IsNotNullAndNotEmpty() ? AssignToTypeEnum.User : AssignToTypeEnum.Team;
                model.OwnerUserId = _userContext.UserId;
                model.RequestedUserId = _userContext.UserId;
            }
            else
            {
                model.ActiveUserId = _userContext.UserId;
            }
            if (model.TemplateCode == "SWS_INCIDENT")
            {
                if (udf1.IsNotNull())
                {
                    model.TextValue1 = udf1;
                    model.TextBoxLink2 = model.TextBoxLink2 + "&keyword=" + udf1 + "&hideback=true";
                }

            }
            if (model.TemplateCode == "WORKER_POOL_HR" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
            {
                var service = await _taskBusiness.GetSingleById(model.ReferenceTypeId);
                var applicant = await _applicationBusiness.GetSingleById(service.ReferenceTypeId);
                if (applicant.IsNotNull())
                {
                    model.TextValue1 = applicant.SalaryOnAppointment;
                }

            }
            if (model.TemplateCode == "WORKER_SALARY_AGENCY" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
            {
                var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                var recruiter = service.Where(x => x.TemplateCode == "WORKER_POOL_HR").FirstOrDefault();
                if (recruiter.IsNotNull())
                {
                    model.TextValue1 = recruiter.TextValue1;
                }

            }
            //if (model.TemplateCode == "TICKET_ATTACH" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
            //{
            //    var service = await _taskBusiness.GetSingleById(model.ReferenceTypeId);
            //    var app = await _applicationBusiness.GetApplicationDetail(service.ReferenceTypeId);
            //    if (app.ManpowerTypeCode == "Staff")
            //    {
            //        model.IsRequiredTextBoxDisplay1 = true;
            //    }
            //    else
            //    {
            //        model.TextBoxDisplayType1 = NtsFieldType.NTS_Hidden;
            //    }
            //}
            if (model.TemplateCode == "JOBDESCRIPTION_HM")
            {
                if (taskId.IsNullOrEmpty())
                {
                    if (udf1.IsNotNull())
                    {
                        model.DropdownValue1 = udf1;
                        model.DropdownDisplayValue1 = udfValue1;
                    }
                    if (udf2.IsNotNull())
                    {
                        model.DropdownValue2 = udf2;
                        model.DropdownDisplayValue2 = udfValue2;
                    }
                }

                if (model.TextBoxLink3.IsNotNullAndNotEmpty())
                {
                    model.TextBoxLink3 = model.TextBoxLink3.Replace("{jobId}", model.DropdownValue1);
                    model.TextBoxLink3 = model.TextBoxLink3.Replace("{orgId}", model.DropdownValue2);
                    if (model.TextBoxLink3.Contains("{TaskStatus}"))
                    {
                        model.TextBoxLink3 = model.TextBoxLink3.Replace("{TaskStatus}", model.TaskStatusCode);
                    }

                }

            }
            if (model.TemplateCode == "SCHEDULE_INTERVIEW_CANDIDATE" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
            {
                var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                var recruiter = service.Where(x => x.TemplateCode == "SCHEDULE_INTERVIEW_RECRUITER").FirstOrDefault();
                if (recruiter.IsNotNull())
                {
                    model.DatePickerValue1 = recruiter.DatePickerValue3;
                }

            }
            if (model.TemplateCode == "TASK_DIRECT_HIRING")
            {
                var service = await _taskBusiness.GetSingleById(model.ReferenceTypeId);
                if (service.IsNotNull())
                {
                    model.DropdownValue1 = service.DropdownValue1;
                    model.DropdownValue2 = service.DropdownValue2;
                }



                if (model.TextBoxLink3.IsNotNullAndNotEmpty())
                {
                    //if (model.TextValue6.IsNotNullAndNotEmpty())
                    //{
                    //    model.TextBoxLink3 = model.TextBoxLink3.Replace("{candId}", model.TextValue6);
                    //}
                    //else
                    //{
                    //    model.TextBoxLink3 = model.TextBoxLink3.Replace("{candId}", "");
                    //}
                    model.TextBoxLink3 = model.TextBoxLink3.Replace("{TaskId}", model.Id);

                }

                if (model.TextBoxLink4.IsNotNullAndNotEmpty())
                {
                    model.TextBoxLink4 = model.TextBoxLink4.Replace("{JobId}", model.DropdownValue1);
                    model.TextBoxLink4 = model.TextBoxLink4.Replace("{OrgId}", model.DropdownValue2);
                    model.TextBoxLink4 = model.TextBoxLink4.Replace("{TaskId}", model.Id);

                }

            }

            if (model.TemplateCode == "CHECK_MEDICAL_REPORT_INFORM_PRO" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
            {
                var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                var data = service.Where(x => x.TemplateCode == "INFORM_CANDIDATE_FOR_MEDICAL").FirstOrDefault();
                if (data.IsNotNull())
                {
                    model.AttachmentCode1 = data.AttachmentCode1;
                    model.AttachmentValue1 = data.AttachmentValue1;
                }
            }
            if (model.TemplateCode == "FINAL_OFFER_APPROVAL_CANDIDATE" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
            {
                var service = await _taskBusiness.GetSingleById(model.ReferenceTypeId);
                var application = await _applicationBusiness.GetApplicationDetail(service.ReferenceTypeId);
                if (application.VisaCategoryCode == "VISA_TRANSFER")
                {
                    model.TextBoxDisplay10 = @"<p>Attach with this the employment contract for your review and acceptance.</p>
                        <p>&nbsp;</p>
                        <p>Please scan and send us the following for initiating the Online visa transfer process:&nbsp;</p>
                        <ul>
                        <li>Duly signed copy of the employment contract (sign on all pages)</li>
                        <li>Credit check from Qatar Credit Bureau (Location: Opposite to Holiday villa hotel in Qatar financial square)</li>
                        <li>Copy of your resignation/termination letter.</li>
                        <li>NOC from your current Company</li>
                        <li>MOFA Attested educational certificate &amp; its Arabic translation</li>
                        <li>MMUP Card (For Engineering Profession only)</li>
                        <li>Current MOL attested labour contract</li>
                        </ul>
                        ";
                }
                else if (application.VisaCategoryCode == "LOCAL_WORK_PERMIT")
                {
                    model.TextBoxDisplay10 = @"<p>Attach with this the employment contract for your review and acceptance.&nbsp;</p>
                    <p>&nbsp;</p>
                    <p>Please scan and send us the following for initiating the Online visa transfer process:&nbsp;</p>
                    <ul>
                    <li>Duly signed copy of the employment contract (sign on all pages)</li>
                    <li>Credit check from Qatar Credit Bureau (Location: Opposite to Holiday villa hotel in Qatar financial square)</li>
                    <li>Copy of your resignation/termination letter.</li>
                    <li>MOFA Attested educational certificate &amp; its Arabic translation</li>
                    <li>MMUP Card (For Engineering Profession only)</li>
                    <li>Current MOL attested labour contract</li>
                    </ul>
                        ";
                }
                else if (application.VisaCategoryCode == "BUSINESS_VISA" || application.VisaCategoryCode == "BUSINESS_VISA_MULTIPLE" || application.VisaCategoryCode == "WORK_VISA")
                {
                    model.TextBoxDisplay10 = @"<p>Please find attached employment contract for your acceptance and signature.</p>
                        <p>&nbsp;</p>
                        <p>As a part of joining formalities you will be required to sign an agreement of general terms and conditions of employment with the organization which also includes clauses related to Confidentiality &amp; No-Compete. You would be governed by the terms and conditions of employment with the organization post joining. &nbsp;Request you to please scan and return following documents for starting your visa processing:</p>
                        <p>&nbsp;</p>
                        <ul>
                        <li>Duly Signed Employment Contract (On all Pages)</li>
                        <li>Medical Report from GAMCA approved medical center/Hospital (Stating Fit to Work)&nbsp;</li>
                        </ul>
                        <ul>
                        <li>10 Passport size photographs at the time of joining</li>
                        </ul>
                        ";
                }

            }

            if (model.TemplateCode == "RECEIVE_WORK_PERMIT_INFORM_JOINING_DATE" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
            {
                var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                var data = service.Where(x => x.TemplateCode == "VERIFY_WORK_PERMIT_OBTAINED").FirstOrDefault();
                if (data.IsNotNull())
                {
                    model.DatePickerValue1 = data.DatePickerValue2;
                    model.AttachmentCode2 = data.AttachmentCode1;
                    model.AttachmentValue2 = data.AttachmentValue1;

                }
            }

            if (model.TemplateCode == "OBTAIN_BUSINESS_VISA" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
            {
                var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                var data = service.Where(x => x.TemplateCode == "INFORM_CANDIDATE_FOR_MEDICAL").FirstOrDefault();
                if (data.IsNotNull())
                {
                    model.AttachmentCode2 = data.AttachmentCode1;
                    model.AttachmentValue2 = data.AttachmentValue1;
                }
            }

            if (model.TemplateCode == "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
            {
                var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                var data = service.Where(x => x.TemplateCode == "OBTAIN_BUSINESS_VISA").FirstOrDefault();
                if (data.IsNotNull())
                {
                    model.AttachmentCode1 = data.AttachmentCode3;
                    model.AttachmentValue1 = data.AttachmentValue3;

                    model.AttachmentCode4 = data.AttachmentCode4;
                    model.AttachmentValue4 = data.AttachmentValue4;
                }
            }


            if (model.TemplateCode == "CONFIRM_TRAVELING_DATE" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
            {
                var service = await _taskBusiness.GetSingleById(model.ReferenceTypeId);
                if (service.IsNotNull())
                {
                    var previousService = await _taskBusiness.GetSingle(x => x.ReferenceTypeId == service.ReferenceTypeId && (x.TemplateCode == "BUSINESS_VISA_MEDICAL"
                    || x.TemplateCode == "WORKER_VISA_MEDICAL") &&
                    x.NtsType == NtsTypeEnum.Service);
                    if (previousService.IsNotNull())
                    {
                        var service1 = await _taskBusiness.GetStepTaskListByService(previousService.Id);
                        var data = service1.Where(x => x.TemplateCode == "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            if (!model.DatePickerValue1.IsNotNull())
                            {
                                model.DatePickerValue1 = data.DatePickerValue2;
                            }
                        }
                        var data1 = service1.Where(x => x.TemplateCode == "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE_WORKVISA").FirstOrDefault();
                        if (data1.IsNotNull())
                        {
                            if (!model.DatePickerValue1.IsNotNull())
                            {
                                model.DatePickerValue1 = data1.DatePickerValue2;
                            }
                        }
                    }
                }

            }


            if (model.TemplateCode == "BOOK_TICKET_ATTACH" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
            {
                var service = await _taskBusiness.GetSingleById(model.ReferenceTypeId);
                if (service.IsNotNull())
                {
                    var previousService = await _taskBusiness.GetSingle(x => x.ReferenceTypeId == service.ReferenceTypeId && x.ReferenceTypeCode == ReferenceTypeEnum.REC_Application && (x.TemplateCode == "BUSINESS_VISA_MEDICAL"
                    || x.TemplateCode == "WORKER_VISA_MEDICAL") &&
                    x.NtsType == NtsTypeEnum.Service);
                    if (previousService.IsNotNull())
                    {
                        var service1 = await _taskBusiness.GetStepTaskListByService(previousService.Id);
                        var data = service1.Where(x => x.TemplateCode == "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            model.DatePickerValue1 = data.DatePickerValue2;
                            model.AttachmentCode4 = data.AttachmentCode1;
                            model.AttachmentValue4 = data.AttachmentValue1;
                        }
                        var data1 = service1.Where(x => x.TemplateCode == "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE_WORKVISA").FirstOrDefault();
                        if (data1.IsNotNull())
                        {
                            model.DatePickerValue1 = data1.DatePickerValue2;
                            model.AttachmentCode4 = data1.AttachmentCode1;
                            model.AttachmentValue4 = data1.AttachmentValue1;
                        }

                    }

                    var finalService = await _taskBusiness.GetSingle(x => x.ReferenceTypeId == service.ReferenceTypeId && x.ReferenceTypeCode == ReferenceTypeEnum.REC_Application && x.TemplateCode == "PREPARE_FINAL_OFFER"
                     && x.NtsType == NtsTypeEnum.Service);
                    if (finalService.IsNotNull())
                    {
                        var service1 = await _taskBusiness.GetStepTaskListByService(finalService.Id);
                        var data = service1.Where(x => x.TemplateCode == "FINAL_OFFER_APPROVAL_CANDIDATE").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            model.AttachmentCode5 = data.AttachmentCode4;
                            model.AttachmentValue5 = data.AttachmentValue4;
                        }

                    }
                }
            }


            if (model.TemplateCode == "CONFIRM_RECEIPT_TICKET_DATE_OF_TRAVEL" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
            {
                var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                var data = service.Where(x => x.TemplateCode == "TICKET_ATTACH").FirstOrDefault();
                if (data.IsNotNull())
                {
                    model.AttachmentCode1 = data.AttachmentCode1;
                    model.AttachmentValue1 = data.AttachmentValue1;

                    model.AttachmentCode5 = data.AttachmentCode3;
                    model.AttachmentValue5 = data.AttachmentValue3;

                }

                var pservice = await _taskBusiness.GetSingleById(model.ReferenceTypeId);
                if (pservice.IsNotNull())
                {
                    var previousService = await _taskBusiness.GetSingle(x => x.ReferenceTypeId == pservice.ReferenceTypeId && (x.TemplateCode == "BUSINESS_VISA_MEDICAL"
                    || x.TemplateCode == "WORKER_VISA_MEDICAL") &&
                    x.NtsType == NtsTypeEnum.Service);
                    if (previousService.IsNotNull())
                    {
                        var service1 = await _taskBusiness.GetStepTaskListByService(previousService.Id);
                        var data1 = service1.Where(x => x.TemplateCode == "OBTAIN_BUSINESS_VISA").FirstOrDefault();
                        if (data1.IsNotNull())
                        {
                            model.AttachmentCode3 = data1.AttachmentCode3;
                            model.AttachmentValue3 = data1.AttachmentValue3;

                            model.AttachmentCode4 = data1.AttachmentCode4;
                            model.AttachmentValue4 = data1.AttachmentValue4;
                        }

                        var data2 = service1.Where(x => x.TemplateCode == "FIT_UNFIT_ATTACH_VISA_COPY").FirstOrDefault();
                        if (data2.IsNotNull())
                        {
                            model.AttachmentCode3 = data2.AttachmentCode1;
                            model.AttachmentValue3 = data2.AttachmentValue1;

                            model.AttachmentCode4 = data2.AttachmentCode3;
                            model.AttachmentValue4 = data2.AttachmentValue3;
                        }
                    }
                }
            }

            if (model.TemplateCode == "ARRANGE_ACCOMMODATION" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
            {
                var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                var data = service.Where(x => x.TemplateCode == "TICKET_ATTACH").FirstOrDefault();
                if (data.IsNotNull())
                {
                    model.AttachmentCode1 = data.AttachmentCode1;
                    model.AttachmentValue1 = data.AttachmentValue1;
                }
            }

            if (model.TemplateCode == "ARRANGE_VEHICLE_PICKUP" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
            {
                var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                var data = service.Where(x => x.TemplateCode == "TICKET_ATTACH").FirstOrDefault();
                if (data.IsNotNull())
                {
                    model.AttachmentCode1 = data.AttachmentCode1;
                    model.AttachmentValue1 = data.AttachmentValue1;
                }

                var data1 = service.Where(x => x.TemplateCode == "ARRANGE_ACCOMMODATION").FirstOrDefault();
                if (data1.IsNotNull())
                {
                    model.TextValue3 = data1.TextValue3;
                }
            }

            //work visa
            if (model.TemplateCode == "BOOK_QVC_APPOINTMENT" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
            {
                var service = await _taskBusiness.GetSingleById(model.ReferenceTypeId);
                if (service.IsNotNull())
                {

                    var finalService = await _taskBusiness.GetSingle(x => x.ReferenceTypeId == service.ReferenceTypeId && x.ReferenceTypeCode == ReferenceTypeEnum.REC_Application && x.TemplateCode == "PREPARE_FINAL_OFFER"
                     && x.NtsType == NtsTypeEnum.Service);
                    if (finalService.IsNotNull())
                    {
                        var service1 = await _taskBusiness.GetStepTaskListByService(finalService.Id);
                        var data = service1.Where(x => x.TemplateCode == "FINAL_OFFER_APPROVAL_CANDIDATE").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            model.AttachmentCode4 = data.AttachmentCode4;
                            model.AttachmentValue4 = data.AttachmentValue4;
                        }
                        var data1 = service1.Where(x => x.TemplateCode == "APPLY_WORK_VISA_THROUGH_MOL").FirstOrDefault();
                        if (data1.IsNotNull())
                        {
                            model.AttachmentCode5 = data1.AttachmentCode1;
                            model.AttachmentValue5 = data1.AttachmentValue1;
                        }

                    }
                }
            }
            if (model.TemplateCode == "CONDUCT_MEDICAL_FINGER_PRINT" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
            {
                var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                var data = service.Where(x => x.TemplateCode == "BOOK_QVC_APPOINTMENT").FirstOrDefault();
                if (data.IsNotNull())
                {
                    model.DatePickerValue2 = data.DatePickerValue2;
                }
            }


            if (model.TemplateCode == "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE_WORKVISA" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
            {
                var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                var data = service.Where(x => x.TemplateCode == "FIT_UNFIT_ATTACH_VISA_COPY").FirstOrDefault();
                if (data.IsNotNull())
                {
                    model.AttachmentCode1 = data.AttachmentCode1;
                    model.AttachmentValue1 = data.AttachmentValue1;
                }
            }
            // Visa Transfer

            if (model.TemplateCode == "VERIFY_DOCUMENTS" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
            {
                var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                var data = service.Where(x => x.TemplateCode == "SUBMIT_VISA_TRANSFER_DOCUMENTS").FirstOrDefault();
                if (data.IsNotNull())
                {
                    model.AttachmentCode1 = data.AttachmentCode1;
                    model.AttachmentValue1 = data.AttachmentValue1;
                    model.AttachmentCode2 = data.AttachmentCode2;
                    model.AttachmentValue2 = data.AttachmentValue2;
                    model.AttachmentCode3 = data.AttachmentCode3;
                    model.AttachmentValue3 = data.AttachmentValue3;
                    model.AttachmentCode4 = data.AttachmentCode4;
                    model.AttachmentValue4 = data.AttachmentValue4;
                    model.AttachmentCode5 = data.AttachmentCode5;
                    model.AttachmentValue5 = data.AttachmentValue5;
                    model.AttachmentCode6 = data.AttachmentCode7;
                    model.AttachmentValue6 = data.AttachmentValue7;
                    model.AttachmentCode7 = data.AttachmentCode8;
                    model.AttachmentValue7 = data.AttachmentValue8;
                }
            }

            if (model.TemplateCode == "SUBMIT_VISA_TRANSFER_CONFIRM_SPONSORSHIP" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
            {
                var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                var data = service.Where(x => x.TemplateCode == "SUBMIT_VISA_TRANSFER_DOCUMENTS").FirstOrDefault();
                if (data.IsNotNull())
                {
                    model.AttachmentCode1 = data.AttachmentCode1;
                    model.AttachmentValue1 = data.AttachmentValue1;
                    model.AttachmentCode2 = data.AttachmentCode2;
                    model.AttachmentValue2 = data.AttachmentValue2;
                    model.AttachmentCode3 = data.AttachmentCode3;
                    model.AttachmentValue3 = data.AttachmentValue3;
                    model.AttachmentCode4 = data.AttachmentCode4;
                    model.AttachmentValue4 = data.AttachmentValue4;
                    model.AttachmentCode5 = data.AttachmentCode5;
                    model.AttachmentValue5 = data.AttachmentValue5;
                }
            }

            if (model.TemplateCode == "VERIFY_VISA_TRANSFER_COMPLETED" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
            {
                var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                var data = service.Where(x => x.TemplateCode == "SUBMIT_VISA_TRANSFER_CONFIRM_SPONSORSHIP").FirstOrDefault();
                if (data.IsNotNull())
                {
                    model.AttachmentCode1 = data.AttachmentCode6;
                    model.AttachmentValue1 = data.AttachmentValue6;
                }
            }

            if (model.TemplateCode == "RECEIVE_VISA_TRANSFER_INFORM_JOINING_DATE" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
            {
                var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                var data = service.Where(x => x.TemplateCode == "SUBMIT_VISA_TRANSFER_CONFIRM_SPONSORSHIP").FirstOrDefault();
                if (data.IsNotNull())
                {
                    model.AttachmentCode2 = data.AttachmentCode6;
                    model.AttachmentValue2 = data.AttachmentValue6;
                }
            }

            //work permit

            if (model.TemplateCode == "VERIFY_WORK_PERMIT_DOCUMENTS" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
            {
                var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                var data = service.Where(x => x.TemplateCode == "SUBMIT_WORK_PERMIT_DOCUMENTS").FirstOrDefault();
                if (data.IsNotNull())
                {
                    model.AttachmentCode1 = data.AttachmentCode1;
                    model.AttachmentValue1 = data.AttachmentValue1;
                    model.AttachmentCode2 = data.AttachmentCode2;
                    model.AttachmentValue2 = data.AttachmentValue2;
                    model.AttachmentCode3 = data.AttachmentCode3;
                    model.AttachmentValue3 = data.AttachmentValue3;
                    model.AttachmentCode4 = data.AttachmentCode4;
                    model.AttachmentValue4 = data.AttachmentValue4;
                    model.AttachmentCode9 = data.AttachmentCode9;
                    model.AttachmentValue9 = data.AttachmentValue9;
                }
            }


            if (model.TemplateCode == "OBTAIN_WORK_PERMIT_ATTACH" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
            {
                var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                var data = service.Where(x => x.TemplateCode == "SUBMIT_WORK_PERMIT_DOCUMENTS").FirstOrDefault();
                if (data.IsNotNull())
                {
                    model.AttachmentCode1 = data.AttachmentCode1;
                    model.AttachmentValue1 = data.AttachmentValue1;
                    model.AttachmentCode2 = data.AttachmentCode2;
                    model.AttachmentValue2 = data.AttachmentValue2;
                    model.AttachmentCode3 = data.AttachmentCode3;
                    model.AttachmentValue3 = data.AttachmentValue3;
                    model.AttachmentCode4 = data.AttachmentCode4;
                    model.AttachmentValue4 = data.AttachmentValue4;
                }
            }

            if (model.TemplateCode == "VERIFY_WORK_PERMIT_OBTAINED" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
            {
                var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                var data = service.Where(x => x.TemplateCode == "OBTAIN_WORK_PERMIT_ATTACH").FirstOrDefault();
                if (data.IsNotNull())
                {
                    model.AttachmentCode1 = data.AttachmentValue5;
                    model.AttachmentValue1 = data.AttachmentValue5;
                }
            }


            if (model.TemplateCode == "VERIFY_WORK_PERMIT_OBTAINED" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
            {
                var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                var data = service.Where(x => x.TemplateCode == "OBTAIN_WORK_PERMIT_ATTACH").FirstOrDefault();
                if (data.IsNotNull())
                {
                    model.AttachmentCode1 = data.AttachmentCode5;
                    model.AttachmentValue1 = data.AttachmentValue5;
                }
            }

            if (model.TemplateCode == "VERIFY_WORK_PERMIT_OBTAINED" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
            {
                var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                var data = service.Where(x => x.TemplateCode == "OBTAIN_WORK_PERMIT_ATTACH").FirstOrDefault();
                if (data.IsNotNull())
                {
                    model.AttachmentCode2 = data.AttachmentCode5;
                    model.AttachmentValue2 = data.AttachmentValue5;
                }
            }
            //if (model.TemplateCode == "STAFF_CONFIRM_PROBATION_DATE_BY_HR" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
            //{
            //    var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
            //    var data = service.Where(x => x.TemplateCode == "STAFF_CONFIRM_PROBATION_DATE_BY_HM").FirstOrDefault();
            //    if (data.IsNotNull())
            //    {
            //        model.DropdownValue1 = data.DropdownValue1;
            //        model.DropdownValue2 = data.DropdownValue2;
            //    }
            //}
            //if (model.TemplateCode == "WORKER_CONFIRM_PROBATION_DATE_BY_HR" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
            //{
            //    var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
            //    var data = service.Where(x => x.TemplateCode == "WORKER_CONFIRM_PROBATION_DATE_BY_HM").FirstOrDefault();
            //    if (data.IsNotNull())
            //    {
            //        model.DropdownValue1 = data.DropdownValue1;
            //        model.DropdownValue2 = data.DropdownValue2;
            //    }
            //}
            if (isPopUp)
            {
                ViewBag.Layout = "/Views/Shared/_PopupLayoutTask.cshtml";
            }
            var template = await _taskTemplateBusiness.GetSingle(x => x.TemplateCode == model.TemplateCode);
            if (template.IsNotNull())
            {
                model.BannerId = template.BannerId;
                model.BannerStyle = template.BannerStyle;
            }
            return View("Task", model);
        }
        public async Task<IActionResult> Service(string taskId, string assignTo, string teamId, string templateCode1, ReferenceTypeEnum referenceTypeCode, string referenceId, bool isPopUp = false, bool isSimpleCard = false, long? versionId = null)
        {

            var model = await _taskBusiness.GetTaskDetails(new RecTaskViewModel { Id = taskId, TemplateCode = templateCode1, ActiveUserId = _userContext.UserId, ReferenceTypeCode = referenceTypeCode, ReferenceTypeId = referenceId, TaskVersionId = versionId });
            if (!taskId.IsNotNullAndNotEmpty())
            {
                model.TemplateCode = templateCode1;
                model.Id = Guid.NewGuid().ToString();
                model.DataAction = DataActionEnum.Create;
                model.ActiveUserId = _userContext.UserId;
                model.AssigneeUserId = assignTo.IsNotNullAndNotEmpty() ? assignTo : model.AssigneeUserId;
                model.AssigneeTeamId = teamId.IsNotNullAndNotEmpty() ? teamId : model.AssigneeTeamId;
                model.AssignToType = model.AssigneeUserId.IsNotNullAndNotEmpty() ? AssignToTypeEnum.User : AssignToTypeEnum.Team;
                model.OwnerUserId = _userContext.UserId;
                model.RequestedUserId = _userContext.UserId;
                if (model.TemplateCode == "EMPLOYEE_APPOINTMENT" && model.ReferenceTypeId.IsNotNullAndNotEmpty())
                {
                    var applicant = await _applicationBusiness.GetCandiadteShortListApplicationDataByApplication(model.ReferenceTypeId, null);
                    if (applicant.IsNotNull())
                    {
                        model.TextValue1 = applicant.FullName;
                        model.TextValue2 = applicant.InterviewByUserName;
                        model.TextValue3 = "";
                        model.TextValue4 = "";
                        model.TextValue5 = "";
                        model.TextValue6 = applicant.SalaryOnAppointment;
                        model.TextValue7 = "";
                        model.TextValue8 = "";
                        model.TextValue10 = "";
                        model.DropdownValue3 = applicant.JobId;
                        model.DropdownValue6 = applicant.AccommodationId;
                    }
                }
            }
            else
            {
                model.ActiveUserId = _userContext.UserId;
            }
            if (isPopUp)
            {
                ViewBag.Layout = "/Views/Shared/_PopupLayoutTask.cshtml";
            }
            ViewBag.IsSimpleCard = isSimpleCard;
            return View("Service", model);
        }

        public IActionResult AddTaskAttachment(string taskId)
        {
            var model = new FileViewModel();
            model.ReferenceTypeId = taskId;
            model.ReferenceTypeCode = ReferenceTypeEnum.NTS_Task;
            return View("_Attachment", model);
        }
        [HttpPost]
        public async Task<ActionResult> Manage(RecTaskViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (model.DataAction == DataActionEnum.Create)
                    {
                        if (model.TemplateAction == NtsActionEnum.Submit)
                        {
                            model.TaskStatusName = "In Progress";
                            model.TaskStatusCode = "INPROGRESS";
                            model.SubmittedDate = System.DateTime.Now;
                            model.StartDate = model.StartDate >= DateTime.Now ? model.StartDate : DateTime.Now;
                            model.DueDate = model.StartDate.Value.AddDays(Convert.ToDouble(model.SLA));
                        }
                        else if (model.TemplateAction == NtsActionEnum.Draft)
                        {
                            model.TaskStatusName = "Draft";
                            model.TaskStatusCode = "DRAFT";
                        }
                        var result = await PreScriptTask(model);
                        if (result != "success")
                        {
                            return Json(new { success = false, error = result });
                        }
                        var Taskresult = await _taskBusiness.Create(model);
                        if (Taskresult.IsSuccess)
                        {
                            await PostScriptTask(model);
                            return Json(new { success = true, Id = Taskresult.Item.Id });
                        }

                        else
                        {
                            ModelState.AddModelErrors(Taskresult.Messages);
                            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                        }
                    }
                    else if (model.DataAction == DataActionEnum.Edit)
                    {

                        if (model.TemplateAction == NtsActionEnum.Submit)
                        {
                            model.TaskStatusName = "In Progress";
                            model.TaskStatusCode = "INPROGRESS";
                            model.SubmittedDate = System.DateTime.Now;
                            model.StartDate = model.StartDate >= DateTime.Now ? model.StartDate : DateTime.Now;
                            model.DueDate = model.StartDate.Value.AddDays(Convert.ToDouble(model.SLA));
                        }
                        else if (model.TemplateAction == NtsActionEnum.Complete)
                        {
                            model.TaskStatusName = "Completed";
                            model.TaskStatusCode = "COMPLETED";
                            model.CompletionDate = System.DateTime.Now;
                        }
                        else if (model.TemplateAction == NtsActionEnum.Draft)
                        {
                            model.TaskStatusName = "Draft";
                            model.TaskStatusCode = "DRAFT";
                        }
                        else if (model.TemplateAction == NtsActionEnum.SaveChanges)
                        {
                            model.TaskStatusName = "In Progress";
                            model.TaskStatusCode = "INPROGRESS";
                        }
                        else if (model.TemplateAction == NtsActionEnum.Reject)
                        {
                            model.TaskStatusName = "Rejected";
                            model.TaskStatusCode = "REJECTED";
                            model.RejectedDate = System.DateTime.Now;
                        }
                        else if (model.TemplateAction == NtsActionEnum.NotApplicable)
                        {
                            model.TaskStatusName = "Not Applicable";
                            model.TaskStatusCode = "NOTAPPLICABLE";
                        }
                        else if (model.TemplateAction == NtsActionEnum.Cancel)
                        {
                            model.TaskStatusName = "Cancelled";
                            model.TaskStatusCode = "CANCELLED";
                            model.CanceledDate = System.DateTime.Now;
                        }
                        else if (model.TemplateAction == NtsActionEnum.Close)
                        {
                            model.TaskStatusName = "Closed";
                            model.TaskStatusCode = "CLOSED";
                            model.ClosedDate = System.DateTime.Now;
                        }
                        else if (model.TemplateAction == NtsActionEnum.EditAsNewVersion)
                        {
                            model.TemplateAction = NtsActionEnum.Submit;
                            model.TaskStatusName = "In Progress";
                            model.TaskStatusCode = "INPROGRESS";
                            //model.VersionNo = model.VersionNo + 1;
                        }
                        else
                        {

                        }
                        var result = await PreScriptTask(model);
                        if (result != "success")
                        {
                            return Json(new { success = false, error = result });
                        }
                        var Taskresult = await _taskBusiness.Edit(model);
                        if (Taskresult.IsSuccess)
                        {
                            await PostScriptTask(model);
                            return Json(new { success = true, Id = Taskresult.Item.Id });
                        }
                        else
                        {
                            ModelState.AddModelErrors(Taskresult.Messages);
                            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                        }

                    }

                }
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, error = ex.Message });
            }


        }
        [HttpPost]
        public async Task<ActionResult> Manage1([FromBody] RecTaskViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.TemplateAction = model.GridTemplateAction;
                    if (model.DataAction == DataActionEnum.Create)
                    {
                        if (model.TemplateAction == NtsActionEnum.Submit)
                        {
                            model.TaskStatusName = "In Progress";
                            model.TaskStatusCode = "INPROGRESS";
                            model.SubmittedDate = System.DateTime.Now;
                            model.StartDate = model.StartDate >= DateTime.Now ? model.StartDate : DateTime.Now;
                            model.DueDate = model.StartDate.Value.AddDays(Convert.ToDouble(model.SLA));
                        }
                        else if (model.TemplateAction == NtsActionEnum.Draft)
                        {
                            model.TaskStatusName = "Draft";
                            model.TaskStatusCode = "DRAFT";
                        }
                        var result = await PreScriptTask(model);
                        if (result != "success")
                        {
                            return Json(new { success = false, error = result });
                        }
                        var Taskresult = await _taskBusiness.Create(model);
                        if (Taskresult.IsSuccess)
                        {
                            await PostScriptTask(model);
                            return Json(new { success = true, Id = Taskresult.Item.Id });
                        }

                        else
                        {
                            ModelState.AddModelErrors(Taskresult.Messages);
                            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                        }
                    }
                    else if (model.DataAction == DataActionEnum.Edit)
                    {

                        if (model.TemplateAction == NtsActionEnum.Submit)
                        {
                            model.TaskStatusName = "In Progress";
                            model.TaskStatusCode = "INPROGRESS";
                            model.SubmittedDate = System.DateTime.Now;
                            model.StartDate = model.StartDate >= DateTime.Now ? model.StartDate : DateTime.Now;
                            model.DueDate = model.StartDate.Value.AddDays(Convert.ToDouble(model.SLA));
                        }
                        else if (model.TemplateAction == NtsActionEnum.Complete)
                        {
                            model.TaskStatusName = "Completed";
                            model.TaskStatusCode = "COMPLETED";
                            model.CompletionDate = System.DateTime.Now;
                        }
                        else if (model.TemplateAction == NtsActionEnum.Draft)
                        {
                            model.TaskStatusName = "Draft";
                            model.TaskStatusCode = "DRAFT";
                        }
                        else if (model.TemplateAction == NtsActionEnum.Reject)
                        {
                            model.TaskStatusName = "Rejected";
                            model.TaskStatusCode = "REJECTED";
                            model.RejectedDate = System.DateTime.Now;
                        }
                        else if (model.TemplateAction == NtsActionEnum.NotApplicable)
                        {
                            model.TaskStatusName = "Not Applicable";
                            model.TaskStatusCode = "NOTAPPLICABLE";
                        }
                        else if (model.TemplateAction == NtsActionEnum.Cancel)
                        {
                            model.TaskStatusName = "Cancelled";
                            model.TaskStatusCode = "CANCELLED";
                            model.CanceledDate = System.DateTime.Now;
                        }
                        else if (model.TemplateAction == NtsActionEnum.Close)
                        {
                            model.TaskStatusName = "Closed";
                            model.TaskStatusCode = "CLOSED";
                            model.ClosedDate = System.DateTime.Now;
                        }
                        else if (model.TemplateAction == NtsActionEnum.EditAsNewVersion)
                        {
                            model.TemplateAction = NtsActionEnum.Submit;
                            model.TaskStatusName = "In Progress";
                            model.TaskStatusCode = "INPROGRESS";
                            //model.VersionNo = model.VersionNo + 1;
                        }
                        else
                        {

                        }
                        var result = await PreScriptTask(model);
                        if (result != "success")
                        {
                            var msg = new Dictionary<string, string>();
                            msg.Add("error", result);
                            ModelState.AddModelErrors(msg);
                            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                        }
                        var Taskresult = await _taskBusiness.Edit(model);
                        if (Taskresult.IsSuccess)
                        {
                            await PostScriptTask(model);
                            return Json(new { success = true, Id = Taskresult.Item.Id });
                        }
                        else
                        {
                            ModelState.AddModelErrors(Taskresult.Messages);
                            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                        }

                    }

                }
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, error = ex.Message });
            }


        }
        public async Task<string> PreScriptTask(RecTaskViewModel model)
        {
            //if (model.TemplateAction == NtsActionEnum.Complete && model.IsRequiredTextBoxDisplay3 && !model.DatePickerValue3.IsNotNull())
            //{
            //    return model.TextDisplay3 + " is mandatory !";
            //}
            if (model.NtsType == NtsTypeEnum.Service && model.TemplateAction == NtsActionEnum.Submit)
            {
                if (model.TextBoxDisplayType1 == NtsFieldType.NTS_Attachment && model.IsRequiredTextBoxDisplay1 && model.AttachmentValue1.IsNullOrEmpty())
                {
                    return model.TextBoxDisplay1 + " is mandatory !";
                }
                if (model.TextBoxDisplayType2 == NtsFieldType.NTS_Attachment && model.IsRequiredTextBoxDisplay2 && model.AttachmentValue2.IsNullOrEmpty())
                {
                    return model.TextBoxDisplay2 + " is mandatory !";
                }
                if (model.TextBoxDisplayType3 == NtsFieldType.NTS_Attachment && model.IsRequiredTextBoxDisplay3 && model.AttachmentValue3.IsNullOrEmpty())
                {
                    return model.TextBoxDisplay3 + " is mandatory !";
                }
                if (model.TextBoxDisplayType4 == NtsFieldType.NTS_Attachment && model.IsRequiredTextBoxDisplay4 && model.AttachmentValue4.IsNullOrEmpty())
                {
                    return model.TextBoxDisplay4 + " is mandatory !";
                }
                if (model.TextBoxDisplayType5 == NtsFieldType.NTS_Attachment && model.IsRequiredTextBoxDisplay5 && model.AttachmentValue5.IsNullOrEmpty())
                {
                    return model.TextBoxDisplay5 + " is mandatory !";
                }
                if (model.TextBoxDisplayType6 == NtsFieldType.NTS_Attachment && model.IsRequiredTextBoxDisplay6 && model.AttachmentValue6.IsNullOrEmpty())
                {
                    return model.TextBoxDisplay6 + " is mandatory !";
                }
                if (model.TextBoxDisplayType7 == NtsFieldType.NTS_Attachment && model.IsRequiredTextBoxDisplay7 && model.AttachmentValue7.IsNullOrEmpty())
                {
                    return model.TextBoxDisplay7 + " is mandatory !";
                }
                if (model.TextBoxDisplayType8 == NtsFieldType.NTS_Attachment && model.IsRequiredTextBoxDisplay8 && model.AttachmentValue8.IsNullOrEmpty())
                {
                    return model.TextBoxDisplay8 + " is mandatory !";
                }
                if (model.TextBoxDisplayType9 == NtsFieldType.NTS_Attachment && model.IsRequiredTextBoxDisplay9 && model.AttachmentValue9.IsNullOrEmpty())
                {
                    return model.TextBoxDisplay9 + " is mandatory !";
                }
                if (model.TextBoxDisplayType10 == NtsFieldType.NTS_Attachment && model.IsRequiredTextBoxDisplay10 && model.AttachmentValue10.IsNullOrEmpty())
                {
                    return model.TextBoxDisplay10 + " is mandatory !";
                }
                // date time picker & date picker
                if ((model.TextBoxDisplayType1 == NtsFieldType.NTS_DateTimePicker || model.TextBoxDisplayType1 == NtsFieldType.NTS_DatePicker) && model.IsRequiredTextBoxDisplay1 && !model.DatePickerValue1.IsNotNull())
                {
                    return model.TextBoxDisplay1 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType2 == NtsFieldType.NTS_DateTimePicker || model.TextBoxDisplayType2 == NtsFieldType.NTS_DatePicker) && model.IsRequiredTextBoxDisplay2 && !model.DatePickerValue2.IsNotNull())
                {
                    return model.TextBoxDisplay2 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType3 == NtsFieldType.NTS_DateTimePicker || model.TextBoxDisplayType3 == NtsFieldType.NTS_DatePicker) && model.IsRequiredTextBoxDisplay3 && !model.DatePickerValue3.IsNotNull())
                {
                    return model.TextBoxDisplay3 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType4 == NtsFieldType.NTS_DateTimePicker || model.TextBoxDisplayType4 == NtsFieldType.NTS_DatePicker) && model.IsRequiredTextBoxDisplay4 && !model.DatePickerValue4.IsNotNull())
                {
                    return model.TextBoxDisplay4 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType5 == NtsFieldType.NTS_DateTimePicker || model.TextBoxDisplayType5 == NtsFieldType.NTS_DatePicker) && model.IsRequiredTextBoxDisplay5 && !model.DatePickerValue5.IsNotNull())
                {
                    return model.TextBoxDisplay5 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType6 == NtsFieldType.NTS_DateTimePicker || model.TextBoxDisplayType6 == NtsFieldType.NTS_DatePicker) && model.IsRequiredTextBoxDisplay6 && !model.DatePickerValue6.IsNotNull())
                {
                    return model.TextBoxDisplay6 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType7 == NtsFieldType.NTS_DateTimePicker || model.TextBoxDisplayType7 == NtsFieldType.NTS_DatePicker) && model.IsRequiredTextBoxDisplay7 && !model.DatePickerValue7.IsNotNull())
                {
                    return model.TextBoxDisplay7 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType8 == NtsFieldType.NTS_DateTimePicker || model.TextBoxDisplayType8 == NtsFieldType.NTS_DatePicker) && model.IsRequiredTextBoxDisplay8 && !model.DatePickerValue8.IsNotNull())
                {
                    return model.TextBoxDisplay8 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType9 == NtsFieldType.NTS_DateTimePicker || model.TextBoxDisplayType9 == NtsFieldType.NTS_DatePicker) && model.IsRequiredTextBoxDisplay9 && !model.DatePickerValue9.IsNotNull())
                {
                    return model.TextBoxDisplay9 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType10 == NtsFieldType.NTS_DateTimePicker || model.TextBoxDisplayType10 == NtsFieldType.NTS_DatePicker) && model.IsRequiredTextBoxDisplay10 && !model.DatePickerValue10.IsNotNull())
                {
                    return model.TextBoxDisplay10 + " is mandatory !";
                }
                // textbox & textarea
                if ((model.TextBoxDisplayType1 == NtsFieldType.NTS_TextBox || model.TextBoxDisplayType1 == NtsFieldType.NTS_TextArea) && model.IsRequiredTextBoxDisplay1 && !model.TextValue1.IsNotNull())
                {
                    return model.TextBoxDisplay1 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType2 == NtsFieldType.NTS_TextBox || model.TextBoxDisplayType2 == NtsFieldType.NTS_TextArea) && model.IsRequiredTextBoxDisplay2 && !model.TextValue2.IsNotNull())
                {
                    return model.TextBoxDisplay2 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType3 == NtsFieldType.NTS_TextBox || model.TextBoxDisplayType3 == NtsFieldType.NTS_TextArea) && model.IsRequiredTextBoxDisplay3 && !model.TextValue3.IsNotNull())
                {
                    return model.TextBoxDisplay3 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType4 == NtsFieldType.NTS_TextBox || model.TextBoxDisplayType4 == NtsFieldType.NTS_TextArea) && model.IsRequiredTextBoxDisplay4 && !model.TextValue4.IsNotNull())
                {
                    return model.TextBoxDisplay4 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType5 == NtsFieldType.NTS_TextBox || model.TextBoxDisplayType5 == NtsFieldType.NTS_TextArea) && model.IsRequiredTextBoxDisplay5 && !model.TextValue5.IsNotNull())
                {
                    return model.TextBoxDisplay5 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType6 == NtsFieldType.NTS_TextBox || model.TextBoxDisplayType6 == NtsFieldType.NTS_TextArea) && model.IsRequiredTextBoxDisplay6 && !model.TextValue6.IsNotNull())
                {
                    return model.TextBoxDisplay6 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType7 == NtsFieldType.NTS_TextBox || model.TextBoxDisplayType7 == NtsFieldType.NTS_TextArea) && model.IsRequiredTextBoxDisplay7 && !model.TextValue7.IsNotNull())
                {
                    return model.TextBoxDisplay7 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType8 == NtsFieldType.NTS_TextBox || model.TextBoxDisplayType8 == NtsFieldType.NTS_TextArea) && model.IsRequiredTextBoxDisplay8 && !model.TextValue8.IsNotNull())
                {
                    return model.TextBoxDisplay8 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType9 == NtsFieldType.NTS_TextBox || model.TextBoxDisplayType9 == NtsFieldType.NTS_TextArea) && model.IsRequiredTextBoxDisplay9 && !model.TextValue9.IsNotNull())
                {
                    return model.TextBoxDisplay9 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType10 == NtsFieldType.NTS_TextBox || model.TextBoxDisplayType10 == NtsFieldType.NTS_TextArea) && model.IsRequiredTextBoxDisplay10 && !model.TextValue10.IsNotNull())
                {
                    return model.TextBoxDisplay10 + " is mandatory !";
                }

                switch (model.TemplateCode)
                {
                    case "SCHEDULE_INTERVIEW":
                        if (model.TextBoxDisplay1 == "Interview Date" && model.DatePickerValue1.IsNotNull())
                        {
                            if (model.DatePickerValue1.Value < DateTime.Now)
                            {
                                return " Interview Date cannot be less then todays date time.";
                            }
                        }
                        break;
                    case "TASK_DIRECT_HIRING":
                        var task = await _taskBusiness.GetSingleById(model.Id);
                        if (task.TextValue5.IsNotNullAndNotEmpty())
                        {
                            model.TextValue5 = task.TextValue5;
                        }
                        if (task.TextValue6.IsNotNullAndNotEmpty())
                        {
                            model.TextValue6 = task.TextValue6;
                        }

                        break;
                }
            }
            if (model.NtsType == NtsTypeEnum.Task && model.TemplateAction == NtsActionEnum.Complete)
            {
                if (model.TextBoxDisplayType1 == NtsFieldType.NTS_Attachment && model.IsRequiredTextBoxDisplay1 && model.AttachmentValue1.IsNullOrEmpty())
                {
                    return model.TextBoxDisplay1 + " is mandatory !";
                }
                if (model.TextBoxDisplayType2 == NtsFieldType.NTS_Attachment && model.IsRequiredTextBoxDisplay2 && model.AttachmentValue2.IsNullOrEmpty())
                {
                    return model.TextBoxDisplay2 + " is mandatory !";
                }
                if (model.TextBoxDisplayType3 == NtsFieldType.NTS_Attachment && model.IsRequiredTextBoxDisplay3 && model.AttachmentValue3.IsNullOrEmpty())
                {
                    return model.TextBoxDisplay3 + " is mandatory !";
                }
                if (model.TextBoxDisplayType4 == NtsFieldType.NTS_Attachment && model.IsRequiredTextBoxDisplay4 && model.AttachmentValue4.IsNullOrEmpty())
                {
                    return model.TextBoxDisplay4 + " is mandatory !";
                }
                if (model.TextBoxDisplayType5 == NtsFieldType.NTS_Attachment && model.IsRequiredTextBoxDisplay5 && model.AttachmentValue5.IsNullOrEmpty())
                {
                    return model.TextBoxDisplay5 + " is mandatory !";
                }
                if (model.TextBoxDisplayType6 == NtsFieldType.NTS_Attachment && model.IsRequiredTextBoxDisplay6 && model.AttachmentValue6.IsNullOrEmpty())
                {
                    return model.TextBoxDisplay6 + " is mandatory !";
                }
                if (model.TextBoxDisplayType7 == NtsFieldType.NTS_Attachment && model.IsRequiredTextBoxDisplay7 && model.AttachmentValue7.IsNullOrEmpty())
                {
                    return model.TextBoxDisplay7 + " is mandatory !";
                }
                if (model.TextBoxDisplayType8 == NtsFieldType.NTS_Attachment && model.IsRequiredTextBoxDisplay8 && model.AttachmentValue8.IsNullOrEmpty())
                {
                    return model.TextBoxDisplay8 + " is mandatory !";
                }
                if (model.TextBoxDisplayType9 == NtsFieldType.NTS_Attachment && model.IsRequiredTextBoxDisplay9 && model.AttachmentValue9.IsNullOrEmpty())
                {
                    return model.TextBoxDisplay9 + " is mandatory !";
                }
                if (model.TextBoxDisplayType10 == NtsFieldType.NTS_Attachment && model.IsRequiredTextBoxDisplay10 && model.AttachmentValue10.IsNullOrEmpty())
                {
                    return model.TextBoxDisplay10 + " is mandatory !";
                }
                // date time picker & date picker
                if ((model.TextBoxDisplayType1 == NtsFieldType.NTS_DateTimePicker || model.TextBoxDisplayType1 == NtsFieldType.NTS_DatePicker) && model.IsRequiredTextBoxDisplay1 && !model.DatePickerValue1.IsNotNull())
                {
                    return model.TextBoxDisplay1 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType2 == NtsFieldType.NTS_DateTimePicker || model.TextBoxDisplayType2 == NtsFieldType.NTS_DatePicker) && model.IsRequiredTextBoxDisplay2 && !model.DatePickerValue2.IsNotNull())
                {
                    return model.TextBoxDisplay2 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType3 == NtsFieldType.NTS_DateTimePicker || model.TextBoxDisplayType3 == NtsFieldType.NTS_DatePicker) && model.IsRequiredTextBoxDisplay3 && !model.DatePickerValue3.IsNotNull())
                {
                    return model.TextBoxDisplay3 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType4 == NtsFieldType.NTS_DateTimePicker || model.TextBoxDisplayType4 == NtsFieldType.NTS_DatePicker) && model.IsRequiredTextBoxDisplay4 && !model.DatePickerValue4.IsNotNull())
                {
                    return model.TextBoxDisplay4 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType5 == NtsFieldType.NTS_DateTimePicker || model.TextBoxDisplayType5 == NtsFieldType.NTS_DatePicker) && model.IsRequiredTextBoxDisplay5 && !model.DatePickerValue5.IsNotNull())
                {
                    return model.TextBoxDisplay5 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType6 == NtsFieldType.NTS_DateTimePicker || model.TextBoxDisplayType6 == NtsFieldType.NTS_DatePicker) && model.IsRequiredTextBoxDisplay6 && !model.DatePickerValue6.IsNotNull())
                {
                    return model.TextBoxDisplay6 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType7 == NtsFieldType.NTS_DateTimePicker || model.TextBoxDisplayType7 == NtsFieldType.NTS_DatePicker) && model.IsRequiredTextBoxDisplay7 && !model.DatePickerValue7.IsNotNull())
                {
                    return model.TextBoxDisplay7 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType8 == NtsFieldType.NTS_DateTimePicker || model.TextBoxDisplayType8 == NtsFieldType.NTS_DatePicker) && model.IsRequiredTextBoxDisplay8 && !model.DatePickerValue8.IsNotNull())
                {
                    return model.TextBoxDisplay8 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType9 == NtsFieldType.NTS_DateTimePicker || model.TextBoxDisplayType9 == NtsFieldType.NTS_DatePicker) && model.IsRequiredTextBoxDisplay9 && !model.DatePickerValue9.IsNotNull())
                {
                    return model.TextBoxDisplay9 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType10 == NtsFieldType.NTS_DateTimePicker || model.TextBoxDisplayType10 == NtsFieldType.NTS_DatePicker) && model.IsRequiredTextBoxDisplay10 && !model.DatePickerValue10.IsNotNull())
                {
                    return model.TextBoxDisplay10 + " is mandatory !";
                }
                // textbox & textarea
                if ((model.TextBoxDisplayType1 == NtsFieldType.NTS_TextBox || model.TextBoxDisplayType1 == NtsFieldType.NTS_TextArea) && model.IsRequiredTextBoxDisplay1 && !model.TextValue1.IsNotNull())
                {
                    return model.TextBoxDisplay1 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType2 == NtsFieldType.NTS_TextBox || model.TextBoxDisplayType2 == NtsFieldType.NTS_TextArea) && model.IsRequiredTextBoxDisplay2 && !model.TextValue2.IsNotNull())
                {
                    return model.TextBoxDisplay2 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType3 == NtsFieldType.NTS_TextBox || model.TextBoxDisplayType3 == NtsFieldType.NTS_TextArea) && model.IsRequiredTextBoxDisplay3 && !model.TextValue3.IsNotNull())
                {
                    return model.TextBoxDisplay3 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType4 == NtsFieldType.NTS_TextBox || model.TextBoxDisplayType4 == NtsFieldType.NTS_TextArea) && model.IsRequiredTextBoxDisplay4 && !model.TextValue4.IsNotNull())
                {
                    return model.TextBoxDisplay4 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType5 == NtsFieldType.NTS_TextBox || model.TextBoxDisplayType5 == NtsFieldType.NTS_TextArea) && model.IsRequiredTextBoxDisplay5 && !model.TextValue5.IsNotNull())
                {
                    return model.TextBoxDisplay5 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType6 == NtsFieldType.NTS_TextBox || model.TextBoxDisplayType6 == NtsFieldType.NTS_TextArea) && model.IsRequiredTextBoxDisplay6 && !model.TextValue6.IsNotNull())
                {
                    return model.TextBoxDisplay6 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType7 == NtsFieldType.NTS_TextBox || model.TextBoxDisplayType7 == NtsFieldType.NTS_TextArea) && model.IsRequiredTextBoxDisplay7 && !model.TextValue7.IsNotNull())
                {
                    return model.TextBoxDisplay7 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType8 == NtsFieldType.NTS_TextBox || model.TextBoxDisplayType8 == NtsFieldType.NTS_TextArea) && model.IsRequiredTextBoxDisplay8 && !model.TextValue8.IsNotNull())
                {
                    return model.TextBoxDisplay8 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType9 == NtsFieldType.NTS_TextBox || model.TextBoxDisplayType9 == NtsFieldType.NTS_TextArea) && model.IsRequiredTextBoxDisplay9 && !model.TextValue9.IsNotNull())
                {
                    return model.TextBoxDisplay9 + " is mandatory !";
                }
                if ((model.TextBoxDisplayType10 == NtsFieldType.NTS_TextBox || model.TextBoxDisplayType10 == NtsFieldType.NTS_TextArea) && model.IsRequiredTextBoxDisplay10 && !model.TextValue10.IsNotNull())
                {
                    return model.TextBoxDisplay10 + " is mandatory !";
                }
                //Dropdown
                //if (model.IsRequiredDropdownDisplay1 && model.DropdownValue1.IsNullOrEmpty())
                //{
                //    return model.IsDropdownDisplay1 + " is mandatory !";
                //}
                //if (model.IsRequiredDropdownDisplay2 && model.DropdownValue2.IsNullOrEmpty())
                //{
                //    return model.IsDropdownDisplay2 + " is mandatory !";
                //}
                //if (model.IsRequiredDropdownDisplay3 && model.DropdownValue3.IsNullOrEmpty())
                //{
                //    return model.IsDropdownDisplay3 + " is mandatory !";
                //}
                //if (model.IsRequiredDropdownDisplay4 && model.DropdownValue4.IsNullOrEmpty())
                //{
                //    return model.IsDropdownDisplay4 + " is mandatory !";
                //}
                //if (model.IsRequiredDropdownDisplay5 && model.DropdownValue5.IsNullOrEmpty())
                //{
                //    return model.IsDropdownDisplay5 + " is mandatory !";
                //}
                //if (model.IsRequiredDropdownDisplay6 && model.DropdownValue6.IsNullOrEmpty())
                //{
                //    return model.IsDropdownDisplay6 + " is mandatory !";
                //}
                //if (model.IsRequiredDropdownDisplay7 && model.DropdownValue7.IsNullOrEmpty())
                //{
                //    return model.IsDropdownDisplay7 + " is mandatory !";
                //}
                //if (model.IsRequiredDropdownDisplay8 && model.DropdownValue8.IsNullOrEmpty())
                //{
                //    return model.IsDropdownDisplay8 + " is mandatory !";
                //}
                //if (model.IsRequiredDropdownDisplay9 && model.DropdownValue9.IsNullOrEmpty())
                //{
                //    return model.IsDropdownDisplay9 + " is mandatory !";
                //}
                //if (model.IsRequiredDropdownDisplay10 && model.DropdownValue10.IsNullOrEmpty())
                //{
                //    return model.IsDropdownDisplay10 + " is mandatory !";
                //}
                switch (model.TemplateCode)
                {

                    case "SCHEDULE_INTERVIEW_RECRUITER":
                        if (model.TextBoxDisplay3 == "Interview Date" && model.DatePickerValue3.IsNotNull())
                        {
                            if (model.DatePickerValue3.Value < DateTime.Now)
                            {
                                return " Interview Date cannot be less then todays date";
                            }
                        }

                        break;
                    case "INTERVIEW_EVALUATION_HM":
                        var evalService = await _taskBusiness.GetSingleById(model.ReferenceTypeId);
                        var evalApplication = await _applicationBusiness.GetSingleById(evalService.ReferenceTypeId);
                        //!= InterviewFeedbackEnum.Selected || evalApplication.InterviewSelectionFeedback != InterviewFeedbackEnum.CanBeSelected || evalApplication.InterviewSelectionFeedback != InterviewFeedbackEnum.Rejected
                        if (!evalApplication.InterviewSelectionFeedback.IsNotNull())
                        {
                            return " Please Fill Interview Evaluation. ";
                        }
                        break;
                    case "EMPLOYEE_APPOINTMENT_APPROVAL1":
                        //Prepare Intent To Offer
                        var intentOfferService = await _taskBusiness.GetSingleById(model.ReferenceTypeId);
                        var intentOfferApplication = await _applicationBusiness.GetSingleById(intentOfferService.ReferenceTypeId);
                        if (intentOfferApplication.VisaCategory.IsNullOrEmpty() || intentOfferApplication.OfferGrade.IsNullOrEmpty())
                        {
                            return " Please Fill Prepare Intent To Offer. ";
                        }
                        break;
                    case "FINAL_OFFER_APPROVAL_RECRUITER":
                        //Prepare Final Offer
                        var finalOfferService = await _taskBusiness.GetSingleById(model.ReferenceTypeId);
                        var finalOfferApplication = await _applicationBusiness.GetSingleById(finalOfferService.ReferenceTypeId);
                        if (!finalOfferApplication.ContractStartDate.IsNotNull())
                        {
                            return " Please Fill Prepare Final Offer. ";
                        }
                        break;
                    case "FILL_WORKER_DETAILS":
                        //Prepare Joining Report
                        var joiningService = await _taskBusiness.GetSingleById(model.ReferenceTypeId);
                        var joiningApplication = await _applicationBusiness.GetSingleById(joiningService.ReferenceTypeId);
                        if (joiningApplication.JobNo.IsNullOrEmpty())
                        {
                            return " Please Fill Prepare Joining Report. ";
                        }
                        break;
                    case "FILL_STAFF_DETAILS":
                        //Staff Prepare Joining Report
                        var sjoiningService = await _taskBusiness.GetSingleById(model.ReferenceTypeId);
                        var sjoiningApplication = await _applicationBusiness.GetSingleById(sjoiningService.ReferenceTypeId);
                        if (sjoiningApplication.JobNo.IsNullOrEmpty())
                        {
                            return " Please Fill Prepare Joining Report. ";
                        }
                        break;
                    case "BOOK_QVC_APPOINTMENT":
                        if (model.TextBoxDisplay2 == "Qvc Appointment Date" && model.DatePickerValue2.IsNotNull())
                        {
                            if (model.DatePickerValue2.Value < DateTime.Now)
                            {
                                return " Qvc Appointment Date cannot be less then todays date";
                            }
                        }

                        break;

                    case "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE":
                        if (model.TextBoxDisplay2 == "Travelling Date" && model.DatePickerValue2.IsNotNull())
                        {
                            if (model.DatePickerValue2.Value < DateTime.Now)
                            {
                                return "Travelling Date cannot be less then todays date";
                            }
                        }

                        break;
                    case "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE_WORKVISA":
                        if (model.TextBoxDisplay2 == "Travelling Date" && model.DatePickerValue2.IsNotNull())
                        {
                            if (model.DatePickerValue2.Value < DateTime.Now)
                            {
                                return "Travelling Date cannot be less then todays date";
                            }
                        }

                        break;
                    case "JOBDESCRIPTION_HM":
                        var jobId = model.DropdownValue1;
                        var orgId = model.DropdownValue2;
                        var jd = await _jobDescriptionBusiness.GetSingle(x => x.JobId == jobId);
                        if (jd == null)
                        {
                            return "Job Description Link - Please fill required fields.";
                        }
                        break;
                    case "STAFF_CONFIRM_PROBATION_DATE_BY_HM":
                        var sprobationStatusId = model.DropdownValue1;
                        var sprobationPeriodId = model.DropdownValue2;
                        var ps = await _lovBusiness.GetSingle(x => x.Id == sprobationStatusId);
                        if (ps != null)
                        {
                            var pp = await _lovBusiness.GetSingle(x => x.Id == sprobationPeriodId);
                            if (ps.Code == "PROBATION_EXTEND")
                            {
                                if (pp == null)
                                {
                                    return "Please select probation period.";
                                }
                            }
                            else
                            {
                                if (pp != null)
                                {
                                    return "Please un-select probation period (only for Extend).";
                                }
                            }
                        }
                        else
                        {
                            return "Please select probation status.";
                        }
                        break;
                    case "STAFF_CONFIRM_PROBATION_DATE_BY_HR":
                        var sprobationStatusId2 = model.DropdownValue1;
                        var sprobationPeriodId2 = model.DropdownValue2;
                        var ps2 = await _lovBusiness.GetSingle(x => x.Id == sprobationStatusId2);
                        if (ps2 != null)
                        {
                            var pp2 = await _lovBusiness.GetSingle(x => x.Id == sprobationPeriodId2);
                            if (ps2.Code == "PROBATION_EXTEND")
                            {
                                if (pp2 == null)
                                {
                                    return "Please select probation period.";
                                }
                            }
                            else
                            {
                                if (pp2 != null)
                                {
                                    return "Please un-select probation period (only for Extend).";
                                }
                            }
                        }
                        else
                        {
                            return "Please select probation status.";
                        }
                        break;
                    case "WORKER_CONFIRM_PROBATION_DATE_BY_HM":
                        var wprobationStatusId = model.DropdownValue1;
                        var wprobationPeriodId = model.DropdownValue2;
                        var wps = await _lovBusiness.GetSingle(x => x.Id == wprobationStatusId);
                        if (wps != null)
                        {
                            var wpp = await _lovBusiness.GetSingle(x => x.Id == wprobationPeriodId);
                            if (wps.Code == "PROBATION_EXTEND")
                            {
                                if (wpp == null)
                                {
                                    return "Please select probation period.";
                                }
                            }
                            else
                            {
                                if (wpp != null)
                                {
                                    return "Please un-select probation period (only for Extend).";
                                }
                            }
                        }
                        else
                        {
                            return "Please select probation status.";
                        }
                        break;
                    case "WORKER_CONFIRM_PROBATION_DATE_BY_HR":
                        var wprobationStatusId2 = model.DropdownValue1;
                        var wprobationPeriodId2 = model.DropdownValue2;
                        var wps2 = await _lovBusiness.GetSingle(x => x.Id == wprobationStatusId2);
                        if (wps2 != null)
                        {
                            var wpp2 = await _lovBusiness.GetSingle(x => x.Id == wprobationPeriodId2);
                            if (wps2.Code == "PROBATION_EXTEND")
                            {
                                if (wpp2 == null)
                                {
                                    return "Please select probation period.";
                                }
                            }
                            else
                            {
                                if (wpp2 != null)
                                {
                                    return "Please un-select probation period (only for Extend).";
                                }
                            }
                        }
                        else
                        {
                            return "Please select probation status.";
                        }
                        break;
                    case "TASK_DIRECT_HIRING":
                        var task = await _taskBusiness.GetSingleById(model.Id);
                        if (task.TextValue5.IsNullOrEmpty() || task.TextValue6.IsNullOrEmpty())
                        {
                            return "Please fill Candidate Profile details and batch details.";
                        }
                        else
                        {
                            model.TextValue5 = task.TextValue5;
                            model.TextValue6 = task.TextValue6;
                        }

                        break;
                        //case "WORKER_POOL_HOD":
                        //    var service = await _taskBusiness.GetSingleById(model.ReferenceTypeId);
                        //    var applicationlist = await _applicationBusiness.GetList(x => x.WorkerBatchId == service.ReferenceTypeId);
                        //    // var app1 = await _applicationBusiness.GetApplicationDetail(service1.ReferenceTypeId);
                        //    foreach (var app1 in applicationlist)
                        //    {
                        //        if (app1.HodApproval == null)
                        //        {
                        //            string res = "Please Select Yes or No Approval for" + " " + app1.FirstName;
                        //            return res;
                        //        }

                        //    }

                        //    break;
                        //case "WORKER_POOL_HR_HEAD":
                        //    var service1 = await _taskBusiness.GetSingleById(model.ReferenceTypeId);
                        //    var applicationlist1 = await _applicationBusiness.GetList(x => x.WorkerBatchId == service1.ReferenceTypeId);
                        //    // var app1 = await _applicationBusiness.GetApplicationDetail(service1.ReferenceTypeId);
                        //    foreach (var app1 in applicationlist1)
                        //    {
                        //        if (app1.HRHeadApproval == null)
                        //        {
                        //            string res = "Please Select Yes or No Approval for" + " " + app1.FirstName;
                        //            return res;
                        //        }

                        //    }


                        //    break;
                        //case "WORKER_POOL_PLANNING":
                        //    var service2 = await _taskBusiness.GetSingleById(model.ReferenceTypeId);
                        //    var applicationlist2 = await _applicationBusiness.GetList(x => x.WorkerBatchId == service2.ReferenceTypeId);
                        //    // var app1 = await _applicationBusiness.GetApplicationDetail(service1.ReferenceTypeId);
                        //    foreach (var app1 in applicationlist2)
                        //    {
                        //        if (app1.PlanningApproval == null)
                        //        {
                        //            string res = "Please Select Yes or No Approval for" + " " + app1.FirstName;
                        //            return res;
                        //        }

                        //    }

                        //    break;
                        //case "WORKER_POOL_ED":
                        //    var service3 = await _taskBusiness.GetSingleById(model.ReferenceTypeId);
                        //    var applicationlist3 = await _applicationBusiness.GetList(x => x.WorkerBatchId == service3.ReferenceTypeId);
                        //    // var app1 = await _applicationBusiness.GetApplicationDetail(service1.ReferenceTypeId);
                        //    foreach (var app1 in applicationlist3)
                        //    {
                        //        if (app1.EDApproval == null)
                        //        {
                        //            string res = "Please Select Yes or No Approval for" + " " + app1.FirstName;
                        //            return res;
                        //        }

                        //    }

                        //    break;

                }
            }
            return "success";
        }
        public async Task<bool> PostScriptTask(RecTaskViewModel model)
        {
            var referenceId = "";
            var batchId = "";

            if (model.TemplateAction != NtsActionEnum.Draft)
            {
                try
                {
                    await CreateTaskNotification(model);
                    await SendExternalTaskNotification(model);
                }
                catch (Exception ex)
                {

                }
            }

            if (model.ReferenceTypeCode == ReferenceTypeEnum.REC_Application)
            {
                referenceId = model.ReferenceTypeId;
            }
            else if (model.ReferenceTypeCode == ReferenceTypeEnum.REC_WorkerPoolBatch)
            {
                //referenceId = model.ReferenceTypeId;
                batchId = model.ReferenceTypeId;
            }
            else if (model.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
            {
                var service = await _taskBusiness.GetSingleById(model.ReferenceTypeId);
                if (service.ReferenceTypeCode == ReferenceTypeEnum.REC_Application)
                {
                    referenceId = service.ReferenceTypeId;
                }
                else if (service.ReferenceTypeCode == ReferenceTypeEnum.REC_WorkerPoolBatch)
                {
                    batchId = service.ReferenceTypeId;
                }

            }
            if (model.TemplateAction == NtsActionEnum.Complete || model.TemplateAction == NtsActionEnum.Reject || model.TemplateAction == NtsActionEnum.NotApplicable || model.TemplateAction == NtsActionEnum.Cancel)
            {
                var versionModel = _autoMapper.Map<RecTaskViewModel, RecTaskViewModel>(model);
                versionModel.ReferenceTypeCode = ReferenceTypeEnum.NTS_Task;
                versionModel.ReferenceTypeId = model.Id;
                versionModel.Id = null;
                await _taskBusiness.Createversion(versionModel);

                if (model.TemplateAction == NtsActionEnum.Reject && model.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                {
                    if (model.ReturnTemplateName.IsNullOrEmpty())
                    {
                        await CompleteStepTask(model.ReferenceTypeId, NtsActionEnum.Cancel);
                    }
                    else
                    {
                        var stepTask = await _taskBusiness.GetStepTaskId(model.ReferenceTypeId);
                        var returnTask = stepTask.Where(x => x.TemplateCode == model.ReturnTemplateName).FirstOrDefault();
                        if (returnTask.IsNotNull() && returnTask.Id.IsNotNullAndNotEmpty())
                        {
                            await CompleteStepTask(returnTask.Id, NtsActionEnum.EditAsNewVersion);
                        }
                    }
                    //RecTaskViewModel previousTask = null;
                    //foreach (var item in stepTask)
                    //{                        
                    //    if (item.Id == model.Id)
                    //    {
                    //        break;
                    //    }
                    //    previousTask = item;

                    //}
                    //if (previousTask == null)
                    //{
                    //    await CompleteStepTask(model.ReferenceTypeId, NtsActionEnum.Cancel);
                    //}
                    //else
                    //{
                    //    await CompleteStepTask(previousTask.Id, NtsActionEnum.EditAsNewVersion);
                    //}
                }

            }
            if (model.TemplateAction == NtsActionEnum.Submit && model.NtsType == NtsTypeEnum.Task)
            {
                if (referenceId.IsNotNullAndNotEmpty())
                {
                    var application = await _applicationBusiness.GetApplicationDetail(referenceId);
                    if (application != null)
                    {
                        await _applicationBusiness.CreateApplicationStatusTrack(application.Id, model.TemplateCode, model.Id);
                    }

                }
                else if (batchId.IsNotNullAndNotEmpty())
                {
                    var applicationlist = await _applicationBusiness.GetList(x => x.WorkerBatchId == batchId);
                    foreach (var application in applicationlist)
                    {
                        await _applicationBusiness.CreateApplicationStatusTrack(application.Id, model.TemplateCode, model.Id);
                    }
                }
            }
            if (model.TemplateAction == NtsActionEnum.Submit && model.NtsType == NtsTypeEnum.Service)
            {
                switch (model.TemplateCode)
                {
                    case "SCHEDULE_INTERVIEW":
                        await _applicationBusiness.UpdateApplicationtStatus(referenceId, "InterviewRequested");
                        await _applicationBusiness.CreateApplicationStatusTrack(referenceId);
                        break;
                }
                await AssignStepTask(model.StepTemplateIds, model.Id, model.VersionNo.ToString(), model.OwnerUserId);
            }
            if (model.TemplateAction == NtsActionEnum.Cancel && model.NtsType == NtsTypeEnum.Service)
            {
                await CancelAllStepTask(model.Id);
                //if (model.TemplateCode == "TRAVEL")
                //{
                //    await CompleteStepTask(model.Id, NtsActionEnum.EditAsNewVersion);
                //}

            }
            if (model.TemplateAction == NtsActionEnum.Complete || model.TemplateAction == NtsActionEnum.NotApplicable)
            {
                if (model.NtsType == NtsTypeEnum.Service)
                {

                }
                var application = await _applicationBusiness.GetApplicationDetail(referenceId);
                var OwnerId = _userContext.UserId;
                var batch = new BatchViewModel();
                if (application == null)
                {
                    batch = await _applicationBusiness.GetSingle<BatchViewModel, Batch>(x => x.Id == batchId);
                }
                else
                {
                    batch = await _applicationBusiness.GetSingle<BatchViewModel, Batch>(x => x.Id == application.BatchId);
                }

                if (batch != null)
                {
                    OwnerId = batch.CreatedBy;
                }
                switch (model.TemplateCode)
                {
                    case "FILL_STAFF_DETAILS":
                        await _applicationBusiness.UpdateApplicationState(referenceId, "PostStaffJoined");
                        await _manpowerRecruitmentSummaryBusiness.UpdateManpowerRecruitmentSummaryForAvailable(referenceId);
                        break;
                    case "FILL_WORKER_DETAILS":
                        await _applicationBusiness.UpdateApplicationState(referenceId, "PostWorkerJoined");
                        await _manpowerRecruitmentSummaryBusiness.UpdateManpowerRecruitmentSummaryForAvailable(referenceId);
                        break;
                    case "INTERVIEW_WAITLIST_HR":
                        var code = await _lovBusiness.GetSingleById(model.DropdownValue3);
                        if (code != null && code.Code == "SELECT_CANDIDATE")
                        {
                            if (application.ManpowerTypeCode == "Staff")
                            {
                                await CreateService("EMPLOYEE_APPOINTMENT", referenceId, "", ReferenceTypeEnum.REC_Application, OwnerId);
                                await _applicationBusiness.UpdateApplicationState(referenceId, "IntentToOffer");
                            }

                            await _applicationBusiness.UpdateApplicationtStatus(referenceId, "SELECTED");
                            await _applicationBusiness.CreateApplicationStatusTrack(referenceId);
                        }
                        else if (code != null && code.Code == "REREVIEW")
                        {
                            await _applicationBusiness.UpdateApplicationState(referenceId, "Rereviewed");
                            await _applicationBusiness.CreateApplicationStatusTrack(referenceId);
                        }
                        break;
                    case "SCHEDULE_INTERVIEW_CANDIDATE":
                        await _applicationBusiness.UpdateApplicationtStatus(referenceId, "Interview");
                        await _applicationBusiness.CreateApplicationStatusTrack(referenceId);
                        break;
                    case "INTERVIEW_EVALUATION_HM":
                        await _applicationBusiness.CreateApplicationStatusTrack(referenceId);
                        if (application.InterviewSelectionFeedback == InterviewFeedbackEnum.Selected || application.InterviewSelectionFeedback == InterviewFeedbackEnum.CanBeSelected)
                        {
                            if (application.ManpowerTypeCode == "Staff")
                            {
                                await _applicationBusiness.UpdateApplicationState(referenceId, "IntentToOffer");
                                await _applicationBusiness.CreateApplicationStatusTrack(referenceId);
                                var raiseresult = await this.ServiceEmployeeAppointment(referenceId);
                            }
                            else
                            {
                                await _applicationBusiness.UpdateApplicationState(referenceId, "WorkerPool");
                                await _applicationBusiness.CreateApplicationStatusTrack(referenceId);
                                var raiseresult = await this.WorkerAppointment(referenceId);
                            }
                        }
                        break;
                    case "DIRECT_HIRING":

                        if (application.ManpowerTypeCode == "Staff")
                        {
                            await _applicationBusiness.UpdateApplicationState(referenceId, "IntentToOffer");
                            await _applicationBusiness.CreateApplicationStatusTrack(referenceId);
                            var raiseresult = await this.ServiceEmployeeAppointment(referenceId);
                        }
                        else
                        {
                            await _applicationBusiness.UpdateApplicationState(referenceId, "WorkerPool");
                            await _applicationBusiness.CreateApplicationStatusTrack(referenceId);
                            var raiseresult = await this.WorkerAppointment(referenceId);
                        }

                        break;
                    case "WORKER_POOL_HR":
                        if (model.TextValue1.IsNotNullAndNotEmpty())
                        {
                            var app = await _applicationBusiness.GetSingleById(referenceId);
                            app.SalaryOnAppointment = model.TextValue1;
                            await _applicationBusiness.Edit(app);
                        }
                        break;
                    case "EMPLOYEE_APPOINTMENT":
                        await _applicationBusiness.UpdateApplicationState(referenceId, "FinalOffer");
                        await _applicationBusiness.CreateApplicationStatusTrack(referenceId);
                        await CreateService("PREPARE_FINAL_OFFER", referenceId, "", ReferenceTypeEnum.REC_Application, OwnerId);

                        break;
                    case "PREPARE_FINAL_OFFER":

                        if (application.VisaCategoryCode == "VISA_TRANSFER")
                        {
                            await _applicationBusiness.UpdateApplicationState(referenceId, "VisaTransfer");
                            await _applicationBusiness.CreateApplicationStatusTrack(referenceId);
                            await CreateService("VISA_TRANSFER", referenceId, "", ReferenceTypeEnum.REC_Application, OwnerId);
                        }
                        else if (application.VisaCategoryCode == "LOCAL_WORK_PERMIT")
                        {
                            await _applicationBusiness.UpdateApplicationState(referenceId, "WorkPermit");
                            await _applicationBusiness.CreateApplicationStatusTrack(referenceId);
                            await CreateService("LOCAL_WORK_PERMIT", referenceId, "", ReferenceTypeEnum.REC_Application, OwnerId);
                        }
                        else if (application.VisaCategoryCode == "BUSINESS_VISA")
                        {
                            await _applicationBusiness.UpdateApplicationState(referenceId, "BusinessVisa");
                            await _applicationBusiness.CreateApplicationStatusTrack(referenceId);
                            await CreateService("BUSINESS_VISA_MEDICAL", referenceId, "", ReferenceTypeEnum.REC_Application, OwnerId);
                        }
                        else if (application.VisaCategoryCode == "BUSINESS_VISA_MULTIPLE")
                        {
                            await _applicationBusiness.UpdateApplicationState(referenceId, "BusinessVisa");
                            await _applicationBusiness.CreateApplicationStatusTrack(referenceId);
                            await CreateService("BUSINESS_VISA_MEDICAL", referenceId, "", ReferenceTypeEnum.REC_Application, OwnerId);
                        }
                        else if (application.VisaCategoryCode == "WORK_VISA")
                        {
                            await _applicationBusiness.UpdateApplicationState(referenceId, "WorkVisa");
                            await _applicationBusiness.CreateApplicationStatusTrack(referenceId);
                            await CreateService("WORKER_VISA_MEDICAL", referenceId, "", ReferenceTypeEnum.REC_Application, OwnerId);
                        }

                        break;
                    case "VISA_TRANSFER":
                        if (application.ManpowerTypeCode == "Staff")
                        {
                            await _applicationBusiness.UpdateApplicationState(referenceId, "StaffJoined");
                            await _applicationBusiness.CreateApplicationStatusTrack(referenceId);
                            await CreateService("STAFF_JOINING_FORMALITIES", referenceId, "", ReferenceTypeEnum.REC_Application, OwnerId);

                        }
                        else
                        {
                            await _applicationBusiness.UpdateApplicationState(referenceId, "WorkerJoined");
                            await _applicationBusiness.CreateApplicationStatusTrack(referenceId);
                            await CreateService("WORKER_JOINING_FORMALITIES", referenceId, "", ReferenceTypeEnum.REC_Application, OwnerId);

                        }
                        break;
                    case "LOCAL_WORK_PERMIT":
                        if (application.ManpowerTypeCode == "Staff")
                        {
                            await _applicationBusiness.UpdateApplicationState(referenceId, "StaffJoined");
                            await _applicationBusiness.CreateApplicationStatusTrack(referenceId);
                            await CreateService("STAFF_JOINING_FORMALITIES", referenceId, "", ReferenceTypeEnum.REC_Application, OwnerId);

                        }
                        else
                        {
                            await _applicationBusiness.UpdateApplicationState(referenceId, "WorkerJoined");
                            await _applicationBusiness.CreateApplicationStatusTrack(referenceId);
                            await CreateService("WORKER_JOINING_FORMALITIES", referenceId, "", ReferenceTypeEnum.REC_Application, OwnerId);

                        }
                        break;

                    case "BUSINESS_VISA_MEDICAL":
                        await CreateService("TRAVEL", referenceId, "", ReferenceTypeEnum.REC_Application, OwnerId);
                        // await _applicationBusiness.UpdateApplicationState(referenceId, "StaffJoined");
                        //await _applicationBusiness.CreateApplicationStatusTrack(model.ReferenceTypeId);  

                        break;

                    case "WORKER_VISA_MEDICAL":
                        await CreateService("TRAVEL", referenceId, "", ReferenceTypeEnum.REC_Application, OwnerId);
                        //await _applicationBusiness.UpdateApplicationState(referenceId, "StaffJoined");
                        //await _applicationBusiness.CreateApplicationStatusTrack(model.ReferenceTypeId);                                            
                        break;

                    case "TRAVEL":
                        //if (application.ManpowerTypeCode == "Staff")
                        //{
                        //    await _applicationBusiness.UpdateApplicationState(referenceId, "StaffJoined");
                        //    await _applicationBusiness.CreateApplicationStatusTrack(referenceId);
                        //    await CreateService("STAFF_JOINING_FORMALITIES", referenceId, "", ReferenceTypeEnum.REC_Application, OwnerId);

                        //}
                        //else
                        //{
                        //    await _applicationBusiness.UpdateApplicationState(referenceId, "WorkerJoined");
                        //    await _applicationBusiness.CreateApplicationStatusTrack(referenceId);
                        //    await CreateService("WORKER_JOINING_FORMALITIES", referenceId, "", ReferenceTypeEnum.REC_Application, OwnerId);

                        //}
                        await AssignTaskToAdminforAccomadation(model.Id, null);
                        await AssignTaskToPlantforPickup(model.Id, null);

                        break;

                    case "CONFIRM_RECEIPT_TICKET_DATE_OF_TRAVEL":

                        // await AssignTaskToAdminforAccomadation(model.ReferenceTypeId, null);
                        // await AssignTaskToPlantforPickup(model.ReferenceTypeId, null);
                        break;
                    case "ARRANGE_ACCOMMODATION":
                        var service1 = await _taskBusiness.GetSingleById(model.ReferenceTypeId);
                        var applicant1 = await _applicationBusiness.GetApplicationDetail(service1.ReferenceTypeId);
                        var state1 = await _applicationBusiness.GetSingleGlobal<IdNameViewModel, ApplicationState>(x => x.Id == applicant1.ApplicationState);
                        if (state1.Code != "StaffJoined" && state1.Code != "WorkerJoined")
                        {
                            if (applicant1.ManpowerTypeCode == "Staff")
                            {
                                await _applicationBusiness.UpdateApplicationState(applicant1.Id, "StaffJoined");
                                await _applicationBusiness.CreateApplicationStatusTrack(applicant1.Id);
                                await CreateService("STAFF_JOINING_FORMALITIES", applicant1.Id, "", ReferenceTypeEnum.REC_Application, OwnerId);

                            }
                            else
                            {
                                await _applicationBusiness.UpdateApplicationState(applicant1.Id, "WorkerJoined");
                                await _applicationBusiness.CreateApplicationStatusTrack(applicant1.Id);
                                await CreateService("WORKER_JOINING_FORMALITIES", applicant1.Id, "", ReferenceTypeEnum.REC_Application, OwnerId);

                            }
                        }

                        break;

                    case "ARRANGE_VEHICLE_PICKUP":
                        var service2 = await _taskBusiness.GetSingleById(model.ReferenceTypeId);
                        var applicant2 = await _applicationBusiness.GetApplicationDetail(service2.ReferenceTypeId);
                        var state2 = await _applicationBusiness.GetSingleGlobal<IdNameViewModel, ApplicationState>(x => x.Id == applicant2.ApplicationState);
                        if (state2.Code != "StaffJoined" && state2.Code != "WorkerJoined")
                        {
                            if (applicant2.ManpowerTypeCode == "Staff")
                            {
                                await _applicationBusiness.UpdateApplicationState(applicant2.Id, "StaffJoined");
                                await _applicationBusiness.CreateApplicationStatusTrack(applicant2.Id);
                                await CreateService("STAFF_JOINING_FORMALITIES", applicant2.Id, "", ReferenceTypeEnum.REC_Application, OwnerId);

                            }
                            else
                            {
                                await _applicationBusiness.UpdateApplicationState(applicant2.Id, "WorkerJoined");
                                await _applicationBusiness.CreateApplicationStatusTrack(applicant2.Id);
                                await CreateService("WORKER_JOINING_FORMALITIES", applicant2.Id, "", ReferenceTypeEnum.REC_Application, OwnerId);

                            }
                        }

                        break;
                    //case "WORKER_POOL_REQUEST":
                    //    var applicantlist = await _applicationBusiness.GetList(x => x.WorkerBatchId == batchId);

                    //    foreach (var applicant in applicantlist)
                    //    {                            
                    //        await AssignTaskToAgencyForWorkerSalaryApproval(applicant.Id, null);
                    //    }
                    //    break;

                    case "WORKER_POOL_REQUEST":
                        //var applicantlist = await _applicationBusiness.GetList(x => x.WorkerBatchId == batchId);
                        //var applicants=applicantlist.Select(x => x.Id).ToArray();
                        //var applicantsId=string.Join(",", applicants);

                        if (model.TemplateAction == NtsActionEnum.Complete)
                        {
                            var applicant = await _applicationBusiness.GetSingleById(referenceId);
                            if (applicant != null)
                            {
                                var batch1 = await _applicationBusiness.GetSingle<BatchViewModel, Batch>(x => x.Id == applicant.BatchId);
                                await _applicationBusiness.UpdateApplicationState(referenceId, "FinalOffer");
                                await _applicationBusiness.CreateApplicationStatusTrack(referenceId);
                                await CreateService("PREPARE_FINAL_OFFER", model.ReferenceTypeId, "", ReferenceTypeEnum.REC_Application, batch1.CreatedBy);
                            }

                        }

                        //if (applicant.HodApproval == true && applicant.HRHeadApproval == true && applicant.PlanningApproval == true && applicant.EDApproval == true)
                        //{

                        //}


                        break;
                    case "STAFF_JOINING_FORMALITIES":
                        //await _applicationBusiness.UpdateApplicationState(referenceId, "Joined");
                        //await _applicationBusiness.CreateApplicationStatusTrack(referenceId);
                        //await _manpowerRecruitmentSummaryBusiness.UpdateManpowerRecruitmentSummaryForAvailable(referenceId);
                        await CreateService("STAFF_CONFIRM_PROBATION_DATE", referenceId, "", ReferenceTypeEnum.REC_Application, OwnerId);
                        break;

                    case "WORKER_JOINING_FORMALITIES":
                        //await _applicationBusiness.UpdateApplicationState(referenceId, "Joined");
                        //await _applicationBusiness.CreateApplicationStatusTrack(referenceId);
                        //await _manpowerRecruitmentSummaryBusiness.UpdateManpowerRecruitmentSummaryForAvailable(referenceId);
                        await CreateService("WORKER_CONFIRM_PROBATION_DATE", referenceId, "", ReferenceTypeEnum.REC_Application, OwnerId);
                        break;
                    case "STAFF_CONFIRM_PROBATION_DATE":
                        var service = await _taskBusiness.GetStepTaskListByService(model.Id);
                        var data = service.Where(x => x.TemplateCode == "STAFF_CONFIRM_PROBATION_DATE_BY_HR").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            var probationStatusId = data.DropdownValue1;
                            var probationPeriodId = data.DropdownValue2;
                            var ps = await _lovBusiness.GetSingle(x => x.Id == probationStatusId);
                            if (ps != null)
                            {
                                var pp = await _lovBusiness.GetSingle(x => x.Id == probationPeriodId);
                                if (ps.Code == "PROBATION_EXTEND")
                                {
                                    if (pp != null)
                                    {
                                        var month = pp.Code.Split("PROBATION_PERIOD_");
                                        var date = DateTime.Now;
                                        var startdate = date.AddMonths(Convert.ToInt32(month[1]));
                                        var newmodel = model;
                                        newmodel.StartDate = startdate;
                                        newmodel.DataAction = DataActionEnum.Edit;
                                        newmodel.TemplateAction = NtsActionEnum.EditAsNewVersion;
                                        newmodel.VersionNo++;
                                        var newresult = await Manage(model);
                                    }
                                }
                                else
                                {
                                    await _applicationBusiness.UpdateApplicationState(referenceId, "Joined");
                                    await _applicationBusiness.CreateApplicationStatusTrack(referenceId);
                                }
                            }
                        }
                        break;
                    case "WORKER_CONFIRM_PROBATION_DATE":
                        var wservice = await _taskBusiness.GetStepTaskListByService(model.Id);
                        var wdata = wservice.Where(x => x.TemplateCode == "WORKER_CONFIRM_PROBATION_DATE_BY_HR").FirstOrDefault();
                        if (wdata.IsNotNull())
                        {
                            var probationStatusId = wdata.DropdownValue1;
                            var probationPeriodId = wdata.DropdownValue2;
                            var ps = await _lovBusiness.GetSingle(x => x.Id == probationStatusId);
                            if (ps != null)
                            {
                                var pp = await _lovBusiness.GetSingle(x => x.Id == probationPeriodId);
                                if (ps.Code == "PROBATION_EXTEND")
                                {
                                    if (pp != null)
                                    {
                                        var month = pp.Code.Split("PROBATION_PERIOD_");
                                        var date = DateTime.Now;
                                        var startdate = date.AddMonths(Convert.ToInt32(month[1]));
                                        var newmodel = model;
                                        newmodel.StartDate = startdate;
                                        newmodel.DataAction = DataActionEnum.Edit;
                                        newmodel.TemplateAction = NtsActionEnum.EditAsNewVersion;
                                        newmodel.VersionNo++;
                                        var newresult = await Manage(model);
                                    }
                                    else
                                    {
                                        await _applicationBusiness.UpdateApplicationState(referenceId, "Joined");
                                        await _applicationBusiness.CreateApplicationStatusTrack(referenceId);
                                    }
                                }
                            }
                        }
                        break;
                    case "TASK_DIRECT_HIRING":
                        var task = await _taskBusiness.GetSingleById(model.Id);
                        var batchData = await _batchBusiness.GetSingleById(task.TextValue5);

                        var res = await _applicationBusiness.CreateApplication(new CandidateProfileViewModel { Id = task.TextValue6 }, batchData);
                        await _taskBusiness.UpdateServiceReference(model.ReferenceTypeId, res.Item.Id);
                        break;
                        ////
                        //case "MEDICAL_TEST":
                        //    await AssignTaskForVisaAppointment(model.ReferenceTypeId,null);
                        //    await _applicationBusiness.UpdateApplicationState(model.ReferenceTypeId, "MedicalCompleted");
                        //    break;
                        //case "VISA_APPOINTMENT":
                        //    await AssignTaskForBiometricCompletion(model.ReferenceTypeId, null);
                        //    await _applicationBusiness.UpdateApplicationState(model.ReferenceTypeId, "VisaAppointmentTaken");
                        //    break;
                        //case "BIOMETRIC_COMPLETION":
                        //    await AssignTaskForVisaApproval(model.ReferenceTypeId, null);
                        //    await _applicationBusiness.UpdateApplicationState(model.ReferenceTypeId, "BiometricCompleted");
                        //    break;
                        //case "VISA_APPROVAL":
                        //    await AssignTaskForVisaSentToCandidate(model.ReferenceTypeId, null);
                        //    await _applicationBusiness.UpdateApplicationState(model.ReferenceTypeId, "VisaApproved");
                        //    break;
                        //case "VISA_TO_CANDIDATE":
                        //    await AssignTaskForFlightTicketBooking(model.ReferenceTypeId, null);
                        //    await _applicationBusiness.UpdateApplicationState(model.ReferenceTypeId, "VisaSentToCandidates");
                        //    break;
                        //case "FLIGHT_TICKET_BOOKING":
                        //    await AssignTaskForCandiadteArrival(model.ReferenceTypeId, null);
                        //    await _applicationBusiness.UpdateApplicationState(model.ReferenceTypeId, "FlightTicketsBooked");
                        //    break;
                        //case "CANDIDATE_ARRIVAL":
                        //    await AssignTaskForCandiadteJoining(model.ReferenceTypeId, null);
                        //    await _applicationBusiness.UpdateApplicationState(model.ReferenceTypeId, "CandidateArrived");
                        //    break;
                        //case "CANDIADTE_JOINING":                        
                        //    await _applicationBusiness.UpdateApplicationState(model.ReferenceTypeId, "Joined");
                        //    break;
                        //case "EMPLOYEE_APPOINTMENT":
                        //    await _applicationBusiness.UpdateApplicationState(model.ReferenceTypeId, "ApprovedApplication");
                        //    break;
                }

                if (model.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                {
                    var serviceId = await _taskBusiness.GetServiceId(model.Id);
                    if (serviceId.IsNotNullAndNotEmpty())
                    {
                        var stepTask = await _taskBusiness.GetStepTaskId(serviceId);
                        bool triggerNext = false;
                        int i = 0;
                        foreach (var item in stepTask)
                        {
                            if (triggerNext)
                            {
                                await CompleteStepTask(item.Id, NtsActionEnum.Submit);
                                break;
                            }
                            if (++i == stepTask.Count)
                            {
                                await CompleteStepTask(serviceId, NtsActionEnum.Complete);
                                break;
                            }

                            if (model.Id == item.Id)
                            {
                                triggerNext = true;
                            }

                        }
                    }

                }

            }


            return true;
        }
        public async Task<bool> AssignStepTask(string templateCodes, string parentId, string parentVersion, string ownerUserId = "")
        {
            var templateCode = templateCodes.Split(',');
            await CancelAllStepTask(parentId);
            int i = 0;
            foreach (var item in templateCode)
            {

                var model = await _taskBusiness.GetTaskDetails(new RecTaskViewModel { TemplateCode = item, ActiveUserId = _userContext.UserId, ReferenceTypeCode = ReferenceTypeEnum.NTS_Task, ReferenceTypeId = parentId });
                var service1 = await _taskBusiness.GetSingleById(parentId);
                if (service1.StartDate > DateTime.Now)
                {
                    model.StartDate = service1.StartDate;
                    model.DueDate = service1.StartDate.Value.AddDays(Convert.ToInt32(model.SLA));
                }
                model.Id = Guid.NewGuid().ToString();
                model.DataAction = DataActionEnum.Create;
                model.TemplateAction = i == 0 ? NtsActionEnum.Submit : NtsActionEnum.Draft;
                model.ActiveUserId = _userContext.UserId;
                model.AssigneeUserId = model.AssigneeUserId;
                //model.AssigneeTeamId = teamId.IsNotNullAndNotEmpty() ? teamId : model.AssigneeTeamId;
                model.AssignToType = model.AssigneeUserId.IsNotNullAndNotEmpty() ? AssignToTypeEnum.User : AssignToTypeEnum.Team;
                model.OwnerUserId = ownerUserId.IsNotNullAndNotEmpty() ? ownerUserId : _userContext.UserId;
                model.RequestedUserId = _userContext.UserId;
                if (parentVersion.IsNotNullAndNotEmpty())
                {
                    model.ParentVersionNo = parentVersion;
                }
                if (model.TemplateCode == "SCHEDULE_INTERVIEW_RECRUITER")
                {
                    var service = await _taskBusiness.GetSingleById(parentId);
                    if (service.IsNotNull())
                    {
                        model.DatePickerValue3 = service.DatePickerValue1;
                    }

                }
                var result = await Manage(model);
                i++;
            }

            return true;
        }
        public async Task<bool> CancelAllStepTask(string parentId)
        {

            //cancel all steps if it's not cancelled
            var tasklist = await _taskBusiness.GetStepTaskListByService(parentId);
            foreach (var task in tasklist)
            {

                var model = await _taskBusiness.GetTaskDetails(new RecTaskViewModel { Id = task.Id });
                model.DataAction = DataActionEnum.Edit;
                model.TemplateAction = NtsActionEnum.Cancel;
                var result = await Manage(model);
            }

            return true;
        }
        public async Task<bool> CompleteStepTask(string id, NtsActionEnum templateAction)
        {
            var model = await _taskBusiness.GetTaskDetails(new RecTaskViewModel { Id = id, ActiveUserId = _userContext.UserId });

            model.DataAction = DataActionEnum.Edit;
            if (model.TaskStatusCode == "REJECTED" || model.TaskStatusCode == "COMPLETED" || model.TaskStatusCode == "NOTAPPLICABLE")
            {
                model.TemplateAction = NtsActionEnum.EditAsNewVersion;
                model.VersionNo = model.VersionNo + 1;
            }
            else if (templateAction == NtsActionEnum.EditAsNewVersion)
            {
                model.TemplateAction = templateAction;
                model.VersionNo = model.VersionNo + 1;
            }
            else
            {
                model.TemplateAction = templateAction;
            }

            //model.ActiveUserId = _userContext.UserId;
            //model.AssigneeUserId = model.AssigneeUserId;
            ////model.AssigneeTeamId = teamId.IsNotNullAndNotEmpty() ? teamId : model.AssigneeTeamId;
            //model.AssignToType = model.AssigneeUserId.IsNotNullAndNotEmpty() ? AssignToTypeEnum.User : AssignToTypeEnum.Team;
            //model.OwnerUserId = _userContext.UserId;
            //model.RequestedUserId = _userContext.UserId;
            if (model.TemplateCode == "STAFF_CONFIRM_PROBATION_DATE_BY_HR" && model.ReferenceTypeId.IsNotNullAndNotEmpty() && model.TemplateAction == NtsActionEnum.Submit)
            {
                var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                var data = service.Where(x => x.TemplateCode == "STAFF_CONFIRM_PROBATION_DATE_BY_HM").FirstOrDefault();
                if (data.IsNotNull())
                {
                    model.DropdownValue1 = data.DropdownValue1;
                    model.DropdownDisplayValue1 = data.DropdownDisplayValue1;
                    model.DropdownValue2 = data.DropdownValue2;
                    model.DropdownDisplayValue2 = data.DropdownDisplayValue2;
                }
            }
            if (model.TemplateCode == "WORKER_CONFIRM_PROBATION_DATE_BY_HR" && model.ReferenceTypeId.IsNotNullAndNotEmpty() && model.TemplateAction == NtsActionEnum.Submit)
            {
                var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                var data = service.Where(x => x.TemplateCode == "WORKER_CONFIRM_PROBATION_DATE_BY_HM").FirstOrDefault();
                if (data.IsNotNull())
                {
                    model.DropdownValue1 = data.DropdownValue1;
                    model.DropdownDisplayValue1 = data.DropdownDisplayValue1;
                    model.DropdownValue2 = data.DropdownValue2;
                    model.DropdownDisplayValue2 = data.DropdownDisplayValue2;
                }
            }
            var result = await Manage(model);
            return true;
        }
        //public async Task<ActionResult> AssignService(string templateCode, ReferenceTypeEnum referenceTypeCode ,string referenceTypeId, string assignTo)
        //{
        //    var model = _taskBusiness.GetTaskDetails(new RecTaskViewModel { TemplateCode = templateCode, ActiveUserId = _userContext.UserId, ReferenceTypeCode = referenceTypeCode, ReferenceTypeId = referenceTypeId }).Result;

        //    model.Id = Guid.NewGuid().ToString();
        //    model.DataAction = DataActionEnum.Create;
        //    model.TemplateAction = NtsActionEnum.Submit;
        //    model.ActiveUserId = _userContext.UserId;
        //    model.AssigneeUserId = assignTo.IsNotNullAndNotEmpty() ? assignTo : model.AssigneeUserId;
        //    //model.AssigneeTeamId = teamId.IsNotNullAndNotEmpty() ? teamId : model.AssigneeTeamId;
        //    model.AssignToType = model.AssigneeUserId.IsNotNullAndNotEmpty() ? AssignToTypeEnum.User : AssignToTypeEnum.Team;
        //    model.OwnerUserId = _userContext.UserId;
        //    model.RequestedUserId = _userContext.UserId;
        //    var result = await Manage(model);
        //    return result;
        //}
        //public async Task<ActionResult> AssignTaskForJobAdvertisement(string referenceId, string assignTo)
        //{
        //    var model = _taskBusiness.GetTaskDetails(new RecTaskViewModel { TemplateCode = "JOB_ADVERTISEMENT_APPROVAL", ActiveUserId = _userContext.UserId, ReferenceTypeCode = ReferenceTypeEnum.REC_Application, ReferenceTypeId = referenceId }).Result;

        //    model.Id = Guid.NewGuid().ToString();
        //    model.DataAction = DataActionEnum.Create;
        //    model.TemplateAction = NtsActionEnum.Submit;
        //    model.ActiveUserId = _userContext.UserId;
        //    model.AssigneeUserId = assignTo.IsNotNullAndNotEmpty() ? assignTo : model.AssigneeUserId;
        //    //model.AssigneeTeamId = teamId.IsNotNullAndNotEmpty() ? teamId : model.AssigneeTeamId;
        //    model.AssignToType = model.AssigneeUserId.IsNotNullAndNotEmpty() ? AssignToTypeEnum.User : AssignToTypeEnum.Team;
        //    model.OwnerUserId = _userContext.UserId;
        //    model.RequestedUserId = _userContext.UserId;
        //    var result = await Manage(model);
        //    return result;
        //}
        public async Task<ActionResult> CreateService(string tempCode, string applicantId, string assignTo, ReferenceTypeEnum refTypeCode = ReferenceTypeEnum.REC_Application, string ownerUserId = "")
        {
            var model = await _taskBusiness.GetTaskDetails(new RecTaskViewModel { TemplateCode = tempCode, ActiveUserId = _userContext.UserId, ReferenceTypeCode = refTypeCode, ReferenceTypeId = applicantId });

            model.Id = Guid.NewGuid().ToString();
            model.DataAction = DataActionEnum.Create;
            model.TemplateAction = NtsActionEnum.Submit;
            model.ActiveUserId = _userContext.UserId;
            model.AssigneeUserId = assignTo.IsNotNullAndNotEmpty() ? assignTo : model.AssigneeUserId;
            //model.AssigneeTeamId = teamId.IsNotNullAndNotEmpty() ? teamId : model.AssigneeTeamId;
            model.AssignToType = model.AssigneeUserId.IsNotNullAndNotEmpty() ? AssignToTypeEnum.User : AssignToTypeEnum.Team;
            model.OwnerUserId = ownerUserId.IsNotNullAndNotEmpty() ? ownerUserId : _userContext.UserId;
            model.RequestedUserId = _userContext.UserId;

            var result = await Manage(model);
            return result;
        }
        public async Task<ActionResult> AssignTaskWaitlistByHM(string applicantId, string assignTo)
        {
            var model = await _taskBusiness.GetTaskDetails(new RecTaskViewModel { TemplateCode = "INTERVIEW_WAITLIST_HR", ActiveUserId = _userContext.UserId, ReferenceTypeCode = ReferenceTypeEnum.REC_Application, ReferenceTypeId = applicantId });

            model.Id = Guid.NewGuid().ToString();
            model.DataAction = DataActionEnum.Create;
            model.TemplateAction = NtsActionEnum.Submit;
            model.ActiveUserId = _userContext.UserId;
            model.AssigneeUserId = assignTo.IsNotNullAndNotEmpty() ? assignTo : model.AssigneeUserId;
            //model.AssigneeTeamId = teamId.IsNotNullAndNotEmpty() ? teamId : model.AssigneeTeamId;
            model.AssignToType = model.AssigneeUserId.IsNotNullAndNotEmpty() ? AssignToTypeEnum.User : AssignToTypeEnum.Team;
            model.OwnerUserId = _userContext.UserId;
            model.RequestedUserId = _userContext.UserId;
            var result = await Manage(model);
            return result;
        }
        public async Task<ActionResult> AssignTaskForMedical(string applicantId, string assignTo)
        {
            var model = await _taskBusiness.GetTaskDetails(new RecTaskViewModel { TemplateCode = "MEDICAL_TEST", ActiveUserId = _userContext.UserId, ReferenceTypeCode = ReferenceTypeEnum.REC_Application, ReferenceTypeId = applicantId });

            model.Id = Guid.NewGuid().ToString();
            model.DataAction = DataActionEnum.Create;
            model.TemplateAction = NtsActionEnum.Submit;
            model.ActiveUserId = _userContext.UserId;
            model.AssigneeUserId = assignTo.IsNotNullAndNotEmpty() ? assignTo : model.AssigneeUserId;
            //model.AssigneeTeamId = teamId.IsNotNullAndNotEmpty() ? teamId : model.AssigneeTeamId;
            model.AssignToType = model.AssigneeUserId.IsNotNullAndNotEmpty() ? AssignToTypeEnum.User : AssignToTypeEnum.Team;
            model.OwnerUserId = _userContext.UserId;
            model.RequestedUserId = _userContext.UserId;
            var result = await Manage(model);
            return result;
        }
        public async Task<ActionResult> AssignTaskForVisaAppointment(string applicantId, string assignTo)
        {
            var model = await _taskBusiness.GetTaskDetails(new RecTaskViewModel { TemplateCode = "VISA_APPOINTMENT", ActiveUserId = _userContext.UserId, ReferenceTypeCode = ReferenceTypeEnum.REC_Application, ReferenceTypeId = applicantId });

            model.Id = Guid.NewGuid().ToString();
            model.DataAction = DataActionEnum.Create;
            model.TemplateAction = NtsActionEnum.Submit;
            model.ActiveUserId = _userContext.UserId;
            model.AssigneeUserId = assignTo.IsNotNullAndNotEmpty() ? assignTo : model.AssigneeUserId;
            //model.AssigneeTeamId = teamId.IsNotNullAndNotEmpty() ? teamId : model.AssigneeTeamId;
            model.AssignToType = model.AssigneeUserId.IsNotNullAndNotEmpty() ? AssignToTypeEnum.User : AssignToTypeEnum.Team;
            model.OwnerUserId = _userContext.UserId;
            model.RequestedUserId = _userContext.UserId;
            var result = await Manage(model);
            return result;
        }

        public async Task<ActionResult> AssignTaskToAdminforAccomadation(string refId, string assignTo)
        {
            var model = await _taskBusiness.GetTaskDetails(new RecTaskViewModel { TemplateCode = "ARRANGE_ACCOMMODATION", ActiveUserId = _userContext.UserId, ReferenceTypeCode = ReferenceTypeEnum.NTS_Task, ReferenceTypeId = refId });
            var service = await _taskBusiness.GetSingleById(refId);
            model.Id = Guid.NewGuid().ToString();
            model.DataAction = DataActionEnum.Create;
            model.TemplateAction = NtsActionEnum.Submit;
            model.ActiveUserId = _userContext.UserId;
            model.AssigneeUserId = assignTo.IsNotNullAndNotEmpty() ? assignTo : model.AssigneeUserId;
            //model.AssigneeTeamId = teamId.IsNotNullAndNotEmpty() ? teamId : model.AssigneeTeamId;
            model.AssignToType = model.AssigneeUserId.IsNotNullAndNotEmpty() ? AssignToTypeEnum.User : AssignToTypeEnum.Team;
            model.OwnerUserId = _userContext.UserId;
            model.RequestedUserId = _userContext.UserId;
            model.ParentVersionNo = service.VersionNo.ToString();
            var result = await Manage(model);
            return result;
        }

        public async Task<ActionResult> AssignTaskToPlantforPickup(string refId, string assignTo)
        {
            var model = await _taskBusiness.GetTaskDetails(new RecTaskViewModel { TemplateCode = "ARRANGE_VEHICLE_PICKUP", ActiveUserId = _userContext.UserId, ReferenceTypeCode = ReferenceTypeEnum.NTS_Task, ReferenceTypeId = refId });
            var service = await _taskBusiness.GetSingleById(refId);
            model.Id = Guid.NewGuid().ToString();
            model.DataAction = DataActionEnum.Create;
            model.TemplateAction = NtsActionEnum.Submit;
            model.ActiveUserId = _userContext.UserId;
            model.AssigneeUserId = assignTo.IsNotNullAndNotEmpty() ? assignTo : model.AssigneeUserId;
            //model.AssigneeTeamId = teamId.IsNotNullAndNotEmpty() ? teamId : model.AssigneeTeamId;
            model.AssignToType = model.AssigneeUserId.IsNotNullAndNotEmpty() ? AssignToTypeEnum.User : AssignToTypeEnum.Team;
            model.OwnerUserId = _userContext.UserId;
            model.RequestedUserId = _userContext.UserId;
            model.ParentVersionNo = service.VersionNo.ToString();
            var result = await Manage(model);
            return result;
        }


        public async Task<ActionResult> AssignTaskForBiometricCompletion(string applicantId, string assignTo)
        {
            var model = await _taskBusiness.GetTaskDetails(new RecTaskViewModel { TemplateCode = "BIOMETRIC_COMPLETION", ActiveUserId = _userContext.UserId, ReferenceTypeCode = ReferenceTypeEnum.REC_Application, ReferenceTypeId = applicantId });

            model.Id = Guid.NewGuid().ToString();
            model.DataAction = DataActionEnum.Create;
            model.TemplateAction = NtsActionEnum.Submit;
            model.ActiveUserId = _userContext.UserId;
            model.AssigneeUserId = assignTo.IsNotNullAndNotEmpty() ? assignTo : model.AssigneeUserId;
            //model.AssigneeTeamId = teamId.IsNotNullAndNotEmpty() ? teamId : model.AssigneeTeamId;
            model.AssignToType = model.AssigneeUserId.IsNotNullAndNotEmpty() ? AssignToTypeEnum.User : AssignToTypeEnum.Team;
            model.OwnerUserId = _userContext.UserId;
            model.RequestedUserId = _userContext.UserId;
            var result = await Manage(model);
            return result;
        }
        public async Task<ActionResult> AssignTaskForVisaApproval(string applicantId, string assignTo)
        {
            var model = await _taskBusiness.GetTaskDetails(new RecTaskViewModel { TemplateCode = "VISA_APPROVAL", ActiveUserId = _userContext.UserId, ReferenceTypeCode = ReferenceTypeEnum.REC_Application, ReferenceTypeId = applicantId });

            model.Id = Guid.NewGuid().ToString();
            model.DataAction = DataActionEnum.Create;
            model.TemplateAction = NtsActionEnum.Submit;
            model.ActiveUserId = _userContext.UserId;
            model.AssigneeUserId = assignTo.IsNotNullAndNotEmpty() ? assignTo : model.AssigneeUserId;
            //model.AssigneeTeamId = teamId.IsNotNullAndNotEmpty() ? teamId : model.AssigneeTeamId;
            model.AssignToType = model.AssigneeUserId.IsNotNullAndNotEmpty() ? AssignToTypeEnum.User : AssignToTypeEnum.Team;
            model.OwnerUserId = _userContext.UserId;
            model.RequestedUserId = _userContext.UserId;
            var result = await Manage(model);
            return result;
        }
        public async Task<ActionResult> AssignTaskForVisaSentToCandidate(string applicantId, string assignTo)
        {
            var model = await _taskBusiness.GetTaskDetails(new RecTaskViewModel { TemplateCode = "VISA_TO_CANDIDATE", ActiveUserId = _userContext.UserId, ReferenceTypeCode = ReferenceTypeEnum.REC_Application, ReferenceTypeId = applicantId });

            model.Id = Guid.NewGuid().ToString();
            model.DataAction = DataActionEnum.Create;
            model.TemplateAction = NtsActionEnum.Submit;
            model.ActiveUserId = _userContext.UserId;
            model.AssigneeUserId = assignTo.IsNotNullAndNotEmpty() ? assignTo : model.AssigneeUserId;
            //model.AssigneeTeamId = teamId.IsNotNullAndNotEmpty() ? teamId : model.AssigneeTeamId;
            model.AssignToType = model.AssigneeUserId.IsNotNullAndNotEmpty() ? AssignToTypeEnum.User : AssignToTypeEnum.Team;
            model.OwnerUserId = _userContext.UserId;
            model.RequestedUserId = _userContext.UserId;
            var result = await Manage(model);
            return result;
        }
        public async Task<ActionResult> AssignTaskForFlightTicketBooking(string applicantId, string assignTo)
        {
            var model = await _taskBusiness.GetTaskDetails(new RecTaskViewModel { TemplateCode = "FLIGHT_TICKET_BOOKING", ActiveUserId = _userContext.UserId, ReferenceTypeCode = ReferenceTypeEnum.REC_Application, ReferenceTypeId = applicantId });

            model.Id = Guid.NewGuid().ToString();
            model.DataAction = DataActionEnum.Create;
            model.TemplateAction = NtsActionEnum.Submit;
            model.ActiveUserId = _userContext.UserId;
            model.AssigneeUserId = assignTo.IsNotNullAndNotEmpty() ? assignTo : model.AssigneeUserId;
            //model.AssigneeTeamId = teamId.IsNotNullAndNotEmpty() ? teamId : model.AssigneeTeamId;
            model.AssignToType = model.AssigneeUserId.IsNotNullAndNotEmpty() ? AssignToTypeEnum.User : AssignToTypeEnum.Team;
            model.OwnerUserId = _userContext.UserId;
            model.RequestedUserId = _userContext.UserId;
            var result = await Manage(model);
            return result;
        }
        public async Task<ActionResult> AssignTaskForCandiadteArrival(string applicantId, string assignTo)
        {
            var model = await _taskBusiness.GetTaskDetails(new RecTaskViewModel { TemplateCode = "CANDIDATE_ARRIVAL", ActiveUserId = _userContext.UserId, ReferenceTypeCode = ReferenceTypeEnum.REC_Application, ReferenceTypeId = applicantId });

            model.Id = Guid.NewGuid().ToString();
            model.DataAction = DataActionEnum.Create;
            model.TemplateAction = NtsActionEnum.Submit;
            model.ActiveUserId = _userContext.UserId;
            model.AssigneeUserId = assignTo.IsNotNullAndNotEmpty() ? assignTo : model.AssigneeUserId;
            //model.AssigneeTeamId = teamId.IsNotNullAndNotEmpty() ? teamId : model.AssigneeTeamId;
            model.AssignToType = model.AssigneeUserId.IsNotNullAndNotEmpty() ? AssignToTypeEnum.User : AssignToTypeEnum.Team;
            model.OwnerUserId = _userContext.UserId;
            model.RequestedUserId = _userContext.UserId;
            var result = await Manage(model);
            return result;
        }
        public async Task<ActionResult> AssignTaskForCandiadteJoining(string applicantId, string assignTo)
        {
            var model = await _taskBusiness.GetTaskDetails(new RecTaskViewModel { TemplateCode = "CANDIDATE_JOINING", ActiveUserId = _userContext.UserId, ReferenceTypeCode = ReferenceTypeEnum.REC_Application, ReferenceTypeId = applicantId });

            model.Id = Guid.NewGuid().ToString();
            model.DataAction = DataActionEnum.Create;
            model.TemplateAction = NtsActionEnum.Submit;
            model.ActiveUserId = _userContext.UserId;
            model.AssigneeUserId = assignTo.IsNotNullAndNotEmpty() ? assignTo : model.AssigneeUserId;
            //model.AssigneeTeamId = teamId.IsNotNullAndNotEmpty() ? teamId : model.AssigneeTeamId;
            model.AssignToType = model.AssigneeUserId.IsNotNullAndNotEmpty() ? AssignToTypeEnum.User : AssignToTypeEnum.Team;
            model.OwnerUserId = _userContext.UserId;
            model.RequestedUserId = _userContext.UserId;
            var result = await Manage(model);
            return result;
        }
        public async Task<ActionResult> AssignTaskForAppointment(string applicantId, string assignTo)
        {
            var model = await _taskBusiness.GetTaskDetails(new RecTaskViewModel { TemplateCode = "EMPLOYEE_APPOINTMENT", ActiveUserId = _userContext.UserId, ReferenceTypeCode = ReferenceTypeEnum.REC_Application, ReferenceTypeId = applicantId });

            model.Id = Guid.NewGuid().ToString();
            model.DataAction = DataActionEnum.Create;
            model.TemplateAction = NtsActionEnum.Submit;
            model.ActiveUserId = _userContext.UserId;
            model.AssigneeUserId = assignTo.IsNotNullAndNotEmpty() ? assignTo : model.AssigneeUserId;
            //model.AssigneeTeamId = teamId.IsNotNullAndNotEmpty() ? teamId : model.AssigneeTeamId;
            model.AssignToType = model.AssigneeUserId.IsNotNullAndNotEmpty() ? AssignToTypeEnum.User : AssignToTypeEnum.Team;
            model.OwnerUserId = _userContext.UserId;
            model.RequestedUserId = _userContext.UserId;
            model.TextValue1 = "test 1";
            model.TextValue2 = "test 2";
            model.TextValue3 = "test 3";
            model.TextValue4 = "test 4";
            model.TextValue5 = "test 5";
            model.TextValue6 = "test 6";
            model.TextValue7 = "test 7";
            model.TextValue8 = "test 8";
            model.TextValue9 = "test 9";
            model.TextValue10 = "test 10";
            var result = await Manage(model);
            return result;
        }

        [HttpPost]
        public async Task<IActionResult> SaveTaskAttachment(IList<IFormFile> file, string referenceTypeId, ReferenceTypeEnum referenceTypeCode)
        {
            try
            {
                foreach (var f in file)
                {
                    var ms = new MemoryStream();
                    f.OpenReadStream().CopyTo(ms);
                    var result = await _fileBusiness.Create(new FileViewModel
                    {
                        ContentByte = ms.ToArray(),
                        ContentType = f.ContentType,
                        ContentLength = f.Length,
                        FileName = f.FileName,
                        ReferenceTypeId = referenceTypeId,
                        ReferenceTypeCode = referenceTypeCode,
                        FileExtension = Path.GetExtension(f.FileName)
                    }
                    );
                    if (result.IsSuccess)
                    {
                        Response.Headers.Add("fileId", result.Item.Id);
                        Response.Headers.Add("fileName", result.Item.FileName);
                        return Json(new { success = true, fileId = result.Item.Id, filename = result.Item.FileName });
                    }
                    else
                    {
                        Response.Headers.Add("status", "false");
                        return Content("");
                    }



                }
            }
            catch (Exception ex)
            {

            }
            return Content("");
        }
        [HttpPost]
        public async Task<IActionResult> SaveAttachment(IList<IFormFile> files)
        {
            try
            {
                foreach (var file in files)
                {
                    var ms = new MemoryStream();
                    file.OpenReadStream().CopyTo(ms);
                    var result = await _fileBusiness.Create(new FileViewModel
                    {
                        ContentByte = ms.ToArray(),
                        ContentType = file.ContentType,
                        ContentLength = file.Length,
                        FileName = file.FileName,
                        FileExtension = Path.GetExtension(file.FileName)
                    }
                    );
                    if (result.IsSuccess)
                    {
                        Response.Headers.Add("fileId", result.Item.Id);
                        Response.Headers.Add("fileName", result.Item.FileName);
                        return Json(new { success = true, fileId = result.Item.Id, filename = result.Item.FileName });
                    }
                    else
                    {
                        Response.Headers.Add("status", "false");
                        return Content("");
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Content("");
        }
        public async Task<ActionResult> GetTaskAttachmentList(string taskId)
        {
            var list = await _fileBusiness.GetList(x => x.ReferenceTypeId == taskId);
            return Json(list);
        }

        public async Task<NotificationViewModel> CreateTaskNotification(RecTaskViewModel model)
        {
            try
            {
                var notificationModel = new NotificationViewModel();
                var userModel = await _userBusiness.GetSingleById(model.AssigneeUserId);
                var ownerModel = await _userBusiness.GetSingleById(model.OwnerUserId);
                var assigntype = await _taskTemplateBusiness.GetSingle(x => x.TemplateCode == model.TemplateCode);
                var takeActionLink = /*_configuration.GetValue<string>("ApplicationBaseUrl") +*/ "/CMS/task/Index?taskId=" + model.Id + "&assignTo=" + model.AssigneeUserId + "&teamId=" + model.AssigneeTeamId + "&isPopUp=" + true;
                var ActionLink = $@"{_webApi.GetHost()}portal/Recruitment?page=TaskList";
                if (assigntype.AssignToType == AssignToTypeEnum.Candidate)
                {
                    ActionLink = $@"{_webApi.GetHost()}portal/CareerPortal?page=TaskList";
                }
                var refType = ReferenceTypeEnum.NTS_Task;
                if (model.NtsType == NtsTypeEnum.Service)
                {
                    refType = ReferenceTypeEnum.NTS_Service;
                }
                notificationModel.ReferenceType = refType;
                notificationModel.ReferenceTypeId = model.Id;
                notificationModel.ReferenceTemplateCode = model.TemplateCode;
                notificationModel.IsIncludeAttachment = model.IsIncludeEmailAttachment;
                if (model.EmailSettingId.IsNotNullAndNotEmpty())
                {
                    notificationModel.ShowOriginalSender = true;
                }
                switch (model.NtsType)
                {
                    case NtsTypeEnum.Service:
                        var serviceTakeActionLink = /*_configuration.GetValue<string>("ApplicationBaseUrl") +*/
                            "/CMS/task/Service?taskId=" + model.Id + "&assignTo=" + model.AssigneeUserId + "&teamId=" + model.AssigneeTeamId + "&isPopUp=" + true;

                        if (model.TaskStatusCode == "INPROGRESS")
                        {
                            notificationModel.Subject = "Submitted Service | " + DateTime.Now + " | " + model.Subject + " | " + ownerModel.Email;
                            notificationModel.Body = string.Concat("<div><h4>Hello ", userModel.Name, "</h4>Service Details:<br><br>Service No: ", model.TaskNo,
                                                        "<br>Subject : ", model.Subject, "<br><br>",
                                                        await GetTaskUdf(model),
                                                        model.TextBoxDisplay1.IsNotNullAndNotEmpty() ? model.TextBoxDisplay1 + " : " + (model.TextBoxDisplayType1 == NtsFieldType.NTS_Attachment ? model.AttachmentValue1 : ((model.TextBoxDisplayType1 == NtsFieldType.NTS_DatePicker || model.TextBoxDisplayType1 == NtsFieldType.NTS_DateTimePicker) ? model.DatePickerValue1 : model.TextValue1)) : model.TextValue1,
                                                        "<a href = '#' onclick='openNotificationAction(\"" + serviceTakeActionLink + "\")' > Take Action </a></div> ");
                            notificationModel.EmailBody = string.Concat("<div><h4>Hello ", userModel.Name, "</h4>Service Details:<br><br>Service No: ", model.TaskNo,
                                                       "<br>Subject : ", model.Subject, "<br><br>",
                                                       await GetTaskUdf(model),
                                                       model.TextBoxDisplay1.IsNotNullAndNotEmpty() ? model.TextBoxDisplay1 + " : " + (model.TextBoxDisplayType1 == NtsFieldType.NTS_Attachment ? model.AttachmentValue1 : ((model.TextBoxDisplayType1 == NtsFieldType.NTS_DatePicker || model.TextBoxDisplayType1 == NtsFieldType.NTS_DateTimePicker) ? model.DatePickerValue1 : model.TextValue1)) : model.TextValue1
                                                       );
                            notificationModel.From = ownerModel.Email;
                            notificationModel.ToUserId = model.AssigneeUserId;
                            notificationModel.To = userModel.Email;
                        }
                        else if (model.TaskStatusCode == "OVERDUE")
                        {
                            notificationModel.Subject = "Overdue Service | " + DateTime.Now + " | " + model.Subject + " | " + ownerModel.Email;
                            //notificationModel.Subject = model.Subject;
                            notificationModel.Body = string.Concat("<div><h4>Hello ", userModel.Name, "</h4>Service Details:<br><br>Service No: ", model.TaskNo,
                                                        "<br>Subject : ", model.Subject, "<br><br>",
                                                        await GetTaskUdf(model),
                                                        "<a href = '#' onclick='openNotificationAction(\"" + serviceTakeActionLink + "\")' > Take Action </a></div> ");
                            notificationModel.EmailBody = string.Concat("<div><h4>Hello ", userModel.Name, "</h4>Service Details:<br><br>Service No: ", model.TaskNo,
                                                       "<br>Subject : ", model.Subject, "<br><br>",
                                                       await GetTaskUdf(model),
                                                       " ");
                            notificationModel.From = ownerModel.Email;
                            notificationModel.ToUserId = model.AssigneeUserId;
                            notificationModel.To = userModel.Email;
                        }
                        else if (model.TaskStatusCode == "COMPLETED")
                        {
                            notificationModel.Subject = "Completed Service | " + DateTime.Now + " | " + model.Subject + " | " + ownerModel.Email;
                            //notificationModel.Subject = model.Subject;
                            notificationModel.Body = string.Concat("<div><h4>Hello ", ownerModel.Name, "</h4>Service Details:<br><br>Service No: ", model.TaskNo,
                                                        "<br>Subject : ", model.Subject, "<br><br>",
                                                        await GetTaskUdf(model),
                                                        "<a href = '#' onclick='openNotificationAction(\"" + serviceTakeActionLink + "\")' > Take Action </a></div> ");
                            notificationModel.EmailBody = string.Concat("<div><h4>Hello ", ownerModel.Name, "</h4>Service Details:<br><br>Service No: ", model.TaskNo,
                                                      "<br>Subject : ", model.Subject, "<br><br>",
                                                      await GetTaskUdf(model),
                                                      " ");
                            notificationModel.From = userModel.Email;
                            notificationModel.ToUserId = model.OwnerUserId;
                            notificationModel.To = ownerModel.Email;
                        }
                        else if (model.TaskStatusCode == "CANCELLED")
                        {
                            notificationModel.Subject = "Cancelled Service | " + DateTime.Now + " | " + model.Subject + " | " + ownerModel.Email;
                            //notificationModel.Subject = model.Subject;
                            notificationModel.Body = string.Concat("<div><h4>Hello ", ownerModel.Name, "</h4>Service Details:<br><br>Service No: ", model.TaskNo,
                                                        "<br>Subject : ", model.Subject, "<br><br>",
                                                        await GetTaskUdf(model),
                                                        "<a href ='#' onclick='openNotificationAction(\"" + serviceTakeActionLink + "\")' > Take Action </a></div> ");
                            notificationModel.EmailBody = string.Concat("<div><h4>Hello ", ownerModel.Name, "</h4>Service Details:<br><br>Service No: ", model.TaskNo,
                                                      "<br>Subject : ", model.Subject, "<br><br>",
                                                      await GetTaskUdf(model),
                                                      " ");
                            notificationModel.From = userModel.Email;
                            notificationModel.ToUserId = model.OwnerUserId;
                            notificationModel.To = ownerModel.Email;
                        }
                        else if (model.TaskStatusCode == "CLOSED")
                        {
                            notificationModel.Subject = "Cancelled Service | " + DateTime.Now + " | " + model.Subject + " | " + ownerModel.Email;
                            //notificationModel.Subject = model.Subject;
                            notificationModel.Body = string.Concat("<div><h4>Hello ", ownerModel.Name, "</h4>Service Details:<br><br>Service No: ", model.TaskNo,
                                                        "<br>Subject : ", model.Subject, "<br><br>",
                                                       await GetTaskUdf(model),
                                                        "<a href ='#' onclick='openNotificationAction(\"" + serviceTakeActionLink + "\")' > Take Action </a></div> ");
                            notificationModel.EmailBody = string.Concat("<div><h4>Hello ", ownerModel.Name, "</h4>Service Details:<br><br>Service No: ", model.TaskNo,
                                                      "<br>Subject : ", model.Subject, "<br><br>",
                                                     await GetTaskUdf(model),
                                                      " ");
                            notificationModel.From = userModel.Email;
                            notificationModel.ToUserId = model.OwnerUserId;
                            notificationModel.To = ownerModel.Email;
                        }
                        break;
                    case NtsTypeEnum.Task:
                        if (model.TaskStatusCode == "INPROGRESS")
                        {
                            notificationModel.Subject = "Assigned Task | " + DateTime.Now + " | " + model.Subject + " | " + userModel.Email;
                            //notificationModel.Subject = model.Subject;
                            notificationModel.Body = string.Concat("<div><h5>Hello ", userModel.Name, "</h5>Task Details:<br><br>Task No: ", model.TaskNo,
                                                        "<br>Subject : ", model.Subject, "<br><br>",
                                                        await GetTaskUdf(model),
                                                        "<a href = '#' onclick='openNotificationAction(\"" + takeActionLink + "\")'> Take Action </a></div> ");
                            notificationModel.EmailBody = string.Concat("<div><h5>Hello ", userModel.Name, "</h5>Task Details:<br><br>Task No: ", model.TaskNo,
                                                       "<br>Subject : ", model.Subject, "<br><br>",
                                                       await GetTaskUdf(model),
                                                       "<a href = '" + ActionLink + "' target='_blank'> Take Action </a></div> ");
                            notificationModel.From = ownerModel.Email;
                            notificationModel.ToUserId = model.AssigneeUserId;
                            notificationModel.To = userModel.Email;
                            if (model.TemplateCode == "SL_BATCH_SEND_HM")
                            {
                                notificationModel.Subject = "Assigned Task | " + DateTime.Now + " | " + model.Subject + " | " + userModel.Email;
                                //notificationModel.Subject = model.Subject;
                                notificationModel.Body = string.Concat("<div><h5>Hello ", userModel.Name, "</h5>Task Details:<br><br>Task No: ", model.TaskNo,
                                                            "<br>Subject : ", model.Subject, "<br><br>",
                                                            await GetTaskUdf(model),
                                                            "<a href = '#' onclick='openHMBatch()'> Take Action </a></div> ");
                                notificationModel.EmailBody = string.Concat("<div><h5>Hello ", userModel.Name, "</h5>Task Details:<br><br>Task No: ", model.TaskNo,
                                                            "<br>Subject : ", model.Subject, "<br><br>",
                                                            await GetTaskUdf(model),
                                                            "<a href = '" + ActionLink + "' target='_blank'> Take Action </a></div> ");
                                notificationModel.From = ownerModel.Email;
                                notificationModel.ToUserId = model.AssigneeUserId;
                                notificationModel.To = userModel.Email;
                            }
                        }
                        else if (model.TaskStatusCode == "OVERDUE")
                        {
                            notificationModel.Subject = "Overdue Task | " + DateTime.Now + " | " + model.Subject + " | " + userModel.Email;
                            //notificationModel.Subject = model.Subject;
                            notificationModel.Body = string.Concat("<div><h4>Hello ", userModel.Name, "</h4>Task Details:<br><br>Task No: ", model.TaskNo,
                                                        "<br>Subject : ", model.Subject, "<br><br>",
                                                        await GetTaskUdf(model),
                                                        "<a href =  '#' onclick='openNotificationAction(\"" + takeActionLink + "\")' > Take Action </a></div> ");
                            notificationModel.EmailBody = string.Concat("<div><h4>Hello ", userModel.Name, "</h4>Task Details:<br><br>Task No: ", model.TaskNo,
                                                        "<br>Subject : ", model.Subject, "<br><br>",
                                                        await GetTaskUdf(model),
                                                        "<a href = '" + ActionLink + "' target='_blank'> Take Action </a></div> ");
                            notificationModel.From = ownerModel.Email;
                            notificationModel.ToUserId = model.AssigneeUserId;
                            notificationModel.To = userModel.Email;
                        }
                        else if (model.TaskStatusCode == "COMPLETED")
                        {
                            notificationModel.Subject = "Completed Task | " + DateTime.Now + " | " + model.Subject + " | " + userModel.Email;
                            //notificationModel.Subject = model.Subject;
                            notificationModel.Body = string.Concat("<div><h4>Hello ", ownerModel.Name, "</h4>Task Details:<br><br>Task No: ", model.TaskNo,
                                                        "<br>Subject : ", model.Subject, "<br><br>",
                                                        await GetTaskUdf(model),
                                                        "<a href =  '#' onclick='openNotificationAction(\"" + takeActionLink + "\")'> Take Action </a></div> ");
                            notificationModel.EmailBody = string.Concat("<div><h4>Hello ", ownerModel.Name, "</h4>Task Details:<br><br>Task No: ", model.TaskNo,
                                                        "<br>Subject : ", model.Subject, "<br><br>",
                                                        await GetTaskUdf(model),
                                                        " ");
                            notificationModel.From = userModel.Email;
                            notificationModel.ToUserId = model.OwnerUserId;
                            notificationModel.To = ownerModel.Email;
                            //if (model.TemplateCode == "SL_BATCH_SEND_HM")
                            //{
                            //    notificationModel.Subject = "Completed Task | " + DateTime.Now + " | " + model.Subject + " | " + userModel.Email;
                            //    //notificationModel.Subject = model.Subject;
                            //    notificationModel.Body = string.Concat("<div><h5>Hello ", userModel.Name, "</h5>Task Details:<br><br>Task No: ", model.TaskNo,
                            //                                "<br>Subject : ", model.Subject, "<br><br>",
                            //                                await GetTaskUdf(model),
                            //                                "<a href = '#' onclick='openHMBatch()'> Take Action </a></div> ");
                            //    notificationModel.From = ownerModel.Email;
                            //    notificationModel.ToUserId = model.AssigneeUserId;
                            //    notificationModel.To = userModel.Email;
                            //}
                            //if (model.TemplateCode == "BOOK_TICKET_ATTACH")
                            //{
                            //    notificationModel.Subject = "Completed Task|" + DateTime.Now + "|" + string.Concat(model.Subject, "-", model.TextValue3) + "|" + userModel.Email;
                            //    notificationModel.Body = string.Concat("<div><h4>Hello ", ownerModel.Name, "</h4>Task Details:<br><br>Task No: ", model.TaskNo,
                            //                                "<br>Subject : ", notificationModel.Subject, "<br><br>",
                            //                                "<a href = '#' onclick='openNotificationAction(\"" + takeActionLink + "\")' > Take Action </a></div> ");
                            //    notificationModel.From = userModel.Email;
                            //    notificationModel.ToUserDisplay = model.TextValue2;//take for udf to email
                            //    notificationModel.To = model.TextValue2;//take from udf
                            //}
                        }
                        else if (model.TaskStatusCode == "REJECTED")
                        {
                            notificationModel.Subject = "Rejected Task | " + DateTime.Now + " | " + model.Subject + " | " + userModel.Email;
                            //notificationModel.Subject = model.Subject;
                            var subject = model.Subject;
                            if (model.TemplateCode == "SCHEDULE_INTERVIEW_CANDIDATE")
                            {
                                notificationModel.Subject = "Rejected Task | " + DateTime.Now + " | " + string.Concat(model.Subject, "-", model.TextValue3) + " | " + userModel.Email;
                                subject = notificationModel.Subject;
                            }
                            if (model.TemplateCode == "INTENT_TO_OFFER")
                            {
                                notificationModel.Subject = "Rejected Task | " + DateTime.Now + " | " + string.Concat(model.Subject, "-", model.TextValue2) + " | " + userModel.Email;
                                subject = notificationModel.Subject;
                            }
                            notificationModel.Body = string.Concat("<div><h4>Hello ", ownerModel.Name, "</h4>Task Details:<br><br>Task No: ", model.TaskNo,
                                                        "<br>Subject : ", subject, "<br><br>",
                                                        await GetTaskUdf(model),
                                                        "<a href =  '#' onclick='openNotificationAction(\"" + takeActionLink + "\")' > Take Action </a></div> ");
                            notificationModel.EmailBody = string.Concat("<div><h4>Hello ", ownerModel.Name, "</h4>Task Details:<br><br>Task No: ", model.TaskNo,
                                                       "<br>Subject : ", subject, "<br><br>",
                                                       await GetTaskUdf(model),
                                                       " ");
                            notificationModel.From = userModel.Email;
                            notificationModel.ToUserId = model.OwnerUserId;
                            notificationModel.To = ownerModel.Email;

                        }
                        else if (model.TaskStatusCode == "CANCELLED")
                        {
                            notificationModel.Subject = "Cancelled Task | " + DateTime.Now + " | " + model.Subject + " | " + userModel.Email;
                            notificationModel.Subject = model.Subject;
                            notificationModel.Body = string.Concat("<div><h4>Hello ", ownerModel.Name, "</h4>Task Details:<br><br>Task No: ", model.TaskNo,
                                                        "<br>Subject : ", model.Subject, "<br><br>",
                                                        await GetTaskUdf(model),
                                                        "<a href =  '#' onclick='openNotificationAction(\"" + takeActionLink + "\")' > Take Action </a></div> ");
                            notificationModel.EmailBody = string.Concat("<div><h4>Hello ", ownerModel.Name, "</h4>Task Details:<br><br>Task No: ", model.TaskNo,
                                                        "<br>Subject : ", model.Subject, "<br><br>",
                                                        await GetTaskUdf(model),
                                                        "<a href = '" + ActionLink + "' target='_blank'> Take Action </a></div> ");
                            notificationModel.From = userModel.Email;
                            notificationModel.ToUserId = model.OwnerUserId;
                            notificationModel.To = ownerModel.Email;
                        }
                        else if (model.TaskStatusCode == "CLOSED")
                        {
                            notificationModel.Subject = "Cancelled Task | " + DateTime.Now + " | " + model.Subject + " | " + userModel.Email;
                            //notificationModel.Subject = model.Subject;
                            notificationModel.Body = string.Concat("<div><h4>Hello ", ownerModel.Name, "</h4>Task Details:<br><br>Task No: ", model.TaskNo,
                                                        "<br>Subject : ", model.Subject, "<br><br>",
                                                       await GetTaskUdf(model),
                                                        "<a href =  '#' onclick='openNotificationAction(\"" + takeActionLink + "\")' > Take Action </a></div> ");
                            notificationModel.EmailBody = string.Concat("<div><h4>Hello ", ownerModel.Name, "</h4>Task Details:<br><br>Task No: ", model.TaskNo,
                                                        "<br>Subject : ", model.Subject, "<br><br>",
                                                       await GetTaskUdf(model),
                                                        "<a href='" + ActionLink + "' target='_blank'> Take Action </a></div> ");
                            notificationModel.From = userModel.Email;
                            notificationModel.ToUserId = model.OwnerUserId;
                            notificationModel.To = ownerModel.Email;
                        }
                        break;
                    default:
                        break;
                }

                //notificationModel.To = "arshad@extranet.ae;mthamil107@gmail.com";

                var result = await _notiificationBusiness.Create(notificationModel);
                if (result.IsSuccess)
                {
                    try
                    {
                        BackgroundJob.Enqueue<Synergy.App.Business.HangfireScheduler>(x => x.SendEmailUsingHangfire(result.Item.EmailUniqueId));
                    }
                    catch (Exception e)
                    {

                    }
                    return result.Item;

                }
                return notificationModel;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<NotificationViewModel> SendExternalTaskNotification(RecTaskViewModel model)
        {
            try
            {
                var notificationModel = new NotificationViewModel();
                var userModel = await _userBusiness.GetSingleById(model.AssigneeUserId);
                var ownerModel = await _userBusiness.GetSingleById(model.OwnerUserId);
                //var takeActionLink = /*_configuration.GetValue<string>("ApplicationBaseUrl") +*/ "/CMS/task/Index?taskId=" + model.Id + "&assignTo=" + model.AssigneeUserId + "&teamId=" + model.AssigneeTeamId + "&isPopUp=" + true;
                var refType = ReferenceTypeEnum.NTS_Task;
                if (model.NtsType == NtsTypeEnum.Service)
                {
                    refType = ReferenceTypeEnum.NTS_Service;
                }
                notificationModel.ReferenceType = refType;
                notificationModel.ReferenceTypeId = model.Id;
                notificationModel.ReferenceTemplateCode = model.TemplateCode;
                notificationModel.IsIncludeAttachment = model.IsIncludeEmailAttachment;
                if (model.EmailSettingId.IsNotNullAndNotEmpty())
                {
                    notificationModel.ShowOriginalSender = true;
                }
                switch (model.NtsType)
                {
                    case NtsTypeEnum.Task:
                        if (model.TaskStatusCode == "COMPLETED")
                        {
                            if (model.TemplateCode == "BOOK_TICKET_ATTACH")
                            {
                                notificationModel.Subject = "Book ticket for attached candidate";
                                notificationModel.Body = string.Concat("<div><h4>Hello ", model.TextValue2, ",",
                                                            "<br>We have attached passport and visa copy for ticket booking.please book ticket as per travelling date", "<br><br>",
                                                           await GetTaskUdf(model));
                                notificationModel.From = userModel.Email;
                                notificationModel.ToUserDisplay = model.TextValue2;//take for udf to email
                                notificationModel.To = model.TextValue2;//take from udf
                            }
                        }
                        break;
                    default:
                        break;
                }

                //notificationModel.To = "arshad@extranet.ae;mthamil107@gmail.com";

                var result = await _notiificationBusiness.Create(notificationModel);
                if (result.IsSuccess)
                {
                    try
                    {
                        BackgroundJob.Enqueue<Synergy.App.Business.HangfireScheduler>(x => x.SendEmailUsingHangfire(result.Item.EmailUniqueId));
                    }
                    catch (Exception e)
                    {

                    }
                    return result.Item;

                }
                return notificationModel;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        private async Task<string> GetTaskUdf(RecTaskViewModel model)
        {
            try
            {
                if (model.TemplateCode == "WORKER_POOL_HR" && model.ReferenceTypeId.IsNotNullAndNotEmpty() && model.TextValue1.IsNullOrEmpty())
                {
                    var service = await _taskBusiness.GetSingleById(model.ReferenceTypeId);
                    var applicant = await _applicationBusiness.GetSingleById(service.ReferenceTypeId);
                    if (applicant.IsNotNull())
                    {
                        model.TextValue1 = applicant.SalaryOnAppointment;
                    }

                }
                if (model.TemplateCode == "WORKER_SALARY_AGENCY" && model.ReferenceTypeId.IsNotNullAndNotEmpty() && model.TextValue1.IsNullOrEmpty())
                {
                    var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                    var recruiter = service.Where(x => x.TemplateCode == "WORKER_POOL_HR").FirstOrDefault();
                    if (recruiter.IsNotNull())
                    {
                        model.TextValue1 = recruiter.TextValue1;
                    }

                }
                if (model.TemplateCode == "SCHEDULE_INTERVIEW_CANDIDATE" && model.ReferenceTypeId.IsNotNullAndNotEmpty() && !model.DatePickerValue1.IsNotNull())
                {
                    var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                    var recruiter = service.Where(x => x.TemplateCode == "SCHEDULE_INTERVIEW_RECRUITER").FirstOrDefault();
                    if (recruiter.IsNotNull())
                    {
                        model.DatePickerValue1 = recruiter.DatePickerValue3;
                    }

                }

                if (model.TemplateCode == "CHECK_MEDICAL_REPORT_INFORM_PRO" && model.ReferenceTypeId.IsNotNullAndNotEmpty() && model.AttachmentCode1.IsNullOrEmpty())
                {
                    var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                    var data = service.Where(x => x.TemplateCode == "INFORM_CANDIDATE_FOR_MEDICAL").FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        model.AttachmentCode1 = data.AttachmentCode1;
                        model.AttachmentValue1 = data.AttachmentValue1;
                    }
                }
                if (model.TemplateCode == "FINAL_OFFER_APPROVAL_CANDIDATE" && model.ReferenceTypeId.IsNotNullAndNotEmpty() && model.TextBoxDisplay10.IsNullOrEmpty())
                {
                    var service = await _taskBusiness.GetSingleById(model.ReferenceTypeId);
                    var application = await _applicationBusiness.GetApplicationDetail(service.ReferenceTypeId);
                    if (application.VisaCategoryCode == "VISA_TRANSFER")
                    {
                        model.TextBoxDisplay10 = @"<p>Attach with this the employment contract for your review and acceptance.</p>
                        <p>&nbsp;</p>
                        <p>Please scan and send us the following for initiating the Online visa transfer process:&nbsp;</p>
                        <ul>
                        <li>Duly signed copy of the employment contract (sign on all pages)</li>
                        <li>Credit check from Qatar Credit Bureau (Location: Opposite to Holiday villa hotel in Qatar financial square)</li>
                        <li>Copy of your resignation/termination letter.</li>
                        <li>NOC from your current Company</li>
                        <li>MOFA Attested educational certificate &amp; its Arabic translation</li>
                        <li>MMUP Card (For Engineering Profession only)</li>
                        <li>Current MOL attested labour contract</li>
                        </ul>
                        ";
                    }
                    else if (application.VisaCategoryCode == "LOCAL_WORK_PERMIT")
                    {
                        model.TextBoxDisplay10 = @"<p>Attach with this the employment contract for your review and acceptance.&nbsp;</p>
                    <p>&nbsp;</p>
                    <p>Please scan and send us the following for initiating the Online visa transfer process:&nbsp;</p>
                    <ul>
                    <li>Duly signed copy of the employment contract (sign on all pages)</li>
                    <li>Credit check from Qatar Credit Bureau (Location: Opposite to Holiday villa hotel in Qatar financial square)</li>
                    <li>Copy of your resignation/termination letter.</li>
                    <li>MOFA Attested educational certificate &amp; its Arabic translation</li>
                    <li>MMUP Card (For Engineering Profession only)</li>
                    <li>Current MOL attested labour contract</li>
                    </ul>
                        ";
                    }
                    else if (application.VisaCategoryCode == "BUSINESS_VISA" || application.VisaCategoryCode == "BUSINESS_VISA_MULTIPLE" || application.VisaCategoryCode == "WORK_VISA")
                    {
                        model.TextBoxDisplay10 = @"<p>Please find attached employment contract for your acceptance and signature.</p>
                        <p>&nbsp;</p>
                        <p>As a part of joining formalities you will be required to sign an agreement of general terms and conditions of employment with the organization which also includes clauses related to Confidentiality &amp; No-Compete. You would be governed by the terms and conditions of employment with the organization post joining. &nbsp;Request you to please scan and return following documents for starting your visa processing:</p>
                        <p>&nbsp;</p>
                        <ul>
                        <li>Duly Signed Employment Contract (On all Pages)</li>
                        <li>Medical Report from GAMCA approved medical center/Hospital (Stating Fit to Work)&nbsp;</li>
                        </ul>
                        <ul>
                        <li>10 Passport size photographs at the time of joining</li>
                        </ul>
                        ";
                    }

                }

                if (model.TemplateCode == "RECEIVE_WORK_PERMIT_INFORM_JOINING_DATE" && model.ReferenceTypeId.IsNotNullAndNotEmpty() && model.AttachmentCode2.IsNullOrEmpty())
                {
                    var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                    var data = service.Where(x => x.TemplateCode == "VERIFY_WORK_PERMIT_OBTAINED").FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        model.DatePickerValue1 = data.DatePickerValue2;
                        model.AttachmentCode2 = data.AttachmentCode1;
                        model.AttachmentValue2 = data.AttachmentValue1;

                    }
                }

                if (model.TemplateCode == "OBTAIN_BUSINESS_VISA" && model.ReferenceTypeId.IsNotNullAndNotEmpty() && model.AttachmentCode2.IsNullOrEmpty())
                {
                    var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                    var data = service.Where(x => x.TemplateCode == "INFORM_CANDIDATE_FOR_MEDICAL").FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        model.AttachmentCode2 = data.AttachmentCode1;
                        model.AttachmentValue2 = data.AttachmentValue1;
                    }
                }

                if (model.TemplateCode == "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE" && model.ReferenceTypeId.IsNotNullAndNotEmpty() && model.AttachmentCode1.IsNullOrEmpty())
                {
                    var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                    var data = service.Where(x => x.TemplateCode == "OBTAIN_BUSINESS_VISA").FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        model.AttachmentCode1 = data.AttachmentCode3;
                        model.AttachmentValue1 = data.AttachmentValue3;

                        model.AttachmentCode4 = data.AttachmentCode4;
                        model.AttachmentValue4 = data.AttachmentValue4;
                    }
                }


                if (model.TemplateCode == "CONFIRM_TRAVELING_DATE" && model.ReferenceTypeId.IsNotNullAndNotEmpty() && !model.DatePickerValue1.IsNotNull())
                {
                    var service = await _taskBusiness.GetSingleById(model.ReferenceTypeId);
                    if (service.IsNotNull())
                    {
                        var previousService = await _taskBusiness.GetSingle(x => x.ReferenceTypeId == service.ReferenceTypeId && (x.TemplateCode == "BUSINESS_VISA_MEDICAL"
                        || x.TemplateCode == "WORKER_VISA_MEDICAL") &&
                        x.NtsType == NtsTypeEnum.Service);
                        if (previousService.IsNotNull())
                        {
                            var service1 = await _taskBusiness.GetStepTaskListByService(previousService.Id);
                            var data = service1.Where(x => x.TemplateCode == "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE").FirstOrDefault();
                            if (data.IsNotNull())
                            {
                                if (!model.DatePickerValue1.IsNotNull())
                                {
                                    model.DatePickerValue1 = data.DatePickerValue2;
                                }
                            }
                            var data1 = service1.Where(x => x.TemplateCode == "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE_WORKVISA").FirstOrDefault();
                            if (data1.IsNotNull())
                            {
                                if (!model.DatePickerValue1.IsNotNull())
                                {
                                    model.DatePickerValue1 = data1.DatePickerValue2;
                                }
                            }
                        }
                    }

                }


                if (model.TemplateCode == "BOOK_TICKET_ATTACH" && model.ReferenceTypeId.IsNotNullAndNotEmpty() && model.AttachmentCode4.IsNullOrEmpty())
                {
                    var service = await _taskBusiness.GetSingleById(model.ReferenceTypeId);
                    if (service.IsNotNull())
                    {
                        var previousService = await _taskBusiness.GetSingle(x => x.ReferenceTypeId == service.ReferenceTypeId && x.ReferenceTypeCode == ReferenceTypeEnum.REC_Application && (x.TemplateCode == "BUSINESS_VISA_MEDICAL"
                        || x.TemplateCode == "WORKER_VISA_MEDICAL") &&
                        x.NtsType == NtsTypeEnum.Service);
                        if (previousService.IsNotNull())
                        {
                            var service1 = await _taskBusiness.GetStepTaskListByService(previousService.Id);
                            var data = service1.Where(x => x.TemplateCode == "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE").FirstOrDefault();
                            if (data.IsNotNull())
                            {
                                model.DatePickerValue1 = data.DatePickerValue2;
                                model.AttachmentCode4 = data.AttachmentCode1;
                                model.AttachmentValue4 = data.AttachmentValue1;
                            }
                            var data1 = service1.Where(x => x.TemplateCode == "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE_WORKVISA").FirstOrDefault();
                            if (data1.IsNotNull())
                            {
                                model.DatePickerValue1 = data1.DatePickerValue2;
                                model.AttachmentCode4 = data1.AttachmentCode1;
                                model.AttachmentValue4 = data1.AttachmentValue1;
                            }

                        }

                        var finalService = await _taskBusiness.GetSingle(x => x.ReferenceTypeId == service.ReferenceTypeId && x.ReferenceTypeCode == ReferenceTypeEnum.REC_Application && x.TemplateCode == "PREPARE_FINAL_OFFER"
                         && x.NtsType == NtsTypeEnum.Service);
                        if (finalService.IsNotNull())
                        {
                            var service1 = await _taskBusiness.GetStepTaskListByService(finalService.Id);
                            var data = service1.Where(x => x.TemplateCode == "FINAL_OFFER_APPROVAL_CANDIDATE").FirstOrDefault();
                            if (data.IsNotNull())
                            {
                                model.AttachmentCode5 = data.AttachmentCode4;
                                model.AttachmentValue5 = data.AttachmentValue4;
                            }

                        }
                    }
                }


                if (model.TemplateCode == "CONFIRM_RECEIPT_TICKET_DATE_OF_TRAVEL" && model.ReferenceTypeId.IsNotNullAndNotEmpty() && model.AttachmentCode1.IsNullOrEmpty())
                {
                    var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                    var data = service.Where(x => x.TemplateCode == "TICKET_ATTACH").FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        model.AttachmentCode1 = data.AttachmentCode1;
                        model.AttachmentValue1 = data.AttachmentValue1;

                        model.AttachmentCode5 = data.AttachmentCode3;
                        model.AttachmentValue5 = data.AttachmentValue3;

                    }

                    var pservice = await _taskBusiness.GetSingleById(model.ReferenceTypeId);
                    if (pservice.IsNotNull())
                    {
                        var previousService = await _taskBusiness.GetSingle(x => x.ReferenceTypeId == pservice.ReferenceTypeId && (x.TemplateCode == "BUSINESS_VISA_MEDICAL"
                        || x.TemplateCode == "WORKER_VISA_MEDICAL") &&
                        x.NtsType == NtsTypeEnum.Service);
                        if (previousService.IsNotNull())
                        {
                            var service1 = await _taskBusiness.GetStepTaskListByService(previousService.Id);
                            var data1 = service1.Where(x => x.TemplateCode == "OBTAIN_BUSINESS_VISA").FirstOrDefault();
                            if (data1.IsNotNull())
                            {
                                model.AttachmentCode3 = data1.AttachmentCode3;
                                model.AttachmentValue3 = data1.AttachmentValue3;

                                model.AttachmentCode4 = data1.AttachmentCode4;
                                model.AttachmentValue4 = data1.AttachmentValue4;
                            }

                            var data2 = service1.Where(x => x.TemplateCode == "FIT_UNFIT_ATTACH_VISA_COPY").FirstOrDefault();
                            if (data2.IsNotNull())
                            {
                                model.AttachmentCode3 = data2.AttachmentCode1;
                                model.AttachmentValue3 = data2.AttachmentValue1;

                                model.AttachmentCode4 = data2.AttachmentCode3;
                                model.AttachmentValue4 = data2.AttachmentValue3;
                            }
                        }
                    }
                }

                if (model.TemplateCode == "ARRANGE_ACCOMMODATION" && model.ReferenceTypeId.IsNotNullAndNotEmpty() && model.AttachmentCode1.IsNullOrEmpty())
                {
                    var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                    var data = service.Where(x => x.TemplateCode == "TICKET_ATTACH").FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        model.AttachmentCode1 = data.AttachmentCode1;
                        model.AttachmentValue1 = data.AttachmentValue1;
                    }
                }

                if (model.TemplateCode == "ARRANGE_VEHICLE_PICKUP" && model.ReferenceTypeId.IsNotNullAndNotEmpty() && model.AttachmentCode1.IsNullOrEmpty())
                {
                    var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                    var data = service.Where(x => x.TemplateCode == "TICKET_ATTACH").FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        model.AttachmentCode1 = data.AttachmentCode1;
                        model.AttachmentValue1 = data.AttachmentValue1;
                    }

                    var data1 = service.Where(x => x.TemplateCode == "ARRANGE_ACCOMMODATION").FirstOrDefault();
                    if (data1.IsNotNull())
                    {
                        model.TextValue3 = data1.TextValue3;
                    }
                }

                //work visa
                if (model.TemplateCode == "BOOK_QVC_APPOINTMENT" && model.ReferenceTypeId.IsNotNullAndNotEmpty() && model.AttachmentCode4.IsNullOrEmpty())
                {
                    var service = await _taskBusiness.GetSingleById(model.ReferenceTypeId);
                    if (service.IsNotNull())
                    {

                        var finalService = await _taskBusiness.GetSingle(x => x.ReferenceTypeId == service.ReferenceTypeId && x.ReferenceTypeCode == ReferenceTypeEnum.REC_Application && x.TemplateCode == "PREPARE_FINAL_OFFER"
                         && x.NtsType == NtsTypeEnum.Service);
                        if (finalService.IsNotNull())
                        {
                            var service1 = await _taskBusiness.GetStepTaskListByService(finalService.Id);
                            var data = service1.Where(x => x.TemplateCode == "FINAL_OFFER_APPROVAL_CANDIDATE").FirstOrDefault();
                            if (data.IsNotNull())
                            {
                                model.AttachmentCode4 = data.AttachmentCode4;
                                model.AttachmentValue4 = data.AttachmentValue4;
                            }

                        }
                    }
                }
                if (model.TemplateCode == "CONDUCT_MEDICAL_FINGER_PRINT" && model.ReferenceTypeId.IsNotNullAndNotEmpty() && !model.DatePickerValue2.IsNotNull())
                {
                    var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                    var data = service.Where(x => x.TemplateCode == "BOOK_QVC_APPOINTMENT").FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        model.DatePickerValue2 = data.DatePickerValue2;
                    }
                }


                if (model.TemplateCode == "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE_WORKVISA" && model.ReferenceTypeId.IsNotNullAndNotEmpty() && model.AttachmentCode1.IsNullOrEmpty())
                {
                    var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                    var data = service.Where(x => x.TemplateCode == "FIT_UNFIT_ATTACH_VISA_COPY").FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        model.AttachmentCode1 = data.AttachmentCode1;
                        model.AttachmentValue1 = data.AttachmentValue1;
                    }
                }
                // Visa Transfer

                if (model.TemplateCode == "VERIFY_DOCUMENTS" && model.ReferenceTypeId.IsNotNullAndNotEmpty() && model.AttachmentCode1.IsNullOrEmpty())
                {
                    var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                    var data = service.Where(x => x.TemplateCode == "SUBMIT_VISA_TRANSFER_DOCUMENTS").FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        model.AttachmentCode1 = data.AttachmentCode1;
                        model.AttachmentValue1 = data.AttachmentValue1;
                        model.AttachmentCode2 = data.AttachmentCode2;
                        model.AttachmentValue2 = data.AttachmentValue2;
                        model.AttachmentCode3 = data.AttachmentCode3;
                        model.AttachmentValue3 = data.AttachmentValue3;
                        model.AttachmentCode4 = data.AttachmentCode4;
                        model.AttachmentValue4 = data.AttachmentValue4;
                        model.AttachmentCode5 = data.AttachmentCode5;
                        model.AttachmentValue5 = data.AttachmentValue5;
                    }
                }

                if (model.TemplateCode == "SUBMIT_VISA_TRANSFER_CONFIRM_SPONSORSHIP" && model.ReferenceTypeId.IsNotNullAndNotEmpty() && model.AttachmentCode1.IsNullOrEmpty())
                {
                    var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                    var data = service.Where(x => x.TemplateCode == "SUBMIT_VISA_TRANSFER_DOCUMENTS").FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        model.AttachmentCode1 = data.AttachmentCode1;
                        model.AttachmentValue1 = data.AttachmentValue1;
                        model.AttachmentCode2 = data.AttachmentCode2;
                        model.AttachmentValue2 = data.AttachmentValue2;
                        model.AttachmentCode3 = data.AttachmentCode3;
                        model.AttachmentValue3 = data.AttachmentValue3;
                        model.AttachmentCode4 = data.AttachmentCode4;
                        model.AttachmentValue4 = data.AttachmentValue4;
                        model.AttachmentCode5 = data.AttachmentCode5;
                        model.AttachmentValue5 = data.AttachmentValue5;
                    }
                }

                if (model.TemplateCode == "VERIFY_VISA_TRANSFER_COMPLETED" && model.ReferenceTypeId.IsNotNullAndNotEmpty() && model.AttachmentCode1.IsNullOrEmpty())
                {
                    var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                    var data = service.Where(x => x.TemplateCode == "SUBMIT_VISA_TRANSFER_CONFIRM_SPONSORSHIP").FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        model.AttachmentCode1 = data.AttachmentCode6;
                        model.AttachmentValue1 = data.AttachmentValue6;
                    }
                }

                if (model.TemplateCode == "RECEIVE_VISA_TRANSFER_INFORM_JOINING_DATE" && model.ReferenceTypeId.IsNotNullAndNotEmpty() && model.AttachmentCode2.IsNullOrEmpty())
                {
                    var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                    var data = service.Where(x => x.TemplateCode == "SUBMIT_VISA_TRANSFER_CONFIRM_SPONSORSHIP").FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        model.AttachmentCode2 = data.AttachmentCode6;
                        model.AttachmentValue2 = data.AttachmentValue6;
                    }
                }

                //work permit

                if (model.TemplateCode == "VERIFY_WORK_PERMIT_DOCUMENTS" && model.ReferenceTypeId.IsNotNullAndNotEmpty() && model.AttachmentCode1.IsNullOrEmpty())
                {
                    var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                    var data = service.Where(x => x.TemplateCode == "SUBMIT_WORK_PERMIT_DOCUMENTS").FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        model.AttachmentCode1 = data.AttachmentCode1;
                        model.AttachmentValue1 = data.AttachmentValue1;
                        model.AttachmentCode2 = data.AttachmentCode2;
                        model.AttachmentValue2 = data.AttachmentValue2;
                        model.AttachmentCode3 = data.AttachmentCode3;
                        model.AttachmentValue3 = data.AttachmentValue3;
                        model.AttachmentCode4 = data.AttachmentCode4;
                        model.AttachmentValue4 = data.AttachmentValue4;
                    }
                }


                if (model.TemplateCode == "OBTAIN_WORK_PERMIT_ATTACH" && model.ReferenceTypeId.IsNotNullAndNotEmpty() && model.AttachmentCode1.IsNullOrEmpty())
                {
                    var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                    var data = service.Where(x => x.TemplateCode == "SUBMIT_WORK_PERMIT_DOCUMENTS").FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        model.AttachmentCode1 = data.AttachmentCode1;
                        model.AttachmentValue1 = data.AttachmentValue1;
                        model.AttachmentCode2 = data.AttachmentCode2;
                        model.AttachmentValue2 = data.AttachmentValue2;
                        model.AttachmentCode3 = data.AttachmentCode3;
                        model.AttachmentValue3 = data.AttachmentValue3;
                        model.AttachmentCode4 = data.AttachmentCode4;
                        model.AttachmentValue4 = data.AttachmentValue4;
                    }
                }

                if (model.TemplateCode == "VERIFY_WORK_PERMIT_OBTAINED" && model.ReferenceTypeId.IsNotNullAndNotEmpty() && model.AttachmentCode1.IsNullOrEmpty())
                {
                    var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                    var data = service.Where(x => x.TemplateCode == "OBTAIN_WORK_PERMIT_ATTACH").FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        model.AttachmentCode1 = data.AttachmentValue5;
                        model.AttachmentValue1 = data.AttachmentValue5;
                    }
                }


                if (model.TemplateCode == "VERIFY_WORK_PERMIT_OBTAINED" && model.ReferenceTypeId.IsNotNullAndNotEmpty() && model.AttachmentCode1.IsNullOrEmpty())
                {
                    var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                    var data = service.Where(x => x.TemplateCode == "OBTAIN_WORK_PERMIT_ATTACH").FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        model.AttachmentCode1 = data.AttachmentCode5;
                        model.AttachmentValue1 = data.AttachmentValue5;
                    }
                }

                if (model.TemplateCode == "VERIFY_WORK_PERMIT_OBTAINED" && model.ReferenceTypeId.IsNotNullAndNotEmpty() && model.AttachmentCode2.IsNullOrEmpty())
                {
                    var service = await _taskBusiness.GetStepTaskListByService(model.ReferenceTypeId);
                    var data = service.Where(x => x.TemplateCode == "OBTAIN_WORK_PERMIT_ATTACH").FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        model.AttachmentCode2 = data.AttachmentCode5;
                        model.AttachmentValue2 = data.AttachmentValue5;
                    }
                }
                var body = new StringBuilder();
                var colorcode1 = "#08509b";
                var colorcode2 = "#39a2d5";
                body.Append("<table cellpadding='5' cellspacing='1' style='width:100%;max-width:600px;border:1px solid #fff;font-family:Verdana;font-size:10px;'>");
                body.Append("<tr><td colspan='2' style='padding:6px;text-align:center;font-weight:bold;background-color:" + colorcode1 + ";color:#fff;'>Details</td></tr>");
                if (model.TextBoxDisplay1.IsNotNullAndNotEmpty() && model.TextBoxDisplayType1 != NtsFieldType.NTS_HyperLink)
                {
                    if (model.TextBoxDisplayType1 == NtsFieldType.NTS_HtmlArea)
                    {
                        body.Append(string.Concat("<tr><td colspan='2' style='background-color:#d0d2d3;'>", model.TextBoxDisplay1, "</td></tr>"));
                    }
                    else
                    {
                        body.Append(string.Concat("<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:" + colorcode2 + ";'>", model.TextBoxDisplay1, "</td><td style='background-color:#d0d2d3;'>", model.TextBoxDisplayType1 == NtsFieldType.NTS_Attachment ? model.AttachmentValue1 : ((model.TextBoxDisplayType1 == NtsFieldType.NTS_DatePicker || model.TextBoxDisplayType1 == NtsFieldType.NTS_DateTimePicker) ? model.DatePickerValue1 : model.TextValue1), "</td></tr>"));
                    }

                }
                if (model.DropdownDisplay1.IsNotNullAndNotEmpty())
                {

                    body.Append(string.Concat("<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:" + colorcode2 + ";'>", model.DropdownDisplay1, "</td><td style='background-color:#d0d2d3;'>", model.DropdownDisplayValue1, "</td></tr>"));

                }
                if (model.TextBoxDisplay2.IsNotNullAndNotEmpty() && model.TextBoxDisplayType2 != NtsFieldType.NTS_HyperLink)
                {

                    body.Append(string.Concat("<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:" + colorcode2 + ";'>", model.TextBoxDisplay2, "</td><td style='background-color:#d0d2d3;'>", model.TextBoxDisplayType2 == NtsFieldType.NTS_Attachment ? model.AttachmentValue2 : ((model.TextBoxDisplayType2 == NtsFieldType.NTS_DatePicker || model.TextBoxDisplayType2 == NtsFieldType.NTS_DateTimePicker) ? model.DatePickerValue2 : model.TextValue2), "</td></tr>"));

                }
                if (model.DropdownDisplay2.IsNotNullAndNotEmpty())
                {

                    body.Append(string.Concat("<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:" + colorcode2 + ";'>", model.DropdownDisplay2, "</td><td style='background-color:#d0d2d3;'>", model.DropdownDisplayValue2, "</td></tr>"));

                }
                if (model.TextBoxDisplay3.IsNotNullAndNotEmpty() && model.TextBoxDisplayType3 != NtsFieldType.NTS_HyperLink)
                {

                    body.Append(string.Concat("<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:" + colorcode2 + ";'>", model.TextBoxDisplay3, "</td><td style='background-color:#d0d2d3;'>", model.TextBoxDisplayType3 == NtsFieldType.NTS_Attachment ? model.AttachmentValue3 : ((model.TextBoxDisplayType3 == NtsFieldType.NTS_DatePicker || model.TextBoxDisplayType3 == NtsFieldType.NTS_DateTimePicker) ? model.DatePickerValue3 : model.TextValue3), "</td></tr>"));

                }
                if (model.DropdownDisplay3.IsNotNullAndNotEmpty())
                {

                    body.Append(string.Concat("<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:" + colorcode2 + ";'>", model.DropdownDisplay3, "</td><td style='background-color:#d0d2d3;'>", model.DropdownDisplayValue3, "</td></tr>"));

                }
                if (model.TextBoxDisplay4.IsNotNullAndNotEmpty() && model.TextBoxDisplayType4 != NtsFieldType.NTS_HyperLink)
                {

                    body.Append(string.Concat("<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:" + colorcode2 + ";'>", model.TextBoxDisplay4, "</td><td style='background-color:#d0d2d3;'>", model.TextBoxDisplayType4 == NtsFieldType.NTS_Attachment ? model.AttachmentValue4 : ((model.TextBoxDisplayType4 == NtsFieldType.NTS_DatePicker || model.TextBoxDisplayType4 == NtsFieldType.NTS_DateTimePicker) ? model.DatePickerValue4 : model.TextValue4), "</td></tr>"));

                }
                if (model.DropdownDisplay4.IsNotNullAndNotEmpty())
                {

                    body.Append(string.Concat("<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:" + colorcode2 + ";'>", model.DropdownDisplay4, "</td><td style='background-color:#d0d2d3;'>", model.DropdownDisplayValue4, "</td></tr>"));

                }
                if (model.TextBoxDisplay5.IsNotNullAndNotEmpty() && model.TextBoxDisplayType5 != NtsFieldType.NTS_HyperLink)
                {

                    body.Append(string.Concat("<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:" + colorcode2 + ";'>", model.TextBoxDisplay5, "</td><td style='background-color:#d0d2d3;'>", model.TextBoxDisplayType5 == NtsFieldType.NTS_Attachment ? model.AttachmentValue5 : ((model.TextBoxDisplayType5 == NtsFieldType.NTS_DatePicker || model.TextBoxDisplayType5 == NtsFieldType.NTS_DateTimePicker) ? model.DatePickerValue5 : model.TextValue5), "</td></tr>"));

                }
                if (model.DropdownDisplay5.IsNotNullAndNotEmpty())
                {

                    body.Append(string.Concat("<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:" + colorcode2 + ";'>", model.DropdownDisplay5, "</td><td style='background-color:#d0d2d3;'>", model.DropdownDisplayValue5, "</td></tr>"));

                }
                if (model.TextBoxDisplay6.IsNotNullAndNotEmpty() && model.TextBoxDisplayType6 != NtsFieldType.NTS_HyperLink)
                {

                    body.Append(string.Concat("<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:" + colorcode2 + ";'>", model.TextBoxDisplay6, "</td><td style='background-color:#d0d2d3;'>", model.TextBoxDisplayType6 == NtsFieldType.NTS_Attachment ? model.AttachmentValue6 : ((model.TextBoxDisplayType6 == NtsFieldType.NTS_DatePicker || model.TextBoxDisplayType6 == NtsFieldType.NTS_DateTimePicker) ? model.DatePickerValue6 : model.TextValue6), "</td></tr>"));

                }
                if (model.DropdownDisplay6.IsNotNullAndNotEmpty())
                {

                    body.Append(string.Concat("<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:" + colorcode2 + ";'>", model.DropdownDisplay6, "</td><td style='background-color:#d0d2d3;'>", model.DropdownDisplayValue6, "</td></tr>"));

                }
                if (model.TextBoxDisplay7.IsNotNullAndNotEmpty() && model.TextBoxDisplayType7 != NtsFieldType.NTS_HyperLink)
                {

                    body.Append(string.Concat("<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:" + colorcode2 + ";'>", model.TextBoxDisplay7, "</td><td style='background-color:#d0d2d3;'>", model.TextBoxDisplayType7 == NtsFieldType.NTS_Attachment ? model.AttachmentValue7 : ((model.TextBoxDisplayType7 == NtsFieldType.NTS_DatePicker || model.TextBoxDisplayType7 == NtsFieldType.NTS_DateTimePicker) ? model.DatePickerValue7 : model.TextValue7), "</td></tr>"));

                }
                if (model.DropdownDisplay7.IsNotNullAndNotEmpty())
                {

                    body.Append(string.Concat("<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:" + colorcode2 + ";'>", model.DropdownDisplay7, "</td><td style='background-color:#d0d2d3;'>", model.DropdownDisplayValue7, "</td></tr>"));

                }
                if (model.TextBoxDisplay8.IsNotNullAndNotEmpty() && model.TextBoxDisplayType8 != NtsFieldType.NTS_HyperLink)
                {

                    body.Append(string.Concat("<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:" + colorcode2 + ";'>", model.TextBoxDisplay8, "</td><td style='background-color:#d0d2d3;'>", model.TextBoxDisplayType8 == NtsFieldType.NTS_Attachment ? model.AttachmentValue8 : ((model.TextBoxDisplayType8 == NtsFieldType.NTS_DatePicker || model.TextBoxDisplayType8 == NtsFieldType.NTS_DateTimePicker) ? model.DatePickerValue8 : model.TextValue8), "</td></tr>"));

                }
                if (model.DropdownDisplay8.IsNotNullAndNotEmpty())
                {

                    body.Append(string.Concat("<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:" + colorcode2 + ";'>", model.DropdownDisplay8, "</td><td style='background-color:#d0d2d3;'>", model.DropdownDisplayValue8, "</td></tr>"));

                }
                if (model.TextBoxDisplay9.IsNotNullAndNotEmpty() && model.TextBoxDisplayType9 != NtsFieldType.NTS_HyperLink)
                {

                    body.Append(string.Concat("<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:" + colorcode2 + ";'>", model.TextBoxDisplay9, "</td><td style='background-color:#d0d2d3;'>", model.TextBoxDisplayType9 == NtsFieldType.NTS_Attachment ? model.AttachmentValue9 : ((model.TextBoxDisplayType9 == NtsFieldType.NTS_DatePicker || model.TextBoxDisplayType9 == NtsFieldType.NTS_DateTimePicker) ? model.DatePickerValue9 : model.TextValue9), "</td></tr>"));

                }
                if (model.DropdownDisplay9.IsNotNullAndNotEmpty())
                {

                    body.Append(string.Concat("<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:" + colorcode2 + ";'>", model.DropdownDisplay9, "</td><td style='background-color:#d0d2d3;'>", model.DropdownDisplayValue9, "</td></tr>"));

                }
                if (model.TextBoxDisplay10.IsNotNullAndNotEmpty() && model.TextBoxDisplayType10 != NtsFieldType.NTS_HyperLink)
                {
                    if (model.TextBoxDisplayType10 == NtsFieldType.NTS_HtmlArea)
                    {
                        body.Append(string.Concat("<tr><td colspan='2' style='background-color:#d0d2d3;'>", model.TextBoxDisplay10, "</td></tr>"));
                    }
                    else
                    {
                        body.Append(string.Concat("<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:" + colorcode2 + ";'>", model.TextBoxDisplay10, "</td><td style='background-color:#d0d2d3;'>", model.TextBoxDisplayType10 == NtsFieldType.NTS_Attachment ? model.AttachmentValue10 : ((model.TextBoxDisplayType10 == NtsFieldType.NTS_DatePicker || model.TextBoxDisplayType10 == NtsFieldType.NTS_DateTimePicker) ? model.DatePickerValue10 : model.TextValue10), "</td></tr>"));
                    }
                }
                if (model.DropdownDisplay10.IsNotNullAndNotEmpty())
                {

                    body.Append(string.Concat("<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:" + colorcode2 + ";'>", model.DropdownDisplay10, "</td><td style='background-color:#d0d2d3;'>", model.DropdownDisplayValue10, "</td></tr>"));

                }
                body.Append("</table>");
                return body.ToString();
            }
            catch (Exception)
            {
                return "";
            }

        }
        public IActionResult ViewTaskList(string taskid = null)
        {
            var model = new RecTaskViewModel { Id = taskid };
            return View(model);
        }
        private async Task<List<IdNameViewModel>> GetDropDownDataSource(string action)
        {
            if (action.ToLower().Contains("/master/"))
            {
                var str = action.Split('=');
                var type = str[1];
                var data = await _MasterBusiness.GetIdNameList(type);
                return data.ToList();
            }
            else if (action.ToLower().Contains("/listofvalue/"))
            {
                var str = action.Split('=');
                var type = str[1];
                var data = await _lovBusiness.GetList(x => x.ListOfValueType == type && x.Status != StatusEnum.Inactive);
                var list = data.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name });
                return list.ToList();
            }
            else if (action.ToLower().Contains("/user/"))
            {
                var list = await _userBusiness.GetUserIdNameList();
                return list;
            }
            else
            {
                return new List<IdNameViewModel>();
            }


        }
        public async Task<IActionResult> TaskList(string code = "Test_Template", string status = "INPROGRESS", string batch = null)
        {
            var ids = await _taskBusiness.GetTaskIdsByUserId(_userContext.UserId, code, status, batch);
            //result = result.Where(x=>x.TemplateCode == code && x.TaskStatusCode == status).Take(5).ToList();
            if (ids.IsNotNullAndNotEmpty())
            {
                var list = await _taskBusiness.GetTaskDetailsList(ids, code, _userContext.UserId);
                if (list.IsNotNull() && batch.IsNotNullAndNotEmpty())
                {
                    list = list.Where(x => x.BatchId == batch).ToList();
                }
                if (list.Count > 0)
                {
                    ViewBag.DataSource = list;
                    ViewBag.DataSourceHeader = list.First();
                    var template = list.First();
                    ViewBag.DataSourceUser = await _userBusiness.GetUserIdNameList();
                    if (template.DropdownValueMethod1.IsNotNullAndNotEmpty())
                    {
                        ViewBag.DDL1 = this.GetDropDownDataSource(template.DropdownValueMethod1);
                    }
                    if (template.DropdownValueMethod2.IsNotNullAndNotEmpty())
                    {
                        ViewBag.DDL2 = this.GetDropDownDataSource(template.DropdownValueMethod2);
                    }
                    if (template.DropdownValueMethod3.IsNotNullAndNotEmpty())
                    {
                        ViewBag.DDL3 = this.GetDropDownDataSource(template.DropdownValueMethod3);
                    }
                    if (template.DropdownValueMethod4.IsNotNullAndNotEmpty())
                    {
                        ViewBag.DDL4 = this.GetDropDownDataSource(template.DropdownValueMethod4);
                    }
                    if (template.DropdownValueMethod5.IsNotNullAndNotEmpty())
                    {
                        ViewBag.DDL5 = this.GetDropDownDataSource(template.DropdownValueMethod5);
                    }
                    if (template.DropdownValueMethod6.IsNotNullAndNotEmpty())
                    {
                        ViewBag.DDL6 = this.GetDropDownDataSource(template.DropdownValueMethod6);
                    }
                    if (template.DropdownValueMethod7.IsNotNullAndNotEmpty())
                    {
                        ViewBag.DDL7 = this.GetDropDownDataSource(template.DropdownValueMethod7);
                    }
                    if (template.DropdownValueMethod8.IsNotNullAndNotEmpty())
                    {
                        ViewBag.DDL8 = this.GetDropDownDataSource(template.DropdownValueMethod8);
                    }
                    if (template.DropdownValueMethod9.IsNotNullAndNotEmpty())
                    {
                        ViewBag.DDL9 = this.GetDropDownDataSource(template.DropdownValueMethod9);
                    }
                    if (template.DropdownValueMethod10.IsNotNullAndNotEmpty())
                    {
                        ViewBag.DDL10 = this.GetDropDownDataSource(template.DropdownValueMethod10);
                    }

                    return View();
                }
                else
                {
                    var template = await _taskBusiness.GetTemplateDetails(code);
                    ViewBag.DataSourceHeader = template;
                    return View();
                }

            }
            else
            {
                var template = await _taskBusiness.GetTemplateDetails(code);
                ViewBag.DataSourceHeader = template;
                return View();
            }

        }


        public async Task<ActionResult> TaskListPartial(string code = "Test_Template", string status = "INPROGRESS", string batch = null, bool disableBulk = false, LayoutModeEnum layoutMode = LayoutModeEnum.Popup)
        {
            ViewBag.Layout = "/Views/Shared/_PopupLayout.cshtml";
            if (layoutMode == LayoutModeEnum.None)
            {
                ViewBag.Layout = null;
            }
            var model = new RecTaskViewModel { TemplateCode = code, TaskStatusCode = status, BatchId = batch };
            ViewBag.IsDisableBulk = disableBulk;
            var ids = await _taskBusiness.GetTaskIdsByUserId(_userContext.UserId, code, status, batch);
            //result = result.Where(x=>x.TemplateCode == code && x.TaskStatusCode == status).Take(5).ToList();
            if (ids.IsNotNullAndNotEmpty())
            {
                var list = await _taskBusiness.GetTaskDetailsList(ids, code, _userContext.UserId);
                if (list.IsNotNull() && batch.IsNotNullAndNotEmpty())
                {
                    list = list.Where(x => x.BatchId == batch).ToList();
                }
                if (list.Count > 0)
                {
                    ViewBag.DataSource = list;
                    ViewBag.DataSourceHeader = list.First();
                    var template = list.First();
                    if (template.DropdownValueMethod1.IsNotNullAndNotEmpty())
                    {
                        ViewBag.DDL1 = await this.GetDropDownDataSource(template.DropdownValueMethod1);
                    }
                    if (template.DropdownValueMethod2.IsNotNullAndNotEmpty())
                    {
                        ViewBag.DDL2 = await this.GetDropDownDataSource(template.DropdownValueMethod2);
                    }
                    if (template.DropdownValueMethod3.IsNotNullAndNotEmpty())
                    {
                        ViewBag.DDL3 = await this.GetDropDownDataSource(template.DropdownValueMethod3);
                    }
                    if (template.DropdownValueMethod4.IsNotNullAndNotEmpty())
                    {
                        ViewBag.DDL4 = await this.GetDropDownDataSource(template.DropdownValueMethod4);
                    }
                    if (template.DropdownValueMethod5.IsNotNullAndNotEmpty())
                    {
                        ViewBag.DDL5 = await this.GetDropDownDataSource(template.DropdownValueMethod5);
                    }
                    if (template.DropdownValueMethod6.IsNotNullAndNotEmpty())
                    {
                        ViewBag.DDL6 = await this.GetDropDownDataSource(template.DropdownValueMethod6);
                    }
                    if (template.DropdownValueMethod7.IsNotNullAndNotEmpty())
                    {
                        ViewBag.DDL7 = await this.GetDropDownDataSource(template.DropdownValueMethod7);
                    }
                    if (template.DropdownValueMethod8.IsNotNullAndNotEmpty())
                    {
                        ViewBag.DDL8 = await this.GetDropDownDataSource(template.DropdownValueMethod8);
                    }
                    if (template.DropdownValueMethod9.IsNotNullAndNotEmpty())
                    {
                        ViewBag.DDL9 = await this.GetDropDownDataSource(template.DropdownValueMethod9);
                    }
                    if (template.DropdownValueMethod10.IsNotNullAndNotEmpty())
                    {
                        ViewBag.DDL10 = await this.GetDropDownDataSource(template.DropdownValueMethod10);
                    }
                    ViewBag.DataSourceUser = await _userBusiness.GetUserIdNameList();
                    return View(model);
                }
                else
                {
                    var template = await _taskBusiness.GetTemplateDetails(code);
                    ViewBag.DataSourceHeader = template;
                    return View(model);
                }

            }
            else
            {
                var template = await _taskBusiness.GetTemplateDetails(code);
                ViewBag.DataSourceHeader = template;
                return View(model);
            }

        }
        public async Task<IActionResult> TaskListWithoutEditing(string code = "Test_Template", string status = "INPROGRESS", string batch = null)
        {
            var ids = await _taskBusiness.GetTaskIdsByUserId(_userContext.UserId, code, status, batch);
            //result = result.Where(x=>x.TemplateCode == code && x.TaskStatusCode == status).Take(5).ToList();
            if (ids.IsNotNullAndNotEmpty())
            {
                var list = await _taskBusiness.GetTaskDetailsList(ids, code, _userContext.UserId);
                if (list.IsNotNull() && batch.IsNotNullAndNotEmpty())
                {
                    list = list.Where(x => x.BatchId == batch).ToList();
                }
                if (list.Count > 0)
                {
                    ViewBag.DataSource = list;
                    ViewBag.DataSourceHeader = list.First();
                    ViewBag.DataSourceUser = await _userBusiness.GetUserIdNameList();
                    return View();
                }
                else
                {
                    var template = await _taskBusiness.GetTemplateDetails(code);
                    ViewBag.DataSourceHeader = template;
                    return View();
                }
            }
            else
            {
                var template = await _taskBusiness.GetTemplateDetails(code);
                ViewBag.DataSourceHeader = template;
                return View();
            }

        }
        public IActionResult ReportPendingInduction()
        {
            return View();
        }
        public IActionResult ReportPendingJobDescription()
        {
            return View();
        }

        public IActionResult ServiceList()
        {
            return View();
        }
        public async Task<IActionResult> ReadStepTaskData(string serviceid, string versionNo = "")
        {
            var result = await _taskBusiness.GetActiveStepTaskListByService(serviceid, versionNo);
            var j = Json(result);
            return j;
        }
        public async Task<IActionResult> ReadTaskDataInProgress()
        {
            var result = await _taskBusiness.GetActiveListByUserId(_userContext.UserId);
            var j = Json(result.Where(x => x.TaskStatusCode == "INPROGRESS" && x.DueDate >= DateTime.Now).OrderByDescending(x => x.StartDate));
            return j;
        }
        public async Task<IActionResult> ReadTaskDataPending([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _taskBusiness.GetActiveListByUserId(_userContext.UserId);
            var j = Json(result.Where(x => x.TemplateCode == "CONFIRM_INDUCTION_DATE_TO_CANDIDATE" && (x.TaskStatusCode == "INPROGRESS" || x.TaskStatusCode == "OVERDUE")).OrderByDescending(x => x.StartDate).ToDataSourceResult(request));
            return j;
        }
        public async Task<IActionResult> ReadJDTaskDataPending([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _taskBusiness.GetTaskByTemplateCode("JOBDESCRIPTION_HM");
            var j = Json(result.OrderByDescending(x => x.StartDate).ToDataSourceResult(request));
            return j;
        }
        public async Task<IActionResult> ReadTaskDataOverdue()
        {
            var result = await _taskBusiness.GetActiveListByUserId(_userContext.UserId);
            var j = Json(result.Where(x => x.TaskStatusCode == "OVERDUE").OrderByDescending(x => x.StartDate));
            return j;
        }
        public async Task<IActionResult> ReadTaskDataCompleted()
        {
            var result = await _taskBusiness.GetActiveListByUserId(_userContext.UserId);
            var j = Json(result.Where(x => x.TaskStatusCode == "COMPLETED").OrderByDescending(x => x.StartDate));
            return j;
        }
        public async Task<IActionResult> ReadServiceDataInProgress()
        {
            var result = await _taskBusiness.GetActiveServiceListByUserId(_userContext.UserId);
            var j = Json(result.Where(x => x.TaskStatusCode == "INPROGRESS").OrderByDescending(x => x.StartDate));
            return j;
        }

        public async Task<IActionResult> ReadServiceDataOverdue()
        {
            var result = await _taskBusiness.GetActiveServiceListByUserId(_userContext.UserId);
            var j = Json(result.Where(x => x.TaskStatusCode == "OVERDUE").OrderByDescending(x => x.StartDate));
            return j;
        }
        public async Task<ActionResult> ViewAttachment(string fileId, bool canEdit = false)
        {
            var file = await _fileBusiness.GetSingleById(fileId);
            var doc = await _fileBusiness.GetFileByte(fileId);
            if (doc != null)
            {
                file.ContentByte = doc;
                file.ContentBase64 = Convert.ToBase64String(doc, 0, doc.Length);
            }
            //if (Helper.Is2JpegSupportable(file.FileExtension))
            //{
            //    file.FileSnapshotIds = _fileBusiness.GetFileSnapshotIdList(fileId);
            //}
            ViewBag.CanEdit = canEdit;
            return View("_ViewAttachment", file);
        }

        public async Task<ActionResult> AssignTaskForScheduleInterview(string applicantIds, string assignTo)
        {
            var applicantlist = applicantIds.Split(",");

            foreach (var applicantId in applicantlist)
            {
                if (applicantId.IsNotNullAndNotEmpty())
                {
                    var application = await _applicationBusiness.GetSingle(x => x.Id == applicantId);
                    var OwnerId = _userContext.UserId;
                    if (application != null)
                    {
                        var batch = await _applicationBusiness.GetSingle<BatchViewModel, Batch>(x => x.Id == application.BatchId);
                        if (batch != null)
                        {
                            OwnerId = batch.CreatedBy;
                        }
                    }

                    //var model = _taskBusiness.GetTaskDetails(new RecTaskViewModel { TemplateCode = "SCHEDULE_INTERVIEW", ActiveUserId = _userContext.UserId, ReferenceTypeCode = ReferenceTypeEnum.REC_Application, ReferenceTypeId = applicantId }).Result;

                    //model.Id = Guid.NewGuid().ToString();
                    //model.DataAction = DataActionEnum.Create;
                    //model.TemplateAction = NtsActionEnum.Submit;
                    //model.ActiveUserId = _userContext.UserId;
                    //model.AssigneeUserId = assignTo.IsNotNullAndNotEmpty() ? assignTo : model.AssigneeUserId;
                    ////model.AssigneeTeamId = teamId.IsNotNullAndNotEmpty() ? teamId : model.AssigneeTeamId;
                    //model.AssignToType = model.AssigneeUserId.IsNotNullAndNotEmpty() ? AssignToTypeEnum.User : AssignToTypeEnum.Team;
                    //model.OwnerUserId = _userContext.UserId;
                    //model.RequestedUserId = _userContext.UserId;
                    //var result = await Manage(model);

                    var result = await this.CreateService("SCHEDULE_INTERVIEW", applicantId, null, ReferenceTypeEnum.REC_Application, OwnerId);
                }
            }
            return Json(new { success = true });
        }
        public async Task<ActionResult> CancelTaskForScheduleInterview(string taskIds, string assignTo)
        {
            var tasklist = taskIds.Split(",");

            foreach (var taskId in tasklist)
            {
                if (taskId.IsNotNullAndNotEmpty())
                {
                    var model = await _taskBusiness.GetTaskDetails(new RecTaskViewModel { Id = taskId });
                    if (model != null)
                    {
                        //model.TemplateCode = templateCode;
                        //model.Id = Guid.NewGuid().ToString();
                        model.DataAction = DataActionEnum.Edit;
                        model.TemplateAction = NtsActionEnum.Cancel;
                        model.ActiveUserId = _userContext.UserId;
                        model.AssigneeUserId = assignTo.IsNotNullAndNotEmpty() ? assignTo : model.AssigneeUserId;
                        //model.AssigneeTeamId = teamId.IsNotNullAndNotEmpty() ? teamId : model.AssigneeTeamId;
                        model.AssignToType = model.AssigneeUserId.IsNotNullAndNotEmpty() ? AssignToTypeEnum.User : AssignToTypeEnum.Team;
                        model.OwnerUserId = _userContext.UserId;
                        model.RequestedUserId = _userContext.UserId;
                        var result = await Manage(model);
                    }
                }
            }
            return Json(new { success = true });
        }
        [HttpGet]
        public async Task<ActionResult> GetVersionList(string taskId, long? versionId = null)
        {
            var data = await _taskBusiness.GetVersionList(taskId, versionId);
            //data=data.OrderByDescending(x=>x.VersionNo).Select(x => new IdNameViewModel { Id = x.Id, Name = string.Concat("Ver: ", x.VersionNo) }).ToList();
            return Json(data);
        }

        public async Task<ActionResult> ServiceEmployeeAppointment(string applicationId)
        {
            if (applicationId.IsNotNullAndNotEmpty())
            {
                var application = await _applicationBusiness.GetSingle(x => x.Id == applicationId);
                var OwnerId = _userContext.UserId;
                if (application != null)
                {
                    var batch = await _applicationBusiness.GetSingle<BatchViewModel, Batch>(x => x.Id == application.BatchId);
                    if (batch != null)
                    {
                        OwnerId = batch.CreatedBy;
                    }
                }
                var result = await this.CreateService("EMPLOYEE_APPOINTMENT", applicationId, null, ReferenceTypeEnum.REC_Application, OwnerId);
                return result;
            }
            return Json(new { success = false });
        }
        public async Task<ActionResult> WorkerAppointment(string applicationId)
        {
            if (applicationId.IsNotNullAndNotEmpty())
            {
                var application = await _applicationBusiness.GetSingle(x => x.Id == applicationId);
                var OwnerId = _userContext.UserId;
                if (application != null)
                {
                    var batch = await _applicationBusiness.GetSingle<BatchViewModel, Batch>(x => x.Id == application.BatchId);
                    if (batch != null)
                    {
                        OwnerId = batch.CreatedBy;
                    }
                }
                var result = await this.CreateService("WORKER_POOL_REQUEST", applicationId, null, ReferenceTypeEnum.REC_Application, OwnerId);
                return result;
            }
            return Json(new { success = false });
        }
        public async Task<ActionResult> ServiceWorkerBatchApproval(string batchId)
        {
            var OwnerId = _userContext.UserId;
            var batch = await _applicationBusiness.GetSingle<BatchViewModel, Batch>(x => x.Id == batchId);
            if (batch != null)
            {
                OwnerId = batch.CreatedBy;
            }
            if (batchId.IsNotNullAndNotEmpty())
            {
                var result = await this.CreateService("WORKER_POOL_REQUEST", batchId, null, ReferenceTypeEnum.REC_WorkerPoolBatch, OwnerId);
                // Update Batch Status To Sent for Approval
                var workerBatch = await _batchBusiness.GetSingleById(batchId);
                await _batchBusiness.UpdateBatchStatus(batchId, "SENTFORAPPROVAL");
                var applicantlist = await _applicationBusiness.GetList(x => x.WorkerBatchId == batchId);
                var applicants = applicantlist.Select(x => x.Id).ToArray();
                var applicantsId = string.Join(",", applicants);
                await _applicationBusiness.UpdateApplicationState(applicantsId, "WorkerPool");
                foreach (var applicant in applicantlist)
                {
                    await _applicationBusiness.UpdateApplicationtStatus(applicant.Id, "UnderApproval");
                    await _applicationBusiness.CreateApplicationStatusTrack(applicant.Id);
                }

                return result;
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public async Task<ActionResult> CancelServiceTaskForReturnCandidatePool(string Comment, string appId, string appStateCode, string tempCode)
        {
            ApplicationStateCommentViewModel modelcomment = new ApplicationStateCommentViewModel();
            modelcomment.Comment = Comment;
            modelcomment.ApplicationId = appId;
            var state1 = await _applicationBusiness.GetSingleGlobal<IdNameViewModel, ApplicationState>(x => x.Code == appStateCode);
            modelcomment.ApplicationStateId = state1.Id;

            var result = await _applicationStateCommentBusiness.Create(modelcomment);

            var service = await _taskBusiness.GetSingle(x => x.ReferenceTypeId == appId && x.TemplateCode == tempCode && x.NtsType == NtsTypeEnum.Service && x.TaskStatusCode != "CLOSED");
            if (service.IsNotNull())
            {
                var taskList = await _taskBusiness.GetStepTaskListByService(service.Id);
                foreach (var task in taskList)
                {
                    task.TemplateAction = NtsActionEnum.Close;
                    task.DataAction = DataActionEnum.Edit;
                    //task.CancelReason = Comment;
                    var taskresult = await this.Manage(task);

                }
                service.TemplateAction = NtsActionEnum.Close;
                service.DataAction = DataActionEnum.Edit;
                //service.CancelReason = Comment;
                var serviceresult = await this.Manage(service);

                await _applicationBusiness.UpdateApplicationState(appId, "Rereviewed");
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public async Task<IActionResult> ManageBulkScheduleInterview(string applicationIds)
        {
            var model = new ApplicationViewModel();
            model.ApplicationIds = applicationIds;

            return View("_ManageBulkScheduleInterview", model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageBulkScheduleInterview(ApplicationViewModel model)
        {
            if (model.ApplicationIds.IsNotNullAndNotEmpty())
            {
                var appIds = model.ApplicationIds.Trim(',').Split(',');
                foreach (var appId in appIds)
                {
                    if (appId.IsNotNullAndNotEmpty())
                    {
                        var service = await _taskBusiness.GetTaskDetails(new RecTaskViewModel { TemplateCode = "SCHEDULE_INTERVIEW", ActiveUserId = _userContext.UserId, ReferenceTypeCode = ReferenceTypeEnum.REC_Application, ReferenceTypeId = appId });
                        if (service != null)
                        {
                            service.Id = Guid.NewGuid().ToString();
                            service.DataAction = DataActionEnum.Create;
                            service.TemplateAction = NtsActionEnum.Submit;
                            service.ActiveUserId = _userContext.UserId;
                            service.AssigneeUserId = _userContext.UserId;
                            service.AssignToType = AssignToTypeEnum.User;
                            service.OwnerUserId = _userContext.UserId;
                            service.RequestedUserId = _userContext.UserId;
                            service.DatePickerValue1 = model.ScheduleInterveiwDate;
                            service.TextValue2 = model.ScheduleInterveiwComments;
                            var result = await Manage(service);

                        }
                    }
                }
                return Json(new { success = true });
            }
            return View("_ManageBulkScheduleInterview", model);
        }
        public async Task<ActionResult> AssignTaskToAgencyForWorkerSalaryApproval(string applicantId, string assignTo)
        {
            var model = await _taskBusiness.GetTaskDetails(new RecTaskViewModel { TemplateCode = "WORKER_SALARY_AGENCY", ActiveUserId = _userContext.UserId, ReferenceTypeCode = ReferenceTypeEnum.REC_Application, ReferenceTypeId = applicantId });
            var applicant = await _applicationBusiness.GetSingleById(model.ReferenceTypeId);
            model.Id = Guid.NewGuid().ToString();
            model.DataAction = DataActionEnum.Create;
            model.TemplateAction = NtsActionEnum.Submit;
            model.ActiveUserId = _userContext.UserId;
            model.AssigneeUserId = assignTo.IsNotNullAndNotEmpty() ? assignTo : model.AssigneeUserId;
            model.AssignToType = model.AssigneeUserId.IsNotNullAndNotEmpty() ? AssignToTypeEnum.User : AssignToTypeEnum.Team;
            model.OwnerUserId = _userContext.UserId;
            model.RequestedUserId = _userContext.UserId;
            if (applicant != null)
            {
                model.DropdownValue1 = applicant.JobId;
                model.DropdownValue2 = applicant.OrganizationId;
                model.TextValue3 = applicant.SalaryOnAppointment;
            }
            var result = await Manage(model);
            return result;
        }

        public async Task<ActionResult> TaskBatchSendHm(string batchId)
        {
            var batch = await _batchBusiness.GetSingleById(batchId);
            if (batch != null)
            {
                var model = await _taskBusiness.GetTaskDetails(new RecTaskViewModel { TemplateCode = "SL_BATCH_SEND_HM", ActiveUserId = _userContext.UserId, ReferenceTypeCode = ReferenceTypeEnum.REC_Batch, ReferenceTypeId = batch.Id });
                model.Id = Guid.NewGuid().ToString();
                model.DataAction = DataActionEnum.Create;
                model.TemplateAction = NtsActionEnum.Submit;
                model.ActiveUserId = _userContext.UserId;
                model.AssigneeUserId = batch.HiringManager;
                model.AssignToType = AssignToTypeEnum.User;
                model.OwnerUserId = _userContext.UserId;
                model.RequestedUserId = _userContext.UserId;
                model.DropdownValue1 = batch.JobId;
                model.DropdownValue2 = batch.OrganizationId;
                var result = await Manage(model);
                return result;
            }
            return Json(new { success = true });
        }
        public ActionResult DummyTaskView()
        {
            return View("~/Views/Cms/Task.cshtml");
        }


        public async Task<ActionResult> BulkTaskApprove()
        {
            var taskList = await _taskBusiness.GetActiveListByUserId(_userContext.UserId);
            var model = taskList.Where(x => x.TaskStatusCode == "INPROGRESS" || x.TaskStatusCode == "OVERDUE").GroupBy(x => x.TemplateCode).Select(g => g.First()).OrderBy(x => x.SequenceOrder).ToList();
            return View(model);
        }
        public async Task<ActionResult> BulkTaskAction(string code, string status, string batch, bool disableBulk = false, LayoutModeEnum layoutMode = LayoutModeEnum.Popup)
        {
            ViewBag.Layout = "/Views/Shared/_PopupLayout.cshtml";
            var template = await _taskTemplateBusiness.GetSingle(x => x.TemplateCode == code);
            if (layoutMode == LayoutModeEnum.None)
            {
                ViewBag.Layout = null;
            }
            var model = new RecTaskViewModel { TemplateCode = code, TaskStatusCode = status, BatchId = batch };
            if (template.IsNotNull())
            {
                model.BannerId = template.BannerId;
                model.BannerStyle = template.BannerStyle;
            }
            ViewBag.IsDisableBulk = disableBulk;
            return View(model);
        }
        public async Task<IActionResult> TaskUdfDetails(string taskId, string ids)
        {
            var model = await _taskBusiness.GetTaskDetails(new RecTaskViewModel { Id = taskId, TaskListJsonString = ids, ActiveUserId = _userContext.UserId });
            model.ActiveUserId = _userContext.UserId;
            model.TaskListJsonString = ids;
            return View(model);
        }
        //[HttpPost]
        //public async Task<IActionResult> TaskUdfDetails([FromBody] List<RecTaskViewModel> list)
        //{

        //    var firstrecord = list.First();
        //    var sermodel = JsonConvert.SerializeObject(list);
        //    var model = await _taskBusiness.GetTaskDetails(new RecTaskViewModel { Id = firstrecord.Id, ActiveUserId = _userContext.UserId });
        //    model.ActiveUserId = _userContext.UserId;
        //    model.TaskListJsonString = sermodel;
        //    return View(model);
        //}
        [HttpPost]
        public async Task<ActionResult> BulkManage(RecTaskViewModel bulkmodel)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var list = await _taskBusiness.GetTaskDetailsList(bulkmodel.TaskListJsonString.Trim(), bulkmodel.TemplateCode, _userContext.UserId);

                    foreach (var model in list)
                    {
                        MapUdf(model, bulkmodel);
                        model.TemplateAction = bulkmodel.TemplateAction;
                        if (model.TemplateAction == NtsActionEnum.Submit)
                        {
                            model.TaskStatusName = "In Progress";
                            model.TaskStatusCode = "INPROGRESS";
                            model.SubmittedDate = System.DateTime.Now;
                            model.StartDate = model.StartDate >= DateTime.Now ? model.StartDate : DateTime.Now;
                            model.DueDate = model.StartDate.Value.AddDays(Convert.ToDouble(model.SLA));
                        }
                        else if (model.TemplateAction == NtsActionEnum.Complete)
                        {
                            model.TaskStatusName = "Completed";
                            model.TaskStatusCode = "COMPLETED";
                            model.CompletionDate = System.DateTime.Now;
                        }
                        else if (model.TemplateAction == NtsActionEnum.Reject)
                        {
                            model.TaskStatusName = "Rejected";
                            model.TaskStatusCode = "REJECTED";
                            model.RejectedDate = System.DateTime.Now;
                        }
                        else if (model.TemplateAction == NtsActionEnum.NotApplicable)
                        {
                            model.TaskStatusName = "Not Applicable";
                            model.TaskStatusCode = "NOTAPPLICABLE";
                        }
                        var result = await PreScriptTask(model);
                        if (result != "success")
                        {
                            return Json(new { success = false, error = result });
                        }
                        var Taskresult = await _taskBusiness.Edit(model);
                        if (Taskresult.IsSuccess)
                        {
                            await PostScriptTask(model);
                            //return Json(new { success = true, Id = Taskresult.Item.Id });
                        }
                        else
                        {
                            ModelState.AddModelErrors(Taskresult.Messages);
                            //return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                        }
                    }



                }
                if (ModelState.ErrorCount > 0)
                {
                    return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
                }
                else
                {
                    return Json(new { success = true });

                }
            }
            catch (Exception ex)
            {

                return Json(new { success = false, error = ex.Message });
            }


        }
        public void MapUdf(RecTaskViewModel model, RecTaskViewModel newmodel)
        {
            model.TextValue1 = newmodel.TextValue1.IsNotNullAndNotEmpty() ? newmodel.TextValue1 : model.TextValue1;
            model.TextValue2 = newmodel.TextValue2.IsNotNullAndNotEmpty() ? newmodel.TextValue2 : model.TextValue2;
            model.TextValue3 = newmodel.TextValue3.IsNotNullAndNotEmpty() ? newmodel.TextValue3 : model.TextValue3;
            model.TextValue4 = newmodel.TextValue4.IsNotNullAndNotEmpty() ? newmodel.TextValue4 : model.TextValue4;
            model.TextValue5 = newmodel.TextValue5.IsNotNullAndNotEmpty() ? newmodel.TextValue5 : model.TextValue5;
            model.TextValue6 = newmodel.TextValue6.IsNotNullAndNotEmpty() ? newmodel.TextValue6 : model.TextValue6;
            model.TextValue7 = newmodel.TextValue7.IsNotNullAndNotEmpty() ? newmodel.TextValue7 : model.TextValue7;
            model.TextValue8 = newmodel.TextValue8.IsNotNullAndNotEmpty() ? newmodel.TextValue8 : model.TextValue8;
            model.TextValue9 = newmodel.TextValue9.IsNotNullAndNotEmpty() ? newmodel.TextValue9 : model.TextValue9;
            model.TextValue10 = newmodel.TextValue10.IsNotNullAndNotEmpty() ? newmodel.TextValue10 : model.TextValue10;

            model.DropdownValue1 = newmodel.DropdownValue1.IsNotNullAndNotEmpty() ? newmodel.DropdownValue1 : model.DropdownValue1;
            model.DropdownValue2 = newmodel.DropdownValue2.IsNotNullAndNotEmpty() ? newmodel.DropdownValue2 : model.DropdownValue2;
            model.DropdownValue3 = newmodel.DropdownValue3.IsNotNullAndNotEmpty() ? newmodel.DropdownValue3 : model.DropdownValue3;
            model.DropdownValue4 = newmodel.DropdownValue4.IsNotNullAndNotEmpty() ? newmodel.DropdownValue4 : model.DropdownValue4;
            model.DropdownValue5 = newmodel.DropdownValue5.IsNotNullAndNotEmpty() ? newmodel.DropdownValue5 : model.DropdownValue5;
            model.DropdownValue6 = newmodel.DropdownValue6.IsNotNullAndNotEmpty() ? newmodel.DropdownValue6 : model.DropdownValue6;
            model.DropdownValue7 = newmodel.DropdownValue7.IsNotNullAndNotEmpty() ? newmodel.DropdownValue7 : model.DropdownValue7;
            model.DropdownValue8 = newmodel.DropdownValue8.IsNotNullAndNotEmpty() ? newmodel.DropdownValue8 : model.DropdownValue8;
            model.DropdownValue9 = newmodel.DropdownValue9.IsNotNullAndNotEmpty() ? newmodel.DropdownValue9 : model.DropdownValue9;
            model.DropdownValue10 = newmodel.DropdownValue10.IsNotNullAndNotEmpty() ? newmodel.DropdownValue10 : model.DropdownValue10;

            model.DropdownDisplayValue1 = newmodel.DropdownDisplayValue1.IsNotNullAndNotEmpty() ? newmodel.DropdownDisplayValue1 : model.DropdownDisplayValue1;
            model.DropdownDisplayValue2 = newmodel.DropdownDisplayValue2.IsNotNullAndNotEmpty() ? newmodel.DropdownDisplayValue2 : model.DropdownDisplayValue2;
            model.DropdownDisplayValue3 = newmodel.DropdownDisplayValue3.IsNotNullAndNotEmpty() ? newmodel.DropdownDisplayValue3 : model.DropdownDisplayValue3;
            model.DropdownDisplayValue4 = newmodel.DropdownDisplayValue4.IsNotNullAndNotEmpty() ? newmodel.DropdownDisplayValue4 : model.DropdownDisplayValue4;
            model.DropdownDisplayValue5 = newmodel.DropdownDisplayValue5.IsNotNullAndNotEmpty() ? newmodel.DropdownDisplayValue5 : model.DropdownDisplayValue5;
            model.DropdownDisplayValue6 = newmodel.DropdownDisplayValue6.IsNotNullAndNotEmpty() ? newmodel.DropdownDisplayValue6 : model.DropdownDisplayValue6;
            model.DropdownDisplayValue7 = newmodel.DropdownDisplayValue7.IsNotNullAndNotEmpty() ? newmodel.DropdownDisplayValue7 : model.DropdownDisplayValue7;
            model.DropdownDisplayValue8 = newmodel.DropdownDisplayValue8.IsNotNullAndNotEmpty() ? newmodel.DropdownDisplayValue8 : model.DropdownDisplayValue8;
            model.DropdownDisplayValue9 = newmodel.DropdownDisplayValue9.IsNotNullAndNotEmpty() ? newmodel.DropdownDisplayValue9 : model.DropdownDisplayValue9;
            model.DropdownDisplayValue10 = newmodel.DropdownDisplayValue10.IsNotNullAndNotEmpty() ? newmodel.DropdownDisplayValue10 : model.DropdownDisplayValue10;

            model.DatePickerValue1 = newmodel.DatePickerValue1.IsNotNull() ? newmodel.DatePickerValue1 : model.DatePickerValue1;
            model.DatePickerValue2 = newmodel.DatePickerValue2.IsNotNull() ? newmodel.DatePickerValue2 : model.DatePickerValue2;
            model.DatePickerValue3 = newmodel.DatePickerValue3.IsNotNull() ? newmodel.DatePickerValue3 : model.DatePickerValue3;
            model.DatePickerValue4 = newmodel.DatePickerValue4.IsNotNull() ? newmodel.DatePickerValue4 : model.DatePickerValue4;
            model.DatePickerValue5 = newmodel.DatePickerValue5.IsNotNull() ? newmodel.DatePickerValue5 : model.DatePickerValue5;
            model.DatePickerValue6 = newmodel.DatePickerValue6.IsNotNull() ? newmodel.DatePickerValue6 : model.DatePickerValue6;
            model.DatePickerValue7 = newmodel.DatePickerValue7.IsNotNull() ? newmodel.DatePickerValue7 : model.DatePickerValue7;
            model.DatePickerValue8 = newmodel.DatePickerValue8.IsNotNull() ? newmodel.DatePickerValue8 : model.DatePickerValue8;
            model.DatePickerValue9 = newmodel.DatePickerValue9.IsNotNull() ? newmodel.DatePickerValue9 : model.DatePickerValue9;
            model.DatePickerValue10 = newmodel.DatePickerValue10.IsNotNull() ? newmodel.DatePickerValue10 : model.DatePickerValue10;

            model.AttachmentCode1 = newmodel.AttachmentCode1.IsNotNullAndNotEmpty() ? newmodel.AttachmentCode1 : model.AttachmentCode1;
            model.AttachmentCode2 = newmodel.AttachmentCode2.IsNotNullAndNotEmpty() ? newmodel.AttachmentCode2 : model.AttachmentCode2;
            model.AttachmentCode3 = newmodel.AttachmentCode3.IsNotNullAndNotEmpty() ? newmodel.AttachmentCode3 : model.AttachmentCode3;
            model.AttachmentCode4 = newmodel.AttachmentCode4.IsNotNullAndNotEmpty() ? newmodel.AttachmentCode4 : model.AttachmentCode4;
            model.AttachmentCode5 = newmodel.AttachmentCode5.IsNotNullAndNotEmpty() ? newmodel.AttachmentCode5 : model.AttachmentCode5;
            model.AttachmentCode6 = newmodel.AttachmentCode6.IsNotNullAndNotEmpty() ? newmodel.AttachmentCode6 : model.AttachmentCode6;
            model.AttachmentCode7 = newmodel.AttachmentCode7.IsNotNullAndNotEmpty() ? newmodel.AttachmentCode7 : model.AttachmentCode7;
            model.AttachmentCode8 = newmodel.AttachmentCode8.IsNotNullAndNotEmpty() ? newmodel.AttachmentCode8 : model.AttachmentCode8;
            model.AttachmentCode9 = newmodel.AttachmentCode9.IsNotNullAndNotEmpty() ? newmodel.AttachmentCode9 : model.AttachmentCode9;
            model.AttachmentCode10 = newmodel.AttachmentCode10.IsNotNullAndNotEmpty() ? newmodel.AttachmentCode10 : model.AttachmentCode10;

            model.AttachmentValue1 = newmodel.AttachmentValue1.IsNotNullAndNotEmpty() ? newmodel.AttachmentValue1 : model.AttachmentValue1;
            model.AttachmentValue2 = newmodel.AttachmentValue2.IsNotNullAndNotEmpty() ? newmodel.AttachmentValue2 : model.AttachmentValue2;
            model.AttachmentValue3 = newmodel.AttachmentValue3.IsNotNullAndNotEmpty() ? newmodel.AttachmentValue3 : model.AttachmentValue3;
            model.AttachmentValue4 = newmodel.AttachmentValue4.IsNotNullAndNotEmpty() ? newmodel.AttachmentValue4 : model.AttachmentValue4;
            model.AttachmentValue5 = newmodel.AttachmentValue5.IsNotNullAndNotEmpty() ? newmodel.AttachmentValue5 : model.AttachmentValue5;
            model.AttachmentValue6 = newmodel.AttachmentValue6.IsNotNullAndNotEmpty() ? newmodel.AttachmentValue6 : model.AttachmentValue6;
            model.AttachmentValue7 = newmodel.AttachmentValue7.IsNotNullAndNotEmpty() ? newmodel.AttachmentValue7 : model.AttachmentValue7;
            model.AttachmentValue8 = newmodel.AttachmentValue8.IsNotNullAndNotEmpty() ? newmodel.AttachmentValue8 : model.AttachmentValue8;
            model.AttachmentValue9 = newmodel.AttachmentValue9.IsNotNullAndNotEmpty() ? newmodel.AttachmentValue9 : model.AttachmentValue9;
            model.AttachmentValue10 = newmodel.AttachmentValue10.IsNotNullAndNotEmpty() ? newmodel.AttachmentValue10 : model.AttachmentValue10;
        }

        //public async Task<IActionResult> GetBulkApprovalMenuItem(string id, string type, string parentId,string userRoleId, string userId, string stageName, string stageId,string batchId)
        //{
        //    var result = await _taskBusiness.GetBulkApprovalMenuItem(id, type, parentId, userRoleId, _userContext.UserId, _userContext.UserRoleIds, stageName, stageId,batchId);
        //    var model = result.ToList();
        ////    return Json(model);
        //}

        //[HttpPost]
        //public async Task<ActionResult> ManageBusinessDiagram(BusinessDiagramViewModel model)
        //{
        //    var res = await _tBusiness.ManageBusinessDiagramTask(model);
        //    return Json(res);
        //}
        public async Task<IActionResult> GetTaskSummary()
        {
            var result = await _tBusiness.GetTaskSummary(_userContext.PortalId, _userContext.UserId);
            return Json(result);
        }
    }
}
