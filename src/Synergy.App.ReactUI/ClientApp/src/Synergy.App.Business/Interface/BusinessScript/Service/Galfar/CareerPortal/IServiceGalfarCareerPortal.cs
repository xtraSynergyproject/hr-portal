using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Interface.BusinessScript.Service.Galfar.CareerPortal
{
    public interface IServiceGalfarCareerPortal
    {
        CommandResult<ServiceTemplateViewModel> Test(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
    }
}
