using CMS.UI.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ERP.Data.GraphModel;
using CMS.Data.Model;
using CMS.Business;
using AutoMapper;

namespace CMS.Console
{
    public class DmsMigration
    {
        private List<ADM_User> SourceUserList;
        private List<User> TargetUserList;
        private string WebApiUrl = "http://95.111.235.64:91/";
        private IServiceProvider _serviceProvider;
        private IUserBusiness _userBusiness;
        private IMapper _autoMapper;
        public DmsMigration()
        {
            //_serviceProvider = serviceProvider;
            //_userBusiness = userBusiness;
            //_autoMapper = new AutoMapper.Mapper();

        }
        public async Task Extract()
        {
            await ExtractUser();
        }

        private async Task ExtractUser()
        {
            SourceUserList = await GetApiListAsync<ADM_User>();
        }

        public async Task Transform()
        {
            await TransformUser();
        }

        private async Task TransformUser()
        {
            //throw new NotImplementedException();
            if (SourceUserList.Count > 0)
            {
                TargetUserList = new List<User>();
                foreach(var su in SourceUserList)
                {
                    var tu = new User
                    {
                        Id = su.Id.ToString(),
                        UserId = su.Id.ToString(),
                        Name = su.UserName,
                        Email = su.Email,
                        JobTitle = su.JobTitle,
                        Password = su.Password,
                        //PhotoId=,
                        Mobile = su.MobileNo,
                        //ForgotPasswordOTP=,
                        PasswordChanged=true,
                        IsSystemAdmin = su.IsAdmin,
                        //IsGuestUser=,
                        //UserRoles=,
                        //UserPermissions=,
                        //SignatureId=,
                        EnableRegularEmail = su.EnableRegularEmail,
                        EnableSummaryEmail = su.EnableEmailSummary,
                        //LineManagerId = ,
                        //ActivationCode=,
                        //LegalEntityIds=,
                        IsDeleted = su.IsDeleted == 0 ? false : true,
                        Status = su.Status==ERP.Utility.StatusEnum.Active? Common.StatusEnum.Active : Common.StatusEnum.Inactive
                    };
                    TargetUserList.Add(tu);
                }
            }
        }

        public async Task Load()
        {
            await LoadUser();
        }

        private async Task LoadUser()
        {
            //var _userBusiness = _serviceProvider.GetService<IUserBusiness>();
            //throw new NotImplementedException();
            if (TargetUserList.Count>0)
            {
                foreach (var tu in TargetUserList)
                {
                    var user = _autoMapper.Map<User,UserViewModel>(tu);
                    var isUser = await _userBusiness.GetSingle(x=>x.UserId==tu.UserId && x.Email==tu.Email);
                    if (isUser==null)
                    {
                        await _userBusiness.Create(user);
                    }
                    else
                    {
                        await _userBusiness.Edit(user);
                    }
                }
            }
        }

        public async Task<T> GetApiAsync<T>()
        {
            using (var client = new HttpClient())
            {
                var address = new Uri($"{WebApiUrl}api/getlist?type={nameof(T)}");
                var response = await client.GetAsync(address);
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
        }
        public async Task<List<T>> GetApiListAsync<T>()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var address = new Uri($"{WebApiUrl}api/getlist?type={typeof(T).Name}");
                    var response = await client.GetAsync(address);
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<T>>(content);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
