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


namespace Synergy.App.Business
{
    public class CandidateProjectBusiness : BusinessBase<CandidateProjectViewModel, CandidateProject>, ICandidateProjectBusiness
    {
        public CandidateProjectBusiness(IRepositoryBase<CandidateProjectViewModel, CandidateProject> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<CandidateProjectViewModel>> Create(CandidateProjectViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<CandidateProjectViewModel>(model);
            var validateSequenceOrder = await IsExists(data);
            if (!validateSequenceOrder.IsSuccess)
            {
                return CommandResult<CandidateProjectViewModel>.Instance(model, false, validateSequenceOrder.Messages);
            }
            var res = await base.GetList(x => x.CandidateProfileId == model.CandidateProfileId);
            if (res.Count == 0)
            {
                data.IsLatest = true;
            }
            var result = await base.Create(data, autoCommit);

            return CommandResult<CandidateProjectViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<CandidateProjectViewModel>> Edit(CandidateProjectViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<CandidateProjectViewModel>(model);
            var validateSequenceOrder = await IsExists(data);
            if (!validateSequenceOrder.IsSuccess)
            {
                return CommandResult<CandidateProjectViewModel>.Instance(model, false, validateSequenceOrder.Messages);
            }
            var result = await base.Edit(model,autoCommit);

            return CommandResult<CandidateProjectViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        private async Task<CommandResult<CandidateProjectViewModel>> IsExists(CandidateProjectViewModel model)
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
                return CommandResult<CandidateProjectViewModel>.Instance(model, false, errorList);
            }

            return CommandResult<CandidateProjectViewModel>.Instance();
        }
    }
}
