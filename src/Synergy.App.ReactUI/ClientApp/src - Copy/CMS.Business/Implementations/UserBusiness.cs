using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace CMS.Business
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
        public UserBusiness(IRepositoryBase<UserViewModel, User> repo, IHierarchyMasterBusiness hierarchyMasterBusiness, IUserContext userContext
            , IMapper autoMapper, IUserPortalBusiness userportalBusiness, IPortalBusiness portalBusiness, IUserHierarchyPermissionBusiness userHierarchyPermissionBusiness
            , IUserRoleUserBusiness userRoleUserBusiness, IServiceProvider serviceProvider, IRepositoryQueryBase<PayrollPersonViewModel> queryPayPer
            , IRepositoryQueryBase<LegalEntityViewModel> querylegal, ILOVBusiness lovBusiness, IDocumentPermissionBusiness documentPermissionBusiness
            , IRepositoryQueryBase<UserViewModel> queryRepo, IRepositoryQueryBase<UserListOfValue> queryUserListRepo) : base(repo, autoMapper)
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
                    string query = $@"select a.* from  cms.""N_CoreHR_HRAssignment"" as a
            join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false
            join public.""User"" as u on p.""UserId""=u.""Id"" and u.""IsDeleted""=false
            where u.""Id""='{user.Id}' and a.""IsDeleted""=false";
                    var assignment = await _queryRepo.ExecuteQuerySingle<AssignmentViewModel>(query, null);
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

        public async override Task<CommandResult<UserViewModel>> Create(UserViewModel model)
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
                model.Password = Convert.ToString(random.Next(10000000, 99999999));
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
                var result = await base.Create(model);

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

        public async override Task<CommandResult<UserViewModel>> Edit(UserViewModel model)
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
            if (model.Password != model.ConfirmPassword)
            {
                errorList.Add("ConfirmPassword", "Confirm Password is different from Password");
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
                var result = await base.Edit(data);
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
        //    var result = await base.Create(model);
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

        public async Task<CommandResult<UserViewModel>> EditUser(UserViewModel model)
        {
            var result = await base.Edit(model);
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
                if (user.EnableTwoFactorAuth)
                {
                    var model = new UserViewModel();
                    var random = new Random();
                    string otp = Convert.ToString(random.Next(100000, 999999));
                    user.TwoFactorAuthOTP = otp;
                    user.OTPExpiry = DateTime.Now.AddMinutes(10);
                    var res = await base.Edit(user);
                }
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
            string query = @$"SELECT * ,CONCAT(""Name"",'<',""Email"",'>') as Name
                            FROM public.""User""
                            where ""IsDeleted""=false";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return list;
        }
        public async Task<IList<UserViewModel>> GetUserListForPortal()
        {
            string query = @$"SELECT * 
                            FROM public.""User""
                            where ""IsDeleted""=false and ""PortalId""='{_repo.UserContext.PortalId}' and ""LegalEntityId""='{_repo.UserContext.LegalEntityId}' and ""CompanyId""='{_repo.UserContext.CompanyId}'";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return list;
        }
        public async Task<IList<UserViewModel>> GetActiveUserList()
        {
            string query = @$"SELECT * 
                            FROM public.""User""
                            where ""IsDeleted""=false and ""Status""=1 ";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return list;
        }
        public async Task<IList<UserViewModel>> GetActiveUserListForSwitchProfile()
        {
            string query = @$"SELECT u.* 
                            FROM public.""User"" as u 
join public.""UserPortal"" as up on up.""UserId""=u.""Id"" and up.""IsDeleted""=false
                            where u.""IsDeleted""=false and u.""Status""=1 and up.""PortalId""='{_userContext.PortalId}'";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return list;
        }
        public async Task<IList<UserViewModel>> GetActiveUserListForPortal()
        {
            string query = @$"SELECT * 
                            FROM public.""User""
                            where ""IsDeleted""=false and ""Status""=1 and ""PortalId""='{_repo.UserContext.PortalId}' and ""LegalEntityId""='{_repo.UserContext.LegalEntityId}'  and ""CompanyId""='{_repo.UserContext.CompanyId}'";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return list;
        }
        public async Task<IList<UserViewModel>> GetUserTeamList(string id)
        {
            string query = @$"SELECT u.* ,CONCAT(u.""Name"",'<',u.""Email"",'>') as Name
                            FROM public.""TeamUser"" as tu
                            inner join public.""Team"" as t on t.""Id""=tu.""TeamId"" and t.""IsDeleted""=false
                            inner join public.""User"" as u on u.""Id""=tu.""UserId"" and u.""IsDeleted""=false
                            where tu.""IsDeleted""=false and t.""Id""='{id}'";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return list;
        }
        public async Task<IList<UserViewModel>> GetUserTeamListForPortal()
        {
            string query = @$"SELECT u.* 
                            FROM public.""TeamUser"" as tu
                            inner join public.""Team"" as t on t.""Id""=tu.""TeamId"" and t.""IsDeleted""=false and tu.""IsDeleted""=false and tu.""CompanyId""='{_repo.UserContext.CompanyId}'
                            inner join public.""User"" as u on u.""Id""=tu.""UserId"" and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where u.""IsDeleted""=false and u.""PortalId""='{_repo.UserContext.PortalId}' and u.""LegalEntityId""='{_repo.UserContext.LegalEntityId}'";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return list;
        }
        public async Task<IList<UserViewModel>> GetUserListForEmailSummary()
        {
            string query = @$"SELECT * 
                            FROM public.""User""
                            where ""IsDeleted""=false and ""EnableSummaryEmail""=true 
                            and ""Id"" not in (select ""ToUserId"" from public.""Notification"" where ""Subject"" Like 'Synergy Summary%' COLLATE ""tr-TR-x-icu"" and date(""CreatedDate"") =CURRENT_DATE  
                            ) ";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
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
                string query = @$"SELECT u.*,coalesce(ga.""CreatedBy"",u.""Id"") as CreatedBy
                            FROM public.""GrantAccess"" as ga
                            right join public.""User"" as u on u.""Id""=ga.""UserId"" and u.""IsDeleted""=false
join public.""UserPortal"" as up on up.""UserId""=u.""Id"" and up.""IsDeleted""=false
                            where u.""Id""='{userId}' and (ga.""GrantStatus""=0 or ga.""GrantStatus"" is null) and ga.""IsDeleted""=false and up.""PortalId""='{_userContext.PortalId}'";
                var grantedList = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
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

        public async Task<IdNameViewModel> GetPersonWithSponsor(string userId)
        {
            var query = $@"Select p.Id as Id, sp.Code as Code
from public.""User"" as u 
join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and u.""IsDeleted""=false and p.""IsDeleted""=false
left join cms.""N_CoreHR_HRContract"" as c on c.""EmployeeId""=p.""Id"" and c.""IsDeleted""=false
left join cms.""N_CoreHR_HRSponsor"" as sp on c.""SponsorId""=s.""Id"" and sp.""IsDeleted""=false
where c.EffectiveStartDate::TIMESTAMP::DATE <= Now()::TIMESTAMP::DATE <= c.EffectiveEndDate::TIMESTAMP::DATE
and u.""Id""='{userId}'

";
            //            var cypher = @"match(u:ADM_User{IsDeleted:0,Status:{Status},CompanyId:{CompanyId},Id:{UserId}})
            //                match(u)-[:R_User_PersonRoot]-(pr:HRS_PersonRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //               optional match(pr)<-[:R_ContractRoot_PersonRoot]-(cr:HRS_ContractRoot)-[:R_ContractRoot]-(contract:HRS_Contract) 
            //where datetime(contract.EffectiveStartDate) <= datetime() <= datetime(contract.EffectiveEndDate)
            //optional match(contract)-[:R_Contract_Sponsor]->(sp:HRS_Sponsor) 
            //                return pr.Id as Id, sp.Code as Code";

            //var prms = new Dictionary<string, object>
            //{
            //    { "Status", StatusEnum.Active.ToString() },
            //    { "CompanyId", CompanyId },
            //    { "UserId", userId },
            //};
            return await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
        }
        public async Task<IList<UserListOfValue>> ViewModelList(string userId)
        {

            var cypher = $@"Select u.""Id"" as Id,u.""Name"" as UserName,u.""Id"" as UserId,p.""Id"" as PersonId
                ,p.""PersonFullName"" as Name
                ,p.""SponsorshipNo"" as SponsorshipNo,u.""Email"" as Email,p.""PersonNo"" as PersonNo
                ,po.""PositionName"" as PositionName,o.""DepartmentName"" as OrganizationName,j.""JobTitle"" as JobName,g.""GradeName"" as GradeName
                ,u.""PhotoId"" as PhotoId,po.""Id"" as PositionId
from   public.""User"" as u 
left join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false
left join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false
left join cms.""N_CoreHR_HRJob"" as j on j.""Id"" = a.""JobId"" and j.""IsDeleted""=false
left join cms.""N_CoreHR_HRDepartment"" as o on o.""Id""=a.""DepartmentId"" and o.""IsDeleted""=false
and  o.""EffectiveStartDate""::Date <= '{DateTime.Now.ApplicationNow().Date}'::Date 
and '{DateTime.Now.ApplicationNow().Date}'::Date <= o.""EffectiveEndDate""::Date
left join cms.""N_CoreHR_HRGrade"" as g on g.""Id""=a.""AssignmentGradeId"" and g.""IsDeleted""=false
and  g.""EffectiveStartDate""::Date <= '{DateTime.Now.ApplicationNow().Date}'::Date 
and '{DateTime.Now.ApplicationNow().Date}'::Date <= g.""EffectiveEndDate""::Date
left join cms.""N_CoreHR_HRPosition"" as po on po.""Id""=a.""PositionId"" and po.""IsDeleted""=false
and  po.""EffectiveStartDate""::Date <= '{DateTime.Now.ApplicationNow().Date}'::Date 
and '{DateTime.Now.ApplicationNow().Date}'::Date <= po.""EffectiveEndDate""::Date
where u.""Id""='{userId}' and u.""IsDeleted""=false
";
            // parameters.AddIfNotExists("ESD", DateTime.Now.ApplicationNow().Date);
            // parameters.AddIfNotExists("EED", DateTime.Now.ApplicationNow().Date);
            //parameters.AddIfNotExists("Status", StatusEnum.Active);
            //parameters.AddIfNotExists("CompanyId", CompanyId);
            //parameters.AddIfNotExists("PhotoReferenceType", ReferenceTypeEnum.HRS_PersonPhoto);
            //var cypher = @"match(u:ADM_User{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //    optional match(u)-[:R_User_PersonRoot]-(pr:HRS_PersonRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //    optional match(pr)-[:R_PersonRoot]-(p:HRS_Person{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //    where  p.EffectiveStartDate <= {ESD} and p.EffectiveEndDate >= {EED}
            //    optional match(pr)-[:R_AssignmentRoot_PersonRoot]-(ar:HRS_AssignmentRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //    optional match(ar)-[:R_AssignmentRoot]-(a:HRS_Assignment{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //    where a.EffectiveStartDate <= {ESD} and a.EffectiveEndDate >= {EED}
            //    optional match(a)-[:R_Assignment_PositionRoot]-(por:HRS_PositionRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //    optional match(por)-[:R_PositionRoot]-(po:HRS_Position{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //    where po.EffectiveStartDate <= {ESD} and po.EffectiveEndDate >= {EED}
            //    optional match(a)-[:R_Assignment_JobRoot]-(jr:HRS_JobRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //    optional match(jr)-[:R_JobRoot]-(j:HRS_Job{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //    where j.EffectiveStartDate <= {ESD} and j.EffectiveEndDate >= {EED}
            //    optional match(a)-[:R_Assignment_OrganizationRoot]-(orr:HRS_OrganizationRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //    optional match(orr)-[:R_OrganizationRoot]-(o:HRS_Organization{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //    where o.EffectiveStartDate <= {ESD} and o.EffectiveEndDate >= {EED}
            //    optional match(a)-[:R_Assignment_GradeRoot]-(gr:HRS_GradeRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //    optional match(gr)-[:R_GradeRoot]-(g:HRS_Grade{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //    where g.EffectiveStartDate <= {ESD} and g.EffectiveEndDate >= {EED}
            //    optional match(p)<-[:R_Attachment_Reference{ReferenceTypeCode:{PhotoReferenceType}}]-(ppa:GEN_Attachment{IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //    optional match(u)-[:R_User_GeoLocation]-(geo:ADM_GeoLocation{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //    optional match(u)-[:R_User_Mapping_Team]-(tem:ADM_TEAM{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //    with u,pr,p,ar,por,po,orr,o,gr,g,jr,j,ppa,a,geo,tem ";

            //if (cypherWhere.IsNotNullAndNotEmpty())
            //{
            //    cypherWhere = string.Concat(@"where ", cypherWhere);
            //}
            //if (returnValues.IsNullOrEmpty())
            //{
            //    returnValues = string.Concat(@"return u.Id as Id,u.UserName as UserName,u.Id as UserId,pr.Id as PersonId
            //    ,p.FirstName as Name
            //    ,p.SponsorshipNo as SponsorshipNo,u.Email as Email,p.Mobile as Mobile,p.PersonNo as PersonNo
            //    ,po.Name as PositionName,o.Name as OrganizationName,j.Name as JobName,g.Name as GradeName
            //    ,ppa.FileId as PhotoId,p.PhotoVersion as PhotoVersion,po.Id as PositionId,"
            //    , Helper.UserDisplayName("u", "as DisplayName,", "p")
            //    , Helper.UserNameWithEmail("u", "as UserNameWithEmail,")
            //    , Helper.PersonFullNameWithSponsorshipNo("p", "as FullNameWithSponsorshipNo")
            //    );
            //}
            // cypher = string.Concat(cypher, cypherWhere, returnValues);


            return await _queryUserListRepo.ExecuteQueryList<UserListOfValue>(cypher, null);

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
                            var query = $@"select ""ParentPositionId""
                        from cms.""UserHierarchy""
                        where ""UserId"" = '{userHierarchyId}' and ""HierarchyMasterId"" = '{hierarchy.Id}' and ""IsDeleted""=false ";
                            var parentPositionRoot = await _queryRepo.ExecuteScalar<string>(query, null);
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
                            var query = $@"select ""ParentUserId""
                        from public.""UserHierarchy""
                        where ""PositionId"" = '{userHierarchyId}' and ""HierarchyId"" = '{hierarchy.Id}' and ""IsDeleted""=false ";
                            var parentPositionRoot = await _queryRepo.ExecuteScalar<string>(query, null);
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
                            var query = $@"select ""ParentDepartmentId""
                        from cms.""N_CoreHR_HRDepartmentHierarchy""
                        where ""DepartmentId"" = '{userHierarchyId}' and ""HierarchyId"" = '{hierarchy.Id}' and ""IsDeleted""=false ";
                            var parentOrganizationRoot = await _queryRepo.ExecuteScalar<string>(query, null);
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
            string query = $@"   WITH RECURSIVE Users AS(
                                 select d.""UserId"" as ""Id"",d.""UserId"" as ""ParentId"",'Parent' as Type
                                from public.""UserHierarchy"" as d
                                where d.""UserId"" = '{userId}' and  d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'


                              union all

                                 select distinct d.""UserId"" as Id,d.""ParentUserId"" as ""ParentId"",'Child' as Type
                                from public.""UserHierarchy"" as d                                
                                where  d.""IsDeleted""=false and d.""CompanyId""='{_userContext.CompanyId}'
                             )
                            select Count(""Id""),""ParentId"" from Users  where Type = 'Child' group by ""ParentId""
						
                            ";



            var queryData = await _queryUserListRepo.ExecuteScalarList<double>(query, null);
            var list = queryData;
            return list;
        }
        public async Task<List<PositionChartViewModel>> GetUserHierarchy(string parentId, int levelUpto, string hierarchyId, int level = 1, int option = 1)
        {
            var dt = DateTime.Today.ToDatabaseDateFormat();
            string query = $@"with recursive hrchy as(
                                select u.""Id"" as ""Id"",u.""UserId"" as ""ParentId"",'Parent' as Type,0 As level
                                from public.""User"" as u where u.""Id"" = '{parentId}' and  u.""IsDeleted""=false 
                                union all
                                select  uh.""UserId"" as ""Id"",uh.""ParentUserId"" as ""ParentId"",'Child' as Type,hrchy.level+ 1 as level
                                from public.""HierarchyMaster"" as h
								join public.""UserHierarchy"" as uh on ""uh"".""HierarchyMasterId""=h.""Id"" and  uh.""IsDeleted""=false 
                                join hrchy  on hrchy.""Id""=uh.""ParentUserId"" 
                                where uh.""LevelNo""={level} and uh.""OptionNo""={option} and  h.""Id""='{hierarchyId}'  and  h.""IsDeleted""=false 
                             )select u.""Id"" as Id,j.""Id"" as JobId,j.""JobTitle"" as JobName,
                            o.""Id"" as OrganizationId, o.""DepartmentName"" as OrganizationName,g.""GradeName"" as GradeName,
                            hrchy.""ParentId"" as ParentId,
                            coalesce(dc.""DC"",0) as DirectChildCount,
                            u.""PhotoId"" as PhotoId,
                            u.""Name"" as DisplayName,
                            u.""Id"" as UserId,
                            '{hierarchyId}' as HierarchyId,
                            Case when p.""Id"" is not null then 'org-node-1' else 'org-node-3' end as CssClass,
                            pd.""PerformanceStageName"",pd.""GoalCount"",pd.""CompetencyCount""
                            from hrchy 
							join public.""User"" as u on hrchy.""Id""=u.""Id""
                            left join 
                            (
                                select uh.""ParentUserId"" as ""UserId"",count(uh.""UserId"") as ""DC""
                                from public.""HierarchyMaster"" as h
								join public.""UserHierarchy"" as uh on ""uh"".""HierarchyMasterId""=h.""Id"" and  uh.""IsDeleted""=false 
                                where uh.""LevelNo""={level} and uh.""OptionNo""={option} and  h.""Id""='{hierarchyId}'  
                                and  h.""IsDeleted""=false 
                                group by uh.""ParentUserId""
                            )as dc on u.""Id""=dc.""UserId""
                            left join 
                            (
                                select  pdn.""OwnerUserId"" as ""UserId"",max(pdms.""Name"") as ""PerformanceStageName""
                                ,max(g.""GC"") as ""GoalCount"",max(c.""CC"") as ""CompetencyCount""
                                from cms.""N_PerformanceDocument_PMSPerformanceDocument"" as pd 
                                join public.""NtsService"" as pdn on pdn.""UdfNoteTableId""=pd.""Id""
                                join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMaster"" as pdm on pdm.""Id"" = pd.""DocumentMasterId"" and pdm.""IsDeleted"" = false
                                join public.""NtsNote"" as pdmn on pdm.""NtsNoteId""=pdmn.""Id""
                                join public.""NtsNote"" as pdmsn on pdmsn.""ParentNoteId""=pdmn.""Id""
                                join cms.""N_PerformanceDocumentMaster_PerformanceDocumentMasterStage"" pdms on pdms.""NtsNoteId""=pdmsn.""Id""
                                join cms.""N_PerformanceDocument_PMSPerformanceDocumentStage"" pds on pds.""DocumentMasterStageId""=pdms.""Id""
                                left join (
                                    select gs.""OwnerUserId"", gs.""ParentServiceId"", count(g.""Id"") as ""GC"" 
	                                from public.""NtsService"" gs
                                    join cms.""N_PerformanceDocument_PMSGoal"" g on gs.""UdfNoteTableId""=g.""Id"" and g.""IsDeleted""=false
	                                where gs.""IsDeleted""=false-- and gs.""TemplateCode""='df'

                                    group by gs.""OwnerUserId"", gs.""ParentServiceId""
                                ) as g on g.""OwnerUserId""=pdn.""OwnerUserId"" and g.""ParentServiceId""=pdn.""Id""
                                left join (
                                    select gs.""OwnerUserId"", gs.""ParentServiceId"", count(g.""Id"") as ""CC"" 
	                                from public.""NtsService"" gs
                                    join cms.""N_PerformanceDocument_PMSCompentency"" g on gs.""UdfNoteTableId""=g.""Id"" and g.""IsDeleted""=false
	                                where gs.""IsDeleted""=false-- and gs.""TemplateCode""='df'

                                    group by gs.""OwnerUserId"", gs.""ParentServiceId""
                                )as c on c.""OwnerUserId""=pdn.""OwnerUserId"" and c.""ParentServiceId""=pdn.""Id""
                                where pdm.""Year""='{DateTime.Today.Year}' and pdms.""StartDate""<='{dt}' and pdms.""EndDate"">='{dt}'
                                group by pdn.""OwnerUserId""
                            )as pd on u.""Id""=pd.""UserId""
                            Left join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false 
                            Left join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false 
                            Left join cms.""N_CoreHR_HRJob"" as j on j.""Id"" = a.""JobId"" and j.""IsDeleted""=false 
                            Left join cms.""N_CoreHR_HRDepartment"" as o on o.""Id""=a.""DepartmentId"" and o.""IsDeleted""=false 
                            Left join cms.""N_CoreHR_HRGrade"" as g on g.""Id""=a.""AssignmentGradeId"" and g.""IsDeleted""=false 
                            where hrchy.""level""<={levelUpto}";
            var queryData = await _queryUserListRepo.ExecuteQueryList<PositionChartViewModel>(query, null);
            var list = queryData;
            return list;
        }
        public async Task<string> GetPersonDateOfJoining(string userId)
        {
            //var cypher = @"match(u:ADM_User{IsDeleted:0,Status:{Status},CompanyId:{CompanyId},Id:{Id}})
            //     match(u)-[:R_User_PersonRoot]-(pr:HRS_PersonRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //     match(pr)-[:R_PersonRoot]-(p:HRS_Person{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})                 
            //     match(pr)-[:R_AssignmentRoot_PersonRoot]-(ar:HRS_AssignmentRoot{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})
            //     match(ar)-[:R_AssignmentRoot]-(a:HRS_Assignment{ IsDeleted:0,Status:{Status},CompanyId:{CompanyId}})  
            //    return a.DateOfJoin";

            var query = $@"Select a.""DateOfJoin"" From public.""User"" as u
                            Join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false
                            Join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false
                            where u.""Id""='{userId}' and u.""IsDeleted""=false";

            var joinDate = await _queryPayPer.ExecuteScalar<string>(query, null);
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
            string query = @$"select *
                        from public.""LegalEntity""
                        where ""Id"" in ({legalEntity}) and ""IsDeleted""=false order by ""Name""";
            var list = await _querylegal.ExecuteQueryList<LegalEntityViewModel>(query, null);
            return list;
        }
        public async Task<List<UserPermissionViewModel>> ViewUserPermissions(string userId)
        {
            string query = @$"select u.""Name"" as UserName,po.""Name"" as PortalName,p.""Name"" as PageName,up.""Permissions"",'User' as Type from public.""UserPermission"" as up
join public.""Page"" as p on p.""Id""=up.""PageId"" and p.""IsDeleted""=false and up.""CompanyId""='{_repo.UserContext.CompanyId}' and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""Portal"" as po on po.""Id""=p.""PortalId"" and po.""IsDeleted""=false and po.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=up.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}' where u.""Id""='{userId}'

union
select u.""Name"" as UserName,po.""Name"" as PortalName,p.""Name"" as PageName,up.""Permissions"",Concat('UserRole-',ur.""Name"") from public.""UserRolePermission"" as up
join public.""Page"" as p on p.""Id""=up.""PageId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}' and up.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""Portal"" as po on po.""Id""=p.""PortalId"" and po.""IsDeleted""=false and po.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""UserRole"" as ur on ur.""Id""=up.""UserRoleId"" and ur.""IsDeleted""=false and ur.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""UserRoleUser"" as uru on uru.""UserRoleId""=up.""UserRoleId"" and uru.""IsDeleted""=false and uru.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=uru.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}' where u.""Id""='{userId}'";
            var list = await _querylegal.ExecuteQueryList<UserPermissionViewModel>(query, null);
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
                //_repository.RemoveRelationShip<ADM_User, R_User_HierarchyLevel_ParentUser>(userId
                //   , @"r.HierarchyId={HierarchyId} and r.HierarchyLevelNo={Level} and r.OptionNo={Option}
                //    and r.EffectiveStartDate={ESD} and r.EffectiveEndDate={EED}"
                //   , new Dictionary<string, object> {
                //        { "HierarchyId", hierarchyId}
                //        ,{ "ESD", Constant.ApplicationMinDate }
                //        ,{ "EED", Constant.ApplicationMaxDate }
                //       ,{"Level",levelNo }
                //       ,{"Option",optionNo }
                //   }
                //   , false);
                //var data = new R_User_HierarchyLevel_ParentUser
                //{
                //    EffectiveStartDate = esd,
                //    EffectiveEndDate = eed,
                //    HierarchyId = hierarchyId,
                //    HierarchyLevelNo = levelNo,
                //    IsLatest = true,
                //    OptionNo = optionNo
                //};
                //_repository.CreateMultipleOneToOneRelationship<ADM_User, R_User_HierarchyLevel_ParentUser, ADM_User>
                //    (userId, data, levelUserId, false);
            }
        }

        public async Task<IList<UserViewModel>> GetUserListWithEmailText()
        {
            string query = @$"SELECT *, concat('""',""Name"",'"" ','<',""Email"",'>') as EmailText
                            FROM public.""User""
                            where ""IsDeleted""=false and ""Status""=1";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(query, null);
            return list;
        }
        public async Task<List<UserViewModel>> GetAllUsersWithPortalId(string PortalId)
        {
            var Query = $@"SELECT u.* FROM public.""User"" as u 
 join public.""UserPortal"" as up on up.""UserId""=u.""Id"" and up.""IsDeleted""=false
  where u.""IsDeleted""=false  and up.""PortalId""='{PortalId}'";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(Query, null);
            return list;

        }
        public async Task<List<UserViewModel>> GetUsersWithPortalIds()
        {

            var Query = $@"SELECT distinct u.* FROM public.""User"" as u 
 join public.""UserPortal"" as up on up.""UserId""=u.""Id"" and up.""IsDeleted""=false
join public.""Company"" as c on c.""Id""='{_repo.UserContext.CompanyId}' and up.""PortalId""= Any(c.""LicensedPortalIds"") and c.""IsDeleted""=false
  where u.""IsDeleted""=false ";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(Query, null);
            return list;

        }
        public async Task<List<PortalViewModel>> GetAllowedPortalList(string userId)
        {
            var Query = $@"select p.* from public.""User"" as u 
            join public.""UserPortal"" as up on up.""UserId""=u.""Id"" and up.""IsDeleted""=false
            join public.""Portal"" as p on up.""PortalId""=p.""Id"" and p.""IsDeleted""=false
            where u.""IsDeleted""=false  and u.""Id""='{userId}' order by p.""Name""";
            var list = await _queryRepo.ExecuteQueryList<PortalViewModel>(Query, null);
            return list;
        }

        public async Task<List<UserViewModel>> GetDMSPermiisionusersList(string PortalId, string LegalEntityId)
        {
            var Query = $@"SELECT u.*  FROM public.""User"" as u 
 join public.""UserPortal"" as up on up.""UserId""=u.""Id"" and up.""IsDeleted""=false
  where u.""IsDeleted""=false  and up.""PortalId""='{PortalId}'
    and '{LegalEntityId}' = any ( u.""LegalEntityIds"")";
            var list = await _queryRepo.ExecuteQueryList<UserViewModel>(Query, null);
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
                            var query = $@"select ""ParentUserId""
                        from cms.""N_GENERAL_UserHierarchy""
                        where ""UserId"" = '{userHierarchyId}' and ""HierarchyId"" = '{hierarchy.Id}' and ""IsDeleted""=false ";
                            var parentOrganizationRoot = await _queryRepo.ExecuteScalar<string>(query, null);
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

    }
}
