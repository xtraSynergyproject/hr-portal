using Synergy.App.Business.Interface.BusinessScript.Form.General.SEBI;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Form.General.SEBI
{
    class FormGeneralSEBIPostScript : IFormGeneralSEBIPostScript
    {
        public async Task<CommandResult<FormTemplateViewModel>> CreateUserAndPortalAccessForStudent(FormTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _userContext = sp.GetService<IUserContext>();
            var _userBusiness = sp.GetService<IUserBusiness>();            
            var _userRoleBusiness = sp.GetService<IUserRoleBusiness>();
            var _userRoleUserBusiness = sp.GetService<IUserRoleUserBusiness>();
            var _portalBusiness = sp.GetService<IPortalBusiness>();
            var _userPortalBusiness = sp.GetService<IUserPortalBusiness>();

            var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);            

            var name = Convert.ToString(rowData.GetValueOrDefault("FirstName"));
            var addr = Convert.ToString(rowData.GetValueOrDefault("Address"));
            var email = Convert.ToString(rowData.GetValueOrDefault("Email"));
            var pass = Convert.ToString(rowData.GetValueOrDefault("Password"));
            var user = new UserViewModel();
            
            user.Name = name;
            user.Address = addr;
            user.Email = email;
            user.Password = pass;
            var res = await _userBusiness.Create(user);
            if (res.IsSuccess)
            {                
                var portalDetails = await _portalBusiness.GetSingle(x => x.Name == "SEBIInvestor");

                //var userRole = await _userRoleBusiness.GetSingle(x => x.Code == "GARBAGE_COLLECTOR");
                //var userRoleUser = new UserRoleUserViewModel();
                //userRoleUser.UserId = res.Item.Id;
                //userRoleUser.PortalId = _userContext.PortalId;
                //userRoleUser.UserRoleId = userRole.Id;
                //var result = await _userRoleUserBusiness.Create(userRoleUser);

                var resPortal = await _userPortalBusiness.Create(new UserPortalViewModel
                {
                    UserId = res.Item.Id,
                    PortalId = portalDetails.IsNotNull() ? portalDetails.Id : _userContext.PortalId
                });
            }
            
            return CommandResult<FormTemplateViewModel>.Instance(viewModel);
        }       
    }
}
