using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class PushNotificationBusiness : BusinessBase<NotificationViewModel, Notification>, IPushNotificationBusiness
    {
        INotificationTemplateBusiness _noteNotificationTemplateBusiness;
        ITableMetadataBusiness _tableMetadataBusiness;
        IUserBusiness _userBusiness;
        IRecEmailBusiness _emailBusiness;

        private readonly IRepositoryQueryBase<NotificationViewModel> _appqueryRepo;
        public PushNotificationBusiness(IRepositoryBase<NotificationViewModel, Notification> repo, IMapper autoMapper,
            INotificationTemplateBusiness noteNotificationTemplateBusiness
            , ITableMetadataBusiness tableMetadataBusiness, IUserBusiness userBusiness
            , IRecEmailBusiness emailBusiness, IRepositoryQueryBase<NotificationViewModel> appqueryRepo) : base(repo, autoMapper)
        {
            _noteNotificationTemplateBusiness = noteNotificationTemplateBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _userBusiness = userBusiness;
            _emailBusiness = emailBusiness;
            _appqueryRepo = appqueryRepo;
        }

        public async override Task<CommandResult<NotificationViewModel>> Create(NotificationViewModel viewModel)
        {
            var sendemail = false;
            var sendSummary = false;
            if (viewModel.ToUserId.IsNotNullAndNotEmpty())
            {
                var user = await _userBusiness.GetSingleById(viewModel.ToUserId);
                viewModel.RecipientName = user.Name;
                sendemail = user.EnableRegularEmail;
                sendSummary = user.EnableSummaryEmail;

            }
            else
            {
                viewModel.RecipientName = viewModel.ToUserDisplay;
            }


            if (viewModel.RecipientName != null)
            {
                viewModel.Url = string.Empty;
                viewModel.Subject = viewModel.Subject.IsNullOrEmpty() ? "Synergy Notification" : viewModel.Subject;
                viewModel.Body = viewModel.Body.IsNullOrEmpty() ? "Synergy Notification - Hi " + viewModel.RecipientName : viewModel.Body;
                //viewModel.Body = AppendParagraph(viewModel.Body);
                //viewModel.SmsText = FillDynamicVariables(viewModel.SmsText, viewModel);
                //var data = BusinessHelper.MapModel<NotificationViewModel, GEN_Notification>(viewModel);
                var result = await base.Create(viewModel);
                if (sendemail)
                {
                    var email = _autoMapper.Map<NotificationViewModel, EmailViewModel>(viewModel);
                    if(viewModel.EmailBody.IsNotNullAndNotEmpty())
                    {
                    email.Body = viewModel.EmailBody;
                    }
                    email.Operation = DataOperation.Create;
                    email.ReferenceType = viewModel.ReferenceType;
                    email.ReferenceId = viewModel.ReferenceTypeId;
                    email.ReferenceTemplateCode = viewModel.ReferenceTemplateCode;
                    email.IsIncludeAttachment = viewModel.IsIncludeAttachment;
                    email.ShowOriginalSender = viewModel.ShowOriginalSender;
                    email.To = viewModel.To;
                    var emailres = await _emailBusiness.SendMailAsync(email);


                    result.Item.EmailUniqueId = emailres.Item.Id;
                }
                
                result.Item.NotificationStatus = NotificationStatusEnum.Sent;

                //_repository.Edit(result.Item);
                return CommandResult<NotificationViewModel>.Instance(viewModel, result.IsSuccess, result.Messages);
            }
            return CommandResult<NotificationViewModel>.Instance(viewModel, false, new Dictionary<string, string>());
        }

        public async Task<CommandResult<NotificationViewModel>> CreateSummaryMail(NotificationViewModel viewModel)
        {
            
            var sendSummary = false;
            if (viewModel.ToUserId.IsNotNullAndNotEmpty())
            {
                var user = await _userBusiness.GetSingleById(viewModel.ToUserId);
                viewModel.RecipientName = user.Name;              
                sendSummary = user.EnableSummaryEmail;

            }
            else
            {
                viewModel.RecipientName = viewModel.ToUserDisplay;
            }


            if (viewModel.RecipientName != null)
            {
                viewModel.Url = string.Empty;
                viewModel.Subject = viewModel.Subject.IsNullOrEmpty() ? "Synergy Notification" : viewModel.Subject;
                viewModel.Body = viewModel.Body.IsNullOrEmpty() ? "Synergy Notification - Hi " + viewModel.RecipientName : viewModel.Body;
                //viewModel.Body = AppendParagraph(viewModel.Body);
                //viewModel.SmsText = FillDynamicVariables(viewModel.SmsText, viewModel);
                //var data = BusinessHelper.MapModel<NotificationViewModel, GEN_Notification>(viewModel);
                var result = await base.Create(viewModel);
                if (sendSummary)
                {
                    var email = _autoMapper.Map<NotificationViewModel, EmailViewModel>(viewModel);
                    if (viewModel.EmailBody.IsNotNullAndNotEmpty())
                    {
                        email.Body = viewModel.EmailBody;
                    }
                    email.Operation = DataOperation.Create;
                    email.ReferenceType = viewModel.ReferenceType;
                    email.ReferenceId = viewModel.ReferenceTypeId;
                    email.ReferenceTemplateCode = viewModel.ReferenceTemplateCode;
                    email.IsIncludeAttachment = viewModel.IsIncludeAttachment;
                    email.ShowOriginalSender = viewModel.ShowOriginalSender;
                    email.To = viewModel.To;
                    var emailres = await _emailBusiness.SendMailAsync(email);


                    result.Item.EmailUniqueId = emailres.Item.Id;
                }

                result.Item.NotificationStatus = NotificationStatusEnum.Sent;

                //_repository.Edit(result.Item);
                return CommandResult<NotificationViewModel>.Instance(viewModel, result.IsSuccess, result.Messages);
            }
            return CommandResult<NotificationViewModel>.Instance(viewModel, false, new Dictionary<string, string>());
        }
        //private string FillDynamicVariables(string text, NotificationViewModel viewModel)
        //{
        //    if (text.IsNotNullAndNotEmpty() && viewModel != null)
        //    {
        //        if (text.Contains("^^salutation^^"))
        //        {
        //            text = text.Replace("^^salutation^^", string.Concat("Dear ", viewModel.Recipient.DisplayName, ","));
        //        }
        //        if (text.Contains("^^date^^"))
        //        {
        //            text = text.Replace("^^date^^", DateTime.Now.Date.ToDefaultDateFormat());
        //        }
        //        if (text.Contains("^^date_time^^"))
        //        {
        //            text = text.Replace("^^date_time^^", DateTime.Now.Date.ToDefaultDateTimeFormat());
        //        }
        //        if (text.Contains("^^note-text^^"))
        //        {
        //            text = text.Replace("^^note-text^^", "$!model.DynamicObject.Text-$!model.DynamicObject.NoteNo");
        //        }
        //        if (text.Contains("^^note-noteno^^"))
        //        {
        //            text = text.Replace("^^note-text^^", "$!model.DynamicObject.NoteNo");
        //        }
        //        if (text.Contains("^^note-subject^^"))
        //        {
        //            text = text.Replace("^^note-subject^^", "$!model.DynamicObject.Subject");
        //        }
        //        if (text.Contains("^^note-description^^"))
        //        {
        //            text = text.Replace("^^note-description^^", "$!model.DynamicObject.Description");
        //        }
        //        if (text.Contains("^^note-notificationsubject^^"))
        //        {
        //            text = text.Replace("^^note-subject^^", GetNoteSubject(viewModel));
        //        }
        //        if (text.Contains("^^task-subject^^"))
        //        {
        //            text = text.Replace("^^task-subject^^", GetTaskSubject(viewModel));
        //        }
        //        if (text.Contains("^^service-subject^^"))
        //        {
        //            text = text.Replace("^^service-subject^^", GetServiceSubject(viewModel));
        //        }
        //        if (text.Contains("^^task-basic^^"))
        //        {
        //            text = text.Replace("^^task-basic^^", GetTaskBasic(viewModel));
        //        }
        //        if (text.Contains("^^task-udf^^"))
        //        {
        //            text = text.Replace("^^task-udf^^", GetTaskUdf(viewModel));
        //        }
        //        if (text.Contains("^^note-basic^^"))
        //        {
        //            text = text.Replace("^^note-basic^^", GetNoteBasic(viewModel));
        //        }
        //        if (text.Contains("^^service-basic^^"))
        //        {
        //            text = text.Replace("^^service-basic^^", GetServiceBasic(viewModel));
        //        }
        //        if (text.Contains("^^task-service-basic^^"))
        //        {
        //            //text = text.Replace("^^task-service-basic^^", GetTaskServiceBasic(viewModel));
        //        }
        //        if (text.Contains("^^url^^"))
        //        {
        //            text = text.Replace("^^url^^", GetUrl(viewModel));
        //        }
        //        if (text.Contains("^^plain_url^^"))
        //        {
        //            text = text.Replace("^^plain_url^^", viewModel.Url);
        //        }
        //        if (text.Contains("^^Post_Comment^^"))
        //        {
        //            text = text.Replace("^^Post_Comment^^", "$!model.DynamicObject.PostComment");
        //        }
        //        if (text.Contains("^^user_name^^"))
        //        {
        //            text = text.Replace("^^user_name^^", "$!model.DynamicObject.UserName");
        //        }
        //        if (text.Contains("^^password^^"))
        //        {
        //            text = text.Replace("^^password^^", "$!model.DynamicObject.Password");
        //        }
        //        if (text.Contains("^^login_id^^"))
        //        {
        //            text = text.Replace("^^login_id^^", "$!model.DynamicObject.Email");
        //        }
        //        if (text.Contains("^^code^^"))
        //        {
        //            text = text.Replace("^^code^^", "$!model.DynamicObject.Code");
        //        }
        //        if (text.Contains("^^do_not_reply^^"))
        //        {
        //            text = text.Replace("^^do_not_reply^^", GetFooter());
        //        }
        //        if (text.Contains("^^system_signature^^"))
        //        {
        //            text = text.Replace("^^system_signature^^", GetSystemSignature());
        //        }
        //        if (text.Contains("^^lead-text^^"))
        //        {
        //            text = text.Replace("^^lead-text^^", "$!model.DynamicObject.FirstName-$!model.DynamicObject.Id");
        //        }
        //        if (text.Contains("^^from-name^^"))
        //        {
        //            text = text.Replace("^^from-name^^", "$!model.DynamicObject.OwnerDisplayName");
        //        }
        //        if (text.Contains("^^template-name^^"))
        //        {
        //            text = text.Replace("^^template-name^^", "$!model.DynamicObject.TemplateMasterName");
        //        }
        //        if (text.Contains("^^step-task-notification-subject^^"))
        //        {
        //            text = text.Replace("^^step-task-notification-subject^^", "$!model.DynamicObject.NotificationSubject");
        //        }
        //        if (text.Contains("^^step-task-service-owner^^"))
        //        {
        //            text = text.Replace("^^step-task-service-owner^^", "$!model.DynamicObject.ServiceViewModel.OwnerDisplayName");
        //        }
        //        if (text.Contains("^^step-task-service-udf."))
        //        {
        //            text = ManageStepTaskServiceUdfInNotification(text, viewModel);
        //        }
        //        if (text.Contains("^^nts-udf."))
        //        {
        //            text = GetUdfInNotification(text, viewModel);
        //        }
        //        if (text.Contains("^^manual-reports^^"))
        //        {
        //            text = text.Replace("^^manual-reports^^", GetManualRepotVersionDetails(viewModel));
        //        }
        //        //if (text.Contains("^^task-subject^^"))
        //        //{
        //        //    text = ManageTaskSubject(text, viewModel);
        //        //}
        //        //if (text.Contains("^^step-task-subject^^"))

        //        //{
        //        //    text = ManageStepTaskSubject(text, viewModel);
        //        //}
        //        //ManageStepTaskSubject(text, viewModel);
        //        //if (text.Contains("^^service-subject^^"))
        //        //{
        //        //    text = ManageServiceSubject(text, viewModel);
        //        //}
        //        //if (text.Contains("^^task-body^^"))
        //        //{
        //        //    text = ManageTaskBody(text, viewModel);
        //        //}
        //        //if (text.Contains("^^step-task-body^^"))
        //        //{
        //        //    text = ManageStepTaskBody(text, viewModel);
        //        //}
        //        //if (text.Contains("^^service-body^^"))
        //        //{
        //        //    text = ManageServiceBody(text, viewModel);
        //        //}

        //    }
        //    return Helper.DynamicValueBind(text, viewModel);
        //}
        public async override Task<CommandResult<NotificationViewModel>> Edit(NotificationViewModel model)
        {
            var data = _autoMapper.Map<NotificationViewModel>(model);
            var result = await base.Edit(data);
            if (result.IsSuccess)
            {
                // var tableResult = await _tableMetadataBusiness.ManageTemplateTable(new TemplateViewModel { Id = model.TemplateId, Json = model.Json, TemplateType = TemplateTypeEnum.Note });
                //if (!tableResult.IsSuccess) 
                //{
                //    return CommandResult<NotificationViewModel>.Instance(model, tableResult.IsSuccess, tableResult.Messages);
                //}
            }
            return CommandResult<NotificationViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task SetAllNotificationRead(string userId)
        {
            var read = ReadStatusEnum.Read;
            string query = @$"update public.""Notification"" set ""ReadStatus""='1' where ""ToUserId"" ='{userId}'";

            await _appqueryRepo.ExecuteScalar<bool?>(query, null);
        }
        public async Task SetAllTaskNotificationRead(string userId, string taskId)
        {
            var read = ReadStatusEnum.Read;
            string query = @$"update public.""Notification"" set ""ReadStatus""='1' where ""ToUserId"" ='{userId}' and ""ReferenceTypeId""='{taskId}'";

            await _appqueryRepo.ExecuteScalar<bool?>(query, null);
        }
        public async Task<CommandResult<NotificationViewModel>> UpdateAsRead(NotificationViewModel viewModel)
        {
            var existingData = await _repo.GetSingle(x => x.Id == viewModel.Id);
            existingData.ReadStatus = ReadStatusEnum.Read;
            var result = await base.Edit(existingData);
            return CommandResult<NotificationViewModel>.Instance(viewModel, result.IsSuccess, result.Messages);
        }
        public async Task<IList<NotificationViewModel>> GetNotificationList(string userId, long count = 8)
        {

            var query = $@" Select n.*, u.""PhotoId"" as PhotoId,CONCAT( u.""Name"",'<',u.""Email"",'>') as From
            from public.""Notification"" as n 
            left join public.""User"" as u on u.""Id""=n.""FromUserId"" or n.""From""=u.""Email"" and u.""IsDeleted""=false
            where n.""ToUserId""='{userId}' and n.""ReadStatus""='0' and n.""IsDeleted""=false
            order by n.""CreatedDate"" desc";
          var data=  await _appqueryRepo.ExecuteQueryList<NotificationViewModel>(query, null);
            return data;
           // var Notifications = await GetList(x => x.ToUserId == userId && x.ReadStatus == ReadStatusEnum.NotRead);
          //  var users = await _repo.GetList<UserViewModel, User>();
          //  foreach (var notification in Notifications)
          //  {
          //      var user = users.FirstOrDefault(x => x.Email == notification.From);
          //      if (user != null)
          //      {
          //          notification.PhotoId = user.PhotoId;
          //      }
          //      //if (notification.ReferenceTypeName == ReferenceTypeEnum.NTS_Task)
          //      //{
          //      //    var tsk = await _repo.GetSingleById<TaskViewModel, RecTask>(notification.ReferenceTypeId);
          //      //    if (tsk != null)
          //      //    {
          //      //        notification.ReferenceTypeNo = tsk.TaskNo;
          //      //    }

          //      //}
          //  }
          //  return Notifications.OrderByDescending(x => x.CreatedDate).ToList();
        }
        public async Task<IList<NotificationViewModel>> GetTaskNotificationList(string taskId, string userId, long count = 8)
        {
            var Notifications = await GetList(x => x.ToUserId == userId && x.ReferenceTypeId == taskId && x.ReferenceType == ReferenceTypeEnum.NTS_Task);
            var users = await _repo.GetList<UserViewModel, User>();
            foreach (var notification in Notifications)
            {
                var user = users.FirstOrDefault(x => x.Email == notification.From);
                if (user != null)
                {
                    notification.PhotoId = user.PhotoId;
                }
            }
            return Notifications.OrderByDescending(x => x.CreatedDate).ToList();
        }
        public async Task<IList<NotificationViewModel>> GetNoteNotificationList(string noteId, string userId, long count = 8)
        {
            var Notifications = await GetList(x => x.ToUserId == userId && x.ReferenceTypeId == noteId && x.ReferenceType == ReferenceTypeEnum.NTS_Note);
            var users = await _repo.GetList<UserViewModel, User>();
            foreach (var notification in Notifications)
            {
                var user = users.FirstOrDefault(x => x.Email == notification.From);
                if (user != null)
                {
                    notification.PhotoId = user.PhotoId;
                }
            }
            return Notifications.OrderByDescending(x => x.CreatedDate).ToList();
        }
        public async Task<IList<NotificationViewModel>> GetServiceNotificationList(string serviceId, string userId, long count = 8)
        {
            var Notifications = await GetList(x => x.ToUserId == userId && x.ReadStatus == ReadStatusEnum.NotRead && x.ReferenceTypeId == serviceId && x.ReferenceType == ReferenceTypeEnum.NTS_Service);
            var users = await _repo.GetList<UserViewModel, User>();
            foreach (var notification in Notifications)
            {
                var user = users.FirstOrDefault(x => x.Email == notification.From);
                if (user != null)
                {
                    notification.PhotoId = user.PhotoId;
                }
            }
            return Notifications.OrderByDescending(x => x.CreatedDate).ToList();
        }
        //public async Task<string> GetTaskEmailSummary(UserViewModel user)
        //{
        //    var model = await _taskBusiness.GetTaskSummaryCountByUserId(user.Id);
        //    var body = new StringBuilder();
        //    body.Append("<div><h5> Hello "+user.UserName+"</h5>Below is Task summary Details:<br><br></div>");
        //    body.Append("<div style='padding:0px;'><div style='margin-bottom: 20px;margin-top: 30px;color: #333333;background: #fff;border: 1px solid #ffa726;border-radius: 0.5rem;'>");
        //    body.Append("<table style='width:100%'><tr><td>");
        //    body.Append("<div style='padding:10px;font-weight:500;font-size:large;box-shadow: 0 4px 20px 0px rgba(0, 0, 0, 0.14), 0 7px 10px -5px rgba(255, 152, 0, 0.4);background: #ffa726;border-top-left-radius: 0.5rem;border-top-right-radius: 0.5rem;'>Task Summary</div>");
        //    body.Append("</td></tr>");
        //    body.Append("<tr><td style='padding-bottom:10px;'><table style='width:100%;'><tr><td style='width:20%;text-align:center;'>");
        //    body.Append("<p class='box-red-email' style='box-shadow: 0 4px 20px 0px rgba(0, 0, 0, 0.14), 0 7px 10px -5px rgba(244, 67, 54, 0.4);background: #ef5350;border-radius: 0.5rem;padding-right: 5px;padding-left: 5px;padding-top: 10px;padding-bottom: 10px;display: inline-block;text-align: center;height: 28px;width: 42px;font-size: 16px;font-weight: 500;color: #fff;' title='Overdue'>"+ model.OverdueCount +"</p>");
        //    body.Append("<p style='color:gray;font-size:small;font-style: italic;'>Overdue</p></td>");
        //    body.Append("<td style='width:20%;text-align:center;'>");
        //    body.Append("<p style='box-shadow: 0 4px 20px 0px rgba(0, 0, 0, 0.14), 0 7px 10px -5px rgba(0, 188, 212, 0.4);background: #26c6da;border-radius: 0.5rem;padding-right: 5px;padding-left: 5px;padding-top: 10px;padding-bottom: 10px;display: inline-block;text-align: center;height: 28px;width: 42px;font-size: 16px;font-weight: 500;color: #fff;' title='Pending'>" + model.InprogressCount + "</p>");
        //    body.Append("<p style='color:gray;font-size:small;font-style: italic;'>Pending</p></td>");
        //    body.Append("<td style='width:20%;text-align:center'>");
        //    body.Append("<p style='box-shadow: 0 4px 20px 0px rgba(0, 0, 0, 0.14), 0 7px 10px -5px rgba(76, 175, 80, 0.4);background: #66bb6a;border-radius: 0.5rem;padding-right: 5px;padding-left: 5px;padding-top: 10px;padding-bottom: 10px;display: inline-block;text-align: center;height: 28px;width: 42px;font-size: 16px;font-weight: 500;color: #fff;' title='Completed'>" + model.CompletedCount + "</p>");
        //    body.Append("<p style='color:gray;font-size:small;font-style: italic;'>Completed</p></td>");
        //    body.Append("<td style='width:20%;text-align:center;'>");
        //    body.Append("<p style='box-shadow: 0 4px 20px 0px rgba(0, 0, 0, 0.14), 0 7px 10px -5px rgba(255, 152, 0, 0.4);background: #ffa726;border-radius: 0.5rem;padding-right: 5px;padding-left: 5px;padding-top: 10px;padding-bottom: 10px;display: inline-block;text-align: center;height: 28px;width: 42px;font-size: 16px;font-weight: 500;color: #fff;' title='Draft'>" + model.DraftCount + "</p>");
        //    body.Append("<p style='color:gray;font-size:small;font-style: italic;'>Draft</p></td>");
        //    body.Append("</tr></table></td></tr></table></div></div>");
        //    return body.ToString();
        //}
    }
}
