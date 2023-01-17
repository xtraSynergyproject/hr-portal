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
    public class NtsNoteSharedBusiness : BusinessBase<NtsNoteSharedViewModel, NtsNoteShared>, INtsNoteSharedBusiness
    {
        private readonly IRepositoryQueryBase<NtsNoteSharedViewModel> _queryRepo;

        public NtsNoteSharedBusiness(IRepositoryBase<NtsNoteSharedViewModel, NtsNoteShared> repo, IMapper autoMapper, IRepositoryQueryBase<NtsNoteSharedViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;


        }

        public async override Task<CommandResult<NtsNoteSharedViewModel>> Create(NtsNoteSharedViewModel model)
        {
           
           // var data = _autoMapper.Map<UserRoleViewModel>(model);
           
            var result = await base.Create(model);           
            return CommandResult<NtsNoteSharedViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NtsNoteSharedViewModel>> Edit(NtsNoteSharedViewModel model)
        {  
            var result = await base.Edit(model);           
            return CommandResult<NtsNoteSharedViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        

        public async Task<List<NtsNoteSharedViewModel>> GetSearchResult(string NoteId)
        {

            string query = @$"select n.""Id"" as Id,u.""Name"" as Name,'User' as Type,u.""PhotoId"" as PhotoId
                              from public.""NtsNoteShared"" as n
                               join public.""User"" as u ON u.""Id"" = n.""SharedWithUserId"" and u.""IsDeleted""=false
                              
                              where n.""NtsNoteId""='{NoteId}' AND n.""IsDeleted""= false
union select n.""Id"" as Id,t.""Name"" as Name,'Team' as Type, t.""LogoId"" as PhotoId
                              from public.""NtsNoteShared"" as n                              
                               join public.""Team"" as t ON t.""Id"" = n.""SharedWithTeamId"" and t.""IsDeleted""=false
                              where n.""NtsNoteId""='{NoteId}' AND n.""IsDeleted""= false";
            var list = await _queryRepo.ExecuteQueryList<NtsNoteSharedViewModel>(query, null);
            return list;
        }
    


    }
}
