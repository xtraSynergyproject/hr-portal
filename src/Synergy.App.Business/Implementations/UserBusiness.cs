using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class UserBusiness : BusinessBase<UserViewModel, User>, IUserBusiness
    {
        private readonly IUserPortalBusiness _userportalBusiness;
        private readonly IPortalBusiness _portalBusiness;
        private readonly IDocumentPermissionBusiness _documentPermissionBusiness;
        private IUserRoleUserBusiness _userRoleUserBusiness;
        private IUserContext _userContext;
        private ILOVBusiness _lovBusiness;
        private IServiceProvider _serviceProvider;
        private IHierarchyMasterBusiness _hierarchyMasterBusiness;
        private IUserHierarchyPermissionBusiness _userHierarchyPermissionBusiness;
        private readonly IRepositoryQueryBase<UserViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<UserListOfValue> _queryUserListRepo;
        private readonly IRepositoryQueryBase<PayrollPersonViewModel> _queryPayPer;
        private readonly IRepositoryQueryBase<LegalEntityViewModel> _querylegal;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;
        public UserBusiness(IRepositoryBase<UserViewModel, User> repo, IHierarchyMasterBusiness hierarchyMasterBusiness, IUserContext userContext
            , IMapper autoMapper, IUserPortalBusiness userportalBusiness, IPortalBusiness portalBusiness, IUserHierarchyPermissionBusiness userHierarchyPermissionBusiness
            , IUserRoleUserBusiness userRoleUserBusiness, IServiceProvider serviceProvider, IRepositoryQueryBase<PayrollPersonViewModel> queryPayPer
            , IRepositoryQueryBase<LegalEntityViewModel> querylegal, ILOVBusiness lovBusiness, IDocumentPermissionBusiness documentPermissionBusiness
            , IRepositoryQueryBase<UserViewModel> queryRepo, IRepositoryQueryBase<UserListOfValue> queryUserListRepo, ICmsQueryBusiness cmsQueryBusiness) : base(repo, autoMapper)
        {
            _userportalBusiness = userportalBusiness;
            _userContext = userContext;
            _hierarchyMasterBusiness = hierarchyMasterBusiness;
            _userHierarchyPermissionBusiness = userHierarchyPermissionBusiness;
            _portalBusiness = portalBusiness;
            _userRoleUserBusiness = userRoleUserBusiness;
            _serviceProvider = serviceProvider;
            _queryRepo = queryRepo;
            _queryUserListRepo = queryUserListRepo;
            _queryPayPer = queryPayPer;
            _querylegal = querylegal;
            _lovBusiness = lovBusiness;
            _documentPermissionBusiness = documentPermissionBusiness;
            _cmsQueryBusiness = cmsQueryBusiness;
        }

        public async Task<UserViewModel> ValidateLogin(string email, string secret)
        {
            var user = await _repo.GetSingleGlobal(x => x.Email.ToLower() == email.ToLower());
            //if (user == null || user.Password != secret)
            if (user == null)
            {
                return null;
            }
            else
            {
                var decryptPass = Helper.Decrypt(user.Password);
                if (decryptPass != secret)
                {
                    return null;
                }
                return await GetUserDetails(user);
            }

        }
        public async Task<UserViewModel> ValidateUser(string email, string legalEntityId = null)
        {
            var user = await _repo.GetSingleGlobal(x => x.Email == email);
            if (user != null)
            {
                if (legalEntityId.IsNotNullAndNotEmpty())
                {
                    user.LegalEntityId = legalEntityId;
                }
                return await GetUserDetails(user);
            }
            return await Task.FromResult<UserViewModel>(null);
        }
        public async Task<UserViewModel> ValidateUserById(string id, string legalEntityId = null)
        {
            var user = await _repo.GetSingleGlobal(x => x.Id == id);
            if (user != null)
            {
                if (legalEntityId.IsNotNullAndNotEmpty())
                {
                    user.LegalEntityId = legalEntityId;
                }
                return await GetUserDetails(user);
            }
            return await Task.FromResult<UserViewModel>(null);
        }
        public async Task<UserViewModel> ValidateUserByUserId(string userId, string legalEntityId = null)
        {
            var user = await _repo.GetSingleGlobal(x => x.UserId == userId);
            if (user != null)
            {
                if (legalEntityId.IsNotNullAndNotEmpty())
                {
                    user.LegalEntityId = legalEntityId;
                }
                return await GetUserDetails(user);
            }
            return await Task.FromResult<UserViewModel>(null);
        }

        private async Task<UserViewModel> GetUserDetails(UserViewModel user)
        {
            var userRoles = await _repo.GetListGlobal<UserRoleUser, UserRoleUser>(x => x.UserId == user.Id,
                                x => x.UserRole);

            user.UserRoles = userRoles.Select(x => x.UserRole).ToArray();
            var company = await _repo.GetSingleGlobal<Company, Company>(x => x.Id == user.CompanyId);
            if (company != null)
            {
                user.CompanyName = company.Name;
                user.CompanyCode = company.Code;
            }
            var userPortals = await _repo.GetList<UserPortalViewModel, UserPortal>(x => x.UserId == user.Id, x => x.Portal);
            if (userPortals != null)
            {
                user.UserPortals = string.Join(",", userPortals.Where(x => x.Portal != null).Select(x => x.Portal.Name).ToList());

                //if (user.UserPortals.Contains("HR"))
                //{
                try
                {
                    var assignment = await _cmsQueryBusiness.GetUserDetailsData(user.Id);
                    if (assignment != null)
                    {
                        user.PersonId = assignment.EmployeeId;
                        user.PositionId = assignment.PositionId;
                        user.DepartmentId = assignment.DepartmentId;
                    }
                }
                catch (Exception ex)
                {

                }
                //}
            }
            if (user.LegalEntityId.IsNotNullAndNotEmpty())
            {
                var le = await _repo.GetSingle<LegalEntityViewModel, LegalEntity>(x => x.Id == user.LegalEntityId);
                if (le != null)
                {
                    user.LegalEntityCode = le.Code;
                }
            }


            return user;
        }

        public async override Task<CommandResult<UserViewModel>> Create(UserViewModel model, bool autoCommit = true)
        {
            var errorList = new Dictionary<string, string>();

            if (model.UserId.IsNotNullAndNotEmpty() && model.PortalName == "DMS")
            {
                var existuserid = await _repo.GetSingle(x => x.UserId == model.UserId);
                var existemail = await _repo.GetSingle(x => x.Email == model.UserId);
                if (existemail.IsNotNull() || existemail.IsNotNull())
                {
                    errorList.Add("UserId", "UserId already exist.");
                }
            }

            if (model.Name.IsNullOrEmpty())
            {
                errorList.Add("Name", "User Name is required.");
            }
            if (model.Email.IsNullOrEmpty())
            {
                errorList.Add("Email", "User Email is required.");
            }
            //if (model.Password.IsNullOrEmpty())
            //{
            //    errorList.Add("Password", "User Password is required.");

            //}
            if (model.Email != null || model.Email != "")
            {
                var user = await _repo.GetSingle(x => x.Email == model.Email);
                if (user != null)
                {
                    errorList.Add("Email", "Email already exist.");
                }
            }
            if (model.Password.IsNotNullAndNotEmpty() && model.ConfirmPassword.IsNotNullAndNotEmpty())
            {
                if (model.Password != model.ConfirmPassword)
                {
                    errorList.Add("ConfirmPassword", "Confirm Password is different from Password");
                }
            }

            if (errorList.Count > 0)
            {
                return CommandResult<UserViewModel>.Instance(model, false, errorList);
            }
            else
            {
                // var data = _autoMapper.Map<UserViewModel>(model);
                var random = new Random();
                if (model.Password.IsNullOrEmpty())
                {
                    model.Password = Convert.ToString(random.Next(10000000, 99999999));
                }
                model.ConfirmPassword = model.Password;
                model.Password = Helper.Encrypt(model.Password);
                model.ConfirmPassword = Helper.Encrypt(model.Password);
                if (model.EnableTwoFactorAuth)
                {
                    model.PasswordChanged = true;
                }
                // model.EnableSummaryEmail = true;
                //if (model.UserType == UserTypeEnum.CANDIDATE || model.UserType == UserTypeEnum.AGENCY)
                //{
                //    model.EnableRegularEmail = true;
                //}
                //else
                //{
                //    model.EnableSummaryEmail = true;
                //}
                var result = await base.Create(model, autoCommit);

                if (model.UserType.IsNotNull())
                {
                    var user = new UserRoleUserViewModel();
                    user.UserId = result.Item.Id;
                    //var role = await _userRoleBusiness.GetSingle(x=>x.Code==model.UserType.ToString());
                    var role = await _repo.GetSingle<UserRoleViewModel, UserRole>(x => x.Code == model.UserType.ToString());

                    if (role != null)
                    {
                        user.UserRoleId = role.Id;
                        await _userRoleUserBusiness.Create(user);
                    }
                }
                if (model.PortalName == "CareerPortal")
                {
                    var portal = await _portalBusiness.GetSingle(x => x.Name == model.PortalName);
                    if (portal != null)
                    {
                        var res = await _userportalBusiness.Create(new UserPortalViewModel
                        {
                            UserId = result.Item.Id,
                            PortalId = portal.Id,
                        });
                    }

                }
                if (result.IsSuccess && model.PortalName == "DMS")
                {
                    await CreateUserWorkSpace(result.Item.Id);
                }
                if (result.IsSuccess && model.SendWelcomeEmail)
                {
                    await SendWelcomeEmail(model);
                }
                return CommandResult<UserViewModel>.Instance(model, result.IsSuccess, result.Messages);
            }
        }

        public async Task CreateUserWorkSpace(string userId)
        {
            var model = new NoteTemplateViewModel
            {
                TemplateCode = "WORKSPACE_GENERAL",
                DataAction = DataActionEnum.Create,
                OwnerUserId = userId,
            };
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            var _cmsBusiness = _serviceProvider.GetService<ICmsBusiness>();
            var _templateBusiness = _serviceProvider.GetService<ITemplateBusiness>();
            var newmodel = await _noteBusiness.GetNoteDetails(model);
            newmodel.NoteSubject = "My Workspace";
            newmodel.NoteDescription = "My Workspace";
            //  var query=$@"SELECT ""Id"" FROM cms.""N_GENERAL_DOCUMENT_GENERALDOCUMENT"" where ""IsDeleted""=false and ""CompanyId""='{_userContext.CompanyId}' limit 1";
            // var generalDocumentType = await _queryRepo.ExecuteScalar<string>(query, null);

            var type = await _lovBusiness.GetSingle(x => x.Code == "MY_WORKSPACE" && x.IsDeleted == false);

            dynamic exo = new System.Dynamic.ExpandoObject();

            if (type.IsNotNull())
            {
                ((IDictionary<String, Object>)exo).Add("TypeId", type.Id);
            }
            // newmodel.MultipleReference = list;
            //newmodel.MultipleReferenceType = ReferenceTypeEnum.NTS_TemplateMaster;
            //newmodel.MultipleReferenceNodeType = NodeEnum.NTS_TemplateMaster;
            newmodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
            newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
            var myworkspaceId = await _noteBusiness.ManageNote(newmodel);
            if (myworkspaceId.IsSuccess)
            {
                var noteTempModel1 = new NoteTemplateViewModel
                {
                    TemplateCode = "WORKSPACE_DOC_TYPE",
                    DataAction = DataActionEnum.Create,
                    OwnerUserId = userId,
                    ActiveUserId = _userContext.UserId
                };
                var notemodel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);
                var templateId = await _templateBusiness.GetSingle(x => x.Code == "GENERAL_DOCUMENT");
                dynamic exo1 = new System.Dynamic.ExpandoObject();
                if (templateId.IsNotNull())
                {
                    ((IDictionary<String, Object>)exo1).Add("DocumentTypeId", templateId.Id);
                }

                notemodel1.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo1);
                notemodel1.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                notemodel1.ParentNoteId = myworkspaceId.Item.NoteId;
                var result1 = await _noteBusiness.ManageNote(notemodel1);
                var permissionData = new DocumentPermissionViewModel
                {
                    PermissionType = DmsPermissionTypeEnum.Allow,
                    Access = DmsAccessEnum.FullAccess,
                    AppliesTo = DmsAppliesToEnum.ThisFolderSubFoldersAndFiles,
                    //InheritedFrom = "None",
                    PermittedUserId = userId,
                    NoteId = myworkspaceId.Item.NoteId,
                };
                var docperm = await _documentPermissionBusiness.Create(permissionData);
            }
        }

        public async Task SendWelcomeEmail(UserViewModel model)
        {
            model.Password = Helper.Decrypt(model.Password);
            var _notificationBusiness = _serviceProvider.GetService<INotificationBusiness>();
            var notificationTemplateModel = await _repo.GetSingleGlobal<NotificationTemplateViewModel, NotificationTemplate>(x => x.CompanyId == model.CompanyId && x.Code == "WELCOME_EMAIL");
            if (notificationTemplateModel.IsNotNull())
            {

                var viewModel = new NotificationViewModel()
                {
                    To = model.Email,
                    ToUserId = model.Id,
                    FromUserId = model.CreatedBy,
                    Subject = notificationTemplateModel.Subject,
                    Body = notificationTemplateModel.Body,
                    SendAlways = true,
                    NotifyByEmail = true,
                    DynamicObject = model
                };
                if (viewModel.Body.Contains("{{user-password}}"))
                {
                    viewModel.Body = viewModel.Body.Replace("{{user-password}}", model.Password);
                }
                var portaluser = await _userportalBusiness.GetSingle(x => x.UserId == model.Id);
                if (portaluser != null && portaluser.PortalId.IsNotNullAndNotEmpty())
                {
                    var attachments = await _portalBusiness.GetSingleById(portaluser.PortalId);
                    if (attachments != null && attachments.UserGuideId.IsNotNullAndNotEmpty())
                    {
                        viewModel.AttachmentIds = new string[1];
                        viewModel.AttachmentIds[0] = attachments.UserGuideId;
                    }
                }
                await _notificationBusiness.Create(viewModel);
            }

        }

        public async override Task<CommandResult<UserViewModel>> Edit(UserViewModel model, bool autoCommit = true)
        {
            var errorList = new Dictionary<string, string>();

            if (model.UserId.IsNotNullAndNotEmpty() && model.PortalName == "DMS")
            {
                var existuserid = await _repo.GetSingle(x => x.UserId == model.UserId && x.Id != model.Id);
                var existemail = await _repo.GetSingle(x => x.Email == model.UserId && x.Id != model.Id);
                if (existemail.IsNotNull() || existuserid.IsNotNull())
                {
                    errorList.Add("UserId", "UserId already exist.");

                }
            }
            if (model.Name.IsNullOrEmpty())
            {
                errorList.Add("Name", "User Name is required.");

            }
            if (model.Email.IsNullOrEmpty())
            {
                errorList.Add("Email", "User Email is required.");

            }
            if (model.Password.IsNullOrEmpty())
            {
                errorList.Add("Password", "User Password is required.");

            }
            if (model.Email != null || model.Email != "")
            {
                var user = await _repo.GetSingle(x => x.Email == model.Email && x.Id != model.Id);
                if (user != null)
                {
                    errorList.Add("Email", "Email already exist.");

                }

            }
            if (model.Password.IsNotNullAndNotEmpty() && model.ConfirmPassword.IsNotNullAndNotEmpty())
            {
                if (model.Password != model.ConfirmPassword)
                {
                    errorList.Add("ConfirmPassword", "Confirm Password is different from Password");
                }
            }

            if (errorList.Count > 0)
            {
                return CommandResult<UserViewModel>.Instance(model, false, errorList);
            }
            else
            {
                if (model.EnableTwoFactorAuth)
                {
                    model.PasswordChanged = true;
                }
                var data = _autoMapper.Map<UserViewModel>(model);
                var result = await base.Edit(data, autoCommit);
                //return result;
                return CommandResult<UserViewModel>.Instance(model, result.IsSuccess, result.Messages);
            }
        }

        public async Task<CommandResult<ChangePassowrdViewModel>> ChangePassword(ChangePassowrdViewModel model)
        {

            if (string.IsNullOrEmpty(model.UserId))
            {
                model.UserId = _repo.UserContext.UserId;
            }
            var user = await _repo.GetSingleById(model.UserId);
            if (user == null)
            {
                return CommandResult<ChangePassowrdViewModel>.Instance(model, x => x.CurrentPassword, "Invalid user");
            }
            var decryptPass = Helper.Decrypt(user.Password);
            //var decryptConfirmPass = Helper.Decrypt(user.Password);
            //if (user.Password != model.CurrentPassword)
            if (decryptPass != model.CurrentPassword)
            {
                return CommandResult<ChangePassowrdViewModel>.Instance(model, x => x.CurrentPassword, "Current password is not correct");
            }
            //else if (user.Password == model.NewPassword)
            var regex = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&*()])(?!.*userName)(?!.*(.)\\1{2,}).{8,}$";
            var match = Regex.Match(model.NewPassword, regex);
            if (!match.Success)
            {
                return CommandResult<ChangePassowrdViewModel>.Instance(model, x => x.NewPassword, "New Password must be at least 8 characters and must contain the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)");
            }
            else if (decryptPass == model.NewPassword)
            {
                return CommandResult<ChangePassowrdViewModel>.Instance(model, x => x.NewPassword, "New password should not be same as old password");
            }
            else if (model.NewPassword != model.ConfirmPassword)
            {
                return CommandResult<ChangePassowrdViewModel>.Instance(model, x => x.NewPassword, "New Password and Confirm Password does not match");
            }
            else
            {


                if (!match.Success)
                {
                    // does not match
                }
                user.Password = model.NewPassword;
                user.ConfirmPassword = model.ConfirmPassword;
                user.Password = Helper.Encrypt(model.NewPassword);
                user.ConfirmPassword = Helper.Encrypt(model.ConfirmPassword);
                user.PasswordChanged = true;
                await this.Edit(user);
            }
            return CommandResult<ChangePassowrdViewModel>.Instance(model);

        }

        public async Task ChangeUserProfilePhoto(string photoId, string userId)
        {
            var user = await _repo.GetSingleById(photoId);
            if (user != null)
            {
                user.PhotoId = photoId;
                await this.Edit(user);
            }
        }

        public async Task ChangeUserSignature(string signId, string userId)
        {
            var user = await _repo.GetSingleById(signId);
            if (user != null)
            {
                user.SignatureId = signId;
                await this.Edit(user);
            }
        }

        public async Task<List<IdNameViewModel>> GetUserIdNameList()
        {
            List<IdNameViewModel> list = new List<IdNameViewModel>();
            var userslist = await GetList();
            list = userslist.Select(x => new IdNameViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
            return list;
        }
        public async Task<List<IdNameViewModel>> GetUserIdNameListForPortal()
        {
            List<IdNameViewModel> list = new List<IdNameViewModel>();
            var userslist = await GetList(x => x.LegalEntityId == _userContext.LegalEntityId && x.PortalId == _userContext.PortalId);
            list = userslist.Select(x => new IdNameViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
            return list;
        }

        public async Task<UserViewModel> GetGuestUser(string companyId)
        {
            var user = await _repo.GetSingleGlobal(x => x.IsDeleted == false && x.CompanyId == companyId && x.IsGuestUser == true);
            return user;
        }

        //public async Task<CommandResult<UserViewModel>> CreateUser(UserViewModel model)
        //{
        //    if (model.UserType == UserTypeEnum.CANDIDATE || model.UserType == UserTypeEnum.AGENCY)
        //    {
        //        model.EnableRegularEmail = true;
        //    }
        //    else
        //    {
        //        model.EnableSummaryEmail = true;
        //    }
        //    var result = await base.Create(model,autoCommit);
        //    if (model.UserType.IsNotNull())
        //    {
        //        var user = new UserRoleUserViewModel();
        //        user.UserId = result.Item.Id;
        //        //var role = await _userRoleBusiness.GetSingle(x=>x.Code==model.UserType.ToString());
        //        var role = await _repo.GetSingle<UserRoleViewModel, UserRole>(x => x.Code == model.UserType.ToString());

        //        if (role != null)
        //        {
        //            user.UserRoleId = role.Id;
        //            await _userRoleUserBusiness.Create(user);
        //        }
        //    }
        //    return CommandResult<UserViewModel>.Instance(model, result.IsSuccess, result.Messages);
        //}

        public async Task<CommandResult<UserViewModel>> EditUser(UserViewModel model, bool autoCommit = true)
        {
            var result = await base.Edit(model, autoCommit);
            return CommandResult<UserViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<UserViewModel> ConfirmEmailOTP(string email)
        {
            var user = await _repo.GetSingleGlobal(x => x.Email == email);

            if (user != null)
            {
                var model = new UserViewModel();
                var random = new Random();
                string otp = Convert.ToString(random.Next(10000000, 99999999));
                user.ForgotPasswordOTP = otp;
                user.ConfirmPassword = user.Password;
                var res = await Edit(user);

                return user;
            }
            return null;
        }
        public async Task<UserViewModel> TwoFactorAuthOTP(string email)
        {
            var user = await _repo.GetSingleGlobal(x => x.Email == email);

            if (user != null)
            {
                //if (user.EnableTwoFactorAuth)
                //{
                var model = new UserViewModel();
                var random = new Random();
                string otp = Convert.ToString(random.Next(100000, 999999));
                user.TwoFactorAuthOTP = otp;
                user.OTPExpiry = DateTime.Now.AddMinutes(10);
                var res = await base.Edit(user);
                //}
                return user;
            }
            return null;
        }
        public async Task<UserViewModel> ConfirmPasswordChange(string email, string password)
        {
            var user = await _repo.GetSingleGlobal(x => x.Email == email);

            try
            {
                if (user != null)
                {
                    user.ForgotPasswordOTP = null;
                    user.Password = Helper.Encrypt(password);
                    user.ConfirmPassword = Helper.Encrypt(password);
                    var res = await Edit(user);
                    return user;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<IList<UserViewModel>> GetUserList()
        {
            var list = await _cmsQueryBusiness.GetUserListData();
            return list;
        }
        public async Task<IList<UserViewModel>> GetUserListForPortal()
        {
            var list = await _cmsQueryBusiness.GetUserListForPortalData();
            return list;
        }
        public async Task<IList<UserViewModel>> GetActiveUserList()
        {
            var list = await _cmsQueryBusiness.GetActiveUserListData();
            return list;
        }


        public async Task<PagedList<UserViewModel>> GetActiveUserListForSwitch(string filter, int pageSize, int pageNumber, string indexedItemId = null)
        {

            var list = await _cmsQueryBusiness.GetActiveUserListForSwitchData(filter, pageSize, pageNumber, indexedItemId);
            return list;


        }
        public async Task<IList<UserViewModel>> GetActiveUserListForSwitchProfile()
        {
            var list = await _cmsQueryBusiness.GetActiveUserListForSwitchProfileData();
            return list;
        }

        public async Task<PagedList<UserViewModel>> GetActiveUsersListForSwitchProfile(string filter, int pageSize, int pageNumber, string indexedItemId = null)
        {
            var data = await _cmsQueryBusiness.GetActiveUsersListForSwitchProfileData(filter, pageSize, pageNumber, indexedItemId);
            return data;
        }
        public async Task<IList<UserViewModel>> GetActiveUserListForPortal()
        {
            var list = await _cmsQueryBusiness.GetActiveUserListForPortalData();
            return list;
        }
        public async Task<IList<UserViewModel>> GetUserTeamList(string id)
        {
            var list = await _cmsQueryBusiness.GetUserTeamListData(id);
            return list;
        }
        public async Task<IList<UserViewModel>> GetUserTeamListForPortal()
        {
            var list = await _cmsQueryBusiness.GetUserTeamListForPortalData();
            return list;
        }
        public async Task<IList<UserViewModel>> GetUserListForEmailSummary()
        {
            var list = await _cmsQueryBusiness.GetUserListForEmailSummaryData();
            return list;
        }
        public async Task<IList<UserViewModel>> GetSwitchToUserList(string userId, string loggedinAsByUserId, bool hasLoggedInAsPermission, string searchParam)
        {
            var list = new List<UserViewModel>();
            if (hasLoggedInAsPermission)
            {
                var userList = await GetActiveUserListForSwitchProfile();
                list = userList.ToList();
            }
            else
            {

                var grantedList = await _cmsQueryBusiness.GetSwitchToUserListData(userId);
                var user = await GetActiveUserList();
                var result =
                   (from u in user
                    join g in grantedList
                        on u.Id equals g.CreatedBy
                    select u
                    ).ToList();

                list = result;

            }
            if (list != null && list.Count > 0)
            {
                if (userId != loggedinAsByUserId)
                {
                    list.Insert(0, new UserViewModel { Id = loggedinAsByUserId, Name = "Switch To Actual User" });
                }
            }
            return list;
        }

        public async Task<PagedList<UserViewModel>> GetSwitchUserList(string userId, string loggedinAsByUserId, bool hasLoggedInAsPermission, string filter, int pageSize, int pageNumber, string indexedItemId = null)
        {
            var list = PagedList<UserViewModel>.Instance();
            if (hasLoggedInAsPermission)
            {
                var userList = await GetActiveUsersListForSwitchProfile(filter, pageSize, pageNumber, indexedItemId);
                list = userList;
            }
            else
            {

                var grantedList = await _cmsQueryBusiness.GetSwitchUserListData(userId);
                var user = await GetActiveUserListForSwitch(filter, pageSize, pageNumber, indexedItemId);
                var result =
                   (from u in user.Data
                    join g in grantedList
                        on u.Id equals g.CreatedBy
                    select u
                    ).ToList();

                list = PagedList<UserViewModel>.Instance(user.Total, result);

            }
            if (list != null && list.Data.Count > 0)
            {
                if (userId != loggedinAsByUserId)
                {
                    list.Data.Insert(0, new UserViewModel { Id = loggedinAsByUserId, Name = "Switch To Actual User" });
                    list.Total = list.Total + 1;
                }
            }
            return list;
        }

        public async Task<IdNameViewModel> GetPersonWithSponsor(string userId)
        {
            return await _cmsQueryBusiness.GetPersonWithSponsorData(userId);
        }
        public async Task<IList<UserListOfValue>> ViewModelList(string userId)
        {
            return await _cmsQueryBusiness.ViewModelListData(userId);

        }

        private async Task<IList<UserHierarchyPermissionViewModel>> GetHierarchyPermissions(string userId, string hierarchyCode)
        {
            var list = new List<UserHierarchyPermissionViewModel>();
            var user = await _repo.GetSingleById(userId);
            if (user.IsNotNull() && user.Status == StatusEnum.Active)
            {
                var hierarchy = await _hierarchyMasterBusiness.GetSingle(x => x.Code == hierarchyCode);
                if (hierarchy.IsNotNull())
                {
                    list = await _userHierarchyPermissionBusiness.GetList(x => x.UserId == user.Id && x.HierarchyId == hierarchy.Id);
                }

            }
            return list.ToList();
        }

        public async Task<Tuple<string, string, string>> GetHierarchyRootId(string userId, string hierarchyCode, string userHierarchyId, DateTime? asofDate = null)
        {
            string allowedRootId = "";
            string hierarchyRootId = "";
            var hiearchyPermissions = await GetHierarchyPermissions(userId, hierarchyCode);
            var hierarchy = await _hierarchyMasterBusiness.GetSingle(x => x.Code == hierarchyCode);
            if (hierarchy != null)
            {
                var date = asofDate ?? DateTime.Now.Date;

                switch (hierarchy.HierarchyType)
                {
                    case HierarchyTypeEnum.Position:
                        hierarchyRootId = hierarchy.RootNodeId;
                        if (hiearchyPermissions.Any(x => x.HierarchyPermission == HierarchyPermissionEnum.All))
                        {
                            allowedRootId = hierarchyRootId;
                        }
                        else if (hiearchyPermissions.Any(x => x.HierarchyPermission == HierarchyPermissionEnum.Custom))
                        {
                            var permission = hiearchyPermissions.FirstOrDefault(x => x.HierarchyPermission == HierarchyPermissionEnum.Custom);
                            if (permission != null)
                            {
                                //allowedRootId = permission.CustomPermissionId ?? userHierarchyId ?? allowedRootId;
                                allowedRootId = userHierarchyId ?? allowedRootId;
                            }
                            else
                            {
                                allowedRootId = userHierarchyId ?? allowedRootId;
                            }

                        }
                        else if (hiearchyPermissions.Any(x => x.HierarchyPermission == HierarchyPermissionEnum.Parent))
                        {

                            var parentPositionRoot = await _cmsQueryBusiness.GetHierarchyRootIdData(userHierarchyId, hierarchy.Id);
                            if (parentPositionRoot != null)
                            {
                                allowedRootId = parentPositionRoot;
                            }
                            else
                            {
                                allowedRootId = userHierarchyId ?? allowedRootId;
                            }

                        }
                        else
                        {
                            allowedRootId = userHierarchyId ?? allowedRootId;
                        }

                        break;
                    case HierarchyTypeEnum.User:
                        hierarchyRootId = hierarchy.RootNodeId;
                        if (hiearchyPermissions.Any(x => x.HierarchyPermission == HierarchyPermissionEnum.All))
                        {
                            allowedRootId = hierarchyRootId;
                        }
                        else if (hiearchyPermissions.Any(x => x.HierarchyPermission == HierarchyPermissionEnum.Custom))
                        {
                            var permission = hiearchyPermissions.FirstOrDefault(x => x.HierarchyPermission == HierarchyPermissionEnum.Custom);
                            if (permission != null)
                            {
                                //allowedRootId = permission.CustomPermissionId ?? userHierarchyId ?? allowedRootId;
                                allowedRootId = userHierarchyId ?? allowedRootId;
                            }
                            else
                            {
                                allowedRootId = userHierarchyId ?? allowedRootId;
                            }

                        }
                        else if (hiearchyPermissions.Any(x => x.HierarchyPermission == HierarchyPermissionEnum.Parent))
                        {

                            var parentPositionRoot = await _cmsQueryBusiness.GetHierarchyRootIdData1(userHierarchyId, hierarchy.Id);
                            if (parentPositionRoot != null)
                            {
                                allowedRootId = parentPositionRoot;
                            }
                            else
                            {
                                allowedRootId = userHierarchyId ?? allowedRootId;
                            }

                        }
                        else
                        {
                            allowedRootId = userHierarchyId ?? allowedRootId;
                        }

                        break;
                    case HierarchyTypeEnum.Organization:

                        hierarchyRootId = hierarchy.RootNodeId;

                        if (hiearchyPermissions.Any(x => x.HierarchyPermission == HierarchyPermissionEnum.All))
                        {
                            allowedRootId = hierarchyRootId;
                        }
                        else if (hiearchyPermissions.Any(x => x.HierarchyPermission == HierarchyPermissionEnum.Custom))
                        {
                            var permission = hiearchyPermissions.FirstOrDefault(x => x.HierarchyPermission == HierarchyPermissionEnum.Custom);
                            if (permission != null)
                            {
                                //allowedRootId = permission.CustomPermissionId ?? userHierarchyId ?? allowedRootId;
                                allowedRootId = userHierarchyId ?? allowedRootId;
                            }
                            else
                            {
                                allowedRootId = userHierarchyId ?? allowedRootId;
                            }
                        }
                        else if (hiearchyPermissions.Any(x => x.HierarchyPermission == HierarchyPermissionEnum.LegalEntity))
                        {
                            var LegalEntityId = _userContext.LegalEntityId;
                            allowedRootId = LegalEntityId;
                        }
                        else if (hiearchyPermissions.Any(x => x.HierarchyPermission == HierarchyPermissionEnum.Parent))
                        {

                            var parentOrganizationRoot = await _cmsQueryBusiness.GetHierarchyRootIdData2(userHierarchyId, hierarchy.Id);
                            if (parentOrganizationRoot != null)
                            {
                                allowedRootId = parentOrganizationRoot;
                            }
                            else
                            {
                                allowedRootId = userHierarchyId ?? allowedRootId;
                            }

                        }
                        else
                        {
                            allowedRootId = userHierarchyId ?? allowedRootId;
                        }
                        break;
                    default:
                        break;
                }

            }
            return new Tuple<string, string, string>(hierarchyRootId, allowedRootId, hierarchy.Id);

        }
        public async Task<List<double>> GetUserNodeLevel(string userId)
        {
            var queryData = await _cmsQueryBusiness.GetUserNodeLevelData(userId);
            var list = queryData;
            return list;
        }
        public async Task<List<PositionChartViewModel>> GetUserHierarchy(string parentId, int levelUpto, string c, int level = 1, int option = 1)
        {

            var queryData = await _cmsQueryBusiness.GetUserHierarchyData(parentId, levelUpto, c, level, option);
            var list = queryData;
            return list;
        }
        public async Task<List<PositionChartViewModel>> GetUserHierarchyChartData(string parentId, int levelUpto, string c, int level = 1, int option = 1)
        {

            var queryData = await _cmsQueryBusiness.GetUserHierarchyChartData(parentId, levelUpto, c, level, option);
            var list = queryData;
            return list;
        }
        public async Task<string> GetPersonDateOfJoining(string userId)
        {
            var joinDate = await _cmsQueryBusiness.GetPersonDateOfJoiningData(userId);
            if (joinDate.IsNotNull())
            {
                return joinDate.ToString();
            }
            else
            {
                return null;
            }

        }

        public async Task<List<LegalEntityViewModel>> GetEntityByIds(string legalEntity)
        {
            var list = await _cmsQueryBusiness.GetEntityByIdsData(legalEntity);
            return list;
        }
        public async Task<List<UserPermissionViewModel>> ViewUserPermissions(string userId)
        {

            var list = await _cmsQueryBusiness.ViewUserPermissionsData(userId);
            return list;
        }
        public async Task UpdateHierarchyLevel(string hierarchyId, string userId, string levelUserId, int levelNo, int optionNo, DateTime esd, DateTime eed)
        {
            var _userHierarchyBussiness = _serviceProvider.GetService<IUserHierarchyBusiness>();
            if (levelUserId.IsNotNullAndNotEmpty())
            {
                var userHierarchy = await _repo.GetSingle<UserHierarchyViewModel, UserHierarchy>(x => x.HierarchyMasterId == hierarchyId && x.OptionNo == optionNo && x.UserId == userId && x.LevelNo == levelNo);
                if (userHierarchy != null)
                {
                    await _repo.Delete<UserHierarchyViewModel, UserHierarchy>(userHierarchy.Id);
                }
                var model = new UserHierarchyViewModel();
                model.HierarchyMasterId = hierarchyId;
                model.UserId = userId;
                model.ParentUserId = levelUserId;
                model.OptionNo = optionNo;
                model.LevelNo = levelNo;
                var result = await _userHierarchyBussiness.Create(model);
            }
        }

        public async Task<IList<UserViewModel>> GetUserListWithEmailText()
        {
            var list = await _cmsQueryBusiness.GetUserListWithEmailTextData();
            return list;
        }
        public async Task<List<UserViewModel>> GetAllUsersWithPortalId(string PortalId)
        {
            var list = await _cmsQueryBusiness.GetAllUsersWithPortalIdData(PortalId);
            return list;

        }
        public async Task<List<UserViewModel>> GetAllCSCUsersList()
        {
            var list = await _cmsQueryBusiness.GetAllCSCUsersList();
            return list;
        }
        public async Task<List<UserViewModel>> GetUsersWithPortalIds()
        {
            var list = await _cmsQueryBusiness.GetUsersWithPortalIdsData();
            return list;

        }
        public async Task<List<PortalViewModel>> GetAllowedPortalList(string userId)
        {
            var list = await _cmsQueryBusiness.GetAllowedPortalListData(userId);
            return list;
        }

        public async Task<List<UserViewModel>> GetDMSPermiisionusersList(string PortalId, string LegalEntityId)
        {
            var list = await _cmsQueryBusiness.GetDMSPermiisionusersListData(PortalId, LegalEntityId);
            return list;

        }
        public async Task<Tuple<string, string, string>> GetUserHierarchyRootId(string userId, string hierarchyCode, string userHierarchyId, DateTime? asofDate = null)
        {
            string allowedRootId = "";
            string hierarchyRootId = "";
            var hiearchyPermissions = await GetHierarchyPermissions(userId, hierarchyCode);
            var hierarchy = await _hierarchyMasterBusiness.GetSingle(x => x.Code == hierarchyCode);
            if (hierarchy != null)
            {
                var date = asofDate ?? DateTime.Now.Date;

                switch (hierarchy.HierarchyType)
                {
                    case HierarchyTypeEnum.Hybrid:
                        hierarchyRootId = "-1";
                        if (hiearchyPermissions.Any(x => x.HierarchyPermission == HierarchyPermissionEnum.Custom))
                        {
                            var permission = hiearchyPermissions.FirstOrDefault(x => x.HierarchyPermission == HierarchyPermissionEnum.Custom);
                            if (permission != null)
                            {
                                allowedRootId = permission.CustomRootId;
                            }
                        }
                        break;
                    case HierarchyTypeEnum.User:

                        hierarchyRootId = hierarchy.RootNodeId;

                        if (hiearchyPermissions.Any(x => x.HierarchyPermission == HierarchyPermissionEnum.All))
                        {
                            allowedRootId = hierarchyRootId;
                        }
                        else if (hiearchyPermissions.Any(x => x.HierarchyPermission == HierarchyPermissionEnum.Custom))
                        {
                            var permission = hiearchyPermissions.FirstOrDefault(x => x.HierarchyPermission == HierarchyPermissionEnum.Custom);
                            if (permission != null)
                            {
                                //allowedRootId = permission.CustomPermissionId ?? userHierarchyId ?? allowedRootId;
                                allowedRootId = userHierarchyId ?? allowedRootId;
                            }
                            else
                            {
                                allowedRootId = userHierarchyId ?? allowedRootId;
                            }
                        }
                        else if (hiearchyPermissions.Any(x => x.HierarchyPermission == HierarchyPermissionEnum.LegalEntity))
                        {
                            var LegalEntityId = _userContext.LegalEntityId;
                            allowedRootId = LegalEntityId;
                        }
                        else if (hiearchyPermissions.Any(x => x.HierarchyPermission == HierarchyPermissionEnum.Parent))
                        {

                            var parentOrganizationRoot = await _cmsQueryBusiness.GetUserHierarchyRootIdData(userHierarchyId, hierarchy.Id);
                            if (parentOrganizationRoot != null)
                            {
                                allowedRootId = parentOrganizationRoot;
                            }
                            else
                            {
                                allowedRootId = userHierarchyId ?? allowedRootId;
                            }

                        }
                        else
                        {
                            allowedRootId = userHierarchyId ?? allowedRootId;
                        }
                        break;
                    default:
                        break;
                }

            }
            return new Tuple<string, string, string>(hierarchyRootId, allowedRootId, hierarchy.Id);

        }

        public async Task<PageViewModel> GetUserLandingPage(string userId, string portalId)
        {
            var list = await _cmsQueryBusiness.GetUserLandingPage(userId, portalId);
            if (list == null)
            {
                list = await _cmsQueryBusiness.GetUserRoleLandingPage(userId, portalId);
            }
            return list;
        }

        public async Task<List<UserPreferenceViewModel>> GetUserPreferences(string userId)
        {
            var list = await _cmsQueryBusiness.GetUserPreferences(userId);
            return list;

        }
    }
}
