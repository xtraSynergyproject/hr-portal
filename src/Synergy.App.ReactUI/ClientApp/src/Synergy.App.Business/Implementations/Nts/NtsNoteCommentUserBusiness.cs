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
    public class NtsNoteCommentUserBusiness : BusinessBase<NtsNoteCommentUserViewModel, NtsNoteCommentUser>,INtsNoteCommentUserBusiness
    {
        private readonly IRepositoryQueryBase<NtsNoteCommentUserViewModel> _queryRepo;
        private readonly INoteBusiness _noteBusiness;

        public NtsNoteCommentUserBusiness(IRepositoryBase<NtsNoteCommentUserViewModel, NtsNoteCommentUser> repo, IMapper autoMapper, IRepositoryQueryBase<NtsNoteCommentUserViewModel> queryRepo, INoteBusiness noteBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _noteBusiness = noteBusiness;

        }

        public async override Task<CommandResult<NtsNoteCommentUserViewModel>> Create(NtsNoteCommentUserViewModel model, bool autoCommit = true)
        {          
            
            var result = await base.Create(model,autoCommit);           
            return CommandResult<NtsNoteCommentUserViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NtsNoteCommentUserViewModel>> Edit(NtsNoteCommentUserViewModel model, bool autoCommit = true)
        {  
            var result = await base.Edit(model,autoCommit);           
            return CommandResult<NtsNoteCommentUserViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }       

       

    }
}
