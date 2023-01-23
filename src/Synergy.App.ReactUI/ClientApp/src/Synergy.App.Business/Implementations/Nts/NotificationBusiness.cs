using AutoMapper;
using AutoMapper.Configuration;
using Synergy.App.Common;
using Synergy.App.Common.Utilities;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class NotificationBusiness : BusinessBase<NotificationViewModel, Notification>, INotificationBusiness
    {

        private IUserContext _userContext;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        private readonly IFileBusiness _fileBusiness;
        private readonly IEmailBusiness _emailBusiness;
        private readonly IRepositoryQueryBase<NotificationViewModel> _queryRepo;
        private readonly INtsQueryBusiness _ntsQueryBusiness;

        public NotificationBusiness(IRepositoryBase<NotificationViewModel, Notification> repo,
            IMapper autoMapper, IUserContext userContext,
            Microsoft.Extensions.Configuration.IConfiguration configuration,
            IFileBusiness fileBusiness,
            IEmailBusiness emailBusiness,
             IRepositoryQueryBase<NotificationViewModel> queryRepo,
             INtsQueryBusiness ntsQueryBusiness

            ) : base(repo, autoMapper)
        {
            _userContext = userContext;
            _configuration = configuration;
            _fileBusiness = fileBusiness;
            _emailBusiness = emailBusiness;
            _queryRepo = queryRepo;
            _ntsQueryBusiness = ntsQueryBusiness;
        }


        public async override Task<CommandResult<NotificationViewModel>> Create(NotificationViewModel viewModel, bool autoCommit = true)
        {
            if (viewModel.Recipient == null)
            {
                viewModel.Recipient = await GetUser(viewModel.ToUserId, viewModel.To);
            }
            if (viewModel.FromUserId.IsNullOrEmpty() && viewModel.From.IsNullOrEmpty())
            {
                var company = await GetSingleById<CompanyViewModel, Company>(_userContext.CompanyId);
                if (company.IsNotNull())
                {
                    var user = await GetSingle<UserViewModel, User>(x => x.Email == company.SmtpFromId);
                    if (user.IsNotNull())
                    {
                        viewModel.FromUserId = user.Id;
                    }
                }
            }
            if (viewModel.Sender == null)
            {
                viewModel.Sender = await GetUser(viewModel.FromUserId, viewModel.From);
            }

            if (viewModel.Recipient != null)
            {
                viewModel.Url = FillUrl(viewModel.Url, viewModel);
                viewModel.Subject = FillDynamicVariables(viewModel.Subject, viewModel);
                viewModel.Body = FillDynamicVariables(viewModel.Body, viewModel);
                viewModel.SmsText = FillDynamicVariables(viewModel.SmsText, viewModel);
                var result = await _repo.Create(viewModel);
                viewModel.Id = result.Id;
                await ManageEmail(viewModel);
                await _emailBusiness.ManageSms(viewModel);
                result.NotificationStatus = NotificationStatusEnum.Sent;
                await _repo.Edit(result);
                return CommandResult<NotificationViewModel>.Instance(viewModel);
            }
            return CommandResult<NotificationViewModel>.Instance(viewModel, false, "Recipient is not valid");
        }

        private async Task<UserViewModel> GetUser(string userId, string email)
        {
            return await _repo.GetSingle<UserViewModel, User>(x => x.Id == userId || x.Email == email);
        }


        public async Task ManageEmail(NotificationViewModel viewModel)
        {
            var allowEmail = viewModel.NotifyByEmail && viewModel.Recipient.Email.IsNotNullAndNotEmpty() &&
                (
                viewModel.SendAlways ||
                (viewModel.NotificationType == NotificationTypeEnum.Summary && viewModel.Recipient.EnableSummaryEmail) ||
                (viewModel.NotificationType == NotificationTypeEnum.Regular && viewModel.Recipient.EnableRegularEmail)
                );

            if (allowEmail)
            {
                viewModel.To = viewModel.Recipient.Email;
                var email = _autoMapper.Map<NotificationViewModel, EmailViewModel>(viewModel);
                email.Id = Guid.NewGuid().ToString();
                email.DataAction = DataActionEnum.Create;
                if (viewModel.SendAsync.IsNullOrTrue())
                {
                    await _emailBusiness.SendMailAsync(email);
                }
                else
                {
                    await _emailBusiness.SendMail(email);
                }

            }

        }



        private string FillUrl(string url, NotificationViewModel notification)
        {
            if (notification.Recipient == null)
            {
                return null;
            }
            if (url.IsNullOrEmpty())
            {
                return url;
            }
            return Helper.GenerateAbsoluteUrl(Helper.DynamicValueBind(url, notification), _configuration);
        }

        private string FillDynamicVariables(string text, NotificationViewModel viewModel)
        {
            if (text.IsNotNullAndNotEmpty() && viewModel != null)
            {
                if (text.Contains("{{recipient-name}}"))
                {
                    text = text.Replace("{{recipient-name}}", viewModel.Recipient.Name.Coalesce(viewModel.Recipient.UserName).Coalesce(viewModel.Recipient.Email));
                }
                if (text.Contains("{{date}}"))
                {
                    text = text.Replace("{{date}}", DateTime.Now.Date.ToDefaultDateFormat());
                }
                if (text.Contains("{{date-time}}"))
                {
                    text = text.Replace("{{date-time}}", DateTime.Now.Date.ToDefaultDateTimeFormat());
                }
                if (text.Contains("{{note-no}}"))
                {
                    text = text.Replace("{{note-no}}", "$!model.NoteNo");
                }
                if (text.Contains("{{note-subject}}"))
                {
                    text = text.Replace("{{note-subject}}", "$!model.NoteSubject");
                }
                if (text.Contains("{{note-description}}"))
                {
                    text = text.Replace("{{note-description}}", "$!model.NoteDescription");
                }
                if (text.Contains("{{task-no}}"))
                {
                    text = text.Replace("{{task-no}}", "$!model.TaskNo");
                }
                if (text.Contains("{{task-subject}}"))
                {
                    text = text.Replace("{{task-subject}}", "$!model.TaskSubject");
                }
                if (text.Contains("{{task-description}}"))
                {
                    text = text.Replace("{{task-description}}", "$!model.TaskDescription");
                }
                if (text.Contains("{{service-no}}"))
                {
                    text = text.Replace("{{service-no}}", "$!model.ServiceNo");
                }
                if (text.Contains("{{service-subject}}"))
                {
                    text = text.Replace("{{service-subject}}", "$!model.ServiceSubject");
                }
                if (text.Contains("{{service-description}}"))
                {
                    text = text.Replace("{{service-description}}", "$!model.ServiceDescription");
                }
                if (text.Contains("{{task-basic}}"))
                {
                    text = text.Replace("{{task-basic}}", GetTaskDetails(viewModel, false));
                }
                if (text.Contains("{{task-udf}}"))
                {
                    text = text.Replace("{{task-udf}}", GetTaskDetails(viewModel));
                }
                if (text.Contains("{{note-basic}}"))
                {
                    text = text.Replace("{{note-basic}}", GetNoteDetails(viewModel, false));
                }
                if (text.Contains("{{note-udf}}"))
                {
                    text = text.Replace("{{note-udf}}", GetNoteDetails(viewModel));
                }
                if (text.Contains("{{service-basic}}"))
                {
                    text = text.Replace("{{service-basic}}", GetServiceDetails(viewModel, false));
                }
                if (text.Contains("{{service-udf}}"))
                {
                    text = text.Replace("{{service-udf}}", GetServiceDetails(viewModel));
                }

                if (text.Contains("{{url}}"))
                {
                    text = text.Replace("{{url}}", GetUrl(viewModel));
                }
                if (text.Contains("{{base-url}}"))
                {
                    text = text.Replace("{{base-url}}", ApplicationConstant.AppSettings.ApplicationBaseUrl(_configuration));
                }

                if (text.Contains("{{user-name}}"))
                {
                    text = text.Replace("{{user-name}}", "$!model.Name");
                }
                if (text.Contains("{{password}}"))
                {
                    text = text.Replace("{{password}}", "$!model.Password");
                }
                if (text.Contains("{{login-id}}"))
                {
                    text = text.Replace("{{login-id}}", "$!model.Email");
                }

                if (text.Contains("{{do-not-reply}}"))
                {
                    text = text.Replace("{{do-not-reply}}", GetFooter());
                }
                if (text.Contains("{{signature}}"))
                {
                    text = text.Replace("{{signature}}", GetSystemSignature());
                }
                if (text.Contains("{{udf."))
                {
                    text = GetUdfInNotification(text, viewModel);
                }
                if (text.Contains("{{post-comment}}"))
                {
                    text = text.Replace("{{post-comment}}", "$!model.PostComment");
                }
            }
            return Helper.DynamicValueBind(text, null, viewModel.DynamicObject);
        }


        private string GetUdfInNotification(string text, NotificationViewModel viewModel)
        {
            var reg = new Regex("{{.*?}}");
            var matches = reg.Matches(text);
            foreach (var item in matches)
            {
                var str = item.ToString();
                if (text.Contains("{{nts-udf."))
                {
                    var udfName = str.Replace("{{nts-udf.", "");
                    var controls = (List<ColumnMetadataViewModel>)viewModel.DynamicObject?.ColumnList;
                    if (controls != null)
                    {
                        var udf = controls.FirstOrDefault(x => x.Name.ToLower() == udfName.ToLower());
                        if (udf != null)
                        {
                            text = text.Replace(str, Convert.ToString(udf.Value));
                        }
                        else
                        {
                            text = text.Replace(str, "");
                        }
                    }
                }
            }
            return text;
        }

        private string GetSystemSignature()
        {
            return string.Concat("<p>Regards,</p><p>Synergy Support Team</p>");
        }
        private string GetFooter()
        {
            return string.Concat("<p style='font-weight:bold;'>*** This is a system generated e-mail. Please do not reply to this e-mail***</p>");
        }
        private string GetUrl(NotificationViewModel viewModel)
        {
            return string.Concat("<a id='ext_url' href='", viewModel.Url, "' target='_blank'>Click here to see more details</a>");
            //ApplicationConstant.AppSettings.ApplicationBaseUrl(_configuration),
        }

        private string GetTaskDetails(NotificationViewModel nvm, bool includeUdf = true)
        {
            var body = new StringBuilder();

            var taskTemplate = (TaskTemplateViewModel)nvm.DynamicObject;

            var colorcode = "#c1972a";

            body.Append($"<table cellpadding='5' cellspacing='1' style='width:100%;max-width:600px;border:1px solid #fff;font-family:Verdana;font-size:10px;'>");
            body.Append($"<tr><td colspan='2' style='padding:6px;text-align:center;font-weight:bold;background-color:{colorcode};color:#fff;'>Task Details</td></tr>");

            body.Append($"<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:{colorcode};'>{taskTemplate.TaskNoText.ToDefaultTaskNoText()}</td><td style='background-color:#d0d2d3;'>$!model.TaskNo</td></tr>");
            body.Append($"<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:{colorcode};'>{taskTemplate.SubjectText.ToDefaultSubjectText()}</td><td style='background-color:#d0d2d3;'>$!model.TaskSubject</td></tr>");

            body.Append($"<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:{colorcode};'>{taskTemplate.DescriptionText.ToDefaultDescriptionText()}</td><td style='background-color:#d0d2d3;'>$!model.TaskDescription</td></tr>");
            body.Append($"<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:{colorcode};'>Assigned To</td><td style='background-color:#d0d2d3;'>$!model.AssignedToUserName</td></tr>");
            body.Append($"<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:{colorcode};'>Task Status</td><td style='background-color:#d0d2d3;'>$!model.DynamicObject.TaskStatusName</td></tr>");
            body.Append($"<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:{colorcode};'>Task Start Date</td><td style='background-color:#d0d2d3;'>{taskTemplate.StartDate.ToDefaultDateFormat()}</td></tr>");
            body.Append($"<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:{colorcode};'>Task Due Date</td><td style='background-color:#d0d2d3;'>{taskTemplate.DueDate.ToDefaultDateFormat()}</td></tr>");
            if (includeUdf)
            {
                if (taskTemplate.ColumnList != null && taskTemplate.ColumnList.Any())
                {
                    foreach (var item in taskTemplate.ColumnList)
                    {
                        if (item.IsVisible)
                        {
                            body.Append($"<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:{colorcode};'>{item.LabelName}</td><td style='background-color:#d0d2d3;'>{item.Value}</td></tr>");

                        }
                    }
                }
            }
            body.Append($"</table>");
            return body.ToString();
        }
        private string GetServiceDetails(NotificationViewModel nvm, bool includeUdf = true)
        {
            var body = new StringBuilder();

            var serviceTemplate = (ServiceTemplateViewModel)nvm.DynamicObject;

            var colorcode = "#c1972a";

            body.Append($"<table cellpadding='5' cellspacing='1' style='width:100%;max-width:600px;border:1px solid #fff;font-family:Verdana;font-size:10px;'>");
            body.Append($"<tr><td colspan='2' style='padding:6px;text-align:center;font-weight:bold;background-color:{colorcode};color:#fff;'>Task Details</td></tr>");

            body.Append($"<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:{colorcode};'>{serviceTemplate.ServiceNoTextWithDefault}</td><td style='background-color:#d0d2d3;'>$!model.TaskNo</td></tr>");
            body.Append($"<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:{colorcode};'>{serviceTemplate.SubjectText.ToDefaultSubjectText()}</td><td style='background-color:#d0d2d3;'>$!model.ServiceSubject</td></tr>");

            body.Append($"<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:{colorcode};'>{serviceTemplate.DescriptionText.ToDefaultDescriptionText()}</td><td style='background-color:#d0d2d3;'>$!model.ServiceDescription</td></tr>");
            body.Append($"<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:{colorcode};'>Task Status</td><td style='background-color:#d0d2d3;'>$!model.DynamicObject.ServiceStatusName</td></tr>");
            body.Append($"<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:{colorcode};'>Task Start Date</td><td style='background-color:#d0d2d3;'>{serviceTemplate.StartDate.ToDefaultDateFormat()}</td></tr>");
            body.Append($"<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:{colorcode};'>Task Due Date</td><td style='background-color:#d0d2d3;'>{serviceTemplate.DueDate.ToDefaultDateFormat()}</td></tr>");
            if (includeUdf)
            {
                if (serviceTemplate.ColumnList != null && serviceTemplate.ColumnList.Any())
                {
                    foreach (var item in serviceTemplate.ColumnList)
                    {
                        if (item.IsVisible)
                        {
                            body.Append($"<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:{colorcode};'>{item.LabelName}</td><td style='background-color:#d0d2d3;'>{item.Value}</td></tr>");

                        }
                    }
                }
            }
            body.Append($"</table>");
            return body.ToString();
        }

        private string GetNoteDetails(NotificationViewModel nvm, bool includeUdf = true)
        {
            var body = new StringBuilder();

            var noteTemplate = (NoteTemplateViewModel)nvm.DynamicObject;

            var colorcode = "#c1972a";

            body.Append($"<table cellpadding='5' cellspacing='1' style='width:100%;max-width:600px;border:1px solid #fff;font-family:Verdana;font-size:10px;'>");
            body.Append($"<tr><td colspan='2' style='padding:6px;text-align:center;font-weight:bold;background-color:{colorcode};color:#fff;'>Task Details</td></tr>");

            body.Append($"<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:{colorcode};'>{noteTemplate.NoteNoText.ToDefaultSubjectText()}</td><td style='background-color:#d0d2d3;'>$!model.TaskNo</td></tr>");
            body.Append($"<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:{colorcode};'>{noteTemplate.SubjectText.ToDefaultSubjectText()}</td><td style='background-color:#d0d2d3;'>$!model.ServiceSubject</td></tr>");

            body.Append($"<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:{colorcode};'>{noteTemplate.DescriptionText.ToDefaultDescriptionText()}</td><td style='background-color:#d0d2d3;'>$!model.ServiceDescription</td></tr>");
            body.Append($"<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:{colorcode};'>Task Status</td><td style='background-color:#d0d2d3;'>$!model.DynamicObject.ServiceStatusName</td></tr>");
            body.Append($"<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:{colorcode};'>Task Start Date</td><td style='background-color:#d0d2d3;'>{noteTemplate.StartDate.ToDefaultDateFormat()}</td></tr>");
            body.Append($"<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:{colorcode};'>Expiry Date</td><td style='background-color:#d0d2d3;'>{noteTemplate.ExpiryDate.ToDefaultDateFormat()}</td></tr>");
            if (includeUdf)
            {
                if (noteTemplate.ColumnList != null && noteTemplate.ColumnList.Any())
                {
                    foreach (var item in noteTemplate.ColumnList)
                    {
                        if (item.IsVisible)
                        {
                            body.Append($"<tr><td style='color:#fff;padding:6px;border:1px solid #fff;text-align:right;background-color:{colorcode};'>{item.LabelName}</td><td style='background-color:#d0d2d3;'>{item.Value}</td></tr>");

                        }
                    }
                }
            }
            body.Append($"</table>");
            return body.ToString();
        }

        public async Task<IList<NotificationViewModel>> GetNotificationList(string userId, string portalId, long count = 20, string referenceId = null, string id = null)
        {
            var data = await _ntsQueryBusiness.GetNotificationListData(userId, portalId, count = 20, referenceId, id);
            return data;
        }
        public async Task<IList<NotificationViewModel>> GetAllNotifications(string userId, string portalId, int? count = null, bool? isArchived = null, ReadStatusEnum? readStatus = null)
        {
            var data = await _ntsQueryBusiness.GetAllNotificationsData(userId, portalId, count, isArchived, readStatus);
            return data;
        }
        public async Task<NotificationViewModel> GetNotificationDetails(string notificationId)
        {
            var data = await _ntsQueryBusiness.GetNotificationDetailsData(notificationId);
            return data;
        }

        public async Task<long> GetNotificationCount(string userId, string portalId)
        {
            var data = await _ntsQueryBusiness.GetNotificationCountData(userId, portalId);
            return data;
        }

        public async Task MarkNotificationAsRead(string id)
        {
            await _ntsQueryBusiness.MarkNotificationAsReadData(id);
        }

        public async Task MarkNotificationAsNotRead(string id)
        {
            await _ntsQueryBusiness.MarkNotificationAsNotReadData(id);
        }

        public async Task ArchiveNotification(string id)
        {
            await _ntsQueryBusiness.ArchiveNotificationData(id);
        }

        public async Task UnArchiveNotification(string id)
        {
            await _ntsQueryBusiness.UnArchiveNotificationData(id);
        }

        public async Task StarNotification(string id)
        {
            await _ntsQueryBusiness.StarNotificationData(id);
        }

        public async Task UnStarNotification(string id)
        {
            await _ntsQueryBusiness.UnStarNotificationData(id);
        }

        public async Task MarkAllNotificationAsRead(string userId, string portalId)
        {
            await _ntsQueryBusiness.MarkAllNotificationAsRead(userId, portalId);
        }

        public async Task<List<NotificationViewModel>> GetNotificationList(DateTime date, ReferenceTypeEnum? refType = null, bool read = false, bool archive = false, string refTypeId = null)
        {
            DateTime FirstDate;
            DateTime LastDate;
            if (refType.IsNotNull())
            {
                FirstDate = date.Date;
                LastDate = date.Date;
            }
            else
            {
                FirstDate = date.AddDays(-3).Date;
                LastDate = date.AddDays(3).Date;
            }

            //            var query = $@"select m.""Name"" as Module,mg.""Name"" as MenuGroup,p.""Name"" as PageName,t.""DisplayName"" as Template,reference.""referenceno"" as ReferenceNo,
            //case when reference.""subject"" is not null then reference.""subject"" else n.""Subject"" end as Subject,reference.""Id"" as ReferenceTypeId,n.""Id"",n.""ReferenceType"",n.""CreatedDate""
            //,n.""ReadStatus"",n.""IsArchived"",n.""ActionStatus""
            //            from public.""Notification"" as n 
            //			 left join(select note.""TemplateId"",note.""Id"",note.""NoteNo"" as ReferenceNo,note.""NoteSubject"" as Subject from  public.""NtsNote"" as note
            //					   union 
            //					   select note.""TemplateId"",note.""Id"",note.""TaskNo"" as ReferenceNo,note.""TaskSubject"" as Subject  from  public.""NtsTask"" as note
            //					    union 
            //					   select note.""TemplateId"",note.""Id"",note.""ServiceNo"" as ReferenceNo,note.""ServiceSubject"" as Subject  from  public.""NtsService"" as note

            //					  ) as reference on reference.""Id"" = n.""ReferenceTypeId""
            //			 left join public.""Template"" as t on t.""Id""=reference.""TemplateId"" and t.""IsDeleted""=false
            //			 left join public.""Page"" as p on t.""Id""=p.""TemplateId"" and p.""IsDeleted""=false
            //			 left join public.""Module"" as m on m.""Id""=t.""ModuleId"" and m.""IsDeleted""=false
            //			 left  join public.""SubModule"" as sm on sm.""ModuleId""=m.""Id"" and sm.""IsDeleted""=false
            //			 left  join public.""MenuGroup"" as mg on mg.""SubModuleId""=sm.""Id"" and mg.""IsDeleted""=false
            //            left join public.""User"" as u on (u.""Id""=n.""FromUserId"" or n.""From""=u.""Email"") and u.""IsDeleted""=false 
            //            where n.""ToUserId""='{_repo.UserContext.UserId}' and n.""IsDeleted""=false and n.""PortalId""='{_repo.UserContext.PortalId}'
            //            and n.""CreatedDate""::TIMESTAMP::DATE<='{LastDate}'  and n.""CreatedDate""::TIMESTAMP::DATE>='{FirstDate}' #REFWHERE# #REFIDWHERE#
            //              -- order by n.""CreatedDate"" desc 
            //			group by m.""Name"",mg.""Name"",p.""Name"",t.""DisplayName"",reference.""referenceno"",reference.""subject"",n.""Subject"",n.""Id"",reference.""Id"" ";

            //            var refwhere = "";
            //            if (refType.IsNotNull())
            //            {
            //                refwhere = $@" and n.""ReferenceType""='{(int)refType}'";
            //            }
            //            query = query.Replace("#REFWHERE#", refwhere);

            //            var refidwhere = "";
            //            if (refTypeId.IsNotNull())
            //            {
            //                refidwhere = $@" and n.""ReferenceTypeId""='{refTypeId}'";
            //            }
            //            query = query.Replace("#REFIDWHERE#", refidwhere);

            //            var data = await _queryRepo.ExecuteQueryList(query, null);
            var data = await _ntsQueryBusiness.GetNotificationListData(date, FirstDate, LastDate, refType, refTypeId);
            if (!read)
            {
                data = data.Where(x => x.ReadStatus != ReadStatusEnum.Read).ToList();
            }
            if (!archive)
            {
                data = data.Where(x => x.IsArchived == false).ToList();
            }

            List<NotificationViewModel> newList = new List<NotificationViewModel>();
            newList.AddRange(data.DistinctBy(x => x.ReferenceTypeId).Where(x => x.ReferenceTypeId.IsNotNullAndNotEmpty()));
            newList.AddRange(data.Where(x => !x.ReferenceTypeId.IsNotNullAndNotEmpty()));
            foreach (var rec in newList)
            {
                if (rec.ReferenceTypeId.IsNotNullAndNotEmpty())
                {
                    var Notifications = await GetNotificationList(_repo.UserContext.UserId, _repo.UserContext.PortalId, 20, rec.ReferenceTypeId);
                    if (!read)
                    {
                        Notifications = Notifications.Where(x => x.ReadStatus != ReadStatusEnum.Read).ToList();
                    }
                    if (!archive)
                    {
                        Notifications = Notifications.Where(x => x.IsArchived == false).ToList();
                    }

                    rec.Notifications = Notifications;
                    if (refType.IsNotNull())
                    {
                        rec.CurrentDay = Notifications.Where(x => x.CreatedDate.Date == date.Date).ToList();
                    }
                    else
                    {
                        rec.CurrentDay = Notifications.Where(x => x.CreatedDate.Date == date.Date).ToList();
                        rec.FirstDay = Notifications.Where(x => x.CreatedDate.Date == date.AddDays(-3).Date).ToList();
                        rec.SecondDay = Notifications.Where(x => x.CreatedDate.Date == date.AddDays(-2).Date).ToList();
                        rec.ThirdDay = Notifications.Where(x => x.CreatedDate.Date == date.AddDays(-1).Date).ToList();
                        rec.FifthDay = Notifications.Where(x => x.CreatedDate.Date == date.AddDays(1).Date).ToList();
                        rec.SixthDay = Notifications.Where(x => x.CreatedDate.Date == date.AddDays(2).Date).ToList();
                        rec.SeventhDay = Notifications.Where(x => x.CreatedDate.Date == date.AddDays(3).Date).ToList();
                    }

                }
                else
                {
                    var Notifications = await GetNotificationList(_repo.UserContext.UserId, _repo.UserContext.PortalId, 20, null, rec.Id);
                    rec.Notifications = Notifications;
                    if (refType.IsNotNull())
                    {
                        rec.CurrentDay = Notifications.Where(x => x.CreatedDate.Date == date.Date).ToList();
                    }
                    else
                    {
                        rec.CurrentDay = Notifications.Where(x => x.CreatedDate.Date == date.Date).ToList();
                        rec.FirstDay = Notifications.Where(x => x.CreatedDate.Date == date.AddDays(-3).Date).ToList();
                        rec.SecondDay = Notifications.Where(x => x.CreatedDate.Date == date.AddDays(-2).Date).ToList();
                        rec.ThirdDay = Notifications.Where(x => x.CreatedDate.Date == date.AddDays(-1).Date).ToList();
                        rec.FifthDay = Notifications.Where(x => x.CreatedDate.Date == date.AddDays(1).Date).ToList();
                        rec.SixthDay = Notifications.Where(x => x.CreatedDate.Date == date.AddDays(2).Date).ToList();
                        rec.SeventhDay = Notifications.Where(x => x.CreatedDate.Date == date.AddDays(3).Date).ToList();
                    }

                }
            }
            return newList;
        }

        public Task<IList<NotificationViewModel>> GetTopFiveNotifications(string userId, string portalId)
        {
            throw new NotImplementedException();
        }
    }
}

