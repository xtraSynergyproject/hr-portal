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
    public class ApplicationReferencesBusiness : BusinessBase<ApplicationReferencesViewModel, ApplicationReferences>, IApplicationReferencesBusiness
    {
        public ApplicationReferencesBusiness(IRepositoryBase<ApplicationReferencesViewModel, ApplicationReferences> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<ApplicationReferencesViewModel>> Create(ApplicationReferencesViewModel model, bool autoCommit = true)
        {           
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<CandidateReferencesViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Create(model,autoCommit);

            return CommandResult<ApplicationReferencesViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ApplicationReferencesViewModel>> Edit(ApplicationReferencesViewModel model, bool autoCommit = true)
        {
            //var data = _autoMapper.Map<CandidateReferencesViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<CandidateReferencesViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Edit(model,autoCommit);

            return CommandResult<ApplicationReferencesViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<ApplicationReferencesViewModel>> IsNameExists(ApplicationReferencesViewModel model)
        {
                        
            return CommandResult<ApplicationReferencesViewModel>.Instance();
        }
    }
}
