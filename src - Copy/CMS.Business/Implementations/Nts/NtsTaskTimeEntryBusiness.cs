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
    public class NtsTaskTimeEntryBusiness : BusinessBase<TaskTimeEntryViewModel, NtsTaskTimeEntry>,INtsTaskTimeEntryBusiness
    {
        private readonly IRepositoryQueryBase<TaskTimeEntryViewModel> _queryRepo;

        public NtsTaskTimeEntryBusiness(IRepositoryBase<TaskTimeEntryViewModel, NtsTaskTimeEntry> repo, IMapper autoMapper, IRepositoryQueryBase<TaskTimeEntryViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;


        }

        public async override Task<CommandResult<TaskTimeEntryViewModel>> Create(TaskTimeEntryViewModel model)
        {
           
           // var data = _autoMapper.Map<UserRoleViewModel>(model);
           
            var result = await base.Create(model);           
            return CommandResult<TaskTimeEntryViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<TaskTimeEntryViewModel>> Edit(TaskTimeEntryViewModel model)
        {  
            var result = await base.Edit(model);           
            return CommandResult<TaskTimeEntryViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        

        public async Task<List<TaskTimeEntryViewModel>> GetSearchResult(string taskId)
        {

            string query = @$"select n.*,u.""Name"" as UserName,u.""Email"" as UserEmail
                            from public.""NtsTaskTimeEntry"" as n                            
                            join public.""User"" as u ON u.""Id"" = n.""UserId"" and u.""IsDeleted""=false                          
                            
                             where n.""NtsTaskId""='{taskId}' AND n.""IsDeleted""= false";
                            ;
            var result = await _queryRepo.ExecuteQueryList<TaskTimeEntryViewModel>(query, null);            
            return result;
            
        }
        public async Task<List<TaskTimeEntryViewModel>> GetTimeEntriesData(string serviceId, DateTime timelog, string userId=null)
        {

            string query = @$"select n.*,u.""Name"" as UserName,u.""Email"" as UserEmail,t.""TaskNo"" as TaskNo,t.""TaskSubject"" as TaskName,n.""Comment"" as Comment,s.""Id"" as NtsServiceId,s.""ServiceSubject"" as ProjectName
                            from public.""NtsTaskTimeEntry"" as n                            
                            join public.""User"" as u ON u.""Id"" = n.""UserId"" and u.""IsDeleted""=false     
							join public.""NtsTask"" as t on t.""Id""=n.""NtsTaskId"" and t.""IsDeleted""=false  
							join public.""NtsService"" as s on s.""Id""=t.""ParentServiceId"" or s.""Id""=t.""ServicePlusId"" and s.""IsDeleted""=false  
                            where s.""Id""='{serviceId}' #SEARCHDATE# #SEARCHUSER# and n.""IsDeleted""= false";
            var searchDate = "";
            if (timelog!=ApplicationConstant.DateAndTime.MinDate)
            {
                searchDate = @$" AND n.""StartDate""::DATE <= '{timelog}'::DATE and '{timelog}'::DATE <=n.""EndDate""::DATE ";
            }
            var searchUser = "";
            if (userId.IsNotNullAndNotEmpty())
            {
                searchUser = $@" AND u.""Id""='{userId}' ";
            }
            query = query.Replace("#SEARCHDATE#",searchDate);
            query = query.Replace("#SEARCHUSER#", searchUser);
            var result = await _queryRepo.ExecuteQueryList<TaskTimeEntryViewModel>(query, null);
            return result;

        }



    }
}
