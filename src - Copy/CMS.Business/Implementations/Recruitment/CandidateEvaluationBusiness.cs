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
using System.Threading.Tasks;


namespace CMS.Business
{
    public class CandidateEvaluationBusiness : BusinessBase<CandidateEvaluationViewModel, CandidateEvaluation>, ICandidateEvaluationBusiness
    {
        private readonly IRepositoryQueryBase<CandidateEvaluationViewModel> _queryRepo;
        public CandidateEvaluationBusiness(IRepositoryBase<CandidateEvaluationViewModel, CandidateEvaluation> repo, IMapper autoMapper,
            IRepositoryQueryBase<CandidateEvaluationViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<CandidateEvaluationViewModel>> Create(CandidateEvaluationViewModel model)
        {

            var result = await base.Create(model);

            return CommandResult<CandidateEvaluationViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<CandidateEvaluationViewModel>> Edit(CandidateEvaluationViewModel model)
        {
            var result = await base.Edit(model);

            return CommandResult<CandidateEvaluationViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
    }
}
