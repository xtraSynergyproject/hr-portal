using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Linq;

namespace Synergy.App.Business
{
    public class RecruitmentElementBusiness : BusinessBase<RecruitmentCandidateElementInfoViewModel, RecruitmentCandidateElementInfo>, IRecruitmentElementBusiness
    {
        private readonly IRepositoryQueryBase<RecruitmentPayElementViewModel> _queryRepoIdName;
        private readonly IRepositoryQueryBase<RecruitmentCandidateElementInfoViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryIdName;
        private readonly IRepositoryQueryBase<ApplicationBeneficiaryViewModel> _queryRepoBen;
        public RecruitmentElementBusiness(IRepositoryBase<RecruitmentCandidateElementInfoViewModel, RecruitmentCandidateElementInfo> repo, IMapper autoMapper, 
            IRepositoryQueryBase<RecruitmentPayElementViewModel> queryRepoIdName, IRepositoryQueryBase<RecruitmentCandidateElementInfoViewModel> queryRepo,
            IRepositoryQueryBase<IdNameViewModel> queryIdName, IRepositoryQueryBase<ApplicationBeneficiaryViewModel> queryRepoBen) : base(repo, autoMapper)
        {
            _queryRepoIdName = queryRepoIdName;
            _queryRepo = queryRepo;
            _queryIdName = queryIdName;
            _queryRepoBen = queryRepoBen;
        }

        public async override Task<CommandResult<RecruitmentCandidateElementInfoViewModel>> Create(RecruitmentCandidateElementInfoViewModel model, bool autoCommit = true)
        {
            //var data = _autoMapper.Map<JobAdvertisementViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<JobAdvertisementViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Create(model,autoCommit);

            return CommandResult<RecruitmentCandidateElementInfoViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<RecruitmentCandidateElementInfoViewModel>> Edit(RecruitmentCandidateElementInfoViewModel model, bool autoCommit = true)
        {
            //var data = _autoMapper.Map<JobAdvertisementViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<JobAdvertisementViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Edit(model,autoCommit);

            return CommandResult<RecruitmentCandidateElementInfoViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<RecruitmentCandidateElementInfoViewModel>> IsNameExists(RecruitmentCandidateElementInfoViewModel model)
        {
                        
            return CommandResult<RecruitmentCandidateElementInfoViewModel>.Instance();
        }

        public async Task<IList<RecruitmentPayElementViewModel>> GetPayElementIdNameList()
        {
            string query = @$"SELECT ""Id"", ""ElementName""
                                FROM rec.""RecruitmentPayElement"" ";
            var queryData = await _queryRepoIdName.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            return list;
        }

        public async Task<IList<RecruitmentCandidateElementInfoViewModel>> GetElementData(string appid)
        {
            //        string query = @$"SELECT rc.*, rp.""ElementName""

            //FROM rec.""RecruitmentCandidateElementInfo"" as rc

            //join rec.""RecruitmentPayElement"" as rp on rp.""Id"" = rc.""ElementId""

            //where rc.""IsDeleted"" = false and rc.""ApplicationId"" = '{appid}' ";

            string query = @$"SELECT rc.*, rp.""ElementName"",rp.""Id"" as PayId

    FROM rec.""RecruitmentPayElement"" as rp

    left join rec.""RecruitmentCandidateElementInfo"" as rc on rp.""Id"" = rc.""ElementId""

     and rc.""IsDeleted"" = false and rc.""ApplicationId"" = '{appid}'
  where rp.""IsDeleted"" = false  order by rp.""SequenceOrder"" ";
            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            
            return list;
        }

        public async Task<IList<IdNameViewModel>> GetUserIdNameList()
        {
            string query = @$"SELECT ""Id"", ""Name""
                                FROM public.""User""";
            var queryData = await _queryIdName.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            return list;
        }

        public async Task<IList<IdNameViewModel>> GetLocationIdNameList()
        {
            string query = @$"SELECT ""Id"", ""Name""
                                FROM cms.""Location""";
            var queryData = await _queryIdName.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            return list;
        }

        public async Task<IList<IdNameViewModel>> GetGradeIdNameList()
        {
            string query = @$"SELECT ""Id"", ""Name""
                                FROM cms.""Grade""
                                where ""IsDeleted"" = false and ""Status"" = 1 order by ""Name"" ";
            var queryData = await _queryIdName.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            return list;
        }

        public async Task<bool> Beneficiarycreate(ApplicationBeneficiaryViewModel model,DataActionEnum action)
        {
           
            if (action == DataActionEnum.Create)
            {
                var res = await base.Create<ApplicationBeneficiaryViewModel, ApplicationBeneficiary>(model);
                if (res.IsSuccess)
                {
                    return true;
                }
            }
            else if (action == DataActionEnum.Edit)
            {
                var res = await base.Edit<ApplicationBeneficiaryViewModel, ApplicationBeneficiary>(model);
                if (res.IsSuccess)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<IList<ApplicationBeneficiaryViewModel>> GetBeneficiartData(string appid)
        {
            string query = @$"SELECT *
                                FROM rec.""ApplicationBeneficiary""
                                WHERE ""ApplicationId""='{appid}' and ""IsDeleted""=false ";
            var queryData = await _queryRepoBen.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            return list;
        }

        public async Task<ApplicationBeneficiaryViewModel> GetBeneficiartDataByid(string id)
        {
            string query = @$"SELECT *
                                FROM rec.""ApplicationBeneficiary""
                                WHERE ""Id""='{id}' and ""IsDeleted""=false ";
            var queryData = await _queryRepoBen.ExecuteQuerySingle(query, null);
           // var list = queryData.ToList();
            return queryData;
        }

        public async Task<IdNameViewModel> GetAccomadationValue(string id)
        {
            string query = @$"SELECT ""Id"",""Name""
                                FROM rec.""ListOfValue""
                                WHERE ""Id""='{id}' and ""IsDeleted""=false ";
            var queryData = await _queryIdName.ExecuteQuerySingle(query, null);
           //var list = queryData.ToList();
            return queryData;
        }

        public async Task<string> GenerateFinalOfferRef(string appno)
        {
            
            var id = await GenerateFinalOfferSeq();
            return string.Concat(appno, "-", "F", "-", id);
        }
        public async Task<long> GenerateFinalOfferSeq()
        {
            string query = @$"SELECT count(*) FROM rec.""Application"" WHERE ""FinalOfferReference"" IS Not NULL 
                            ";
            var result = await _queryRepo.ExecuteScalar<long>(query, null);
            return result;
        }

        public async Task<IdNameViewModel> GetGrade(string code)
        {
            string query = @$"SELECT ""Id"", ""Name"" FROM cms.""Grade"" WHERE ""Code""='{code}' and ""IsDeleted""=false ";
            var queryData = await _queryIdName.ExecuteQuerySingle(query, null);
            //var list = queryData.ToList();
            return queryData;
        }

    }
}
