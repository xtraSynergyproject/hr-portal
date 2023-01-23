using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class RecTaskBusiness : BusinessBase<RecTaskViewModel, RecTask>, IRecTaskBusiness
    {
        private readonly IRepositoryQueryBase<RecTaskViewModel> _queryRepo;
        private readonly IUserContext _userContext;
        private IRepositoryBase<RecTaskViewModel, RecTaskVersion> _repoVersion;
        private readonly IApplicationBusiness _appBusiness;
        private readonly IMasterBusiness _masterBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IPushNotificationBusiness _notifyBusiness;
        private readonly IConfiguration _configuration;
        private readonly ICandidateProfileBusiness _candidateBusiness;
        private readonly IUserRoleBusiness _userRoleBusiness;
        private readonly IServiceProvider _serviceProvider;
        //private readonly IHangfireScheduler _hangfireScheduler;
        public RecTaskBusiness(IRepositoryBase<RecTaskViewModel, RecTask> repo, IMapper autoMapper, IRepositoryQueryBase<RecTaskViewModel> queryRepo,
            IRepositoryBase<RecTaskViewModel, RecTaskVersion> repoVersion, IUserContext userContext, IApplicationBusiness appBusiness,
            IUserBusiness userBusiness, IConfiguration configuration, IPushNotificationBusiness notifyBusiness, ICandidateProfileBusiness candidateBusiness,
            IUserRoleBusiness userRoleBusiness, IMasterBusiness masterBusiness
             //, IHangfireScheduler hangfireScheduler
             , IServiceProvider serviceProvider
            ) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _userContext = userContext;
            _repoVersion = repoVersion;
            _appBusiness = appBusiness;
            _userBusiness = userBusiness;
            _configuration = configuration;
            _notifyBusiness = notifyBusiness;
            _candidateBusiness = candidateBusiness;
            _userRoleBusiness = userRoleBusiness;
            _masterBusiness = masterBusiness;
            _serviceProvider = serviceProvider;
            //_hangfireScheduler = hangfireScheduler;
        }

        public async override Task<CommandResult<RecTaskViewModel>> Create(RecTaskViewModel model, bool autoCommit = true)
        {

            var result = await base.Create(model, autoCommit);
            return CommandResult<RecTaskViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<CommandResult<RecTaskViewModel>> Createversion(RecTaskViewModel model)
        {

            var result = await _repoVersion.Create(model);
            return CommandResult<RecTaskViewModel>.Instance(model, true, "");
        }
        public async Task<CommandResult<RecTaskViewModel>> AssignTaskForJobAdvertisement(string referenceId, string JobName, DateTime CreatedDate, string assignTo)
        {
            var model = await GetTaskDetails(new RecTaskViewModel { TemplateCode = "JOB_ADVERTISEMENT_APPROVAL", ActiveUserId = _userContext.UserId, ReferenceTypeCode = ReferenceTypeEnum.REC_JobAdvertisement, ReferenceTypeId = referenceId });
            model.Subject = JobName + ' ' + CreatedDate;
            model.Id = Guid.NewGuid().ToString();
            model.DataAction = DataActionEnum.Create;
            model.TemplateAction = NtsActionEnum.Submit;
            model.ActiveUserId = _userContext.UserId;
            model.AssigneeUserId = assignTo.IsNotNullAndNotEmpty() ? assignTo : model.AssigneeUserId;
            //model.AssigneeTeamId = teamId.IsNotNullAndNotEmpty() ? teamId : model.AssigneeTeamId;
            model.AssignToType = model.AssigneeUserId.IsNotNullAndNotEmpty() ? AssignToTypeEnum.User : AssignToTypeEnum.Team;
            model.OwnerUserId = _userContext.UserId;
            model.RequestedUserId = _userContext.UserId;
            var result = await ManageTask(model);
            return result;
        }


        public async Task<RecTaskViewModel> Manage(RecTaskViewModel model)
        {
            try
            {

                if (model.DataAction == DataActionEnum.Create)
                {
                    if (model.TemplateAction == NtsActionEnum.Submit)
                    {
                        model.TaskStatusName = "In Progress";
                        model.TaskStatusCode = "INPROGRESS";
                    }
                    else if (model.TemplateAction == NtsActionEnum.Draft)
                    {
                        model.TaskStatusName = "Draft";
                        model.TaskStatusCode = "DRAFT";
                    }
                    var Taskresult = await Create(model);
                    if (Taskresult.IsSuccess)
                    {
                        return model;
                    }

                    else
                    {
                        return model;
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {

                    if (model.TemplateAction == NtsActionEnum.Submit)
                    {
                        model.TaskStatusName = "In Progress";
                        model.TaskStatusCode = "INPROGRESS";
                    }
                    else if (model.TemplateAction == NtsActionEnum.Complete)
                    {
                        model.TaskStatusName = "Completed";
                        model.TaskStatusCode = "COMPLETED";
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
                    var Taskresult = await Edit(model);
                    if (Taskresult.IsSuccess)
                    {
                        return model;
                    }
                    else
                    {
                        return model;
                    }

                }


                return model;
            }
            catch (Exception ex)
            {

                return model;
            }


        }
        public async Task<CommandResult<RecTaskViewModel>> ManageTask(RecTaskViewModel model, bool autoCommit = true)
        {

            if (model.TemplateAction == NtsActionEnum.Submit)
            {
                model.TaskStatusName = "In Progress";
                model.TaskStatusCode = "INPROGRESS";
            }
            else if (model.TemplateAction == NtsActionEnum.Draft)
            {
                model.TaskStatusName = "Draft";
                model.TaskStatusCode = "DRAFT";
            }
            var result = await base.Create(model, autoCommit);
            if (model.TemplateAction != NtsActionEnum.Draft)
            {
                await CreateTaskNotification(model);
            }
            return CommandResult<RecTaskViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


        public async Task<NotificationViewModel> CreateTaskNotification(RecTaskViewModel model)
        {
            try
            {
                var notificationModel = new NotificationViewModel();
                var userModel = await _userBusiness.GetSingleById(model.AssigneeUserId);
                var ownerModel = await _userBusiness.GetSingleById(model.OwnerUserId);
                var takeActionLink = /*_configuration.GetValue<string>("ApplicationBaseUrl") +*/ "/CMS/task/Index?taskId=" + model.Id + "&assignTo=" + model.AssigneeUserId + "&teamId=" + model.AssigneeTeamId + "&isPopUp=" + true;
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
                                                        GetTaskUdf(model),
                                                        model.TextBoxDisplay1.IsNotNullAndNotEmpty() ? model.TextBoxDisplay1 + " : " + (model.TextBoxDisplayType1 == NtsFieldType.NTS_Attachment ? model.AttachmentValue1 : ((model.TextBoxDisplayType1 == NtsFieldType.NTS_DatePicker || model.TextBoxDisplayType1 == NtsFieldType.NTS_DateTimePicker) ? model.DatePickerValue1 : model.TextValue1)) : model.TextValue1,
                                                        "<a href = '#' onclick='openNotificationAction(\"" + serviceTakeActionLink + "\")' > Take Action </a></div> ");
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
                                                        GetTaskUdf(model),
                                                        "<a href = '#' onclick='openNotificationAction(\"" + serviceTakeActionLink + "\")' > Take Action </a></div> ");
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
                                                        GetTaskUdf(model),
                                                        "<a href = '#' onclick='openNotificationAction(\"" + serviceTakeActionLink + "\")' > Take Action </a></div> ");
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
                                                        GetTaskUdf(model),
                                                        "<a href ='#' onclick='openNotificationAction(\"" + serviceTakeActionLink + "\")' > Take Action </a></div> ");
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
                                                        GetTaskUdf(model),
                                                        "<a href ='#' onclick='openNotificationAction(\"" + serviceTakeActionLink + "\")' > Take Action </a></div> ");
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
                                                        GetTaskUdf(model),
                                                        "<a href = '#' onclick='openNotificationAction(\"" + takeActionLink + "\")'> Take Action </a></div> ");
                            notificationModel.From = ownerModel.Email;
                            notificationModel.ToUserId = model.AssigneeUserId;
                            notificationModel.To = userModel.Email;
                            if (model.TemplateCode == "SL_BATCH_SEND_HM")
                            {
                                notificationModel.Subject = "Assigned Task | " + DateTime.Now + " | " + model.Subject + " | " + userModel.Email;
                                //notificationModel.Subject = model.Subject;
                                notificationModel.Body = string.Concat("<div><h5>Hello ", userModel.Name, "</h5>Task Details:<br><br>Task No: ", model.TaskNo,
                                                            "<br>Subject : ", model.Subject, "<br><br>",
                                                            GetTaskUdf(model),
                                                            "<a href = '#' onclick='openHMBatch()'> Take Action </a></div> ");
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
                                                        GetTaskUdf(model),
                                                        "<a href =  '#' onclick='openNotificationAction(\"" + takeActionLink + "\")' > Take Action </a></div> ");
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
                                                        GetTaskUdf(model),
                                                        "<a href =  '#' onclick='openNotificationAction(\"" + takeActionLink + "\")'> Take Action </a></div> ");
                            notificationModel.From = userModel.Email;
                            notificationModel.ToUserId = model.OwnerUserId;
                            notificationModel.To = ownerModel.Email;
                            if (model.TemplateCode == "SL_BATCH_SEND_HM")
                            {
                                notificationModel.Subject = "Completed Task | " + DateTime.Now + " | " + model.Subject + " | " + userModel.Email;
                                //notificationModel.Subject = model.Subject;
                                notificationModel.Body = string.Concat("<div><h5>Hello ", userModel.Name, "</h5>Task Details:<br><br>Task No: ", model.TaskNo,
                                                            "<br>Subject : ", model.Subject, "<br><br>",
                                                            GetTaskUdf(model),
                                                            "<a href = '#' onclick='openHMBatch()'> Take Action </a></div> ");
                                notificationModel.From = ownerModel.Email;
                                notificationModel.ToUserId = model.AssigneeUserId;
                                notificationModel.To = userModel.Email;
                            }
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
                            notificationModel.Body = string.Concat("<div><h4>Hello ", ownerModel.Name, "</h4>Task Details:<br><br>Task No: ", model.TaskNo,
                                                        "<br>Subject : ", model.Subject, "<br><br>",
                                                        GetTaskUdf(model),
                                                        "<a href =  '#' onclick='openNotificationAction(\"" + takeActionLink + "\")' > Take Action </a></div> ");
                            notificationModel.From = userModel.Email;
                            notificationModel.ToUserId = model.OwnerUserId;
                            notificationModel.To = ownerModel.Email;
                            if (model.TemplateCode == "SCHEDULE_INTERVIEW_CANDIDATE")
                            {
                                notificationModel.Subject = "Rejected Task | " + DateTime.Now + " | " + string.Concat(model.Subject, "-", model.TextValue3) + " | " + userModel.Email;
                                notificationModel.Body = string.Concat("<div><h4>Hello ", ownerModel.Name, "</h4>Task Details:<br><br>Task No: ", model.TaskNo,
                                                            "<br>Subject : ", notificationModel.Subject, "<br><br>",
                                                            "<a href = '#' onclick='openNotificationAction(\"" + takeActionLink + "\")' > Take Action </a></div> ");
                                notificationModel.From = userModel.Email;
                                notificationModel.ToUserId = model.OwnerUserId;
                                notificationModel.To = ownerModel.Email;
                            }
                            if (model.TemplateCode == "INTENT_TO_OFFER")
                            {
                                notificationModel.Subject = "Rejected Task | " + DateTime.Now + " | " + string.Concat(model.Subject, "-", model.TextValue2) + " | " + userModel.Email;
                                notificationModel.Body = string.Concat("<div><h4>Hello ", ownerModel.Name, "</h4>Task Details:<br><br>Task No: ", model.TaskNo,
                                                            "<br>Subject : ", notificationModel.Subject, "<br><br>",
                                                            "<a href = '#' onclick='openNotificationAction(\"" + takeActionLink + "\")' > Take Action </a></div> ");
                                notificationModel.From = userModel.Email;
                                notificationModel.ToUserId = model.OwnerUserId;
                                notificationModel.To = ownerModel.Email;
                            }
                        }
                        else if (model.TaskStatusCode == "CANCELLED")
                        {
                            notificationModel.Subject = "Cancelled Task | " + DateTime.Now + " | " + model.Subject + " | " + userModel.Email;
                            notificationModel.Subject = model.Subject;
                            notificationModel.Body = string.Concat("<div><h4>Hello ", ownerModel.Name, "</h4>Task Details:<br><br>Task No: ", model.TaskNo,
                                                        "<br>Subject : ", model.Subject, "<br><br>",
                                                        GetTaskUdf(model),
                                                        "<a href =  '#' onclick='openNotificationAction(\"" + takeActionLink + "\")' > Take Action </a></div> ");
                            notificationModel.From = userModel.Email;
                            notificationModel.ToUserId = model.OwnerUserId;
                            notificationModel.To = ownerModel.Email;
                            if (model.TemplateCode == "SCHEDULE_INTERVIEW_CANDIDATE")
                            {
                                notificationModel.Subject = "Rejected Task | " + DateTime.Now + " | " + string.Concat(model.Subject, "-", model.TextValue3) + " | " + userModel.Email;
                                notificationModel.Body = string.Concat("<div><h4>Hello ", ownerModel.Name, "</h4>Task Details:<br><br>Task No: ", model.TaskNo,
                                                            "<br>Subject : ", notificationModel.Subject, "<br><br>",
                                                            "<a href = '#' onclick='openNotificationAction(\"" + takeActionLink + "\")' > Take Action </a></div> ");
                                notificationModel.From = userModel.Email;
                                notificationModel.ToUserId = model.OwnerUserId;
                                notificationModel.To = ownerModel.Email;
                            }
                            if (model.TemplateCode == "INTENT_TO_OFFER")
                            {
                                notificationModel.Subject = "Rejected Task | " + DateTime.Now + " | " + string.Concat(model.Subject, "-", model.TextValue2) + " | " + userModel.Email;
                                notificationModel.Body = string.Concat("<div><h4>Hello ", ownerModel.Name, "</h4>Task Details:<br><br>Task No: ", model.TaskNo,
                                                            "<br>Subject : ", notificationModel.Subject, "<br><br>",
                                                            "<a href = '#' onclick='openNotificationAction(\"" + takeActionLink + "\")' > Take Action </a></div> ");
                                notificationModel.From = userModel.Email;
                                notificationModel.ToUserId = model.OwnerUserId;
                                notificationModel.To = ownerModel.Email;
                            }
                        }
                        else if (model.TaskStatusCode == "CLOSED")
                        {
                            notificationModel.Subject = "Cancelled Task | " + DateTime.Now + " | " + model.Subject + " | " + userModel.Email;
                            //notificationModel.Subject = model.Subject;
                            notificationModel.Body = string.Concat("<div><h4>Hello ", ownerModel.Name, "</h4>Task Details:<br><br>Task No: ", model.TaskNo,
                                                        "<br>Subject : ", model.Subject, "<br><br>",
                                                        GetTaskUdf(model),
                                                        "<a href =  '#' onclick='openNotificationAction(\"" + takeActionLink + "\")' > Take Action </a></div> ");
                            notificationModel.From = userModel.Email;
                            notificationModel.ToUserId = model.OwnerUserId;
                            notificationModel.To = ownerModel.Email;
                        }
                        break;
                    default:
                        break;
                }

                //notificationModel.To = "arshad@extranet.ae;mthamil107@gmail.com";

                var result = await _notifyBusiness.Create(notificationModel);
                if (result.IsSuccess)
                {
                    try
                    {
                        var hangfireScheduler = _serviceProvider.GetService<IHangfireScheduler>();
                        BackgroundJob.Enqueue<HangfireScheduler>(x => x.SendEmailUsingHangfire(result.Item.EmailUniqueId));
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
        private string GetTaskUdf(RecTaskViewModel model)
        {
            try
            {
                var body = new System.Text.StringBuilder();
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
        public async override Task<CommandResult<RecTaskViewModel>> Edit(RecTaskViewModel model, bool autoCommit = true)
        {

            var result = await base.Edit(model, autoCommit);
            return CommandResult<RecTaskViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<RecTaskViewModel> GetTaskDetails(RecTaskViewModel model)
        {
            var userId = model.ActiveUserId;
            var result1 = new RecTaskViewModel();
            if (model.Id.IsNotNullAndNotEmpty())
            {
                string query1 = @$"SELECT t.* FROM public.""RecTask"" as t 
                            
                            where t.""Id""='{model.Id}'
                            ";

                result1 = await _queryRepo.ExecuteQuerySingle<RecTaskViewModel>(query1, null);
            }
            var templateCode = model.TemplateCode.IsNullOrEmpty() ? result1.TemplateCode : model.TemplateCode;
            string query = @$"SELECT  * FROM public.""RecTaskTemplate"" as tt
                            where tt.""TemplateCode""='{templateCode}'
                            ";
            var result = await _queryRepo.ExecuteQuerySingle<RecTaskViewModel>(query, null);
            if (result.IsNotNull())
            {
                result.TemplateId = result.Id;
                result.Id = "";
            }
            if (result.IsNotNull() && model.IsNotNull() && model.Id.IsNullOrEmpty())
            {
                if (model.ReferenceTypeId.IsNotNullAndNotEmpty())
                {
                    result.ReferenceTypeCode = model.ReferenceTypeCode;
                    result.ReferenceTypeId = model.ReferenceTypeId;
                }
                result.AssignToType = result.AssignToType;
                result.TemplateAction = NtsActionEnum.Draft;
                result.TaskStatusName = "Draft";
                result.TaskStatusCode = "DRAFT";
                await MapTemplate(null, result);
                result.AssigneeUserId = result.AssignedToUserId;
                result.AssigneeTeamId = result.AssignedToTeamId;
                result.VersionNo = 1;
                result.StartDate = result.StartDate >= DateTime.Now ? result.StartDate : DateTime.Now;
                result.DueDate = result.StartDate.Value.AddDays(Convert.ToDouble(result.SLA));
                result.DisplayMode = FieldDisplayModeEnum.Editable;
                result.TemplateUserType = NtsUserTypeEnum.Owner;
                result.DataAction = DataActionEnum.Create;
                result.CanEditHeader = true;
                //result.DraftButton = true;
                result.SaveButton = true;
                if (result.NtsType == NtsTypeEnum.Task || result.NtsType == null)
                {
                    result.TaskNo = await GenerateNextTaskNo();

                }
                else if (result.NtsType == NtsTypeEnum.Service)
                {
                    result.TaskNo = await GenerateNextServiceNo();
                    result.OwnerUserId = userId;
                    result.AssigneeUserId = userId;
                }
                if (result.AssignToType == AssignToTypeEnum.Candidate)
                {
                    if (model.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(model.ReferenceTypeId);
                        var app = await _appBusiness.GetSingleById(service.ReferenceTypeId);
                        if (app != null)
                        {
                            var user = await _candidateBusiness.GetSingleById(app.CandidateProfileId);
                            var userid = "";
                            if (user != null)
                            {
                                userid = user.UserId;

                            }
                            result.AssigneeUserId = app.SourceFrom == "Agency" ? app.AgencyId : userid;
                        }
                    }
                    else
                    {
                        var app = await _appBusiness.GetSingleById(model.ReferenceTypeId);
                        if (app != null)
                        {
                            var user = await _candidateBusiness.GetSingleById(app.CandidateProfileId);
                            var userid = "";
                            if (user != null)
                            {
                                userid = user.UserId;

                            }
                            result.AssigneeUserId = app.SourceFrom == "Agency" ? app.AgencyId : userid;
                        }
                    }

                }
                if (result.AssignToType == AssignToTypeEnum.HiringManager)
                {
                    if (model.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(model.ReferenceTypeId);
                        var app = await _appBusiness.GetSingleById(service.ReferenceTypeId);
                        if (app != null)
                        {
                            var user = await _appBusiness.GetHiringManagerByApplication(app.Id);
                            var userid = "";
                            if (user != null)
                            {
                                userid = user.Id;

                            }
                            result.AssigneeUserId = userid;
                        }
                    }
                    else
                    {
                        var app = await _appBusiness.GetSingleById(model.ReferenceTypeId);
                        if (app != null)
                        {
                            var user = await _appBusiness.GetHiringManagerByApplication(app.Id);
                            var userid = "";
                            if (user != null)
                            {
                                userid = user.Id;

                            }
                            result.AssigneeUserId = userid;
                        }
                    }

                }
                if (result.AssignToType == AssignToTypeEnum.HeadOfDepartment)
                {
                    if (model.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(model.ReferenceTypeId);
                        var app = await _appBusiness.GetSingleById(service.ReferenceTypeId);
                        if (app != null)
                        {
                            var user = await _appBusiness.GetHeadOfDepartmentByApplication(app.Id);
                            var userid = "";
                            if (user != null)
                            {
                                userid = user.Id;

                            }
                            result.AssigneeUserId = userid;
                        }
                    }
                    else
                    {
                        var app = await _appBusiness.GetSingleById(model.ReferenceTypeId);
                        if (app != null)
                        {
                            var user = await _appBusiness.GetHeadOfDepartmentByApplication(app.Id);
                            var userid = "";
                            if (user != null)
                            {
                                userid = user.Id;

                            }
                            result.AssigneeUserId = userid;
                        }
                    }

                }
                SetDisplayMode(result);

                if (result.Subject.IsNotNullAndNotEmpty() && result.Subject.Contains("{batchName}"))
                {
                    if (result.ReferenceTypeCode == ReferenceTypeEnum.REC_WorkerPoolBatch)
                    {
                        var batch = await _repo.GetSingleGlobal<BatchViewModel, Batch>(x => x.Id == result.ReferenceTypeId);
                        var link = result.Subject.Replace("{batchName}", batch.IsNotNull() ? batch.Name : "");
                        result.Subject = link;

                    }
                    else if (result.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(result.ReferenceTypeId);
                        if (service.IsNotNull())
                        {
                            var batch = await _repo.GetSingleGlobal<BatchViewModel, Batch>(x => x.Id == service.ReferenceTypeId);
                            var link = result.Subject.Replace("{batchName}", batch.IsNotNull() ? batch.Name : "");
                            result.Subject = link;
                        }

                    }

                }
                if (result.Subject.IsNotNullAndNotEmpty() && result.Subject.Contains("{employeeType}"))
                {
                    if (result.ReferenceTypeCode == ReferenceTypeEnum.REC_Application)
                    {
                        var app = await _appBusiness.GetApplicationDetail(result.ReferenceTypeId);
                        var link = result.Subject.Replace("{employeeType}", app.IsNotNull() ? (app.ManpowerTypeName == "Staff" ? "Staff – Receive & Attach Ticket, Attach Hotel quarantine booking" : "Worker – Email sent to agency with ticket & hotel quarantine") : "");
                        result.Subject = link;

                    }
                    else if (result.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(result.ReferenceTypeId);
                        if (service.IsNotNull())
                        {
                            var app = await _appBusiness.GetApplicationDetail(service.ReferenceTypeId);
                            var link = result.Subject.Replace("{employeeType}", app.IsNotNull() ? (app.ManpowerTypeName == "Staff" ? "Staff – Receive & Attach Ticket, Attach Hotel quarantine booking" : "Worker – Email sent to agency with ticket & hotel quarantine") : "");
                            result.Subject = link;
                        }

                    }

                }
                return result;
            }
            else if (model.TaskVersionId != null && model.TaskVersionId.Value > 0)
            {

                return await GetTaskVersionDetails(model);
            }
            else
            {
                if (result1.IsNotNull())
                {
                    if (model.TaskVersionId == 0)
                    {
                        result1.TemplateAction = NtsActionEnum.Draft;
                        result1.TaskStatusCode = "DRAFT";
                        result1.TaskStatusName = "Draft";
                        result1.TaskVersionId = 0;
                        result1.VersionNo++;

                    }
                    if (result1.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(result1.ReferenceTypeId);
                        if (service.IsNotNull() && service.ReferenceTypeCode == ReferenceTypeEnum.REC_Application)
                        {
                            var application = await _appBusiness.GetSingleById(service.ReferenceTypeId);
                            if (application.IsNotNull())
                            {
                                if (application.FirstName.IsNotNullAndNotEmpty())
                                {
                                    result1.CandidateName = application.FirstName;
                                }
                                if (application.JobId.IsNotNullAndNotEmpty())
                                {
                                    var job = await _masterBusiness.GetJobNameById(application.JobId);
                                    if (job.IsNotNull() && job.Name.IsNotNullAndNotEmpty())
                                    {
                                        result1.JobName = job.Name;
                                    }
                                }
                                if (application.BatchId.IsNotNullAndNotEmpty())
                                {
                                    var org = await _masterBusiness.GetOrgNameByBatchId(application.BatchId);
                                    if (org.IsNotNull() && org.Name.IsNotNullAndNotEmpty())
                                    {
                                        result1.OrgUnitName = org.Name;
                                    }
                                }
                                if (application.GaecNo.IsNotNullAndNotEmpty())
                                {
                                    result1.GaecNo = application.GaecNo;
                                }

                            }
                        }
                    }
                    result1.TaskVersionId = model.TaskVersionId;
                    await MapTemplate(result1, result);
                    result1.TemplateAction = result1.TaskStatusCode == "DRAFT" ? NtsActionEnum.Draft : NtsActionEnum.Submit;
                    result1.DisplayMode = FieldDisplayModeEnum.Readonly;
                    if (result1.OwnerUserId == userId)
                    {
                        result1.TemplateUserType = NtsUserTypeEnum.Owner;
                    }
                    else if (result1.AssigneeUserId == userId)
                    {
                        result1.TemplateUserType = NtsUserTypeEnum.Assignee;
                    }
                    if (result1.OwnerUserId == userId && result1.TaskStatusCode == "DRAFT")
                    {
                        result1.TemplateUserType = NtsUserTypeEnum.Owner;
                    }
                    else if (result1.AssigneeUserId == userId && (result1.TaskStatusCode == "INPROGRESS" || result1.TaskStatusCode == "OVERDUE"))
                    {
                        result1.TemplateUserType = NtsUserTypeEnum.Assignee;
                    }
                    if (result1.OwnerUserId == userId && result1.NtsType == NtsTypeEnum.Service)
                    {
                        result1.TemplateUserType = NtsUserTypeEnum.Owner;
                    }
                    result1.DataAction = DataActionEnum.Read;

                    if (result1.TemplateAction == NtsActionEnum.Draft && result1.TemplateUserType == NtsUserTypeEnum.Owner)
                    {
                        if (model.TaskVersionId == 0)
                        {
                            result1.SaveNewVersionButton = true;
                            result1.SaveButton = false;
                            result1.DataAction = DataActionEnum.Edit;
                        }
                        else
                        {
                            result1.SaveButton = true;
                        }
                        result1.DisplayMode = FieldDisplayModeEnum.Editable;
                        result1.CanEditHeader = true;
                        //result1.DraftButton = true;                        
                        result1.NotApplicableButton = false;
                        result1.RejectButton = false;
                        result1.CompleteButton = false;
                        result1.CancelButton = false;

                    }
                    else if ((result1.TaskStatusCode == "INPROGRESS" || result1.TaskStatusCode == "OVERDUE") && result1.TemplateUserType == NtsUserTypeEnum.Assignee && result1.NtsType == NtsTypeEnum.Task)
                    {
                        result1.DisplayMode = FieldDisplayModeEnum.Editable;
                        result1.DataAction = DataActionEnum.Edit;
                        result1.CanEditHeader = false;
                        result1.DraftButton = false;
                        result1.SaveButton = false;
                        result1.CompleteButton = true;
                        result1.SaveChangesButton = true;

                    }
                    else if ((result1.TaskStatusCode == "INPROGRESS" || result1.TaskStatusCode == "OVERDUE") && result1.TemplateUserType == NtsUserTypeEnum.Owner && result1.NtsType == NtsTypeEnum.Service)
                    {
                        result1.DisplayMode = FieldDisplayModeEnum.Editable;
                        result1.DataAction = DataActionEnum.Edit;
                        result1.CanEditHeader = false;
                        result1.DraftButton = false;
                        result1.SaveButton = false;
                        result1.CompleteButton = false;
                        result1.CancelButton = true;

                    }
                    else if ((result1.TaskStatusCode == "COMPLETED" || result1.TaskStatusCode == "REJECTED" || result1.TaskStatusCode == "NOTAPPLICABLE" || result1.TaskStatusCode == "CANCELLED") && result1.TemplateUserType == NtsUserTypeEnum.Owner && result1.NtsType == NtsTypeEnum.Service)
                    {
                        result1.DisplayMode = FieldDisplayModeEnum.Readonly;
                        result1.CanEditHeader = false;
                        result1.DraftButton = false;
                        result1.SaveButton = false;
                        result1.CreateNewVersionButton = true;
                        result1.NotApplicableButton = false;
                        result1.RejectButton = false;
                        result1.CompleteButton = false;
                        result1.CancelButton = false;

                    }
                    else if ((result1.TaskStatusCode == "COMPLETED" || result1.TaskStatusCode == "REJECTED" || result1.TaskStatusCode == "NOTAPPLICABLE" || result1.TaskStatusCode == "CANCELLED") && result1.TemplateUserType == NtsUserTypeEnum.Owner && result1.NtsType == NtsTypeEnum.Task && result1.ReferenceTypeCode != ReferenceTypeEnum.NTS_Task)
                    {
                        result1.DisplayMode = FieldDisplayModeEnum.Readonly;
                        result1.CanEditHeader = false;
                        result1.DraftButton = false;
                        result1.SaveButton = false;
                        result1.CreateNewVersionButton = true;
                        result1.NotApplicableButton = false;
                        result1.RejectButton = false;
                        result1.CompleteButton = false;
                        result1.CancelButton = false;

                    }
                    else
                    {
                        result1.DisplayMode = FieldDisplayModeEnum.Readonly;
                        result1.CanEditHeader = false;
                        result1.DraftButton = false;
                        result1.SaveButton = false;
                        result1.CreateNewVersionButton = false;
                        result1.NotApplicableButton = false;
                        result1.RejectButton = false;
                        result1.CompleteButton = false;
                        result1.CancelButton = false;
                    }
                    SetDisplayMode(result1);

                    //if (result.TaskVersionId != null && result.TaskVersionId.Value > 0)
                    //{
                    //    return GetVersionDetails(viewModel);
                    //}


                }

                return result1;
            }

        }
        public async Task<List<RecTaskViewModel>> GetTaskDetailsList(string ids, string templateCode, string userId)
        {
            string query = @$"SELECT  * FROM public.""RecTaskTemplate"" as tt
                            where tt.""TemplateCode""='{templateCode}'
                            ";
            var result = await _queryRepo.ExecuteQuerySingle<RecTaskViewModel>(query, null);
            if (result.IsNotNull())
            {
                result.TemplateId = result.Id;
                result.Id = "";
            }
            //var Ids = string.Join(",", list.Select(x=>x.Id));
            //Ids = Ids.Trim(',');
            ids = "'" + ids.Replace(",", "','") + "'";
            string querytask = @$"SELECT t.* FROM public.""RecTask"" as t where t.""Id"" in ({ids})";

            var resultlist = await _queryRepo.ExecuteQueryList<RecTaskViewModel>(querytask, null);

            var newlist = new List<RecTaskViewModel>();
            foreach (var result1 in resultlist)
            {
                //var userId = result1.ActiveUserId;
                //var result1 = new RecTaskViewModel();
                //if (model.Id.IsNotNullAndNotEmpty())
                //{
                //    string query1 = @$"SELECT t.* FROM public.""RecTask"" as t 

                //            where t.""Id""='{model.Id}'
                //            ";

                //    result1 = await _queryRepo.ExecuteQuerySingle<RecTaskViewModel>(query1, null);
                //}


                if (result1.IsNotNull())
                {
                    if (result1.TaskVersionId == 0)
                    {
                        result1.TemplateAction = NtsActionEnum.Draft;
                        result1.TaskStatusCode = "DRAFT";
                        result1.TaskStatusName = "Draft";
                        result1.TaskVersionId = 0;
                        result1.VersionNo++;

                    }
                    if (result1.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(result1.ReferenceTypeId);
                        if (service.IsNotNull() && service.ReferenceTypeCode == ReferenceTypeEnum.REC_Application)
                        {
                            var application = await _appBusiness.GetSingleById(service.ReferenceTypeId);
                            if (application.IsNotNull())
                            {
                                result1.BatchId = application.BatchId;
                                if (application.FirstName.IsNotNullAndNotEmpty())
                                {
                                    result1.CandidateName = application.FirstName;
                                }
                                if (application.JobId.IsNotNullAndNotEmpty())
                                {
                                    var job = await _masterBusiness.GetJobNameById(application.JobId);
                                    if (job.IsNotNull() && job.Name.IsNotNullAndNotEmpty())
                                    {
                                        result1.JobName = job.Name;
                                    }
                                }
                                if (application.BatchId.IsNotNullAndNotEmpty())
                                {
                                    var org = await _masterBusiness.GetOrgNameByBatchId(application.BatchId);
                                    if (org.IsNotNull() && org.Name.IsNotNullAndNotEmpty())
                                    {
                                        result1.OrgUnitName = org.Name;
                                    }
                                }
                                if (application.GaecNo.IsNotNullAndNotEmpty())
                                {
                                    result1.GaecNo = application.GaecNo;
                                }

                            }
                        }
                    }
                    if (result1.JobName.IsNullOrEmpty() && result1.OrgUnitName.IsNullOrEmpty())
                    {
                        var job = await _masterBusiness.GetJobNameById(result1.DropdownValue1);
                        if (job.IsNotNull() && job.Name.IsNotNullAndNotEmpty())
                        {
                            result1.JobName = job.Name;
                        }
                        var org = await _masterBusiness.GetOrgNameById(result1.DropdownValue2);
                        if (org.IsNotNull() && org.Name.IsNotNullAndNotEmpty())
                        {
                            result1.OrgUnitName = org.Name;
                        }
                    }

                    result1.TaskVersionId = result1.TaskVersionId;
                    await MapTemplate(result1, result);
                    result1.TemplateAction = result1.TaskStatusCode == "DRAFT" ? NtsActionEnum.Draft : NtsActionEnum.Submit;
                    result1.DisplayMode = FieldDisplayModeEnum.Readonly;
                    if (result1.OwnerUserId == userId)
                    {
                        result1.TemplateUserType = NtsUserTypeEnum.Owner;
                    }
                    else if (result1.AssigneeUserId == userId)
                    {
                        result1.TemplateUserType = NtsUserTypeEnum.Assignee;
                    }
                    if (result1.OwnerUserId == userId && result1.TaskStatusCode == "DRAFT")
                    {
                        result1.TemplateUserType = NtsUserTypeEnum.Owner;
                    }
                    else if (result1.AssigneeUserId == userId && result1.TaskStatusCode == "INPROGRESS")
                    {
                        result1.TemplateUserType = NtsUserTypeEnum.Assignee;
                    }
                    if (result1.OwnerUserId == userId && result1.NtsType == NtsTypeEnum.Service)
                    {
                        result1.TemplateUserType = NtsUserTypeEnum.Owner;
                    }
                    result1.DataAction = DataActionEnum.Read;

                    if (result1.TemplateAction == NtsActionEnum.Draft && result1.TemplateUserType == NtsUserTypeEnum.Owner)
                    {
                        if (result1.TaskVersionId == 0)
                        {
                            result1.SaveNewVersionButton = true;
                            result1.SaveButton = false;
                            result1.DataAction = DataActionEnum.Edit;
                        }
                        else
                        {
                            result1.SaveButton = true;
                        }
                        result1.DisplayMode = FieldDisplayModeEnum.Editable;
                        result1.CanEditHeader = true;
                        //result1.DraftButton = true;                        
                        result1.NotApplicableButton = false;
                        result1.RejectButton = false;
                        result1.CompleteButton = false;
                        result1.CancelButton = false;

                    }
                    else if ((result1.TaskStatusCode == "INPROGRESS" || result1.TaskStatusCode == "OVERDUE") && result1.TemplateUserType == NtsUserTypeEnum.Assignee && result1.NtsType == NtsTypeEnum.Task)
                    {
                        result1.DisplayMode = FieldDisplayModeEnum.Editable;
                        result1.DataAction = DataActionEnum.Edit;
                        result1.CanEditHeader = false;
                        result1.DraftButton = false;
                        result1.SaveButton = false;
                        result1.CompleteButton = true;

                    }
                    else if ((result1.TaskStatusCode == "INPROGRESS" || result1.TaskStatusCode == "OVERDUE") && result1.TemplateUserType == NtsUserTypeEnum.Owner && result1.NtsType == NtsTypeEnum.Service)
                    {
                        result1.DisplayMode = FieldDisplayModeEnum.Editable;
                        result1.DataAction = DataActionEnum.Edit;
                        result1.CanEditHeader = false;
                        result1.DraftButton = false;
                        result1.SaveButton = false;
                        result1.CompleteButton = false;
                        result1.CancelButton = true;

                    }
                    else if ((result1.TaskStatusCode == "COMPLETED" || result1.TaskStatusCode == "REJECTED" || result1.TaskStatusCode == "NOTAPPLICABLE" || result1.TaskStatusCode == "CANCELLED") && result1.TemplateUserType == NtsUserTypeEnum.Owner && result1.NtsType == NtsTypeEnum.Service)
                    {
                        result1.DisplayMode = FieldDisplayModeEnum.Readonly;
                        result1.CanEditHeader = false;
                        result1.DraftButton = false;
                        result1.SaveButton = false;
                        result1.CreateNewVersionButton = true;
                        result1.NotApplicableButton = false;
                        result1.RejectButton = false;
                        result1.CompleteButton = false;
                        result1.CancelButton = false;

                    }
                    else if ((result1.TaskStatusCode == "COMPLETED" || result1.TaskStatusCode == "REJECTED" || result1.TaskStatusCode == "NOTAPPLICABLE" || result1.TaskStatusCode == "CANCELLED") && result1.TemplateUserType == NtsUserTypeEnum.Owner && result1.NtsType == NtsTypeEnum.Task && result1.ReferenceTypeCode != ReferenceTypeEnum.NTS_Task)
                    {
                        result1.DisplayMode = FieldDisplayModeEnum.Readonly;
                        result1.CanEditHeader = false;
                        result1.DraftButton = false;
                        result1.SaveButton = false;
                        result1.CreateNewVersionButton = true;
                        result1.NotApplicableButton = false;
                        result1.RejectButton = false;
                        result1.CompleteButton = false;
                        result1.CancelButton = false;

                    }
                    else
                    {
                        result1.DisplayMode = FieldDisplayModeEnum.Readonly;
                        result1.CanEditHeader = false;
                        result1.DraftButton = false;
                        result1.SaveButton = false;
                        result1.CreateNewVersionButton = false;
                        result1.NotApplicableButton = false;
                        result1.RejectButton = false;
                        result1.CompleteButton = false;
                        result1.CancelButton = false;
                    }
                    SetDisplayMode(result1);
                    if (result1.TemplateCode == "WORKER_POOL_HR" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetSingleById(result1.ReferenceTypeId);
                        var applicant = await _appBusiness.GetSingleById(service.ReferenceTypeId);
                        if (applicant.IsNotNull())
                        {
                            result1.TextValue1 = applicant.SalaryOnAppointment;
                        }

                    }
                    if (result1.TemplateCode == "WORKER_SALARY_AGENCY" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var recruiter = service.Where(x => x.TemplateCode == "WORKER_POOL_HR").FirstOrDefault();
                        if (recruiter.IsNotNull())
                        {
                            result1.TextValue1 = recruiter.TextValue1;
                        }

                    }
                    //if (result1.TemplateCode == "TICKET_ATTACH" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    //{
                    //    var service = await GetSingleById(result1.ReferenceTypeId);
                    //    var app = await _appBusiness.GetApplicationDetail(service.ReferenceTypeId);
                    //    if (app.ManpowerTypeCode == "Staff")
                    //    {
                    //        result1.IsRequiredTextBoxDisplay1 = true;
                    //    }
                    //    else
                    //    {
                    //        result1.TextBoxDisplayType1 = NtsFieldType.NTS_Hidden;
                    //    }
                    //}
                    if (result1.TemplateCode == "JOBDESCRIPTION_HM")
                    {
                        if (result1.TextBoxLink3.IsNotNullAndNotEmpty())
                        {
                            result1.TextBoxLink3 = result1.TextBoxLink3.Replace("{jobId}", result1.DropdownValue1);
                            result1.TextBoxLink3 = result1.TextBoxLink3.Replace("{orgId}", result1.DropdownValue2);
                            if (result1.TextBoxLink3.Contains("{TaskStatus}"))
                            {
                                result1.TextBoxLink3 = result1.TextBoxLink3.Replace("{TaskStatus}", result1.TaskStatusCode);
                            }
                        }
                    }
                    if (result1.TemplateCode == "TASK_DIRECT_HIRING")
                    {
                        var service = await GetSingleById(result1.ReferenceTypeId);
                        if (service.IsNotNull())
                        {
                            result1.DropdownValue1 = service.DropdownValue1;
                            result1.DropdownValue2 = service.DropdownValue2;
                        }



                        if (result1.TextBoxLink3.IsNotNullAndNotEmpty())
                        {

                            result1.TextBoxLink3 = result1.TextBoxLink3.Replace("{TaskId}", result1.Id);

                        }

                        if (result1.TextBoxLink4.IsNotNullAndNotEmpty())
                        {
                            result1.TextBoxLink4 = result1.TextBoxLink4.Replace("{JobId}", result1.DropdownValue1);
                            result1.TextBoxLink4 = result1.TextBoxLink4.Replace("{OrgId}", result1.DropdownValue2);
                            result1.TextBoxLink4 = result1.TextBoxLink4.Replace("{TaskId}", result1.Id);

                        }

                    }
                    if (result1.TemplateCode == "SCHEDULE_INTERVIEW_CANDIDATE" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var recruiter = service.Where(x => x.TemplateCode == "SCHEDULE_INTERVIEW_RECRUITER").FirstOrDefault();
                        if (recruiter.IsNotNull())
                        {
                            result1.DatePickerValue1 = recruiter.DatePickerValue3;
                        }

                    }

                    if (result1.TemplateCode == "CHECK_MEDICAL_REPORT_INFORM_PRO" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "INFORM_CANDIDATE_FOR_MEDICAL").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode1 = data.AttachmentCode1;
                            result1.AttachmentValue1 = data.AttachmentValue1;
                        }
                    }

                    if (result1.TemplateCode == "RECEIVE_WORK_PERMIT_INFORM_JOINING_DATE" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "VERIFY_WORK_PERMIT_OBTAINED").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.DatePickerValue1 = data.DatePickerValue2;
                            result1.AttachmentCode2 = data.AttachmentCode1;
                            result1.AttachmentValue2 = data.AttachmentValue1;

                        }
                    }

                    if (result1.TemplateCode == "OBTAIN_BUSINESS_VISA" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "INFORM_CANDIDATE_FOR_MEDICAL").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode2 = data.AttachmentCode1;
                            result1.AttachmentValue2 = data.AttachmentValue1;
                        }
                    }

                    if (result1.TemplateCode == "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "OBTAIN_BUSINESS_VISA").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode1 = data.AttachmentCode3;
                            result1.AttachmentValue1 = data.AttachmentValue3;

                            result1.AttachmentCode4 = data.AttachmentCode4;
                            result1.AttachmentValue4 = data.AttachmentValue4;
                        }
                    }


                    if (result1.TemplateCode == "CONFIRM_TRAVELING_DATE" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetSingleById(result1.ReferenceTypeId);
                        if (service.IsNotNull())
                        {
                            var previousService = await GetSingle(x => x.ReferenceTypeId == service.ReferenceTypeId && (x.TemplateCode == "BUSINESS_VISA_MEDICAL"
                            || x.TemplateCode == "WORKER_VISA_MEDICAL") &&
                            x.NtsType == NtsTypeEnum.Service);
                            if (previousService.IsNotNull())
                            {
                                var service1 = await GetStepTaskListByService(previousService.Id);
                                var data = service1.Where(x => x.TemplateCode == "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE").FirstOrDefault();
                                if (data.IsNotNull())
                                {
                                    if (!result1.DatePickerValue1.IsNotNull())
                                    {
                                        result1.DatePickerValue1 = data.DatePickerValue2;
                                    }
                                }
                                var data1 = service1.Where(x => x.TemplateCode == "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE_WORKVISA").FirstOrDefault();
                                if (data1.IsNotNull())
                                {
                                    if (!result1.DatePickerValue1.IsNotNull())
                                    {
                                        result1.DatePickerValue1 = data1.DatePickerValue2;
                                    }
                                }
                            }
                        }

                    }


                    if (result1.TemplateCode == "BOOK_TICKET_ATTACH" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetSingleById(result1.ReferenceTypeId);
                        if (service.IsNotNull())
                        {
                            var previousService = await GetSingle(x => x.ReferenceTypeId == service.ReferenceTypeId && x.ReferenceTypeCode == ReferenceTypeEnum.REC_Application && (x.TemplateCode == "BUSINESS_VISA_MEDICAL"
                            || x.TemplateCode == "WORKER_VISA_MEDICAL") &&
                            x.NtsType == NtsTypeEnum.Service);
                            if (previousService.IsNotNull())
                            {
                                var service1 = await GetStepTaskListByService(previousService.Id);
                                var data = service1.Where(x => x.TemplateCode == "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE").FirstOrDefault();
                                if (data.IsNotNull())
                                {
                                    result1.DatePickerValue1 = data.DatePickerValue2;
                                    result1.AttachmentCode4 = data.AttachmentCode1;
                                    result1.AttachmentValue4 = data.AttachmentValue1;
                                }
                                var data1 = service1.Where(x => x.TemplateCode == "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE_WORKVISA").FirstOrDefault();
                                if (data1.IsNotNull())
                                {
                                    result1.DatePickerValue1 = data1.DatePickerValue2;
                                    result1.AttachmentCode4 = data1.AttachmentCode1;
                                    result1.AttachmentValue4 = data1.AttachmentValue1;
                                }

                            }

                            var finalService = await GetSingle(x => x.ReferenceTypeId == service.ReferenceTypeId && x.ReferenceTypeCode == ReferenceTypeEnum.REC_Application && x.TemplateCode == "PREPARE_FINAL_OFFER"
                             && x.NtsType == NtsTypeEnum.Service);
                            if (finalService.IsNotNull())
                            {
                                var service1 = await GetStepTaskListByService(finalService.Id);
                                var data = service1.Where(x => x.TemplateCode == "FINAL_OFFER_APPROVAL_CANDIDATE").FirstOrDefault();
                                if (data.IsNotNull())
                                {
                                    result1.AttachmentCode5 = data.AttachmentCode4;
                                    result1.AttachmentValue5 = data.AttachmentValue4;
                                }

                            }
                        }
                    }

                    if (result1.TemplateCode == "CONFIRM_RECEIPT_TICKET_DATE_OF_TRAVEL" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "TICKET_ATTACH").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode1 = data.AttachmentCode1;
                            result1.AttachmentValue1 = data.AttachmentValue1;

                            result1.AttachmentCode5 = data.AttachmentCode3;
                            result1.AttachmentValue5 = data.AttachmentValue3;

                        }

                        var pservice = await GetSingleById(result1.ReferenceTypeId);
                        if (pservice.IsNotNull())
                        {
                            var previousService = await GetSingle(x => x.ReferenceTypeId == pservice.ReferenceTypeId && (x.TemplateCode == "BUSINESS_VISA_MEDICAL"
                            || x.TemplateCode == "WORKER_VISA_MEDICAL") &&
                            x.NtsType == NtsTypeEnum.Service);
                            if (previousService.IsNotNull())
                            {
                                var service1 = await GetStepTaskListByService(previousService.Id);
                                var data1 = service1.Where(x => x.TemplateCode == "OBTAIN_BUSINESS_VISA").FirstOrDefault();
                                if (data1.IsNotNull())
                                {
                                    result1.AttachmentCode3 = data1.AttachmentCode3;
                                    result1.AttachmentValue3 = data1.AttachmentValue3;

                                    result1.AttachmentCode4 = data1.AttachmentCode4;
                                    result1.AttachmentValue4 = data1.AttachmentValue4;
                                }

                                var data2 = service1.Where(x => x.TemplateCode == "FIT_UNFIT_ATTACH_VISA_COPY").FirstOrDefault();
                                if (data2.IsNotNull())
                                {
                                    result1.AttachmentCode3 = data2.AttachmentCode1;
                                    result1.AttachmentValue3 = data2.AttachmentValue1;

                                    result1.AttachmentCode4 = data2.AttachmentCode3;
                                    result1.AttachmentValue4 = data2.AttachmentValue3;
                                }
                            }
                        }
                    }

                    //if (result1.TemplateCode == "CONFIRM_RECEIPT_TICKET_DATE_OF_TRAVEL" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    //{
                    //    var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                    //    var data = service.Where(x => x.TemplateCode == "TICKET_ATTACH").FirstOrDefault();
                    //    if (data.IsNotNull())
                    //    {
                    //        result1.AttachmentCode1 = data.AttachmentCode1;
                    //        result1.AttachmentValue1 = data.AttachmentValue1;
                    //    }
                    //}

                    if (result1.TemplateCode == "ARRANGE_ACCOMMODATION" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "TICKET_ATTACH").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode1 = data.AttachmentCode1;
                            result1.AttachmentValue1 = data.AttachmentValue1;
                        }
                    }

                    if (result1.TemplateCode == "ARRANGE_VEHICLE_PICKUP" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "TICKET_ATTACH").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode1 = data.AttachmentCode1;
                            result1.AttachmentValue1 = data.AttachmentValue1;
                        }

                        var data1 = service.Where(x => x.TemplateCode == "ARRANGE_ACCOMMODATION").FirstOrDefault();
                        if (data1.IsNotNull())
                        {
                            result1.TextValue3 = data1.TextValue3;
                        }
                    }

                    //work visa

                    if (result1.TemplateCode == "BOOK_QVC_APPOINTMENT" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetSingleById(result1.ReferenceTypeId);
                        if (service.IsNotNull())
                        {

                            var finalService = await GetSingle(x => x.ReferenceTypeId == service.ReferenceTypeId && x.ReferenceTypeCode == ReferenceTypeEnum.REC_Application && x.TemplateCode == "PREPARE_FINAL_OFFER"
                             && x.NtsType == NtsTypeEnum.Service);
                            if (finalService.IsNotNull())
                            {
                                var service1 = await GetStepTaskListByService(finalService.Id);
                                var data = service1.Where(x => x.TemplateCode == "FINAL_OFFER_APPROVAL_CANDIDATE").FirstOrDefault();
                                if (data.IsNotNull())
                                {
                                    result1.AttachmentCode4 = data.AttachmentCode4;
                                    result1.AttachmentValue4 = data.AttachmentValue4;
                                }

                            }
                        }
                    }
                    if (result1.TemplateCode == "CONDUCT_MEDICAL_FINGER_PRINT" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "BOOK_QVC_APPOINTMENT").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.DatePickerValue2 = data.DatePickerValue2;
                        }
                    }


                    if (result1.TemplateCode == "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE_WORKVISA" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "FIT_UNFIT_ATTACH_VISA_COPY").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode1 = data.AttachmentCode1;
                            result1.AttachmentValue1 = data.AttachmentValue1;
                        }
                    }
                    // Visa Transfer

                    if (result1.TemplateCode == "VERIFY_DOCUMENTS" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "SUBMIT_VISA_TRANSFER_DOCUMENTS").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode1 = data.AttachmentCode1;
                            result1.AttachmentValue1 = data.AttachmentValue1;
                            result1.AttachmentCode2 = data.AttachmentCode2;
                            result1.AttachmentValue2 = data.AttachmentValue2;
                            result1.AttachmentCode3 = data.AttachmentCode3;
                            result1.AttachmentValue3 = data.AttachmentValue3;
                            result1.AttachmentCode4 = data.AttachmentCode4;
                            result1.AttachmentValue4 = data.AttachmentValue4;
                            result1.AttachmentCode5 = data.AttachmentCode5;
                            result1.AttachmentValue5 = data.AttachmentValue5;
                            result1.AttachmentCode6 = data.AttachmentCode7;
                            result1.AttachmentValue6 = data.AttachmentValue7;
                            result1.AttachmentCode7 = data.AttachmentCode8;
                            result1.AttachmentValue7 = data.AttachmentValue8;
                        }
                    }

                    if (result1.TemplateCode == "SUBMIT_VISA_TRANSFER_CONFIRM_SPONSORSHIP" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "SUBMIT_VISA_TRANSFER_DOCUMENTS").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode1 = data.AttachmentCode1;
                            result1.AttachmentValue1 = data.AttachmentValue1;
                            result1.AttachmentCode2 = data.AttachmentCode2;
                            result1.AttachmentValue2 = data.AttachmentValue2;
                            result1.AttachmentCode3 = data.AttachmentCode3;
                            result1.AttachmentValue3 = data.AttachmentValue3;
                            result1.AttachmentCode4 = data.AttachmentCode4;
                            result1.AttachmentValue4 = data.AttachmentValue4;
                            result1.AttachmentCode5 = data.AttachmentCode5;
                            result1.AttachmentValue5 = data.AttachmentValue5;
                        }
                    }

                    if (result1.TemplateCode == "VERIFY_VISA_TRANSFER_COMPLETED" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "SUBMIT_VISA_TRANSFER_CONFIRM_SPONSORSHIP").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode1 = data.AttachmentCode6;
                            result1.AttachmentValue1 = data.AttachmentValue6;
                        }
                    }

                    if (result1.TemplateCode == "RECEIVE_VISA_TRANSFER_INFORM_JOINING_DATE" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "SUBMIT_VISA_TRANSFER_CONFIRM_SPONSORSHIP").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode2 = data.AttachmentCode6;
                            result1.AttachmentValue2 = data.AttachmentValue6;
                        }
                    }

                    //work permit

                    if (result1.TemplateCode == "VERIFY_WORK_PERMIT_DOCUMENTS" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "SUBMIT_WORK_PERMIT_DOCUMENTS").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode1 = data.AttachmentCode1;
                            result1.AttachmentValue1 = data.AttachmentValue1;
                            result1.AttachmentCode2 = data.AttachmentCode2;
                            result1.AttachmentValue2 = data.AttachmentValue2;
                            result1.AttachmentCode3 = data.AttachmentCode3;
                            result1.AttachmentValue3 = data.AttachmentValue3;
                            result1.AttachmentCode4 = data.AttachmentCode4;
                            result1.AttachmentValue4 = data.AttachmentValue4;
                            result1.AttachmentCode9 = data.AttachmentCode9;
                            result1.AttachmentValue9 = data.AttachmentValue9;
                        }
                    }


                    if (result1.TemplateCode == "OBTAIN_WORK_PERMIT_ATTACH" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "SUBMIT_WORK_PERMIT_DOCUMENTS").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode1 = data.AttachmentCode1;
                            result1.AttachmentValue1 = data.AttachmentValue1;
                            result1.AttachmentCode2 = data.AttachmentCode2;
                            result1.AttachmentValue2 = data.AttachmentValue2;
                            result1.AttachmentCode3 = data.AttachmentCode3;
                            result1.AttachmentValue3 = data.AttachmentValue3;
                            result1.AttachmentCode4 = data.AttachmentCode4;
                            result1.AttachmentValue4 = data.AttachmentValue4;
                        }
                    }

                    if (result1.TemplateCode == "VERIFY_WORK_PERMIT_OBTAINED" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "OBTAIN_WORK_PERMIT_ATTACH").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode1 = data.AttachmentValue5;
                            result1.AttachmentValue1 = data.AttachmentValue5;
                        }
                    }


                    if (result1.TemplateCode == "VERIFY_WORK_PERMIT_OBTAINED" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "OBTAIN_WORK_PERMIT_ATTACH").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode1 = data.AttachmentCode5;
                            result1.AttachmentValue1 = data.AttachmentValue5;
                        }
                    }

                    if (result1.TemplateCode == "VERIFY_WORK_PERMIT_OBTAINED" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "OBTAIN_WORK_PERMIT_ATTACH").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode2 = data.AttachmentCode5;
                            result1.AttachmentValue2 = data.AttachmentValue5;
                        }
                    }

                    //if (result.TaskVersionId != null && result.TaskVersionId.Value > 0)
                    //{
                    //    return GetVersionDetails(viewModel);
                    //}


                }

                newlist.Add(result1);

            }
            return newlist;

        }
        public async Task<List<RecTaskViewModel>> GetTaskDetailsSummaryList(string ids, string templateCode, string userId)
        {
            string query = @$"SELECT  * FROM public.""RecTaskTemplate"" as tt
                            where tt.""TemplateCode""='{templateCode}'
                            ";
            var result = await _queryRepo.ExecuteQuerySingle<RecTaskViewModel>(query, null);
            if (result.IsNotNull())
            {
                result.TemplateId = result.Id;
                result.Id = "";
            }
            ids = "'" + ids.Replace(",", "','") + "'";
            string querytask = @$"SELECT t.* FROM public.""RecTask"" as t where t.""Id"" in ({ids})";

            var resultlist = await _queryRepo.ExecuteQueryList<RecTaskViewModel>(querytask, null);

            var newlist = new List<RecTaskViewModel>();
            foreach (var result1 in resultlist)
            {

                if (result1.IsNotNull())
                {
                    if (result1.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(result1.ReferenceTypeId);
                        if (service.IsNotNull() && service.ReferenceTypeCode == ReferenceTypeEnum.REC_Application)
                        {
                            var application = await _appBusiness.GetSingleById(service.ReferenceTypeId);
                            if (application.IsNotNull())
                            {
                                result1.BatchId = application.BatchId;
                                if (application.FirstName.IsNotNullAndNotEmpty())
                                {
                                    result1.CandidateName = application.FirstName;
                                }
                                if (application.JobId.IsNotNullAndNotEmpty())
                                {
                                    var job = await _masterBusiness.GetJobNameById(application.JobId);
                                    if (job.IsNotNull() && job.Name.IsNotNullAndNotEmpty())
                                    {
                                        result1.JobName = job.Name;
                                    }
                                }
                                if (application.BatchId.IsNotNullAndNotEmpty())
                                {
                                    var org = await _masterBusiness.GetOrgNameByBatchId(application.BatchId);
                                    if (org.IsNotNull() && org.Name.IsNotNullAndNotEmpty())
                                    {
                                        result1.OrgUnitName = org.Name;
                                    }
                                }
                                if (application.GaecNo.IsNotNullAndNotEmpty())
                                {
                                    result1.GaecNo = application.GaecNo;
                                }

                            }
                        }
                    }
                    if (result1.JobName.IsNullOrEmpty() && result1.OrgUnitName.IsNullOrEmpty())
                    {
                        var job = await _masterBusiness.GetJobNameById(result1.DropdownValue1);
                        if (job.IsNotNull() && job.Name.IsNotNullAndNotEmpty())
                        {
                            result1.JobName = job.Name;
                        }
                        var org = await _masterBusiness.GetOrgNameById(result1.DropdownValue2);
                        if (org.IsNotNull() && org.Name.IsNotNullAndNotEmpty())
                        {
                            result1.OrgUnitName = org.Name;
                        }
                    }


                    await MapTemplate(result1, result);
                    result1.DataAction = DataActionEnum.Read;
                    if (result1.TemplateCode == "WORKER_POOL_HR" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetSingleById(result1.ReferenceTypeId);
                        var applicant = await _appBusiness.GetSingleById(service.ReferenceTypeId);
                        if (applicant.IsNotNull())
                        {
                            result1.TextValue1 = applicant.SalaryOnAppointment;
                        }

                    }
                    if (result1.TemplateCode == "WORKER_SALARY_AGENCY" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var recruiter = service.Where(x => x.TemplateCode == "WORKER_POOL_HR").FirstOrDefault();
                        if (recruiter.IsNotNull())
                        {
                            result1.TextValue1 = recruiter.TextValue1;
                        }

                    }
                    if (result1.TemplateCode == "TICKET_ATTACH" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetSingleById(result1.ReferenceTypeId);
                        var app = await _appBusiness.GetApplicationDetail(service.ReferenceTypeId);
                        if (app.ManpowerTypeCode == "Staff")
                        {
                            result1.IsRequiredTextBoxDisplay1 = true;
                        }
                        else
                        {
                            result1.TextBoxDisplayType1 = NtsFieldType.NTS_Hidden;
                        }
                    }
                    if (result1.TemplateCode == "JOBDESCRIPTION_HM")
                    {
                        if (result1.TextBoxLink3.IsNotNullAndNotEmpty())
                        {
                            result1.TextBoxLink3 = result1.TextBoxLink3.Replace("{jobId}", result1.DropdownValue1);
                            result1.TextBoxLink3 = result1.TextBoxLink3.Replace("{orgId}", result1.DropdownValue2);
                            if (result1.TextBoxLink3.Contains("{TaskStatus}"))
                            {
                                result1.TextBoxLink3 = result1.TextBoxLink3.Replace("{TaskStatus}", result1.TaskStatusCode);
                            }
                        }
                    }
                    if (result1.TemplateCode == "SCHEDULE_INTERVIEW_CANDIDATE" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var recruiter = service.Where(x => x.TemplateCode == "SCHEDULE_INTERVIEW_RECRUITER").FirstOrDefault();
                        if (recruiter.IsNotNull())
                        {
                            result1.DatePickerValue1 = recruiter.DatePickerValue3;
                        }

                    }

                    if (result1.TemplateCode == "CHECK_MEDICAL_REPORT_INFORM_PRO" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "INFORM_CANDIDATE_FOR_MEDICAL").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode1 = data.AttachmentCode1;
                            result1.AttachmentValue1 = data.AttachmentValue1;
                        }
                    }

                    if (result1.TemplateCode == "RECEIVE_WORK_PERMIT_INFORM_JOINING_DATE" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "VERIFY_WORK_PERMIT_OBTAINED").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.DatePickerValue1 = data.DatePickerValue2;
                            result1.AttachmentCode2 = data.AttachmentCode1;
                            result1.AttachmentValue2 = data.AttachmentValue1;

                        }
                    }

                    if (result1.TemplateCode == "OBTAIN_BUSINESS_VISA" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "INFORM_CANDIDATE_FOR_MEDICAL").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode2 = data.AttachmentCode1;
                            result1.AttachmentValue2 = data.AttachmentValue1;
                        }
                    }

                    if (result1.TemplateCode == "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "OBTAIN_BUSINESS_VISA").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode1 = data.AttachmentCode3;
                            result1.AttachmentValue1 = data.AttachmentValue3;

                            result1.AttachmentCode4 = data.AttachmentCode4;
                            result1.AttachmentValue4 = data.AttachmentValue4;
                        }
                    }


                    if (result1.TemplateCode == "CONFIRM_TRAVELING_DATE" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetSingleById(result1.ReferenceTypeId);
                        if (service.IsNotNull())
                        {
                            var previousService = await GetSingle(x => x.ReferenceTypeId == service.ReferenceTypeId && (x.TemplateCode == "BUSINESS_VISA_MEDICAL"
                            || x.TemplateCode == "WORKER_VISA_MEDICAL") &&
                            x.NtsType == NtsTypeEnum.Service);
                            if (previousService.IsNotNull())
                            {
                                var service1 = await GetStepTaskListByService(previousService.Id);
                                var data = service1.Where(x => x.TemplateCode == "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE").FirstOrDefault();
                                if (data.IsNotNull())
                                {
                                    if (!result1.DatePickerValue1.IsNotNull())
                                    {
                                        result1.DatePickerValue1 = data.DatePickerValue2;
                                    }
                                }
                                var data1 = service1.Where(x => x.TemplateCode == "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE_WORKVISA").FirstOrDefault();
                                if (data1.IsNotNull())
                                {
                                    if (!result1.DatePickerValue1.IsNotNull())
                                    {
                                        result1.DatePickerValue1 = data1.DatePickerValue2;
                                    }
                                }
                            }
                        }

                    }


                    if (result1.TemplateCode == "BOOK_TICKET_ATTACH" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetSingleById(result1.ReferenceTypeId);
                        if (service.IsNotNull())
                        {
                            var previousService = await GetSingle(x => x.ReferenceTypeId == service.ReferenceTypeId && x.ReferenceTypeCode == ReferenceTypeEnum.REC_Application && (x.TemplateCode == "BUSINESS_VISA_MEDICAL"
                            || x.TemplateCode == "WORKER_VISA_MEDICAL") &&
                            x.NtsType == NtsTypeEnum.Service);
                            if (previousService.IsNotNull())
                            {
                                var service1 = await GetStepTaskListByService(previousService.Id);
                                var data = service1.Where(x => x.TemplateCode == "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE").FirstOrDefault();
                                if (data.IsNotNull())
                                {
                                    result1.DatePickerValue1 = data.DatePickerValue2;
                                    result1.AttachmentCode4 = data.AttachmentCode1;
                                    result1.AttachmentValue4 = data.AttachmentValue1;
                                }
                                var data1 = service1.Where(x => x.TemplateCode == "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE_WORKVISA").FirstOrDefault();
                                if (data1.IsNotNull())
                                {
                                    result1.DatePickerValue1 = data1.DatePickerValue2;
                                    result1.AttachmentCode4 = data1.AttachmentCode1;
                                    result1.AttachmentValue4 = data1.AttachmentValue1;
                                }

                            }

                            var finalService = await GetSingle(x => x.ReferenceTypeId == service.ReferenceTypeId && x.ReferenceTypeCode == ReferenceTypeEnum.REC_Application && x.TemplateCode == "PREPARE_FINAL_OFFER"
                             && x.NtsType == NtsTypeEnum.Service);
                            if (finalService.IsNotNull())
                            {
                                var service1 = await GetStepTaskListByService(finalService.Id);
                                var data = service1.Where(x => x.TemplateCode == "FINAL_OFFER_APPROVAL_CANDIDATE").FirstOrDefault();
                                if (data.IsNotNull())
                                {
                                    result1.AttachmentCode5 = data.AttachmentCode4;
                                    result1.AttachmentValue5 = data.AttachmentValue4;
                                }

                            }
                        }
                    }

                    if (result1.TemplateCode == "CONFIRM_RECEIPT_TICKET_DATE_OF_TRAVEL" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "TICKET_ATTACH").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode1 = data.AttachmentCode1;
                            result1.AttachmentValue1 = data.AttachmentValue1;

                            result1.AttachmentCode5 = data.AttachmentCode3;
                            result1.AttachmentValue5 = data.AttachmentValue3;

                        }

                        var pservice = await GetSingleById(result1.ReferenceTypeId);
                        if (pservice.IsNotNull())
                        {
                            var previousService = await GetSingle(x => x.ReferenceTypeId == pservice.ReferenceTypeId && (x.TemplateCode == "BUSINESS_VISA_MEDICAL"
                            || x.TemplateCode == "WORKER_VISA_MEDICAL") &&
                            x.NtsType == NtsTypeEnum.Service);
                            if (previousService.IsNotNull())
                            {
                                var service1 = await GetStepTaskListByService(previousService.Id);
                                var data1 = service1.Where(x => x.TemplateCode == "OBTAIN_BUSINESS_VISA").FirstOrDefault();
                                if (data1.IsNotNull())
                                {
                                    result1.AttachmentCode3 = data1.AttachmentCode3;
                                    result1.AttachmentValue3 = data1.AttachmentValue3;

                                    result1.AttachmentCode4 = data1.AttachmentCode4;
                                    result1.AttachmentValue4 = data1.AttachmentValue4;
                                }

                                var data2 = service1.Where(x => x.TemplateCode == "FIT_UNFIT_ATTACH_VISA_COPY").FirstOrDefault();
                                if (data2.IsNotNull())
                                {
                                    result1.AttachmentCode3 = data2.AttachmentCode1;
                                    result1.AttachmentValue3 = data2.AttachmentValue1;

                                    result1.AttachmentCode4 = data2.AttachmentCode3;
                                    result1.AttachmentValue4 = data2.AttachmentValue3;
                                }
                            }
                        }
                    }


                    if (result1.TemplateCode == "ARRANGE_ACCOMMODATION" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "TICKET_ATTACH").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode1 = data.AttachmentCode1;
                            result1.AttachmentValue1 = data.AttachmentValue1;
                        }
                    }

                    if (result1.TemplateCode == "ARRANGE_VEHICLE_PICKUP" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "TICKET_ATTACH").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode1 = data.AttachmentCode1;
                            result1.AttachmentValue1 = data.AttachmentValue1;
                        }

                        var data1 = service.Where(x => x.TemplateCode == "ARRANGE_ACCOMMODATION").FirstOrDefault();
                        if (data1.IsNotNull())
                        {
                            result1.TextValue3 = data1.TextValue3;
                        }
                    }

                    //work visa

                    if (result1.TemplateCode == "BOOK_QVC_APPOINTMENT" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetSingleById(result1.ReferenceTypeId);
                        if (service.IsNotNull())
                        {

                            var finalService = await GetSingle(x => x.ReferenceTypeId == service.ReferenceTypeId && x.ReferenceTypeCode == ReferenceTypeEnum.REC_Application && x.TemplateCode == "PREPARE_FINAL_OFFER"
                             && x.NtsType == NtsTypeEnum.Service);
                            if (finalService.IsNotNull())
                            {
                                var service1 = await GetStepTaskListByService(finalService.Id);
                                var data = service1.Where(x => x.TemplateCode == "FINAL_OFFER_APPROVAL_CANDIDATE").FirstOrDefault();
                                if (data.IsNotNull())
                                {
                                    result1.AttachmentCode4 = data.AttachmentCode4;
                                    result1.AttachmentValue4 = data.AttachmentValue4;
                                }
                                var data1 = service1.Where(x => x.TemplateCode == "APPLY_WORK_VISA_THROUGH_MOL").FirstOrDefault();
                                if (data1.IsNotNull())
                                {
                                    result1.AttachmentCode5 = data1.AttachmentCode1;
                                    result1.AttachmentValue5 = data1.AttachmentValue1;
                                }


                            }
                        }
                    }
                    if (result1.TemplateCode == "CONDUCT_MEDICAL_FINGER_PRINT" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "BOOK_QVC_APPOINTMENT").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.DatePickerValue2 = data.DatePickerValue2;
                        }
                    }


                    if (result1.TemplateCode == "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE_WORKVISA" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "FIT_UNFIT_ATTACH_VISA_COPY").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode1 = data.AttachmentCode1;
                            result1.AttachmentValue1 = data.AttachmentValue1;
                        }
                    }
                    // Visa Transfer

                    if (result1.TemplateCode == "VERIFY_DOCUMENTS" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "SUBMIT_VISA_TRANSFER_DOCUMENTS").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode1 = data.AttachmentCode1;
                            result1.AttachmentValue1 = data.AttachmentValue1;
                            result1.AttachmentCode2 = data.AttachmentCode2;
                            result1.AttachmentValue2 = data.AttachmentValue2;
                            result1.AttachmentCode3 = data.AttachmentCode3;
                            result1.AttachmentValue3 = data.AttachmentValue3;
                            result1.AttachmentCode4 = data.AttachmentCode4;
                            result1.AttachmentValue4 = data.AttachmentValue4;
                            result1.AttachmentCode5 = data.AttachmentCode5;
                            result1.AttachmentValue5 = data.AttachmentValue5;
                        }
                    }

                    if (result1.TemplateCode == "SUBMIT_VISA_TRANSFER_CONFIRM_SPONSORSHIP" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "SUBMIT_VISA_TRANSFER_DOCUMENTS").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode1 = data.AttachmentCode1;
                            result1.AttachmentValue1 = data.AttachmentValue1;
                            result1.AttachmentCode2 = data.AttachmentCode2;
                            result1.AttachmentValue2 = data.AttachmentValue2;
                            result1.AttachmentCode3 = data.AttachmentCode3;
                            result1.AttachmentValue3 = data.AttachmentValue3;
                            result1.AttachmentCode4 = data.AttachmentCode4;
                            result1.AttachmentValue4 = data.AttachmentValue4;
                            result1.AttachmentCode5 = data.AttachmentCode5;
                            result1.AttachmentValue5 = data.AttachmentValue5;
                        }
                    }

                    if (result1.TemplateCode == "VERIFY_VISA_TRANSFER_COMPLETED" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "SUBMIT_VISA_TRANSFER_CONFIRM_SPONSORSHIP").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode1 = data.AttachmentCode6;
                            result1.AttachmentValue1 = data.AttachmentValue6;
                        }
                    }

                    if (result1.TemplateCode == "RECEIVE_VISA_TRANSFER_INFORM_JOINING_DATE" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "SUBMIT_VISA_TRANSFER_CONFIRM_SPONSORSHIP").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode2 = data.AttachmentCode6;
                            result1.AttachmentValue2 = data.AttachmentValue6;
                        }
                    }

                    //work permit

                    if (result1.TemplateCode == "VERIFY_WORK_PERMIT_DOCUMENTS" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "SUBMIT_WORK_PERMIT_DOCUMENTS").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode1 = data.AttachmentCode1;
                            result1.AttachmentValue1 = data.AttachmentValue1;
                            result1.AttachmentCode2 = data.AttachmentCode2;
                            result1.AttachmentValue2 = data.AttachmentValue2;
                            result1.AttachmentCode3 = data.AttachmentCode3;
                            result1.AttachmentValue3 = data.AttachmentValue3;
                            result1.AttachmentCode4 = data.AttachmentCode4;
                            result1.AttachmentValue4 = data.AttachmentValue4;
                        }
                    }


                    if (result1.TemplateCode == "OBTAIN_WORK_PERMIT_ATTACH" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "SUBMIT_WORK_PERMIT_DOCUMENTS").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode1 = data.AttachmentCode1;
                            result1.AttachmentValue1 = data.AttachmentValue1;
                            result1.AttachmentCode2 = data.AttachmentCode2;
                            result1.AttachmentValue2 = data.AttachmentValue2;
                            result1.AttachmentCode3 = data.AttachmentCode3;
                            result1.AttachmentValue3 = data.AttachmentValue3;
                            result1.AttachmentCode4 = data.AttachmentCode4;
                            result1.AttachmentValue4 = data.AttachmentValue4;
                        }
                    }

                    if (result1.TemplateCode == "VERIFY_WORK_PERMIT_OBTAINED" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "OBTAIN_WORK_PERMIT_ATTACH").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode1 = data.AttachmentValue5;
                            result1.AttachmentValue1 = data.AttachmentValue5;
                        }
                    }


                    if (result1.TemplateCode == "VERIFY_WORK_PERMIT_OBTAINED" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "OBTAIN_WORK_PERMIT_ATTACH").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode1 = data.AttachmentCode5;
                            result1.AttachmentValue1 = data.AttachmentValue5;
                        }
                    }

                    if (result1.TemplateCode == "VERIFY_WORK_PERMIT_OBTAINED" && result1.ReferenceTypeId.IsNotNullAndNotEmpty())
                    {
                        var service = await GetStepTaskListByService(result1.ReferenceTypeId);
                        var data = service.Where(x => x.TemplateCode == "OBTAIN_WORK_PERMIT_ATTACH").FirstOrDefault();
                        if (data.IsNotNull())
                        {
                            result1.AttachmentCode2 = data.AttachmentCode5;
                            result1.AttachmentValue2 = data.AttachmentValue5;
                        }
                    }
                }

                newlist.Add(result1);

            }
            return newlist;

        }
        private async Task<RecTaskViewModel> GetTaskVersionDetails(RecTaskViewModel model)
        {
            var userId = model.ActiveUserId;
            var result1 = new RecTaskViewModel();
            if (model.Id.IsNotNullAndNotEmpty())
            {
                string query1 = @$"SELECT t.* FROM public.""RecTaskVersion"" as t 
                            
                            where t.""ReferenceTypeId""='{model.Id}' and t.""VersionNo""='{model.TaskVersionId}'
                            ";

                result1 = await _queryRepo.ExecuteQuerySingle<RecTaskViewModel>(query1, null);
            }
            var templateCode = model.TemplateCode.IsNullOrEmpty() ? result1.TemplateCode : model.TemplateCode;
            string query = @$"SELECT  * FROM public.""RecTaskTemplate"" as tt
                            where tt.""TemplateCode""='{templateCode}'
                            ";
            var result = await _queryRepo.ExecuteQuerySingle<RecTaskViewModel>(query, null);
            if (result1.IsNotNull() && result.IsNotNull())
            {
                result1.Id = model.Id;
                result1.TaskVersionId = model.TaskVersionId;
                await MapTemplate(result1, result);
                result1.TemplateAction = NtsActionEnum.View;
                result1.DisplayMode = FieldDisplayModeEnum.Readonly;
                result1.TemplateUserType = NtsUserTypeEnum.Shared;
                result1.DataAction = DataActionEnum.Read;
                result1.CanEditHeader = false;
                result1.DraftButton = false;
                result1.SaveButton = false;
                result1.CreateNewVersionButton = false;
                result1.NotApplicableButton = false;
                result1.RejectButton = false;
                result1.CompleteButton = false;
                result1.CancelButton = false;
            }
            return result1;


        }
        //public async Task<RecTaskViewModel> GetVersionDetails(RecTaskViewModel viewModel)
        //{            
        //    //var versionDeatils = _repoVersion.GetList(x => x.ReferenceTypeId == viewModel.Id && x.VersionNo == viewModel.TaskVersionId).Result;
        //    //fetch from version table where refeid=id & version=versionNo  return single
        //    return null;
        //}
        public async Task<string> GenerateNextTaskNo()
        {
            var date = DateTime.Now.Date;
            var id = await GenerateNextDatedId(NtsTypeEnum.Task);
            return string.Concat("T-", String.Format("{0:dd.MM.yyyy}", date), "-", id);
        }
        public async Task<string> GenerateNextServiceNo()
        {
            var date = DateTime.Now.Date;
            var id = await GenerateNextDatedId(NtsTypeEnum.Service);
            return string.Concat("S-", String.Format("{0:dd.MM.yyyy}", date), "-", id);
        }

        public async Task<long> GenerateNextDatedId(NtsTypeEnum ntsType = NtsTypeEnum.Task)
        {
            if (ntsType == NtsTypeEnum.Task)
            {
                string query = @$"SELECT  count(*) as cc FROM public.""RecTask"" as t
                                where Date(t.""CreatedDate"")=Date('{ToDD_MMM_YYYY_HHMMSS(DateTime.Now)}') and (t.""NtsType"" is null or t.""NtsType""=1)
                            ";
                var result = await _queryRepo.ExecuteScalar<long>(query, null);
                return result;
            }
            else if (ntsType == NtsTypeEnum.Service)
            {
                string query = @$"SELECT  count(*) as cc FROM public.""RecTask"" as t
                                where Date(t.""CreatedDate"")=Date('{ToDD_MMM_YYYY_HHMMSS(DateTime.Now)}') and  t.""NtsType""=2
                            ";
                var result = await _queryRepo.ExecuteScalar<long>(query, null);
                return result;
            }
            else
            {
                string query = @$"SELECT  count(*) as cc FROM public.""RecTask"" as t
                                where Date(t.""CreatedDate"")=Date('{ToDD_MMM_YYYY_HHMMSS(DateTime.Now)}' and  t.""NtsType""=0 )
                            ";
                var result = await _queryRepo.ExecuteScalar<long>(query, null);
                return result;
            }


        }
        public string ToDD_MMM_YYYY_HHMMSS(DateTime value)
        {
            return String.Format("{0:yyyy-MM-dd}", value);
        }
        private async Task MapTemplate(RecTaskViewModel viewModel, RecTaskViewModel template)
        {

            if (viewModel.IsNotNull() && template.IsNotNull())
            {
                viewModel.UdfIframeSrc = template.UdfIframeSrc;
                if (viewModel.UdfIframeSrc.IsNotNullAndNotEmpty() && viewModel.UdfIframeSrc.Contains("{batchId}"))
                {
                    if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.REC_WorkerPoolBatch)
                    {
                        var link = viewModel.UdfIframeSrc.Replace("{batchId}", viewModel.ReferenceTypeId);
                        viewModel.UdfIframeSrc = link;

                    }
                    else if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(viewModel.ReferenceTypeId);
                        var link = viewModel.UdfIframeSrc.Replace("{batchId}", service.IsNotNull() ? service.ReferenceTypeId : "");
                        viewModel.UdfIframeSrc = link;
                    }

                }
                //viewModel.EnableSLAChangeRequest = template.EnableSLAChangeRequest;
                //viewModel.TemplateMaximumColumn = template.LayoutColumnCount;
                //viewModel.FieldSectionText = template.FieldSectionText;
                //viewModel.FieldSectionMessage = template.FieldSectionMessage;

                //viewModel.StepSectionName = template.StepSectionText;
                //viewModel.StepSectionMessage = template.StepSectionMessage;
                //if (template.Subject.IsNotNullAndNotEmpty() && (template.Subject.Contains("{batchName}")|| template.Subject.Contains("{employeeType}")))
                //{
                //    viewModel.Subject = viewModel.Subject;
                //}
                //else
                //{
                //    viewModel.Subject = template.Subject;
                //}               
                viewModel.Description = template.Description;
                viewModel.TemplateCode = template.TemplateCode;
                viewModel.ReturnTemplateName = template.ReturnTemplateName;
                viewModel.DraftButtonText = template.DraftButtonText;
                viewModel.SaveButtonText = template.SaveButtonText;
                viewModel.CompleteButtonText = template.CompleteButtonText;
                viewModel.NtsType = template.NtsType;
                viewModel.SLA = template.SLA;
                viewModel.StepTemplateIds = template.StepTemplateIds;
                viewModel.ServiceDetailsHeight = template.ServiceDetailsHeight;
                viewModel.IsIncludeEmailAttachment = template.IsIncludeEmailAttachment;
                viewModel.EmailSettingId = template.EmailSettingId;
                viewModel.RejectButtonText = template.RejectButtonText;


                viewModel.ReturnButtonText = template.ReturnButtonText;

                viewModel.DelegateButtonText = template.DelegateButtonText;
                viewModel.CancelButtonText = template.CancelButtonText;
                viewModel.BackButtonText = template.BackButtonText;
                viewModel.DraftButton = template.DraftButton;
                viewModel.SaveButton = template.SaveButton;
                viewModel.SaveChangesButton = template.SaveChangesButton;
                viewModel.SaveChangesButtonText = template.SaveChangesButtonText;

                viewModel.ReopenButton = template.ReopenButton;
                viewModel.ReopenButtonText = template.ReopenButtonText;
                viewModel.IsReopenReasonRequired = template.IsReopenReasonRequired;



                viewModel.CompleteButton = template.CompleteButton;
                viewModel.IsCompleteReasonRequired = template.IsCompleteReasonRequired;
                viewModel.RejectButton = template.RejectButton;
                viewModel.IsRejectionReasonRequired = template.IsRejectionReasonRequired;
                viewModel.ReturnButton = template.ReturnButton;
                viewModel.IsReturnReasonRequired = template.IsReturnReasonRequired;
                viewModel.DelegateButton = template.DelegateButton;
                viewModel.IsDelegateReasonRequired = template.IsDelegateReasonRequired;
                viewModel.CancelButton = template.CancelButton;
                viewModel.IsCancelReasonRequired = template.IsCancelReasonRequired;
                viewModel.BackButton = template.BackButton;
                //viewModel.CollapseHeader = template.CollapseHeader;
                viewModel.SubjectLabelName = template.SubjectLabelName;
                viewModel.IsSubjectRequired = template.IsSubjectRequired;
                viewModel.IsSubjectEditable = template.IsSubjectEditable;

                viewModel.DescriptionLabelName = template.DescriptionLabelName;
                viewModel.IsDescriptionRequired = template.IsDescriptionRequired;
                viewModel.IsDescriptionEditable = template.IsDescriptionEditable;
                //viewModel.ServerValidationScript = template.ServerValidationScript;
                //viewModel.PostSubmitExecutionMethod = viewModel.SkipPreAndPostScript ? string.Empty : template.PostSubmitExecutionMethod;
                //viewModel.PreSubmitExecutionMethod = viewModel.SkipPreAndPostScript ? string.Empty : template.PreSubmitExecutionMethod;
                //viewModel.SaveChangesButtonVisibilityMethod = template.SaveChangesButtonVisibilityMethod;
                //viewModel.EditButtonVisibilityMethod = template.EditButtonVisibilityMethod;
                //viewModel.HeaderSectionText = template.HeaderSectionText;
                //viewModel.HeaderSectionMessage = template.HeaderSectionMessage;

                //viewModel.DisableMessage = template.DisableMessage;
                //viewModel.ClientValidationScript = template.ClientValidationScript;
                //viewModel.ServerValidationScript = template.ServerValidationScript;

                //viewModel.ModuleName = template.ModuleName;
                //viewModel.NotificationUrlPattern = template.NotificationUrlPattern;
                //viewModel.EnableTaskAutoComplete = template.EnableTaskAutoComplete;
                //viewModel.TemplateMasterCode = template.TemplateMaster.Code;

                viewModel.CanViewVersions = template.CanViewVersions;
                viewModel.CreateNewVersionButton = template.CreateNewVersionButton;
                viewModel.CreateNewVersionButtonText = template.CreateNewVersionButtonText;
                viewModel.SaveNewVersionButtonText = template.SaveNewVersionButtonText;
                //viewModel.IsAttachmentRequired = template.IsAttachmentRequired;
                //viewModel.ServiceOwnerActAsStepTaskAssignee = template.ServiceOwnerActAsStepTaskAssignee;

                viewModel.NotApplicableButton = template.NotApplicableButton;
                //viewModel.DisableStepTask = template.DisableStepTask;
                //viewModel.LegalEntityCode = template.LegalEntityCode;
                //viewModel.AdminCanEditUdf = template.AdminCanEditUdf;
                //viewModel.NotificationSubject = template.NotificationSubject;

                //if (viewModel.ServiceTaskTemplateId != null)
                //{
                //    var serviceTaskTemplate = _serviceTaskTemplateRepo.GetSingleById(viewModel.ServiceTaskTemplateId);
                //    if (serviceTaskTemplate != null)
                //    {
                //        viewModel.NotificationSubject = serviceTaskTemplate.NotificationSubject;
                //    }
                //}

                //viewModel.EnableTeamAsOwner = template.EnableTeamAsOwner;
                //viewModel.IsTeamAsOwnerMandatory = template.IsTeamAsOwnerMandatory;
                //if (viewModel.OverrideEnableTeamAsOwner.IsTrue())
                //{
                //    viewModel.EnableTeamAsOwner = viewModel.OverrideEnableTeamAsOwner.IsTrue();
                //}
                //if (viewModel.OverrideTeamAsOwnerMandatory.IsTrue())
                //{
                //    viewModel.IsTeamAsOwnerMandatory = viewModel.OverrideTeamAsOwnerMandatory.IsTrue();
                //}

                //viewModel.HideSubject = template.HideSubject;
                //viewModel.HideDescription = template.HideDescription;
                //viewModel.Layout = template.Layout;
                //viewModel.ReturnUrl = template.ReturnUrl;
                //viewModel.TemplateMasterColor = template.TemplateMasterColor;
                //viewModel.TemplateMasterFileId = template.TemplateMasterFileId;
                //viewModel.PrintButton = template.EnablePrintButton;
                //viewModel.PrintButtonText = template.PrintButtonText;
                //viewModel.PrintMethodName = template.PrintMethodName;
                //viewModel.EnableParent = template.EnableParent;

                viewModel.TextBoxDisplay1 = template.TextBoxDisplay1;
                viewModel.TextBoxLink1 = template.TextBoxLink1;
                if (viewModel.TextBoxLink1.IsNotNullAndNotEmpty() && viewModel.TextBoxLink1.Contains("{AppId}"))
                {
                    if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.REC_Application)
                    {
                        var link = viewModel.TextBoxLink1.Replace("{AppId}", viewModel.ReferenceTypeId);
                        viewModel.TextBoxLink1 = link;

                    }
                    else if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(viewModel.ReferenceTypeId);
                        var link = viewModel.TextBoxLink1.Replace("{AppId}", service.ReferenceTypeId);
                        viewModel.TextBoxLink1 = link;
                    }
                    if (viewModel.TextBoxLink1.Contains("{TaskStatus}"))
                    {
                        var link = viewModel.TextBoxLink1.Replace("{TaskStatus}", viewModel.TaskStatusCode);
                        viewModel.TextBoxLink1 = link;
                    }

                }
                else if (viewModel.TextBoxLink1.IsNotNullAndNotEmpty() && viewModel.TextBoxLink1.Contains("{AdvId}"))
                {
                    if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.REC_JobAdvertisement)
                    {
                        var link = viewModel.TextBoxLink1.Replace("{AdvId}", viewModel.ReferenceTypeId);
                        viewModel.TextBoxLink1 = link;

                    }
                    else if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(viewModel.ReferenceTypeId);
                        var link = viewModel.TextBoxLink1.Replace("{AdvId}", service.ReferenceTypeId);
                        viewModel.TextBoxLink1 = link;
                    }
                    if (viewModel.TextBoxLink1.Contains("{TaskStatus}"))
                    {
                        var link = viewModel.TextBoxLink1.Replace("{TaskStatus}", viewModel.TaskStatusCode);
                        viewModel.TextBoxLink1 = link;
                    }
                }


                viewModel.TextBoxLink2 = template.TextBoxLink2;
                if (viewModel.TextBoxLink2.IsNotNullAndNotEmpty() && viewModel.TextBoxLink2.Contains("{AppId}"))
                {
                    if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.REC_Application)
                    {
                        var link = viewModel.TextBoxLink2.Replace("{AppId}", viewModel.ReferenceTypeId);
                        viewModel.TextBoxLink2 = link;

                    }
                    else if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(viewModel.ReferenceTypeId);
                        var link = viewModel.TextBoxLink2.Replace("{AppId}", service.ReferenceTypeId);
                        viewModel.TextBoxLink2 = link;
                    }
                    if (viewModel.TextBoxLink2.Contains("{TaskStatus}"))
                    {
                        var link = viewModel.TextBoxLink2.Replace("{TaskStatus}", viewModel.TaskStatusCode);
                        viewModel.TextBoxLink2 = link;
                    }

                }
                else if (viewModel.TextBoxLink2.IsNotNullAndNotEmpty() && viewModel.TextBoxLink2.Contains("{AdvId}"))
                {
                    if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.REC_JobAdvertisement)
                    {
                        var link = viewModel.TextBoxLink2.Replace("{AdvId}", viewModel.ReferenceTypeId);
                        viewModel.TextBoxLink2 = link;

                    }
                    else if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(viewModel.ReferenceTypeId);
                        var link = viewModel.TextBoxLink2.Replace("{AdvId}", service.ReferenceTypeId);
                        viewModel.TextBoxLink2 = link;
                    }
                    if (viewModel.TextBoxLink2.Contains("{TaskStatus}"))
                    {
                        var link = viewModel.TextBoxLink2.Replace("{TaskStatus}", viewModel.TaskStatusCode);
                        viewModel.TextBoxLink2 = link;
                    }
                }
                viewModel.TextBoxLink3 = template.TextBoxLink3;
                if (viewModel.TextBoxLink3.IsNotNullAndNotEmpty() && viewModel.TextBoxLink3.Contains("{AppId}"))
                {
                    if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.REC_Application)
                    {
                        var link = viewModel.TextBoxLink3.Replace("{AppId}", viewModel.ReferenceTypeId);
                        viewModel.TextBoxLink3 = link;

                    }
                    else if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(viewModel.ReferenceTypeId);
                        var link = viewModel.TextBoxLink3.Replace("{AppId}", service.ReferenceTypeId);
                        viewModel.TextBoxLink3 = link;
                    }
                    if (viewModel.TextBoxLink3.Contains("{TaskStatus}"))
                    {
                        var link = viewModel.TextBoxLink3.Replace("{TaskStatus}", viewModel.TaskStatusCode);
                        viewModel.TextBoxLink3 = link;
                    }
                }
                else if (viewModel.TextBoxLink3.IsNotNullAndNotEmpty() && viewModel.TextBoxLink3.Contains("{AdvId}"))
                {
                    if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.REC_JobAdvertisement)
                    {
                        var link = viewModel.TextBoxLink3.Replace("{AdvId}", viewModel.ReferenceTypeId);
                        viewModel.TextBoxLink3 = link;

                    }
                    else if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(viewModel.ReferenceTypeId);
                        var link = viewModel.TextBoxLink3.Replace("{AdvId}", service.ReferenceTypeId);
                        viewModel.TextBoxLink3 = link;
                    }
                    if (viewModel.TextBoxLink3.Contains("{TaskStatus}"))
                    {
                        var link = viewModel.TextBoxLink3.Replace("{TaskStatus}", viewModel.TaskStatusCode);
                        viewModel.TextBoxLink3 = link;
                    }
                }
                viewModel.TextBoxLink4 = template.TextBoxLink4;
                if (viewModel.TextBoxLink4.IsNotNullAndNotEmpty() && viewModel.TextBoxLink4.Contains("{AppId}"))
                {
                    if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.REC_Application)
                    {
                        var link = viewModel.TextBoxLink4.Replace("{AppId}", viewModel.ReferenceTypeId);
                        viewModel.TextBoxLink4 = link;

                    }
                    else if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(viewModel.ReferenceTypeId);
                        var link = viewModel.TextBoxLink4.Replace("{AppId}", service.ReferenceTypeId);
                        viewModel.TextBoxLink4 = link;
                    }
                    if (viewModel.TextBoxLink4.Contains("{TaskStatus}"))
                    {
                        var link = viewModel.TextBoxLink4.Replace("{TaskStatus}", viewModel.TaskStatusCode);
                        viewModel.TextBoxLink4 = link;
                    }

                }
                else if (viewModel.TextBoxLink4.IsNotNullAndNotEmpty() && viewModel.TextBoxLink4.Contains("{AdvId}"))
                {
                    if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.REC_JobAdvertisement)
                    {
                        var link = viewModel.TextBoxLink4.Replace("{AdvId}", viewModel.ReferenceTypeId);
                        viewModel.TextBoxLink4 = link;

                    }
                    else if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(viewModel.ReferenceTypeId);
                        var link = viewModel.TextBoxLink4.Replace("{AdvId}", service.ReferenceTypeId);
                        viewModel.TextBoxLink4 = link;
                    }
                    if (viewModel.TextBoxLink4.Contains("{TaskStatus}"))
                    {
                        var link = viewModel.TextBoxLink4.Replace("{TaskStatus}", viewModel.TaskStatusCode);
                        viewModel.TextBoxLink4 = link;
                    }
                }
                viewModel.TextBoxLink5 = template.TextBoxLink5;
                if (viewModel.TextBoxLink5.IsNotNullAndNotEmpty() && viewModel.TextBoxLink5.Contains("{AppId}"))
                {
                    if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.REC_Application)
                    {
                        var link = viewModel.TextBoxLink5.Replace("{AppId}", viewModel.ReferenceTypeId);
                        viewModel.TextBoxLink5 = link;

                    }
                    else if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(viewModel.ReferenceTypeId);
                        var link = viewModel.TextBoxLink5.Replace("{AppId}", service.ReferenceTypeId);
                        viewModel.TextBoxLink5 = link;
                    }
                    if (viewModel.TextBoxLink5.Contains("{TaskStatus}"))
                    {
                        var link = viewModel.TextBoxLink5.Replace("{TaskStatus}", viewModel.TaskStatusCode);
                        viewModel.TextBoxLink5 = link;
                    }
                }
                else if (viewModel.TextBoxLink5.IsNotNullAndNotEmpty() && viewModel.TextBoxLink5.Contains("{AdvId}"))
                {
                    if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.REC_JobAdvertisement)
                    {
                        var link = viewModel.TextBoxLink5.Replace("{AdvId}", viewModel.ReferenceTypeId);
                        viewModel.TextBoxLink5 = link;

                    }
                    else if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(viewModel.ReferenceTypeId);
                        var link = viewModel.TextBoxLink5.Replace("{AdvId}", service.ReferenceTypeId);
                        viewModel.TextBoxLink5 = link;
                    }
                    if (viewModel.TextBoxLink5.Contains("{TaskStatus}"))
                    {
                        var link = viewModel.TextBoxLink5.Replace("{TaskStatus}", viewModel.TaskStatusCode);
                        viewModel.TextBoxLink5 = link;
                    }
                }
                viewModel.TextBoxLink6 = template.TextBoxLink6;
                if (viewModel.TextBoxLink6.IsNotNullAndNotEmpty() && viewModel.TextBoxLink6.Contains("{AppId}"))
                {
                    if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.REC_Application)
                    {
                        var link = viewModel.TextBoxLink6.Replace("{AppId}", viewModel.ReferenceTypeId);
                        viewModel.TextBoxLink6 = link;

                    }
                    else if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(viewModel.ReferenceTypeId);
                        var link = viewModel.TextBoxLink6.Replace("{AppId}", service.ReferenceTypeId);
                        viewModel.TextBoxLink6 = link;
                    }
                    if (viewModel.TextBoxLink6.Contains("{TaskStatus}"))
                    {
                        var link = viewModel.TextBoxLink6.Replace("{TaskStatus}", viewModel.TaskStatusCode);
                        viewModel.TextBoxLink6 = link;
                    }

                }
                else if (viewModel.TextBoxLink6.IsNotNullAndNotEmpty() && viewModel.TextBoxLink6.Contains("{AdvId}"))
                {
                    if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.REC_JobAdvertisement)
                    {
                        var link = viewModel.TextBoxLink6.Replace("{AdvId}", viewModel.ReferenceTypeId);
                        viewModel.TextBoxLink6 = link;

                    }
                    else if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(viewModel.ReferenceTypeId);
                        var link = viewModel.TextBoxLink6.Replace("{AdvId}", service.ReferenceTypeId);
                        viewModel.TextBoxLink6 = link;
                    }
                    if (viewModel.TextBoxLink6.Contains("{TaskStatus}"))
                    {
                        var link = viewModel.TextBoxLink6.Replace("{TaskStatus}", viewModel.TaskStatusCode);
                        viewModel.TextBoxLink6 = link;
                    }
                }
                viewModel.TextBoxLink7 = template.TextBoxLink7;
                if (viewModel.TextBoxLink7.IsNotNullAndNotEmpty() && viewModel.TextBoxLink7.Contains("{AppId}"))
                {
                    if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.REC_Application)
                    {
                        var link = viewModel.TextBoxLink7.Replace("{AppId}", viewModel.ReferenceTypeId);
                        viewModel.TextBoxLink7 = link;

                    }
                    else if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(viewModel.ReferenceTypeId);
                        var link = viewModel.TextBoxLink7.Replace("{AppId}", service.ReferenceTypeId);
                        viewModel.TextBoxLink7 = link;
                    }
                    if (viewModel.TextBoxLink7.Contains("{TaskStatus}"))
                    {
                        var link = viewModel.TextBoxLink7.Replace("{TaskStatus}", viewModel.TaskStatusCode);
                        viewModel.TextBoxLink7 = link;
                    }
                }
                else if (viewModel.TextBoxLink7.IsNotNullAndNotEmpty() && viewModel.TextBoxLink7.Contains("{AdvId}"))
                {
                    if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.REC_JobAdvertisement)
                    {
                        var link = viewModel.TextBoxLink7.Replace("{AdvId}", viewModel.ReferenceTypeId);
                        viewModel.TextBoxLink7 = link;

                    }
                    else if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(viewModel.ReferenceTypeId);
                        var link = viewModel.TextBoxLink7.Replace("{AdvId}", service.ReferenceTypeId);
                        viewModel.TextBoxLink7 = link;
                    }
                    if (viewModel.TextBoxLink7.Contains("{TaskStatus}"))
                    {
                        var link = viewModel.TextBoxLink7.Replace("{TaskStatus}", viewModel.TaskStatusCode);
                        viewModel.TextBoxLink7 = link;
                    }
                }
                viewModel.TextBoxLink8 = template.TextBoxLink8;
                if (viewModel.TextBoxLink8.IsNotNullAndNotEmpty() && viewModel.TextBoxLink8.Contains("{AppId}"))
                {
                    if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.REC_Application)
                    {
                        var link = viewModel.TextBoxLink8.Replace("{AppId}", viewModel.ReferenceTypeId);
                        viewModel.TextBoxLink8 = link;

                    }
                    else if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(viewModel.ReferenceTypeId);
                        var link = viewModel.TextBoxLink8.Replace("{AppId}", service.ReferenceTypeId);
                        viewModel.TextBoxLink8 = link;
                    }
                    if (viewModel.TextBoxLink8.Contains("{TaskStatus}"))
                    {
                        var link = viewModel.TextBoxLink8.Replace("{TaskStatus}", viewModel.TaskStatusCode);
                        viewModel.TextBoxLink8 = link;
                    }
                }
                else if (viewModel.TextBoxLink8.IsNotNullAndNotEmpty() && viewModel.TextBoxLink8.Contains("{AdvId}"))
                {
                    if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.REC_JobAdvertisement)
                    {
                        var link = viewModel.TextBoxLink8.Replace("{AdvId}", viewModel.ReferenceTypeId);
                        viewModel.TextBoxLink8 = link;

                    }
                    else if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(viewModel.ReferenceTypeId);
                        var link = viewModel.TextBoxLink8.Replace("{AdvId}", service.ReferenceTypeId);
                        viewModel.TextBoxLink8 = link;
                    }
                    if (viewModel.TextBoxLink8.Contains("{TaskStatus}"))
                    {
                        var link = viewModel.TextBoxLink8.Replace("{TaskStatus}", viewModel.TaskStatusCode);
                        viewModel.TextBoxLink8 = link;
                    }
                }
                viewModel.TextBoxLink9 = template.TextBoxLink9;
                if (viewModel.TextBoxLink9.IsNotNullAndNotEmpty() && viewModel.TextBoxLink9.Contains("{AppId}"))
                {
                    if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.REC_Application)
                    {
                        var link = viewModel.TextBoxLink9.Replace("{AppId}", viewModel.ReferenceTypeId);
                        viewModel.TextBoxLink9 = link;

                    }
                    else if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(viewModel.ReferenceTypeId);
                        var link = viewModel.TextBoxLink9.Replace("{AppId}", service.ReferenceTypeId);
                        viewModel.TextBoxLink9 = link;
                    }
                    if (viewModel.TextBoxLink9.Contains("{TaskStatus}"))
                    {
                        var link = viewModel.TextBoxLink9.Replace("{TaskStatus}", viewModel.TaskStatusCode);
                        viewModel.TextBoxLink9 = link;
                    }
                }
                else if (viewModel.TextBoxLink9.IsNotNullAndNotEmpty() && viewModel.TextBoxLink9.Contains("{AdvId}"))
                {
                    if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.REC_JobAdvertisement)
                    {
                        var link = viewModel.TextBoxLink9.Replace("{AdvId}", viewModel.ReferenceTypeId);
                        viewModel.TextBoxLink9 = link;

                    }
                    else if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(viewModel.ReferenceTypeId);
                        var link = viewModel.TextBoxLink9.Replace("{AdvId}", service.ReferenceTypeId);
                        viewModel.TextBoxLink9 = link;
                    }
                    if (viewModel.TextBoxLink9.Contains("{TaskStatus}"))
                    {
                        var link = viewModel.TextBoxLink9.Replace("{TaskStatus}", viewModel.TaskStatusCode);
                        viewModel.TextBoxLink9 = link;
                    }
                }
                viewModel.TextBoxLink10 = template.TextBoxLink10;
                if (viewModel.TextBoxLink10.IsNotNullAndNotEmpty() && viewModel.TextBoxLink10.Contains("{AppId}"))
                {
                    if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.REC_Application)
                    {
                        var link = viewModel.TextBoxLink10.Replace("{AppId}", viewModel.ReferenceTypeId);
                        viewModel.TextBoxLink10 = link;

                    }
                    else if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(viewModel.ReferenceTypeId);
                        var link = viewModel.TextBoxLink10.Replace("{AppId}", service.ReferenceTypeId);
                        viewModel.TextBoxLink10 = link;
                    }
                    if (viewModel.TextBoxLink10.Contains("{TaskStatus}"))
                    {
                        var link = viewModel.TextBoxLink10.Replace("{TaskStatus}", viewModel.TaskStatusCode);
                        viewModel.TextBoxLink10 = link;
                    }
                }
                else if (viewModel.TextBoxLink10.IsNotNullAndNotEmpty() && viewModel.TextBoxLink10.Contains("{AdvId}"))
                {
                    if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.REC_JobAdvertisement)
                    {
                        var link = viewModel.TextBoxLink10.Replace("{AdvId}", viewModel.ReferenceTypeId);
                        viewModel.TextBoxLink10 = link;

                    }
                    else if (viewModel.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                    {
                        var service = await GetSingleById(viewModel.ReferenceTypeId);
                        var link = viewModel.TextBoxLink10.Replace("{AdvId}", service.ReferenceTypeId);
                        viewModel.TextBoxLink10 = link;
                    }
                    if (viewModel.TextBoxLink10.Contains("{TaskStatus}"))
                    {
                        var link = viewModel.TextBoxLink10.Replace("{TaskStatus}", viewModel.TaskStatusCode);
                        viewModel.TextBoxLink10 = link;
                    }
                }
                viewModel.TextBoxDisplayType1 = template.TextBoxDisplayType1;
                viewModel.IsDropdownDisplay1 = template.IsDropdownDisplay1;
                viewModel.DropdownValueMethod1 = template.DropdownValueMethod1;
                viewModel.TextBoxDisplay2 = template.TextBoxDisplay2;
                viewModel.TextBoxDisplayType2 = template.TextBoxDisplayType2;
                viewModel.IsDropdownDisplay2 = template.IsDropdownDisplay2;
                viewModel.DropdownValueMethod2 = template.DropdownValueMethod2;
                viewModel.TextBoxDisplay3 = template.TextBoxDisplay3;
                viewModel.TextBoxDisplayType3 = template.TextBoxDisplayType3;
                viewModel.IsDropdownDisplay3 = template.IsDropdownDisplay3;
                viewModel.DropdownValueMethod3 = template.DropdownValueMethod3;
                viewModel.TextBoxDisplay4 = template.TextBoxDisplay4;
                viewModel.TextBoxDisplayType4 = template.TextBoxDisplayType4;
                viewModel.IsDropdownDisplay4 = template.IsDropdownDisplay4;
                viewModel.DropdownValueMethod4 = template.DropdownValueMethod4;
                viewModel.TextBoxDisplay5 = template.TextBoxDisplay5;
                viewModel.TextBoxDisplayType5 = template.TextBoxDisplayType5;
                viewModel.IsDropdownDisplay5 = template.IsDropdownDisplay5;
                viewModel.DropdownValueMethod5 = template.DropdownValueMethod5;

                viewModel.TextBoxDisplay6 = template.TextBoxDisplay6;
                viewModel.TextBoxDisplayType6 = template.TextBoxDisplayType6;
                viewModel.IsDropdownDisplay6 = template.IsDropdownDisplay6;
                viewModel.DropdownValueMethod6 = template.DropdownValueMethod6;
                viewModel.TextBoxDisplay7 = template.TextBoxDisplay7;
                viewModel.TextBoxDisplayType7 = template.TextBoxDisplayType7;
                viewModel.IsDropdownDisplay7 = template.IsDropdownDisplay7;
                viewModel.DropdownValueMethod7 = template.DropdownValueMethod7;
                viewModel.TextBoxDisplay8 = template.TextBoxDisplay8;
                viewModel.TextBoxDisplayType8 = template.TextBoxDisplayType8;
                viewModel.IsDropdownDisplay8 = template.IsDropdownDisplay8;
                viewModel.DropdownValueMethod8 = template.DropdownValueMethod8;
                viewModel.TextBoxDisplay9 = template.TextBoxDisplay9;
                viewModel.TextBoxDisplayType9 = template.TextBoxDisplayType9;
                viewModel.IsDropdownDisplay9 = template.IsDropdownDisplay9;
                viewModel.DropdownValueMethod9 = template.DropdownValueMethod9;
                viewModel.TextBoxDisplay10 = template.TextBoxDisplay10;
                viewModel.TextBoxDisplayType10 = template.TextBoxDisplayType10;
                viewModel.IsDropdownDisplay10 = template.IsDropdownDisplay10;
                viewModel.DropdownValueMethod10 = template.DropdownValueMethod10;
                viewModel.IsAssigneeAvailableTextBoxDisplay1 = template.IsAssigneeAvailableTextBoxDisplay1;
                viewModel.IsAssigneeAvailableTextBoxDisplay2 = template.IsAssigneeAvailableTextBoxDisplay2;
                viewModel.IsAssigneeAvailableTextBoxDisplay3 = template.IsAssigneeAvailableTextBoxDisplay3;
                viewModel.IsAssigneeAvailableTextBoxDisplay4 = template.IsAssigneeAvailableTextBoxDisplay4;
                viewModel.IsAssigneeAvailableTextBoxDisplay5 = template.IsAssigneeAvailableTextBoxDisplay5;
                viewModel.IsAssigneeAvailableTextBoxDisplay6 = template.IsAssigneeAvailableTextBoxDisplay6;
                viewModel.IsAssigneeAvailableTextBoxDisplay7 = template.IsAssigneeAvailableTextBoxDisplay7;
                viewModel.IsAssigneeAvailableTextBoxDisplay8 = template.IsAssigneeAvailableTextBoxDisplay8;
                viewModel.IsAssigneeAvailableTextBoxDisplay9 = template.IsAssigneeAvailableTextBoxDisplay9;
                viewModel.IsAssigneeAvailableTextBoxDisplay10 = template.IsAssigneeAvailableTextBoxDisplay10;
                viewModel.IsAssigneeAvailableDropdownDisplay1 = template.IsAssigneeAvailableDropdownDisplay1;
                viewModel.IsAssigneeAvailableDropdownDisplay2 = template.IsAssigneeAvailableDropdownDisplay2;
                viewModel.IsAssigneeAvailableDropdownDisplay3 = template.IsAssigneeAvailableDropdownDisplay3;
                viewModel.IsAssigneeAvailableDropdownDisplay4 = template.IsAssigneeAvailableDropdownDisplay4;
                viewModel.IsAssigneeAvailableDropdownDisplay5 = template.IsAssigneeAvailableDropdownDisplay5;
                viewModel.IsAssigneeAvailableDropdownDisplay6 = template.IsAssigneeAvailableDropdownDisplay6;
                viewModel.IsAssigneeAvailableDropdownDisplay7 = template.IsAssigneeAvailableDropdownDisplay7;
                viewModel.IsAssigneeAvailableDropdownDisplay8 = template.IsAssigneeAvailableDropdownDisplay8;
                viewModel.IsAssigneeAvailableDropdownDisplay9 = template.IsAssigneeAvailableDropdownDisplay9;
                viewModel.IsAssigneeAvailableDropdownDisplay10 = template.IsAssigneeAvailableDropdownDisplay10;

                viewModel.TextDisplay1 = template.TextBoxDisplay1;
                viewModel.TextDisplay2 = template.TextBoxDisplay2;
                viewModel.TextDisplay3 = template.TextBoxDisplay3;
                viewModel.TextDisplay4 = template.TextBoxDisplay4;
                viewModel.TextDisplay5 = template.TextBoxDisplay5;
                viewModel.TextDisplay6 = template.TextBoxDisplay6;
                viewModel.TextDisplay7 = template.TextBoxDisplay7;
                viewModel.TextDisplay8 = template.TextBoxDisplay8;
                viewModel.TextDisplay9 = template.TextBoxDisplay9;
                viewModel.TextDisplay10 = template.TextBoxDisplay10;
                viewModel.DropdownDisplay1 = template.IsDropdownDisplay1;
                viewModel.DropdownDisplay2 = template.IsDropdownDisplay2;
                viewModel.DropdownDisplay3 = template.IsDropdownDisplay3;
                viewModel.DropdownDisplay4 = template.IsDropdownDisplay4;
                viewModel.DropdownDisplay5 = template.IsDropdownDisplay5;
                viewModel.DropdownDisplay6 = template.IsDropdownDisplay6;
                viewModel.DropdownDisplay7 = template.IsDropdownDisplay7;
                viewModel.DropdownDisplay8 = template.IsDropdownDisplay8;
                viewModel.DropdownDisplay9 = template.IsDropdownDisplay9;
                viewModel.DropdownDisplay10 = template.IsDropdownDisplay10;

                viewModel.IsRequiredTextBoxDisplay1 = template.IsRequiredTextBoxDisplay1;
                viewModel.IsRequiredTextBoxDisplay2 = template.IsRequiredTextBoxDisplay2;
                viewModel.IsRequiredTextBoxDisplay3 = template.IsRequiredTextBoxDisplay3;
                viewModel.IsRequiredTextBoxDisplay4 = template.IsRequiredTextBoxDisplay4;
                viewModel.IsRequiredTextBoxDisplay5 = template.IsRequiredTextBoxDisplay5;
                viewModel.IsRequiredTextBoxDisplay6 = template.IsRequiredTextBoxDisplay6;
                viewModel.IsRequiredTextBoxDisplay7 = template.IsRequiredTextBoxDisplay7;
                viewModel.IsRequiredTextBoxDisplay8 = template.IsRequiredTextBoxDisplay8;
                viewModel.IsRequiredTextBoxDisplay9 = template.IsRequiredTextBoxDisplay9;
                viewModel.IsRequiredTextBoxDisplay10 = template.IsRequiredTextBoxDisplay10;
                viewModel.IsRequiredDropdownDisplay1 = template.IsRequiredDropdownDisplay1;
                viewModel.IsRequiredDropdownDisplay2 = template.IsRequiredDropdownDisplay2;
                viewModel.IsRequiredDropdownDisplay3 = template.IsRequiredDropdownDisplay3;
                viewModel.IsRequiredDropdownDisplay4 = template.IsRequiredDropdownDisplay4;
                viewModel.IsRequiredDropdownDisplay5 = template.IsRequiredDropdownDisplay5;
                viewModel.IsRequiredDropdownDisplay6 = template.IsRequiredDropdownDisplay6;
                viewModel.IsRequiredDropdownDisplay7 = template.IsRequiredDropdownDisplay7;
                viewModel.IsRequiredDropdownDisplay8 = template.IsRequiredDropdownDisplay8;
                viewModel.IsRequiredDropdownDisplay9 = template.IsRequiredDropdownDisplay9;
                viewModel.IsRequiredDropdownDisplay10 = template.IsRequiredDropdownDisplay10;

            }
            else
            {
                if (template.IsNotNull())
                {
                    if (template.UdfIframeSrc.IsNotNullAndNotEmpty() && template.UdfIframeSrc.Contains("{batchId}"))
                    {
                        if (template.ReferenceTypeCode == ReferenceTypeEnum.REC_WorkerPoolBatch)
                        {
                            var link = template.UdfIframeSrc.Replace("{batchId}", template.ReferenceTypeId);
                            template.UdfIframeSrc = link;

                        }
                        else if (template.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                        {
                            var service = await GetSingleById(template.ReferenceTypeId);
                            var link = template.UdfIframeSrc.Replace("{batchId}", service.ReferenceTypeId);
                            template.UdfIframeSrc = link;
                        }

                    }
                    if (template.TextBoxLink1.IsNotNullAndNotEmpty() && template.TextBoxLink1.Contains("{AppId}"))
                    {
                        if (template.ReferenceTypeCode == ReferenceTypeEnum.REC_Application)
                        {
                            var link = template.TextBoxLink1.Replace("{AppId}", template.ReferenceTypeId);
                            template.TextBoxLink1 = link;

                        }
                        else if (template.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                        {
                            var service = await GetSingleById(template.ReferenceTypeId);
                            var link = template.TextBoxLink1.Replace("{AppId}", service.ReferenceTypeId);
                            template.TextBoxLink1 = link;
                        }
                        if (template.TextBoxLink1.Contains("{TaskStatus}"))
                        {
                            var link = template.TextBoxLink1.Replace("{TaskStatus}", template.TaskStatusCode);
                            template.TextBoxLink1 = link;
                        }
                    }
                    else if (template.TextBoxLink1.IsNotNullAndNotEmpty() && template.TextBoxLink1.Contains("{AdvId}"))
                    {
                        if (template.ReferenceTypeCode == ReferenceTypeEnum.REC_JobAdvertisement)
                        {
                            var link = template.TextBoxLink1.Replace("{AdvId}", template.ReferenceTypeId);
                            template.TextBoxLink1 = link;

                        }
                        else if (template.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                        {
                            var service = await GetSingleById(template.ReferenceTypeId);
                            var link = template.TextBoxLink1.Replace("{AdvId}", service.ReferenceTypeId);
                            template.TextBoxLink1 = link;
                        }
                        if (template.TextBoxLink1.Contains("{TaskStatus}"))
                        {
                            var link = template.TextBoxLink1.Replace("{TaskStatus}", template.TaskStatusCode);
                            template.TextBoxLink1 = link;
                        }
                    }
                    if (template.TextBoxLink2.IsNotNullAndNotEmpty() && template.TextBoxLink2.Contains("{AppId}"))
                    {
                        if (template.ReferenceTypeCode == ReferenceTypeEnum.REC_Application)
                        {
                            var link = template.TextBoxLink2.Replace("{AppId}", template.ReferenceTypeId);
                            template.TextBoxLink2 = link;

                        }
                        else if (template.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                        {
                            var service = await GetSingleById(template.ReferenceTypeId);
                            var link = template.TextBoxLink2.Replace("{AppId}", service.ReferenceTypeId);
                            template.TextBoxLink2 = link;
                        }
                        if (template.TextBoxLink2.Contains("{TaskStatus}"))
                        {
                            var link = template.TextBoxLink2.Replace("{TaskStatus}", template.TaskStatusCode);
                            template.TextBoxLink2 = link;
                        }
                    }
                    else if (template.TextBoxLink2.IsNotNullAndNotEmpty() && template.TextBoxLink2.Contains("{AdvId}"))
                    {
                        if (template.ReferenceTypeCode == ReferenceTypeEnum.REC_JobAdvertisement)
                        {
                            var link = template.TextBoxLink2.Replace("{AdvId}", template.ReferenceTypeId);
                            template.TextBoxLink2 = link;

                        }
                        else if (template.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                        {
                            var service = await GetSingleById(template.ReferenceTypeId);
                            var link = template.TextBoxLink2.Replace("{AdvId}", service.ReferenceTypeId);
                            template.TextBoxLink2 = link;
                        }
                        if (template.TextBoxLink2.Contains("{TaskStatus}"))
                        {
                            var link = template.TextBoxLink2.Replace("{TaskStatus}", template.TaskStatusCode);
                            template.TextBoxLink2 = link;
                        }
                    }
                    if (template.TextBoxLink3.IsNotNullAndNotEmpty() && template.TextBoxLink3.Contains("{AppId}"))
                    {
                        if (template.ReferenceTypeCode == ReferenceTypeEnum.REC_Application)
                        {
                            var link = template.TextBoxLink3.Replace("{AppId}", template.ReferenceTypeId);
                            template.TextBoxLink3 = link;

                        }
                        else if (template.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                        {
                            var service = await GetSingleById(template.ReferenceTypeId);
                            var link = template.TextBoxLink3.Replace("{AppId}", service.ReferenceTypeId);
                            template.TextBoxLink3 = link;
                        }
                        if (template.TextBoxLink3.Contains("{TaskStatus}"))
                        {
                            var link = template.TextBoxLink3.Replace("{TaskStatus}", template.TaskStatusCode);
                            template.TextBoxLink3 = link;
                        }
                    }
                    else if (template.TextBoxLink3.IsNotNullAndNotEmpty() && template.TextBoxLink3.Contains("{AdvId}"))
                    {
                        if (template.ReferenceTypeCode == ReferenceTypeEnum.REC_JobAdvertisement)
                        {
                            var link = template.TextBoxLink3.Replace("{AdvId}", template.ReferenceTypeId);
                            template.TextBoxLink3 = link;

                        }
                        else if (template.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                        {
                            var service = await GetSingleById(template.ReferenceTypeId);
                            var link = template.TextBoxLink3.Replace("{AdvId}", service.ReferenceTypeId);
                            template.TextBoxLink3 = link;
                        }
                        if (template.TextBoxLink3.Contains("{TaskStatus}"))
                        {
                            var link = template.TextBoxLink3.Replace("{TaskStatus}", template.TaskStatusCode);
                            template.TextBoxLink3 = link;
                        }
                    }
                    if (template.TextBoxLink4.IsNotNullAndNotEmpty() && template.TextBoxLink4.Contains("{AppId}"))
                    {
                        if (template.ReferenceTypeCode == ReferenceTypeEnum.REC_Application)
                        {
                            var link = template.TextBoxLink4.Replace("{AppId}", template.ReferenceTypeId);
                            template.TextBoxLink4 = link;

                        }
                        else if (template.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                        {
                            var service = await GetSingleById(template.ReferenceTypeId);
                            var link = template.TextBoxLink4.Replace("{AppId}", service.ReferenceTypeId);
                            template.TextBoxLink4 = link;
                        }
                        if (template.TextBoxLink4.Contains("{TaskStatus}"))
                        {
                            var link = template.TextBoxLink4.Replace("{TaskStatus}", template.TaskStatusCode);
                            template.TextBoxLink4 = link;
                        }
                    }
                    else if (template.TextBoxLink4.IsNotNullAndNotEmpty() && template.TextBoxLink4.Contains("{AdvId}"))
                    {
                        if (template.ReferenceTypeCode == ReferenceTypeEnum.REC_JobAdvertisement)
                        {
                            var link = template.TextBoxLink4.Replace("{AdvId}", template.ReferenceTypeId);
                            template.TextBoxLink4 = link;

                        }
                        else if (template.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                        {
                            var service = await GetSingleById(template.ReferenceTypeId);
                            var link = template.TextBoxLink4.Replace("{AdvId}", service.ReferenceTypeId);
                            template.TextBoxLink4 = link;
                        }
                        if (template.TextBoxLink4.Contains("{TaskStatus}"))
                        {
                            var link = template.TextBoxLink4.Replace("{TaskStatus}", template.TaskStatusCode);
                            template.TextBoxLink4 = link;
                        }
                    }
                    if (template.TextBoxLink5.IsNotNullAndNotEmpty() && template.TextBoxLink5.Contains("{AppId}"))
                    {
                        if (template.ReferenceTypeCode == ReferenceTypeEnum.REC_Application)
                        {
                            var link = template.TextBoxLink5.Replace("{AppId}", template.ReferenceTypeId);
                            template.TextBoxLink5 = link;

                        }
                        else if (template.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                        {
                            var service = await GetSingleById(template.ReferenceTypeId);
                            var link = template.TextBoxLink5.Replace("{AppId}", service.ReferenceTypeId);
                            template.TextBoxLink5 = link;
                        }
                        if (template.TextBoxLink5.Contains("{TaskStatus}"))
                        {
                            var link = template.TextBoxLink5.Replace("{TaskStatus}", template.TaskStatusCode);
                            template.TextBoxLink5 = link;
                        }
                    }
                    else if (template.TextBoxLink5.IsNotNullAndNotEmpty() && template.TextBoxLink5.Contains("{AdvId}"))
                    {
                        if (template.ReferenceTypeCode == ReferenceTypeEnum.REC_JobAdvertisement)
                        {
                            var link = template.TextBoxLink5.Replace("{AdvId}", template.ReferenceTypeId);
                            template.TextBoxLink5 = link;

                        }
                        else if (template.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                        {
                            var service = await GetSingleById(template.ReferenceTypeId);
                            var link = template.TextBoxLink5.Replace("{AdvId}", service.ReferenceTypeId);
                            template.TextBoxLink5 = link;
                        }
                        if (template.TextBoxLink5.Contains("{TaskStatus}"))
                        {
                            var link = template.TextBoxLink5.Replace("{TaskStatus}", template.TaskStatusCode);
                            template.TextBoxLink5 = link;
                        }
                    }
                    if (template.TextBoxLink6.IsNotNullAndNotEmpty() && template.TextBoxLink6.Contains("{AppId}"))
                    {
                        if (template.ReferenceTypeCode == ReferenceTypeEnum.REC_Application)
                        {
                            var link = template.TextBoxLink6.Replace("{AppId}", template.ReferenceTypeId);
                            template.TextBoxLink6 = link;

                        }
                        else if (template.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                        {
                            var service = await GetSingleById(template.ReferenceTypeId);
                            var link = template.TextBoxLink6.Replace("{AppId}", service.ReferenceTypeId);
                            template.TextBoxLink6 = link;
                        }
                        if (template.TextBoxLink6.Contains("{TaskStatus}"))
                        {
                            var link = template.TextBoxLink6.Replace("{TaskStatus}", template.TaskStatusCode);
                            template.TextBoxLink6 = link;
                        }
                    }
                    else if (template.TextBoxLink6.IsNotNullAndNotEmpty() && template.TextBoxLink6.Contains("{AdvId}"))
                    {
                        if (template.ReferenceTypeCode == ReferenceTypeEnum.REC_JobAdvertisement)
                        {
                            var link = template.TextBoxLink6.Replace("{AdvId}", template.ReferenceTypeId);
                            template.TextBoxLink6 = link;

                        }
                        else if (template.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                        {
                            var service = await GetSingleById(template.ReferenceTypeId);
                            var link = template.TextBoxLink6.Replace("{AdvId}", service.ReferenceTypeId);
                            template.TextBoxLink6 = link;
                        }
                        if (template.TextBoxLink6.Contains("{TaskStatus}"))
                        {
                            var link = template.TextBoxLink6.Replace("{TaskStatus}", template.TaskStatusCode);
                            template.TextBoxLink6 = link;
                        }
                    }
                    if (template.TextBoxLink7.IsNotNullAndNotEmpty() && template.TextBoxLink7.Contains("{AppId}"))
                    {
                        if (template.ReferenceTypeCode == ReferenceTypeEnum.REC_Application)
                        {
                            var link = template.TextBoxLink7.Replace("{AppId}", template.ReferenceTypeId);
                            template.TextBoxLink7 = link;

                        }
                        else if (template.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                        {
                            var service = await GetSingleById(template.ReferenceTypeId);
                            var link = template.TextBoxLink7.Replace("{AppId}", service.ReferenceTypeId);
                            template.TextBoxLink7 = link;
                        }
                        if (template.TextBoxLink7.Contains("{TaskStatus}"))
                        {
                            var link = template.TextBoxLink7.Replace("{TaskStatus}", template.TaskStatusCode);
                            template.TextBoxLink7 = link;
                        }
                    }
                    else if (template.TextBoxLink7.IsNotNullAndNotEmpty() && template.TextBoxLink7.Contains("{AdvId}"))
                    {
                        if (template.ReferenceTypeCode == ReferenceTypeEnum.REC_JobAdvertisement)
                        {
                            var link = template.TextBoxLink7.Replace("{AdvId}", template.ReferenceTypeId);
                            template.TextBoxLink7 = link;

                        }
                        else if (template.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                        {
                            var service = await GetSingleById(template.ReferenceTypeId);
                            var link = template.TextBoxLink7.Replace("{AdvId}", service.ReferenceTypeId);
                            template.TextBoxLink7 = link;
                        }
                        if (template.TextBoxLink7.Contains("{TaskStatus}"))
                        {
                            var link = template.TextBoxLink7.Replace("{TaskStatus}", template.TaskStatusCode);
                            template.TextBoxLink7 = link;
                        }
                    }
                    if (template.TextBoxLink8.IsNotNullAndNotEmpty() && template.TextBoxLink8.Contains("{AppId}"))
                    {
                        if (template.ReferenceTypeCode == ReferenceTypeEnum.REC_Application)
                        {
                            var link = template.TextBoxLink8.Replace("{AppId}", template.ReferenceTypeId);
                            template.TextBoxLink8 = link;

                        }
                        else if (template.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                        {
                            var service = await GetSingleById(template.ReferenceTypeId);
                            var link = template.TextBoxLink8.Replace("{AppId}", service.ReferenceTypeId);
                            template.TextBoxLink8 = link;
                        }
                        if (template.TextBoxLink8.Contains("{TaskStatus}"))
                        {
                            var link = template.TextBoxLink8.Replace("{TaskStatus}", template.TaskStatusCode);
                            template.TextBoxLink8 = link;
                        }
                    }
                    else if (template.TextBoxLink8.IsNotNullAndNotEmpty() && template.TextBoxLink8.Contains("{AdvId}"))
                    {
                        if (template.ReferenceTypeCode == ReferenceTypeEnum.REC_JobAdvertisement)
                        {
                            var link = template.TextBoxLink8.Replace("{AdvId}", template.ReferenceTypeId);
                            template.TextBoxLink8 = link;

                        }
                        else if (template.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                        {
                            var service = await GetSingleById(template.ReferenceTypeId);
                            var link = template.TextBoxLink8.Replace("{AdvId}", service.ReferenceTypeId);
                            template.TextBoxLink8 = link;
                        }
                        if (template.TextBoxLink8.Contains("{TaskStatus}"))
                        {
                            var link = template.TextBoxLink8.Replace("{TaskStatus}", template.TaskStatusCode);
                            template.TextBoxLink8 = link;
                        }
                    }
                    if (template.TextBoxLink9.IsNotNullAndNotEmpty() && template.TextBoxLink9.Contains("{AppId}"))
                    {
                        if (template.ReferenceTypeCode == ReferenceTypeEnum.REC_Application)
                        {
                            var link = template.TextBoxLink9.Replace("{AppId}", template.ReferenceTypeId);
                            template.TextBoxLink9 = link;

                        }
                        else if (template.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                        {
                            var service = await GetSingleById(template.ReferenceTypeId);
                            var link = template.TextBoxLink9.Replace("{AppId}", service.ReferenceTypeId);
                            template.TextBoxLink9 = link;
                        }
                        if (template.TextBoxLink9.Contains("{TaskStatus}"))
                        {
                            var link = template.TextBoxLink9.Replace("{TaskStatus}", template.TaskStatusCode);
                            template.TextBoxLink9 = link;
                        }
                    }
                    else if (template.TextBoxLink9.IsNotNullAndNotEmpty() && template.TextBoxLink9.Contains("{AdvId}"))
                    {
                        if (template.ReferenceTypeCode == ReferenceTypeEnum.REC_JobAdvertisement)
                        {
                            var link = template.TextBoxLink9.Replace("{AdvId}", template.ReferenceTypeId);
                            template.TextBoxLink9 = link;

                        }
                        else if (template.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                        {
                            var service = await GetSingleById(template.ReferenceTypeId);
                            var link = template.TextBoxLink9.Replace("{AdvId}", service.ReferenceTypeId);
                            template.TextBoxLink9 = link;
                        }
                        if (template.TextBoxLink9.Contains("{TaskStatus}"))
                        {
                            var link = template.TextBoxLink9.Replace("{TaskStatus}", template.TaskStatusCode);
                            template.TextBoxLink9 = link;
                        }
                    }
                    if (template.TextBoxLink10.IsNotNullAndNotEmpty() && template.TextBoxLink10.Contains("{AppId}"))
                    {
                        if (template.ReferenceTypeCode == ReferenceTypeEnum.REC_Application)
                        {
                            var link = template.TextBoxLink10.Replace("{AppId}", template.ReferenceTypeId);
                            template.TextBoxLink10 = link;

                        }
                        else if (template.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                        {
                            var service = await GetSingleById(template.ReferenceTypeId);
                            var link = template.TextBoxLink10.Replace("{AppId}", service.ReferenceTypeId);
                            template.TextBoxLink10 = link;
                        }
                        if (template.TextBoxLink10.Contains("{TaskStatus}"))
                        {
                            var link = template.TextBoxLink10.Replace("{TaskStatus}", template.TaskStatusCode);
                            template.TextBoxLink10 = link;
                        }
                    }
                    else if (template.TextBoxLink10.IsNotNullAndNotEmpty() && template.TextBoxLink10.Contains("{AdvId}"))
                    {
                        if (template.ReferenceTypeCode == ReferenceTypeEnum.REC_JobAdvertisement)
                        {
                            var link = template.TextBoxLink10.Replace("{AdvId}", template.ReferenceTypeId);
                            template.TextBoxLink10 = link;

                        }
                        else if (template.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task)
                        {
                            var service = await GetSingleById(template.ReferenceTypeId);
                            var link = template.TextBoxLink10.Replace("{AdvId}", service.ReferenceTypeId);
                            template.TextBoxLink10 = link;
                        }
                        if (template.TextBoxLink10.Contains("{TaskStatus}"))
                        {
                            var link = template.TextBoxLink10.Replace("{TaskStatus}", template.TaskStatusCode);
                            template.TextBoxLink10 = link;
                        }
                    }
                    template.TextDisplay1 = template.TextBoxDisplay1;
                    template.TextDisplay2 = template.TextBoxDisplay2;
                    template.TextDisplay3 = template.TextBoxDisplay3;
                    template.TextDisplay4 = template.TextBoxDisplay4;
                    template.TextDisplay5 = template.TextBoxDisplay5;
                    template.TextDisplay6 = template.TextBoxDisplay6;
                    template.TextDisplay7 = template.TextBoxDisplay7;
                    template.TextDisplay8 = template.TextBoxDisplay8;
                    template.TextDisplay9 = template.TextBoxDisplay9;
                    template.TextDisplay10 = template.TextBoxDisplay10;
                    template.DropdownDisplay1 = template.IsDropdownDisplay1;
                    template.DropdownDisplay2 = template.IsDropdownDisplay2;
                    template.DropdownDisplay3 = template.IsDropdownDisplay3;
                    template.DropdownDisplay4 = template.IsDropdownDisplay4;
                    template.DropdownDisplay5 = template.IsDropdownDisplay5;
                    template.DropdownDisplay6 = template.IsDropdownDisplay6;
                    template.DropdownDisplay7 = template.IsDropdownDisplay7;
                    template.DropdownDisplay8 = template.IsDropdownDisplay8;
                    template.DropdownDisplay9 = template.IsDropdownDisplay9;
                    template.DropdownDisplay10 = template.IsDropdownDisplay10;


                }


            }
        }
        public void SetDisplayMode(RecTaskViewModel template)
        {
            template.IsTextBoxEdit1 = template.DisplayMode == FieldDisplayModeEnum.Editable && (template.IsAssigneeAvailableTextBoxDisplay1 && template.TemplateUserType == NtsUserTypeEnum.Assignee) || (!template.IsAssigneeAvailableTextBoxDisplay1 && template.TemplateUserType == NtsUserTypeEnum.Owner && template.TemplateAction == NtsActionEnum.Draft) ? true : false;
            template.IsTextBoxEdit2 = template.DisplayMode == FieldDisplayModeEnum.Editable && (template.IsAssigneeAvailableTextBoxDisplay2 && template.TemplateUserType == NtsUserTypeEnum.Assignee) || (!template.IsAssigneeAvailableTextBoxDisplay2 && template.TemplateUserType == NtsUserTypeEnum.Owner && template.TemplateAction == NtsActionEnum.Draft) ? true : false;
            template.IsTextBoxEdit3 = template.DisplayMode == FieldDisplayModeEnum.Editable && (template.IsAssigneeAvailableTextBoxDisplay3 && template.TemplateUserType == NtsUserTypeEnum.Assignee) || (!template.IsAssigneeAvailableTextBoxDisplay3 && template.TemplateUserType == NtsUserTypeEnum.Owner && template.TemplateAction == NtsActionEnum.Draft) ? true : false;
            template.IsTextBoxEdit4 = template.DisplayMode == FieldDisplayModeEnum.Editable && (template.IsAssigneeAvailableTextBoxDisplay4 && template.TemplateUserType == NtsUserTypeEnum.Assignee) || (!template.IsAssigneeAvailableTextBoxDisplay4 && template.TemplateUserType == NtsUserTypeEnum.Owner && template.TemplateAction == NtsActionEnum.Draft) ? true : false;
            template.IsTextBoxEdit5 = template.DisplayMode == FieldDisplayModeEnum.Editable && (template.IsAssigneeAvailableTextBoxDisplay5 && template.TemplateUserType == NtsUserTypeEnum.Assignee) || (!template.IsAssigneeAvailableTextBoxDisplay5 && template.TemplateUserType == NtsUserTypeEnum.Owner && template.TemplateAction == NtsActionEnum.Draft) ? true : false;
            template.IsTextBoxEdit6 = template.DisplayMode == FieldDisplayModeEnum.Editable && (template.IsAssigneeAvailableTextBoxDisplay6 && template.TemplateUserType == NtsUserTypeEnum.Assignee) || (!template.IsAssigneeAvailableTextBoxDisplay6 && template.TemplateUserType == NtsUserTypeEnum.Owner && template.TemplateAction == NtsActionEnum.Draft) ? true : false;
            template.IsTextBoxEdit7 = template.DisplayMode == FieldDisplayModeEnum.Editable && (template.IsAssigneeAvailableTextBoxDisplay7 && template.TemplateUserType == NtsUserTypeEnum.Assignee) || (!template.IsAssigneeAvailableTextBoxDisplay7 && template.TemplateUserType == NtsUserTypeEnum.Owner && template.TemplateAction == NtsActionEnum.Draft) ? true : false;
            template.IsTextBoxEdit8 = template.DisplayMode == FieldDisplayModeEnum.Editable && (template.IsAssigneeAvailableTextBoxDisplay8 && template.TemplateUserType == NtsUserTypeEnum.Assignee) || (!template.IsAssigneeAvailableTextBoxDisplay8 && template.TemplateUserType == NtsUserTypeEnum.Owner && template.TemplateAction == NtsActionEnum.Draft) ? true : false;
            template.IsTextBoxEdit9 = template.DisplayMode == FieldDisplayModeEnum.Editable && (template.IsAssigneeAvailableTextBoxDisplay9 && template.TemplateUserType == NtsUserTypeEnum.Assignee) || (!template.IsAssigneeAvailableTextBoxDisplay9 && template.TemplateUserType == NtsUserTypeEnum.Owner && template.TemplateAction == NtsActionEnum.Draft) ? true : false;
            template.IsTextBoxEdit10 = template.DisplayMode == FieldDisplayModeEnum.Editable && (template.IsAssigneeAvailableTextBoxDisplay10 && template.TemplateUserType == NtsUserTypeEnum.Assignee) || (!template.IsAssigneeAvailableTextBoxDisplay10 && template.TemplateUserType == NtsUserTypeEnum.Owner && template.TemplateAction == NtsActionEnum.Draft) ? true : false;
            template.IsDropDownEdit1 = template.DisplayMode == FieldDisplayModeEnum.Editable && (template.IsAssigneeAvailableDropdownDisplay1 && template.TemplateUserType == NtsUserTypeEnum.Assignee) || (!template.IsAssigneeAvailableDropdownDisplay1 && template.TemplateUserType == NtsUserTypeEnum.Owner && template.TemplateAction == NtsActionEnum.Draft) ? true : false;
            template.IsDropDownEdit2 = template.DisplayMode == FieldDisplayModeEnum.Editable && (template.IsAssigneeAvailableDropdownDisplay2 && template.TemplateUserType == NtsUserTypeEnum.Assignee) || (!template.IsAssigneeAvailableDropdownDisplay2 && template.TemplateUserType == NtsUserTypeEnum.Owner && template.TemplateAction == NtsActionEnum.Draft) ? true : false;
            template.IsDropDownEdit3 = template.DisplayMode == FieldDisplayModeEnum.Editable && (template.IsAssigneeAvailableDropdownDisplay3 && template.TemplateUserType == NtsUserTypeEnum.Assignee) || (!template.IsAssigneeAvailableDropdownDisplay3 && template.TemplateUserType == NtsUserTypeEnum.Owner && template.TemplateAction == NtsActionEnum.Draft) ? true : false;
            template.IsDropDownEdit4 = template.DisplayMode == FieldDisplayModeEnum.Editable && (template.IsAssigneeAvailableDropdownDisplay4 && template.TemplateUserType == NtsUserTypeEnum.Assignee) || (!template.IsAssigneeAvailableDropdownDisplay4 && template.TemplateUserType == NtsUserTypeEnum.Owner && template.TemplateAction == NtsActionEnum.Draft) ? true : false;
            template.IsDropDownEdit5 = template.DisplayMode == FieldDisplayModeEnum.Editable && (template.IsAssigneeAvailableDropdownDisplay5 && template.TemplateUserType == NtsUserTypeEnum.Assignee) || (!template.IsAssigneeAvailableDropdownDisplay5 && template.TemplateUserType == NtsUserTypeEnum.Owner && template.TemplateAction == NtsActionEnum.Draft) ? true : false;
            template.IsDropDownEdit6 = template.DisplayMode == FieldDisplayModeEnum.Editable && (template.IsAssigneeAvailableDropdownDisplay6 && template.TemplateUserType == NtsUserTypeEnum.Assignee) || (!template.IsAssigneeAvailableDropdownDisplay6 && template.TemplateUserType == NtsUserTypeEnum.Owner && template.TemplateAction == NtsActionEnum.Draft) ? true : false;
            template.IsDropDownEdit7 = template.DisplayMode == FieldDisplayModeEnum.Editable && (template.IsAssigneeAvailableDropdownDisplay7 && template.TemplateUserType == NtsUserTypeEnum.Assignee) || (!template.IsAssigneeAvailableDropdownDisplay7 && template.TemplateUserType == NtsUserTypeEnum.Owner && template.TemplateAction == NtsActionEnum.Draft) ? true : false;
            template.IsDropDownEdit8 = template.DisplayMode == FieldDisplayModeEnum.Editable && (template.IsAssigneeAvailableDropdownDisplay8 && template.TemplateUserType == NtsUserTypeEnum.Assignee) || (!template.IsAssigneeAvailableDropdownDisplay8 && template.TemplateUserType == NtsUserTypeEnum.Owner && template.TemplateAction == NtsActionEnum.Draft) ? true : false;
            template.IsDropDownEdit9 = template.DisplayMode == FieldDisplayModeEnum.Editable && (template.IsAssigneeAvailableDropdownDisplay9 && template.TemplateUserType == NtsUserTypeEnum.Assignee) || (!template.IsAssigneeAvailableDropdownDisplay9 && template.TemplateUserType == NtsUserTypeEnum.Owner && template.TemplateAction == NtsActionEnum.Draft) ? true : false;
            template.IsDropDownEdit10 = template.DisplayMode == FieldDisplayModeEnum.Editable && (template.IsAssigneeAvailableDropdownDisplay10 && template.TemplateUserType == NtsUserTypeEnum.Assignee) || (!template.IsAssigneeAvailableDropdownDisplay10 && template.TemplateUserType == NtsUserTypeEnum.Owner && template.TemplateAction == NtsActionEnum.Draft) ? true : false;
        }

        public async Task<IList<RecTaskViewModel>> GetActiveListByUserId(string userId)
        {
            var list = new List<RecTaskViewModel>();
            string query = @$"SELECT task.*, ou.""Name"" as OwnerUserName, au.""Name"" as AssigneeUserName , substring( ou.""Name"" for 1) as TaskOwnerFirstLetter,
                        app.""FirstName"" as CandidateName,case when job.""Name"" is not null then job.""Name"" else hjob.""Name"" end as JobName,case when org.""Name"" is not null then org.""Name"" else horg.""Name"" end as OrgUnitName,app.""GaecNo"" as GaecNo,
                        tt.""SequenceOrder"" as SequenceOrder
                        FROM public.""RecTask"" as task
                        left join public.""RecTask"" as service on  service.""Id"" = task.""ReferenceTypeId"" 
                        left join rec.""Application"" as app on app.""Id"" = service.""ReferenceTypeId""
                        left join cms.""Job"" as job on  job.""Id"" = app.""JobId""
                        left join rec.""Batch"" as bt on bt.""Id"" = app.""BatchId""
                        left join cms.""Organization"" as org on org.""Id"" = bt.""OrganizationId""
                        left join public.""User"" as ou on  ou.""Id"" = task.""OwnerUserId""
                        left join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                        left join public.""RecTaskTemplate"" as tt on tt.""TemplateCode""=task.""TemplateCode""  
                        left join cms.""Job"" as hjob on  hjob.""Id"" = task.""DropdownValue1""
                        left join cms.""Organization"" as horg on horg.""Id"" = task.""DropdownValue2""
                        where (task.""AssigneeUserId"" ='{userId}') and (task.""NtsType"" is null or task.""NtsType""=1)";
            var result = await _queryRepo.ExecuteQueryList(query, null);
            foreach (var i in result)
            {
                i.DisplayDueDate = i.DueDate?.ToString("dd/MM/yyyy HH:mm:ss");
            }
            return result;
        }
        public async Task<IList<RecTaskViewModel>> GetTaskByTemplateCode(string tempCode)
        {
            var list = new List<RecTaskViewModel>();
            string query = @$"SELECT task.*, ou.""Name"" as OwnerUserName, au.""Name"" as AssigneeUserName , substring( ou.""Name"" for 1) as TaskOwnerFirstLetter,
                        app.""FirstName"" as CandidateName,case when job.""Name"" is not null then job.""Name"" else hjob.""Name"" end as JobName,case when org.""Name"" is not null then org.""Name"" else horg.""Name"" end as OrgUnitName,app.""GaecNo"" as GaecNo,
                        tt.""SequenceOrder"" as SequenceOrder
                        FROM public.""RecTask"" as task
                        left join public.""RecTask"" as service on  service.""Id"" = task.""ReferenceTypeId"" 
                        left join rec.""Application"" as app on app.""Id"" = service.""ReferenceTypeId""
                        left join cms.""Job"" as job on  job.""Id"" = app.""JobId""
                        left join rec.""Batch"" as bt on bt.""Id"" = app.""BatchId""
                        left join cms.""Organization"" as org on org.""Id"" = bt.""OrganizationId""
                        left join public.""User"" as ou on  ou.""Id"" = task.""OwnerUserId""
                        left join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                        left join public.""RecTaskTemplate"" as tt on tt.""TemplateCode""=task.""TemplateCode""  
                        left join cms.""Job"" as hjob on  hjob.""Id"" = task.""DropdownValue1""
                        left join cms.""Organization"" as horg on horg.""Id"" = task.""DropdownValue2""
                        where (task.""TemplateCode"" ='{tempCode}') and (task.""NtsType"" is null or task.""NtsType""=1)";
            var result = await _queryRepo.ExecuteQueryList(query, null);
            foreach (var i in result)
            {
                i.DisplayDueDate = i.DueDate?.ToString("dd/MM/yyyy HH:mm:ss");
            }
            return result;
        }
        public async Task<TaskEmailSummaryViewModel> GetTaskSummaryCountByUserId(string userId)
        {
            string query = @$"SELECT sum(case when task.""TaskStatusCode""='COMPLETED' then 1 else 0 end) as CompletedCount,
                            sum(case when task.""TaskStatusCode"" = 'OVERDUE' then 1 else 0 end) as OverdueCount,
		                    sum(case when task.""TaskStatusCode"" = 'INPROGRESS' then 1 else 0 end) as InprogressCount,
		                    sum(case when task.""TaskStatusCode"" = 'DRAFT' then 1 else 0 end) as DraftCount
                            FROM public.""RecTask"" as task
                            where (task.""AssigneeUserId"" ='{userId}') and (task.""NtsType"" is null or task.""NtsType""=1)";
            var result = await _queryRepo.ExecuteQuerySingle<TaskEmailSummaryViewModel>(query, null);
            return result;
        }
        public async Task<List<RecTaskViewModel>> GetTasksTemplateCodeByUserId(string userId)
        {
            string query = @$"SELECT distinct task.""TemplateCode"" as TemplateCode
                            FROM public.""RecTask"" as task
                            where (task.""AssigneeUserId"" ='{userId}') 
                            and (task.""NtsType"" is null or task.""NtsType""=1)
                            and (task.""TaskStatusCode"" = 'OVERDUE' or  task.""TaskStatusCode"" = 'INPROGRESS')";
            var result = await _queryRepo.ExecuteQueryList<RecTaskViewModel>(query, null);
            return result;
        }
        public async Task<string> GetTaskIdsByUserId(string userId, string templateCode, string status, string batch)
        {
            var list = new List<RecTaskViewModel>();
            string query = "";
            if (status.IsNotNullAndNotEmpty())
            {
                status = status.Replace(",", "','");
            }
            if (batch.IsNotNullAndNotEmpty())
            {
                query = @$"SELECT string_agg(task.""Id""::text, ',') as Id
                        FROM public.""RecTask"" as task
                        left join public.""RecTask"" as service on  service.""Id"" = task.""ReferenceTypeId"" 
                        left join rec.""Application"" as app on app.""Id"" = service.""ReferenceTypeId""
                        where task.""AssigneeUserId"" ='{userId}' and task.""TemplateCode""='{templateCode}' and task.""TaskStatusCode"" in ('{status}') and app.""BatchId""='{batch}' and (task.""NtsType"" is null or task.""NtsType""=1)";

            }
            else
            {
                query = @$"SELECT string_agg(""Id""::text, ',') as Id
                        FROM public.""RecTask""                        
                        where ""AssigneeUserId"" ='{userId}' and ""TemplateCode""='{templateCode}' and ""TaskStatusCode"" in ('{status}') and (""NtsType"" is null or ""NtsType""=1)";
            }
            var result = await _queryRepo.ExecuteQuerySingle(query, null);
            return result.Id;
        }
        public async Task<IList<RecTaskViewModel>> GetActiveServiceListByUserId(string userId)
        {
            var list = new List<RecTaskViewModel>();
            string query = @$"SELECT task.*, ou.""Name"" as OwnerUserName, au.""Name"" as AssigneeUserName , substring( ou.""Name"" for 1) as TaskOwnerFirstLetter
                        FROM public.""RecTask"" as task
                        left join public.""User"" as ou on  ou.""Id"" = task.""OwnerUserId""
                        left join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                        where (""AssigneeUserId"" ='{userId}' or ""OwnerUserId"" = '{userId}' or ""RequestedUserId"" = '{userId}') and ""NtsType""=2";
            var result = await _queryRepo.ExecuteQueryList(query, null);
            foreach (var i in result)
            {
                i.DisplayDueDate = i.DueDate?.ToString("dd/MM/yyyy HH:mm:ss");
            }
            return result;
        }
        public async Task<IList<RecTaskViewModel>> GetActiveStepTaskListByService(string serviceId, string versionNo = "")
        {
            var list = new List<RecTaskViewModel>();
            string query = "";
            if (versionNo.IsNotNullAndNotEmpty())
            {
                query = @$"SELECT task.*, ou.""Name"" as OwnerUserName, au.""Name"" as AssigneeUserName , substring( au.""Name"" for 1) as TaskOwnerFirstLetter
                        FROM public.""RecTask"" as s
                        left join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id""  and task.""ParentVersionNo""='{versionNo}'
                        left join public.""User"" as ou on  ou.""Id"" = task.""OwnerUserId""
                        left join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                        where s.""Id"" ='{serviceId}' and s.""NtsType"" = 2 order by task.""CreatedDate"" asc ";
            }
            else
            {
                query = @$"SELECT task.*, ou.""Name"" as OwnerUserName, au.""Name"" as AssigneeUserName , substring( au.""Name"" for 1) as TaskOwnerFirstLetter
                        FROM public.""RecTask"" as s
                        left join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id""  and task.""ParentVersionNo""=CAST (s.""VersionNo"" AS TEXT)
                        left join public.""User"" as ou on  ou.""Id"" = task.""OwnerUserId""
                        left join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                        where s.""Id"" ='{serviceId}' and s.""NtsType"" = 2 order by task.""CreatedDate"" asc ";
            }

            var result = await _queryRepo.ExecuteQueryList(query, null);
            return result;
        }
        public async Task<IList<RecTaskViewModel>> GetStepTaskListByService(string serviceId)
        {
            var list = new List<RecTaskViewModel>();
            string query = @$"SELECT task.*, ou.""Name"" as OwnerUserName, au.""Name"" as AssigneeUserName , substring( ou.""Name"" for 1) as TaskOwnerFirstLetter
                        FROM public.""RecTask"" as s
                        left join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id"" 
                        left join public.""User"" as ou on  ou.""Id"" = task.""OwnerUserId""
                        left join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                        where s.""Id"" ='{serviceId}' and s.""NtsType"" = 2  and task.""TaskStatusCode""<>'CANCELLED' order by task.""CreatedDate"" asc ";
            var result = await _queryRepo.ExecuteQueryList(query, null);
            return result;
        }
        public async Task<string> GetServiceId(string steptaskId)
        {

            string query = @$"SELECT s.""Id""
                        FROM public.""RecTask"" as s
                        left join public.""RecTask"" as t on  t.""ReferenceTypeId"" = s.""Id""                        
                        where t.""Id"" ='{steptaskId}' and s.""NtsType""=2 ";
            var result = await _queryRepo.ExecuteScalar<string>(query, null);
            return result;
        }
        public async Task<IList<RecTaskViewModel>> GetStepTaskId(string serviceId)
        {

            string query = @$"SELECT t.*
                        FROM public.""RecTask"" as s
                        left join public.""RecTask"" as t on  t.""ReferenceTypeId"" = s.""Id""  and t.""ParentVersionNo""=CAST (s.""VersionNo"" AS TEXT)                      
                        where s.""Id"" ='{serviceId}' and s.""NtsType""=2 order by t.""CreatedDate"" asc ";
            var result = await _queryRepo.ExecuteQueryList(query, null);
            return result;
        }
        public async Task<string> UpdateOverdueTaskAndServiceStatus(DateTime currentDate)
        {

            string query = @$"Update public.""RecTask""  set  ""TaskStatusCode""='OVERDUE', ""TaskStatusName""='Overdue'                                            
                        where ""DueDate"" < CURRENT_TIMESTAMP and ""TaskStatusCode""='INPROGRESS' ";
            var result = await _queryRepo.ExecuteScalar<string>(query, null);
            return result;
        }
        public async Task<string> UpdateTaskBatchId(string taskid, string batchId)
        {

            string query = @$"Update public.""RecTask""  set  ""TextValue5""='{batchId}'                                         
                        where ""Id""= '{taskid}'";
            var result = await _queryRepo.ExecuteScalar<string>(query, null);
            return result;
        }
        public async Task<string> UpdateTaskCandidateId(string taskid, string candidateId)
        {

            string query = @$"Update public.""RecTask""  set  ""TextValue6""='{candidateId}'                                         
                        where ""Id""= '{taskid}'";
            var result = await _queryRepo.ExecuteScalar<string>(query, null);
            return result;
        }
        public async Task<string> UpdateServiceReference(string serviceId, string appId)
        {

            string query = @$"Update public.""RecTask""  set  ""ReferenceTypeId""='{appId}',""ReferenceTypeCode""=32                                        
                        where ""Id""= '{serviceId}'";
            var result = await _queryRepo.ExecuteScalar<string>(query, null);
            return result;
        }
        public async Task<IList<IdNameViewModel>> GetVersionList(string id, long? versionId = null)
        {

            string query = @$"SELECT Cast(t.""VersionNo"" as TEXT) as Id,Cast(t.""VersionNo"" as INT) as EnumId FROM public.""RecTask"" as t
                         where t.""Id"" = '{id}' order by t.""VersionNo"" desc";
            var data1 = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            var maxVersion = (data1.IsNotNull() && data1.Count != 0) ? data1[0].EnumId : 0;
            var data = new List<IdNameViewModel>();
            if (maxVersion == 0)
            {
                data.Insert(0, new IdNameViewModel { Id = null, EnumId = 1 });
            }
            else
            {
                for (int i = 1; i <= maxVersion; i++)
                {
                    if (i == maxVersion)
                    {
                        data.Insert(0, new IdNameViewModel { Id = null, EnumId = i });
                    }
                    else
                    {
                        data.Insert(0, new IdNameViewModel { Id = i.ToString(), EnumId = i });
                    }

                }
            }
            if (versionId == 0 && maxVersion != 0)
            {
                data.Insert(0, new IdNameViewModel { Id = null, EnumId = maxVersion + 1 });
            }
            //if (maxVersion!=0 && versionId != 0)
            //{

            //}
            return data;
        }
        //public async Task<IList<IdNameViewModel>> GetVersionDetailsData(string id, long? versionId = null)
        //{

        //    string query = @$"SELECT t.* FROM public.""RecTaskVersion"" as t
        //                 where t.""ReferenceTypeId"" = '{id}' order by t.""VersionNo"" desc";
        //    var data = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
        //    var maxVersion = (data.IsNotNull() && data.Count != 0) ? data[0].EnumId : 0;
        //    if (maxVersion == 0)
        //    {
        //        data.Insert(0, new IdNameViewModel { Id = null, EnumId = 1 });
        //    }
        //    if ((versionId == 0 || versionId == null) && maxVersion != 0)
        //    {
        //        data.Insert(0, new IdNameViewModel { Id = null, EnumId = maxVersion + 1 });
        //    }
        //    if (maxVersion != 0 && versionId != 0)
        //    {

        //    }
        //    return data;
        //}

        public async Task<IList<System.Dynamic.ExpandoObject>> GetPendingTaskDetailsForUser(string userId, string orgId, string userRoleCodes)
        {
            var list = new List<System.Dynamic.ExpandoObject>();
            string query = "";
            var isHR = false;
            var userRoles = userRoleCodes.Split(",");
            //await _repo.GetListGlobal<UserRoleUser, UserRoleUser>(x => x.UserId == userId,
            //  x => x.UserRole);
            if (userRoles.IsNotNull())
            {
                var roles = userRoles.Select(x => x).Where(x => x == "HR");
                if (roles.IsNotNull() && roles.Count() > 0)
                {
                    query = @$"SELECT  count(task.""Id"") as Count, url.""Code"" as UserRole, j.""Name"" as Position, batch.""OrganizationId"" as OrgId FROM public.""RecTask"" as task
                        join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                        join public.""UserRoleUser"" as ur on ur.""UserId"" = task.""AssigneeUserId""
                        join public.""UserRole"" as url on url.""Id"" = ur.""UserRoleId""
                        join public.""RecTask"" as s on  s.""Id"" = task.""ReferenceTypeId""--service id
                        join rec.""Application"" as ap on  ap.""Id"" = s.""ReferenceTypeId""-- app id
                        Join rec.""Batch"" as batch on batch.""Id""=ap.""BatchId""
                        join cms.""Job"" as j on j.""Id"" = ap.""JobId""
                        where (task.""NtsType"" is null or task.""NtsType""=1)
                        and task.""TaskStatusCode"" in ('INPROGRESS','OVERDUE')
                        GROUP BY url.""Code"", j.""Id"", batch.""OrganizationId""";
                    isHR = true;
                }
                else
                {
                    query = @$"SELECT  count(task.""Id"") as Count, url.""Code"" as UserRole, j.""Name"" as Position, batch.""OrganizationId"" as OrgId FROM public.""RecTask"" as task
                join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                join public.""UserRoleUser"" as ur on ur.""UserId"" = task.""AssigneeUserId""
                join public.""UserRole"" as url on url.""Id"" = ur.""UserRoleId""
                join public.""RecTask"" as s on  s.""Id"" = task.""ReferenceTypeId""--service id
                join rec.""Application"" as ap on  ap.""Id"" = s.""ReferenceTypeId""-- app id
                Join rec.""Batch"" as batch on batch.""Id""=ap.""BatchId""
                join cms.""Job"" as j on j.""Id"" = ap.""JobId""
                where (task.""AssigneeUserId"" ='{userId}') and(task.""NtsType"" is null or task.""NtsType""=1)
                and task.""TaskStatusCode"" in ('INPROGRESS','OVERDUE')
                GROUP BY url.""Code"",j.""Id"", batch.""OrganizationId""";
                }
            }

            var result = await _queryRepo.ExecuteQueryList(query, null);
            if (result.IsNotNull())
            {

                if (orgId.IsNotNullAndNotEmpty())
                {
                    result = result.Where(x => x.OrgId == orgId).ToList();
                }

                var positions = result.Where(x => x.Position != "").Select(x => x.Position).Distinct().ToList();
                positions.Add("Count");
                var roleString = "";
                if (!isHR)
                {
                    roleString = result.Select(x => x.UserRole).FirstOrDefault();
                }
                else
                {
                    //var allRoles = await _userRoleBusiness.GetList();
                    //if (allRoles.IsNotNull())
                    //{
                    //    var r = allRoles.Select(x => x.Code);
                    //    roleString = String.Join(",", r);
                    //}

                    roleString = "ORG_UNIT,HRHEAD,ED,HR,HM,PRO,TICKETING,ADMIN,PLANT,CANDIDATE,AGENCY";
                }
                List<string> roles = new List<string>();
                if (roleString.IsNotNullAndNotEmpty())
                {
                    roles = roleString.Split(",").ToList();
                }

                foreach (var p in positions)
                {
                    var hrCount = 0;
                    var planningCount = 0;
                    var hMCount = 0;
                    var orgCount = 0;
                    var edCount = 0;
                    var candidateCount = 0;
                    var proCount = 0;
                    var tktCount = 0;
                    var adminCount = 0;
                    var plantCount = 0;
                    var agencyCount = 0;
                    var hrHeadCount = 0;
                    dynamic exo = new System.Dynamic.ExpandoObject();
                    var count = list.Cast<System.Dynamic.ExpandoObject>()
                   .Where(x => x.Any(y => y.Value != null && y.Value.ToString().IndexOf(p) >= 0))
                   .Count();
                    if (count == 0)
                    {
                        ((IDictionary<String, Object>)exo).Add("Position", p);
                    }
                    var tasks = new List<RecTaskViewModel>();
                    if (p != "Count")
                    {
                        tasks = result.Where(x => x.Position == p).ToList();
                    }
                    else
                    {
                        tasks = result.Select(x => new RecTaskViewModel { UserRole = x.UserRole, Count = x.Count }).ToList();

                        tasks = tasks.GroupBy(x => x.UserRole).Select(x => new RecTaskViewModel { UserRole = x.Key, Count = x.Sum(y => y.Count) }).ToList();
                    }
                    foreach (var r in roles)
                    {
                        var tr = tasks.Where(x => x.UserRole == r).FirstOrDefault();

                        if (tr != null)
                        {
                            AddPropertyinExpndoObject(exo, r, tr.Count);
                        }
                        else
                        {
                            AddPropertyinExpndoObject(exo, r, 0);
                        }

                        var expandoDict = exo as IDictionary<string, object>;
                        if (expandoDict.ContainsKey("Position"))
                        {

                            list.Add(exo);
                        }
                        //                        foreach (var r in roles)
                        //                        {
                        //                            var ro = r.Replace(" ", String.Empty);
                        //                            switch (ro)
                        //                            {
                        //                                case "HR":
                        //                                    if (t.TemplateCode is "SCHEDULE_INTERVIEW_RECRUITER" or
                        //                               "EMPLOYEE_APPOINTMENT_APPROVAL1" or
                        //                               "FINAL_OFFER_APPROVAL_RECRUITER" or
                        //                               "CHECK_MEDICAL_REPORT_INFORM_PRO" or
                        //                               "ONFIRM_TRAVELLING_DATE" or
                        //                               "VERIFY_VISA_TRANSFER_COMPLETED" or
                        //                               "VERIFY_WORK_PERMIT_DOCUMENTS" or
                        //                               "VERIFY_WORK_PERMIT_OBTAINED" or
                        //                               "FILL_STAFF_DETAILS" or
                        //                               "PROVIDE_CASH_VOUCHER" or
                        //                               "SEND_INTIMATION_EMAIL_ORG_UNIT_HOD_COPYING_IT" or
                        //                               "SEND_FRA_HRA_REQUEST_FINANCE" or
                        //                               "UPLOAD_PASSPORT_VISA_QATARID" or
                        //                               "UPDATE_EMPLOYEE_FILE_IN_SAP" or
                        //                               "CONFIRM_INDUCTION_DATE_TO_CANDIDATE" or
                        //                               "VERIFY_DOCUMENTS")
                        //                                    {
                        //                                        hrCount++;
                        //                                    }
                        //                                    break;
                        //                                case "PLANNING":
                        //                                    if (t.TemplateCode is "EMPLOYEE_APPOINTMENT_APPROVAL3" or
                        //                   "WORKER_POOL_PLANNING")
                        //                                    {
                        //                                        planningCount++;
                        //                                    }
                        //                                    break;
                        //                                case "CORPCOMP":
                        //                                    break;
                        //                                case "HRHEAD":
                        //                                    if (t.TemplateCode is "FINAL_OFFER_APPROVAL_HR_HEAD" or
                        //                   "WORKER_POOL_HR_HEAD")
                        //                                    {
                        //                                        hrHeadCount++;
                        //                                    }
                        //                                    break;
                        //                                case "ORG_UNIT":
                        //                                    if (t.TemplateCode is "EMPLOYEE_APPOINTMENT_APPROVAL3" or
                        //"WORKER_POOL_PLANNING" or "WORKER_POOL_HOD")
                        //                                    {
                        //                                        orgCount++;
                        //                                    }
                        //                                    break;
                        //                                case "HM":
                        //                                    if (t.TemplateCode is "INTERVIEW_EVALUATION_HM" or "SCHEDULE_INTERVIEW_CANDIDATE")
                        //                                    {
                        //                                        hMCount++;
                        //                                    }
                        //                                    break;
                        //                                case "ED":
                        //                                    if (t.TemplateCode is "EMPLOYEE_APPOINTMENT_APPROVAL4" or "WORKER_POOL_ED")
                        //                                    {
                        //                                        edCount++;
                        //                                    }
                        //                                    break;
                        //                                case "CANDIDATE":
                        //                                    if (t.TemplateCode is "SCHEDULE_INTERVIEW_CANDIDATE" or "FINAL_OFFER_APPROVAL_CANDIDATE"
                        //or "VISA_CONFIRMATION_AND_BOOKING"
                        //or "VISA_TO_CANDIDATE" or "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE" or "CONFIRM_RECEIPT_OF_TICKET" or "SUBMIT_VISA_TRANSFER_DOCUMENTS"
                        //or "RECEIVE_VISA_TRANSFER_INFORM_JOINING_DATE" or "SUBMIT_WORK_PERMIT_DOCUMENTS" or "RECEIVE_WORK_PERMIT_INFORM_JOINING_DATE")
                        //                                    {
                        //                                        candidateCount++;
                        //                                    }
                        //                                    break;
                        //                                case "AGENCY":
                        //                                    if (t.TemplateCode is "CONDUCT_MEDICAL_FINGER_PRINT")
                        //                                    {
                        //                                        agencyCount++;
                        //                                    }
                        //                                    break;
                        //                                case "PRO":
                        //                                    if (t.TemplateCode is "OBTAIN_BUSINESS_VISA" or "BOOK_QVC_APPOINTMENT"
                        //or "FIT_UNFIT_ATTACH_VISA_COPY" or
                        //"SUBMIT_VISA_TRANSFER_CONFIRM_SPONSORSHIP" or "OBTAIN_WORK_PERMIT_ATTACH")
                        //                                    {
                        //                                        proCount++;
                        //                                    }
                        //                                    break;
                        //                                case "TICKETING":
                        //                                    if (t.TemplateCode is "BOOK_TICKET_ATTACH")
                        //                                    {
                        //                                        tktCount++;
                        //                                    }
                        //                                    break;
                        //                                case "ADMIN":
                        //                                    if (t.TemplateCode is "ARRANGE_ACCOMMODATION")
                        //                                    {
                        //                                        adminCount++;
                        //                                    }
                        //                                    break;
                        //                                case "PLANT":
                        //                                    if (t.TemplateCode is "ARRANGE_VEHICLE_PICKUP")
                        //                                    {
                        //                                        plantCount++;
                        //                                    }
                        //                                    break;
                        //                            }
                        //                        }

                        //                        foreach (string r in roles)
                        //                        {
                        //                            var countOfRole = 0;
                        //                            var re = r.Replace(" ", string.Empty);
                        //                            switch (re)
                        //                            {
                        //                                case "HR":
                        //                                    {
                        //                                        if (t.TemplateCode is "SCHEDULE_INTERVIEW_RECRUITER" or
                        //                                            "EMPLOYEE_APPOINTMENT_APPROVAL1" or
                        //                                            "FINAL_OFFER_APPROVAL_RECRUITER" or
                        //                                            "CHECK_MEDICAL_REPORT_INFORM_PRO" or
                        //                                            "ONFIRM_TRAVELLING_DATE" or
                        //                                            "VERIFY_VISA_TRANSFER_COMPLETED" or
                        //                                            "VERIFY_WORK_PERMIT_DOCUMENTS" or
                        //                                            "VERIFY_WORK_PERMIT_OBTAINED" or
                        //                                            "FILL_STAFF_DETAILS" or
                        //                                            "PROVIDE_CASH_VOUCHER" or
                        //                                            "SEND_INTIMATION_EMAIL_ORG_UNIT_HOD_COPYING_IT" or
                        //                                            "SEND_FRA_HRA_REQUEST_FINANCE" or
                        //                                            "UPLOAD_PASSPORT_VISA_QATARID" or
                        //                                            "UPDATE_EMPLOYEE_FILE_IN_SAP" or
                        //                                            "CONFIRM_INDUCTION_DATE_TO_CANDIDATE" or
                        //                                            "VERIFY_DOCUMENTS")
                        //                                        {

                        //                                            var query1 = @$"select distinct (ap.""BatchId"") from public.""RecTask"" as task
                        //                            join public.""RecTask"" as s on  s.""Id"" = task.""ReferenceTypeId""
                        //                            join rec.""Application"" as ap on ap.""Id"" = s.""ReferenceTypeId""
                        //                             join cms.""Job"" as j on j.""Id"" = ap.""JobId""
                        //                             where j.""Name"" = '{p}' and (task.""AssigneeUserId"" ='{userId}')";

                        //                                            var res = await _queryRepo.ExecuteQueryList(query1, null);

                        //                                            var query2 = @$"select distinct (ap.""WorkerBatchId"") from public.""RecTask"" as task
                        //                            join public.""RecTask"" as s on  s.""Id"" = task.""ReferenceTypeId""
                        //                            join rec.""Application"" as ap on ap.""Id"" = s.""ReferenceTypeId""
                        //                             join cms.""Job"" as j on j.""Id"" = ap.""JobId""
                        //                             where j.""Name"" = '{p}' and (task.""AssigneeUserId"" ='{userId}')";

                        //                                            var res2 = await _queryRepo.ExecuteQueryList(query2, null);

                        //                                            countOfRole = hrCount + res.Count + res2.Count;
                        //                                            if (p == "Count")
                        //                                            {
                        //                                                countOfRole = 0;
                        //                                            }
                        //                                        }

                        //                                        break;
                        //                                    }
                        //                                case "HRHEAD":
                        //                                    if (t.TemplateCode is "FINAL_OFFER_APPROVAL_HR_HEAD" or
                        //                   "WORKER_POOL_HR_HEAD")
                        //                                    {
                        //                                        countOfRole = hrHeadCount;
                        //                                        if (p == "Count")
                        //                                        {
                        //                                            countOfRole = 0;
                        //                                        }
                        //                                    }
                        //                                    break;
                        //                                case "PLANNING":
                        //                                    if (t.TemplateCode is "EMPLOYEE_APPOINTMENT_APPROVAL3" or
                        //                                                     "WORKER_POOL_PLANNING")
                        //                                    {
                        //                                        countOfRole = planningCount;

                        //                                        if (p == "Count")
                        //                                        {
                        //                                            countOfRole = 0;
                        //                                        }
                        //                                    }
                        //                                    break;
                        //                                case "CORPCOMP":
                        //                                    break;
                        //                                case "ORG_UNIT":
                        //                                    if (t.TemplateCode is "EMPLOYEE_APPOINTMENT_APPROVAL3" or
                        //"WORKER_POOL_PLANNING" or "WORKER_POOL_HOD")
                        //                                    {
                        //                                        countOfRole = orgCount;

                        //                                        if (p == "Count")
                        //                                        {
                        //                                            countOfRole = 0;
                        //                                        }
                        //                                    }
                        //                                    break;
                        //                                case "HM":
                        //                                    if (t.TemplateCode is "INTERVIEW_EVALUATION_HM" or "SCHEDULE_INTERVIEW_CANDIDATE")
                        //                                    {
                        //                                        var query3 = $@"select  (ap.""BatchId"") as Id from public.""RecTask"" as task
                        //                            join public.""RecTask"" as s on  s.""Id"" = task.""ReferenceTypeId""
                        //                            join rec.""Application"" as ap on ap.""Id"" = s.""ReferenceTypeId""
                        //                             join cms.""Job"" as j on j.""Id"" = ap.""JobId""
                        //                             join rec.""Batch"" as b on b.""Id"" = ap.""BatchId""
                        //                             JOIN rec.""ListOfValue"" as lov ON lov.""Id"" = b.""BatchStatus"" AND lov.""Code"" = 'PendingwithHM'
                        //                             where j.""Name"" = '{p.Replace("\n", string.Empty)}' and (b.""HiringManager"" ='{userId}')
                        //                             Group By ap.""BatchId"" ";
                        //                                        var res3 = await _queryRepo.ExecuteQueryList(query3, null);
                        //                                        countOfRole = hMCount + res3.Where(x => x.Id != null).Count();

                        //                                        if (p == "Count")
                        //                                        {
                        //                                            countOfRole = 0;
                        //                                        }
                        //                                    }

                        //                                    break;

                        //                                case "ED":
                        //                                    if (t.TemplateCode is "EMPLOYEE_APPOINTMENT_APPROVAL4" or "WORKER_POOL_ED")
                        //                                    {
                        //                                        countOfRole = edCount;
                        //                                        if (p == "Count")
                        //                                        {
                        //                                            countOfRole = 0;
                        //                                        }
                        //                                    }
                        //                                    break;
                        //                                case "CANDIDATE":
                        //                                    if (t.TemplateCode is "SCHEDULE_INTERVIEW_CANDIDATE" or "FINAL_OFFER_APPROVAL_CANDIDATE"
                        //or "VISA_CONFIRMATION_AND_BOOKING"
                        //or "VISA_TO_CANDIDATE" or "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE" or "CONFIRM_RECEIPT_OF_TICKET" or "SUBMIT_VISA_TRANSFER_DOCUMENTS"
                        //or "RECEIVE_VISA_TRANSFER_INFORM_JOINING_DATE" or "SUBMIT_WORK_PERMIT_DOCUMENTS" or "RECEIVE_WORK_PERMIT_INFORM_JOINING_DATE")
                        //                                        countOfRole = candidateCount;
                        //                                    if (p == "Count")
                        //                                    {
                        //                                        countOfRole = 0;
                        //                                    }
                        //                                    break;
                        //                                case "AGENCY":
                        //                                    if (t.TemplateCode is "CONDUCT_MEDICAL_FINGER_PRINT")
                        //                                    {
                        //                                        countOfRole = agencyCount;
                        //                                        if (p == "Count")
                        //                                        {
                        //                                            countOfRole = 0;
                        //                                        }
                        //                                    }
                        //                                    break;
                        //                                case "PRO":
                        //                                    if (t.TemplateCode is "OBTAIN_BUSINESS_VISA" or "BOOK_QVC_APPOINTMENT" or "FIT_UNFIT_ATTACH_VISA_COPY" or "SUBMIT_VISA_TRANSFER_CONFIRM_SPONSORSHIP" or "OBTAIN_WORK_PERMIT_ATTACH")
                        //                                    {
                        //                                        countOfRole = proCount;
                        //                                        if (p == "Count")
                        //                                        {
                        //                                            countOfRole = 0;
                        //                                        }
                        //                                    }
                        //                                    break;
                        //                                case "TICKETING":
                        //                                    if (t.TemplateCode is "BOOK_TICKET_ATTACH")
                        //                                    {
                        //                                        countOfRole = tktCount;
                        //                                        if (p == "Count")
                        //                                        {
                        //                                            countOfRole = 0;
                        //                                        }
                        //                                    }
                        //                                    break;
                        //                                case "ADMIN":
                        //                                    if (t.TemplateCode is "ARRANGE_ACCOMMODATION")
                        //                                    {
                        //                                        countOfRole = adminCount;
                        //                                        if (p == "Count")
                        //                                        {
                        //                                            countOfRole = 0;
                        //                                        }
                        //                                    }
                        //                                    break;
                        //                                case "PLANT":
                        //                                    if (t.TemplateCode is "ARRANGE_VEHICLE_PICKUP")
                        //                                    {
                        //                                        countOfRole = plantCount;
                        //                                        if (p == "Count")
                        //                                        {
                        //                                            countOfRole = 0;
                        //                                        }
                        //                                    }
                        //                                    break;
                        //                            }

                        //                            AddPropertyinExpndoObject(exo, re, countOfRole);
                        //                        }


                    }

                }
                var resList = list.Distinct().ToList();

                //foreach (string re in roles)
                //{

                //    var sum = resList.Sum(item => (int)((IDictionary<string, object>)item)[re]);
                //    if (sum == 0)
                //    {
                //        foreach (var x in resList)
                //        {
                //            ((IDictionary<String, Object>)x).Remove(re);
                //        }

                //    }
                //    else
                //    {
                //        ((IDictionary<String, Object>)resList[^1])[re] = sum;
                //    }
                //}
                return resList;
            }
            return list;
        }


        public static void AddPropertyinExpndoObject(System.Dynamic.ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }

        public async Task<double> GetPendingTaskCount(string userId, string userRoleCodes)
        {
            var countOfRole = 0;
            var list = new List<System.Dynamic.ExpandoObject>();
            string query = "";
            var isHR = false;
            var userRoles = userRoleCodes.Split(","); //await _repo.GetListGlobal<UserRoleUser, UserRoleUser>(x => x.UserId == userId,
                                                      //  x => x.UserRole);
            if (userRoles.IsNotNull())
            {
                var roles = userRoles.Select(x => x).Where(x => x == "HR");
                if (roles.IsNotNull() && roles.Count() > 0)
                {
                    query = @$"SELECT  task.*, string_agg(url.""Code"", ', ') as UserRole, j.""Name"" as Position, batch.""OrganizationId"" as OrgId FROM public.""RecTask"" as task
                        join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                        join public.""UserRoleUser"" as ur on ur.""UserId"" = task.""AssigneeUserId""
                        join public.""UserRole"" as url on url.""Id"" = ur.""UserRoleId""
                        join public.""RecTask"" as s on  s.""Id"" = task.""ReferenceTypeId""--service id
                        join rec.""Application"" as ap on  ap.""Id"" = s.""ReferenceTypeId""-- app id
                        Join rec.""Batch"" as batch on batch.""Id""=ap.""BatchId""
                        join cms.""Job"" as j on j.""Id"" = ap.""JobId""
                        where (task.""NtsType"" is null or task.""NtsType""=1)
                        and task.""TaskStatusCode"" = 'INPROGRESS'
                        GROUP BY task.""Id"", ap.""Id"", j.""Id"", batch.""OrganizationId""";
                    isHR = true;
                }
                else
                {
                    query = @$"SELECT  task.*, string_agg(url.""Code"", ', ') as UserRole, j.""Name"" as Position, batch.""OrganizationId"" as OrgId FROM public.""RecTask"" as task
                join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                join public.""UserRoleUser"" as ur on ur.""UserId"" = task.""AssigneeUserId""
                join public.""UserRole"" as url on url.""Id"" = ur.""UserRoleId""
                join public.""RecTask"" as s on  s.""Id"" = task.""ReferenceTypeId""--service id
                join rec.""Application"" as ap on  ap.""Id"" = s.""ReferenceTypeId""-- app id
                Join rec.""Batch"" as batch on batch.""Id""=ap.""BatchId""
                join cms.""Job"" as j on j.""Id"" = ap.""JobId""
                where (task.""AssigneeUserId"" ='{userId}') and(task.""NtsType"" is null or task.""NtsType""=1)
                and task.""TaskStatusCode"" = 'INPROGRESS'
                GROUP BY task.""Id"", ap.""Id"", j.""Id"", batch.""OrganizationId""";
                }
            }

            var result = await _queryRepo.ExecuteQueryList(query, null);
            if (result.IsNotNull())
            {


                var positions = result.Where(x => x.Position != "").Select(x => x.Position).Distinct().ToList();
                positions.Add("Count");
                var roleString = "";
                if (!isHR)
                {
                    roleString = result.Select(x => x.UserRole).FirstOrDefault();
                }
                else
                {
                    //var allRoles = await _userRoleBusiness.GetList();
                    //if (allRoles.IsNotNull())
                    //{
                    //    var r = allRoles.Select(x => x.Code);
                    //    roleString = String.Join(",", r);
                    //}
                    roleString = "ORG_UNIT,HRHEAD,PLANNING,ED,HR,HM,PRO,TICKETING,ADMIN,PLANT,CANDIDATE,AGENCY";

                }
                List<string> roles = new List<string>();
                if (roleString.IsNotNullAndNotEmpty())
                {
                    roles = roleString.Split(",").ToList();
                }

                foreach (var p in positions)
                {
                    dynamic exo = new System.Dynamic.ExpandoObject();
                    var count = list.Cast<System.Dynamic.ExpandoObject>()
                   .Where(x => x.Any(y => y.Value != null && y.Value.ToString().IndexOf(p) >= 0))
                   .Count();
                    if (count == 0)
                    {
                        ((IDictionary<String, Object>)exo).Add("Position", p);
                    }
                    var task = result.Where(x => x.Position == p).ToList();
                    foreach (var t in task)
                    {

                        foreach (string r in roles)
                        {
                            var re = r.Replace(" ", string.Empty);
                            switch (re)
                            {
                                case "HR":
                                    {
                                        if (t.TemplateCode is "SCHEDULE_INTERVIEW_RECRUITER" or
                                            "EMPLOYEE_APPOINTMENT_APPROVAL1" or
                                            "FINAL_OFFER_APPROVAL_RECRUITER" or
                                            "CHECK_MEDICAL_REPORT_INFORM_PRO" or
                                            "ONFIRM_TRAVELLING_DATE" or
                                            "VERIFY_VISA_TRANSFER_COMPLETED" or
                                            "VERIFY_WORK_PERMIT_DOCUMENTS" or
                                            "VERIFY_WORK_PERMIT_OBTAINED" or
                                            "FILL_STAFF_DETAILS" or
                                            "PROVIDE_CASH_VOUCHER" or
                                            "SEND_INTIMATION_EMAIL_ORG_UNIT_HOD_COPYING_IT" or
                                            "SEND_FRA_HRA_REQUEST_FINANCE" or
                                            "UPLOAD_PASSPORT_VISA_QATARID" or
                                            "UPDATE_EMPLOYEE_FILE_IN_SAP" or
                                            "CONFIRM_INDUCTION_DATE_TO_CANDIDATE" or
                                            "VERIFY_DOCUMENTS")
                                        {

                                            countOfRole++;

                                        }

                                        break;
                                    }
                                case "HRHEAD":
                                    if (t.TemplateCode is "FINAL_OFFER_APPROVAL_HR_HEAD" or
                   "WORKER_POOL_HR_HEAD")
                                    {
                                        countOfRole++;
                                    }
                                    break;
                                case "PLANNING":
                                    if (t.TemplateCode is "EMPLOYEE_APPOINTMENT_APPROVAL3" or
                                                     "WORKER_POOL_PLANNING")
                                    {
                                        countOfRole++;

                                    }
                                    break;
                                case "CORPCOMP":
                                    break;
                                case "ORG_UNIT":
                                    if (t.TemplateCode is "EMPLOYEE_APPOINTMENT_APPROVAL3" or
    "WORKER_POOL_PLANNING" or "WORKER_POOL_HOD")
                                    {
                                        countOfRole++;
                                    }
                                    break;
                                case "HM":
                                    {
                                        if (t.TemplateCode is "INTERVIEW_EVALUATION_HM" or "SCHEDULE_INTERVIEW_CANDIDATE")
                                        {
                                            countOfRole++;
                                        }

                                        break;
                                    }

                                case "ED":
                                    if (t.TemplateCode is "EMPLOYEE_APPOINTMENT_APPROVAL4" or "WORKER_POOL_ED")
                                    {
                                        countOfRole++;
                                    }
                                    break;
                                case "CANDIDATE":
                                    if (t.TemplateCode is "SCHEDULE_INTERVIEW_CANDIDATE" or "FINAL_OFFER_APPROVAL_CANDIDATE"
    or "VISA_CONFIRMATION_AND_BOOKING"
    or "VISA_TO_CANDIDATE" or "RECEIVE_VISA_COPY_ADVISE_TRAVELLING_DATE" or "CONFIRM_RECEIPT_OF_TICKET" or "SUBMIT_VISA_TRANSFER_DOCUMENTS"
    or "RECEIVE_VISA_TRANSFER_INFORM_JOINING_DATE" or "SUBMIT_WORK_PERMIT_DOCUMENTS" or "RECEIVE_WORK_PERMIT_INFORM_JOINING_DATE")
                                        countOfRole++;
                                    break;
                                case "AGENCY":
                                    if (t.TemplateCode is "CONDUCT_MEDICAL_FINGER_PRINT")
                                    {
                                        countOfRole++;
                                    }
                                    break;
                                case "PRO":
                                    if (t.TemplateCode is "OBTAIN_BUSINESS_VISA" or "BOOK_QVC_APPOINTMENT" or "FIT_UNFIT_ATTACH_VISA_COPY" or "SUBMIT_VISA_TRANSFER_CONFIRM_SPONSORSHIP" or "OBTAIN_WORK_PERMIT_ATTACH")
                                    {
                                        countOfRole++;
                                    }
                                    break;
                                case "TICKETING":
                                    if (t.TemplateCode is "BOOK_TICKET_ATTACH")
                                    {
                                        countOfRole++;
                                    }
                                    break;
                                case "ADMIN":
                                    if (t.TemplateCode is "ARRANGE_ACCOMMODATION")
                                    {
                                        countOfRole++;
                                    }
                                    break;
                                case "PLANT":
                                    if (t.TemplateCode is "ARRANGE_VEHICLE_PICKUP")
                                    {
                                        countOfRole++;
                                    }
                                    break;
                            }

                        }

                    }
                    var query1 = @$"select distinct (ap.""BatchId"") as Id from public.""RecTask"" as task
                            join public.""RecTask"" as s on  s.""Id"" = task.""ReferenceTypeId""
                            join rec.""Application"" as ap on ap.""Id"" = s.""ReferenceTypeId""
                             join cms.""Job"" as j on j.""Id"" = ap.""JobId""
                             where j.""Name"" = '{p}' and (task.""AssigneeUserId"" ='{userId}')";

                    var res = await _queryRepo.ExecuteQueryList(query1, null);
                    countOfRole += res.Where(x => x.Id != null).Count();
                    var query2 = @$"select distinct (ap.""WorkerBatchId"") as Id from public.""RecTask"" as task
                            join public.""RecTask"" as s on  s.""Id"" = task.""ReferenceTypeId""
                            join rec.""Application"" as ap on ap.""Id"" = s.""ReferenceTypeId""
                             join cms.""Job"" as j on j.""Id"" = ap.""JobId""
                             where j.""Name"" = '{p}' and (task.""AssigneeUserId"" ='{userId}')";

                    var res2 = await _queryRepo.ExecuteQueryList(query2, null);
                    countOfRole += res2.Where(x => x.Id != null).Count();

                    var query3 = $@"select  (ap.""BatchId"") as Id from public.""RecTask"" as task
                            join public.""RecTask"" as s on  s.""Id"" = task.""ReferenceTypeId""
                            join rec.""Application"" as ap on ap.""Id"" = s.""ReferenceTypeId""
                             join cms.""Job"" as j on j.""Id"" = ap.""JobId""
                             join rec.""Batch"" as b on b.""Id"" = ap.""BatchId""
                             JOIN rec.""ListOfValue"" as lov ON lov.""Id"" = b.""BatchStatus"" AND lov.""Code"" = 'PendingwithHM'
                             where j.""Name"" = '{p.Replace("\n", string.Empty)}' and (b.""HiringManager"" ='{userId}')
                             Group By ap.""BatchId"" ";

                    var res3 = await _queryRepo.ExecuteQueryList(query3, null);
                    countOfRole += res3.Where(x => x.Id != null).Count();
                }
            }
            return countOfRole;
        }
        public async Task<RecTaskViewModel> GetTemplateDetails(string templateCode)
        {
            string query = @$"SELECT  * FROM public.""RecTaskTemplate"" as tt
                            where tt.""TemplateCode""='{templateCode}'
                            ";
            var result = await _queryRepo.ExecuteQuerySingle<RecTaskViewModel>(query, null);
            return result;
        }

        //  public async Task<IList<TreeViewViewModel>> GetBulkApprovalMenuItem(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds)
        //  {
        //      var roles = userRoleIds.Split(",");
        //      var list = new List<TreeViewViewModel>();
        //      var query = "";
        //      if (id.IsNullOrEmpty())
        //      {
        //          list.Add(new TreeViewViewModel
        //          {
        //              id = "INBOX",
        //              Name = "Inbox",
        //              DisplayName = "Inbox",
        //              ParentId = null,
        //              hasChildren = true,
        //              expanded = true,
        //              Type = "INBOX"

        //          });

        //      }
        //      else if (id == "INBOX")
        //      {
        //          foreach (var r in roles)
        //          {
        //              var roleDetails = await _userRoleBusiness.GetSingleById(r);
        //              if (roleDetails.IsNotNull())
        //              {
        //                  list.Add(new TreeViewViewModel
        //                  {
        //                      id = roleDetails.Id,
        //                      Name = roleDetails.Name,
        //                      DisplayName = roleDetails.Name,
        //                      ParentId = "INBOX",
        //                      hasChildren = true,
        //                      expanded = false,
        //                      Type = "USERROLE",
        //                      UserRoleId = roleDetails.Id
        //                  });
        //              }
        //          }
        //      }
        //      else if (type == "USERROLE")
        //      {
        //          //query = $@"select  aps.""Code"" as Id,
        //          //aps.""Name"" || ' (' || Count(distinct task.""TemplateCode"") || ')' as DisplayName,
        //          //true as hasChildren,
        //          //'INBOX' as ParentId,'STAGE' as Type
        //          //FROM public.""RecTask"" as s
        //          //left join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id""
        //          //left join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
        //          //left join rec.""Application"" as app on app.""Id"" = s.""ReferenceTypeId""
        //          //left join rec.""ApplicationState"" as aps on aps.""Id"" = app.""ApplicationState""
        //          //left join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode""
        //          //Where task.""TaskStatusCode"" in('INPROGRESS','OVERDUE','COMPLETED','REJECTED') and app.""Id"" is not null 
        //          //and task.""AssigneeUserId"" = '{userId}'
        //          //GROUP BY aps.""Code"", aps.""Name"", aps.""SequenceOrder""
        //          //order by aps.""SequenceOrder"" asc ";

        //          query = $@"Select usp.""InboxStageName""  as Name, usp.""Id"" as id, 'INBOX' as ParentId, 'STAGE' as Type,
        //                      true as hasChildren, '{userRoleId}' as UserRoleId
        //                      from public.""UserRole"" as ur
        //                       join rec.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id""
        //                      where ur.""Id"" = '{userRoleId}'
        //                      Group By usp.""InboxStageName"", usp.""StageSequence"", usp.""Id""
        //                      order by CAST(coalesce(usp.""StageSequence"", '0') AS integer) asc
        //                  ";

        //          list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

        //      }
        //      else if (type == "STAGE")
        //      {
        //          //query = $@"select  tmp.""TemplateCode"" as id,
        //          //tmp.""Subject"" ||' (' || Count( distinct b.""Id"") || ')' as DisplayName,
        //          //true as hasChildren,
        //          //'{id}' as ParentId,'TEMPLATE' as Type
        //          //FROM public.""RecTask"" as s
        //          //join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id""
        //          //join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode""
        //          //join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
        //          //join rec.""Application"" as app on app.""Id"" = s.""ReferenceTypeId""
        //          //join rec.""Batch"" as b on b.""Id"" = app.""BatchId""
        //          //join rec.""ApplicationState"" as aps on aps.""Id"" = app.""ApplicationState""
        //          //Where aps.""Code""='{id}' and task.""TaskStatusCode"" in('INPROGRESS','OVERDUE','COMPLETED','REJECTED') 
        //          //and app.""Id"" is not null and task.""AssigneeUserId"" = '{userId}'
        //          //GROUP BY tmp.""TemplateCode"", tmp.""Subject"", tmp.""SequenceOrder""
        //          //order by tmp.""SequenceOrder"" asc ";
        //          query = $@"Select usp.""TemplateShortName""  as Name, usp.""TemplateCode"" as id, usp.""Id"" as ParentId, 'TEMPLATE' as Type,
        //                      true as hasChildren
        //                      from public.""UserRole"" as ur
        //                       join rec.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id""
        //                      where ur.""Id"" = '{userRoleId}' and usp.""Id"" = '{id}'
        //                      Group By usp.""TemplateShortName"", usp.""TemplateCode"", usp.""Id"", usp.""ChildSequence""
        //                      order by CAST(coalesce(usp.""ChildSequence"", '0') AS integer) asc";
        //          list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
        //          foreach (var i in list)
        //          {
        //              long cnt = 0;
        //              try
        //              {
        //                  var q = $@" Select Count(task.""Id"") as cc
        //                      from public.""UserRole"" as ur
        //                       join rec.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id""
        //                      join public.""RecTask"" as task on  task.""TemplateCode"" =usp.""TemplateCode""
        //                      where ur.""Id"" = '{userRoleId}' and task.""TaskStatusCode"" in ('INPROGRESS','OVERDUE')
        //                      and usp.""Id"" = '{i.ParentId}' and task.""TemplateCode"" = '{i.id}'
        //                      Group By usp.""TemplateShortName"", usp.""TemplateCode"", usp.""Id""";
        //                cnt  = await _queryRepo.ExecuteScalar<long>(q, null);
        //              } catch(Exception ex)
        //              {
        //                  cnt = 0;
        //              }

        //              i.Name = i.Name + " (" + cnt + ")";

        //          }

        //      }
        //      else if (type == "TEMPLATE")
        //      {
        //          //query = $@"select  b.""Id"" as id,
        //          //b.""Name"" ||' (' || Count( distinct task.""Id"") || ')' as DisplayName,
        //          //true as hasChildren,
        //          //'{id}' as ParentId,'BATCH' as Type
        //          //FROM public.""RecTask"" as s
        //          //join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id""
        //          //join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode""
        //          //join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
        //          //join rec.""Application"" as app on app.""Id"" = s.""ReferenceTypeId""
        //          //join rec.""Batch"" as b on b.""Id"" = app.""BatchId""
        //          //join rec.""ApplicationState"" as aps on aps.""Id"" = app.""ApplicationState""
        //          //Where aps.""Code""='{parentId}' and tmp.""TemplateCode""='{id}' and task.""TaskStatusCode"" in('INPROGRESS','OVERDUE','COMPLETED','REJECTED') 
        //          //and app.""Id"" is not null and task.""AssigneeUserId"" = '{userId}'
        //          //GROUP BY b.""Id"", b.""Name"", b.""SequenceOrder""
        //          //order by b.""SequenceOrder"" asc ";

        //          query = $@"select  b.""Id"" as id,
        //          b.""Name"" ||' (' || Count( distinct task.""Id"") || ')' as Name,
        //          true as hasChildren,
        //          '{id}' as ParentId,'BATCH' as Type
        //          FROM public.""RecTask"" as s
        //          join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id""
        //          join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode""
        //          join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
        //          join rec.""Application"" as app on app.""Id"" = s.""ReferenceTypeId""
        //          join rec.""Batch"" as b on b.""Id"" = app.""BatchId""
        //          join rec.""ApplicationState"" as aps on aps.""Id"" = app.""ApplicationState""
        //          Where tmp.""TemplateCode""='{id}' and task.""TaskStatusCode"" in('INPROGRESS','OVERDUE') 
        //          and app.""Id"" is not null and task.""AssigneeUserId"" = '{userId}'
        //          GROUP BY b.""Id"", b.""Name"", b.""SequenceOrder""
        //          order by b.""SequenceOrder"" asc ";
        //          list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

        //      }
        //      else if (type == "BATCH")
        //      {

        //          query = $@"select  task.""TaskStatusCode"" as id,count(task.""TaskStatusCode"") as RootId
        //          FROM public.""RecTask"" as s
        //          join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id""
        //          join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode""
        //          join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
        //          join rec.""Application"" as app on app.""Id"" = s.""ReferenceTypeId""
        //          join rec.""Batch"" as b on b.""Id"" = app.""BatchId""
        //          join rec.""ApplicationState"" as aps on aps.""Id"" = app.""ApplicationState""
        //          Where tmp.""TemplateCode""='{parentId}' and b.""Id""='{id}' 
        //          and task.""TaskStatusCode"" in('INPROGRESS','OVERDUE','COMPLETED','REJECTED') 
        //          and app.""Id"" is not null and task.""AssigneeUserId"" = '{userId}'
        //          GROUP BY task.""TaskStatusCode""";
        //          var listItems = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

        //          var pending = new TreeViewViewModel
        //          {
        //              id = "INPROGRESS",
        //              Type = "STATUS",
        //              ParentId = id,
        //              hasChildren = false,
        //              DisplayName = "Pending (0)",
        //              Name = "Pending (0)"
        //          };

        //          long pendingCount = 0;

        //          var pendingItem = listItems.FirstOrDefault(x => x.id == "INPROGRESS");
        //          if (pendingItem != null)
        //          {
        //              pendingCount += pendingItem.RootId ?? 0;
        //          }
        //          var overDueItem = listItems.FirstOrDefault(x => x.id == "OVERDUE");
        //          if (overDueItem != null)
        //          {
        //              pendingCount += overDueItem.RootId ?? 0;
        //          }
        //          pending.DisplayName = $"Pending ({pendingCount})";
        //          pending.Name = $"Pending ({pendingCount})";
        //          list.Add(pending);

        //          var completed = new TreeViewViewModel
        //          {
        //              id = "COMPLETED",
        //              Type = "STATUS",
        //              ParentId = id,
        //              hasChildren = false,
        //              DisplayName = "Completed (0)",
        //              Name = "Completed (0)"
        //          };
        //          var completedItem = listItems.FirstOrDefault(x => x.id == "COMPLETED");
        //          if (completedItem != null)
        //          {
        //              completed.DisplayName = $"Completed ({completedItem.RootId ?? 0})";
        //              completed.Name = $"Completed ({completedItem.RootId ?? 0})";
        //          }
        //          list.Add(completed);

        //          var rejected = new TreeViewViewModel
        //          {
        //              id = "REJECTED",
        //              Type = "STATUS",
        //              ParentId = id,
        //              hasChildren = false,
        //              DisplayName = "Rejected (0)",
        //              Name = "Rejected (0)"
        //          };
        //          var rejectedItem = listItems.FirstOrDefault(x => x.id == "REJECTED");
        //          if (rejectedItem != null)
        //          {
        //              rejected.DisplayName = $"Rejected ({rejectedItem.RootId ?? 0})";
        //              rejected.Name = $"Rejected ({rejectedItem.RootId ?? 0})";
        //          }
        //          list.Add(rejected);


        //      }
        //      //       else if (id == "OUTBOX")
        //      //       {

        //      //           var query2 = $@"select  app.""ApplicationState"" as id,
        //      //                           aps.""Name"" ||' (' || Count( distinct task.""TemplateCode"") || ')' as Name,
        //      //                           true as hasChildren,
        //      //                           'outbox' as ParentId
        //      //                           FROM public.""RecTask"" as s
        //      //                   left join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id""
        //      //                   left join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
        //      //	left join rec.""Application"" as app on app.""Id"" = s.""ReferenceTypeId""
        //      //	left join rec.""ApplicationState"" as aps on aps.""Id"" = app.""ApplicationState""
        //      //	left join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode""
        //      //	Where task.""TaskStatusCode"" = 'COMPLETED' and app.""Id"" is not null and  task.""AssigneeUserId"" = '{userId}'
        //      //                   GROUP BY app.""ApplicationState"", aps.""Name"", aps.""SequenceOrder""
        //      //                   order by aps.""SequenceOrder"" asc
        //      //";

        //      //           var outboxlist = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query2, null);
        //      //           if (outboxlist != null && outboxlist.Any())
        //      //           {
        //      //               list.AddRange(outboxlist);
        //      //           }
        //      //       }
        //      else
        //      {

        //          query = @$"select  app.""ApplicationState"" as ParentId,
        //                          aps.""Name"" as DisplayName,
        //                          tmp.""Subject"" ||' (' || Count( distinct b.""Id"") || ')' as Name,
        //                          tmp.""TemplateCode"" as id,
        //                          true as hasChildren
        //                          FROM public.""RecTask"" as s
        //                  left join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id""
        //                  left join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
        //left join rec.""Application"" as app on app.""Id"" = s.""ReferenceTypeId""
        //left join rec.""ApplicationState"" as aps on aps.""Id"" = app.""ApplicationState""
        //left join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode""
        //                  left join rec.""Batch"" as b on b.""Id"" = app.""BatchId""
        //Where task.""TaskStatusCode"" = 'INPROGRESS' and app.""Id"" is not null and  task.""AssigneeUserId"" = '{userId}'
        //                  Group by tmp.""Subject"" , app.""ApplicationState"", aps.""Name"", tmp.""Id"", aps.""SequenceOrder""
        //                  order by aps.""SequenceOrder"" asc";
        //          var result = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

        //          string query2 = @$"select  tmp.""TemplateCode"" as ParentId,
        //                  tmp.""Subject"" as DisplayName,
        //                  b.""Name"" as Name,
        //                  b.""Id"" as id,
        //                  true as hasChildren
        //                  FROM public.""RecTask"" as s
        //                  left join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id""
        //                  left join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
        //                  left join rec.""Application"" as app on app.""Id"" = s.""ReferenceTypeId""
        //                  left join rec.""ApplicationState"" as aps on aps.""Id"" = app.""ApplicationState""
        //                  left join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode""
        //                  left join rec.""Batch"" as b on b.""Id"" = app.""BatchId""
        //                  Where task.""TaskStatusCode"" = 'INPROGRESS' and app.""Id"" is not null and  task.""AssigneeUserId"" = '{userId}' --and task.""TemplateCode"" = '{id}'
        //                  Group by tmp.""Subject"" , tmp.""Id"", aps.""SequenceOrder"", b.""Name"", b.""Id""
        //                  order by aps.""SequenceOrder"" asc";
        //          var result2 = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query2, null);

        //          var defaultChild = new List<TreeViewViewModel>();

        //          foreach (var r in result2)
        //          {
        //              //var pendingTaskCount = await GetTaskCountAccordingToStatusCode("INPROGRESS", userId, id);
        //              //var completedTaskCount = await GetTaskCountAccordingToStatusCode("COMPLETED", userId, id);
        //              //var rejectedTaskCount = await GetTaskCountAccordingToStatusCode("CANCELLED", userId, id);

        //              defaultChild.Add(new TreeViewViewModel
        //              {
        //                  id = "Pending",
        //                  Name = "Pending" + " (" + 4 + ")",
        //                  ParentId = r.id
        //              }); defaultChild.Add(new TreeViewViewModel
        //              {
        //                  id = "Completed",
        //                  Name = "Completed" + " (" + 5 + ")",
        //                  ParentId = r.id
        //              }); defaultChild.Add(new TreeViewViewModel
        //              {
        //                  id = "Rejected",
        //                  Name = "Rejected" + " (" + 5 + ")",
        //                  ParentId = r.id
        //              });
        //          }
        //          result.AddRange(result2);
        //          result.AddRange(defaultChild);


        //          var menugroup = result.Where(x => x.ParentId == id).Distinct();
        //          if (menugroup != null)
        //          {
        //              list.AddRange(menugroup);
        //          }
        //      }
        //      return list;
        //  }

        public async Task<IList<TreeViewViewModel>> GetBulkApprovalMenuItem(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string batchId, string expandingList, string userroleCode)
        {
            var expObj = new List<TreeViewViewModel>();
            if (expandingList != null)
            {
                expObj = JsonConvert.DeserializeObject<List<TreeViewViewModel>>(expandingList);
                var obj = expObj.Where(x => x.id == id).FirstOrDefault();
                if (obj.IsNotNull())
                {
                    id = obj.id;
                    type = obj.Type;
                    parentId = obj.ParentId;
                    userRoleId = obj.UserRoleId;
                    stageName = obj.StageName;
                    stageId = obj.StageId;
                    batchId = obj.BatchId;
                }
            }

            var list = new List<TreeViewViewModel>();
            var query = "";
            if (id.IsNullOrEmpty())
            {
                var roles = userRoleIds.Split(",");
                var roleText = "";
                foreach (var i in roles)
                {
                    roleText += $"'{i}',";
                }
                roleText = roleText.Trim(',');
                query = $@" select  COALESCE(count(task.""Id""),0) +  COALESCE(hraps.""Count"",0)  +  COALESCE(hmaps.""Count"",0)  as ""Count""
	                FROM public.""UserRole"" as ur
                    join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='RECRUITMENT'
	                join public.""RecTask"" as task on task.""TemplateCode"" =usp.""TemplateCode""
                    join public.""RecTask"" as s on  task.""ReferenceTypeId"" = s.""Id""
	                join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode""
	                join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
	                join rec.""Application"" as app on app.""Id"" = s.""ReferenceTypeId""
	                join rec.""Batch"" as b on b.""Id"" = app.""BatchId""
                        JOIN rec.""ListOfValue"" as l ON l.""Id"" = b.""BatchStatus"" and l.""Code""='PendingWithHM'
	                join rec.""ApplicationState"" as aps on aps.""Id"" = app.""ApplicationState""
                    left join(
	                    select     'HR' as ""Code"",count(distinct app.""Id"") as ""Count""
                        FROM rec.""Batch"" as b
                        JOIN rec.""ListOfValue"" as l ON l.""Id"" = b.""BatchStatus"" --and l.""Code""='PendingWithHM'
                        join rec.""Application"" as app on app.""BatchId"" = b.""Id""
                        join rec.""ApplicationState"" as aps on aps.""Id"" = app.""ApplicationState""
                        join rec.""ApplicationStatus"" as apst on apst.""Id"" = app.""ApplicationStatus""
                        where l.""Code"" in('PendingWithHM', 'Draft', 'Close')
                    ) hraps on ur.""Code""=hraps.""Code""
                    left join(
	                 select 'HM' as ""Code"",count(distinct app.""Id"") as ""Count""
                    FROM rec.""Batch"" as b               
                    JOIN rec.""ListOfValue"" as l ON l.""Id"" = b.""BatchStatus"" --and l.""Code""='PendingWithHM'
                    join rec.""Application"" as app on app.""BatchId"" = b.""Id""
                    join rec.""ApplicationState"" as aps on aps.""Id"" = app.""ApplicationState""
                    join rec.""ApplicationStatus"" as apst on apst.""Id"" = app.""ApplicationStatus""
                    where b.""HiringManager"" = '{userId}' and l.""Code"" = 'PendingWithHM'
                    and apst.""Code"" in('NotShortlisted', 'ShortlistedHM', 'InterviewRequested')
                    ) hmaps on ur.""Code""=hmaps.""Code""
	                where ur.""Id"" in ({roleText}) and  task.""AssigneeUserId"" = '{userId}'  and task.""TaskStatusCode"" in ('INPROGRESS','OVERDUE')
                    and ur.""IsDeleted""=false and usp.""IsDeleted""=false and task.""IsDeleted""=false and s.""IsDeleted""=false and
                    tmp.""IsDeleted""=false and au.""IsDeleted""=false and app.""IsDeleted""=false and b.""IsDeleted""=false and aps.""IsDeleted""=false
                    group by hmaps.""Count"", hraps.""Count""";
                var count = await _queryRepo.ExecuteScalar<long?>(query, null);


                var jdQry = $@"select count(distinct task.""Id"") as ""Count""
                    from public.""RecTask"" as task
                    where (task.""TemplateCode""='JOBDESCRIPTION_HM' or task.""TemplateCode"" ='TASK_DIRECT_HIRING' or task.""TemplateCode"" ='DIRECTHIRING_EVALUATIONFORM') and task.""AssigneeUserId"" = '{userId}' and task.""TaskStatusCode"" in ('INPROGRESS','OVERDUE') and    task.""IsDeleted"" = false";


                var countjd = await _queryRepo.ExecuteScalar<long?>(jdQry, null);


                var item = new TreeViewViewModel
                {
                    id = "INBOX",
                    Name = "Inbox",
                    DisplayName = "Inbox",
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = "INBOX"
                };
                if (count != null)
                {
                    item.Name = item.DisplayName = $"Inbox ({count + countjd})";
                }
                list.Add(item);

            }
            else if (id == "INBOX")
            {
                var roles = userRoleIds.Split(",");
                var roleText = "";
                foreach (var item in roles)
                {
                    roleText += $"'{item}',";
                }
                roleText = roleText.Trim(',');
                query = $@"Select distinct ur.""Id"" as id,ur.""Name"" ||' (' || COALESCE(t.""Count"",0)+COALESCE(hraps.""Count"",0)+COALESCE(hmaps.""Count"",0)+COALESCE(jd.""Count"",0)|| ')' as Name
                , 'INBOX' as ParentId, 'USERROLE' as Type,
                true as hasChildren, ur.""Id"" as UserRoleId
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='RECRUITMENT'
                left join(
	                select ur.""Id"",count(task.""Id"")  as ""Count""
	                FROM public.""UserRole"" as ur
                    join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='RECRUITMENT'
	                join public.""RecTask"" as task on task.""TemplateCode"" =usp.""TemplateCode""
                    join public.""RecTask"" as s on  task.""ReferenceTypeId"" = s.""Id""
	                join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode""
	                join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
	                join rec.""Application"" as app on app.""Id"" = s.""ReferenceTypeId""
	                join rec.""Batch"" as b on b.""Id"" = app.""BatchId""
                    JOIN rec.""ListOfValue"" as l ON l.""Id"" = b.""BatchStatus"" and l.""Code""='PendingWithHM'
	                join rec.""ApplicationState"" as aps on aps.""Id"" = app.""ApplicationState""
	                where  task.""AssigneeUserId"" = '{userId}'  and task.""TaskStatusCode"" in ('INPROGRESS','OVERDUE')
                    and ur.""IsDeleted""=false and usp.""IsDeleted""=false and task.""IsDeleted""=false and s.""IsDeleted""=false and
                    tmp.""IsDeleted""=false and au.""IsDeleted""=false and app.""IsDeleted""=false and b.""IsDeleted""=false and aps.""IsDeleted""=false
	                group by ur.""Id""   
                ) t on ur.""Id""=t.""Id""
                left join(
	                select     'HR' as ""Code"",count(distinct app.""Id"") as ""Count""
                    FROM rec.""Batch"" as b
                    JOIN rec.""ListOfValue"" as l ON l.""Id"" = b.""BatchStatus"" --and l.""Code""='PendingWithHM'
                    join rec.""Application"" as app on app.""BatchId"" = b.""Id""
                    join rec.""ApplicationState"" as aps on aps.""Id"" = app.""ApplicationState""
                    join rec.""ApplicationStatus"" as apst on apst.""Id"" = app.""ApplicationStatus""
                    where l.""Code"" in('PendingWithHM', 'Draft', 'Close')
                ) hraps on ur.""Code""=hraps.""Code""
                left join(
	                 select 'HM' as ""Code"",count(distinct app.""Id"") as ""Count""
                    FROM rec.""Batch"" as b               
                    JOIN rec.""ListOfValue"" as l ON l.""Id"" = b.""BatchStatus"" --and l.""Code""='PendingWithHM'
                    join rec.""Application"" as app on app.""BatchId"" = b.""Id"" 
                    join rec.""ApplicationState"" as aps on aps.""Id"" = app.""ApplicationState""
                    join rec.""ApplicationStatus"" as apst on apst.""Id"" = app.""ApplicationStatus""
                    where b.""HiringManager"" = '{userId}' and l.""Code"" = 'PendingWithHM'
                    and apst.""Code"" in('NotShortlisted', 'ShortlistedHM', 'InterviewRequested')
                ) hmaps on ur.""Code""=hmaps.""Code""
                left join(
	                select ur.""Id"",count(task.""Id"")  as ""Count""                    
	                from public.""RecTask"" as task   
                    join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                    join public.""UserRoleUser"" as uru on uru.""UserId"" = task.""AssigneeUserId""
                    join public.""UserRole"" as ur on ur.""Id"" = uru.""UserRoleId"" and ur.""Id"" in ({roleText})
                    
	                where (task.""TemplateCode""='JOBDESCRIPTION_HM' or task.""TemplateCode"" ='TASK_DIRECT_HIRING' or task.""TemplateCode"" ='DIRECTHIRING_EVALUATIONFORM')  and task.""AssigneeUserId"" = '{userId}'  and task.""TaskStatusCode"" in ('INPROGRESS','OVERDUE')
                    and ur.""IsDeleted""=false and task.""IsDeleted""=false 
	                group by ur.""Id""  
                ) jd on ur.""Id""=jd.""Id""
                where ur.""Id"" in ({roleText})
                --order by CAST(coalesce(usp.""StageSequence"", '0') AS integer) asc";
                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
                //expanded -> type= userrole - from coming list find type as userRole
                // if found then find the item in list as selcted item id
                //make expanded true

                var obj = expObj.Where(x => x.Type == "USERROLE").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.UserRoleId).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }

            }
            else if (type == "USERROLE")
            {

                query = $@"Select usp.""InboxStageName"" ||' (' || COALESCE(t.""Count"",0)+COALESCE(hraps.""Count"",0)+COALESCE(hmaps.""Count"",0)+COALESCE(jd.""Count"",0)|| ')' as Name
                , usp.""InboxStageName"" as id, '{id}' as ParentId, 'STAGE' as Type,
                true as hasChildren, '{userRoleId}' as UserRoleId
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='RECRUITMENT'
                left join(
	                select usp.""InboxStageName"",count(task.""Id"")  as ""Count""
	                FROM public.""UserRole"" as ur
                    join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='RECRUITMENT'
	                join public.""RecTask"" as task on task.""TemplateCode"" =usp.""TemplateCode""
                    join public.""RecTask"" as s on  task.""ReferenceTypeId"" = s.""Id""
	                join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode""
	                join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
	                join rec.""Application"" as app on app.""Id"" = s.""ReferenceTypeId""
	                join rec.""Batch"" as b on b.""Id"" = app.""BatchId""
                    JOIN rec.""ListOfValue"" as l ON l.""Id"" = b.""BatchStatus"" and l.""Code""='PendingWithHM'
	                join rec.""ApplicationState"" as aps on aps.""Id"" = app.""ApplicationState""
	                where ur.""Id"" = '{userRoleId}' and  task.""AssigneeUserId"" = '{userId}'  and task.""TaskStatusCode"" in ('INPROGRESS','OVERDUE')
                    and ur.""IsDeleted""=false and usp.""IsDeleted""=false and task.""IsDeleted""=false and s.""IsDeleted""=false and
                    tmp.""IsDeleted""=false and au.""IsDeleted""=false and app.""IsDeleted""=false and b.""IsDeleted""=false and aps.""IsDeleted""=false
	                group by usp.""InboxStageName""   
                ) t on usp.""InboxStageName""=t.""InboxStageName""
                left join(
	                select     'Shortlist by HR' as ""Code"",count(distinct app.""Id"") as ""Count""
                    FROM rec.""Batch"" as b
                    JOIN rec.""ListOfValue"" as l ON l.""Id"" = b.""BatchStatus"" -- and l.""Code""='PendingWithHM'
                    join rec.""Application"" as app on app.""BatchId"" = b.""Id""
                    join rec.""ApplicationState"" as aps on aps.""Id"" = app.""ApplicationState""
                    join rec.""ApplicationStatus"" as apst on apst.""Id"" = app.""ApplicationStatus""
                    where l.""Code"" in('PendingWithHM', 'Draft', 'Close')
                ) hraps on usp.""InboxStageName""=hraps.""Code""
                left join(
	                select 'Shortlist by HM' as ""Code"",count(distinct app.""Id"") as ""Count""
                    FROM rec.""Batch"" as b               
                    JOIN rec.""ListOfValue"" as l ON l.""Id"" = b.""BatchStatus"" --and l.""Code""='PendingWithHM'
                    join rec.""Application"" as app on app.""BatchId"" = b.""Id""
                    join rec.""ApplicationState"" as aps on aps.""Id"" = app.""ApplicationState""
                    join rec.""ApplicationStatus"" as apst on apst.""Id"" = app.""ApplicationStatus""
                    where b.""HiringManager"" = '{userId}' and l.""Code"" = 'PendingWithHM'
                    and apst.""Code"" in('NotShortlisted', 'ShortlistedHM', 'InterviewRequested')
                ) hmaps on usp.""InboxStageName""=hmaps.""Code""
                 left join(
	                select usp.""InboxStageName"",count(task.""Id"")  as ""Count""
	                FROM public.""UserRole"" as ur
                    join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and (usp.""TemplateCode"" ='JOBDESCRIPTION_HM' or usp.""TemplateCode"" ='TASK_DIRECT_HIRING' or usp.""TemplateCode"" ='DIRECTHIRING_EVALUATIONFORM') and usp.""InboxCode""='RECRUITMENT'
	                join public.""RecTask"" as task on task.""TemplateCode"" =usp.""TemplateCode"" 
                  
	                where ur.""Id"" = '{userRoleId}' and  task.""AssigneeUserId"" = '{userId}'  and task.""TaskStatusCode"" in ('INPROGRESS','OVERDUE')
                    and ur.""IsDeleted""=false and usp.""IsDeleted""=false and task.""IsDeleted""=false 
	                group by usp.""InboxStageName""   
                ) jd on usp.""InboxStageName""=jd.""InboxStageName""
                where ur.""Id"" = '{userRoleId}'
                Group By hmaps.""Count"",hraps.""Count"",t.""Count"",jd.""Count"",usp.""InboxStageName"", usp.""StageSequence"", usp.""InboxStageName""
                order by CAST(coalesce(usp.""StageSequence"", '0') AS integer) asc";

                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);



                var obj = expObj.Where(x => x.Type == "STAGE").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }

            }
            else if (type == "STAGE")
            {
                query = $@"Select  case when  coalesce(usp.""TemplateCode"", usp.""TemplateName"")='NotSelected' then usp.""TemplateShortName"" 
                else usp.""TemplateShortName"" ||' (' || COALESCE(t.""Count"",0)|| ')' end  as Name,
                coalesce(usp.""TemplateCode"", usp.""TemplateName"") as id, '{id}' as ParentId,
                case when  coalesce(usp.""TemplateCode"", usp.""TemplateName"")='OpenBatches' 
                or coalesce(usp.""TemplateCode"", usp.""TemplateName"")='DraftBatches'
                or coalesce(usp.""TemplateCode"", usp.""TemplateName"")='ClosedBatches' then 'ShortlistByHR'
                when  coalesce(usp.""TemplateCode"", usp.""TemplateName"")='NotShortlisted' 
                or coalesce(usp.""TemplateCode"", usp.""TemplateName"")='ShortlistedByHM' 
                or coalesce(usp.""TemplateCode"", usp.""TemplateName"")='InterviewRequested' then 'ShortlistByHM'
                when  coalesce(usp.""TemplateCode"", usp.""TemplateName"")='NotSelected' then 'NotSelected'
                else 'TEMPLATE' end as Type,'{userRoleId}' as UserRoleId,
                case when  coalesce(usp.""TemplateCode"", usp.""TemplateName"")='NotSelected' then false else true end as hasChildren
                from public.""UserRole"" as ur
                join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='RECRUITMENT'
                left join(
	                select tmp.""TemplateCode"",count(task.""Id"")  as ""Count""
	                FROM public.""UserRole"" as ur
                    join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='RECRUITMENT'
	                join public.""RecTask"" as task on task.""TemplateCode"" =usp.""TemplateCode""
                    join public.""RecTask"" as s on  task.""ReferenceTypeId"" = s.""Id""
	                join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode""
	                join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
	                join rec.""Application"" as app on app.""Id"" = s.""ReferenceTypeId""
	                join rec.""Batch"" as b on b.""Id"" = app.""BatchId""
                    JOIN rec.""ListOfValue"" as l ON l.""Id"" = b.""BatchStatus"" and l.""Code""='PendingWithHM'
	                join rec.""ApplicationState"" as aps on aps.""Id"" = app.""ApplicationState""
	                where ur.""Id"" = '{userRoleId}' and  task.""AssigneeUserId"" = '{userId}' and usp.""InboxStageName"" = '{id}'  and task.""TaskStatusCode"" in ('INPROGRESS','OVERDUE')
                    and ur.""IsDeleted""=false and usp.""IsDeleted""=false and task.""IsDeleted""=false and s.""IsDeleted""=false and
                    tmp.""IsDeleted""=false and au.""IsDeleted""=false and app.""IsDeleted""=false and b.""IsDeleted""=false and aps.""IsDeleted""=false
	                group by tmp.""TemplateCode""   
                    union
                    select    case when  l.""Code""='PendingWithHM' then 'OpenBatches'
                    when l.""Code"" = 'Draft' then 'DraftBatches'
                    when l.""Code"" = 'Close' then 'ClosedBatches' end as ""TemplateCode"",count(distinct app.""Id"") as ""Count""
                    FROM rec.""Batch"" as b
                    JOIN rec.""ListOfValue"" as l ON l.""Id"" = b.""BatchStatus"" --and l.""Code""='PendingWithHM'
                    join rec.""Application"" as app on app.""BatchId"" = b.""Id""
                    join rec.""ApplicationState"" as aps on aps.""Id"" = app.""ApplicationState""
                    join rec.""ApplicationStatus"" as apst on apst.""Id"" = app.""ApplicationStatus""
                    where l.""Code"" in('PendingWithHM', 'Draft', 'Close')
                    GROUP BY l.""Code"", b.""SequenceOrder""
                    union
                    select    case when  apst.""Code""='NotShortlisted' then 'NotShortlisted'
                    when apst.""Code"" = 'ShortlistedHM' then 'ShortlistedByHM'
                    when apst.""Code"" = 'InterviewRequested' then 'InterviewRequested' end as ""TemplateCode"",count(distinct app.""Id"") as ""Count""
                    FROM rec.""Batch"" as b               
                    JOIN rec.""ListOfValue"" as l ON l.""Id"" = b.""BatchStatus"" --and l.""Code""='PendingWithHM'
                    join rec.""Application"" as app on app.""BatchId"" = b.""Id""
                    join rec.""ApplicationState"" as aps on aps.""Id"" = app.""ApplicationState""
                    join rec.""ApplicationStatus"" as apst on apst.""Id"" = app.""ApplicationStatus""
                    where b.""HiringManager"" = '{userId}' and l.""Code"" = 'PendingWithHM'
                    and apst.""Code"" in('NotShortlisted', 'ShortlistedHM', 'InterviewRequested')
                    GROUP BY apst.""Code""
                    union
                    select usp.""TemplateCode"",count(task.""Id"")  as ""Count""
	                FROM public.""UserRole"" as ur
                    join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and (usp.""TemplateCode"" ='JOBDESCRIPTION_HM' or usp.""TemplateCode"" ='TASK_DIRECT_HIRING' or usp.""TemplateCode"" ='DIRECTHIRING_EVALUATIONFORM') and usp.""InboxCode""='RECRUITMENT'
	                join public.""RecTask"" as task on task.""TemplateCode"" =usp.""TemplateCode""
                  
	                where ur.""Id"" = '{userRoleId}' and  task.""AssigneeUserId"" = '{userId}' and usp.""InboxStageName"" = '{id}'  and task.""TaskStatusCode"" in ('INPROGRESS','OVERDUE')
                    and ur.""IsDeleted""=false and usp.""IsDeleted""=false and task.""IsDeleted""=false 
	                group by usp.""TemplateCode"" 
                 
                ) t on coalesce(usp.""TemplateCode"", usp.""TemplateName"")=t.""TemplateCode""

                where ur.""Id"" = '{userRoleId}' and usp.""InboxStageName"" = '{id}'
                Group By t.""Count"",usp.""TemplateShortName"", usp.""TemplateCode"", usp.""InboxStageName"", usp.""ChildSequence"",id
                order by CAST(coalesce(usp.""ChildSequence"", '0') AS integer) asc";
                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);


                var obj = expObj.Where(x => x.Type == "TEMPLATE" || x.Type == "ShortlistByHM" || x.Type == "ShortlistByHR" || x.Type == "NotSelected").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }

            }
            else if (type == "TEMPLATE")
            {
                query = $@"select  b.""Id"" as id,
                b.""Name"" ||' (' || Count( distinct task.""Id"") || ')' as Name,
                true as hasChildren,b.""Id"" as BatchId,
                '{id}' as ParentId,'BATCH' as Type,sp.""Id"" as StageId
                FROM public.""RecTask"" as s
                join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id""
                join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode""
                join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                join rec.""Application"" as app on app.""Id"" = s.""ReferenceTypeId""
                join rec.""Batch"" as b on b.""Id"" = app.""BatchId""
                JOIN rec.""ListOfValue"" as l ON l.""Id"" = b.""BatchStatus"" and l.""Code""='PendingWithHM'
                join rec.""ApplicationState"" as aps on aps.""Id"" = app.""ApplicationState""
                join public.""UserRoleStageParent"" as sp on sp.""TemplateCode"" = tmp.""TemplateCode"" and sp.""UserRoleId"" = '{userRoleId}' and sp.""InboxCode""='RECRUITMENT'
                Where tmp.""TemplateCode""='{id}' and task.""TaskStatusCode"" in('INPROGRESS','OVERDUE') 
                and app.""Id"" is not null and task.""AssigneeUserId"" = '{userId}' and sp.""InboxStageName"" = '{parentId}'
                GROUP BY b.""Id"", b.""Name"", b.""SequenceOrder"",sp.""Id""
                order by b.""SequenceOrder"" asc ";

                if (id == "JOBDESCRIPTION_HM" || id == "TASK_DIRECT_HIRING" || id == "DIRECTHIRING_EVALUATIONFORM")
                {
                    query = $@"select 'STATUS' as Type,s.""StatusLabel"" ||' (' || COALESCE(t.""Count"",0)|| ')' as Name,
                '{id}' as StageId,s.""StatusCode"" as StatusCode,'' as BatchId,
                false as hasChildren
                FROM public.""UserRoleStageChild"" as s
                join public.""UserRoleStageParent"" as sp on sp.""Id"" = s.""InboxStageId"" and sp.""TemplateCode"" = '{id}' and sp.""UserRoleId"" = '{userRoleId}' and sp.""InboxCode""='RECRUITMENT'
                --left join rec.""UserRoleStatusLabelCode"" as urs on urs.""StatusLabelId"" = s.""Id""
                left join(
                    select case when task.""TaskStatusCode""='OVERDUE' then 'INPROGRESS' else task.""TaskStatusCode"" end as TaskStatusCode,count(task.""Id"")  as ""Count""
                    FROM public.""RecTask"" as task
                    join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode""
                    join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                  
                    Where tmp.""TemplateCode""='{id}' and task.""AssigneeUserId"" = '{userId}'
                    and task.""IsDeleted""=false and tmp.""IsDeleted""=false and au.""IsDeleted""=false 
                    
                    group by TaskStatusCode  
                ) t on t.TaskStatusCode=ANY(s.""StatusCode"")
               
                order by s.""SequenceOrder"" asc";
                }

                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);


                var obj = expObj.Where(x => x.Type == "BATCH").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }


            }
            else if (type == "ShortlistByHR")
            {
                query = $@"select  b.""Id"" as id,
                b.""Name"" ||' (' || Count( distinct app.""Id"") || ')' as Name,
                false as hasChildren,b.""Id"" as BatchId,
                '{id}' as ParentId,'HRBATCH' as Type
                FROM rec.""Batch"" as b               
                JOIN rec.""ListOfValue"" as l ON l.""Id"" = b.""BatchStatus""
                join rec.""Application"" as app on app.""BatchId"" = b.""Id""              
                join rec.""ApplicationState"" as aps on aps.""Id"" = app.""ApplicationState""
                join rec.""ApplicationStatus"" as apst on apst.""Id"" = app.""ApplicationStatus""
                #WHERE#
                GROUP BY b.""Id"", b.""Name"", b.""SequenceOrder""
                order by b.""SequenceOrder"" asc ";
                string where = "";
                if (id == "DraftBatches")
                {
                    where = $@" where l.""Code""='Draft'";
                }
                else if (id == "OpenBatches")
                {
                    where = $@" where l.""Code""='PendingWithHM'";
                }
                else if (id == "ClosedBatches")
                {
                    where = $@" where l.""Code""='Close'";
                }
                query = query.Replace("#WHERE#", where);
                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

                var obj = expObj.Where(x => x.Type == "HRBATCH").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }
            }
            else if (type == "ShortlistByHM")
            {
                query = $@"select  b.""Id"" as id,
                b.""Name"" ||' (' || Count( distinct app.""Id"") || ')' as Name,
                false as hasChildren,b.""Id"" as BatchId,
                '{id}' as ParentId,'HMBATCH' as Type
                FROM rec.""Batch"" as b               
                JOIN rec.""ListOfValue"" as l ON l.""Id"" = b.""BatchStatus""
                join rec.""Application"" as app on app.""BatchId"" = b.""Id""              
                join rec.""ApplicationState"" as aps on aps.""Id"" = app.""ApplicationState""
                join rec.""ApplicationStatus"" as apst on apst.""Id"" = app.""ApplicationStatus""
                where b.""HiringManager""='{userId}' and l.""Code""='PendingWithHM'
                #WHERE#
                GROUP BY b.""Id"", b.""Name"", b.""SequenceOrder""
                order by b.""SequenceOrder"" asc ";
                string where = "";
                if (id == "NotShortlisted")
                {
                    where = $@" and apst.""Code""='NotShortlisted'";
                }
                else if (id == "ShortlistedByHM")
                {
                    where = $@" and apst.""Code""='ShortlistedHM'";
                }
                else if (id == "InterviewRequested")
                {
                    where = $@" and apst.""Code""='InterviewRequested'";
                }
                query = query.Replace("#WHERE#", where);
                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

                var obj = expObj.Where(x => x.Type == "ShortlistByHM").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }
            }
            else if (type == "BATCH")
            {
                query = $@"select 'STATUS' as Type,s.""StatusLabel"" ||' (' || COALESCE(t.""Count"",0)|| ')' as Name,
                '{batchId}' as BatchId,'{parentId}' as StageId,s.""StatusCode"" as StatusCode,
                false as hasChildren
                FROM public.""UserRoleStageChild"" as s
                --left join rec.""UserRoleStatusLabelCode"" as urs on urs.""StatusLabelId"" = s.""Id""
                left join(
                    select case when task.""TaskStatusCode""='OVERDUE' then 'INPROGRESS' else task.""TaskStatusCode"" end as TaskStatusCode,count(task.""Id"")  as ""Count""
                    FROM public.""RecTask"" as s
                    join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id""
                    join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode""
                    join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                    join rec.""Application"" as app on app.""Id"" = s.""ReferenceTypeId""
                    join rec.""Batch"" as b on b.""Id"" = app.""BatchId""
                    join rec.""ApplicationState"" as aps on aps.""Id"" = app.""ApplicationState""
                    Where tmp.""TemplateCode""='{parentId}' and b.""Id""='{batchId}' and task.""AssigneeUserId"" = '{userId}'
                    and s.""IsDeleted""=false and task.""IsDeleted""=false and tmp.""IsDeleted""=false and au.""IsDeleted""=false and
                    app.""IsDeleted""=false and b.""IsDeleted""=false and aps.""IsDeleted""=false
                    group by TaskStatusCode  
                ) t on t.TaskStatusCode=ANY(s.""StatusCode"")
                where s.""InboxStageId""='{stageId}' and s.""IsDeleted""=false
                order by s.""SequenceOrder"" asc";

                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

                //var obj = expObj.Where(x => x.Name == type);
                //if (obj.Any())
                //{
                //    list.Find(x => x.id == obj.FirstOrDefault().Id).expanded = true;
                //}

                //var pending = new TreeViewViewModel
                //{
                //    id = "INPROGRESS",
                //    Type = "STATUS",
                //    ParentId = id,
                //    hasChildren = false,
                //    DisplayName = "Pending (0)",
                //    Name = "Pending (0)"
                //};

                //long pendingCount = 0;

                //var pendingItem = listItems.FirstOrDefault(x => x.id == "INPROGRESS");
                //if (pendingItem != null)
                //{
                //    pendingCount += pendingItem.RootId ?? 0;
                //}
                //var overDueItem = listItems.FirstOrDefault(x => x.id == "OVERDUE");
                //if (overDueItem != null)
                //{
                //    pendingCount += overDueItem.RootId ?? 0;
                //}
                //pending.DisplayName = $"Pending ({pendingCount})";
                //pending.Name = $"Pending ({pendingCount})";
                //list.Add(pending);

                //var completed = new TreeViewViewModel
                //{
                //    id = "COMPLETED",
                //    Type = "STATUS",
                //    ParentId = id,
                //    hasChildren = false,
                //    DisplayName = "Completed (0)",
                //    Name = "Completed (0)"
                //};
                //var completedItem = listItems.FirstOrDefault(x => x.id == "COMPLETED");
                //if (completedItem != null)
                //{
                //    completed.DisplayName = $"Completed ({completedItem.RootId ?? 0})";
                //    completed.Name = $"Completed ({completedItem.RootId ?? 0})";
                //}
                //list.Add(completed);

                //var rejected = new TreeViewViewModel
                //{
                //    id = "REJECTED",
                //    Type = "STATUS",
                //    ParentId = id,
                //    hasChildren = false,
                //    DisplayName = "Rejected (0)",
                //    Name = "Rejected (0)"
                //};
                //var rejectedItem = listItems.FirstOrDefault(x => x.id == "REJECTED");
                //if (rejectedItem != null)
                //{
                //    rejected.DisplayName = $"Rejected ({rejectedItem.RootId ?? 0})";
                //    rejected.Name = $"Rejected ({rejectedItem.RootId ?? 0})";
                //}
                //list.Add(rejected);


            }
            else
            {

                //          query = @$"select  
                //                          s.""StatusLabel"" as Name,
                //                          --tmp.""Subject"" ||' (' || Count( distinct b.""Id"") || ')' as Name,
                //                          tmp.""TemplateCode"" as id,
                //                          false as hasChildren
                //                          FROM rec.""UserRoleStageChild"" as s
                //                 -- left join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id""
                //                  --left join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                //--left join rec.""Application"" as app on app.""Id"" = s.""ReferenceTypeId""
                //--left join rec.""ApplicationState"" as aps on aps.""Id"" = app.""ApplicationState""
                //--left join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode""
                //                 -- left join rec.""Batch"" as b on b.""Id"" = app.""BatchId""
                //--Where task.""TaskStatusCode"" = 'INPROGRESS' and app.""Id"" is not null and  task.""AssigneeUserId"" = '{userId}'
                //                  where s.""InboxStageId""='{stageId}'

                //                  order by s.""SequenceOrder"" asc";
                query = $@"select 'STATUS' as Type,s.""StatusLabel"" ||' (' || COALESCE(t.""Count"",0)|| ')' as Name,
                '{parentId}' as StageId,s.""StatusCode"" as StatusCode,
                false as hasChildren
                FROM public.""UserRoleStageChild"" as s
                --left join rec.""UserRoleStatusLabelCode"" as urs on urs.""StatusLabelId"" = s.""Id""
                left join(
                    select case when task.""TaskStatusCode""='OVERDUE' then 'INPROGRESS' else task.""TaskStatusCode"" end as TaskStatusCode,count(task.""Id"")  as ""Count""
                    FROM public.""RecTask"" as s
                    join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id""
                    join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode""
                    join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""
                  
                    Where tmp.""TemplateCode""='{parentId}' and task.""AssigneeUserId"" = '{userId}'
                    and s.""IsDeleted""=false and task.""IsDeleted""=false and tmp.""IsDeleted""=false and au.""IsDeleted""=false 
                    
                    group by TaskStatusCode  
                ) t on t.TaskStatusCode=ANY(s.""StatusCode"")
                where s.""InboxStageId""='{stageId}'
                order by s.""SequenceOrder"" asc";

                list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);



            }
            return list;
        }

    }
}
