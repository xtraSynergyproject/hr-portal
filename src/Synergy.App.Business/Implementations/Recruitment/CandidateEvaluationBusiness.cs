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
    public class CandidateEvaluationBusiness : BusinessBase<CandidateEvaluationViewModel, CandidateEvaluation>, ICandidateEvaluationBusiness
    {
        private readonly IRepositoryQueryBase<CandidateEvaluationViewModel> _queryRepo;
        public CandidateEvaluationBusiness(IRepositoryBase<CandidateEvaluationViewModel, CandidateEvaluation> repo, IMapper autoMapper,
            IRepositoryQueryBase<CandidateEvaluationViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<CandidateEvaluationViewModel>> Create(CandidateEvaluationViewModel model, bool autoCommit = true)
        {

            var result = await base.Create(model,autoCommit);

            return CommandResult<CandidateEvaluationViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<CandidateEvaluationViewModel>> Edit(CandidateEvaluationViewModel model, bool autoCommit = true)
        {
            var result = await base.Edit(model,autoCommit);

            return CommandResult<CandidateEvaluationViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
    }
}
