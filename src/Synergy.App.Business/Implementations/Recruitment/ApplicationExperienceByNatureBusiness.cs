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
    public class ApplicationExperienceByNatureBusiness : BusinessBase<ApplicationeExperienceByNatureViewModel, ApplicationeExperienceByNature>, IApplicationExperienceByNatureBusiness
    {
        public ApplicationExperienceByNatureBusiness(IRepositoryBase<ApplicationeExperienceByNatureViewModel, ApplicationeExperienceByNature> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<ApplicationeExperienceByNatureViewModel>> Create(ApplicationeExperienceByNatureViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<ApplicationeExperienceByNatureViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<CandidateEducationalViewModel>.Instance(model, false, validateName.Messages);
            //}
            //var res = await base.GetList(x => x.CandidateProfileId == model.CandidateProfileId);
            //if (res.Count == 0)
            //{
            //    data.IsLatest = true;
            //}
            var result = await base.Create(data, autoCommit);

            return CommandResult<ApplicationeExperienceByNatureViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ApplicationeExperienceByNatureViewModel>> Edit(ApplicationeExperienceByNatureViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<ApplicationeExperienceByNatureViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<CandidateExperienceByNatureViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Edit(model,autoCommit);

            return CommandResult<ApplicationeExperienceByNatureViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }        

        private async Task<CommandResult<ApplicationeExperienceByNatureViewModel>> IsNameExists(ApplicationeExperienceByNatureViewModel model)
        {
                        
            return CommandResult<ApplicationeExperienceByNatureViewModel>.Instance();
        }
    }
}
