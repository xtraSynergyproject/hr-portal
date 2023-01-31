using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class RecEmailBusiness : BusinessBase<EmailViewModel, Email>, IRecEmailBusiness
    {
        private ICompanyBusiness _companyBusiness;
        private IUserContext _userContext;
        private readonly IConfiguration _configuration;        
        private readonly IFileBusiness _fileBusiness;
        private readonly IRecTaskTemplateBusiness _templateBusiness;

        public RecEmailBusiness(IRepositoryBase<EmailViewModel, Email> repo, IMapper autoMapper, ICompanyBusiness companyBusiness,
            IUserContext userContext, IConfiguration configuration,IFileBusiness fileBusiness, IRecTaskTemplateBusiness templateBusiness
            ) : base(repo, autoMapper)
        {
            _companyBusiness = companyBusiness;
            _userContext = userContext;
            _configuration = configuration;            
            _fileBusiness = fileBusiness;
            _templateBusiness = templateBusiness;
        }

        public async override Task<CommandResult<EmailViewModel>> Create(EmailViewModel model)
        {

            var data = _autoMapper.Map<EmailViewModel>(model);
            
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<EmailViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Create(data);
            //if (model.UserIds != null)
            //{
            //    foreach (var id in model.UserIds)
            //    {

            //        var team = new TeamUserViewModel();
            //        if (model.TeamOwnerId == id)
            //        { team.IsTeamOwner = true; }
            //        else { team.IsTeamOwner = false; }
            //        team.TeamId = result.Item.Id;
            //        team.UserId = id;
            //        await _teamuserBusiness.Create(team);

            //    }
            //}
            if (result.IsSuccess)
            {
                model.Id = result.Item.Id;
            }
            return CommandResult<EmailViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


        public async override Task<CommandResult<EmailViewModel>> Edit(EmailViewModel model)
        {
            var errorList = new Dictionary<string, string>();
            var dmodel = await _repo.GetSingle(x => x.Id == model.Id);
            if(dmodel != null)
            {
                var result = await base.Edit(dmodel);
                return CommandResult<EmailViewModel>.Instance(dmodel, result.IsSuccess, result.Messages);
            }

            return CommandResult<EmailViewModel>.Instance(model, false, "Error");
        }
        private void SetEnvironment(MailMessage mailMessage, EmailViewModel email)
        {
            var env = _configuration.GetValue<string>("ApplicationEnvironment");
            if(env == "DEV")
            {
                var testRecipients = _configuration.GetValue<string>("TestEmailRecipients");

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

            } 
            else
            {
                    AppendActualRecipientsInBody(mailMessage);
                    mailMessage.To.Clear();
                    mailMessage.CC.Clear();
                    mailMessage.Bcc.Clear();
                    var toList = email.To.Split(';');
                    foreach (var item in toList)
                    {
                        if (item.IsNotNullAndNotEmpty())
                        {
                            mailMessage.To.Add(item.Trim());
                        }
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
        public async Task<CommandResult<EmailViewModel>> SendMail(EmailViewModel email)
        {
            var config = await PrepareEmailConfig(email);
            var mailMessage =await PrepareMailMessage(email, config);
            try
            {
                SetEnvironment(mailMessage, email);
                config.SmtpClient.Send(mailMessage);
                email.EmailStatus = NotificationStatusEnum.Sent;
                var result = await this.Edit(email);

            }
            catch (System.Exception ex)
            {
                email.EmailStatus = NotificationStatusEnum.Error;
                email.Error = ex.ToString();
                return CommandResult<EmailViewModel>.Instance(email, false, "Error");
            }
            finally
            {
                await ManageEmail(email, config, mailMessage);
                if (mailMessage != null)
                {
                    mailMessage.Dispose();
                }
            }
            return CommandResult<EmailViewModel>.Instance(email);

        }
        private async Task<CommandResult<EmailViewModel>> ManageEmail(EmailViewModel email, EmailProperties config, MailMessage mailMessage)
        {
            try
            {
                if (email.Operation == DataOperation.Create)
                {
                    email.CreatedBy = email.From;
                    email.LastUpdatedBy = email.From;
                    email.SmtpHost = config.SmtpClient.Host;
                    email.SmtpPort = config.SmtpClient.Port;
                    email.SmtpUserId = config.SmtpUserId;
                    email.From = mailMessage.From.Address;
                    email.To = email.To;
                    email.Id = null;
                    email.SenderName = mailMessage.From.DisplayName;
                    var result = await this.Create(email);
                    if (result.IsSuccess)
                    {
                        email.Id = result.Item.Id;
                    }
                    return CommandResult<EmailViewModel>.Instance(email, result.IsSuccess, result.Message);
                }
                else
                {
                    email.RetryCount = email.RetryCount + 1;
                    email.EmailStatus = email.EmailStatus;
                    var result = await base.Edit(email);
                    return CommandResult<EmailViewModel>.Instance(email, result.IsSuccess, result.Message);
                }
            }
            catch(Exception ex)
            {

            }
           
            return CommandResult<EmailViewModel>.Instance(email);
        }

        private MailMessage GetMailMessage(EmailViewModel email, EmailProperties config)
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
            return mailMessage;
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
                var task =await _repo.GetSingleById<RecTaskViewModel, RecTask>(email.ReferenceId);
                if (task.IsNotNull() && task.AttachmentCode1.IsNotNull())
                {
                    var file =await _fileBusiness.GetFileByte(task.AttachmentCode1);
                    mailMessage.Attachments.Add(new Attachment(new MemoryStream(file), task.AttachmentValue1));
                }
                if (task.IsNotNull() && task.AttachmentCode2.IsNotNull())
                {
                    var file =await _fileBusiness.GetFileByte(task.AttachmentCode2);
                    mailMessage.Attachments.Add(new Attachment(new MemoryStream(file), task.AttachmentValue2));
                }
                if (task.IsNotNull() && task.AttachmentCode3.IsNotNull())
                {
                    var file =await _fileBusiness.GetFileByte(task.AttachmentCode3);
                    mailMessage.Attachments.Add(new Attachment(new MemoryStream(file), task.AttachmentValue3));
                }
                if (task.IsNotNull() && task.AttachmentCode4.IsNotNull())
                {
                    var file =await _fileBusiness.GetFileByte(task.AttachmentCode4);
                    mailMessage.Attachments.Add(new Attachment(new MemoryStream(file), task.AttachmentValue4));
                }
                if (task.IsNotNull() && task.AttachmentCode5.IsNotNull())
                {
                    var file =await _fileBusiness.GetFileByte(task.AttachmentCode5);
                    mailMessage.Attachments.Add(new Attachment(new MemoryStream(file), task.AttachmentValue5));
                }
                if (task.IsNotNull() && task.AttachmentCode6.IsNotNull())
                {
                    var file =await _fileBusiness.GetFileByte(task.AttachmentCode6);
                    mailMessage.Attachments.Add(new Attachment(new MemoryStream(file), task.AttachmentValue6));
                }
                if (task.IsNotNull() && task.AttachmentCode7.IsNotNull())
                {
                    var file =await _fileBusiness.GetFileByte(task.AttachmentCode7);
                    mailMessage.Attachments.Add(new Attachment(new MemoryStream(file), task.AttachmentValue7));
                }
                if (task.IsNotNull() && task.AttachmentCode8.IsNotNull())
                {
                    var file =await _fileBusiness.GetFileByte(task.AttachmentCode8);
                    mailMessage.Attachments.Add(new Attachment(new MemoryStream(file), task.AttachmentValue8));
                }
                if (task.IsNotNull() && task.AttachmentCode9.IsNotNull())
                {
                    var file =await _fileBusiness.GetFileByte(task.AttachmentCode9);
                    mailMessage.Attachments.Add(new Attachment(new MemoryStream(file), task.AttachmentValue9));
                }
                if (task.IsNotNull() && task.AttachmentCode10.IsNotNull())
                {
                    var file =await _fileBusiness.GetFileByte(task.AttachmentCode10);
                    mailMessage.Attachments.Add(new Attachment(new MemoryStream(file), task.AttachmentValue10));
                }
            }
            return mailMessage;
        }

        public async Task<EmailProperties> GetEmailConfig(EmailViewModel email)
        {
            var company = new CompanyViewModel();
            if(_repo.UserContext!= null && _repo.UserContext.CompanyId.IsNotNullAndNotEmpty())
            {
                company = await _companyBusiness.GetSingleById(_repo.UserContext.CompanyId);

            }
            else
            {
                company = await _companyBusiness.GetSingleById(email.CompanyId);
            }
            var emailConfig = new EmailProperties();
            if (company != null)
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

            return emailConfig;
        }
        public async Task<EmailProperties> PrepareEmailConfig(EmailViewModel email)
        {
            var company = await _companyBusiness.GetSingleById(email.CompanyId);
            //if (_repo.UserContext != null && _repo.UserContext.CompanyId.IsNotNullAndNotEmpty())
            //{
            //    company = await _companyBusiness.GetSingleById(_repo.UserContext.CompanyId);

            //}
            //else
            //{
            //    company = await _companyBusiness.GetSingleById(email.CompanyId);
            //}
            var emailConfig = new EmailProperties();
            if (company != null)
            {
                if(company.SmtpHost.IsNullOrEmpty())
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
                    emailConfig = new EmailProperties
                    {
                        FromEmailId = company.SmtpFromId,
                        SenderName = company.SmtpSenderName,
                        SmtpHost = company.SmtpHost,
                        SmtpPort = company.SmtpPort,
                        SmtpUserId = company.SmtpUserId,
                        SmtpPassword = company.SmtpPassword,

                        SmtpClient = new SmtpClient
                        {
                            Host = company.SmtpHost,
                            Port = company.SmtpPort,
                            EnableSsl = true,
                            UseDefaultCredentials = false,
                            Credentials = new System.Net.NetworkCredential(company.SmtpFromId, company.SmtpPassword)

                        }

                    };
                }
            }
            else
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
            if (email.ShowOriginalSender)
            {
                var eid = await _templateBusiness.GetSingle(x => x.TemplateCode == email.ReferenceTemplateCode);
                if (eid.IsNotNull() && eid.EmailSettingId.IsNotNullAndNotEmpty())
                {
                    var emailSetting = await _templateBusiness.GetEmailSettingById(eid.EmailSettingId);
                    if (emailSetting.IsNotNull())
                    {
                        emailConfig = new EmailProperties
                        {
                            FromEmailId = emailSetting.SmtpUserId,
                            SenderName = emailSetting.SenderName,
                            SmtpHost = emailSetting.SmtpHost,
                            SmtpPort = emailSetting.SmtpPort,
                            SmtpUserId = emailSetting.SmtpUserId,
                            SmtpPassword = emailSetting.SmtpPassword,

                            SmtpClient = new SmtpClient
                            {
                                Host = "smtp.office365.com",
                                Port = 587,
                                EnableSsl = true,
                                UseDefaultCredentials = false,
                                Credentials = new System.Net.NetworkCredential(emailSetting.SmtpUserId, emailSetting.SmtpPassword)

                            }

                        };
                    }
                }
            }

            return emailConfig;
        }

        public async Task<CommandResult<EmailViewModel>> SendMailAsync(EmailViewModel email)
        {
            var config = await PrepareEmailConfig(email);            
            var mailMessage = GetMailMessage(email, config);
            try
            {
                email.IsAsync = true;
                email.EmailStatus = NotificationStatusEnum.NotSent;
                var result = await ManageEmail(email, config, mailMessage);
                if(result.IsSuccess)
                {
                    email.Id = result.Item.Id;
                }
                return CommandResult<EmailViewModel>.Instance(email, result.IsSuccess, result.Message);
            }
            catch (Exception)
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


    }
}
