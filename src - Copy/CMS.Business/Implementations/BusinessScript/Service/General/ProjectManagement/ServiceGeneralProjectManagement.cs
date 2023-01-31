using CMS.Business.Interface.BusinessScript.Service.General.ProjectManagement;
using CMS.Common;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMS.Business.Implementations.BusinessScript.Service.General.ProjectManagement
{
    public class ServiceGeneralProjectManagement : IServiceGeneralProjectManagement
    {
        public CommandResult<ServiceTemplateViewModel> Test(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method for to validate the project name is already exist or not
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<ServiceTemplateViewModel>> ValidateProjectName(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _business = sp.GetService<IServiceBusiness>();
            var res = await _business.ValidateProjectName(viewModel);
            if (res == false)
            {
                var errorList = new Dictionary<string, string>();
                errorList.Add("Validate", "Project Name already exist");
                return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, false, errorList);
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
    }
}
