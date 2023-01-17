using Synergy.App.DataModel;
using Synergy.App.ViewModel;
//using Syncfusion.EJ2.Diagrams;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IBusinessRuleConnectorBusiness : IBusinessBase<BusinessRuleConnectorViewModel, BusinessRuleConnector>
    {
        //public Task CreateConnector(DiagramConnector connector, string BussinessRuleId);
        //public Task<List<DiagramConnector>> GetConnector(string BussinessRuleId);
        Task<bool> CopyBusinessRuleConnector(List<dynamic> BRIds, List<dynamic> nodeIds, List<BusinessRuleConnectorViewModel> oldList);
    }

}
