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
    public class ApplicationProjectBusiness : BusinessBase<ApplicationProjectViewModel, ApplicationProject>, IApplicationProjectBusiness
    {
        public ApplicationProjectBusiness(IRepositoryBase<ApplicationProjectViewModel, ApplicationProject> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<ApplicationProjectViewModel>> Create(ApplicationProjectViewModel model, bool autoCommit = true)
        {           
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<CandidateProjectViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Create(model,autoCommit);

            return CommandResult<ApplicationProjectViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ApplicationProjectViewModel>> Edit(ApplicationProjectViewModel model, bool autoCommit = true)
        {
            //var data = _autoMapper.Map<CandidateProjectViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<CandidateProjectViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Edit(model,autoCommit);

            return CommandResult<ApplicationProjectViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<ApplicationProjectViewModel>> IsNameExists(ApplicationProjectViewModel model)
        {
                        
            return CommandResult<ApplicationProjectViewModel>.Instance();
        }
    }
}
