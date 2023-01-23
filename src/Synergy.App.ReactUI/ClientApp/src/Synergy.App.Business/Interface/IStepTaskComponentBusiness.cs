using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IStepTaskComponentBusiness : IBusinessBase<StepTaskComponentViewModel, StepTaskComponent>
    {

        Task RemoveStepTask(string componentId);
        Task<IList<IdNameViewModel>> GetComponentList();
        Task<IList<IdNameViewModel>> GetStepTaskParentList(string templateId);


    }
}
