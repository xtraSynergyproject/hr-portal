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
    public class NtsTaskTimeEntryBusiness : BusinessBase<TaskTimeEntryViewModel, NtsTaskTimeEntry>,INtsTaskTimeEntryBusiness
    {
        private readonly IRepositoryQueryBase<TaskTimeEntryViewModel> _queryRepo;
        private readonly INtsQueryBusiness _ntsQueryBusiness;

        public NtsTaskTimeEntryBusiness(IRepositoryBase<TaskTimeEntryViewModel, NtsTaskTimeEntry> repo, IMapper autoMapper, IRepositoryQueryBase<TaskTimeEntryViewModel> queryRepo
            , INtsQueryBusiness ntsQueryBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _ntsQueryBusiness = ntsQueryBusiness;


        }

        public async override Task<CommandResult<TaskTimeEntryViewModel>> Create(TaskTimeEntryViewModel model, bool autoCommit = true)
        {
           
           // var data = _autoMapper.Map<UserRoleViewModel>(model);
           
            var result = await base.Create(model,autoCommit);           
            return CommandResult<TaskTimeEntryViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<TaskTimeEntryViewModel>> Edit(TaskTimeEntryViewModel model, bool autoCommit = true)
        {  
            var result = await base.Edit(model,autoCommit);           
            return CommandResult<TaskTimeEntryViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        

        public async Task<List<TaskTimeEntryViewModel>> GetSearchResult(string taskId)
        {
            var result = await _ntsQueryBusiness.GetSearchTaskTimeEntryResult(taskId);
            return result;
            
        }
        public async Task<List<TaskTimeEntryViewModel>> GetTimeEntriesData(string serviceId, DateTime timelog, string userId=null)
        {
            var result = await _ntsQueryBusiness.GetTimeEntriesData(serviceId, timelog, userId);
            return result;

        }



    }
}
