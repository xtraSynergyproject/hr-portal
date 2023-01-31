using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
  public  interface IUserInfoBusiness:IBusinessBase<NoteViewModel, NtsNote>
    {

        Task<IList<UserInfoViewModel>> GetIncludePersonList(string deviceId, string searchParam);
       Task<IList<UserInfoViewModel>> GetExcludePersonList(string deviceId, string searchParam);

        Task<bool> IncludePerson(string deviceId, string persons,string Userid);
        Task<bool> ExcludePerson(string deviceId, string persons);
        Task<UserInfoViewModel> GetUserInfoDetails(string biometricId);
        Task<IList<IdNameViewModel>> GetAllDevice();
        Task<PayPropertyTaxViewModel> GetPropertyTaxbyId(string PropertyId);
    }
}
