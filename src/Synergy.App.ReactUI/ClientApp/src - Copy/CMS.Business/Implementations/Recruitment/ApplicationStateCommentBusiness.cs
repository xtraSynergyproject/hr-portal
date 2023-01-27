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
    public class ApplicationStateCommentBusiness : BusinessBase<ApplicationStateCommentViewModel, ApplicationStateComment>, IApplicationStateCommentBusiness
    {

        public ApplicationStateCommentBusiness(IRepositoryBase<ApplicationStateCommentViewModel, ApplicationStateComment> repo, IMapper autoMapper
           ) : base(repo, autoMapper)
        {
           
        }

        public async override Task<CommandResult<ApplicationStateCommentViewModel>> Create(ApplicationStateCommentViewModel model)
        {           
           
            var result = await base.Create(model);

            return CommandResult<ApplicationStateCommentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ApplicationStateCommentViewModel>> Edit(ApplicationStateCommentViewModel model)
        {
            
            var result = await base.Edit(model);

            return CommandResult<ApplicationStateCommentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        
    }
}
