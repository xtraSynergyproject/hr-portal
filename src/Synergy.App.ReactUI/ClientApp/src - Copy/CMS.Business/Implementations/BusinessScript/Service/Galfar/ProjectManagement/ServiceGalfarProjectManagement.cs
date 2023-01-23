using CMS.Business.BusinessScript.Service.Galfar.ProjectManagement;
using CMS.Common;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.Implementations.BusinessScript.Service.Galfar.ProjectManagement
{
    public class ServiceGalfarProjectManagement : IServiceGalfarProjectManagement
    {
        public CommandResult<ServiceTemplateViewModel> Test(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            throw new NotImplementedException();
        }
    }
}
