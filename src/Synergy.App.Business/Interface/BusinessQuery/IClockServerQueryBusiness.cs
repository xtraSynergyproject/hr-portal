using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IClockServerQueryBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<IList<IdNameViewModel>> GetAllDeviceData();
        Task<UserInfoViewModel> GetUserInfoDetailsData(string biometricId);
        Task<IList<UserInfoViewModel>> GetIncludePersonListData(string deviceId, string searchParam);
        Task<IList<UserInfoViewModel>> GetExcludePersonListData(string deviceId, string searchParam);
        Task<PayPropertyTaxViewModel> GetPropertyTaxDataById(string PropertyId);
        Task UpdateIncludeExcludeUserInfoDeviceData(string deviceId, IncludeExclude action, string id, string isDeleted);
        Task<List<UserInfoViewModel>> GetUserInfoDeviceData(string deviceId, string id);
    }
}
