using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IBusinessDiagramBusiness : IBusinessBase<TaskViewModel, NtsTask>
    {
        Task<bool> ManageBusinessDiagramTask(BusinessDiagramViewModel model);
        Task<bool> ManageGenericDiagramTask(BusinessDiagramViewModel model);

    }
}
