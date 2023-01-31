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
    public class NtsTaskPrecedenceBusiness : BusinessBase<NtsTaskPrecedenceViewModel, NtsTaskPrecedence>, INtsTaskPrecedenceBusiness
    {
        private readonly IRepositoryQueryBase<NtsTaskPrecedenceViewModel> _queryRepo;
        private readonly INtsQueryBusiness _ntsQueryBusiness;

        public NtsTaskPrecedenceBusiness(IRepositoryBase<NtsTaskPrecedenceViewModel, NtsTaskPrecedence> repo, IMapper autoMapper, IRepositoryQueryBase<NtsTaskPrecedenceViewModel> queryRepo
            , INtsQueryBusiness ntsQueryBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _ntsQueryBusiness = ntsQueryBusiness;


        }

        public async override Task<CommandResult<NtsTaskPrecedenceViewModel>> Create(NtsTaskPrecedenceViewModel model, bool autoCommit = true)
        {

            // var data = _autoMapper.Map<UserRoleViewModel>(model);
            
            var result = await base.Create(model,autoCommit);           
            return CommandResult<NtsTaskPrecedenceViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NtsTaskPrecedenceViewModel>> Edit(NtsTaskPrecedenceViewModel model, bool autoCommit = true)
        {  
            var result = await base.Edit(model,autoCommit);           
            return CommandResult<NtsTaskPrecedenceViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        

        public async Task<List<NtsTaskPrecedenceViewModel>> GetSearchResult(string taskId)
        {
            var result = await _ntsQueryBusiness.GetSearchTaskPrecedenceResult(taskId);
            return result;
            
        }
        public async Task<List<NtsTaskPrecedenceViewModel>> GetTaskPredecessor(string taskId)
        {
            var result = await _ntsQueryBusiness.GetTaskPredecessorData(taskId);
            return result;

        }
        public async Task<List<NtsTaskPrecedenceViewModel>> GetTaskSuccessor(string taskId)
        {

            var result = await _ntsQueryBusiness.GetTaskSuccessorData(taskId);
            return result;

        }
    }
}
