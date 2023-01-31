using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IBusinessDataBusiness : IBusinessBase<BusinessDataViewModel,BusinessData>
    {
        Task<List<BusinessDataTreeViewModel>> GetBusinessDataTreeList(string companyId);
    }

}
