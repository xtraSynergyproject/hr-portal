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
    public class PublishPageContentBusiness : BusinessBase<PublishPageContentViewModel, PageContentPublished>, IPublishPageContentBusiness
    {
       
        public PublishPageContentBusiness(IRepositoryBase<PublishPageContentViewModel, PageContentPublished> repo, IMapper autoMapper) : base(repo, autoMapper)
        {
           
         }
        public async override Task<CommandResult<PublishPageContentViewModel>> Create(PublishPageContentViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<PublishPageContentViewModel>(model);            
            var result = await base.Create(data, autoCommit);
            return CommandResult<PublishPageContentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<PublishPageContentViewModel>> Edit(PublishPageContentViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<PublishPageContentViewModel>(model);           
            var result = await base.Edit(data,autoCommit);
            return CommandResult<PublishPageContentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        } 
    }
}
