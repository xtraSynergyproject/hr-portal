using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IBusinessDiagramBusiness : IBusinessBase<TaskViewModel, NtsTask>
    {
        Task<bool> ManageBusinessDiagramTask(BusinessDiagramViewModel model);
        Task<bool> ManageGenericDiagramTask(BusinessDiagramViewModel model);

    }
}
