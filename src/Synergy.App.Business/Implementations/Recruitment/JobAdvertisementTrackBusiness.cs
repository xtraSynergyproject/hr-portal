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
    public class JobAdvertisementTrackBusiness : BusinessBase<JobAdvertisementTrackViewModel, JobAdvertisementTrack>, IJobAdvertisementTrackBusiness
    {
        public JobAdvertisementTrackBusiness(IRepositoryBase<JobAdvertisementTrackViewModel, JobAdvertisementTrack> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<JobAdvertisementTrackViewModel>> Create(JobAdvertisementTrackViewModel model, bool autoCommit = true)
        {
            //var data = _autoMapper.Map<JobAdvertisementViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<JobAdvertisementViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Create(model,autoCommit);

            return CommandResult<JobAdvertisementTrackViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<JobAdvertisementTrackViewModel>> Edit(JobAdvertisementTrackViewModel model, bool autoCommit = true)
        {
            //var data = _autoMapper.Map<JobAdvertisementViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<JobAdvertisementViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Edit(model,autoCommit);

            return CommandResult<JobAdvertisementTrackViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<JobAdvertisementTrackViewModel>> IsNameExists(JobAdvertisementTrackViewModel model)
        {
                        
            return CommandResult<JobAdvertisementTrackViewModel>.Instance();
        }
    }
}
