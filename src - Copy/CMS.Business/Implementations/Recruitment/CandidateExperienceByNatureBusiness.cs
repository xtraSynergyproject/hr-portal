using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class CandidateExperienceByNatureBusiness : BusinessBase<CandidateExperienceByNatureViewModel, CandidateExperienceByNature>, ICandidateExperienceByNatureBusiness
    {
        public CandidateExperienceByNatureBusiness(IRepositoryBase<CandidateExperienceByNatureViewModel, CandidateExperienceByNature> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<CandidateExperienceByNatureViewModel>> Create(CandidateExperienceByNatureViewModel model)
        {
            var data = _autoMapper.Map<CandidateExperienceByNatureViewModel>(model);
            var validateSequenceOrder = await IsExists(data);
            if (!validateSequenceOrder.IsSuccess)
            {
                return CommandResult<CandidateExperienceByNatureViewModel>.Instance(model, false, validateSequenceOrder.Messages);
            }
            //var res = await base.GetList(x => x.CandidateProfileId == model.CandidateProfileId);
            //if (res.Count == 0)
            //{
            //    data.IsLatest = true;
            //}
            var result = await base.Create(data);

            return CommandResult<CandidateExperienceByNatureViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<CandidateExperienceByNatureViewModel>> Edit(CandidateExperienceByNatureViewModel model)
        {
            var data = _autoMapper.Map<CandidateExperienceByNatureViewModel>(model);
            var validateSequenceOrder = await IsExists(data);
            if (!validateSequenceOrder.IsSuccess)
            {
                return CommandResult<CandidateExperienceByNatureViewModel>.Instance(model, false, validateSequenceOrder.Messages);
            }
            var result = await base.Edit(model);

            return CommandResult<CandidateExperienceByNatureViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<CandidateExperienceByNatureViewModel>> IsExists(CandidateExperienceByNatureViewModel model)
        {
            var errorList = new Dictionary<string, string>();
            if (model.SequenceOrder == null)
            {
                errorList.Add("SlNo", "Sl No is required.");
            }
            else
            {
                var slno = await _repo.GetSingle(x => x.SequenceOrder == model.SequenceOrder && x.Id != model.Id && x.CandidateProfileId == model.CandidateProfileId && x.IsDeleted==false);
                if (slno != null)
                {
                    errorList.Add("SlNo", "Sl No already exist.");
                }
            }
            if (errorList.Count > 0)
            {
                return CommandResult<CandidateExperienceByNatureViewModel>.Instance(model, false, errorList);
            }

            return CommandResult<CandidateExperienceByNatureViewModel>.Instance();
        }
    }
}
