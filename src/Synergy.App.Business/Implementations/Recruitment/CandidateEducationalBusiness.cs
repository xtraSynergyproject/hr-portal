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
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class CandidateEducationalBusiness : BusinessBase<CandidateEducationalViewModel, CandidateEducational>, ICandidateEducationalBusiness
    {
        private readonly IRepositoryQueryBase<CandidateEducationalViewModel> _queryRepo;
        public CandidateEducationalBusiness(IRepositoryBase<CandidateEducationalViewModel, CandidateEducational> repo, IMapper autoMapper,
            IRepositoryQueryBase<CandidateEducationalViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<CandidateEducationalViewModel>> Create(CandidateEducationalViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<CandidateEducationalViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<CandidateEducationalViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Create(data, autoCommit);

            return CommandResult<CandidateEducationalViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<CandidateEducationalViewModel>> Edit(CandidateEducationalViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<CandidateEducationalViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<CandidateEducationalViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Edit(data,autoCommit);

            return CommandResult<CandidateEducationalViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<CandidateEducationalViewModel>> IsNameExists(CandidateEducationalViewModel model)
        {
                        
            return CommandResult<CandidateEducationalViewModel>.Instance();
        }

        public async Task<IList<CandidateEducationalViewModel>> GetListByCandidate(QualificationTypeEnum qualificationType, string candidateProfileId)
        {
            string query = @$"SELECT c.*,
                            q.""Name"" as QualificationName,
                              s.""Name"" as SpecializationName, e.""Name"" as EducationTypeName,
                                ct.""Name"" as CountryName, d.""FileName"" as AttachmentName	
                                FROM rec.""CandidateEducational"" as c
                                LEFT JOIN rec.""ListOfValue"" as q ON q.""Id"" = c.""QualificationId""
                                LEFT JOIN rec.""ListOfValue"" as s ON s.""Id"" = c.""SpecializationId""
                                LEFT JOIN rec.""ListOfValue"" as e ON e.""Id"" = c.""EducationType""
                                LEFT JOIN cms.""Country"" as ct ON ct.""Id"" = c.""CountryId""
                                LEFT JOIN public.""File"" as d ON d.""Id"" = c.""AttachmentId""
                                WHERE c.""IsDeleted""='false' AND c.""CandidateProfileId""='" + candidateProfileId + "'";

            //var queryData = await _queryRepo.ExecuteQueryDataTable(query, null);
            var queryData = await _queryRepo.ExecuteQueryList(query,null);
            var list = queryData.Where(x => x.QualificationTypeId == qualificationType).ToList();
            return list;
        }
    }
}
