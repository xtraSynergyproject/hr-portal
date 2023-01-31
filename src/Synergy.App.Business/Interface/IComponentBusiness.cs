using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IComponentBusiness : IBusinessBase<ComponentViewModel,Component>
    {
        Task<List<ComponentParentViewModel>> GetComponentParent(string componentId);
        Task RemoveParents(string componentId);
        Task CreateComponentParents(string componentId, string[] parents);
        Task RemoveComponentsByProcessDesignId(string ProcessDesignId);
        Task RemoveComponentsAndItsChild(string ComponentId);
        Task<List<ComponentViewModel>> GetComponentsAndChilds(string ComponentId, List<ComponentViewModel> list);
    }
}
