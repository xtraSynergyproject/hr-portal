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
    public class NtsTaskSharedBusiness : BusinessBase<NtsTaskSharedViewModel, NtsTaskShared>, INtsTaskSharedBusiness
    {
        private readonly IRepositoryQueryBase<NtsTaskSharedViewModel> _queryRepo;

        public NtsTaskSharedBusiness(IRepositoryBase<NtsTaskSharedViewModel, NtsTaskShared> repo, IMapper autoMapper, IRepositoryQueryBase<NtsTaskSharedViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;


        }

        public async override Task<CommandResult<NtsTaskSharedViewModel>> Create(NtsTaskSharedViewModel model)
        {
           
           // var data = _autoMapper.Map<UserRoleViewModel>(model);
           
            var result = await base.Create(model);           
            return CommandResult<NtsTaskSharedViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NtsTaskSharedViewModel>> Edit(NtsTaskSharedViewModel model)
        {  
            var result = await base.Edit(model);           
            return CommandResult<NtsTaskSharedViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        

        public async Task<List<NtsTaskSharedViewModel>> GetSearchResult(string TaskId)
        {

            string query = @$"select n.""Id"" as Id,u.""Name"" as Name,'User' as Type,u.""PhotoId"" as PhotoId
                              from public.""NtsTaskShared"" as n
                               join public.""User"" as u ON u.""Id"" = n.""SharedWithUserId"" and u.""IsDeleted""=false
                              
                              where n.""NtsTaskId""='{TaskId}' AND n.""IsDeleted""= false
union select n.""Id"" as Id,t.""Name"" as Name,'Team' as Type, t.""LogoId"" as PhotoId
                              from public.""NtsTaskShared"" as n                              
                               join public.""Team"" as t ON t.""Id"" = n.""SharedWithTeamId"" and t.""IsDeleted""=false
                              where n.""NtsTaskId""='{TaskId}' AND n.""IsDeleted""= false";
            var list = await _queryRepo.ExecuteQueryList<NtsTaskSharedViewModel>(query, null);
            return list;
        }
    


    }
}
