
using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
   public class UserInfoBusiness : BusinessBase<NoteViewModel, NtsNote>, IUserInfoBusiness
    {
        INoteBusiness _noteBusiness;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo1;
        private readonly IRepositoryQueryBase<UserInfoViewModel> _queryRepo2;
        private readonly IClockServerQueryBusiness _clockServerQueryBusiness;

        IUserContext _userContext;
        public UserInfoBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper, IRepositoryQueryBase<IdNameViewModel> queryRepo1, IRepositoryQueryBase<UserInfoViewModel> queryRepo2, INoteBusiness noteBusiness, IUserContext userContext, IClockServerQueryBusiness clockServerQueryBusiness)
           : base(repo, autoMapper)
        {

            _queryRepo2 = queryRepo2;
            _queryRepo1 = queryRepo1;
            _noteBusiness = noteBusiness;

            _userContext = userContext;
            _clockServerQueryBusiness = clockServerQueryBusiness;

        }


        public async Task<IList<IdNameViewModel>> GetAllDevice() 
        {
            var result = await _clockServerQueryBusiness.GetAllDeviceData();
            return result;
        }

        public async Task<UserInfoViewModel> GetUserInfoDetails(string biometricId)
        {
            var result = await _clockServerQueryBusiness.GetUserInfoDetailsData(biometricId);
            return result;
        }
        public async Task<IList<UserInfoViewModel>> GetIncludePersonList(string deviceId, string searchParam)
        {
            var res = await _clockServerQueryBusiness.GetIncludePersonListData(deviceId, searchParam);
            return res;

        }

        public async Task<IList<UserInfoViewModel>> GetExcludePersonList(string deviceId, string searchParam)
       {
            var result = await _clockServerQueryBusiness.GetExcludePersonListData(deviceId,searchParam);
            return result;
        }

        public async Task<bool> IncludePerson(string deviceId, string persons,string Userid)
        {

            string[] spl = persons.Split(',');
            for (int i = 0; i < spl.Length; i++)
            {
                string ids = spl[i].ToString();

                var result1 = await _clockServerQueryBusiness.GetUserInfoDeviceData(deviceId, ids);

                if (result1.Count>0)
                {
                    var include = IncludeExclude.Include;
                    await _clockServerQueryBusiness.UpdateIncludeExcludeUserInfoDeviceData(deviceId, include, ids, "false");
                    
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
                await _clockServerQueryBusiness.UpdateIncludeExcludeUserInfoDeviceData(deviceId, exclude, ids, "true");
                

            }

            return true;

            
        }


        public async Task<PayPropertyTaxViewModel> GetPropertyTaxbyId(string PropertyId)
        {
            var result = await _clockServerQueryBusiness.GetPropertyTaxDataById(PropertyId);
            return result;
        }
    }
}
