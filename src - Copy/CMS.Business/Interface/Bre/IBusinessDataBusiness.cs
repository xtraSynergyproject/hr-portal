using CMS.Data.Model;
using CMS.UI.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IBusinessDataBusiness : IBusinessBase<BusinessDataViewModel,BusinessData>
    {
        Task<List<BusinessDataTreeViewModel>> GetBusinessDataTreeList(string companyId);
    }

}
