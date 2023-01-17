using Newtonsoft.Json;
using Synergy.App.Business.Interface.BusinessScript.Form.General.BLS;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Form.General.BLS
{
    class FormGeneralBLSPreScript : IFormGeneralBLSPreScript
    {
        public async Task<CommandResult<FormTemplateViewModel>> ValidateDetailsForVACExecutive(FormTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _userBusiness = sp.GetService<IUserBusiness>();
            var _userRoleBusiness = sp.GetService<IUserRoleBusiness>();
            var _userRoleUserBusiness = sp.GetService<IUserRoleUserBusiness>();
            var portalBusiness = sp.GetService<IPortalBusiness>();
            var _userPortalBusiness = sp.GetService<IUserPortalBusiness>();
            var _pageBusiness = sp.GetService<IPageBusiness>();

            var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);

            var name = Convert.ToString(rowData.GetValueOrDefault("UserName"));
            var email = Convert.ToString(rowData.GetValueOrDefault("Email"));
            var password = Convert.ToString(rowData.GetValueOrDefault("Password"));
            var confirmPassword = Convert.ToString(rowData.GetValueOrDefault("ConfirmPassword"));

            if (viewModel.DataAction == DataActionEnum.Create) 
            {
                var le = await _pageBusiness.GetSingle<LegalEntityViewModel, Synergy.App.DataModel.LegalEntity>
                    (x => x.Code == "CASABLANCA");
                string[] LegalEntityIds = { uc.LegalEntityId, le?.Id };
                UserViewModel userModel = new();
                userModel.Name = name;
                userModel.Email = email;
                userModel.Password = password;
                userModel.ConfirmPassword = confirmPassword;
                userModel.PortalId = uc.PortalId;
                userModel.LegalEntityIds = LegalEntityIds;
                var res = await _userBusiness.Create(userModel);
                if (res.IsSuccess)
                {
                    rowData["UserId"] = res.Item.Id;
                    var userRole = await _userRoleBusiness.GetSingle(x => x.Code == "VAC_EX_APP_INIT");
                    var userRoleUser = new UserRoleUserViewModel();
                    userRoleUser.UserId = res.Item.Id;
                    userRoleUser.PortalId = uc.PortalId;
                    userRoleUser.UserRoleId = userRole.Id;
                    var result = await _userRoleUserBusiness.Create(userRoleUser);
                    var resPortal = await _userPortalBusiness.Create(new UserPortalViewModel
                    {
                        UserId = res.Item.Id,
                        PortalId = uc.PortalId
                    });
                }
                else
                {
                    return CommandResult<FormTemplateViewModel>.Instance(viewModel, false, res.ErrorText);
                }

            }
            else if (viewModel.DataAction == DataActionEnum.Edit)
            {
                var userId = Convert.ToString(rowData.GetValueOrDefault("UserId"));
                UserViewModel user = new();
                user.Name = name;
                user.Email = email;
                user.Password = password;
                user.ConfirmPassword = confirmPassword;
                user.Id = userId;
                var res = await _userBusiness.Edit(user);
                if (!res.IsSuccess)
                {
                    return CommandResult<FormTemplateViewModel>.Instance(viewModel, false, res.ErrorText);
                }
            }

            var data = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
            viewModel.Json = data;
            return CommandResult<FormTemplateViewModel>.Instance(viewModel);
        }
    }
}
