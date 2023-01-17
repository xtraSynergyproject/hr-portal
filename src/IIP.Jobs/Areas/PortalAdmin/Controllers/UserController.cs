using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Synergy.App.DataModel;

namespace Synergy.App.Api.Areas.PortalAdmin.Controllers
{
    [Route("portalAdmin/User")]
    [ApiController]
    public class UserController : ApiController
    {

        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private readonly IServiceProvider _serviceProvider;
        private IUserBusiness _userBusiness;
        private ICandidateProfileBusiness _candidateProfileBusiness;
        private IUserPortalBusiness _userPortalBusiness;
        private ILegalEntityBusiness _legalEntityBusiness;
        private IPortalBusiness _portalBusiness;
        private IUserHierarchyPermissionBusiness _userHierarchyPermissionBusiness;
        private IPageBusiness _pageBusiness;
        public UserController(AuthSignInManager<ApplicationIdentityUser> customUserManager, IPageBusiness pageBusiness, IUserHierarchyPermissionBusiness userHierarchyPermissionBusiness, IPortalBusiness portalBusiness, ILegalEntityBusiness legalEntityBusiness, IServiceProvider serviceProvider, IUserPortalBusiness userPortalBusiness,ICandidateProfileBusiness candidateProfileBusiness,
            IUserBusiness userBusiness) : base(serviceProvider)
        {

            _userBusiness = userBusiness;
            _serviceProvider = serviceProvider;
            _customUserManager = customUserManager;
            _userPortalBusiness = userPortalBusiness;
            _candidateProfileBusiness= candidateProfileBusiness;
            _legalEntityBusiness= legalEntityBusiness;
            _portalBusiness= portalBusiness;    
            _userHierarchyPermissionBusiness= userHierarchyPermissionBusiness;
            _pageBusiness= pageBusiness;
        }

        [HttpGet]
        [Route("ReadData")]
        public async Task<ActionResult> ReadData(string portalName, string userId)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var model = await _userBusiness.GetUsersWithPortalIds();
            return Ok(model);
        }

        [HttpGet]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser(string portalName,string userId)
        {
            await Authenticate(userId, portalName);
            var _userContext = _serviceProvider.GetService<IUserContext>();


            var portal = await _userBusiness.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
            //if (portal != null)
            //{
            //    ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{portal.Theme.ToString()}/_Layout.cshtml";
            //}
            //else
            //{
            //    ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
            //}
            // string[] LegalEntityIds = { _userContext.LegalEntityId };

            var model = new UserViewModel
            {
                DataAction = DataActionEnum.Create,
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                // Password = "123",
                // ConfirmPassword = "123",
                LegalEntityId = _userContext.LegalEntityId,
                PortalId = _userContext.PortalId,

                Portal = new List<string> { _userContext.PortalId },

                PortalName = portal.IsNotNull() ? portal.Name : ""
                //LegalEntityIds = LegalEntityIds
            };


            return Ok(model);


        }

        [HttpGet]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var user = await _userBusiness.GetSingleById(Id);
            if (user != null)
            {
                var candidatelist = await _candidateProfileBusiness.GetList(x => x.Email == user.Email);
                if (candidatelist != null && candidatelist.Count() > 0)
                {
                    foreach (var candidate in candidatelist)
                    {
                        await _candidateProfileBusiness.Delete(candidate.Id);
                    }
                }
            }
            await _userBusiness.Delete(Id);

            return Ok(true);
        }

        [HttpGet]
        [Route("EditUser")]
        public async Task<IActionResult> EditUser(string Id, string portalId,string userId)
        {
            await Authenticate(userId, "Recruitment");
            var _userContext = _serviceProvider.GetService<IUserContext>();

            var portal = await _userBusiness.GetSingleById<PortalViewModel, Portal>(_userContext.PortalId);
            //if (portal != null)
            //{
            //    ViewBag.Layout = $"~/Areas/Core/Views/Shared/Themes/{portal.Theme.ToString()}/_Layout.cshtml";
            //}
            //else
            //{
            //    ViewBag.Layout = "~/Areas/Core/Views/Shared/_PopupLayout.cshtml";
            //}
            var member = await _userBusiness.GetSingleById(Id);
            //if (member.IsSystemAdmin)
            //{
            var userPortals = await _userPortalBusiness.GetPortalByUser(Id);
            member.Portal = new List<string>();
            if (userPortals.IsNotNull())
            {
                foreach (var up in userPortals)
                {
                    member.Portal.Add(up.PortalId);
                }
            }
            //}
            //else { member.Portal = new List<string> { portalId }; }

            if (member != null)
            {
                member.ConfirmPassword = member.Password;
                member.DataAction = DataActionEnum.Edit;
                member.PortalName = portal.IsNotNull() ? portal.Name : "";
                return Ok( member);
            }
            return Ok( new UserViewModel());
        }



        [HttpPost]
        [Route("ManagePortalUser")]

        public async Task<IActionResult> ManagePortalUser(UserViewModel model)
        {
            await Authenticate(model.UserId, "Recruitment");
            var _userContext = _serviceProvider.GetService<IUserContext>();

            if (ModelState.IsValid)
            {
                //  model.PortalId = _userContext.PortalId;
                //  model.LegalEntityId = _userContext.LegalEntityId;
                if (model.DataAction == DataActionEnum.Create)
                {
                    string[] LegalEntityIds = { _userContext.LegalEntityId };
                    model.LegalEntityIds = LegalEntityIds;
                    //model.Portal = new List<string>() { model.PortalId };
                    var result = await _userBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        //ViewBag.Success = true;
                        //Add UserPortal data
                        if (model.Portal.IsNotNull())
                        {
                            foreach (var p in model.Portal)
                            {
                                var res = await _userPortalBusiness.Create(new UserPortalViewModel
                                {
                                    UserId = result.Item.Id,
                                    PortalId = p,
                                });
                            }
                        }
                        return Ok(new { success = true, result = result.Item });
                    }
                    else
                    {
                        //ModelState.AddModelErrors(result.Messages);
                        return Ok(new { success = false, error =result.Message  });
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var existingItem = await _userBusiness.GetSingleById(model.Id);
                    existingItem.Name = model.Name;
                    existingItem.JobTitle = model.JobTitle;
                    existingItem.Email = model.Email;
                    //existingItem.Password = model.Password;
                    // existingItem.ConfirmPassword = model.ConfirmPassword;
                    model.Password = existingItem.Password;
                    model.ConfirmPassword = existingItem.Password;
                    if (!existingItem.LegalEntityIds.Contains(_userContext.LegalEntityId))
                    {
                        string[] LegalEntityIds = { _userContext.LegalEntityId };
                        model.LegalEntityIds = LegalEntityIds;
                    }

                    var result = await _userBusiness.Edit(model);
                    if (result.IsSuccess)
                    {

                        var portals = await _userPortalBusiness.GetPortalByUser(model.Id);
                        //var list = portals.Select(x => x.PortalId).ToList();
                        //model.Portal = new List<string>();
                        //model.Portal.AddRange(list);
                        if (portals.IsNotNull())
                        {
                            foreach (var p in portals)
                            {
                                if (model.Portal == null || model.Portal.Contains(p.PortalId) == false)
                                {
                                    await _userPortalBusiness.Delete(p.Id);
                                }
                            }
                        }
                        if (model.Portal.IsNotNull())
                        {
                            foreach (var p in model.Portal)
                            {
                                if (!portals.Where(x => x.PortalId == p).Any())
                                {
                                    var res = await _userPortalBusiness.Create(new UserPortalViewModel
                                    {
                                        UserId = result.Item.Id,
                                        PortalId = p,
                                    });
                                }
                            }
                        }
                        return Ok(new { success = true, result = result.Item });
                    }
                    else
                    {
                        //ModelState.AddModelErrors(result.Messages);
                        return Ok(new { success = false, error = result.Message});
                    }
                }
            }

            return Ok(new { success = false, error = ModelState.SerializeErrors() });
        }


        [HttpGet]
        [Route("GetLegalEntitiesList")]
        public async Task<ActionResult> GetLegalEntitiesList(string userId)
        {
            var data = new List<LegalEntityViewModel>();
            if (userId.IsNotNullAndNotEmpty())
            {
                var userdetails = await _userBusiness.GetSingleById(userId);
                var legalentity = "'" + string.Join("','", userdetails.LegalEntityIds) + "'";
                data = await _userBusiness.GetEntityByIds(legalentity);
            }
            else
            {
                data = await _legalEntityBusiness.GetList();
            }
            return Ok(data);
        }

        
        [HttpGet]
        [Route("GetPortalForUser")]
        public async Task<IActionResult> GetPortalForUser(string id)
        {
            var data = await _portalBusiness.GetPortalForUser(id);
            return Ok(data);
        }

        [HttpGet]
        [Route("SendWelcomeEmail")]
        public async Task<IActionResult> SendWelcomeEmail(string userId)
        {
            var user = await _userBusiness.GetSingleById(userId);
            if (user != null)
            {
                await _userBusiness.SendWelcomeEmail(user);
            }
            return Ok(true);
        }

        [HttpGet]
        [Route("GetPortalFancyTreeList")]
        public async Task<IActionResult> GetPortalFancyTreeList(string id, string userId)
        {
            var result = await _pageBusiness.GetPortalTreeList(id, userId);
            var newList = new List<FileExplorerViewModel>();
            newList.AddRange(result.ToList().Select(x => new FileExplorerViewModel
            {
                key = x.id,
                title = x.Name,
                lazy = true,
                FieldDataType = x.FieldDataType.ToString(),
                ParentId = id,
                checkbox = x.HideCheckbox,
                selected = x.Checked
            }));
            var json = JsonConvert.SerializeObject(newList);
            return Ok(json);
        }

        [HttpGet]
        [Route("ReadUserHierarchyPermission")]
        public async Task<IActionResult> ReadUserHierarchyPermission(string userId)
        {
            var model = await _userHierarchyPermissionBusiness.GetUserPermissionHierarchy(userId);
            var data = model;
            return Ok(data);
        }


        [HttpGet]
        [Route("GetHierarchyIdNameList")]
        public async Task<IActionResult> GetHierarchyIdNameList()
        {
            var dataList = await _userHierarchyPermissionBusiness.GetListGlobal<HierarchyMasterViewModel, HierarchyMaster>();
            return Ok(dataList.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name, Code = x.HierarchyType.ToString() }).ToList());
        }
    }
}
