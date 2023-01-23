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
    public class NtsNoteCommentUserBusiness : BusinessBase<NtsNoteCommentUserViewModel, NtsNoteCommentUser>,INtsNoteCommentUserBusiness
    {
        private readonly IRepositoryQueryBase<NtsNoteCommentUserViewModel> _queryRepo;
        private readonly INoteBusiness _noteBusiness;

        public NtsNoteCommentUserBusiness(IRepositoryBase<NtsNoteCommentUserViewModel, NtsNoteCommentUser> repo, IMapper autoMapper, IRepositoryQueryBase<NtsNoteCommentUserViewModel> queryRepo, INoteBusiness noteBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _noteBusiness = noteBusiness;

        }

        public async override Task<CommandResult<NtsNoteCommentUserViewModel>> Create(NtsNoteCommentUserViewModel model)
        {          
            
            var result = await base.Create(model);           
            return CommandResult<NtsNoteCommentUserViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NtsNoteCommentUserViewModel>> Edit(NtsNoteCommentUserViewModel model)
        {  
            var result = await base.Edit(model);           
            return CommandResult<NtsNoteCommentUserViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }       

       

    }
}
