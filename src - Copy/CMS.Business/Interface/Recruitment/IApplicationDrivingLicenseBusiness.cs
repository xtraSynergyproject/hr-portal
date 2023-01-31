using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IApplicationDrivingLicenseBusiness : IBusinessBase<ApplicationDrivingLicenseViewModel, ApplicationDrivingLicense>
    {
        Task<IList<ApplicationDrivingLicenseViewModel>> GetLicenseListByApplication(string candidateProfileId);
        //Task<IList<IdNameViewModel>> GetCountryListData();
    }
}
