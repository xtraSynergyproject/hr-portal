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
    public class NtsTaskPrecedenceBusiness : BusinessBase<NtsTaskPrecedenceViewModel, NtsTaskPrecedence>, INtsTaskPrecedenceBusiness
    {
        private readonly IRepositoryQueryBase<NtsTaskPrecedenceViewModel> _queryRepo;

        public NtsTaskPrecedenceBusiness(IRepositoryBase<NtsTaskPrecedenceViewModel, NtsTaskPrecedence> repo, IMapper autoMapper, IRepositoryQueryBase<NtsTaskPrecedenceViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;


        }

        public async override Task<CommandResult<NtsTaskPrecedenceViewModel>> Create(NtsTaskPrecedenceViewModel model)
        {

            // var data = _autoMapper.Map<UserRoleViewModel>(model);
            
            var result = await base.Create(model);           
            return CommandResult<NtsTaskPrecedenceViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NtsTaskPrecedenceViewModel>> Edit(NtsTaskPrecedenceViewModel model)
        {  
            var result = await base.Edit(model);           
            return CommandResult<NtsTaskPrecedenceViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        

        public async Task<List<NtsTaskPrecedenceViewModel>> GetSearchResult(string taskId)
        {

            string query = @$"select n.*
                            from public.""NtsTaskPrecedence"" as n  
                             where n.""NtsTaskId""='{taskId}' AND n.""IsDeleted""= false";
                            ;
            var result = await _queryRepo.ExecuteQueryList<NtsTaskPrecedenceViewModel>(query, null);            
            return result;
            
        }
        public async Task<List<NtsTaskPrecedenceViewModel>> GetTaskPredecessor(string taskId)
        {

            string query = @$"select  t.""TaskSubject"" as PredecessorsName,n.""Id"" as Id,n.""PrecedenceRelationshipType""
as PrecedenceRelationshipType, n.""PredecessorType"" as PredecessorType,t.""TaskNo"" as TaskNo
                            from public.""NtsTaskPrecedence"" as n  
join public.""NtsTask"" as t on t.""Id""=n.""PredecessorId"" and t.""IsDeleted""=false
                             where n.""NtsTaskId""='{taskId}' AND n.""IsDeleted""= false and n.""PredecessorType""='1'
Union
select t.""ServiceSubject"" as PredecessorsName,n.""Id"" as Id,n.""PrecedenceRelationshipType""
as PrecedenceRelationshipType, n.""PredecessorType"" as PredecessorType,t.""ServiceNo"" as TaskNo
                            from public.""NtsTaskPrecedence"" as n
join public.""NtsService"" as t on t.""Id""=n.""PredecessorId"" and t.""IsDeleted""=false
                             where n.""NtsTaskId""='{taskId}' AND n.""IsDeleted""= false and n.""PredecessorType""='2'

Union
select t.""NoteSubject"" as PredecessorsName,n.""Id"" as Id,n.""PrecedenceRelationshipType""
as PrecedenceRelationshipType, n.""PredecessorType"" as PredecessorType,t.""NoteNo"" as TaskNo
                            from public.""NtsTaskPrecedence"" as n
join public.""NtsNote"" as t on t.""Id""=n.""PredecessorId"" and t.""IsDeleted""=false
                             where n.""NtsTaskId""='{taskId}' AND n.""IsDeleted""= false and n.""PredecessorType""='0'

";
            ;
            var result = await _queryRepo.ExecuteQueryList<NtsTaskPrecedenceViewModel>(query, null);
            return result;

        }
        public async Task<List<NtsTaskPrecedenceViewModel>> GetTaskSuccessor(string taskId)
        {

            string query = @$"select  t.""TaskSubject"" as PredecessorsName,n.""Id"" as Id,n.""PrecedenceRelationshipType""
as PrecedenceRelationshipType, n.""PredecessorType"" as PredecessorType,t.""TaskNo"" as TaskNo
                            from  public.""NtsTaskPrecedence"" as n 
join  public.""NtsTask"" as t   on t.""Id""=n.""NtsTaskId"" and t.""IsDeleted""=false
                             where n.""PredecessorId""='{taskId}' AND n.""IsDeleted""= false" ;
            
            var result = await _queryRepo.ExecuteQueryList<NtsTaskPrecedenceViewModel>(query, null);
            return result;

        }
    }
}
