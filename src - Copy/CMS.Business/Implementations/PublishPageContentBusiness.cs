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
    public class PublishPageContentBusiness : BusinessBase<PublishPageContentViewModel, PageContentPublished>, IPublishPageContentBusiness
    {
       
        public PublishPageContentBusiness(IRepositoryBase<PublishPageContentViewModel, PageContentPublished> repo, IMapper autoMapper) : base(repo, autoMapper)
        {
           
         }
        public async override Task<CommandResult<PublishPageContentViewModel>> Create(PublishPageContentViewModel model)
        {
            var data = _autoMapper.Map<PublishPageContentViewModel>(model);            
            var result = await base.Create(data);
            return CommandResult<PublishPageContentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<PublishPageContentViewModel>> Edit(PublishPageContentViewModel model)
        {
            var data = _autoMapper.Map<PublishPageContentViewModel>(model);           
            var result = await base.Edit(data);
            return CommandResult<PublishPageContentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        } 
    }
}
