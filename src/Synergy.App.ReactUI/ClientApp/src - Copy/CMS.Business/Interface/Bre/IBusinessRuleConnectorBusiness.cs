using CMS.Data.Model;
using CMS.UI.ViewModel;
using Syncfusion.EJ2.Diagrams;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IBusinessRuleConnectorBusiness : IBusinessBase<BusinessRuleConnectorViewModel, BusinessRuleConnector>
    {
        public Task CreateConnector(DiagramConnector connector, string BussinessRuleId);
        public Task<List<DiagramConnector>> GetConnector(string BussinessRuleId);
    }

}
