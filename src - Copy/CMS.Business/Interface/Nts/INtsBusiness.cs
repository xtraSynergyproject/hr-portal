using CMS.Common;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface INtsBusiness
    {
        Task<IList<TreeViewViewModel>> GetNtsMenuList(string id, string userId, string email);
        Task<IList<NTSMessageViewModel>> GetAttachedReplies(string userId, string taskId);
        Task UpdateOverdueNts(DateTime dateTime);
        Task DisbaleGrievenceReopenService(DateTime dateTime);
        Task CancelCommunityHallBookingOnExpired(DateTime dateTime);
        Task UpdateNotStartedNts(DateTime dateTime);
        Task UpdateRating(NtsTypeEnum ntsType, string ntsId, string userId, int rating, string ratingComment);
        Task RemoveRating(NtsTypeEnum ntsType, string ntsId, string userId);
        Task SendNotificationForRentServices();
        Task UpdateRentalStatusForVacating();
    }
}
