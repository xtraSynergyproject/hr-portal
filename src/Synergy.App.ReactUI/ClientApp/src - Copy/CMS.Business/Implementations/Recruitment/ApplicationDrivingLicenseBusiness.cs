using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class ApplicationDrivingLicenseBusiness : BusinessBase<ApplicationDrivingLicenseViewModel, ApplicationDrivingLicense>, IApplicationDrivingLicenseBusiness
    {
        private readonly IRepositoryQueryBase<ApplicationDrivingLicenseViewModel> _queryRepo;
        public ApplicationDrivingLicenseBusiness(IRepositoryBase<ApplicationDrivingLicenseViewModel, ApplicationDrivingLicense> repo, IMapper autoMapper,
            IRepositoryQueryBase<ApplicationDrivingLicenseViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<ApplicationDrivingLicenseViewModel>> Create(ApplicationDrivingLicenseViewModel model)
        {
            var data = _autoMapper.Map<ApplicationDrivingLicenseViewModel>(model);
            var validateSequenceOrder = await IsExists(data);
            if (!validateSequenceOrder.IsSuccess)
            {
                return CommandResult<ApplicationDrivingLicenseViewModel>.Instance(data, false, validateSequenceOrder.Messages);
            }
            var res = await base.GetList(x => x.ApplicationId == data.ApplicationId);
            if (res.Count == 0)
            {
                data.IsLatest = true;
            }
            var result = await base.Create(data);

            return CommandResult<ApplicationDrivingLicenseViewModel>.Instance(data, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ApplicationDrivingLicenseViewModel>> Edit(ApplicationDrivingLicenseViewModel model)
        {
            var data = _autoMapper.Map<ApplicationDrivingLicenseViewModel>(model);            
            var validateSequenceOrder = await IsExists(data);
            if (!validateSequenceOrder.IsSuccess)
            {
                return CommandResult<ApplicationDrivingLicenseViewModel>.Instance(data, false, validateSequenceOrder.Messages);
            }
            var result = await base.Edit(data);

            return CommandResult<ApplicationDrivingLicenseViewModel>.Instance(data, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<ApplicationDrivingLicenseViewModel>> IsExists(ApplicationDrivingLicenseViewModel model)
        {
            var errorList = new Dictionary<string, string>();
            if (model.SequenceOrder == null)
            {
                errorList.Add("SlNo", "Sl No is required.");
            }
            else
            {
                var slno = await _repo.GetSingle(x => x.SequenceOrder == model.SequenceOrder && x.Id != model.Id && x.ApplicationId == model.ApplicationId);
                if (slno != null)
                {
                    errorList.Add("SlNo", "Sl No already exist.");
                }
            }
            if (errorList.Count > 0)
            {
                return CommandResult<ApplicationDrivingLicenseViewModel>.Instance(model, false, errorList);
            }
            return CommandResult<ApplicationDrivingLicenseViewModel>.Instance();
        }

        public async Task<IList<ApplicationDrivingLicenseViewModel>> GetLicenseListByApplication(string candidateProfileId)
        {
            string query = @$"SELECT l.*,
                            c.""Name"" as CountryName, lt.""Name"" as LicenseTypeName
                                FROM rec.""ApplicationDrivingLicense"" as l
                                LEFT JOIN cms.""Country"" as c ON c.""Id"" = l.""CountryId""
                                LEFT JOIN rec.""ListOfValue"" as lt ON lt.""Id"" = l.""LicenseType""                                
                                WHERE l.""ApplicationId"" = '{candidateProfileId}' and l.""IsDeleted"" = false order by l.""SequenceOrder"" ";

            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            return list;
        }       
    }
}
