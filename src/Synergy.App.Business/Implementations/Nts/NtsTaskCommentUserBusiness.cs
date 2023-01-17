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
    public class NtsTaskCommentUserBusiness : BusinessBase<NtsTaskCommentUserViewModel, NtsTaskCommentUser>,INtsTaskCommentUserBusiness
    {
        private readonly IRepositoryQueryBase<NtsTaskCommentUserViewModel> _queryRepo;
        private readonly ITaskBusiness _taskBusiness;

        public NtsTaskCommentUserBusiness(IRepositoryBase<NtsTaskCommentUserViewModel, NtsTaskCommentUser> repo, IMapper autoMapper, IRepositoryQueryBase<NtsTaskCommentUserViewModel> queryRepo, ITaskBusiness taskBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _taskBusiness = taskBusiness;

        }

        public async override Task<CommandResult<NtsTaskCommentUserViewModel>> Create(NtsTaskCommentUserViewModel model, bool autoCommit = true)
        {          
            
            var result = await base.Create(model,autoCommit);           
            return CommandResult<NtsTaskCommentUserViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NtsTaskCommentUserViewModel>> Edit(NtsTaskCommentUserViewModel model, bool autoCommit = true)
        {  
            var result = await base.Edit(model,autoCommit);           
            return CommandResult<NtsTaskCommentUserViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }       

       

    }
}
