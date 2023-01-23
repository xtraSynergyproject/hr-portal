using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IBreResultBusiness : IBusinessBase<BreResultViewModel,BreResult>
    {
        Task<List<dynamic>> CopyBreResults(List<dynamic> nodeIds, List<BreResultViewModel> oldList);
    }
}