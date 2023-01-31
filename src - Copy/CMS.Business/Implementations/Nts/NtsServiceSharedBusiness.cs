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
    public class NtsServiceSharedBusiness : BusinessBase<NtsServiceSharedViewModel, NtsServiceShared>, INtsServiceSharedBusiness
    {
        private readonly IRepositoryQueryBase<NtsServiceSharedViewModel> _queryRepo;

        public NtsServiceSharedBusiness(IRepositoryBase<NtsServiceSharedViewModel, NtsServiceShared> repo, IMapper autoMapper, IRepositoryQueryBase<NtsServiceSharedViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;


        }

        public async override Task<CommandResult<NtsServiceSharedViewModel>> Create(NtsServiceSharedViewModel model)
        {

            // var data = _autoMapper.Map<UserRoleViewModel>(model);

            var result = await base.Create(model);
            return CommandResult<NtsServiceSharedViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NtsServiceSharedViewModel>> Edit(NtsServiceSharedViewModel model)
        {
            var result = await base.Edit(model);
            return CommandResult<NtsServiceSharedViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


        public async Task<List<NtsServiceSharedViewModel>> GetSearchResult(string ServiceId)
        {

            string query = @$"select n.""Id"" as Id,u.""Name"" as Name,'User' as Type,u.""PhotoId"" as PhotoId
                              from public.""NtsServiceShared"" as n
                               join public.""User"" as u ON u.""Id"" = n.""SharedWithUserId"" and u.""IsDeleted""=false
                              
                              where n.""NtsServiceId""='{ServiceId}' AND n.""IsDeleted""= false
union select n.""Id"" as Id,t.""Name"" as Name,'Team' as Type, t.""LogoId"" as PhotoId
                              from public.""NtsServiceShared"" as n                              
                               join public.""Team"" as t ON t.""Id"" = n.""SharedWithTeamId"" and t.""IsDeleted""=false
                              where n.""NtsServiceId""='{ServiceId}' AND n.""IsDeleted""= false";
            var list = await _queryRepo.ExecuteQueryList<NtsServiceSharedViewModel>(query, null);
            return list;
        }



    }
}
