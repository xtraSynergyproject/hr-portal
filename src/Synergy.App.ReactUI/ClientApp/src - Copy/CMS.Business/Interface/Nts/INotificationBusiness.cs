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
    public interface INotificationBusiness : IBusinessBase<NotificationViewModel, Notification>
    {
        Task<IList<NotificationViewModel>> GetNotificationList(string userId, string portalId, long count = 20, string referenceId = null, string id = null);
        Task<long> GetNotificationCount(string userId, string portalId);
        Task MarkNotificationAsRead(string id);
        Task MarkNotificationAsNotRead(string id);
        Task ArchiveNotification(string id);
        Task UnArchiveNotification(string id);
        Task<NotificationViewModel> GetNotificationDetails(string notificationId);
        Task<IList<NotificationViewModel>> GetAllNotifications(string userId, string portalId);
        Task<List<NotificationViewModel>> GetNotificationList(DateTime date, ReferenceTypeEnum? refType=null, bool read = false, bool archive = false, string refTypeId=null);
    }
}
