using Synergy.App.Business.BusinessScript.Service.Galfar.DocumentManagement;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Service.Galfar.DocumentManagement
{
    public class ServiceGalfarDocumentManagement : IServiceGalfarDocumentManagement
    {
        public CommandResult<ServiceTemplateViewModel> Test(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            throw new NotImplementedException();
        }
    }
}
