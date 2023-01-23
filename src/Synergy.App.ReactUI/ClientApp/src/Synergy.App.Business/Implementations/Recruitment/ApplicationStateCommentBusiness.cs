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
    public class ApplicationStateCommentBusiness : BusinessBase<ApplicationStateCommentViewModel, ApplicationStateComment>, IApplicationStateCommentBusiness
    {

        public ApplicationStateCommentBusiness(IRepositoryBase<ApplicationStateCommentViewModel, ApplicationStateComment> repo, IMapper autoMapper
           ) : base(repo, autoMapper)
        {
           
        }

        public async override Task<CommandResult<ApplicationStateCommentViewModel>> Create(ApplicationStateCommentViewModel model, bool autoCommit = true)
        {           
           
            var result = await base.Create(model,autoCommit);

            return CommandResult<ApplicationStateCommentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ApplicationStateCommentViewModel>> Edit(ApplicationStateCommentViewModel model, bool autoCommit = true)
        {
            
            var result = await base.Edit(model,autoCommit);

            return CommandResult<ApplicationStateCommentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        
    }
}
