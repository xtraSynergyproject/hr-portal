using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using Hangfire;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using MongoDB.Bson;
using MongoDB.Driver;
//using OpenPop.Mime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CMS.Business
{
    public class EmailBusiness : BusinessBase<EmailViewModel, Email>, IEmailBusiness
    {
        private ICompanyBusiness _companyBusiness;
        private readonly IConfiguration _configuration;
        private readonly IFileBusiness _fileBusiness;
        private IProjectEmailSetupBusiness _projectEmailBusiness;
        private IUserBusiness _userBusiness;
        private ITaskBusiness _taskBusiness;
        private IUserContext _userContext;
        private readonly IRepositoryQueryBase<MessageEmailViewModel> _queryRepo;
        private readonly ITalentAssessmentBusiness _talentAssessmentBusiness;
        public EmailBusiness(IRepositoryBase<EmailViewModel, Email> repo
            , IMapper autoMapper
            , ICompanyBusiness companyBusiness
            , IConfiguration configuration
            , IFileBusiness fileBusiness
            , IProjectEmailSetupBusiness projectEmailBusiness,
             IUserBusiness userBusiness,
             IUserContext userContext,
             ITaskBusiness taskBusiness
            , IRepositoryQueryBase<MessageEmailViewModel> queryRepo, ITalentAssessmentBusiness talentAssessmentBusiness
            ) : base(repo, autoMapper)
        {
            _companyBusiness = companyBusiness;
            _configuration = configuration;
            _fileBusiness = fileBusiness;
            _projectEmailBusiness = projectEmailBusiness;
            _userBusiness = userBusiness;
            _taskBusiness = taskBusiness;
            _userContext = userContext;
            _queryRepo = queryRepo;
            _talentAssessmentBusiness = talentAssessmentBusiness;
        }
        private void SetEnvironment(MailMessage mailMessage, EmailViewModel email)
        {
            var testRecipients = _configuration.GetValue<string>("TestEmailRecipients");
            var appEnv = _configuration.GetValue<string>("ApplicationEnvironment");
            if (appEnv != "PROD")
            {
                if (testRecipients.IsNotNullAndNotEmpty())
                {
                    AppendActualRecipientsInBody(mailMessage);
                    mailMessage.To.Clear();
                    mailMessage.CC.Clear();
                    mailMessage.Bcc.Clear();
                    var toList = testRecipients.Split(';');
                    foreach (var item in toList)
                    {
                        if (item.IsNotNullAndNotEmpty())
                        {
                            mailMessage.To.Add(item.Trim());
                        }
                    }
                }
                else
                {
                    throw new ArgumentNullException("Test Email Recipient is not added in web config");
                }
            }
        }

        private void AppendActualRecipientsInBody(MailMessage mailMessage)
        {
            var recipinets = new StringBuilder();
            if (mailMessage.To != null && mailMessage.To.Count > 0)
            {
                recipinets.Append("TO: ");
                foreach (var item in mailMessage.To)
                {
                    recipinets.Append(string.Concat(item.Address, ";"));
                }
                recipinets.Append("<br/>");
            }
            if (mailMessage.CC != null && mailMessage.CC.Count > 0)
            {
                recipinets.Append("CC: ");
                foreach (var item in mailMessage.CC)
                {
                    recipinets.Append(string.Concat(item.Address, ";"));
                }
                recipinets.Append("<br/>");
            }
            if (mailMessage.Bcc != null && mailMessage.Bcc.Count > 0)
            {
                recipinets.Append("BCC: ");
                foreach (var item in mailMessage.Bcc)
                {
                    recipinets.Append(string.Concat(item.Address, ";"));
                }
                recipinets.Append("<br/>");
            }
            mailMessage.Body = string.Concat(recipinets.ToString(), mailMessage.Body);
        }
        public async Task<CommandResult<EmailViewModel>> SendMail(string emailId)
        {
            var email = await _repo.GetSingleById(emailId);
            return await SendMail(email);
        }
        public async Task<CommandResult<EmailViewModel>> SendMail(EmailViewModel email)
        {
            var config = await GetEmailConfig(email);
            var mailMessage = await GetMailMessage(email, config);
            try
            {
                SetEnvironment(mailMessage, email);
                config.SmtpClient.Send(mailMessage);
                email.EmailStatus = NotificationStatusEnum.Sent;
                var result = await this.Edit(email);

            }
            catch (Exception ex)
            {
                email.EmailStatus = NotificationStatusEnum.Error;
                email.Error = ex.ToString();
            }
            finally
            {
                await ManageEmail(email, config, mailMessage);
                if (mailMessage != null)
                {
                    mailMessage.Dispose();
                }
            }
            return CommandResult<EmailViewModel>.Instance(email, email.EmailStatus != NotificationStatusEnum.Error, email.Error);

        }
        private async Task<CommandResult<EmailViewModel>> ManageEmail(EmailViewModel email, EmailProperties config, MailMessage mailMessage)
        {
            if (email.Operation == DataOperation.Create)
            {
                email.SmtpHost = config.SmtpClient.Host;
                email.SmtpPort = config.SmtpClient.Port;
                email.SmtpUserId = config.SmtpUserId;
                email.From = mailMessage.From.Address;
                email.SenderName = mailMessage.From.DisplayName;
                var result = await Create(email);

                return CommandResult<EmailViewModel>.Instance(email);
            }
            else
            {
                email.RetryCount = email.RetryCount + 1;
                email.EmailStatus = email.EmailStatus;
                email.SequenceNo = email.SequenceNo;

                var result = await base.Edit(email);
                return CommandResult<EmailViewModel>.Instance(email, result.IsSuccess, result.Message);
            }

        }

        private async Task<MailMessage> GetMailMessage(EmailViewModel email, EmailProperties config)
        {
            var mailMessage = new MailMessage
            {
                IsBodyHtml = true,
                BodyEncoding = System.Text.Encoding.UTF8,
                SubjectEncoding = System.Text.Encoding.UTF8
            };
            //mailMessage.Headers.Add("X-ReferenceId", "12345678");
            if (email.ShowOriginalSender)
            {
                mailMessage.From = new MailAddress(email.From, email.SenderName);
            }
            else
            {
                mailMessage.From = new MailAddress(config.FromEmailId, config.SenderName);
            }
            var toCollection = new MailAddressCollection();
            var toList = email.To.Split(';');
            foreach (var item in toList)
            {
                if (item.IsNotNullAndNotEmpty())
                {
                    mailMessage.To.Add(item.Trim());
                }
            }
            if (email.CC.IsNotNullAndNotEmpty())
            {
                var ccCollection = new MailAddressCollection();
                var ccList = email.CC.Split(';');
                foreach (var item in ccList)
                {
                    if (item.IsNotNullAndNotEmpty())
                    {
                        mailMessage.CC.Add(item.Trim());
                    }
                }
            }

            if (email.BCC.IsNotNullAndNotEmpty())
            {
                var bccCollection = new MailAddressCollection();
                var bccList = email.BCC.Split(';');
                foreach (var item in bccList)
                {
                    if (item.IsNotNullAndNotEmpty())
                    {
                        mailMessage.Bcc.Add(item.Trim());
                    }
                }
            }
            mailMessage.Subject = email.Subject;
            mailMessage.Body = email.Body;
            if (email.AttachmentIds!=null && email.AttachmentIds.Any())
            {
                foreach (var item in email.AttachmentIds)
                {
                    var attachment = await _fileBusiness.GetFile(item);
                    mailMessage.Attachments.Add(new Attachment(new MemoryStream(attachment.ContentByte), attachment.FileName));
                }


            }
            
            return mailMessage;
        }
        public async Task<CommandResult<EmailViewModel>> SendMailTask(EmailViewModel email)
        {
            var config = new EmailProperties();
           // var emailSetup = await _projectEmailBusiness.GetSingle(x => x.UserId == email.OwnerUserId);
            var emailSetup = await _projectEmailBusiness.GetSingle(x => x.SmtpUserId == email.SenderEmail);

            if (emailSetup != null)
            {
                var decryptPass = Helper.Decrypt(emailSetup.SmtpPassword);
                config = new EmailProperties
                {
                    FromEmailId = emailSetup.SmtpUserId,
                    SenderName = emailSetup.SmtpUserId,
                    SmtpHost = emailSetup.SmtpHost,
                    SmtpPort = emailSetup.SmtpPort,
                    SmtpUserId = emailSetup.SmtpUserId,
                    SmtpPassword = decryptPass,

                    SmtpClient = new SmtpClient
                    {
                        Host = emailSetup.SmtpHost,
                        Port = emailSetup.SmtpPort,
                        EnableSsl = true,
                        UseDefaultCredentials = false,
                        Credentials = new System.Net.NetworkCredential(emailSetup.SmtpUserId, emailSetup.SmtpPassword)

                    }
                };
            }
            else
            {
                config = await GetEmailConfig(email);
            }

            //if (emailSetup.IsNotNull())
            //{
            //    email.Body = string.Concat(email.Body,emailSetup.Signature);
            //}
            email.Body = string.Concat(email.Body, "<br/><p>Note:This task is sent via synergy system, please do not delete original content of message while replying</p><br/><br/><br/><p style='color:#fff;display:none;'>#SReferenceId_" + email.ReferenceId+"_EReferenceId#</p>");
            
            var mailMessage = await GetMailMessage(email, config);
            
            try
            {
                //SetEnvironment(mailMessage, email);
                config.SmtpClient.Send(mailMessage);
                email.EmailStatus = NotificationStatusEnum.Sent;
               // var result = await this.Edit(email);

            }
            catch (Exception ex)
            {
                email.EmailStatus = NotificationStatusEnum.Error;
                email.Error = ex.ToString();
            }
            finally
            {
                await ManageEmail(email, config, mailMessage);
                if (mailMessage != null)
                {
                    mailMessage.Dispose();
                }
            }
            return CommandResult<EmailViewModel>.Instance(email, email.EmailStatus != NotificationStatusEnum.Error, email.Error);
            //var config = new EmailProperties
            //{
            //    FromEmailId = "noorul.office@gmail.com",
            //    SenderName = "noorul.office@gmail.com",
            //    SmtpHost = "smtp.gmail.com",
            //    SmtpPort = 587,
            //    SmtpUserId = "noorul.office@gmail.com",
            //    SmtpPassword = "Salamath@33",

            //    SmtpClient = new SmtpClient
            //    {
            //        Host = "smtp.gmail.com",
            //        Port = 587,
            //        EnableSsl = true,
            //        UseDefaultCredentials = false,
            //        Credentials = new System.Net.NetworkCredential("noorul.office@gmail.com", "Salamath@33")

            //    }
            //};
            //email.Subject = "new Test1";
            //email.Body = "new1 Test123  #SReferenceId_7837829_EReferenceId#";
            //email.To = "noorulhuthamh@gmail.com";

            //try
            //{

            //    var mailMessage = await GetMailMessage(email, config);


            //    //SetEnvironment(mailMessage, email);
            //    config.SmtpClient.Send(mailMessage);
            //   // email.EmailStatus = NotificationStatusEnum.Sent;
            //   // var result = await this.Edit(email);

            //}
            //catch (Exception ex)
            //{
            //    email.EmailStatus = NotificationStatusEnum.Error;
            //    email.Error = ex.ToString();
            //}
            //finally
            //{
            //    //await ManageEmail(email, config, mailMessage);
            //    //if (mailMessage != null)
            //    //{
            //    //    mailMessage.Dispose();
            //    //}
            //}
            //return CommandResult<EmailViewModel>.Instance(email, email.EmailStatus != NotificationStatusEnum.Error, email.Error);

        }
        public void ReceiveEmailTask()
        {
            var config = new EmailProperties
            {
                FromEmailId = "noorul.office@gmail.com",
                SenderName = "noorul.office@gmail.com",
                SmtpHost = "smtp.gmail.com",
                SmtpPort = 587,
                SmtpUserId = "noorul.office@gmail.com",
                SmtpPassword = "Salamath@33",

                SmtpClient = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential("noorul.office@gmail.com", "Salamath@33")

                }
            };
            var client = new OpenPop.Pop3.Pop3Client();
            client.Connect(config.SmtpHost, 995, true);
            client.Authenticate("recent:" + config.SmtpUserId, config.SmtpPassword);


            var msgCount = (int)client.GetMessageCount();
           // Message message = client.GetMessage(msgCount);

          //  MessagePart plain = message.FindFirstPlainTextVersion();
           // var bd = plain.GetBodyAsText();
          //  MessagePart html = message.FindFirstHtmlVersion();
           // var ht = html.GetBodyAsText();

          //  int first = bd.IndexOf("#SReferenceId_");
          //  int last = bd.IndexOf("_EReferenceId#");
          //  string reference = bd.Substring(first, last - first);
           // if (reference.IsNotNullAndNotEmpty())
          //  {
          //      var refId = reference.Split("_");
         //       var parentId = refId[1];
         //   }

        }
        private async Task<MailMessage> PrepareMailMessage(EmailViewModel email, EmailProperties config)
        {
            var mailMessage = new MailMessage
            {
                IsBodyHtml = true,
                BodyEncoding = System.Text.Encoding.UTF8,
                SubjectEncoding = System.Text.Encoding.UTF8
            };
            if (email.ShowOriginalSender)
            {
                mailMessage.From = new MailAddress(email.From, email.SenderName);
            }
            else
            {
                mailMessage.From = new MailAddress(config.FromEmailId, config.SenderName);
            }
            var toCollection = new MailAddressCollection();
            var toList = email.To.Split(';');
            foreach (var item in toList)
            {
                if (item.IsNotNullAndNotEmpty())
                {
                    mailMessage.To.Add(item.Trim());
                }
            }
            if (email.CC.IsNotNullAndNotEmpty())
            {
                var ccCollection = new MailAddressCollection();
                var ccList = email.CC.Split(';');
                foreach (var item in ccList)
                {
                    if (item.IsNotNullAndNotEmpty())
                    {
                        mailMessage.CC.Add(item.Trim());
                    }
                }
            }

            if (email.BCC.IsNotNullAndNotEmpty())
            {
                var bccCollection = new MailAddressCollection();
                var bccList = email.BCC.Split(';');
                foreach (var item in bccList)
                {
                    if (item.IsNotNullAndNotEmpty())
                    {
                        mailMessage.Bcc.Add(item.Trim());
                    }
                }
            }
            mailMessage.Subject = email.Subject;
            mailMessage.Body = email.Body;
            //Guid id = Guid.NewGuid(); //Save the id in your database 
            //mailMessage.Headers.Add("Message-Id", String.Format("<{0}@{1}>", id.ToString(), config.SmtpHost));
            if (email.IsIncludeAttachment)
            {
                var task = await _repo.GetSingleById<RecTaskViewModel, RecTask>(email.ReferenceId);
                if (task.IsNotNull() && task.AttachmentCode1.IsNotNull())
                {
                    var file = await _fileBusiness.GetFileByte(task.AttachmentCode1);
                    mailMessage.Attachments.Add(new Attachment(new MemoryStream(file), task.AttachmentValue1));
                }
                if (task.IsNotNull() && task.AttachmentCode2.IsNotNull())
                {
                    var file = await _fileBusiness.GetFileByte(task.AttachmentCode2);
                    mailMessage.Attachments.Add(new Attachment(new MemoryStream(file), task.AttachmentValue2));
                }
                if (task.IsNotNull() && task.AttachmentCode3.IsNotNull())
                {
                    var file = await _fileBusiness.GetFileByte(task.AttachmentCode3);
                    mailMessage.Attachments.Add(new Attachment(new MemoryStream(file), task.AttachmentValue3));
                }
                if (task.IsNotNull() && task.AttachmentCode4.IsNotNull())
                {
                    var file = await _fileBusiness.GetFileByte(task.AttachmentCode4);
                    mailMessage.Attachments.Add(new Attachment(new MemoryStream(file), task.AttachmentValue4));
                }
                if (task.IsNotNull() && task.AttachmentCode5.IsNotNull())
                {
                    var file = await _fileBusiness.GetFileByte(task.AttachmentCode5);
                    mailMessage.Attachments.Add(new Attachment(new MemoryStream(file), task.AttachmentValue5));
                }
                if (task.IsNotNull() && task.AttachmentCode6.IsNotNull())
                {
                    var file = await _fileBusiness.GetFileByte(task.AttachmentCode6);
                    mailMessage.Attachments.Add(new Attachment(new MemoryStream(file), task.AttachmentValue6));
                }
                if (task.IsNotNull() && task.AttachmentCode7.IsNotNull())
                {
                    var file = await _fileBusiness.GetFileByte(task.AttachmentCode7);
                    mailMessage.Attachments.Add(new Attachment(new MemoryStream(file), task.AttachmentValue7));
                }
                if (task.IsNotNull() && task.AttachmentCode8.IsNotNull())
                {
                    var file = await _fileBusiness.GetFileByte(task.AttachmentCode8);
                    mailMessage.Attachments.Add(new Attachment(new MemoryStream(file), task.AttachmentValue8));
                }
                if (task.IsNotNull() && task.AttachmentCode9.IsNotNull())
                {
                    var file = await _fileBusiness.GetFileByte(task.AttachmentCode9);
                    mailMessage.Attachments.Add(new Attachment(new MemoryStream(file), task.AttachmentValue9));
                }
                if (task.IsNotNull() && task.AttachmentCode10.IsNotNull())
                {
                    var file = await _fileBusiness.GetFileByte(task.AttachmentCode10);
                    mailMessage.Attachments.Add(new Attachment(new MemoryStream(file), task.AttachmentValue10));
                }
            }
            return mailMessage;
        }

        public async Task<EmailProperties> GetEmailConfig(EmailViewModel email)
        {
            var company = new CompanyViewModel();
            if (_repo.UserContext != null && _repo.UserContext.CompanyId.IsNotNullAndNotEmpty())
            {
                company = await _companyBusiness.GetSingleById(_repo.UserContext.CompanyId);

            }
            else
            {
                company = await _companyBusiness.GetSingleById(email.CompanyId);
            }
            var emailConfig = new EmailProperties();
            if (company == null)
            {
                emailConfig = new EmailProperties
                {
                    FromEmailId = "Info@extranet.ae",
                    SenderName = "Info@extranet.ae",
                    SmtpHost = "smtp.office365.com",
                    SmtpPort = 587,
                    SmtpUserId = "Info@extranet.ae",
                    SmtpPassword = "!Welcome123",

                    SmtpClient = new SmtpClient
                    {
                        Host = "smtp.office365.com",
                        Port = 587,
                        EnableSsl = true,
                        UseDefaultCredentials = false,
                        Credentials = new System.Net.NetworkCredential("Info@extranet.ae", "!Welcome123")

                    }
                };

            }
            else
            {
                var decryptPass = Helper.Decrypt(company.SmtpPassword);
                emailConfig = new EmailProperties
                    {
                        FromEmailId = company.SmtpFromId,
                        SenderName = company.SmtpSenderName,
                        SmtpHost = company.SmtpHost,
                        SmtpPort = company.SmtpPort,
                        SmtpUserId = company.SmtpUserId,
                        SmtpPassword = decryptPass,

                        SmtpClient = new SmtpClient
                        {
                            Host = company.SmtpHost,
                            Port = company.SmtpPort,
                            EnableSsl = true,
                            UseDefaultCredentials = false,
                            Credentials = new System.Net.NetworkCredential(company.SmtpFromId, decryptPass)

                        }

                    };
                
                //emailConfig = new EmailProperties
                //{
                //    FromEmailId = "Info@extranet.ae",
                //    SenderName = "Info@extranet.ae",
                //    SmtpHost = "smtp.office365.com",
                //    SmtpPort = 587,
                //    SmtpUserId = "Info@extranet.ae",
                //    SmtpPassword = "!Welcome123",

                //    SmtpClient = new SmtpClient
                //    {
                //        Host = "smtp.office365.com",
                //        Port = 587,
                //        EnableSsl = true,
                //        UseDefaultCredentials = false,
                //        Credentials = new System.Net.NetworkCredential("Info@extranet.ae", "!Welcome123")

                //    }

                //};
            }

            return emailConfig;
        }

        public async Task<CommandResult<EmailViewModel>> SendMailAsync(EmailViewModel email)
        {
            var config = await GetEmailConfig(email);
            var mailMessage = await GetMailMessage(email, config);
            try
            {
                email.IsAsync = true;
                email.EmailStatus = NotificationStatusEnum.Enqueued;
                var result = await ManageEmail(email, config, mailMessage);
                if (result.IsSuccess)
                {
                   //await SendMail(result.Item.Id);
                    BackgroundJob.Enqueue<HangfireScheduler>(x => x.SendMail(result.Item.Id,_repo.UserContext.ToIdentityUser()));
                }
                return CommandResult<EmailViewModel>.Instance(email, result.IsSuccess, result.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (mailMessage != null)
                {
                    mailMessage.Dispose();
                }
            }
        }

        public async Task ReceiveMail()
        {
            var emailSetup = await _projectEmailBusiness.ReadEmailSetupUsers(); // reading all the user of the Email Setup
            if (emailSetup.IsNotNull())
            {
                foreach (var u in emailSetup) // iterating user for creating WBS - item for new user
                {
                    if (u.ServiceId.IsNotNull() || u.UserId.IsNotNull())
                    {
                        var config = await GetUserEmailConfig(u.UserId, u.ServiceId);
                        // var mail = BusinessHelper.GetInstance<IMailMessageBusiness>();
                        try
                        {
                            var client = new ImapClient();
                            // var client = new OpenPop.Pop3.Pop3Client();
                            client.Connect(config.SmtpHost, 993, true);
                            client.Authenticate(config.SmtpUserId, config.SmtpPassword);
                            var inbox = client.Inbox;
                           inbox.Open(FolderAccess.ReadOnly);
                           // long TotalCount = inbox.Count();
                            // long TotalCount = client.GetMessageCount();
                            // long itemCount = await _projectEmailBusiness.GetEmailSetupTotalCount(u.Id);

                            //Update Total Count in Email Setup
                            // var emailCount = await _projectEmailBusiness.UpdateEmailSetupCount(u.Id, TotalCount);
                            // search for messages sent since a particular date
                            var uids = client.Inbox.Search(SearchQuery.SentOn(DateTime.Now));
                            long TotalCount = uids.Count;
                            // using the uids of the matching messages, fetch the BODYSTRUCTUREs
                            // of each message so that we can figure out which MIME parts to
                            // download individually.
                            //if (itemCount == 0)
                            //{
                            //    itemCount = emailCount.Count;
                            //}


                            //  long differnceInCount = 0;
                            //if (differnceInCount > 0)
                           
                            //for (var i = 1; i <= TotalCount; i++)
                                foreach (var uid in uids)
                                {
                                    //var message = client.GetMessage(msgCount);
                                var message = inbox.GetMessage(uid);
                       

                                var plain = message.GetTextBody(MimeKit.Text.TextFormat.Text);                             
                                var existtask = await GetTaskMessageId(message.MessageId);
                                if (!existtask.IsNotNull())
                                {

                                
                                    if (plain != null)
                                    {
                                        //var bd = plain.GetBodyAsText();
                                        var bd = plain;
                                        //MessagePart html = message.FindFirstHtmlVersion();
                                        //var ht = html.GetBodyAsText();
                                        
                                        int first = bd.IndexOf("#SReferenceId_");
                                        int last = bd.IndexOf("_EReferenceId#");
                                        if (first > 0)
                                        {
                                            string reference = bd.Substring(first, last - first);
                                            if (reference.IsNotNullAndNotEmpty())
                                            {
                                                var refId = reference.Split("_");
                                                var from = "";
                                                foreach (var item in message.From.Mailboxes)
                                                {
                                                    from = item.Address;
                                                }
                                                var to = "";
                                                foreach (var item in message.To.Mailboxes)
                                                {
                                                    to = item.Address;
                                                }
                                                var cc = "";
                                                foreach (var item in message.Cc.Mailboxes)
                                                {
                                                    cc = item.Address;
                                                }
                                                var bcc = "";
                                                foreach (var item in message.Bcc.Mailboxes)
                                                {
                                                    bcc = item.Address;
                                                }
                                                var FromUser = await _userBusiness.GetSingle(x => x.Email == from);
                                                //GetUserIdByEmail(message.Headers.From.Address);
                                                var ToUser = await _userBusiness.GetSingle(x => x.Id == u.UserId);

                                                foreach (var item in message.To.Mailboxes)
                                                {
                                                   // var mail = message.ToMailMessage();
                                                    var taskTemplate = new TaskTemplateViewModel();
                                                    var emailmodel = new EmailTaskViewModel();
                                                    taskTemplate.ActiveUserId = _repo.UserContext.UserId;
                                                    taskTemplate.TemplateCode = "EMAIL_TASK";
                                                    var task = await _taskBusiness.GetTaskDetails(taskTemplate);
                                                    if (FromUser == null)
                                                    {
                                                        task.OwnerUserId = u.UserId;
                                                    }
                                                    else
                                                    {
                                                        task.OwnerUserId = FromUser.Id;
                                                    }
                                                    //var ToUser = await _projectEmailBusiness.GetSingle(x => x.SmtpUserId == item.Address);
                                                    //if (ToUser.IsNotNull())
                                                    //{
                                                    //    task.AssignedToUserId = u.UserId;
                                                    //}
                                            
                                                    task.TaskSubject = message.Subject;
                                                    task.RequestedByUserId = task.OwnerUserId;
                                                    task.AssignedToUserId = u.UserId;
                                                    task.StartDate = DateTime.Now;
                                                    task.DueDate = DateTime.Now.AddDays(2);

                                                    task.Json = "{}";
                                                    task.DataAction = DataActionEnum.Create;
                                                    task.ParentServiceId = u.ServiceId;
                                                    task.TaskDescription = message.HtmlBody;
                                                    task.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                                                    //if (message.Headers.UnknownHeaders.Get("ReferenceId").IsNotNullAndNotEmpty())
                                                    //{
                                                    //    task.ParentTaskId = message.Headers.UnknownHeaders.Get("ReferenceId");
                                                    //}

                                                    task.ParentTaskId = refId[1];                                                  
                                                    emailmodel = _autoMapper.Map<TaskTemplateViewModel, EmailTaskViewModel>(task, emailmodel);
                                                    emailmodel.MessageId = message.MessageId;
                                                    emailmodel.From = from;
                                                   // emailmodel.To = ToUser.Email;
                                                    emailmodel.To = to;
                                                    emailmodel.CC = cc;
                                                    emailmodel.BCC = bcc;
                                                    emailmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(emailmodel);
                                                    var res = await _taskBusiness.ManageTask(emailmodel);
                                                    if (res.IsSuccess)
                                                    {
                                                        foreach (var attachment in message.Attachments)
                                                        {
                                                            // download the attachment just like we did with the body
                                                            var entity = attachment;


                                                            var part = (MimeKit.MimePart)entity;

                                                            // note: it's possible for this to be null, but most will specify a filename
                                                            var fileName = part.FileName;
                                                            var stream = new MemoryStream();
                                                            //var path = Path.Combine(directory, fileName);

                                                            // decode and save the content to a file
                                                            //using (var stream = File.Create(path))
                                                            part.Content.DecodeTo(stream);

                                                            var Content = stream.ToArray();
                                                            var saveattachment = await _fileBusiness.Create(new FileViewModel
                                                            {
                                                                ContentByte = Content,
                                                                ContentType = attachment.ContentType.ToString(),
                                                                ContentLength = stream.Length,
                                                                FileName = fileName,
                                                                ReferenceTypeId = res.Item.TaskId,
                                                                ReferenceTypeCode = ReferenceTypeEnum.NTS_Task,
                                                                FileExtension = Path.GetExtension(fileName)
                                                            }
                                                            );
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                           // }

                        }
                        catch (Exception ex)
                        {

                        }

                    }
                
                }
            }
            var companyId = _repo.UserContext.CompanyId;
            //get info for common email  Info@extranet.ae
            var emailConfig = await GetEmailConfig(new EmailViewModel { CompanyId = companyId });
            try
            {
                //var client = new OpenPop.Pop3.Pop3Client();
                var client = new ImapClient();
                client.Connect(emailConfig.SmtpHost, 993, true);
                client.Authenticate(emailConfig.SmtpUserId, emailConfig.SmtpPassword);
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);
               // long TotalCount = inbox.Count;
                // var companyDetails = await _companyBusiness.GetSingleById(companyId);
                //  long itemCount = companyDetails.Count;
                //  companyDetails.Count = TotalCount;
                var uids = client.Inbox.Search(SearchQuery.SentOn(DateTime.Now));
                long TotalCount = uids.Count;
                //if (differnceInCount > 0)
               
                // var emailCount = await _companyBusiness.Edit(companyDetails);
                // var differnceInCount = TotalCount - itemCount;
                
                   // int msgCount = (int)itemCount + 1;
                    foreach (var i in uids)
                    {
                        var message = inbox.GetMessage(i);
                        var existtask = await GetTaskMessageId(message.MessageId);
                        // MessagePart plain = message.FindFirstPlainTextVersion();
                        var plaintext = message.TextBody;
                    var bodypart = message.HtmlBody;
                 // var  bodytext = System.Text.RegularExpressions.Regex.Replace(bodypart, "<.*?>|&.*?;", string.Empty);
                    var plain1 = message.GetTextBody(MimeKit.Text.TextFormat.Text);
                    var reader = new StringReader(bodypart);
                   
                    var tokenizer = new MimeKit.Text.HtmlTokenizer(reader)
                    {
                        DecodeCharacterReferences = true
                    };
                    var stack = new List<HtmlTagId>();
                    MimeKit.Text.HtmlToken token;
                    var writer = new StringWriter();
                    while (tokenizer.ReadNextToken(out token))
                    {
                        switch (token.Kind)
                        {
                            case HtmlTokenKind.Tag:
                                var tag = (HtmlTagToken)token;

                                if (tag.IsEmptyElement || tag.Id.IsEmptyElement())
                                {
                                    if (tag.Id == HtmlTagId.Br || tag.Id == HtmlTagId.P)
                                        writer.WriteLine();
                                }
                                else if (tag.IsEndTag)
                                {
                                    if (tag.Id == HtmlTagId.P)
                                        writer.WriteLine();

                                    Pop(stack, tag.Id);
                                }
                                else
                                {
                                    if (tag.Id == HtmlTagId.P)
                                        writer.WriteLine();

                                    Push(stack, tag.Id);
                                }
                                break;
                            case HtmlTokenKind.Data:
                                var data = (HtmlDataToken)token;

                                if (stack.Count == 0)
                                    break;

                                switch (stack[stack.Count - 1])
                                {
                                    case HtmlTagId.Head:
                                    case HtmlTagId.Title:
                                    case HtmlTagId.Meta:
                                    case HtmlTagId.Table:
                                    case HtmlTagId.TR:
                                        // ignore
                                        break;
                                    default:
                                        writer.Write(data.Data);
                                        break;
                                }
                                break;
                        }
                    }
                    var plain = writer.ToString();
                    if (!existtask.IsNotNull())
                        {

                      
                        if (plain != null)
                        {
                           // var bd = plain.GetBodyAsText();
                            var bd = plain;
                            //MessagePart html = message.FindFirstHtmlVersion();
                            //var ht = html.GetBodyAsText();

                            int first = bd.IndexOf("#SReferenceId_");
                            int last = bd.IndexOf("_EReferenceId#");
                            if (first > 0)
                            {
                                string reference = bd.Substring(first, last - first);
                                if (reference.IsNotNullAndNotEmpty())
                                {
                                    var refId = reference.Split("_");
                                    var from = "";
                                    foreach (var item in message.From.Mailboxes)
                                    {
                                        from = item.Address;
                                    }
                                    var to = "";
                                    foreach (var item in message.To.Mailboxes)
                                    {
                                        to = item.Address;
                                    }
                                    var cc = "";
                                    foreach (var item in message.Cc.Mailboxes)
                                    {
                                        cc = item.Address;
                                    }
                                    var bcc = "";
                                    foreach (var item in message.Bcc.Mailboxes)
                                    {
                                        bcc = item.Address;
                                    }

                                    var FromUser = await _userBusiness.GetSingle(x => x.Email == from);
                                    var userModel = new UserViewModel();
                                    var parenttask = await _taskBusiness.GetSingleById(refId[1]);
                                    

                                    foreach (var item in message.To.Mailboxes)
                                    {
                                        var emailmodel = new EmailTaskViewModel();
                                        var taskTemplate = new TaskTemplateViewModel();
                                        taskTemplate.ActiveUserId = _repo.UserContext.UserId;
                                        taskTemplate.TemplateCode = "EMAIL_TASK";
                                        var task = await _taskBusiness.GetTaskDetails(taskTemplate);
                                        if (FromUser == null && parenttask.IsNotNull())
                                        {
                                            task.OwnerUserId = parenttask.OwnerUserId;
                                        }
                                        task.TaskSubject = message.Subject;
                                       // task.OwnerUserId = task.AssignedToUserId;
                                        task.AssignedToUserId = parenttask.AssignedToUserId;
                                        task.RequestedByUserId = task.OwnerUserId;
                                        task.StartDate = DateTime.Now;
                                        task.DueDate = DateTime.Now.AddDays(2);
                                        task.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                                        task.Json = "{}";
                                        task.DataAction = DataActionEnum.Create;
                                        //task.Description = mail.Body;
                                        task.TaskDescription = message.HtmlBody;

                                        //task.ParentTaskId = message.Headers.UnknownHeaders.Get("ReferenceId");
                                        task.ParentTaskId = refId[1];
                                        emailmodel = _autoMapper.Map<TaskTemplateViewModel, EmailTaskViewModel>(task, emailmodel);
                                        emailmodel.From = from;
                                        emailmodel.To = to;
                                        emailmodel.CC = cc;
                                        emailmodel.BCC = bcc;
                                        emailmodel.MessageId = message.MessageId;
                                        emailmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(emailmodel);
                                        var res = await _taskBusiness.ManageTask(emailmodel);
                                        if (res.IsSuccess)
                                        {
                                            foreach (var attachment in message.Attachments)
                                            {
                                                // download the attachment just like we did with the body
                                                var entity = attachment;


                                                var part = (MimeKit.MimePart)entity;

                                                // note: it's possible for this to be null, but most will specify a filename
                                                var fileName = part.FileName;
                                                var stream = new MemoryStream();
                                                //var path = Path.Combine(directory, fileName);

                                                // decode and save the content to a file
                                                //using (var stream = File.Create(path))
                                                part.Content.DecodeTo(stream);

                                                var Content = stream.ToArray();
                                                var saveattachment = await _fileBusiness.Create(new FileViewModel
                                                {
                                                    ContentByte = Content,
                                                    ContentType = attachment.ContentType.ToString(),
                                                    ContentLength = stream.Length,
                                                    FileName = fileName,
                                                    ReferenceTypeId = res.Item.TaskId,
                                                    ReferenceTypeCode = ReferenceTypeEnum.NTS_Task,
                                                    FileExtension = Path.GetExtension(fileName)
                                                }
                                                );
                                            }
                                        }
                                    }



                                }
                            }
                        }
                        }
                    }
              

            }
            catch (Exception ex)
            {

            }
        }

        static void Push(ICollection<HtmlTagId> stack, HtmlTagId id)
        {
            if (id != HtmlTagId.Unknown)
                stack.Add(id);
        }

        static void Pop(IList<HtmlTagId> stack, HtmlTagId id)
        {
            if (id == HtmlTagId.Unknown)
                return;

            for (int i = stack.Count; i > 0; i--)
            {
                if (stack[i - 1] == id)
                {
                    stack.RemoveAt(i - 1);
                    return;
                }
            }
        }
        public async Task<List<MessageEmailViewModel>> ReceiveEmailInbox(string id,int skip,int take)
        {
           // var emailSetup = await _projectEmailBusiness.ReadEmailSetupUsers(); // reading all the user of the Email Setup
            var messagemodel = new List<MessageEmailViewModel>();
            var client = new ImapClient();
            int TotalCount = 0;
 
 
                    var config = await GetUserInobxEmailConfig(_userContext.UserId);
                    client.Connect(config.SmtpHost, 993, true);
                    client.Authenticate(config.SmtpUserId, config.SmtpPassword);
            
                try
                {
                    var inbox = client.Inbox;
                
                    inbox.Open(FolderAccess.ReadOnly);
                    if (id == "INBOX")
                    {                        
                        TotalCount = inbox.Count;
                        int count = TotalCount - 1;
                        int skipCount = count - skip;
                        int takecount = skipCount - 10;
                        for (int i = skipCount; i >= takecount; i--)
                        {
                            var message = inbox.GetMessage(i);
                        
                        var bcc = "";
                            var cc = "";
                             var to = "";
                             var from = "";
                            var fromuserid = "";
                            var messageid = "";
                        var AttachmentId = "";
                            bool taskcreated = false;
                            string taskid=string.Empty;
                            bool isusercreated = false;
                            var existtask = await GetTaskMessageId(message.MessageId);
                            if (existtask.IsNotNull())
                            {
                                messageid = existtask.MessageId;
                                taskid = existtask.TaskId;
                            }


                      
                        if (message.InReplyTo != null)
                        {
                           
                                var r = message.TextBody;
                                var refid= r.IndexOf("#SReferenceId_");
                            if (refid.IsNotNull())
                            {
                                taskcreated = true;
                            }
                        }
                        else
                        {
                            taskcreated = false;
                        }

                        
                            if (message.Bcc.Count > 0)
                            {
                                foreach (var item in message.Bcc)
                                {
                                    bcc += item.Name + ",";
                                }

                            }
                            if (message.Cc.Count > 0)
                            {
                                foreach (var item in message.Cc)
                                {
                                    cc += item.Name + ",";
                                }
                            }
                            foreach (var a in message.To.Mailboxes)
                            {
                                to += a.Address + ",";
                            }
                        foreach (var a in message.From.Mailboxes)
                        {
                            from += a.Address + ",";
                        }
                        var fromuser = await _userBusiness.GetSingle(x => x.Email == from);
                        if (fromuser.IsNotNull())
                        {
                            fromuserid = fromuser.Id;
                        }
                        else
                        {
                            fromuserid = _userContext.UserId;
                        }
                        messagemodel.Add(new MessageEmailViewModel()
                            {
                                To = to,
                                From = from,
                                Subject = message.Subject,
                                Bcc = bcc,
                                Cc = cc,
                                Body = message.HtmlBody,
                                MessageId = message.MessageId,
                                IsTaskCreated=taskcreated,
                                // ToUserId=touserid,
                                TaskId = taskid,
                                EmailType=id,
                                FromUserId=fromuserid,
                                AttachmentIds=AttachmentId,
                                Total= TotalCount,

                        }

                           );
                        }
                        // Get the first personal namespace and list the toplevel folders under it.
                        // var personal = client.GetFolder(client.PersonalNamespaces[0]);
                    }
                    // The Inbox folder is always available on all IMAP servers...
                    else
                    {
                        var personal = client.GetFolder(client.PersonalNamespaces[0]);
                        foreach (var folder in personal.GetSubfolders(false))
                        {
                            Console.WriteLine("[folder] {0}", folder.Name);

                                foreach (var subfolder in folder.GetSubfolders(false))
                                {
                                    Console.WriteLine("[subfolder] {0}", subfolder.Name);
                                    if ((subfolder.Name == "Sent Mail" && id == "Sent Mail") || (subfolder.Name == "Drafts" && id == "Drafts"))
                                    {
                                        subfolder.Status(StatusItems.Count | StatusItems.Unread);

                                        TotalCount = subfolder.Count;
                                        Console.WriteLine("Unread: {0}", subfolder.Unread);
                                        subfolder.Open(FolderAccess.ReadWrite);

                                        for (int i = 0; i < TotalCount; i++)
                                        {
                                            var message = subfolder.GetMessage(i);
                                            var bcc = "";
                                            var cc = "";
                                             var to = "";
                                             var from = "";
                                            //var touserid = "";
                                            var messageid = "";
                                            var taskid = "";
                                            //bool isusercreated = false;
                                           // var existtask = await GetTaskMessageId(message.MessageId);
                                           
                                            if (message.Bcc.Count > 0)
                                            {
                                                foreach (var item in message.Bcc)
                                                {
                                                    bcc += item.Name + ",";
                                                }

                                            }
                                            if (message.Cc.Count > 0)
                                            {
                                                foreach (var item in message.Cc)
                                                {
                                                    cc += item.Name + ",";
                                                }
                                            }
                                        foreach (var a in message.To.Mailboxes)
                                        {
                                            to += a.Address + ",";
                                        }
                                    foreach (var a in message.From.Mailboxes)
                                    {
                                        from += a.Address + ",";
                                    }
                                    messagemodel.Add(new MessageEmailViewModel()
                                            {
                                                To = to,
                                                From = from,
                                                Subject = message.Subject,
                                                Bcc = bcc,
                                                Cc = cc,
                                                Body = message.HtmlBody,
                                                MessageId = message.MessageId,
                                                // ToUserId=touserid,
                                                TaskId = taskid
                                            }

                                           );
                                        }
                                        break;
                                    }
                                }
                        
                         
                            
                        }

                    }             
                   
                    client.Disconnect(true);
                }
                catch (Exception ex)
                {

                }

    
                    return messagemodel;
                       
        }
        public async Task<List<MessageEmailViewModel>> SearchEmailInbox(string search)
        {
            // var emailSetup = await _projectEmailBusiness.ReadEmailSetupUsers(); // reading all the user of the Email Setup
            var messagemodel = new List<MessageEmailViewModel>();
            var client = new ImapClient();
            int TotalCount = 0;


            var config = await GetUserInobxEmailConfig(_userContext.UserId);
            client.Connect(config.SmtpHost, 993, true);
            client.Authenticate(config.SmtpUserId, config.SmtpPassword);

            try
            {
                var inbox = client.Inbox;

                inbox.Open(FolderAccess.ReadOnly);
                var query = SearchQuery.SubjectContains(search).Or(SearchQuery.FromContains(search)).Or(SearchQuery.ToContains(search));

                foreach (var uid in inbox.Search(query))
                {
                    
                        var message = inbox.GetMessage((uid));

                        var bcc = "";
                        var cc = "";
                        var to = "";
                        var from = "";
                        var fromuserid = "";
                        var messageid = "";
                        var AttachmentId = "";
                        bool taskcreated = false;
                        string taskid = string.Empty;
                        bool isusercreated = false;
                        var existtask = await GetTaskMessageId(message.MessageId);
                        if (existtask.IsNotNull())
                        {
                            messageid = existtask.MessageId;
                            taskid = existtask.TaskId;
                        }



                        if (message.InReplyTo != null)
                        {

                            var r = message.TextBody;
                            var refid = r.IndexOf("#SReferenceId_");
                            if (refid.IsNotNull())
                            {
                                taskcreated = true;
                            }
                        }
                        else
                        {
                            taskcreated = false;
                        }


                        if (message.Bcc.Count > 0)
                        {
                            foreach (var item in message.Bcc)
                            {
                                bcc += item.Name + ",";
                            }

                        }
                        if (message.Cc.Count > 0)
                        {
                            foreach (var item in message.Cc)
                            {
                                cc += item.Name + ",";
                            }
                        }
                        foreach (var a in message.To.Mailboxes)
                        {
                            to += a.Address + ",";
                        }
                        foreach (var a in message.From.Mailboxes)
                        {
                            from += a.Address + ",";
                        }
                        var fromuser = await _userBusiness.GetSingle(x => x.Email == from);
                        if (fromuser.IsNotNull())
                        {
                            fromuserid = fromuser.Id;
                        }
                        else
                        {
                            fromuserid = _userContext.UserId;
                        }
                        messagemodel.Add(new MessageEmailViewModel()
                        {
                            To = to,
                            From = from,
                            Subject = message.Subject,
                            Bcc = bcc,
                            Cc = cc,
                            Body = message.HtmlBody,
                            MessageId = message.MessageId,
                            IsTaskCreated = taskcreated,
                            // ToUserId=touserid,
                            TaskId = taskid,
                            EmailType = "INBOX",
                            FromUserId = fromuserid,
                            AttachmentIds = AttachmentId,
                            Total = TotalCount,

                        }

                           );
                    }
                    // Get the first personal namespace and list the toplevel folders under it.
                    // var personal = client.GetFolder(client.PersonalNamespaces[0]);
                

                client.Disconnect(true);
            }
            catch (Exception ex)
            {

            }


            return messagemodel;

        }
        public async Task<List<MessageEmailViewModel>> ReceiveEmailCompanyInbox(string id)
        {          
            var messagemodel = new List<MessageEmailViewModel>();
            var emailmodel = new EmailViewModel();
            var client = new ImapClient();
            long TotalCount = 0;
            var emailCon = await GetEmailConfig(emailmodel);
                    //var emailCon = new EmailProperties
                    //{
                    //    FromEmailId = "Info@extranet.ae",
                    //    SenderName = "Info@extranet.ae",
                    //    SmtpHost = "smtp.office365.com",
                    //    SmtpPort = 587,
                    //    SmtpUserId = "Info@extranet.ae",
                    //    SmtpPassword = "!Welcome123",

                    //    SmtpClient = new SmtpClient
                    //    {
                    //        Host = "smtp.office365.com",
                    //        Port = 587,
                    //        EnableSsl = true,
                    //        UseDefaultCredentials = false,
                    //        Credentials = new System.Net.NetworkCredential("Info@extranet.ae", "!Welcome123")

                    //    }
                    //};

                    client.Connect(emailCon.SmtpHost, 993, true);
                    client.Authenticate(emailCon.SmtpUserId, emailCon.SmtpPassword);
 
                try
                {
                    var inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadOnly);
                var today = DateTime.Now;
                var StartDate = new DateTime(today.Year, today.Month, 1);
                if (id == "INBOX")
                    {
                    // TotalCount = inbox.Count;
                    
                    var uids = client.Inbox.Search(SearchQuery.SentSince(StartDate));
                    TotalCount = uids.Count;
                   // for (int i = 0; i < TotalCount; i++)
                    foreach (var i in uids)
                    {
                        var message = inbox.GetMessage(i);
                        var bcc = "";
                        var cc = "";
                        var to = "";
                        var from = "";
                        var fromuserid = "";
                        var messageid = "";
                        bool taskcreated = false;
                        string taskid = string.Empty;
                        bool isusercreated = false;
                        var existtask = await GetTaskMessageId(message.MessageId);
                        if (existtask.IsNotNull())
                        {
                            messageid = existtask.MessageId;
                            taskid = existtask.TaskId;
                        }
                        if (message.InReplyTo != null)
                        {

                            var r = message.TextBody;
                            if (r != null)
                            {
                                var refid = r.IndexOf("#SReferenceId_");
                                if (refid.IsNotNull())
                                {
                                    taskcreated = true;
                                }
                            }
                        }
                        else
                        {
                            taskcreated = false;
                        }


                        if (message.Bcc.Count > 0)
                        {
                            foreach (var item in message.Bcc)
                            {
                                bcc += item.Name + ",";
                            }

                        }
                        if (message.Cc.Count > 0)
                        {
                            foreach (var item in message.Cc)
                            {
                                cc += item.Name + ",";
                            }
                        }
                        foreach (var a in message.To.Mailboxes)
                        {
                            to += a.Address + ",";
                        }
                        foreach (var a in message.From.Mailboxes)
                        {
                            from += a.Address + ",";
                        }
                        var fromuser = await _userBusiness.GetSingle(x => x.Email == from);
                        if (fromuser.IsNotNull())
                        {
                            fromuserid = fromuser.Id;
                        }
                        else
                        {
                            fromuserid = _userContext.UserId;
                        }
                        messagemodel.Add(new MessageEmailViewModel()
                        {
                            To = to,
                            From = from,
                            Subject = message.Subject,
                            Bcc = bcc,
                            Cc = cc,
                            Body = message.HtmlBody,
                            MessageId = message.MessageId,
                            IsTaskCreated = taskcreated,
                            // ToUserId=touserid,
                            TaskId = taskid,
                            EmailType = id,
                            FromUserId = fromuserid
                        }

                           );
                    }
                    // Get the first personal namespace and list the toplevel folders under it.
                    // var personal = client.GetFolder(client.PersonalNamespaces[0]);
                }
                    // The Inbox folder is always available on all IMAP servers...
                    else
                    {
                        var personal = client.GetFolder(client.PersonalNamespaces[0]);
                        foreach (var folder in personal.GetSubfolders(false))
                        {
                            Console.WriteLine("[folder] {0}", folder.Name);


                                if ((folder.Name == "Sent Items" && id == "Sent Items") || (folder.Name == "Drafts" && id == "Drafts"))
                                {


                                    folder.Status(StatusItems.Count | StatusItems.Unread);
                            TotalCount = folder.Count;
                                    Console.WriteLine("Unread: {0}", folder.Unread);
                                    folder.Open(FolderAccess.ReadWrite);
                            var uids = folder.Search(SearchQuery.SentSince(StartDate));
                            long TotaltodayCount = uids.Count;
                            // for (int i = 0; i < TotalCount; i++)
                            foreach (var i in uids)
                                    {
                                        var message = folder.GetMessage(i);
                                        var bcc = "";
                                        var cc = "";
                                         var to = "";
                                         var from = "";
                                        //var touserid = "";
                                        var messageid = "";
                                        var taskid = "";
                                        //bool isusercreated = false;
                                        var existtask = await GetTaskMessageId(message.MessageId);
                                        if (existtask.IsNotNull())
                                        {
                                            messageid = existtask.MessageId;
                                            taskid = existtask.TaskId;
                                        }
                                        if (message.Bcc.Count > 0)
                                        {
                                            foreach (var item in message.Bcc)
                                            {
                                                bcc += item.Name + ",";
                                            }

                                        }
                                        if (message.Cc.Count > 0)
                                        {
                                            foreach (var item in message.Cc)
                                            {
                                                cc += item.Name + ",";
                                            }
                                        }
                                    foreach (var a in message.To.Mailboxes)
                                    {
                                        to += a.Address + ",";
                                    }
                                foreach (var a in message.From.Mailboxes)
                                {
                                    from += a.Address + ",";
                                }
                                messagemodel.Add(new MessageEmailViewModel()
                                        {
                                            To =to,
                                            From = from,
                                            Subject = message.Subject,
                                            Bcc = bcc,
                                            Cc = cc,
                                            Body = message.HtmlBody,
                                            MessageId = message.MessageId,
                                            // ToUserId=touserid,
                                            TaskId = taskid
                                        }

                                       );
                                    }
                                }
                            

                        }

                    }

                    client.Disconnect(true);
                }
                catch (Exception ex)
                {

                }


            return messagemodel;

        }
        public async Task<List<MessageEmailViewModel>> ReceiveEmailProjectInbox(string id,string projectid)
        {
           var emailSetup = await _projectEmailBusiness.GetSingle(x=>x.ServiceId==projectid); // reading all the user of the Email Setup
            var messagemodel = new List<MessageEmailViewModel>();
            var client = new ImapClient();
            long TotalCount = 0;
            if (emailSetup.IsNotNull())
            {

                var config = await GetUserInobxEmailConfig(emailSetup.UserId);
                client.Connect(config.SmtpHost, 993, true);
                client.Authenticate(config.SmtpUserId, config.SmtpPassword);
                try
                {
                    var inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadOnly);
                    if (id == "INBOX")
                    {
                        TotalCount = inbox.Count;
                        for (int i = 0; i < TotalCount; i++)
                        {
                            var message = inbox.GetMessage(i);
                            var bcc = "";
                            var cc = "";
                            var to = "";
                            var from = "";
                            var fromuserid = "";
                            var messageid = "";
                            bool taskcreated = false;
                            string taskid = string.Empty;
                            bool isusercreated = false;
                            var existtask = await GetTaskMessageId(message.MessageId);
                            if (existtask.IsNotNull())
                            {
                                messageid = existtask.MessageId;
                                taskid = existtask.TaskId;
                            }
                            if (message.InReplyTo != null)
                            {

                                var r = message.TextBody;
                                var refid = r.IndexOf("#SReferenceId_");
                                if (refid.IsNotNull())
                                {
                                    taskcreated = true;
                                }
                            }
                            else
                            {
                                taskcreated = false;
                            }


                            if (message.Bcc.Count > 0)
                            {
                                foreach (var item in message.Bcc)
                                {
                                    bcc += item.Name + ",";
                                }

                            }
                            if (message.Cc.Count > 0)
                            {
                                foreach (var item in message.Cc)
                                {
                                    cc += item.Name + ",";
                                }
                            }
                            foreach (var a in message.To.Mailboxes)
                            {
                                to += a.Address + ",";
                            }
                            foreach (var a in message.From.Mailboxes)
                            {
                                from += a.Address + ",";
                            }
                            var fromuser = await _userBusiness.GetSingle(x => x.Email == from);
                            if (fromuser.IsNotNull())
                            {
                                fromuserid = fromuser.Id;
                            }
                            else
                            {
                                fromuserid = _userContext.UserId;
                            }
                            messagemodel.Add(new MessageEmailViewModel()
                            {
                                To = to,
                                From = from,
                                Subject = message.Subject,
                                Bcc = bcc,
                                Cc = cc,
                                Body = message.HtmlBody,
                                MessageId = message.MessageId,
                                IsTaskCreated = taskcreated,
                                // ToUserId=touserid,
                                TaskId = taskid,
                                EmailType = id,
                                FromUserId = fromuserid
                            }

                               );
                        }
                        // Get the first personal namespace and list the toplevel folders under it.
                        // var personal = client.GetFolder(client.PersonalNamespaces[0]);
                    }
                    // The Inbox folder is always available on all IMAP servers...
                    else
                    {
                        var personal = client.GetFolder(client.PersonalNamespaces[0]);
                        foreach (var folder in personal.GetSubfolders(false))
                        {
                            Console.WriteLine("[folder] {0}", folder.Name);

                            foreach (var subfolder in folder.GetSubfolders(false))
                            {
                                Console.WriteLine("[subfolder] {0}", subfolder.Name);
                                if ((subfolder.Name == "Sent Mail" && id == "Sent Mail") || (subfolder.Name == "Drafts" && id == "Drafts"))
                                {
                                    subfolder.Status(StatusItems.Count | StatusItems.Unread);

                                    TotalCount = subfolder.Count;
                                    Console.WriteLine("Unread: {0}", subfolder.Unread);
                                    subfolder.Open(FolderAccess.ReadWrite);

                                    for (int i = 0; i < TotalCount; i++)
                                    {
                                        var message = subfolder.GetMessage(i);
                                        var bcc = "";
                                        var cc = "";
                                        var to = "";
                                        var from = "";
                                        //var touserid = "";
                                        var messageid = "";
                                        var taskid = "";
                                        //bool isusercreated = false;
                                        var existtask = await GetTaskMessageId(message.MessageId);
                                        if (existtask.IsNotNull())
                                        {
                                            messageid = existtask.MessageId;
                                            taskid = existtask.TaskId;
                                        }
                                        if (message.Bcc.Count > 0)
                                        {
                                            foreach (var item in message.Bcc)
                                            {
                                                bcc += item.Name + ",";
                                            }

                                        }
                                        if (message.Cc.Count > 0)
                                        {
                                            foreach (var item in message.Cc)
                                            {
                                                cc += item.Name + ",";
                                            }
                                        }
                                        foreach (var a in message.To.Mailboxes)
                                        {
                                            to += a.Address + ",";
                                        }
                                        foreach (var a in message.From.Mailboxes)
                                        {
                                            from += a.Address + ",";
                                        }
                                        messagemodel.Add(new MessageEmailViewModel()
                                        {
                                            To = to,
                                            From = from,
                                            Subject = message.Subject,
                                            Bcc = bcc,
                                            Cc = cc,
                                            Body = message.HtmlBody,
                                            MessageId = message.MessageId,
                                            // ToUserId=touserid,
                                            TaskId = taskid
                                        }

                                           );
                                    }
                                    break;
                                }
                            }



                        }

                    }

                    client.Disconnect(true);
                }
                catch (Exception ex)
                {

                }
            }



            return messagemodel;

        }
        public async Task<EmailSetupViewModel> GetUserEmailConfig(string userId, string projectId)
        {
            var details = new ProjectEmailSetupViewModel();
            if (projectId.IsNotNullAndNotEmpty())
            {
                var data = await _projectEmailBusiness.ReadEmailSetupByProjectId(projectId);
                details = data.Where(x => x.UserId == userId).FirstOrDefault();
            }
            else
            {
                details = await _projectEmailBusiness.GetSingle(x => x.UserId == userId);
            }
            var decryptPass = Helper.Decrypt(details.SmtpPassword);
            var emailConfig = new EmailSetupViewModel
            {
                FromEmailId = details.SmtpFromId,
                SenderName = details.UserName,
                SmtpHost = details.SmtpHost,
                SmtpPort = details.SmtpPort,
                SmtpUserId = details.SmtpUserId,
                SmtpPassword = decryptPass,

                SmtpClient = new SmtpClient
                {
                    Host = details.SmtpHost,
                    Port = details.SmtpPort,
                    EnableSsl = true,
                    Credentials = new System.Net.NetworkCredential(details.SmtpUserId, details.SmtpPassword)

                }
            };
            return emailConfig;
        }

        public async Task<bool> TestEmailSetupMail(string Id)
        {

            var details = await _projectEmailBusiness.GetSingleById(Id);
            var config = new EmailSetupViewModel
            {
                FromEmailId = details.SmtpFromId,
                SenderName = details.UserName,
                SmtpHost = details.SmtpHost,
                SmtpPort = details.SmtpPort,
                SmtpUserId = details.SmtpUserId,
                SmtpPassword = details.SmtpPassword,

                SmtpClient = new SmtpClient
                {
                    Host = details.SmtpHost,
                    Port = details.SmtpPort,
                    EnableSsl = true,
                    Credentials = new System.Net.NetworkCredential(details.SmtpUserId, details.SmtpPassword)

                }
            };

            try
            {
                var client = new ImapClient();
                client.Connect(config.SmtpHost, 993, true);
                client.Authenticate(config.SmtpUserId, config.SmtpPassword);
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);
                var uids = client.Inbox.Search(SearchQuery.SentOn(DateTime.Now));
                long TotalCount = uids.Count;
                foreach (var uid in uids)
                {
                    //var message = client.GetMessage(msgCount);
                    var message = inbox.GetMessage(uid);
                    if (message.Subject == "Test Synergy Email Setup")
                    {
                        return true;
                    }
                   
                }
                // }

            }
            catch (Exception ex)
            {

            }

            return false;
        }

        public async Task<EmailSetupViewModel> GetUserInobxEmailConfig(string userId)
        {
            var details = await _projectEmailBusiness.GetSingle(x=>x.UserId==userId);
            // var details = data.Where(x => x.UserId == userId).FirstOrDefault();
            var decryptPass = Helper.Decrypt(details.SmtpPassword);
            var emailConfig = new EmailSetupViewModel
            {
                FromEmailId = details.SmtpFromId,
                SenderName = details.UserName,
                SmtpHost = details.SmtpHost,
                SmtpPort = details.SmtpPort,
                SmtpUserId = details.SmtpUserId,
                SmtpPassword = decryptPass,

                SmtpClient = new SmtpClient
                {
                    Host = details.SmtpHost,
                    Port = details.SmtpPort,
                    EnableSsl = true,
                    Credentials = new System.Net.NetworkCredential(details.SmtpUserId, decryptPass)

                }
            };
            return emailConfig;
        }

        private async Task<MessageEmailViewModel> GetTaskMessageId(string MessageId)
        {
            var query = @$"select nt.""Id"" as TaskId,pet.""MessageId"" as MessageId
                from public.""NtsTask"" as nt
                join public.""NtsNote"" as nn on nn.""Id""=nt.""UdfNoteId"" and nn.""IsDeleted""=false
                join cms.""N_GEN_GeneralEmailTask"" as pet on pet.""NtsNoteId""=nn.""Id"" and pet.""IsDeleted""=false

                where 
	            pet.""MessageId""='{MessageId}' and 
	            nt.""TemplateCode""='EMAIL_TASK' and nt.""IsDeleted""=false";
            var data = await _queryRepo.ExecuteQuerySingle<MessageEmailViewModel>(query, null);
            return data;
        }

        public async Task<IList<TreeViewViewModel>> GetInboxMenuItem(string id)
        {
            var list = new List<TreeViewViewModel>();
            long InboxCount = 0;
            var folderlist = new Dictionary<string,long>();
            //var client = new OpenPop.Pop3.Pop3Client();
            //var config = await GetUserInobxEmailConfig(_userContext.UserId);
            //if (config.IsNotNull())
            //{
            //    client.Connect(config.SmtpHost, 995, true);
            //    client.Authenticate(config.SmtpUserId, config.SmtpPassword);
            //    TotalCount = client.GetMessageCount();
            //}
            using (var client = new ImapClient())
            {
 
                    var config = await GetUserInobxEmailConfig(_userContext.UserId);
                    client.Connect(config.SmtpHost, 993, true);
                    client.Authenticate(config.SmtpUserId, config.SmtpPassword);



                // The Inbox folder is always available on all IMAP servers...
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);
                InboxCount = inbox.Count;
                Console.WriteLine("Total messages: {0}", inbox.Count);
                Console.WriteLine("Recent messages: {0}", inbox.Recent);                
                // Get the first personal namespace and list the toplevel folders under it.
                var personal = client.GetFolder(client.PersonalNamespaces[0]);
                foreach (var folder in personal.GetSubfolders(false))
                {
                    Console.WriteLine("[folder] {0}", folder.Name);
                    if (folder.Name == "INBOX")
                    {
                        folderlist.Add(folder.Name, InboxCount);
                    }
                   
                    foreach (var subfolder in folder.GetSubfolders(false))
                    {
                        Console.WriteLine("[subfolder] {0}", subfolder.Name);
                        if(subfolder.Name=="Sent Mail" || subfolder.Name == "Drafts")
                        {
                            subfolder.Status(StatusItems.Count | StatusItems.Unread);

                            var subfoldercount = subfolder.Count;
                            Console.WriteLine("Unread: {0}", subfolder.Unread);
                            folderlist.Add(subfolder.Name, subfoldercount);
                            // If we want to fetch any of the messages (or get any
                            // message metadata, we'll need to actually Open() the
                            // folder:
                            subfolder.Open(FolderAccess.ReadWrite);

                            for (int i = 0; i < subfolder.Count; i++)
                            {
                                var message = subfolder.GetMessage(i);
                                Console.WriteLine("Subject: {0}", message.Subject);
                            }
                        }
                       

                    }
                }
                client.Disconnect(true);
            }
            if (id.IsNullOrEmpty())
            {
                foreach(KeyValuePair<string, long> item in folderlist)
                {
                    var treeitem = new TreeViewViewModel
                    {
                        id = item.Key,
                        Name = item.Key + "(" + item.Value + ")",
                        DisplayName = "Inbox",
                        ParentId = null,
                        hasChildren = false,
                        expanded = true,
                        Type = item.Key,
                        text = item.Key + "(" + item.Value + ")",
                        children = false,
                        parent = "#"
                    };
                    list.Add(treeitem);
                } 
                        
            }
          
           
            
            return list;
        }

        public async Task<IList<TreeViewViewModel>> GetInboxMenuItemCompany(string id)
        {
            var list = new List<TreeViewViewModel>();
            var emailmodel = new EmailViewModel();
            long InboxCount = 0;
            var folderlist = new Dictionary<string, long>();
            var today = DateTime.Now;
            var StartDate = new DateTime(today.Year, today.Month, 1);
            using (var client = new ImapClient())
            {

                //var emailCon = new EmailProperties
                //{
                //    FromEmailId = "Info@extranet.ae",
                //    SenderName = "Info@extranet.ae",
                //    SmtpHost = "smtp.office365.com",
                //    SmtpPort = 587,
                //    SmtpUserId = "Info@extranet.ae",
                //    SmtpPassword = "!Welcome123",

                //    SmtpClient = new SmtpClient
                //    {
                //        Host = "smtp.office365.com",
                //        Port = 587,
                //        EnableSsl = true,
                //        UseDefaultCredentials = false,
                //        Credentials = new System.Net.NetworkCredential("Info@extranet.ae", "!Welcome123")

                //    }
                //};
                var emailCon = await GetEmailConfig(emailmodel);

                client.Connect(emailCon.SmtpHost, 993, true);

                    client.Authenticate(emailCon.SmtpUserId, emailCon.SmtpPassword);


                // The Inbox folder is always available on all IMAP servers...
                //var inbox = client.Inbox;
                //inbox.Open(FolderAccess.ReadOnly);
                //InboxCount = inbox.Count;
                //Console.WriteLine("Total messages: {0}", inbox.Count);
                //Console.WriteLine("Recent messages: {0}", inbox.Recent);
                // Get the first personal namespace and list the toplevel folders under it.
                var personal = client.GetFolder(client.PersonalNamespaces[0]);
                foreach (var folder in personal.GetSubfolders(false))
                {
                    Console.WriteLine("[folder] {0}", folder.Name);
                    if (folder.Name == "INBOX" || folder.Name == "Sent Items" || folder.Name == "Drafts")
                    {
                        folder.Status(StatusItems.Count | StatusItems.Unread);
                        folder.Open(FolderAccess.ReadOnly);
                        var uids = folder.Search(SearchQuery.SentSince(StartDate));
                        long TotaltodayCount = uids.Count;
                        folderlist.Add(folder.Name, TotaltodayCount);
                    }

                    
                }
                client.Disconnect(true);
            }
            if (id.IsNullOrEmpty())
            {
                foreach (KeyValuePair<string, long> item in folderlist)
                {
                    var treeitem = new TreeViewViewModel
                    {
                        id = item.Key,
                        Name = item.Key + "(" + item.Value + ")",
                        DisplayName = "Inbox",
                        ParentId = null,
                        hasChildren = false,
                        expanded = true,
                        Type = item.Key,
                        text = item.Key + "(" + item.Value + ")",
                        children = false,
                        parent="#"
                    };
                    list.Add(treeitem);
                }

            }



            return list;
        }

        public async Task<IList<TreeViewViewModel>> GetInboxMenuItemProject(string id, string projectid)
        {
            var list = new List<TreeViewViewModel>();
            long InboxCount = 0;
            var folderlist = new Dictionary<string, long>();
            var projectsetup = await _projectEmailBusiness.GetSingle(x => x.ServiceId == projectid);
            if (projectsetup.IsNotNull() && projectid.IsNotNullAndNotEmpty())
            {
                using (var client = new ImapClient())
                {

                    var config = await GetUserInobxEmailConfig(projectsetup.UserId);
                    client.Connect(config.SmtpHost, 993, true);
                    client.Authenticate(config.SmtpUserId, config.SmtpPassword);



                    // The Inbox folder is always available on all IMAP servers...
                    var inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadOnly);
                    InboxCount = inbox.Count;
                    Console.WriteLine("Total messages: {0}", inbox.Count);
                    Console.WriteLine("Recent messages: {0}", inbox.Recent);
                    // Get the first personal namespace and list the toplevel folders under it.
                    var personal = client.GetFolder(client.PersonalNamespaces[0]);
                    foreach (var folder in personal.GetSubfolders(false))
                    {
                        Console.WriteLine("[folder] {0}", folder.Name);
                        if (folder.Name == "INBOX")
                        {
                            folderlist.Add(folder.Name, InboxCount);
                        }

                        foreach (var subfolder in folder.GetSubfolders(false))
                        {
                            Console.WriteLine("[subfolder] {0}", subfolder.Name);
                            if (subfolder.Name == "Sent Mail" || subfolder.Name == "Drafts")
                            {
                                subfolder.Status(StatusItems.Count | StatusItems.Unread);

                                var subfoldercount = subfolder.Count;
                                Console.WriteLine("Unread: {0}", subfolder.Unread);
                                folderlist.Add(subfolder.Name, subfoldercount);
                                // If we want to fetch any of the messages (or get any
                                // message metadata, we'll need to actually Open() the
                                // folder:
                                subfolder.Open(FolderAccess.ReadWrite);

                                for (int i = 0; i < subfolder.Count; i++)
                                {
                                    var message = subfolder.GetMessage(i);
                                    Console.WriteLine("Subject: {0}", message.Subject);
                                }
                            }


                        }
                    }
                    client.Disconnect(true);
                }
                if (id.IsNullOrEmpty())
                {
                    foreach (KeyValuePair<string, long> item in folderlist)
                    {
                        var treeitem = new TreeViewViewModel
                        {
                            id = item.Key,
                            Name = item.Key + "(" + item.Value + ")",
                            DisplayName = "Inbox",
                            ParentId = null,
                            hasChildren = false,
                            expanded = true,
                            Type = item.Key,
                            text= item.Key + "(" + item.Value + ")",
                            children= false,
                            parent="#"
                        };
                        list.Add(treeitem);
                    }

                }

            }

            return list;
        }

        public async Task<CommandResult<EmailViewModel>> SendCalendarMail(EmailViewModel cEmail)
        {
            await ReplaceCalendarVariables(cEmail);

            var config = await GetEmailConfig();

            var mailMessage = await GetCalendarMailMessage(cEmail, config);
            //var mailMessage = await GetMailMessage(cEmail, config);
            try
            {
                //SetEnvironmentForCalendar(mailMessage, cEmail);
                SetEnvironment(mailMessage, cEmail);

                config.SmtpClient.Send(mailMessage);
                cEmail.EmailStatus = NotificationStatusEnum.Sent;
                cEmail.Source = "Calendar";
            }
            catch (Exception ex)
            {
                cEmail.EmailStatus = NotificationStatusEnum.Error;
                cEmail.Error = ex.ToString();
                return CommandResult<EmailViewModel>.Instance(cEmail, false, /*"Error",*/ cEmail.Error);
            }
            finally
            {
                await ManageEmail(cEmail, config, mailMessage);
                if (mailMessage != null)
                {
                    mailMessage.Dispose();
                }
            }
            return CommandResult<EmailViewModel>.Instance(cEmail);

        }

        private async Task ReplaceCalendarVariables(EmailViewModel cEmail)
        {
            if (cEmail.SlotId != null)
            {
                //var ab = BusinessHelper.GetInstance<ITalentAssessmentBusiness>();
                var result = await _talentAssessmentBusiness.GetCalendarScheduleList();
                var slot = result.Where(x => x.Id == cEmail.SlotId).FirstOrDefault();
                if (slot != null)
                {
                    cEmail.Body = cEmail.Body.HtmlDecode().Replace("^^MEETING_LINK^^", slot.Url);
                    cEmail.Body = cEmail.Body.HtmlDecode().Replace("^^MEETING_DATE^^", cEmail.StartDate.ToDefaultDateFormat());
                    cEmail.Body = cEmail.Body.HtmlDecode().Replace("^^MEETING_START_TIME^^", cEmail.StartDate.Value.ToString("hh:mm"));
                    cEmail.Body = cEmail.Body.HtmlDecode().Replace("^^MEETING_END_TIME^^", cEmail.EndDate.Value.ToString("hh:mm"));
                    cEmail.Body = cEmail.Body.HtmlDecode().Replace("^^MEETING_LOCATION^^", slot.Location);

                }
            }
            cEmail.Body = cEmail.Body.HtmlDecode().Replace("^^MEETING_SUBJECT^^", cEmail.Subject);
        }

        public async Task<EmailProperties> GetEmailConfig()
        {
            var company = await _companyBusiness.GetSingle(x => x.Id == _repo.UserContext.CompanyId);

            var emailConfig = new EmailProperties
            {
                FromEmailId = company.SmtpFromId,
                SenderName = company.SmtpSenderName,
                SmtpHost = company.SmtpHost,
                SmtpPort = company.SmtpPort,
                SmtpUserId = company.SmtpUserId,
                //SmtpPassword = company.SmtpPassword,
                SmtpPassword = Helper.Decrypt(company.SmtpPassword),

                SmtpClient = new SmtpClient
                {
                    Host = company.SmtpHost,
                    Port = company.SmtpPort,
                    EnableSsl = true,
                    //Credentials = new System.Net.NetworkCredential(company.SmtpUserId, company.SmtpPassword),
                    Credentials = new System.Net.NetworkCredential(company.SmtpUserId, Helper.Decrypt(company.SmtpPassword))

                }
            };
            return emailConfig;
        }


        public async Task<MessageEmailViewModel> ReceiveEmailById(string id)
        {
            // var emailSetup = await _projectEmailBusiness.ReadEmailSetupUsers(); // reading all the user of the Email Setup
            var messagemodel = new MessageEmailViewModel();
            var client = new ImapClient();
            long TotalCount = 0;


            var config = await GetUserInobxEmailConfig(_userContext.UserId);
            client.Connect(config.SmtpHost, 993, true);
            client.Authenticate(config.SmtpUserId, config.SmtpPassword);

            try
            {
                var inbox = client.Inbox;

                inbox.Open(FolderAccess.ReadOnly);
             //   if (id == "INBOX")
                {
                    TotalCount = inbox.Count;
                    for (int i = 0; i < TotalCount; i++)
                    {
                        var message = inbox.GetMessage(i);

                        var bcc = "";
                        var cc = "";
                        var to = "";
                        var from = "";
                        var fromuserid = "";
                        var messageid = "";
                        var AttachmentId = "";
                        bool taskcreated = false;
                        string taskid = string.Empty;
                        bool isusercreated = false;


                        if (id == message.MessageId)
                        {
                            var existtask = await GetTaskMessageId(message.MessageId);

                            //    if (message.Attachments.Count() > 0)

                            {



                                var attachments = message.BodyParts.OfType<MimePart>().Where(part => !string.IsNullOrEmpty(part.FileName));

                                foreach (MimePart atch in attachments)
                                {
                                    using (var memory = new MemoryStream())
                                    {
                                        atch.Content.DecodeTo(memory);
                                        var buffer = memory.ToArray();
                                        //var text = Encoding.UTF8.GetString(buffer);


                                        var result = await _fileBusiness.Create(new FileViewModel
                                        {
                                            ContentByte = buffer,
                                            ContentType = atch.ContentType.MimeType,
                                            ContentLength = buffer.Length,
                                            FileName = atch.FileName,
                                            FileExtension = Path.GetExtension(atch.FileName)
                                        });

                                        if (result.IsSuccess)
                                        {
                                            if (AttachmentId.IsNullOrEmpty())
                                            {
                                                AttachmentId = result.Item.Id;
                                            }
                                            else
                                            {
                                                AttachmentId += "," + result.Item.Id;
                                            }
                                        }
                                    }
                                }


                                //foreach (var attachment in message.Attachments)
                                //{
                                //
                                //    byte[] allBytes = new byte[attachment..Length];
                                //    int bytesRead = attachment.ContentStream.Read(allBytes, 0, (int)attachment.ContentStream.Length);
                                //
                                //    string destinationFile = @"C:\Download\" + attachment.Name;
                                //
                                //    BinaryWriter writer = new BinaryWriter(new FileStream(destinationFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None));
                                //    writer.Write(allBytes);
                                //    writer.Close();
                                //}
                            }

                            if (message.InReplyTo != null)
                            {

                                var r = message.TextBody;
                                var refid = r.IndexOf("#SReferenceId_");
                                if (refid.IsNotNull())
                                {
                                    taskcreated = true;
                                }
                            }
                            else
                            {
                                taskcreated = false;
                            }


                            if (message.Bcc.Count > 0)
                            {
                                foreach (var item in message.Bcc)
                                {
                                    bcc += item.Name + ",";
                                }

                            }
                            if (message.Cc.Count > 0)
                            {
                                foreach (var item in message.Cc)
                                {
                                    cc += item.Name + ",";
                                }
                            }
                            foreach (var a in message.To.Mailboxes)
                            {
                                to += a.Address + ",";
                            }
                            foreach (var a in message.From.Mailboxes)
                            {
                                from += a.Address + ",";
                            }
                            var fromuser = await _userBusiness.GetSingle(x => x.Email == from);
                            if (fromuser.IsNotNull())
                            {
                                fromuserid = fromuser.Id;
                            }
                            else
                            {
                                fromuserid = _userContext.UserId;
                            }
                            messagemodel = new MessageEmailViewModel()
                            {
                                To = to,
                                From = from,
                                Subject = message.Subject,
                                Bcc = bcc,
                                Cc = cc,
                                Body = message.HtmlBody,
                                MessageId = message.MessageId,
                                IsTaskCreated = taskcreated,
                                // ToUserId=touserid,
                                TaskId = taskid,
                                EmailType = id,
                                FromUserId = fromuserid,
                                AttachmentIds = AttachmentId

                            };
                            client.Disconnect(true);
                            return messagemodel;
                        }
                          
                    }
                    // Get the first personal namespace and list the toplevel folders under it.
                    // var personal = client.GetFolder(client.PersonalNamespaces[0]);
                }
                // The Inbox folder is always available on all IMAP servers...
             

                client.Disconnect(true);
            }
            catch (Exception ex)
            {

            }


            return messagemodel;

        }

        private async Task<MailMessage> GetCalendarMailMessage(EmailViewModel email, EmailProperties config)
        {
            if (email.DataAction == DataActionEnum.Create)
            {
                email.Id = null;
                email.SequenceNo = 2;
                email.EmailUniqueId = Guid.NewGuid().ToString();

            }
            email.CalendarInvitationType = email.CalendarInvitationType ?? CalendarInvitationTypeEnum.Create;
            var mailMessage = new MailMessage
            {
                IsBodyHtml = true,
                BodyEncoding = System.Text.Encoding.UTF8,
                SubjectEncoding = System.Text.Encoding.UTF8
            };

            var method = "REQUEST";
            var status = "CONFIRMED";
            switch (email.CalendarInvitationType.Value)
            {
                case CalendarInvitationTypeEnum.Update:
                    var latestEmail1 = await GetList(x => x.EmailUniqueId == email.EmailUniqueId);
                    var latestEmail = latestEmail1.Where(x => x.EmailUniqueId == email.EmailUniqueId).OrderBy(x => x.SequenceNo).FirstOrDefault();
                    if (latestEmail != null)
                    {
                        email.SequenceNo = latestEmail.SequenceNo + 1;
                    }

                    //var existingEmail = _repository.GetSingleById<GEN_Email>(email.EmailId);
                    //if (existingEmail != null)
                    //{
                    //    email.SequenceNo = existingEmail.SequenceNo + 1;
                    //    email.EmailUniqueId = existingEmail.EmailUniqueId;
                    //}
                    break;
                case CalendarInvitationTypeEnum.Cancel:
                    method = "CANCEL";
                    status = "CANCELLED";
                    break;
                case CalendarInvitationTypeEnum.Create:
                default:
                    break;
            }

            StringBuilder str = new StringBuilder();
            str.AppendLine("BEGIN:VCALENDAR");
            str.AppendLine("PRODID:-//Schedule a Meeting");
            str.AppendLine("VERSION:2.0");
            str.AppendLine(string.Format("METHOD:{0}", method));
            str.AppendLine("BEGIN:VEVENT");
            str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", email.StartDate.Value.ToUniversalTime()));
            str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.UtcNow));
            str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", email.EndDate.Value.ToUniversalTime()));
            str.AppendLine("LOCATION: " + "");
            str.AppendLine(string.Format("UID:{0}", email.EmailUniqueId));
            str.AppendLine(string.Format("SEQUENCE:{0}", email.SequenceNo));
            str.AppendLine(string.Format("X-MICROSOFT-CDO-APPT-SEQUENCE:{0}", email.SequenceNo));
            //str.AppendLine(string.Format("DESCRIPTION:{0}", email.Body));
            str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", email.Body));
            str.AppendLine(string.Format("SUMMARY:{0}", email.Subject));
            str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", config.FromEmailId));
            str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", email.RecipientName, email.To));
            str.AppendLine(string.Format("STATUS:", status));
            str.AppendLine("BEGIN:VALARM");
            str.AppendLine("TRIGGER:-PT15M");
            str.AppendLine("ACTION:DISPLAY");
            str.AppendLine("DESCRIPTION:Reminder");
            str.AppendLine("END:VALARM");
            str.AppendLine("END:VEVENT");
            str.AppendLine("END:VCALENDAR");
            System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType("text/calendar");
            contentType.Parameters.Add("method", method);
            contentType.Parameters.Add("name", "invitation.ics");
            byte[] bytes = Encoding.UTF8.GetBytes(str.ToString());
            MemoryStream stream = new MemoryStream(bytes);
            Attachment icsAttachment = new Attachment(stream, "invitation.ics", "text/calendar");
            mailMessage.From = new MailAddress(config.FromEmailId, config.SenderName);
            var toCollection = new MailAddressCollection();
            mailMessage.To.Add(email.To);
            if (email.CC.IsNotNullAndNotEmpty())
            {
                //var ccCollection = new MailAddressCollection();
                var ccList = email.CC.Split(';');
                foreach (var item in ccList)
                {
                    if (item.IsNotNullAndNotEmpty())
                    {
                        mailMessage.CC.Add(item);
                    }
                }
            }
            if (email.BCC.IsNotNullAndNotEmpty())
            {
                //var bccCollection = new MailAddressCollection();
                var bccList = email.BCC.Split(';');
                foreach (var item in bccList)
                {
                    if (item.IsNotNullAndNotEmpty())
                    {
                        mailMessage.Bcc.Add(item);
                    }
                }
            }
            mailMessage.Subject = email.Subject;
            mailMessage.Body = email.Body;
            mailMessage.Attachments.Add(icsAttachment);
            mailMessage.Headers.Add("Content-class", "urn:content-classes:calendarmessage");
            return mailMessage;
        }

        //private void SetEnvironmentForCalendar(MailMessage mailMessage, EmailViewModel email)
        //{
        //    if (Constant.AppSettings.ApplicationEnvironment != ApplicationEnvironmentEnum.PROD)
        //    {
        //        var testRecipients = Constant.AppSettings.TestEmailRecipients;

        //        if (testRecipients.IsNotNullAndNotEmpty())
        //        {
        //            AppendActualRecipientsInBody(mailMessage);
        //            mailMessage.To.Clear();
        //            mailMessage.CC.Clear();
        //            mailMessage.Bcc.Clear();
        //            var toList = testRecipients.Split(';');
        //            foreach (var item in toList)
        //            {
        //                if (item.IsNotNullAndNotEmpty())
        //                {
        //                    mailMessage.To.Add(item);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            throw new ArgumentNullException("Test Email Recipient is not added in web config");
        //        }
        //    };
        //}

        //private CommandResult<EmailViewModel> ManageEmail(EmailViewModel email, EmailProperties config, MailMessage mailMessage)
        //{
        //    if (email.Operation == DataOperation.Create)
        //    {
        //        email.CreatedBy = email.FromUserId;
        //        email.LastUpdatedBy = email.FromUserId;
        //        email.SmtpHost = config.SmtpClient.Host;
        //        email.SmtpPort = config.SmtpClient.Port;
        //        email.SmtpUserId = config.SmtpUserId;
        //        email.From = mailMessage.From.Address;
        //        email.SenderName = mailMessage.From.DisplayName;
        //        var data = BusinessHelper.MapModel<EmailViewModel, GEN_Email>(email);
        //        data.Id = 0;
        //        var result = _repository.Create(data);
        //        return CommandResult<EmailViewModel>.Instance(email, result.IsSuccess);
        //    }
        //    else if (email.Operation == DataOperation.Correct)
        //    {
        //        //var data = BusinessHelper.MapModel<EmailViewModel, GEN_Email>(email);
        //        //data.LastUpdatedDate = DateTime.Now.ApplicationNow();
        //        //data.LastUpdatedBy = Constant.WindowsServiceUserId;
        //        //var result = _repository.Edit(data);
        //        //return CommandResult<EmailViewModel>.Instance(email, result.IsSuccess);
        //        var item = _repository.GetSingle(x => x.Id == email.Id);
        //        if (item != null)
        //        {
        //            item.LastUpdatedDate = DateTime.Now.ApplicationNow();
        //            item.LastUpdatedBy = Constant.WindowsServiceUserId;
        //            item.RetryCount = email.RetryCount;
        //            item.EmailStatus = email.EmailStatus;
        //            item.StartDate = email.StartDate;
        //            item.EndDate = email.EndDate;
        //            item.CalendarInvitationType = email.CalendarInvitationType;
        //            item.Subject = email.Subject;
        //            item.Body = email.Body;
        //            item.SequenceNo = email.SequenceNo;
        //            var result = _repository.Edit(item);
        //            return CommandResult<EmailViewModel>.Instance(email, result.IsSuccess);
        //        }


        //    }
        //    return CommandResult<EmailViewModel>.Instance(email);
        //}

    }
}
