using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IEmailBusiness : IBusinessBase<EmailViewModel, Email>
    {
        Task<CommandResult<EmailViewModel>> SendMail(EmailViewModel email);
        Task<CommandResult<EmailViewModel>> SendMailAsync(EmailViewModel email);
        Task<CommandResult<EmailViewModel>> SendMail(string emailId);
        Task<CommandResult<EmailViewModel>> SendMailTask(EmailViewModel email);
        Task ReceiveMail();
        Task<List<MessageEmailViewModel>> ReceiveEmailInbox(string id,int skip,int taske);
        Task<List<MessageEmailViewModel>> SearchEmailInbox(string search);
        Task<List<MessageEmailViewModel>> ReceiveEmailCompanyInbox(string id);
        Task<List<MessageEmailViewModel>> ReceiveEmailProjectInbox(string id, string projectid);
        Task<IList<TreeViewViewModel>> GetInboxMenuItem(string id);
        Task<IList<TreeViewViewModel>> GetInboxMenuItemCompany(string id);
        Task<IList<TreeViewViewModel>> GetInboxMenuItemProject(string id, string projectid);
        Task<bool> TestEmailSetupMail(string Id);
        Task<CommandResult<EmailViewModel>> SendCalendarMail(EmailViewModel cEmail);
        Task<MessageEmailViewModel> ReceiveEmailById(string id);
    }
}
