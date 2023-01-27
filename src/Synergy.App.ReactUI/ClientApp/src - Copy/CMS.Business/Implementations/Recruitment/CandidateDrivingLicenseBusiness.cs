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
    public class CandidateDrivingLicenseBusiness : BusinessBase<CandidateDrivingLicenseViewModel, CandidateDrivingLicense>, ICandidateDrivingLicenseBusiness
    {
        private readonly IRepositoryQueryBase<CandidateDrivingLicenseViewModel> _queryRepo;
        public CandidateDrivingLicenseBusiness(IRepositoryBase<CandidateDrivingLicenseViewModel, CandidateDrivingLicense> repo, IMapper autoMapper,
            IRepositoryQueryBase<CandidateDrivingLicenseViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<CandidateDrivingLicenseViewModel>> Create(CandidateDrivingLicenseViewModel model)
        {
            var data = _autoMapper.Map<CandidateDrivingLicenseViewModel>(model);
            var validateSequenceOrder = await IsExists(data);
            if (!validateSequenceOrder.IsSuccess)
            {
                return CommandResult<CandidateDrivingLicenseViewModel>.Instance(data, false, validateSequenceOrder.Messages);
            }            
            //var res = await base.GetList(x => x.CandidateProfileId == data.CandidateProfileId);
            //if (res.Count == 0)
            //{
            //    data.IsLatest = true;
            //}
            var result = await base.Create(data);

            return CommandResult<CandidateDrivingLicenseViewModel>.Instance(data, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<CandidateDrivingLicenseViewModel>> Edit(CandidateDrivingLicenseViewModel model)
        {
            var data = _autoMapper.Map<CandidateDrivingLicenseViewModel>(model);
            var validateSequenceOrder = await IsExists(data);
            if (!validateSequenceOrder.IsSuccess)
            {
                return CommandResult<CandidateDrivingLicenseViewModel>.Instance(data, false, validateSequenceOrder.Messages);
            }
            
            var result = await base.Edit(data);

            return CommandResult<CandidateDrivingLicenseViewModel>.Instance(data, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<CandidateDrivingLicenseViewModel>> IsExists(CandidateDrivingLicenseViewModel model)
        {
            var errorList = new Dictionary<string, string>();
            if (model.SequenceOrder == null)
            {
                errorList.Add("SlNo", "Sl No is required.");
            }
            else
            {
                var slno = await _repo.GetSingle(x => x.SequenceOrder == model.SequenceOrder && x.Id != model.Id && x.CandidateProfileId == model.CandidateProfileId && x.IsDeleted == false);
                if (slno != null)
                {
                    errorList.Add("SlNo", "Sl No already exist.");
                }
            }
            if (errorList.Count > 0)
            {
                return CommandResult<CandidateDrivingLicenseViewModel>.Instance(model, false, errorList);
            }
            return CommandResult<CandidateDrivingLicenseViewModel>.Instance();
        }

        public async Task<IList<CandidateDrivingLicenseViewModel>> GetLicenseListByCandidate(string candidateProfileId)
        {
            string query = @$"SELECT l.*,
                            c.""Name"" as CountryName, lt.""Name"" as LicenseTypeName
                                FROM rec.""CandidateDrivingLicense"" as l
                                LEFT JOIN cms.""Country"" as c ON c.""Id"" = l.""CountryId""
                                LEFT JOIN rec.""ListOfValue"" as lt ON lt.""Id"" = l.""LicenseType""                                
                                WHERE l.""CandidateProfileId"" = '{candidateProfileId}' and l.""IsDeleted"" = false order by l.""SequenceOrder"" ";

            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            return list;
        }

        public async Task<IList<IdNameViewModel>> GetCountryListData()
        { 
            var countrylist = await _queryRepo.ExecuteQueryDataTable(@$"SELECT ""Id"",""Name"" FROM cms.""Country"" where ""IsDeleted""=false", null);
            var list = new List<IdNameViewModel>();
            foreach (DataRow row in countrylist.Rows)
            {
                var data = new IdNameViewModel();
                data.Name = row["Name"].ToString();
                data.Id = row["Id"].ToString();
                list.Add(data);
            }
            return list;
            
        }
    }
}
