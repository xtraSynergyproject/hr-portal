
using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
   public class UserInfoBusiness : BusinessBase<NoteViewModel, NtsNote>, IUserInfoBusiness
    {
        INoteBusiness _noteBusiness;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo1;
        private readonly IRepositoryQueryBase<UserInfoViewModel> _queryRepo2;
        
        IUserContext _userContext;
        public UserInfoBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper, IRepositoryQueryBase<IdNameViewModel> queryRepo1, IRepositoryQueryBase<UserInfoViewModel> queryRepo2, INoteBusiness noteBusiness, IUserContext userContext)
           : base(repo, autoMapper)
        {

            _queryRepo2 = queryRepo2;
            _queryRepo1 = queryRepo1;
            _noteBusiness = noteBusiness;

            _userContext = userContext;

        }


        public async Task<IList<IdNameViewModel>> GetAllDevice() {
            var Query = $@"SELECT ""Id"", ""Name"" FROM cms.""N_CLK_Device"" ";
            var result = await _queryRepo1.ExecuteQueryList<IdNameViewModel>(Query, null);
            return result;
        }

        public async Task<UserInfoViewModel> GetUserInfoDetails(string biometricId)
        {
            var query = $@"Select * from cms.""N_CLK_UserInfo"" where ""BiometricId""='{biometricId}' ";
            var result = await _queryRepo2.ExecuteQuerySingle(query, null);
            return result;
        }
        public async Task<IList<UserInfoViewModel>> GetIncludePersonList(string deviceId, string searchParam)
        {
            var Query = $@"select UI.""Id"",hp.""PersonFullName"" as DisplayName,hj.""JobTitle""  as JobName,hd.""DepartmentName"",D.""Name"" as RegisteredDevices,U.""PhotoId"" as PhotoName
    from cms.""N_CLK_UserInfo"" as UI inner join  cms.""N_CoreHR_HRPerson"" as hp on hp.""BiometricId"" = UI.""BiometricId"" and hp.""IsDeleted"" = 'false'
    left join cms.""N_CLK_UserInfoDevice"" as UD on UI.""Id"" = UD.""userInfoId"" and UD.""IsDeleted"" = 'false'
    left join cms.""N_CLK_Device"" as D on D.""Id"" = UD.""DeviceId"" and D.""IsDeleted"" = 'false'
                        left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id"" = assi.""EmployeeId"" and assi.""IsDeleted"" = 'false'
                        left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id"" = assi.""DepartmentId"" and hd.""IsDeleted"" = 'false'
                        left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id"" = assi.""JobId"" and hj.""IsDeleted"" = 'false'
                        left join public.""User"" as u on u.""Id""=hp.""UserId"" and u.""IsDeleted""='false'
                        where UD.""DeviceId"" = '{deviceId}' and UI.""IsDeleted""='false' #param#";

            string vl = "";
            if (searchParam.IsNotNullAndNotEmpty())
            {
                vl = $@" AND hp.""PersonFullName""  like '{searchParam}%' COLLATE ""tr-TR-x-icu""";
            }

            Query = Query.Replace("#param#", vl);
            var result = await _queryRepo2.ExecuteQueryList<UserInfoViewModel>(Query, null);
            return result;

        }

        public async Task<IList<UserInfoViewModel>> GetExcludePersonList(string deviceId, string searchParam)
       {
            var Query = $@"select UI.""Id"",hp.""PersonFullName"" as DisplayName,hj.""JobTitle""  as JobName,hd.""DepartmentName"",D.""Name"" as RegisteredDevices,U.""PhotoId"" as PhotoName
    from cms.""N_CLK_UserInfo"" as UI inner join  cms.""N_CoreHR_HRPerson"" as hp on hp.""BiometricId"" = UI.""BiometricId"" and hp.""IsDeleted"" = 'false'
    left join cms.""N_CLK_UserInfoDevice"" as UD on UI.""Id"" = UD.""userInfoId"" and UD.""IsDeleted"" = 'false'
    left join cms.""N_CLK_Device"" as D on D.""Id"" = UD.""DeviceId"" and D.""IsDeleted"" = 'false'
                        left join cms.""N_CoreHR_HRAssignment"" as assi on hp.""Id"" = assi.""EmployeeId"" and assi.""IsDeleted"" = 'false'
                        left join cms.""N_CoreHR_HRDepartment"" as hd on hd.""Id"" = assi.""DepartmentId"" and hd.""IsDeleted"" = 'false'
                        left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id"" = assi.""JobId"" and hj.""IsDeleted"" = 'false'
                        left join public.""User"" as u on u.""Id""=hp.""UserId"" and u.""IsDeleted""='false'
                        where UD.""DeviceId"" != '{deviceId}' or UD.""DeviceId"" is null and UI.""IsDeleted""='false' #param#";


            string vl= "";
            if (searchParam.IsNotNullAndNotEmpty())
            {
                vl = $@" AND hp.""PersonFullName""  like '{searchParam}%' COLLATE ""tr-TR-x-icu""";
            }

            Query = Query.Replace("#param#", vl);
            var result = await _queryRepo2.ExecuteQueryList<UserInfoViewModel>(Query, null);
            return result;
        }

        public async Task<bool> IncludePerson(string deviceId, string persons,string Userid)
        {

            string[] spl = persons.Split(',');
            for (int i = 0; i < spl.Length; i++)
            {

                var Query = $@"select * from cms.""N_CLK_UserInfoDevice""  where ""DeviceId""='{deviceId}' and ""userInfoId""='{spl[i].ToString()}'";
                var result1 = await _queryRepo2.ExecuteQueryList<UserInfoViewModel>(Query, null);
                if (result1.Count>0)
                {
                    var include = IncludeExclude.Include;
                    var Query2 = $@"update cms.""N_CLK_UserInfoDevice"" set ""IsDeleted""='false',""IncludeOrExclude""='{include}' where ""DeviceId""='{deviceId}' and ""userInfoId""='{spl[i].ToString()}'";

                    await _queryRepo2.ExecuteCommand(Query2, null);
                }
                else
                {
                    if (spl[i].Length > 0)
                    {
                        var model = new UserInfoViewModel();
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.DataAction = DataActionEnum.Create;
                        noteTempModel.ActiveUserId = _repo.UserContext.UserId;
                        noteTempModel.TemplateCode = "USER_INFO_DEVICE";

                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

                        model.DeviceId = deviceId;
                        model.userInfoId = spl[i].ToString();
                        model.IncludeOrExclude = IncludeExclude.Include;

                        notemodel.Json = JsonConvert.SerializeObject(model);
                        notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                        //notemodel.NoteSubject = model.NoteSubject;

                        var result = await _noteBusiness.ManageNote(notemodel);
                    }
                }

            }

            return true;
            
        }
        public async Task<bool> ExcludePerson(string deviceId, string persons)
        {
            string[] spl = persons.Split(',');
            for (int i = 0; i < spl.Length; i++)
            {
                string ids = spl[i].ToString();
                var exclude= IncludeExclude.Exclude;
                var Query = $@"update cms.""N_CLK_UserInfoDevice"" set ""IsDeleted""='true',""IncludeOrExclude""='{exclude}' where ""DeviceId""='{deviceId}' and ""userInfoId""='{ids}'";

                await _queryRepo2.ExecuteCommand(Query, null);

            }

            return true;

            
        }


        public async Task<PayPropertyTaxViewModel> GetPropertyTaxbyId(string PropertyId)
        {
            var query = @$"select  P.""FirstName"" as ""OwnerName"", wd.""Name"" as WardNo,p.""Locality"" as Locality,P.""Zone"" as Zone,
    cl.""Name"" as Colony,s.""ServiceNo"" as PropertyId
    from  public.""NtsService"" as S inner join public.""NtsNote"" as N on N.""Id""=S.""UdfNoteId"" 
	inner join cms.""N_PROP_TAX_NEW_PROPERTY"" as P on P.""NtsNoteId""=N.""Id""
	left join public.""LOV"" as wd on p.""WardNoId""=wd.""Id""
	left join public.""LOV"" as cl on p.""Colony""=cl.""Id""
	where s.""ServiceNo""='{PropertyId}'";

            var result = await _queryRepo1.ExecuteQuerySingle <PayPropertyTaxViewModel>(query, null);
            return result;
        }
    }
}
