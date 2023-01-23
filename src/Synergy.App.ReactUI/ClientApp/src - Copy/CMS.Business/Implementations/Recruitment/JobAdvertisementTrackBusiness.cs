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
    public class JobAdvertisementTrackBusiness : BusinessBase<JobAdvertisementTrackViewModel, JobAdvertisementTrack>, IJobAdvertisementTrackBusiness
    {
        public JobAdvertisementTrackBusiness(IRepositoryBase<JobAdvertisementTrackViewModel, JobAdvertisementTrack> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<JobAdvertisementTrackViewModel>> Create(JobAdvertisementTrackViewModel model)
        {
            //var data = _autoMapper.Map<JobAdvertisementViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<JobAdvertisementViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Create(model);

            return CommandResult<JobAdvertisementTrackViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<JobAdvertisementTrackViewModel>> Edit(JobAdvertisementTrackViewModel model)
        {
            //var data = _autoMapper.Map<JobAdvertisementViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<JobAdvertisementViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Edit(model);

            return CommandResult<JobAdvertisementTrackViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<JobAdvertisementTrackViewModel>> IsNameExists(JobAdvertisementTrackViewModel model)
        {
                        
            return CommandResult<JobAdvertisementTrackViewModel>.Instance();
        }
    }
}
