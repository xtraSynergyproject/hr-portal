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
    public interface IPropertyBusiness : IBusinessBase<NoteViewModel, NtsNote>
    {
        Task<IList<PayPropertyTaxViewModel>> GetPropertySearch(string city, string propertyId, string ownerName, string oldPropertyId, string houseNo, string street, string mobile, string wardNo, string postalCode, string email);
        Task<PayPropertyTaxViewModel> GetPropertyTaxbyId(string PropertyId);
        Task<List<PropertyAreaDetailsViewModel>> GetCurrentYearSummary(string PropertyId);

        List<PayPropertyTaxViewModel> GetYearWiseBreakUp();
        Task<PayPropertyTaxViewModel> GetNDCDetails(string serviceId);
        Task UpdateOTP(string serviceId, string OTP, DateTime ExpiryDate);
        Task<PayPropertyTaxViewModel> ValidateOTP(string serviceId, string curOTP);
    }
}
