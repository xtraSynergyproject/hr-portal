using Synergy.App.Business.Interface.BusinessScript.Form.General.EGOV;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Form.General.EGOV
{
    class FormGeneralEgovPostScript : IFormGeneralEgovPostScript
    {
        public async Task<CommandResult<FormTemplateViewModel>> UpdateUserIdInCollectorForm(FormTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _egovBusiness = sp.GetService<ISmartCityBusiness>();
            var userBusiness = sp.GetService<IUserBusiness>();
            var _userContext = sp.GetService<IUserContext>();
            var _userRoleBusiness = sp.GetService<IUserRoleBusiness>();
            var _userRoleUserBusiness = sp.GetService<IUserRoleUserBusiness>();
            var portalBusiness = sp.GetService<IPortalBusiness>();
            var _userPortalBusiness = sp.GetService<IUserPortalBusiness>();

            var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var phoneNo = Convert.ToString(rowData.GetValueOrDefault("PhoneNumber"));

            var userModel = await userBusiness.GetSingle(x => x.Mobile == phoneNo);
            if (userModel.IsNotNull())
            {
                //rowData.SetPropertyValue("UserId", userModel.Id);
                rowData["UserId"] = userModel.Id;
                await _egovBusiness.UpdateCollectorUserId(viewModel.RecordId, userModel.Id);
            }
            else
            {
                var name = Convert.ToString(rowData.GetValueOrDefault("Name"));
                var addr = Convert.ToString(rowData.GetValueOrDefault("Address"));
                var email = Convert.ToString(rowData.GetValueOrDefault("Email"));
                var user = new UserViewModel();
                user.Mobile = phoneNo;
                user.Name = name;
                user.Address = addr;
                user.Email = email;
                var res = await userBusiness.Create(user);
                if (res.IsSuccess)
                {
                    //rowData.SetPropertyValue("UserId", res.Item.Id);
                    rowData["UserId"] = res.Item.Id;
                    await _egovBusiness.UpdateCollectorUserId(viewModel.RecordId, res.Item.Id);
                    var portalDetails = await portalBusiness.GetSingle(x => x.Name == "JammuSmartCityEmployee");

                    var userRole = await _userRoleBusiness.GetSingle(x => x.Code == "GARBAGE_COLLECTOR");
                    var userRoleUser = new UserRoleUserViewModel();
                    userRoleUser.UserId = res.Item.Id;
                    userRoleUser.PortalId = _userContext.PortalId;
                    userRoleUser.UserRoleId = userRole.Id;
                    var result = await _userRoleUserBusiness.Create(userRoleUser);
                    var resPortal = await _userPortalBusiness.Create(new UserPortalViewModel
                    {
                        UserId = res.Item.Id,
                        PortalId = portalDetails.IsNotNull() ? portalDetails.Id : _userContext.PortalId
                    });
                }
            }
            var data = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
            viewModel.Json = data;
            return CommandResult<FormTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<FormTemplateViewModel>> UpdateUserIdInDriverForm(FormTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _egovBusiness = sp.GetService<ISmartCityBusiness>();
            var userBusiness = sp.GetService<IUserBusiness>();
            var _userContext = sp.GetService<IUserContext>();
            var _userRoleBusiness = sp.GetService<IUserRoleBusiness>();
            var _userRoleUserBusiness = sp.GetService<IUserRoleUserBusiness>();
            var portalBusiness = sp.GetService<IPortalBusiness>();
            var _userPortalBusiness = sp.GetService<IUserPortalBusiness>();

            var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var phoneNo = Convert.ToString(rowData.GetValueOrDefault("PhoneNumber"));

            var userModel = await userBusiness.GetSingle(x => x.Mobile == phoneNo);
            if (userModel.IsNotNull())
            {
                //rowData.SetPropertyValue("UserId", userModel.Id);
                rowData["UserId"] = userModel.Id;
                await _egovBusiness.UpdateCollectorUserId(viewModel.RecordId, userModel.Id);
            }
            else
            {
                var name = Convert.ToString(rowData.GetValueOrDefault("DriverName"));
                var addr = Convert.ToString(rowData.GetValueOrDefault("Address"));
                var email = Convert.ToString(rowData.GetValueOrDefault("Email"));
                var user = new UserViewModel();
                user.Mobile = phoneNo;
                user.Name = name;
                user.Address = addr;
                user.Email = email;
                var res = await userBusiness.Create(user);
                if (res.IsSuccess)
                {
                    //rowData.SetPropertyValue("UserId", res.Item.Id);
                    rowData["UserId"] = res.Item.Id;
                    await _egovBusiness.UpdateDriverUserId(viewModel.RecordId, res.Item.Id);
                    var portalDetails = await portalBusiness.GetSingle(x => x.Name == "JammuSmartCityEmployee");

                    var userRole = await _userRoleBusiness.GetSingle(x => x.Code == "JSC_DRIVER");
                    var userRoleUser = new UserRoleUserViewModel();
                    userRoleUser.UserId = res.Item.Id;
                    userRoleUser.PortalId = _userContext.PortalId;
                    userRoleUser.UserRoleId = userRole.Id;
                    var result = await _userRoleUserBusiness.Create(userRoleUser);
                    var resPortal = await _userPortalBusiness.Create(new UserPortalViewModel
                    {
                        UserId = res.Item.Id,
                        PortalId = portalDetails.IsNotNull() ? portalDetails.Id : _userContext.PortalId
                    });
                }
            }
            var data = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
            viewModel.Json = data;
            return CommandResult<FormTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<FormTemplateViewModel>> UpdateUserIdInVehicleForm(FormTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _egovBusiness = sp.GetService<ISmartCityBusiness>();
            var userBusiness = sp.GetService<IUserBusiness>();
            var _userContext = sp.GetService<IUserContext>();
            var _userRoleBusiness = sp.GetService<IUserRoleBusiness>();
            var _userRoleUserBusiness = sp.GetService<IUserRoleUserBusiness>();
            var portalBusiness = sp.GetService<IPortalBusiness>();
            var _userPortalBusiness = sp.GetService<IUserPortalBusiness>();

            var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var email = Convert.ToString(rowData.GetValueOrDefault("Userid"));

            var userModel = await userBusiness.GetSingle(x => x.Email == email);
            if (userModel.IsNotNull())
            {
                //rowData.SetPropertyValue("UserId", userModel.Id);
                rowData["UserId"] = userModel.Id;
                await _egovBusiness.UpdateDriverUserId(viewModel.RecordId, userModel.Id);
            }
            else
            {
                var name = Convert.ToString(rowData.GetValueOrDefault("VehicleNumber"));
                var addr = Convert.ToString(rowData.GetValueOrDefault("Address"));
                var user = new UserViewModel();
                user.Mobile = "";
                user.Name = name;
                user.Address = "Jammu";
                user.Email = email;
                var res = await userBusiness.Create(user);
                if (res.IsSuccess)
                {
                    //rowData.SetPropertyValue("UserId", res.Item.Id);
                    rowData["UserId"] = res.Item.Id;
                    await _egovBusiness.UpdateDriverUserId(viewModel.RecordId, res.Item.Id);
                    var portalDetails = await portalBusiness.GetSingle(x => x.Name == "JammuSmartCityEmployee");

                    var userRole = await _userRoleBusiness.GetSingle(x => x.Code == "JSC_VEHICLE_USER");
                    var userRoleUser = new UserRoleUserViewModel();
                    userRoleUser.UserId = res.Item.Id;
                    userRoleUser.PortalId = _userContext.PortalId;
                    userRoleUser.UserRoleId = userRole.Id;
                    var result = await _userRoleUserBusiness.Create(userRoleUser);
                    var resPortal = await _userPortalBusiness.Create(new UserPortalViewModel
                    {
                        UserId = res.Item.Id,
                        PortalId = portalDetails.IsNotNull() ? portalDetails.Id : _userContext.PortalId
                    });
                }
            }
            var data = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
            viewModel.Json = data;
            return CommandResult<FormTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<FormTemplateViewModel>> UpdateUserIdInComplaintOperatorForm(FormTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _egovBusiness = sp.GetService<ISmartCityBusiness>();
            var userBusiness = sp.GetService<IUserBusiness>();
            var portalBusiness = sp.GetService<IPortalBusiness>();
            var _userContext = sp.GetService<IUserContext>();
            var _userRoleBusiness = sp.GetService<IUserRoleBusiness>();
            var _userRoleUserBusiness = sp.GetService<IUserRoleUserBusiness>();
            var _userPortalBusiness = sp.GetService<IUserPortalBusiness>();

            var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var phoneNo = Convert.ToString(rowData.GetValueOrDefault("MobileNo"));

            var userModel = await userBusiness.GetSingle(x => x.Mobile == phoneNo);
            if (userModel.IsNotNull())
            {
                //rowData.SetPropertyValue("UserId", userModel.Id);
                rowData["UserId"] = userModel.Id;
                await _egovBusiness.UpdateComplaintOperatorUserId(viewModel.RecordId, userModel.Id);

            }
            else
            {
                var name = Convert.ToString(rowData.GetValueOrDefault("Name"));
                var email = Convert.ToString(rowData.GetValueOrDefault("Email"));
                var user = new UserViewModel();
                user.Mobile = phoneNo;
                user.Name = name;
                user.Email = email;
                var res = await userBusiness.Create(user);
                if (res.IsSuccess)
                {
                    //rowData.SetPropertyValue("UserId", res.Item.Id);
                    rowData["UserId"] = res.Item.Id;
                    await _egovBusiness.UpdateComplaintOperatorUserId(viewModel.RecordId, res.Item.Id);

                    var portalDetails = await portalBusiness.GetSingle(x => x.Name == "JammuSmartCityEmployee");

                    var userRole = await _userRoleBusiness.GetSingle(x => x.Code == "COMPLAINT_OPERATOR");
                    var userRoleUser = new UserRoleUserViewModel();
                    userRoleUser.UserId = res.Item.Id;
                    userRoleUser.PortalId = portalDetails.IsNotNull() ? portalDetails.Id :  _userContext.PortalId;
                    userRoleUser.UserRoleId = userRole.Id;
                    var result = await _userRoleUserBusiness.Create(userRoleUser);
                    var resPortal = await _userPortalBusiness.Create(new UserPortalViewModel
                    {
                        UserId = res.Item.Id,
                        PortalId = portalDetails.IsNotNull() ? portalDetails.Id : _userContext.PortalId
                    });
                }
            }
            var data = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
            viewModel.Json = data;
            return CommandResult<FormTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<FormTemplateViewModel>> UpdateUserIdInTransferStationForm(FormTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _egovBusiness = sp.GetService<ISmartCityBusiness>();
            var userBusiness = sp.GetService<IUserBusiness>();
            var portalBusiness = sp.GetService<IPortalBusiness>();
            var _userContext = sp.GetService<IUserContext>();
            var _userRoleBusiness = sp.GetService<IUserRoleBusiness>();
            var _userRoleUserBusiness = sp.GetService<IUserRoleUserBusiness>();
            var _userPortalBusiness = sp.GetService<IUserPortalBusiness>();

            var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var phoneNo = Convert.ToString(rowData.GetValueOrDefault("MobileNo"));

            var userModel = await userBusiness.GetSingle(x => x.Mobile == phoneNo);
            if (userModel.IsNotNull())
            {
                //rowData.SetPropertyValue("UserId", userModel.Id);
                rowData["UserId"] = userModel.Id;
                await _egovBusiness.UpdateTransferStationUserId(viewModel.RecordId, userModel.Id);

            }
            else
            {
                var name = Convert.ToString(rowData.GetValueOrDefault("UserName"));
                var email = Convert.ToString(rowData.GetValueOrDefault("UserName"));
                var user = new UserViewModel();
                user.Mobile = phoneNo;
                user.Name = name;
                user.Email = email;
                var res = await userBusiness.Create(user);
                if (res.IsSuccess)
                {
                    //rowData.SetPropertyValue("UserId", res.Item.Id);
                    rowData["UserId"] = res.Item.Id;
                    await _egovBusiness.UpdateTransferStationUserId(viewModel.RecordId, res.Item.Id);

                    var portalDetails = await portalBusiness.GetSingle(x => x.Name == "JammuSmartCityEmployee");

                    var userRole = await _userRoleBusiness.GetSingle(x => x.Code == "JSC_TRANSFER_STATION");
                    var userRoleUser = new UserRoleUserViewModel();
                    userRoleUser.UserId = res.Item.Id;
                    userRoleUser.PortalId = portalDetails.IsNotNull() ? portalDetails.Id :  _userContext.PortalId;
                    userRoleUser.UserRoleId = userRole.Id;
                    var result = await _userRoleUserBusiness.Create(userRoleUser);
                    var resPortal = await _userPortalBusiness.Create(new UserPortalViewModel
                    {
                        UserId = res.Item.Id,
                        PortalId = portalDetails.IsNotNull() ? portalDetails.Id : _userContext.PortalId
                    });
                }
            }
            var data = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
            viewModel.Json = data;
            return CommandResult<FormTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<FormTemplateViewModel>> UpdateUserIdInWTPForm(FormTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _egovBusiness = sp.GetService<ISmartCityBusiness>();
            var userBusiness = sp.GetService<IUserBusiness>();
            var portalBusiness = sp.GetService<IPortalBusiness>();
            var _userContext = sp.GetService<IUserContext>();
            var _userRoleBusiness = sp.GetService<IUserRoleBusiness>();
            var _userRoleUserBusiness = sp.GetService<IUserRoleUserBusiness>();
            var _userPortalBusiness = sp.GetService<IUserPortalBusiness>();

            var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var phoneNo = Convert.ToString(rowData.GetValueOrDefault("MobileNo"));

            var userModel = await userBusiness.GetSingle(x => x.Mobile == phoneNo);
            if (userModel.IsNotNull())
            {
                //rowData.SetPropertyValue("UserId", userModel.Id);
                rowData["UserId"] = userModel.Id;
                await _egovBusiness.UpdateWTPUserId(viewModel.RecordId, userModel.Id);

            }
            else
            {
                var name = Convert.ToString(rowData.GetValueOrDefault("UserName"));
                var email = Convert.ToString(rowData.GetValueOrDefault("UserName"));
                var user = new UserViewModel();
                user.Mobile = phoneNo;
                user.Name = name;
                user.Email = email;
                var res = await userBusiness.Create(user);
                if (res.IsSuccess)
                {
                    //rowData.SetPropertyValue("UserId", res.Item.Id);
                    rowData["UserId"] = res.Item.Id;
                    await _egovBusiness.UpdateWTPUserId(viewModel.RecordId, res.Item.Id);

                    var portalDetails = await portalBusiness.GetSingle(x => x.Name == "JammuSmartCityEmployee");

                    var userRole = await _userRoleBusiness.GetSingle(x => x.Code == "JSC_WTP");
                    var userRoleUser = new UserRoleUserViewModel();
                    userRoleUser.UserId = res.Item.Id;
                    userRoleUser.PortalId = portalDetails.IsNotNull() ? portalDetails.Id :  _userContext.PortalId;
                    userRoleUser.UserRoleId = userRole.Id;
                    var result = await _userRoleUserBusiness.Create(userRoleUser);
                    var resPortal = await _userPortalBusiness.Create(new UserPortalViewModel
                    {
                        UserId = res.Item.Id,
                        PortalId = portalDetails.IsNotNull() ? portalDetails.Id : _userContext.PortalId
                    });
                }
            }
            var data = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
            viewModel.Json = data;
            return CommandResult<FormTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<FormTemplateViewModel>> CreateUserSubLogin(FormTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _egovBusiness = sp.GetService<ISmartCityBusiness>();
            var userBusiness = sp.GetService<IUserBusiness>();
            var portalBusiness = sp.GetService<IPortalBusiness>();
            var _userContext = sp.GetService<IUserContext>();
            var _userRoleBusiness = sp.GetService<IUserRoleBusiness>();
            var _userRoleUserBusiness = sp.GetService<IUserRoleUserBusiness>();
            var _userPortalBusiness = sp.GetService<IUserPortalBusiness>();
            var _LovBusiness = sp.GetService<ILOVBusiness>();
            
            var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var phoneNo = Convert.ToString(rowData.GetValueOrDefault("SubMobileNo"));
            var userModel = await userBusiness.GetSingle(x => x.Mobile == phoneNo);
            var name = Convert.ToString(rowData.GetValueOrDefault("SubName"));
            var email = Convert.ToString(rowData.GetValueOrDefault("SubEmail"));
            var password = Convert.ToString(rowData.GetValueOrDefault("SubUserPassword"));
            var loginType= Convert.ToString(rowData.GetValueOrDefault("SubloginType"));
            var Userstatus = Convert.ToString(rowData.GetValueOrDefault("SubloginStatus"));
            var appStatus = await _LovBusiness.GetSingle(x => x.Id == loginType);
           
            if (viewModel.DataAction == DataActionEnum.Create)
            {
                var user = new UserViewModel();
                user.Mobile = phoneNo;
                user.Name = name;
                user.Email = email;
                user.Password = password;
                user.Status = (Userstatus == "Active") ? StatusEnum.Active : StatusEnum.Inactive;
                var res = await userBusiness.Create(user);
                if (res.IsSuccess)
                {
                    rowData["SubloginUserId"] = res.Item.Id;
                    await _egovBusiness.UpdateSubLoginUserId(viewModel.RecordId, res.Item.Id);
                    var portalDetails = await portalBusiness.GetSingle(x => x.Name == "JammuSmartCityEmployee");
                    var userRole = await _userRoleBusiness.GetSingle(x => x.Code == appStatus.Code);
                    var userRoleUser = new UserRoleUserViewModel();
                    userRoleUser.UserId = res.Item.Id;
                    userRoleUser.PortalId = portalDetails.IsNotNull() ? portalDetails.Id : _userContext.PortalId;
                    userRoleUser.UserRoleId = userRole.Id;
                    var result = await _userRoleUserBusiness.Create(userRoleUser);
                    var resPortal = await _userPortalBusiness.Create(new UserPortalViewModel
                    {
                        UserId = res.Item.Id,
                        PortalId = portalDetails.IsNotNull() ? portalDetails.Id : _userContext.PortalId
                    });
                }
            }
            else if (viewModel.DataAction == DataActionEnum.Edit)
            {
                string userId = udf.SubloginUserId;
                if (userId.IsNotNullAndNotEmpty())
                {
                    var userDetail = await userBusiness.GetSingleById(userId);
                    if (userDetail.IsNotNull())
                    {
                        userDetail.Mobile = phoneNo;
                        userDetail.Name = name;
                        userDetail.Email = email;
                        userDetail.Password = password;
                        userDetail.Status = (Userstatus == "Active") ? StatusEnum.Active : StatusEnum.Inactive;
                        var res = await userBusiness.Edit(userDetail);
                    }
                   
                }
                              
            }

            var data = Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
            viewModel.Json = data;
            return CommandResult<FormTemplateViewModel>.Instance(viewModel);

        }
    }
}
