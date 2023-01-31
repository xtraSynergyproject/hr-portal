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
    public class NtsNoteUserReactionBusiness : BusinessBase<NtsNoteUserReactionViewModel, NtsNoteUserReaction>, INtsNoteUserReactionBusiness
    {
        private readonly IRepositoryQueryBase<NtsNoteUserReactionViewModel> _queryRepo;
        private readonly INoteBusiness _noteBusiness;
        private readonly INtsQueryBusiness _ntsQueryBusiness;

        public NtsNoteUserReactionBusiness(IRepositoryBase<NtsNoteUserReactionViewModel, NtsNoteUserReaction> repo, IMapper autoMapper
            , IRepositoryQueryBase<NtsNoteUserReactionViewModel> queryRepo, INoteBusiness noteBusiness
             , INtsQueryBusiness ntsQueryBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _noteBusiness = noteBusiness;
            _ntsQueryBusiness = ntsQueryBusiness;
        }
        public async override Task<CommandResult<NtsNoteUserReactionViewModel>> Create(NtsNoteUserReactionViewModel model, bool autoCommit = true)
        {
            var result = await base.Create(model, autoCommit);
            return CommandResult<NtsNoteUserReactionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NtsNoteUserReactionViewModel>> Edit(NtsNoteUserReactionViewModel model, bool autoCommit = true)
        {
            var result = await base.Edit(model, autoCommit);
            return CommandResult<NtsNoteUserReactionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        //public async Task<List<NtsNoteUserReactionViewModel>> GetSearchResult(string NoteId)
        //{
        //    var list = await _ntsQueryBusiness.GetSearchResultData(NoteId);
        //    return list;
        //}
    }
}
