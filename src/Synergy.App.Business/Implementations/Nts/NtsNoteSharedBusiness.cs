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
    public class NtsNoteSharedBusiness : BusinessBase<NtsNoteSharedViewModel, NtsNoteShared>, INtsNoteSharedBusiness
    {
        private readonly IRepositoryQueryBase<NtsNoteSharedViewModel> _queryRepo;
        private readonly INtsQueryBusiness _ntsQueryBusiness;

        public NtsNoteSharedBusiness(IRepositoryBase<NtsNoteSharedViewModel, NtsNoteShared> repo,INtsQueryBusiness ntsQueryBusiness ,IMapper autoMapper, IRepositoryQueryBase<NtsNoteSharedViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _ntsQueryBusiness = ntsQueryBusiness;


        }

        public async override Task<CommandResult<NtsNoteSharedViewModel>> Create(NtsNoteSharedViewModel model, bool autoCommit = true)
        {
           
           // var data = _autoMapper.Map<UserRoleViewModel>(model);
           
            var result = await base.Create(model,autoCommit);           
            return CommandResult<NtsNoteSharedViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NtsNoteSharedViewModel>> Edit(NtsNoteSharedViewModel model, bool autoCommit = true)
        {  
            var result = await base.Edit(model,autoCommit);           
            return CommandResult<NtsNoteSharedViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        

        public async Task<List<NtsNoteSharedViewModel>> GetSearchResult(string NoteId)
        {

           
            var list = await _ntsQueryBusiness.GetSearchResult(NoteId);
            return list;
        }
    


    }
}
