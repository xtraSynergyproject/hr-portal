using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public class ClockServerQueryPostgreBusiness : BusinessBase<NoteViewModel, NtsNote>, IClockServerQueryBusiness
    {
        IUserContext _uc;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        public ClockServerQueryPostgreBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo
            , IMapper autoMapper
            , IUserContext uc
            , IRepositoryQueryBase<NoteViewModel> queryRepo) : base(repo, autoMapper)
        {
            _uc = uc;
            _queryRepo = queryRepo;
        }

        public async Task<IList<IdNameViewModel>> GetAllDeviceData()
        {
            var Query = $@"SELECT ""Id"", ""Name"" FROM cms.""N_CLK_Device"" ";
            var result = await _queryRepo.ExecuteQueryList<IdNameViewModel>(Query, null);
            return result;
        }

        public async Task<UserInfoViewModel> GetUserInfoDetailsData(string biometricId)
        {
            var query = $@"Select * from cms.""N_CLK_UserInfo"" where ""BiometricId""='{biometricId}' ";
            var result = await _queryRepo.ExecuteQuerySingle<UserInfoViewModel>(query, null);
            return result;
        }

        public async Task<IList<UserInfoViewModel>> GetIncludePersonListData(string deviceId, string searchParam)
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
            var result = await _queryRepo.ExecuteQueryList<UserInfoViewModel>(Query, null);
            return result;

        }

        public async Task<IList<UserInfoViewModel>> GetExcludePersonListData(string deviceId, string searchParam)
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


            string vl = "";
            if (searchParam.IsNotNullAndNotEmpty())
            {
                vl = $@" AND hp.""PersonFullName""  like '{searchParam}%' COLLATE ""tr-TR-x-icu""";
            }

            Query = Query.Replace("#param#", vl);
            var result = await _queryRepo.ExecuteQueryList<UserInfoViewModel>(Query, null);
            return result;
        }

        public async Task<PayPropertyTaxViewModel> GetPropertyTaxDataById(string PropertyId)
        {
            var query = @$"select  P.""FirstName"" as ""OwnerName"", wd.""Name"" as WardNo,p.""Locality"" as Locality,P.""Zone"" as Zone,
                        cl.""Name"" as Colony,s.""ServiceNo"" as PropertyId
                        from  public.""NtsService"" as S inner join public.""NtsNote"" as N on N.""Id""=S.""UdfNoteId"" 
	                    inner join cms.""N_PROP_TAX_NEW_PROPERTY"" as P on P.""NtsNoteId""=N.""Id""
	                    left join public.""LOV"" as wd on p.""WardNoId""=wd.""Id""
	                    left join public.""LOV"" as cl on p.""Colony""=cl.""Id""
	                    where s.""ServiceNo""='{PropertyId}'";

            var result = await _queryRepo.ExecuteQuerySingle<PayPropertyTaxViewModel>(query, null);
            return result;
        }

        public async Task UpdateIncludeExcludeUserInfoDeviceData(string deviceId, IncludeExclude action, string id, string isDeleted)
        {
            var Query2 = $@"update cms.""N_CLK_UserInfoDevice"" set ""IsDeleted""='{isDeleted}',""IncludeOrExclude""='{action}' where ""DeviceId""='{deviceId}' and ""userInfoId""='{id}'";
            await _queryRepo.ExecuteCommand(Query2, null);
        }

        public async Task<List<UserInfoViewModel>> GetUserInfoDeviceData(string deviceId, string id)
        {
            var Query = $@"select * from cms.""N_CLK_UserInfoDevice""  where ""DeviceId""='{deviceId}' and ""userInfoId""='{id}'";
            var result = await _queryRepo.ExecuteQueryList<UserInfoViewModel>(Query, null);
            return result;
        }
    }
}
