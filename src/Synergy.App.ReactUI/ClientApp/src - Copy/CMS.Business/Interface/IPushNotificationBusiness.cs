
using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMS.Business
{

    public interface IPushNotificationBusiness : IBusinessBase<NotificationViewModel, Notification>
    {
        Task SetAllNotificationRead(string userId);
        Task<IList<NotificationViewModel>> GetNotificationList(string userId, long count = 8);
      
        Task<IList<NotificationViewModel>> GetTaskNotificationList(string taskId, string userId, long count = 8);
        Task<IList<NotificationViewModel>> GetNoteNotificationList(string noteId, string userId, long count = 8);
        Task<CommandResult<NotificationViewModel>> UpdateAsRead(NotificationViewModel viewModel);
        Task SetAllTaskNotificationRead(string userId, string taskId);
        Task<IList<NotificationViewModel>> GetServiceNotificationList(string serviceId, string userId, long count = 8);
        //Task<string> GetTaskEmailSummary(UserViewModel user);
        Task<CommandResult<NotificationViewModel>> CreateSummaryMail(NotificationViewModel viewModel);
    }
}
